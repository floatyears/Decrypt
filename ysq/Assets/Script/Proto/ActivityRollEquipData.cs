using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityRollEquipData")]
	[Serializable]
	public class ActivityRollEquipData : IExtensible
	{
		private BaseActivityData _Base;

		private readonly List<int> _ItemID = new List<int>();

		private int _DoubleRate;

		private int _DoubleTimestamp;

		private int _OneCost;

		private int _TenCost;

		private int _OneCost1;

		private int _OneCost2;

		private int _OneCost3;

		private int _AddRewardRate;

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

		[ProtoMember(2, Name = "ItemID", DataFormat = DataFormat.TwosComplement)]
		public List<int> ItemID
		{
			get
			{
				return this._ItemID;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "DoubleRate", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DoubleRate
		{
			get
			{
				return this._DoubleRate;
			}
			set
			{
				this._DoubleRate = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "DoubleTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "OneCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "TenCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TenCost
		{
			get
			{
				return this._TenCost;
			}
			set
			{
				this._TenCost = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "OneCost1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int OneCost1
		{
			get
			{
				return this._OneCost1;
			}
			set
			{
				this._OneCost1 = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "OneCost2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int OneCost2
		{
			get
			{
				return this._OneCost2;
			}
			set
			{
				this._OneCost2 = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "OneCost3", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int OneCost3
		{
			get
			{
				return this._OneCost3;
			}
			set
			{
				this._OneCost3 = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "AddRewardRate", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AddRewardRate
		{
			get
			{
				return this._AddRewardRate;
			}
			set
			{
				this._AddRewardRate = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
