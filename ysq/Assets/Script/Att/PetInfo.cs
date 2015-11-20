using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "PetInfo")]
	[Serializable]
	public class PetInfo : IExtensible
	{
		private int _ID;

		private string _FirstName = string.Empty;

		private string _Name = string.Empty;

		private string _ResLoc = string.Empty;

		private string _Icon = string.Empty;

		private string _Desc = string.Empty;

		private int _Type;

		private int _Quality;

		private int _SubQuality;

		private float _AttackDistance;

		private int _ElementType;

		private float _ScaleInUI;

		private float _ScaleInCard;

		private float _OffsetYInCard;

		private string _UIAction = string.Empty;

		private int _PlayerSkillID;

		private readonly List<int> _SkillID = new List<int>();

		private readonly List<int> _TalentID = new List<int>();

		private readonly List<int> _RelationID = new List<int>();

		private int _MaxHP;

		private int _MaxHPInc;

		private int _Attack;

		private int _AttackInc;

		private int _PhysicDefense;

		private int _PhysicDefenseInc;

		private int _MagicDefense;

		private int _MagicDefenseInc;

		private int _Hit;

		private int _Dodge;

		private int _Crit;

		private int _CritResist;

		private float _Speed;

		private string _HitSound = string.Empty;

		private string _DeadSound = string.Empty;

		private bool _ShowCollection;

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

		[ProtoMember(2, IsRequired = false, Name = "FirstName", DataFormat = DataFormat.Default), DefaultValue("")]
		public string FirstName
		{
			get
			{
				return this._FirstName;
			}
			set
			{
				this._FirstName = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(4, IsRequired = false, Name = "ResLoc", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(5, IsRequired = false, Name = "Icon", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Icon
		{
			get
			{
				return this._Icon;
			}
			set
			{
				this._Icon = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Desc", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(7, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				this._Type = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "Quality", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(9, IsRequired = false, Name = "SubQuality", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SubQuality
		{
			get
			{
				return this._SubQuality;
			}
			set
			{
				this._SubQuality = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "AttackDistance", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float AttackDistance
		{
			get
			{
				return this._AttackDistance;
			}
			set
			{
				this._AttackDistance = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "ElementType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ElementType
		{
			get
			{
				return this._ElementType;
			}
			set
			{
				this._ElementType = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "ScaleInUI", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float ScaleInUI
		{
			get
			{
				return this._ScaleInUI;
			}
			set
			{
				this._ScaleInUI = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "ScaleInCard", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float ScaleInCard
		{
			get
			{
				return this._ScaleInCard;
			}
			set
			{
				this._ScaleInCard = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "OffsetYInCard", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float OffsetYInCard
		{
			get
			{
				return this._OffsetYInCard;
			}
			set
			{
				this._OffsetYInCard = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "UIAction", DataFormat = DataFormat.Default), DefaultValue("")]
		public string UIAction
		{
			get
			{
				return this._UIAction;
			}
			set
			{
				this._UIAction = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "PlayerSkillID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PlayerSkillID
		{
			get
			{
				return this._PlayerSkillID;
			}
			set
			{
				this._PlayerSkillID = value;
			}
		}

		[ProtoMember(18, Name = "SkillID", DataFormat = DataFormat.TwosComplement)]
		public List<int> SkillID
		{
			get
			{
				return this._SkillID;
			}
		}

		[ProtoMember(19, Name = "TalentID", DataFormat = DataFormat.TwosComplement)]
		public List<int> TalentID
		{
			get
			{
				return this._TalentID;
			}
		}

		[ProtoMember(20, Name = "RelationID", DataFormat = DataFormat.TwosComplement)]
		public List<int> RelationID
		{
			get
			{
				return this._RelationID;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "MaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxHP
		{
			get
			{
				return this._MaxHP;
			}
			set
			{
				this._MaxHP = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "MaxHPInc", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxHPInc
		{
			get
			{
				return this._MaxHPInc;
			}
			set
			{
				this._MaxHPInc = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "Attack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Attack
		{
			get
			{
				return this._Attack;
			}
			set
			{
				this._Attack = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "AttackInc", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AttackInc
		{
			get
			{
				return this._AttackInc;
			}
			set
			{
				this._AttackInc = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "PhysicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PhysicDefense
		{
			get
			{
				return this._PhysicDefense;
			}
			set
			{
				this._PhysicDefense = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "PhysicDefenseInc", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PhysicDefenseInc
		{
			get
			{
				return this._PhysicDefenseInc;
			}
			set
			{
				this._PhysicDefenseInc = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "MagicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicDefense
		{
			get
			{
				return this._MagicDefense;
			}
			set
			{
				this._MagicDefense = value;
			}
		}

		[ProtoMember(28, IsRequired = false, Name = "MagicDefenseInc", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicDefenseInc
		{
			get
			{
				return this._MagicDefenseInc;
			}
			set
			{
				this._MagicDefenseInc = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "Hit", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Hit
		{
			get
			{
				return this._Hit;
			}
			set
			{
				this._Hit = value;
			}
		}

		[ProtoMember(30, IsRequired = false, Name = "Dodge", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Dodge
		{
			get
			{
				return this._Dodge;
			}
			set
			{
				this._Dodge = value;
			}
		}

		[ProtoMember(31, IsRequired = false, Name = "Crit", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Crit
		{
			get
			{
				return this._Crit;
			}
			set
			{
				this._Crit = value;
			}
		}

		[ProtoMember(32, IsRequired = false, Name = "CritResist", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CritResist
		{
			get
			{
				return this._CritResist;
			}
			set
			{
				this._CritResist = value;
			}
		}

		[ProtoMember(33, IsRequired = false, Name = "Speed", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float Speed
		{
			get
			{
				return this._Speed;
			}
			set
			{
				this._Speed = value;
			}
		}

		[ProtoMember(34, IsRequired = false, Name = "HitSound", DataFormat = DataFormat.Default), DefaultValue("")]
		public string HitSound
		{
			get
			{
				return this._HitSound;
			}
			set
			{
				this._HitSound = value;
			}
		}

		[ProtoMember(35, IsRequired = false, Name = "DeadSound", DataFormat = DataFormat.Default), DefaultValue("")]
		public string DeadSound
		{
			get
			{
				return this._DeadSound;
			}
			set
			{
				this._DeadSound = value;
			}
		}

		[ProtoMember(36, IsRequired = false, Name = "ShowCollection", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool ShowCollection
		{
			get
			{
				return this._ShowCollection;
			}
			set
			{
				this._ShowCollection = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
