using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TakeNationalDayReward")]
	[Serializable]
	public class MC2S_TakeNationalDayReward : IExtensible
	{
		private int _ExchangeID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ExchangeID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ExchangeID
		{
			get
			{
				return this._ExchangeID;
			}
			set
			{
				this._ExchangeID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
