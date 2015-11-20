using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildWarPrompt")]
	[Serializable]
	public class MS2C_GuildWarPrompt : IExtensible
	{
		private EGuildWarPromptType _ID = EGuildWarPromptType.EWNT_Kill1;

		private string _Value1 = string.Empty;

		private string _Value2 = string.Empty;

		private int _Value3;

		private int _Value4;

		private int _Value5;

		private int _Value6;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarPromptType.EWNT_Kill1)]
		public EGuildWarPromptType ID
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

		[ProtoMember(2, IsRequired = false, Name = "Value1", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Value1
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

		[ProtoMember(3, IsRequired = false, Name = "Value2", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Value2
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

		[ProtoMember(4, IsRequired = false, Name = "Value3", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "Value4", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "Value5", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "Value6", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value6
		{
			get
			{
				return this._Value6;
			}
			set
			{
				this._Value6 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
