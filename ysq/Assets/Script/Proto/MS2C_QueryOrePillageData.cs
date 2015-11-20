using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_QueryOrePillageData")]
	[Serializable]
	public class MS2C_QueryOrePillageData : IExtensible
	{
		private int _Result;

		private readonly List<OrePillageTarget> _Data = new List<OrePillageTarget>();

		private int _Rank;

		private int _OreAmount;

		private int _LostOre;

		private int _PillageCount;

		private int _Status;

		private int _Timestamp;

		private int _LastOreAmount;

		private int _RewardFlag;

		private int _PillageCountTimestamp;

		private int _BuyPillageCount;

		private int _RewardTime;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				this._Result = value;
			}
		}

		[ProtoMember(2, Name = "Data", DataFormat = DataFormat.Default)]
		public List<OrePillageTarget> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Rank", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Rank
		{
			get
			{
				return this._Rank;
			}
			set
			{
				this._Rank = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "OreAmount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "LostOre", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LostOre
		{
			get
			{
				return this._LostOre;
			}
			set
			{
				this._LostOre = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "PillageCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PillageCount
		{
			get
			{
				return this._PillageCount;
			}
			set
			{
				this._PillageCount = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Status", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				this._Status = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "Timestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Timestamp
		{
			get
			{
				return this._Timestamp;
			}
			set
			{
				this._Timestamp = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "LastOreAmount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LastOreAmount
		{
			get
			{
				return this._LastOreAmount;
			}
			set
			{
				this._LastOreAmount = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "RewardFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardFlag
		{
			get
			{
				return this._RewardFlag;
			}
			set
			{
				this._RewardFlag = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "PillageCountTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PillageCountTimestamp
		{
			get
			{
				return this._PillageCountTimestamp;
			}
			set
			{
				this._PillageCountTimestamp = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "BuyPillageCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(13, IsRequired = false, Name = "RewardTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardTime
		{
			get
			{
				return this._RewardTime;
			}
			set
			{
				this._RewardTime = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
