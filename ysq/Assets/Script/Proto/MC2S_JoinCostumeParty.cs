using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_JoinCostumeParty")]
	[Serializable]
	public class MC2S_JoinCostumeParty : IExtensible
	{
		private uint _RoomID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "RoomID", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint RoomID
		{
			get
			{
				return this._RoomID;
			}
			set
			{
				this._RoomID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
