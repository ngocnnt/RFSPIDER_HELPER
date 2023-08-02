using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_KTRA_ROUTER.Interface;
using Foundation;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Receiving;
using UIKit;

namespace APP_KTRA_ROUTER.iOS.Service
{
    public class MqttTaskService
    {
        private IMqttClient _mqttClient;
        private string _sessionPayedTopic;

        public MqttTaskService()
        {

        }
        public void Subscribe(string topic, string clientID)
        {
            _sessionPayedTopic = topic;

            _mqttClient = new MqttClientRepository().Create(
             "10.72.96.25",
             1883,
             "lucnv",
             "lucnv",
             new List<string> { _sessionPayedTopic }, clientID);

            _mqttClient.ApplicationMessageReceivedHandler = new SubscribeCallback(_sessionPayedTopic);
        }
        public void UnSubscribe()
        {
            _mqttClient.ApplicationMessageReceivedHandler = null;
        }


    }

    public class SubscribeCallback : IMqttApplicationMessageReceivedHandler
    {
        private readonly string _sessionPayedTopic;

        public SubscribeCallback(string sessionPayedTopic)
        {
            _sessionPayedTopic = sessionPayedTopic;
        }

        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

            if (e.ApplicationMessage.Topic == _sessionPayedTopic)
            {
                //new NotificationHelper().CreateNotification("Thông Báo", message);
            }

            return Task.CompletedTask;
        }
    }
}