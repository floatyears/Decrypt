using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_ShopBuyItem")]
	[Serializable]
	public class MC2S_ShopBuyItem : IExtensible
	{
		private int _ID;

		private uint _Price;

		private int _ShopType;

		private int _Count;

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

		[ProtoMember(2, IsRequired = false, Name = "Price", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(3, IsRequired = false, Name = "ShopType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ShopType
		{
			get
			{
				return this._ShopType;
			}
			set
			{
				this._ShopType = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Count
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
