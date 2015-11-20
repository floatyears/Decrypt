using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityAchievementData")]
	[Serializable]
	public class ActivityAchievementData : IExtensible
	{
		private BaseActivityData _Base;

		private readonly List<AAItemData> _Data = new List<AAItemData>();

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
		public List<AAItemData> Data
		{
			get
			{
				return this._Data;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
