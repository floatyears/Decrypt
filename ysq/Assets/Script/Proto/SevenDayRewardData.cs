using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "SevenDayRewardData")]
	[Serializable]
	public class SevenDayRewardData : IExtensible
	{
		private int _ID;

		private int _Value;

		private bool _TakeReward;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "TakeReward", DataFormat = DataFormat.Default), DefaultValue(false)]
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
