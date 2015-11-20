using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_RollEquip")]
	[Serializable]
	public class MS2C_RollEquip : IExtensible
	{
		private int _Result;

		private readonly List<RewardData> _Data = new List<RewardData>();

		private int _DoubleTimestamp;

		private bool _DoubleReward;

		private int _Apple;

		private int _OneCost;

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

		[ProtoMember(2, Name = "Data", DataFormat = DataFormat.Default)]
		public List<RewardData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "DoubleTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DoubleTimestamp
		{
			get
			{
				return this._DoubleTimestamp;
			}
			set
			{
				this._DoubleTimestamp = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "DoubleReward", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool DoubleReward
		{
			get
			{
				return this._DoubleReward;
			}
			set
			{
				this._DoubleReward = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Apple", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Apple
		{
			get
			{
				return this._Apple;
			}
			set
			{
				this._Apple = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "OneCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int OneCost
		{
			get
			{
				return this._OneCost;
			}
			set
			{
				this._OneCost = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
