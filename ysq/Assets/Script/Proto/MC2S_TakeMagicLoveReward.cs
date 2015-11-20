using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TakeMagicLoveReward")]
	[Serializable]
	public class MC2S_TakeMagicLoveReward : IExtensible
	{
		private int _Index;

		private int _RewardID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Index", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Index
		{
			get
			{
				return this._Index;
			}
			set
			{
				this._Index = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "RewardID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardID
		{
			get
			{
				return this._RewardID;
			}
			set
			{
				this._RewardID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
