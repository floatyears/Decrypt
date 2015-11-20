using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "MiscInfo")]
	[Serializable]
	public class MiscInfo : IExtensible
	{
		private int _ID;

		private int _SceneResetCost;

		private int _Common2ShopRefreshCost;

		private readonly List<int> _Day7RewardType = new List<int>();

		private readonly List<int> _Day7RewardValue1 = new List<int>();

		private readonly List<int> _Day7RewardValue2 = new List<int>();

		private int _Level;

		private readonly List<int> _RewardType = new List<int>();

		private readonly List<int> _RewardValue1 = new List<int>();

		private readonly List<int> _RewardValue2 = new List<int>();

		private int _FundLevel;

		private int _FundDiamond;

		private int _BuyFundCount;

		private int _WelfareRewardType;

		private int _WelfareRewardValue1;

		private int _WelfareRewardValue2;

		private int _BuyOrePillageCountCost;

		private int _BuyOreRevengeCountCost;

		private int _GuildWarReviveCost;

		private int _AppleRate;

		private int _KRCost;

		private int _SeventhDayRewardMinLevel;

		private int _SeventhDayRewardMaxLevel;

		private int _SeventhDayRewardID;

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

		[ProtoMember(2, IsRequired = false, Name = "SceneResetCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneResetCost
		{
			get
			{
				return this._SceneResetCost;
			}
			set
			{
				this._SceneResetCost = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Common2ShopRefreshCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Common2ShopRefreshCost
		{
			get
			{
				return this._Common2ShopRefreshCost;
			}
			set
			{
				this._Common2ShopRefreshCost = value;
			}
		}

		[ProtoMember(4, Name = "Day7RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> Day7RewardType
		{
			get
			{
				return this._Day7RewardType;
			}
		}

		[ProtoMember(5, Name = "Day7RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> Day7RewardValue1
		{
			get
			{
				return this._Day7RewardValue1;
			}
		}

		[ProtoMember(6, Name = "Day7RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> Day7RewardValue2
		{
			get
			{
				return this._Day7RewardValue2;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Level
		{
			get
			{
				return this._Level;
			}
			set
			{
				this._Level = value;
			}
		}

		[ProtoMember(8, Name = "RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardType
		{
			get
			{
				return this._RewardType;
			}
		}

		[ProtoMember(9, Name = "RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue1
		{
			get
			{
				return this._RewardValue1;
			}
		}

		[ProtoMember(10, Name = "RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue2
		{
			get
			{
				return this._RewardValue2;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "FundLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FundLevel
		{
			get
			{
				return this._FundLevel;
			}
			set
			{
				this._FundLevel = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "FundDiamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FundDiamond
		{
			get
			{
				return this._FundDiamond;
			}
			set
			{
				this._FundDiamond = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "BuyFundCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BuyFundCount
		{
			get
			{
				return this._BuyFundCount;
			}
			set
			{
				this._BuyFundCount = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "WelfareRewardType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int WelfareRewardType
		{
			get
			{
				return this._WelfareRewardType;
			}
			set
			{
				this._WelfareRewardType = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "WelfareRewardValue1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int WelfareRewardValue1
		{
			get
			{
				return this._WelfareRewardValue1;
			}
			set
			{
				this._WelfareRewardValue1 = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "WelfareRewardValue2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int WelfareRewardValue2
		{
			get
			{
				return this._WelfareRewardValue2;
			}
			set
			{
				this._WelfareRewardValue2 = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "BuyOrePillageCountCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BuyOrePillageCountCost
		{
			get
			{
				return this._BuyOrePillageCountCost;
			}
			set
			{
				this._BuyOrePillageCountCost = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "BuyOreRevengeCountCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BuyOreRevengeCountCost
		{
			get
			{
				return this._BuyOreRevengeCountCost;
			}
			set
			{
				this._BuyOreRevengeCountCost = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "GuildWarReviveCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GuildWarReviveCost
		{
			get
			{
				return this._GuildWarReviveCost;
			}
			set
			{
				this._GuildWarReviveCost = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "AppleRate", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AppleRate
		{
			get
			{
				return this._AppleRate;
			}
			set
			{
				this._AppleRate = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "KRCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int KRCost
		{
			get
			{
				return this._KRCost;
			}
			set
			{
				this._KRCost = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "SeventhDayRewardMinLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SeventhDayRewardMinLevel
		{
			get
			{
				return this._SeventhDayRewardMinLevel;
			}
			set
			{
				this._SeventhDayRewardMinLevel = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "SeventhDayRewardMaxLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SeventhDayRewardMaxLevel
		{
			get
			{
				return this._SeventhDayRewardMaxLevel;
			}
			set
			{
				this._SeventhDayRewardMaxLevel = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "SeventhDayRewardID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SeventhDayRewardID
		{
			get
			{
				return this._SeventhDayRewardID;
			}
			set
			{
				this._SeventhDayRewardID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
