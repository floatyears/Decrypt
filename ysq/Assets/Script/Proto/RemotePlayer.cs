using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "RemotePlayer")]
	[Serializable]
	public class RemotePlayer : IExtensible
	{
		private ulong _GUID;

		private string _Name = string.Empty;

		private int _Level;

		private int _VipLevel;

		private int _ConstellationLevel;

		private int _FashionID;

		private int _Gender;

		private readonly List<int> _PetInfoID = new List<int>();

		private string _GuildName = string.Empty;

		private int _CombatValue;

		private int _FurtherLevel;

		private int _AwakeLevel;

		private int _AwakeItemFlag;

		private readonly List<int> _PetAwakeLevel = new List<int>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "GUID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong GUID
		{
			get
			{
				return this._GUID;
			}
			set
			{
				this._GUID = value;
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

		[ProtoMember(3, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Level
		{
			get
			{
				return this._Level;
			}
			set
			{
				this._Level = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "VipLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int VipLevel
		{
			get
			{
				return this._VipLevel;
			}
			set
			{
				this._VipLevel = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "ConstellationLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ConstellationLevel
		{
			get
			{
				return this._ConstellationLevel;
			}
			set
			{
				this._ConstellationLevel = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "FashionID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FashionID
		{
			get
			{
				return this._FashionID;
			}
			set
			{
				this._FashionID = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Gender", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Gender
		{
			get
			{
				return this._Gender;
			}
			set
			{
				this._Gender = value;
			}
		}

		[ProtoMember(8, Name = "PetInfoID", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetInfoID
		{
			get
			{
				return this._PetInfoID;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "GuildName", DataFormat = DataFormat.Default), DefaultValue("")]
		public string GuildName
		{
			get
			{
				return this._GuildName;
			}
			set
			{
				this._GuildName = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "CombatValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CombatValue
		{
			get
			{
				return this._CombatValue;
			}
			set
			{
				this._CombatValue = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "FurtherLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FurtherLevel
		{
			get
			{
				return this._FurtherLevel;
			}
			set
			{
				this._FurtherLevel = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "AwakeLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AwakeLevel
		{
			get
			{
				return this._AwakeLevel;
			}
			set
			{
				this._AwakeLevel = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "AwakeItemFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AwakeItemFlag
		{
			get
			{
				return this._AwakeItemFlag;
			}
			set
			{
				this._AwakeItemFlag = value;
			}
		}

		[ProtoMember(14, Name = "PetAwakeLevel", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetAwakeLevel
		{
			get
			{
				return this._PetAwakeLevel;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
