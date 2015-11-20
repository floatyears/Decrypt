using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "MapInfo")]
	[Serializable]
	public class MapInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _ResLoc = string.Empty;

		private string _Desc = string.Empty;

		private int _PreID;

		private int _NextID;

		private readonly List<int> _RewardType = new List<int>();

		private readonly List<int> _RewardValue1 = new List<int>();

		private readonly List<int> _RewardValue2 = new List<int>();

		private readonly List<int> _NeedStar = new List<int>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "ID", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(2, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "ResLoc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string ResLoc
		{
			get
			{
				return this._ResLoc;
			}
			set
			{
				this._ResLoc = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Desc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Desc
		{
			get
			{
				return this._Desc;
			}
			set
			{
				this._Desc = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "PreID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PreID
		{
			get
			{
				return this._PreID;
			}
			set
			{
				this._PreID = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "NextID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int NextID
		{
			get
			{
				return this._NextID;
			}
			set
			{
				this._NextID = value;
			}
		}

		[ProtoMember(8, Name = "RewardType", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardType
		{
			get
			{
				return this._RewardType;
			}
		}

		[ProtoMember(9, Name = "RewardValue1", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue1
		{
			get
			{
				return this._RewardValue1;
			}
		}

		[ProtoMember(10, Name = "RewardValue2", DataFormat = DataFormat.TwosComplement)]
		public List<int> RewardValue2
		{
			get
			{
				return this._RewardValue2;
			}
		}

		[ProtoMember(11, Name = "NeedStar", DataFormat = DataFormat.TwosComplement)]
		public List<int> NeedStar
		{
			get
			{
				return this._NeedStar;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
