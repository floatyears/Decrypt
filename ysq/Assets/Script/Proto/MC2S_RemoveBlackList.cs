using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_RemoveBlackList")]
	[Serializable]
	public class MC2S_RemoveBlackList : IExtensible
	{
		private ulong _GUID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "GUID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong GUID
		{
			get
			{
				return this._GUID;
			}
			set
			{
				this._GUID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
