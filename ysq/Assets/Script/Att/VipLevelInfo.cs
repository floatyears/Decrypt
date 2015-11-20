using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "VipLevelInfo")]
	[Serializable]
	public class VipLevelInfo : IExtensible
	{
		private int _ID;

		private readonly List<int> _EnhanceRate = new List<int>();

		private int _TotalPay;

		private int _SceneResetCount;

		private readonly List<int> _BuyCount = new List<int>();

		private int _ShopCommon2Count;

		private int _ShopAwakenCount;

		private int _D2MCount;

		private int _Price;

		private int _OffPrice;

		private readonly List<int> _RewardType = new List<int>();

		private readonly List<int> _RewardValue1 = new List<int>();

		private readonly List<int> _RewardValue2 = new List<int>();

		private int _CostumePartyRewardCount;

		private int _ScratchOff;

		private int _MaxEnergy;

		private int _MaxStamina;

		private readonly List<int> _WeekRewardType = new List<int>();

		private readonly List<int> _WeekRewardValue1 = new List<int>();

		private readonly List<int> _WeekRewardValue2 = new List<int>();

		private int _WeekPrice;

		private int _WeekOffPrice;

		private int _BuyPillageCount;

		private int _BuyRevengeCount;

		private int _ShopLopetCount;

		private int _MagicLoveBuyCount;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "ID", DataFormat = DataFormat.TwosComplement)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		[ProtoMember(2, Name = "EnhanceRate", DataFormat = DataFormat.TwosComplement)]
		public List<int> EnhanceRate
		{
			get
			{
				return this._EnhanceRate;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "TotalPay", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TotalPay
		{
			get
			{
				return this._TotalPay;
			}
			set
			{
				this._TotalPay = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "SceneResetCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneResetCount
		{
			get
			{
				return this._SceneResetCount;
			}
			set
			{
				this._SceneResetCount = value;
			}
		}

		[ProtoMember(5, Name = "BuyCount", DataFormat = DataFormat.TwosComplement)]
		public List<int> BuyCount
		{
			get
			{
				return this._BuyCount;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "ShopCommon2Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ShopCommon2Count
		{
			get
			{
				return this._ShopCommon2Count;
			}
			set
			{
				this._ShopCommon2Count = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "ShopAwakenCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ShopAwakenCount
		{
			get
			{
				return this._ShopAwakenCount;
			}
			set
			{
				this._ShopAwakenCount = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "D2MCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int D2MCount
		{
			get
			{
				return this._D2MCount;
			}
			set
			{
				this._D2MCount = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Price", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Price
		{
			get
			{
				return this._Price;
			}
			set
			{
				this._Price = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "OffPrice", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int OffPrice
		{
			get
			{
				return this._OffPrice;
			}
			set
			{
				this._OffPrice = value;
			}
		}

		[ProtoMember(11, Name = "RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardType
		{
			get
			{
				return this._RewardType;
			}
		}

		[ProtoMember(12, Name = "RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue1
		{
			get
			{
				return this._RewardValue1;
			}
		}

		[ProtoMember(13, Name = "RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue2
		{
			get
			{
				return this._RewardValue2;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "CostumePartyRewardCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CostumePartyRewardCount
		{
			get
			{
				return this._CostumePartyRewardCount;
			}
			set
			{
				this._CostumePartyRewardCount = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "ScratchOff", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ScratchOff
		{
			get
			{
				return this._ScratchOff;
			}
			set
			{
				this._ScratchOff = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "MaxEnergy", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxEnergy
		{
			get
			{
				return this._MaxEnergy;
			}
			set
			{
				this._MaxEnergy = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "MaxStamina", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxStamina
		{
			get
			{
				return this._MaxStamina;
			}
			set
			{
				this._MaxStamina = value;
			}
		}

		[ProtoMember(18, Name = "WeekRewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> WeekRewardType
		{
			get
			{
				return this._WeekRewardType;
			}
		}

		[ProtoMember(19, Name = "WeekRewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> WeekRewardValue1
		{
			get
			{
				return this._WeekRewardValue1;
			}
		}

		[ProtoMember(20, Name = "WeekRewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> WeekRewardValue2
		{
			get
			{
				return this._WeekRewardValue2;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "WeekPrice", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int WeekPrice
		{
			get
			{
				return this._WeekPrice;
			}
			set
			{
				this._WeekPrice = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "WeekOffPrice", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int WeekOffPrice
		{
			get
			{
				return this._WeekOffPrice;
			}
			set
			{
				this._WeekOffPrice = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "BuyPillageCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BuyPillageCount
		{
			get
			{
				return this._BuyPillageCount;
			}
			set
			{
				this._BuyPillageCount = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "BuyRevengeCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BuyRevengeCount
		{
			get
			{
				return this._BuyRevengeCount;
			}
			set
			{
				this._BuyRevengeCount = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "ShopLopetCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ShopLopetCount
		{
			get
			{
				return this._ShopLopetCount;
			}
			set
			{
				this._ShopLopetCount = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "MagicLoveBuyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicLoveBuyCount
		{
			get
			{
				return this._MagicLoveBuyCount;
			}
			set
			{
				this._MagicLoveBuyCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
