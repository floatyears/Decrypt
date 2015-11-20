using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_OrderInfo")]
	[Serializable]
	public class MS2C_OrderInfo : IExtensible
	{
		private int _Result;

		private string _OrderID = string.Empty;

		private int _ProductID;

		private string _AccessToken = string.Empty;

		private string _Etc = string.Empty;

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

		[ProtoMember(3, IsRequired = false, Name = "ProductID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ProductID
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

		[ProtoMember(4, IsRequired = false, Name = "AccessToken", DataFormat = DataFormat.Default), DefaultValue("")]
		public string AccessToken
		{
			get
			{
				return this._AccessToken;
			}
			set
			{
				this._AccessToken = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Etc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Etc
		{
			get
			{
				return this._Etc;
			}
			set
			{
				this._Etc = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
