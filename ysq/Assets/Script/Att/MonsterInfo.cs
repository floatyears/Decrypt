using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "MonsterInfo")]
	[Serializable]
	public class MonsterInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _ResLoc = string.Empty;

		private int _Type;

		private int _BossType;

		private readonly List<int> _SkillID = new List<int>();

		private int _MaxHP;

		private int _Attack;

		private int _PhysicDefense;

		private int _MagicDefense;

		private int _Hit;

		private int _Dodge;

		private int _Crit;

		private int _CritResist;

		private int _StunResist;

		private int _RootResist;

		private int _FearResist;

		private int _HitBackResist;

		private int _HitDownResist;

		private int _SilenceResist;

		private int _DamagePlus;

		private int _DamageMinus;

		private int _ElementType;

		private float _Speed;

		private float _AttackDistance;

		private float _FindEnemyDistance;

		private string _HitSound = string.Empty;

		private string _DeadSound = string.Empty;

		private float _ScaleInUI;

		private uint _Level;

		private uint _LootMoney;

		private string _Icon = string.Empty;

		private string _Desc = string.Empty;

		private int _Quality;

		private readonly List<float> _SkillInitCD = new List<float>();

		private string _AIScript = string.Empty;

		private bool _OutLine;

		private bool _SkillToPlayer;

		private int _HPBarCount;

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

		[ProtoMember(4, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "BossType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BossType
		{
			get
			{
				return this._BossType;
			}
			set
			{
				this._BossType = value;
			}
		}

		[ProtoMember(6, Name = "SkillID", DataFormat = DataFormat.TwosComplement)]
		public List<int> SkillID
		{
			get
			{
				return this._SkillID;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "MaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(8, IsRequired = false, Name = "Attack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(9, IsRequired = false, Name = "PhysicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(10, IsRequired = false, Name = "MagicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(11, IsRequired = false, Name = "Hit", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(12, IsRequired = false, Name = "Dodge", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(13, IsRequired = false, Name = "Crit", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(14, IsRequired = false, Name = "CritResist", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(15, IsRequired = false, Name = "StunResist", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StunResist
		{
			get
			{
				return this._StunResist;
			}
			set
			{
				this._StunResist = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "RootResist", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RootResist
		{
			get
			{
				return this._RootResist;
			}
			set
			{
				this._RootResist = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "FearResist", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FearResist
		{
			get
			{
				return this._FearResist;
			}
			set
			{
				this._FearResist = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "HitBackResist", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HitBackResist
		{
			get
			{
				return this._HitBackResist;
			}
			set
			{
				this._HitBackResist = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "HitDownResist", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HitDownResist
		{
			get
			{
				return this._HitDownResist;
			}
			set
			{
				this._HitDownResist = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "SilenceResist", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SilenceResist
		{
			get
			{
				return this._SilenceResist;
			}
			set
			{
				this._SilenceResist = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "DamagePlus", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DamagePlus
		{
			get
			{
				return this._DamagePlus;
			}
			set
			{
				this._DamagePlus = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "DamageMinus", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DamageMinus
		{
			get
			{
				return this._DamageMinus;
			}
			set
			{
				this._DamageMinus = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "ElementType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(24, IsRequired = false, Name = "Speed", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
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

		[ProtoMember(25, IsRequired = false, Name = "AttackDistance", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
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

		[ProtoMember(26, IsRequired = false, Name = "FindEnemyDistance", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float FindEnemyDistance
		{
			get
			{
				return this._FindEnemyDistance;
			}
			set
			{
				this._FindEnemyDistance = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "HitSound", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(28, IsRequired = false, Name = "DeadSound", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(29, IsRequired = false, Name = "ScaleInUI", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
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

		[ProtoMember(30, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Level
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

		[ProtoMember(31, IsRequired = false, Name = "LootMoney", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint LootMoney
		{
			get
			{
				return this._LootMoney;
			}
			set
			{
				this._LootMoney = value;
			}
		}

		[ProtoMember(32, IsRequired = false, Name = "Icon", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(33, IsRequired = false, Name = "Desc", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(34, IsRequired = false, Name = "Quality", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(35, Name = "SkillInitCD", DataFormat = DataFormat.FixedSize)]
		public List<float> SkillInitCD
		{
			get
			{
				return this._SkillInitCD;
			}
		}

		[ProtoMember(36, IsRequired = false, Name = "AIScript", DataFormat = DataFormat.Default), DefaultValue("")]
		public string AIScript
		{
			get
			{
				return this._AIScript;
			}
			set
			{
				this._AIScript = value;
			}
		}

		[ProtoMember(37, IsRequired = false, Name = "OutLine", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool OutLine
		{
			get
			{
				return this._OutLine;
			}
			set
			{
				this._OutLine = value;
			}
		}

		[ProtoMember(38, IsRequired = false, Name = "SkillToPlayer", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool SkillToPlayer
		{
			get
			{
				return this._SkillToPlayer;
			}
			set
			{
				this._SkillToPlayer = value;
			}
		}

		[ProtoMember(39, IsRequired = false, Name = "HPBarCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HPBarCount
		{
			get
			{
				return this._HPBarCount;
			}
			set
			{
				this._HPBarCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
