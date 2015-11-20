using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Att
{
	[ProtoContract(Name = "LootInfo")]
	[Serializable]
	public class LootInfo : IExtensible
	{
		private int _ID;

		private readonly List<int> _ItemID = new List<int>();

		private readonly List<int> _Rate = new List<int>();

		private readonly List<int> _MinCount = new List<int>();

		private readonly List<int> _MaxCount = new List<int>();

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

		[ProtoMember(3, Name = "Rate", DataFormat = DataFormat.TwosComplement)]
		public List<int> Rate
		{
			get
			{
				return this._Rate;
			}
		}

		[ProtoMember(4, Name = "MinCount", DataFormat = DataFormat.TwosComplement)]
		public List<int> MinCount
		{
			get
			{
				return this._MinCount;
			}
		}

		[ProtoMember(5, Name = "MaxCount", DataFormat = DataFormat.TwosComplement)]
		public List<int> MaxCount
		{
			get
			{
				return this._MaxCount;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
