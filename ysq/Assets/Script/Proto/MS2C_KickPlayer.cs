using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_KickPlayer")]
	[Serializable]
	public class MS2C_KickPlayer : IExtensible
	{
		private int _Reason;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Reason", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Reason
		{
			get
			{
				return this._Reason;
			}
			set
			{
				this._Reason = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
