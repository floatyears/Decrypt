using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "AchievementData")]
	[Serializable]
	public class AchievementData : IExtensible
	{
		private int _AchievementID;

		private int _Value;

		private int _CoolDown;

		private bool _TakeReward;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "AchievementID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AchievementID
		{
			get
			{
				return this._AchievementID;
			}
			set
			{
				this._AchievementID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Value", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "CoolDown", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CoolDown
		{
			get
			{
				return this._CoolDown;
			}
			set
			{
				this._CoolDown = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "TakeReward", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool TakeReward
		{
			get
			{
				return this._TakeReward;
			}
			set
			{
				this._TakeReward = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
