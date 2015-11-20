using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "FashionInfo")]
	[Serializable]
	public class FashionInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _Icon = string.Empty;

		private string _ResLoc = string.Empty;

		private string _WeaponResLoc = string.Empty;

		private int _Quality;

		private int _Gender;

		private int _Enable;

		private int _Source;

		private string _Desc = string.Empty;

		private int _Day;

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

		[ProtoMember(4, IsRequired = false, Name = "ResLoc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ResLoc
		{
			get
			{
				return this._ResLoc;
			}
			set
			{
				this._ResLoc = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "WeaponResLoc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string WeaponResLoc
		{
			get
			{
				return this._WeaponResLoc;
			}
			set
			{
				this._WeaponResLoc = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Quality", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "Gender", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Gender
		{
			get
			{
				return this._Gender;
			}
			set
			{
				this._Gender = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "Enable", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Enable
		{
			get
			{
				return this._Enable;
			}
			set
			{
				this._Enable = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Source", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(10, IsRequired = false, Name = "Desc", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(11, IsRequired = false, Name = "Day", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Day
		{
			get
			{
				return this._Day;
			}
			set
			{
				this._Day = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
