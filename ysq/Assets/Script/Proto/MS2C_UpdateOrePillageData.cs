using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdateOrePillageData")]
	[Serializable]
	public class MS2C_UpdateOrePillageData : IExtensible
	{
		private int _PillageCount;

		private int _RewardFlag;

		private int _PillageCountTimestamp;

		private int _BuyPillageCount;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "PillageCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(2, IsRequired = false, Name = "RewardFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "PillageCountTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(4, IsRequired = false, Name = "BuyPillageCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
