using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_Interaction")]
	[Serializable]
	public class MS2C_Interaction : IExtensible
	{
		private int _Result;

		private bool _Failure;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				this._Result = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Failure", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Failure
		{
			get
			{
				return this._Failure;
			}
			set
			{
				this._Failure = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
