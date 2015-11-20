using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "SkillInfo")]
	[Serializable]
	public class SkillInfo : IExtensible
	{
		private int _ID;

		private string _Name = string.Empty;

		private string _Icon = string.Empty;

		private string _Desc = string.Empty;

		private int _Level;

		private int _ManaCost;

		private float _CoolDown;

		private int _CastTargetType;

		private float _MaxRange;

		private int _EffectTargetType;

		private float _Radius;

		private float _Angle;

		private float _Weight;

		private float _Length;

		private int _RadiateSpeed;

		private readonly List<int> _EffectType = new List<int>();

		private readonly List<int> _Rate = new List<int>();

		private readonly List<int> _Value1 = new List<int>();

		private readonly List<int> _Value2 = new List<int>();

		private readonly List<int> _Value3 = new List<int>();

		private readonly List<int> _Value4 = new List<int>();

		private string _CastAction = string.Empty;

		private readonly List<string> _HitAction = new List<string>();

		private float _ThreatBase;

		private float _ThreatCoef;

		private int _CastType;

		private bool _AlwaysHit;

		private int _CastRate;

		private int _CombatValue;

		private int _ComboSkillID;

		private float _SayTime;

		private int _SayID;

		private bool _TriggerDoubleDamage;

		private int _EPValue;

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

		[ProtoMember(3, IsRequired = false, Name = "Icon", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(6, IsRequired = false, Name = "ManaCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ManaCost
		{
			get
			{
				return this._ManaCost;
			}
			set
			{
				this._ManaCost = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "CoolDown", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float CoolDown
		{
			get
			{
				return this._CoolDown;
			}
			set
			{
				this._CoolDown = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "CastTargetType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CastTargetType
		{
			get
			{
				return this._CastTargetType;
			}
			set
			{
				this._CastTargetType = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "MaxRange", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float MaxRange
		{
			get
			{
				return this._MaxRange;
			}
			set
			{
				this._MaxRange = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "EffectTargetType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int EffectTargetType
		{
			get
			{
				return this._EffectTargetType;
			}
			set
			{
				this._EffectTargetType = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Radius", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float Radius
		{
			get
			{
				return this._Radius;
			}
			set
			{
				this._Radius = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "Angle", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float Angle
		{
			get
			{
				return this._Angle;
			}
			set
			{
				this._Angle = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "Weight", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float Weight
		{
			get
			{
				return this._Weight;
			}
			set
			{
				this._Weight = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "Length", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float Length
		{
			get
			{
				return this._Length;
			}
			set
			{
				this._Length = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "RadiateSpeed", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RadiateSpeed
		{
			get
			{
				return this._RadiateSpeed;
			}
			set
			{
				this._RadiateSpeed = value;
			}
		}

		[ProtoMember(16, Name = "EffectType", DataFormat = DataFormat.TwosComplement)]
		public List<int> EffectType
		{
			get
			{
				return this._EffectType;
			}
		}

		[ProtoMember(17, Name = "Rate", DataFormat = DataFormat.TwosComplement)]
		public List<int> Rate
		{
			get
			{
				return this._Rate;
			}
		}

		[ProtoMember(18, Name = "Value1", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value1
		{
			get
			{
				return this._Value1;
			}
		}

		[ProtoMember(19, Name = "Value2", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value2
		{
			get
			{
				return this._Value2;
			}
		}

		[ProtoMember(20, Name = "Value3", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value3
		{
			get
			{
				return this._Value3;
			}
		}

		[ProtoMember(21, Name = "Value4", DataFormat = DataFormat.TwosComplement)]
		public List<int> Value4
		{
			get
			{
				return this._Value4;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "CastAction", DataFormat = DataFormat.Default), DefaultValue("")]
		public string CastAction
		{
			get
			{
				return this._CastAction;
			}
			set
			{
				this._CastAction = value;
			}
		}

		[ProtoMember(23, Name = "HitAction", DataFormat = DataFormat.Default)]
		public List<string> HitAction
		{
			get
			{
				return this._HitAction;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "ThreatBase", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float ThreatBase
		{
			get
			{
				return this._ThreatBase;
			}
			set
			{
				this._ThreatBase = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "ThreatCoef", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float ThreatCoef
		{
			get
			{
				return this._ThreatCoef;
			}
			set
			{
				this._ThreatCoef = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "CastType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CastType
		{
			get
			{
				return this._CastType;
			}
			set
			{
				this._CastType = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "AlwaysHit", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool AlwaysHit
		{
			get
			{
				return this._AlwaysHit;
			}
			set
			{
				this._AlwaysHit = value;
			}
		}

		[ProtoMember(28, IsRequired = false, Name = "CastRate", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CastRate
		{
			get
			{
				return this._CastRate;
			}
			set
			{
				this._CastRate = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "CombatValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(30, IsRequired = false, Name = "ComboSkillID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ComboSkillID
		{
			get
			{
				return this._ComboSkillID;
			}
			set
			{
				this._ComboSkillID = value;
			}
		}

		[ProtoMember(31, IsRequired = false, Name = "SayTime", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float SayTime
		{
			get
			{
				return this._SayTime;
			}
			set
			{
				this._SayTime = value;
			}
		}

		[ProtoMember(32, IsRequired = false, Name = "SayID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SayID
		{
			get
			{
				return this._SayID;
			}
			set
			{
				this._SayID = value;
			}
		}

		[ProtoMember(33, IsRequired = false, Name = "TriggerDoubleDamage", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool TriggerDoubleDamage
		{
			get
			{
				return this._TriggerDoubleDamage;
			}
			set
			{
				this._TriggerDoubleDamage = value;
			}
		}

		[ProtoMember(34, IsRequired = false, Name = "EPValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int EPValue
		{
			get
			{
				return this._EPValue;
			}
			set
			{
				this._EPValue = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
