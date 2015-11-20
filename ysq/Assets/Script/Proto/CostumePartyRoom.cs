using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "CostumePartyRoom")]
	[Serializable]
	public class CostumePartyRoom : IExtensible
	{
		private uint _ID;

		private ulong _MasterID;

		private readonly List<CostumePartyGuest> _Data = new List<CostumePartyGuest>();

		private readonly List<MS2C_InteractionMessage> _msg = new List<MS2C_InteractionMessage>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "MasterID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong MasterID
		{
			get
			{
				return this._MasterID;
			}
			set
			{
				this._MasterID = value;
			}
		}

		[ProtoMember(3, Name = "Data", DataFormat = DataFormat.Default)]
		public List<CostumePartyGuest> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(4, Name = "msg", DataFormat = DataFormat.Default)]
		public List<MS2C_InteractionMessage> msg
		{
			get
			{
				return this._msg;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
