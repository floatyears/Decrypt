using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityPayData")]
	[Serializable]
	public class ActivityPayData : IExtensible
	{
		private BaseActivityData _Base;

		private readonly List<RewardData> _Data = new List<RewardData>();

		private int _ProductID;

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

		[ProtoMember(2, Name = "Data", DataFormat = DataFormat.Default)]
		public List<RewardData> Data
		{
			get
			{
				return this._Data;
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
