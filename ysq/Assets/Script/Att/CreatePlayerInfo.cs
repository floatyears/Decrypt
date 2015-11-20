using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Att
{
	[ProtoContract(Name = "CreatePlayerInfo")]
	[Serializable]
	public class CreatePlayerInfo : IExtensible
	{
		private int _ID;

		private readonly List<int> _EquipInfoID = new List<int>();

		private readonly List<int> _PetInfoID = new List<int>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "ID", DataFormat = DataFormat.TwosComplement)]
		public int ID
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

		[ProtoMember(2, Name = "EquipInfoID", DataFormat = DataFormat.TwosComplement)]
		public List<int> EquipInfoID
		{
			get
			{
				return this._EquipInfoID;
			}
		}

		[ProtoMember(3, Name = "PetInfoID", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetInfoID
		{
			get
			{
				return this._PetInfoID;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
