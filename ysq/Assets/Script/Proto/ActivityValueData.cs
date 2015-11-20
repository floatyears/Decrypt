using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityValueData")]
	[Serializable]
	public class ActivityValueData : IExtensible
	{
		private BaseActivityData _Base;

		private int _Type;

		private int _Value1;

		private int _Value2;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Base", DataFormat = DataFormat.Default), DefaultValue(null)]
		public BaseActivityData Base
		{
			get
			{
				return this._Base;
			}
			set
			{
				this._Base = value;
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
