using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "LopetInfo")]
	[Serializable]
	public class LopetInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _ResLoc = string.Empty;

		private string _Icon = string.Empty;

		private string _Desc = string.Empty;

		private int _Quality;

		private float _ScaleInUI;

		private float _OffsetYInUI;

		private int _PlayerSkillID;

		private int _MaxHP;

		private int _MaxHPInc;

		private readonly List<int> _AwakeMaxHP = new List<int>();

		private int _Attack;

		private int _AttackInc;

		private readonly List<int> _AwakeAttack = new List<int>();

		private int _PhysicDefense;

		private int _PhysicDefenseInc;

		private readonly List<int> _AwakePhysicDefense = new List<int>();

		private int _MagicDefense;

		private int _MagicDefenseInc;

		private readonly List<int> _AwakeMagicDefense = new List<int>();

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

		[ProtoMember(4, IsRequired = false, Name = "Icon", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(5, IsRequired = false, Name = "Desc", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(6, IsRequired = false, Name = "Quality", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "ScaleInUI", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
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

		[ProtoMember(8, IsRequired = false, Name = "OffsetYInUI", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float OffsetYInUI
		{
			get
			{
				return this._OffsetYInUI;
			}
			set
			{
				this._OffsetYInUI = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "PlayerSkillID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(10, IsRequired = false, Name = "MaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(11, IsRequired = false, Name = "MaxHPInc", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(12, Name = "AwakeMaxHP", DataFormat = DataFormat.TwosComplement)]
		public List<int> AwakeMaxHP
		{
			get
			{
				return this._AwakeMaxHP;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "Attack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(14, IsRequired = false, Name = "AttackInc", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(15, Name = "AwakeAttack", DataFormat = DataFormat.TwosComplement)]
		public List<int> AwakeAttack
		{
			get
			{
				return this._AwakeAttack;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "PhysicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(17, IsRequired = false, Name = "PhysicDefenseInc", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(18, Name = "AwakePhysicDefense", DataFormat = DataFormat.TwosComplement)]
		public List<int> AwakePhysicDefense
		{
			get
			{
				return this._AwakePhysicDefense;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "MagicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(20, IsRequired = false, Name = "MagicDefenseInc", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(21, Name = "AwakeMagicDefense", DataFormat = DataFormat.TwosComplement)]
		public List<int> AwakeMagicDefense
		{
			get
			{
				return this._AwakeMagicDefense;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
