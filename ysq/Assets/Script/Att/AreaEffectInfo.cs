using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "AreaEffectInfo")]
	[Serializable]
	public class AreaEffectInfo : IExtensible
	{
		private int _ID;

		private string _ResLoc = string.Empty;

		private int _TickCount;

		private float _TickInterval;

		private int _TriggerSkillID;

		private float _MaxDuration;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "ID", DataFormat = DataFormat.TwosComplement)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ResLoc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ResLoc
		{
			get
			{
				return this._ResLoc;
			}
			set
			{
				this._ResLoc = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "TickCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TickCount
		{
			get
			{
				return this._TickCount;
			}
			set
			{
				this._TickCount = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "TickInterval", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float TickInterval
		{
			get
			{
				return this._TickInterval;
			}
			set
			{
				this._TickInterval = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "TriggerSkillID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TriggerSkillID
		{
			get
			{
				return this._TriggerSkillID;
			}
			set
			{
				this._TriggerSkillID = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "MaxDuration", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float MaxDuration
		{
			get
			{
				return this._MaxDuration;
			}
			set
			{
				this._MaxDuration = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
