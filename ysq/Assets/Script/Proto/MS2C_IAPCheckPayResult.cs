using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_IAPCheckPayResult")]
	[Serializable]
	public class MS2C_IAPCheckPayResult : IExtensible
	{
		private int _Result;

		private string _OrderID = string.Empty;

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

		[ProtoMember(2, IsRequired = false, Name = "OrderID", DataFormat = DataFormat.Default), DefaultValue("")]
		public string OrderID
		{
			get
			{
				return this._OrderID;
			}
			set
			{
				this._OrderID = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
