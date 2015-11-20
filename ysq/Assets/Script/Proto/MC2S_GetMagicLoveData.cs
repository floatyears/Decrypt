using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GetMagicLoveData")]
	[Serializable]
	public class MC2S_GetMagicLoveData : IExtensible
	{
		private uint _version;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "version", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint version
		{
			get
			{
				return this._version;
			}
			set
			{
				this._version = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
