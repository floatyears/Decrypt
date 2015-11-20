using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "ShopInfo")]
	[Serializable]
	public class ShopInfo : IExtensible
	{
		private int _ID;

		private int _InfoID;

		private int _Count;

		private int _CurrencyType;

		private int _Price;

		private int _InfoType;

		private int _IsFashion;

		private int _Times;

		private int _Type;

		private int _Value;

		private int _Price2;

		private int _ResetType;

		private int _Level;

		private int _ShopType;

		private int _CurrencyType2;

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

		[ProtoMember(3, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(4, IsRequired = false, Name = "CurrencyType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "Price", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "InfoType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "IsFashion", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int IsFashion
		{
			get
			{
				return this._IsFashion;
			}
			set
			{
				this._IsFashion = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Times", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Times
		{
			get
			{
				return this._Times;
			}
			set
			{
				this._Times = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(11, IsRequired = false, Name = "Value", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "Price2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(13, IsRequired = false, Name = "ResetType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ResetType
		{
			get
			{
				return this._ResetType;
			}
			set
			{
				this._ResetType = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Level
		{
			get
			{
				return this._Level;
			}
			set
			{
				this._Level = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "ShopType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(16, IsRequired = false, Name = "CurrencyType2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CurrencyType2
		{
			get
			{
				return this._CurrencyType2;
			}
			set
			{
				this._CurrencyType2 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
