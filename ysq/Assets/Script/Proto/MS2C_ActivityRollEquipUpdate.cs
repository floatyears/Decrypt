using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ActivityRollEquipUpdate")]
	[Serializable]
	public class MS2C_ActivityRollEquipUpdate : IExtensible
	{
		private ActivityRollEquipData _REData;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "REData", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ActivityRollEquipData REData
		{
			get
			{
				return this._REData;
			}
			set
			{
				this._REData = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
