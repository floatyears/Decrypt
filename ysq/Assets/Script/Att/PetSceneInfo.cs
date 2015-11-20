using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Att
{
	[ProtoContract(Name = "PetSceneInfo")]
	[Serializable]
	public class PetSceneInfo : IExtensible
	{
		private int _ID;

		private readonly List<int> _SceneIDs = new List<int>();

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

		[ProtoMember(2, Name = "SceneIDs", DataFormat = DataFormat.TwosComplement)]
		public List<int> SceneIDs
		{
			get
			{
				return this._SceneIDs;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
