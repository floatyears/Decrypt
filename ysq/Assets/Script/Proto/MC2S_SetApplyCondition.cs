using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_SetApplyCondition")]
	[Serializable]
	public class MC2S_SetApplyCondition : IExtensible
	{
		private int _Level;

		private bool _NeedVerify;

		private int _CombatValue;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Level
		{
			get
			{
				return this._Level;
			}
			set
			{
				this._Level = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "NeedVerify", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool NeedVerify
		{
			get
			{
				return this._NeedVerify;
			}
			set
			{
				this._NeedVerify = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "CombatValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CombatValue
		{
			get
			{
				return this._CombatValue;
			}
			set
			{
				this._CombatValue = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
