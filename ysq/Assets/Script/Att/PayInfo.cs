using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "PayInfo")]
	[Serializable]
	public class PayInfo : IExtensible
	{
		private int _ID;

		private string _ProductID = string.Empty;

		private string _Name = string.Empty;

		private string _Desc = string.Empty;

		private string _Icon = string.Empty;

		private float _Price;

		private int _Type;

		private int _Diamond;

		private int _Give;

		private int _Platform;

		private bool _Test;

		private float _ShowPrice;

		private bool _Recommend;

		private int _LenovoPID;

		private int _CoolpadPID;

		private string _GooglePID = string.Empty;

		private string _NStorePID = string.Empty;

		private string _TStorePID = string.Empty;

		private int _KuaiFaPID;

		private int _Give2;

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

		[ProtoMember(2, IsRequired = false, Name = "ProductID", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ProductID
		{
			get
			{
				return this._ProductID;
			}
			set
			{
				this._ProductID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(5, IsRequired = false, Name = "Icon", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(6, IsRequired = false, Name = "Price", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float Price
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

		[ProtoMember(7, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(8, IsRequired = false, Name = "Diamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Diamond
		{
			get
			{
				return this._Diamond;
			}
			set
			{
				this._Diamond = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Give", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Give
		{
			get
			{
				return this._Give;
			}
			set
			{
				this._Give = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "Platform", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Platform
		{
			get
			{
				return this._Platform;
			}
			set
			{
				this._Platform = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Test", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Test
		{
			get
			{
				return this._Test;
			}
			set
			{
				this._Test = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "ShowPrice", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float ShowPrice
		{
			get
			{
				return this._ShowPrice;
			}
			set
			{
				this._ShowPrice = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "Recommend", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Recommend
		{
			get
			{
				return this._Recommend;
			}
			set
			{
				this._Recommend = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "LenovoPID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LenovoPID
		{
			get
			{
				return this._LenovoPID;
			}
			set
			{
				this._LenovoPID = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "CoolpadPID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CoolpadPID
		{
			get
			{
				return this._CoolpadPID;
			}
			set
			{
				this._CoolpadPID = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "GooglePID", DataFormat = DataFormat.Default), DefaultValue("")]
		public string GooglePID
		{
			get
			{
				return this._GooglePID;
			}
			set
			{
				this._GooglePID = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "NStorePID", DataFormat = DataFormat.Default), DefaultValue("")]
		public string NStorePID
		{
			get
			{
				return this._NStorePID;
			}
			set
			{
				this._NStorePID = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "TStorePID", DataFormat = DataFormat.Default), DefaultValue("")]
		public string TStorePID
		{
			get
			{
				return this._TStorePID;
			}
			set
			{
				this._TStorePID = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "KuaiFaPID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int KuaiFaPID
		{
			get
			{
				return this._KuaiFaPID;
			}
			set
			{
				this._KuaiFaPID = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "Give2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Give2
		{
			get
			{
				return this._Give2;
			}
			set
			{
				this._Give2 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
