using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_BroadcastDamage")]
	[Serializable]
	public class MS2C_BroadcastDamage : IExtensible
	{
		private int _Slot;

		private int _InfoID;

		private float _HealthPct;

		private string _Name = string.Empty;

		private long _Damage;

		private int _ResurrectType;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Slot", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Slot
		{
			get
			{
				return this._Slot;
			}
			set
			{
				this._Slot = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "InfoID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int InfoID
		{
			get
			{
				return this._InfoID;
			}
			set
			{
				this._InfoID = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "HealthPct", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float HealthPct
		{
			get
			{
				return this._HealthPct;
			}
			set
			{
				this._HealthPct = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Damage", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long Damage
		{
			get
			{
				return this._Damage;
			}
			set
			{
				this._Damage = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "ResurrectType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ResurrectType
		{
			get
			{
				return this._ResurrectType;
			}
			set
			{
				this._ResurrectType = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
