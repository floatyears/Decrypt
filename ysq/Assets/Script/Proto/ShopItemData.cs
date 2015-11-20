using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ShopItemData")]
	[Serializable]
	public class ShopItemData : IExtensible
	{
		private int _ID;

		private int _InfoID;

		private int _Type;

		private uint _Price;

		private uint _Count;

		private uint _BuyCount;

		private int _InfoType;

		private int _Flag;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(2, IsRequired = false, Name = "InfoID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int InfoID
		{
			get
			{
				return this._InfoID;
			}
			set
			{
				this._InfoID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				this._Type = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Price", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Price
		{
			get
			{
				return this._Price;
			}
			set
			{
				this._Price = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				this._Count = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "BuyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint BuyCount
		{
			get
			{
				return this._BuyCount;
			}
			set
			{
				this._BuyCount = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "InfoType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int InfoType
		{
			get
			{
				return this._InfoType;
			}
			set
			{
				this._InfoType = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "Flag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Flag
		{
			get
			{
				return this._Flag;
			}
			set
			{
				this._Flag = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
