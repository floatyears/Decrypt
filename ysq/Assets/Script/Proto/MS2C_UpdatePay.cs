using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdatePay")]
	[Serializable]
	public class MS2C_UpdatePay : IExtensible
	{
		private PayData _Pay;

		private int _Result;

		private int _Value;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Pay", DataFormat = DataFormat.Default), DefaultValue(null)]
		public PayData Pay
		{
			get
			{
				return this._Pay;
			}
			set
			{
				this._Pay = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "Value", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
