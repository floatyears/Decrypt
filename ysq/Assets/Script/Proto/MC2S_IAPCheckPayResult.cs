using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_IAPCheckPayResult")]
	[Serializable]
	public class MC2S_IAPCheckPayResult : IExtensible
	{
		private string _OrderID = string.Empty;

		private string _ReceiptData = string.Empty;

		private int _OrderStatus;

		private string _Currency = string.Empty;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "OrderID", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(2, IsRequired = false, Name = "ReceiptData", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ReceiptData
		{
			get
			{
				return this._ReceiptData;
			}
			set
			{
				this._ReceiptData = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "OrderStatus", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int OrderStatus
		{
			get
			{
				return this._OrderStatus;
			}
			set
			{
				this._OrderStatus = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Currency", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Currency
		{
			get
			{
				return this._Currency;
			}
			set
			{
				this._Currency = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
