using APP_KTRA_ROUTER.Global;
using APP_KTRA_ROUTER.Models;
using APP_KTRA_ROUTER.Popup;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace APP_KTRA_ROUTER.Interface
{
    public class MqttClientRepository
    {
        Dictionary<string, TopicFilter> _topicFilter;

        public   static IMqttClient client;

        public IMqttClient Create(string server, int? port, string userName, string password, List<string> topics, string clientID)
        {
            _topicFilter = new Dictionary<string, TopicFilter>();

            foreach (var topic in topics)
            {
                TopicFilter topicFilter = new TopicFilter
                {
                    Topic = topic,
                    QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce

                };

                _topicFilter.Add(topic, topicFilter);
                
            }

            Task.Run(() => MqttClientRunAsync(server, port, userName, password, clientID)).Wait();
            //tuấn thử thêm vào
            foreach (var topic in topics)
            {
                client.ApplicationMessageReceivedHandler = new SubscribeCallback(_topicFilter);

            }
            
            return client;
        }

        private async Task MqttClientRunAsync(string server, int? port, string userName, string password, string clientID)
        {
            try
            {
                var options = new MqttClientOptions
                {
                    ClientId = clientID,
                    CleanSession = true,
                    ChannelOptions = new MqttClientTcpOptions
                    {
                        Server = server,
                        Port = port
                    },
                    Credentials = new MqttClientCredentials
                    {
                        Username = userName,
                        Password = Encoding.UTF8.GetBytes(password)
                    }
                };

                var factory = new MqttFactory();

                client = factory.CreateMqttClient();

                client.ConnectedHandler = new MqttConnectedHandler(_topicFilter, client);
                client.DisconnectedHandler = new MqttDisconnectedHandler(options, client);

                try
                {
                    await client.ConnectAsync(options);
                }
                catch //(Exception ex)
                {

                }
                
            }
            catch //(Exception ex)
            {

            }
        }

        public async static void PublibMessage(string topic, string content)
        {
            try
            {
                var messge = new MqttApplicationMessage();
                messge.Topic = topic;
                messge.Payload = Encoding.UTF8.GetBytes(content);
                await client.PublishAsync(messge);
            }
            catch (Exception ex)
            {
                if (Device.RuntimePlatform != Device.iOS )
                await new MessageBox("Thông Báo", ex.Message).Show();
            }
           
        }
    }

    public class MqttDisconnectedHandler : IMqttClientDisconnectedHandler
    {
        private IMqttClient _client;
        private MqttClientOptions _options;

        public MqttDisconnectedHandler(MqttClientOptions options, IMqttClient client)
        {
            _options = options;
            _client = client;
        }

        public async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            try
            {
                if (Device.RuntimePlatform != Device.iOS )
                    //await new MessageBox("Thông Báo", "Mất kết nối đến server").Show();
                await _client.ReconnectAsync();
            }
            catch 
            {
               
            }
            
        }
    }

    public class MqttConnectedHandler : IMqttClientConnectedHandler
    {
        private IMqttClient _client;
        private Dictionary<string, TopicFilter> _topicFilter;

        public MqttConnectedHandler(Dictionary<string, TopicFilter> topicFilter, IMqttClient client)
        {
            _topicFilter = topicFilter;
            _client = client;
        }

        public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            await _client.SubscribeAsync(_topicFilter.Values.ToArray());
        }
    }

    public class SubscribeCallback : IMqttApplicationMessageReceivedHandler
    {
        private readonly Dictionary<string, TopicFilter> _sessionPayedTopic;

        public SubscribeCallback(Dictionary<string, TopicFilter> sessionPayedTopic)
        {
            _sessionPayedTopic = sessionPayedTopic;
        }

        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            foreach (var topic in _sessionPayedTopic)
            {
                if (e.ApplicationMessage.Topic == topic.Value.Topic )
                {
                    try
                    {
                        DcuMqttResp _lstDCU = JsonConvert.DeserializeObject<DcuMqttResp>(message);                       
                       Config.messagingCenter.Send(this, "MQTT", _lstDCU);
                    }
                    catch (Exception ex )
                    {                       
                    }
                  
                }
            }    
                

            return  Task.CompletedTask;
        }


    }
}
