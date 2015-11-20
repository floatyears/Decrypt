using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildEvent")]
	[Serializable]
	public class GuildEvent : IExtensible
	{
		private int _ID;

		private int _Type;

		private int _Value1;

		private int _Value2;

		private string _Value3 = string.Empty;

		private string _Value4 = string.Empty;

		private int _TimeStamp;

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

		[ProtoMember(5, IsRequired = false, Name = "Value3", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Value3
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

		[ProtoMember(6, IsRequired = false, Name = "Value4", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Value4
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

		[ProtoMember(7, IsRequired = false, Name = "TimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TimeStamp
		{
			get
			{
				return this._TimeStamp;
			}
			set
			{
				this._TimeStamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
