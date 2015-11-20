using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TakeGuildBossReward")]
	[Serializable]
	public class MS2C_TakeGuildBossReward : IExtensible
	{
		private int _Result;

		private RewardData _Reward;

		private int _Reputation;

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

		[ProtoMember(2, IsRequired = false, Name = "Reward", DataFormat = DataFormat.Default), DefaultValue(null)]
		public RewardData Reward
		{
			get
			{
				return this._Reward;
			}
			set
			{
				this._Reward = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Reputation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Reputation
		{
			get
			{
				return this._Reputation;
			}
			set
			{
				this._Reputation = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
