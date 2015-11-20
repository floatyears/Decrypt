using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_BuyVipReward")]
	[Serializable]
	public class MC2S_BuyVipReward : IExtensible
	{
		private int _VipInfoID;

		private int _Type;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "VipInfoID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int VipInfoID
		{
			get
			{
				return this._VipInfoID;
			}
			set
			{
				this._VipInfoID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				this._Type = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
