using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_InteractionMessage")]
	[Serializable]
	public class MS2C_InteractionMessage : IExtensible
	{
		private int _Type;

		private int _Gender;

		private string _Name1 = string.Empty;

		private string _Name2 = string.Empty;

		private int _Reward;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(2, IsRequired = false, Name = "Gender", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "Name1", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Name1
		{
			get
			{
				return this._Name1;
			}
			set
			{
				this._Name1 = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Name2", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Name2
		{
			get
			{
				return this._Name2;
			}
			set
			{
				this._Name2 = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Reward", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Reward
		{
			get
			{
				return this._Reward;
			}
			set
			{
				this._Reward = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
