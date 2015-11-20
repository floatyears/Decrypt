using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityShopItem")]
	[Serializable]
	public class ActivityShopItem : IExtensible
	{
		private int _ID;

		private int _Type;

		private int _Value1;

		private int _Value2;

		private int _CurrencyType;

		private int _Price;

		private int _BuyCount;

		private int _MaxCount;

		private int _BuyTimes;

		private int _MaxTimes;

		private int _Price2;

		private int _OriginalPrice;

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

		[ProtoMember(2, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "Value1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value1
		{
			get
			{
				return this._Value1;
			}
			set
			{
				this._Value1 = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Value2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value2
		{
			get
			{
				return this._Value2;
			}
			set
			{
				this._Value2 = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "CurrencyType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CurrencyType
		{
			get
			{
				return this._CurrencyType;
			}
			set
			{
				this._CurrencyType = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Price", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Price
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

		[ProtoMember(7, IsRequired = false, Name = "BuyCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BuyCount
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

		[ProtoMember(8, IsRequired = false, Name = "MaxCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxCount
		{
			get
			{
				return this._MaxCount;
			}
			set
			{
				this._MaxCount = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "BuyTimes", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BuyTimes
		{
			get
			{
				return this._BuyTimes;
			}
			set
			{
				this._BuyTimes = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "MaxTimes", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxTimes
		{
			get
			{
				return this._MaxTimes;
			}
			set
			{
				this._MaxTimes = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Price2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Price2
		{
			get
			{
				return this._Price2;
			}
			set
			{
				this._Price2 = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "OriginalPrice", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int OriginalPrice
		{
			get
			{
				return this._OriginalPrice;
			}
			set
			{
				this._OriginalPrice = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
