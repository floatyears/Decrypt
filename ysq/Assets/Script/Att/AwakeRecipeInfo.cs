using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Att
{
	[ProtoContract(Name = "AwakeRecipeInfo")]
	[Serializable]
	public class AwakeRecipeInfo : IExtensible
	{
		private int _ID;

		private readonly List<int> _ItemID = new List<int>();

		private readonly List<int> _Count = new List<int>();

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

		[ProtoMember(2, Name = "ItemID", DataFormat = DataFormat.TwosComplement)]
		public List<int> ItemID
		{
			get
			{
				return this._ItemID;
			}
		}

		[ProtoMember(3, Name = "Count", DataFormat = DataFormat.TwosComplement)]
		public List<int> Count
		{
			get
			{
				return this._Count;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
