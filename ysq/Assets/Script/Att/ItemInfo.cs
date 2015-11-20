using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "ItemInfo")]
	[Serializable]
	public class ItemInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _Icon = string.Empty;

		private string _Desc = string.Empty;

		private int _Type;

		private int _SubType;

		private int _Quality;

		private int _SubQuality;

		private bool _Stackable;

		private int _Price;

		private int _Source;

		private int _Value1;

		private int _Value2;

		private int _Value3;

		private int _Value4;

		private int _Value5;

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

		[ProtoMember(2, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Icon", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Icon
		{
			get
			{
				return this._Icon;
			}
			set
			{
				this._Icon = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Desc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Desc
		{
			get
			{
				return this._Desc;
			}
			set
			{
				this._Desc = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "SubType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SubType
		{
			get
			{
				return this._SubType;
			}
			set
			{
				this._SubType = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "Quality", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Quality
		{
			get
			{
				return this._Quality;
			}
			set
			{
				this._Quality = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "SubQuality", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SubQuality
		{
			get
			{
				return this._SubQuality;
			}
			set
			{
				this._SubQuality = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "Stackable", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Stackable
		{
			get
			{
				return this._Stackable;
			}
			set
			{
				this._Stackable = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Price", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(12, IsRequired = false, Name = "Source", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Source
		{
			get
			{
				return this._Source;
			}
			set
			{
				this._Source = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "Value1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(14, IsRequired = false, Name = "Value2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(15, IsRequired = false, Name = "Value3", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value3
		{
			get
			{
				return this._Value3;
			}
			set
			{
				this._Value3 = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "Value4", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value4
		{
			get
			{
				return this._Value4;
			}
			set
			{
				this._Value4 = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "Value5", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value5
		{
			get
			{
				return this._Value5;
			}
			set
			{
				this._Value5 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
