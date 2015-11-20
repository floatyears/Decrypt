using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "RobotInfo")]
	[Serializable]
	public class RobotInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private int _Gender;

		private int _FashionID;

		private int _Level;

		private int _ConstellationLevel;

		private int _FurtherLevel;

		private readonly List<int> _ItemID = new List<int>();

		private readonly List<int> _ItemEnhance = new List<int>();

		private readonly List<int> _ItemRefine = new List<int>();

		private readonly List<int> _PetInfoID = new List<int>();

		private readonly List<int> _PetLevel = new List<int>();

		private readonly List<int> _PetFurther = new List<int>();

		private readonly List<int> _PetSkill = new List<int>();

		private bool _IsWarFree;

		private int _Quality;

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

		[ProtoMember(3, IsRequired = false, Name = "Gender", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(4, IsRequired = false, Name = "FashionID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "ConstellationLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "FurtherLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(8, Name = "ItemID", DataFormat = DataFormat.TwosComplement)]
		public List<int> ItemID
		{
			get
			{
				return this._ItemID;
			}
		}

		[ProtoMember(9, Name = "ItemEnhance", DataFormat = DataFormat.TwosComplement)]
		public List<int> ItemEnhance
		{
			get
			{
				return this._ItemEnhance;
			}
		}

		[ProtoMember(10, Name = "ItemRefine", DataFormat = DataFormat.TwosComplement)]
		public List<int> ItemRefine
		{
			get
			{
				return this._ItemRefine;
			}
		}

		[ProtoMember(11, Name = "PetInfoID", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetInfoID
		{
			get
			{
				return this._PetInfoID;
			}
		}

		[ProtoMember(12, Name = "PetLevel", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetLevel
		{
			get
			{
				return this._PetLevel;
			}
		}

		[ProtoMember(13, Name = "PetFurther", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetFurther
		{
			get
			{
				return this._PetFurther;
			}
		}

		[ProtoMember(14, Name = "PetSkill", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetSkill
		{
			get
			{
				return this._PetSkill;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "IsWarFree", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool IsWarFree
		{
			get
			{
				return this._IsWarFree;
			}
			set
			{
				this._IsWarFree = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "Quality", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Quality
		{
			get
			{
				return this._Quality;
			}
			set
			{
				this._Quality = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
