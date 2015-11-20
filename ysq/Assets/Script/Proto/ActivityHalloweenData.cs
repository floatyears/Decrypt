using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityHalloweenData")]
	[Serializable]
	public class ActivityHalloweenData : IExtensible
	{
		private BaseActivityData _Base;

		private ActivityHalloweenExt _Ext;

		private bool _Fire;

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

		[ProtoMember(2, IsRequired = false, Name = "Ext", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ActivityHalloweenExt Ext
		{
			get
			{
				return this._Ext;
			}
			set
			{
				this._Ext = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Fire", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Fire
		{
			get
			{
				return this._Fire;
			}
			set
			{
				this._Fire = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
