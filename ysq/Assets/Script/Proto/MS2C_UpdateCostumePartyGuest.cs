using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdateCostumePartyGuest")]
	[Serializable]
	public class MS2C_UpdateCostumePartyGuest : IExtensible
	{
		private ulong _PlayerID;

		private int _Timestamp;

		private int _PetID;

		private int _PetTimestamp;

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

		[ProtoMember(2, IsRequired = false, Name = "Timestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Timestamp
		{
			get
			{
				return this._Timestamp;
			}
			set
			{
				this._Timestamp = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "PetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PetID
		{
			get
			{
				return this._PetID;
			}
			set
			{
				this._PetID = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "PetTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PetTimestamp
		{
			get
			{
				return this._PetTimestamp;
			}
			set
			{
				this._PetTimestamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
