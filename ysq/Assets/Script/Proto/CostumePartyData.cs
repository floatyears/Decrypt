using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "CostumePartyData")]
	[Serializable]
	public class CostumePartyData : IExtensible
	{
		private CostumePartyRoom _RoomData;

		private readonly List<int> _CD = new List<int>();

		private int _Count;

		private bool _HasReward;

		private int _CarnivalType;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "RoomData", DataFormat = DataFormat.Default), DefaultValue(null)]
		public CostumePartyRoom RoomData
		{
			get
			{
				return this._RoomData;
			}
			set
			{
				this._RoomData = value;
			}
		}

		[ProtoMember(2, Name = "CD", DataFormat = DataFormat.TwosComplement)]
		public List<int> CD
		{
			get
			{
				return this._CD;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				this._Count = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "HasReward", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool HasReward
		{
			get
			{
				return this._HasReward;
			}
			set
			{
				this._HasReward = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "CarnivalType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CarnivalType
		{
			get
			{
				return this._CarnivalType;
			}
			set
			{
				this._CarnivalType = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
