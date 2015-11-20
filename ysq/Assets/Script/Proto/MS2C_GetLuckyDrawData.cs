using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetLuckyDrawData")]
	[Serializable]
	public class MS2C_GetLuckyDrawData : IExtensible
	{
		private int _Result;

		private readonly List<RankData> _Data = new List<RankData>();

		private int _Score;

		private int _FreeTimestamp;

		private int _OverTime;

		private readonly List<RewardData> _RankReward = new List<RewardData>();

		private string _Detail = string.Empty;

		private int _RetentionTime;

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
		public List<RankData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Score", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				this._Score = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "FreeTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FreeTimestamp
		{
			get
			{
				return this._FreeTimestamp;
			}
			set
			{
				this._FreeTimestamp = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "OverTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int OverTime
		{
			get
			{
				return this._OverTime;
			}
			set
			{
				this._OverTime = value;
			}
		}

		[ProtoMember(6, Name = "RankReward", DataFormat = DataFormat.Default)]
		public List<RewardData> RankReward
		{
			get
			{
				return this._RankReward;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Detail", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Detail
		{
			get
			{
				return this._Detail;
			}
			set
			{
				this._Detail = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "RetentionTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RetentionTime
		{
			get
			{
				return this._RetentionTime;
			}
			set
			{
				this._RetentionTime = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
