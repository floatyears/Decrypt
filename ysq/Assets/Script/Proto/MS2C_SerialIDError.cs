using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_SerialIDError")]
	[Serializable]
	public class MS2C_SerialIDError : IExtensible
	{
		private uint _MsgSerialID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "MsgSerialID", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MsgSerialID
		{
			get
			{
				return this._MsgSerialID;
			}
			set
			{
				this._MsgSerialID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
