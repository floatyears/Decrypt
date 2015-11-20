using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_TakeMailAffix")]
	[Serializable]
	public class MC2S_TakeMailAffix : IExtensible
	{
		private uint _MailID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "MailID", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MailID
		{
			get
			{
				return this._MailID;
			}
			set
			{
				this._MailID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
