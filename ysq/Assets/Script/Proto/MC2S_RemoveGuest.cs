using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_RemoveGuest")]
	[Serializable]
	public class MC2S_RemoveGuest : IExtensible
	{
		private ulong _PlayerID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "PlayerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong PlayerID
		{
			get
			{
				return this._PlayerID;
			}
			set
			{
				this._PlayerID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
