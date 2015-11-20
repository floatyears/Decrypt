using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_RemoveFriendData")]
	[Serializable]
	public class MS2C_RemoveFriendData : IExtensible
	{
		private ulong _GUID;

		private int _FriendType;

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

		[ProtoMember(2, IsRequired = false, Name = "FriendType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FriendType
		{
			get
			{
				return this._FriendType;
			}
			set
			{
				this._FriendType = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
