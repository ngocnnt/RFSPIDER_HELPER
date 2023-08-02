using APP_KTRA_ROUTER.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP_KTRA_ROUTER.Database
{
	public class Information : BaseViewModel	
	{

		public string MA_NVGCS { get; set; }

		public string MA_KHANG { get; set; }

		public string MA_DDO { get; set; }

		public string MA_DVIQLY { get; set; }

		public string MA_GC { get; set; }

		public string MA_QUYEN { get; set; }

		public string MA_TRAM { get; set; }

		public string BOCSO_ID { get; set; }

		public string LOAI_BCS { get; set; }

		public string LOAI_CS { get; set; }

		public string TEN_KHANG { get; set; }

		public string DIA_CHI { get; set; }

		public string MA_NN { get; set; }

		public double? SO_HO { get; set; }

		public string MA_CTO { get; set; }

		public string SERY_CTO { get; set; }

		public double? HSN { get; set; }

		public double? CS_CU { get; set; }

		public double? TTR_CU { get; set; }

		public double? SL_CU { get; set; }

		public double? SL_TTIEP { get; set; }

		public DateTime? NGAY_CU { get; set; }
		Double? cs_moi;
		public double? CS_MOI
		{
			get => cs_moi;
			set
			{
				if (value != null)
				{
					cs_moi = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(STATUS));
				}

			}
		}

		public string TTR_MOI { get; set; }

		Double? sl_moi;
		public double? SL_MOI
		{
			get => sl_moi;
			set
			{
				if (value != null)
				{
					sl_moi = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(STATUS));
				}

			}
		}

		public string CHUOI_GIA { get; set; }

		public int? KY { get; set; }

		public int? THANG { get; set; }

		public int? NAM { get; set; }

		public DateTime? NGAY_MOI { get; set; }

		public string NGUOI_GCS { get; set; }

		public double? SL_THAO { get; set; }

		public int? KIMUA_CSPK { get; set; }

		public string MA_COT { get; set; }

		public double? SLUONG_1 { get; set; }

		public double? SLUONG_2 { get; set; }

		public double? SLUONG_3 { get; set; }

		public string SO_HOM { get; set; }

		public double? PMAX { get; set; }

		public DateTime? NGAY_PMAX { get; set; }
		public DateTime? NGAY_GIO { get; set; }
		public int? DOCRF { get; set; }
		public int? SLBT { get; set; }
		public double? SL_TONG { get; set; }

		public string METER_TYPE { get; set; }



		public string TENFILE { get; set; }

		public string GHICHU { get; set; }
		int? status;
		public int? STATUS
		{
			get => status;
			set
			{
				if (value != null)
				{
					status = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(CS_MOI));
					OnPropertyChanged(nameof(SL_MOI));
				}

			}
		}


	}
}
