using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "OreInfo")]
	[Serializable]
	public class OreInfo : IExtensible
	{
		private int _ID;

		private int _RankMin;

		private int _RankMax;

		private int _Amount1;

		private int _Amount2;

		private int _DayRankMin;

		private int _DayRankMax;

		private readonly List<int> _DayRewardType = new List<int>();

		private readonly List<int> _DayRewardValue1 = new List<int>();

		private readonly List<int> _DayRewardValue2 = new List<int>();

		private int _OreAmount;

		private int _RewardType;

		private int _RewardValue1;

		private int _RewardValue2;

		private int _Section;

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

		[ProtoMember(2, IsRequired = false, Name = "RankMin", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RankMin
		{
			get
			{
				return this._RankMin;
			}
			set
			{
				this._RankMin = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "RankMax", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RankMax
		{
			get
			{
				return this._RankMax;
			}
			set
			{
				this._RankMax = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Amount1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Amount1
		{
			get
			{
				return this._Amount1;
			}
			set
			{
				this._Amount1 = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Amount2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Amount2
		{
			get
			{
				return this._Amount2;
			}
			set
			{
				this._Amount2 = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "DayRankMin", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DayRankMin
		{
			get
			{
				return this._DayRankMin;
			}
			set
			{
				this._DayRankMin = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "DayRankMax", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DayRankMax
		{
			get
			{
				return this._DayRankMax;
			}
			set
			{
				this._DayRankMax = value;
			}
		}

		[ProtoMember(8, Name = "DayRewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> DayRewardType
		{
			get
			{
				return this._DayRewardType;
			}
		}

		[ProtoMember(9, Name = "DayRewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> DayRewardValue1
		{
			get
			{
				return this._DayRewardValue1;
			}
		}

		[ProtoMember(10, Name = "DayRewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> DayRewardValue2
		{
			get
			{
				return this._DayRewardValue2;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "OreAmount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int OreAmount
		{
			get
			{
				return this._OreAmount;
			}
			set
			{
				this._OreAmount = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "RewardType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardType
		{
			get
			{
				return this._RewardType;
			}
			set
			{
				this._RewardType = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "RewardValue1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardValue1
		{
			get
			{
				return this._RewardValue1;
			}
			set
			{
				this._RewardValue1 = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "RewardValue2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardValue2
		{
			get
			{
				return this._RewardValue2;
			}
			set
			{
				this._RewardValue2 = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "Section", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Section
		{
			get
			{
				return this._Section;
			}
			set
			{
				this._Section = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
