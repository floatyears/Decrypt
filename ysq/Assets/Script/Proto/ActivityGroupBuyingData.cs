using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityGroupBuyingData")]
	[Serializable]
	public class ActivityGroupBuyingData : IExtensible
	{
		private BaseActivityData _Base;

		private readonly List<ActivityGroupBuyingItem> _Data = new List<ActivityGroupBuyingItem>();

		private readonly List<ActivityGroupBuyingScoreReward> _ScoreReward = new List<ActivityGroupBuyingScoreReward>();

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
		public List<ActivityGroupBuyingItem> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(3, Name = "ScoreReward", DataFormat = DataFormat.Default)]
		public List<ActivityGroupBuyingScoreReward> ScoreReward
		{
			get
			{
				return this._ScoreReward;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
