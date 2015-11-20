using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_MailVersionUpdate")]
	[Serializable]
	public class MS2C_MailVersionUpdate : IExtensible
	{
		private uint _MailVersion;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "MailVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint MailVersion
		{
			get
			{
				return this._MailVersion;
			}
			set
			{
				this._MailVersion = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
