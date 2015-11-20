using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_HotTimeData")]
	[Serializable]
	public class MS2C_HotTimeData : IExtensible
	{
		private int _Result;

		private int _Version;

		private bool _InTime;

		private string _Content = string.Empty;

		private string _GiftName = string.Empty;

		private readonly List<RewardData> _Reward = new List<RewardData>();

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

		[ProtoMember(2, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				this._Version = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "InTime", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool InTime
		{
			get
			{
				return this._InTime;
			}
			set
			{
				this._InTime = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Content", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Content
		{
			get
			{
				return this._Content;
			}
			set
			{
				this._Content = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "GiftName", DataFormat = DataFormat.Default), DefaultValue("")]
		public string GiftName
		{
			get
			{
				return this._GiftName;
			}
			set
			{
				this._GiftName = value;
			}
		}

		[ProtoMember(6, Name = "Reward", DataFormat = DataFormat.Default)]
		public List<RewardData> Reward
		{
			get
			{
				return this._Reward;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
