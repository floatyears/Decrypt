using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActorData")]
	[Serializable]
	public class ActorData : IExtensible
	{
		private int _Slot;

		private int _InfoID;

		private int _Level;

		private int _Further;

		private int _MaxHP;

		private int _Attack;

		private int _PhysicDefense;

		private int _MagicDefense;

		private int _Hit;

		private int _Dodge;

		private int _Crit;

		private int _CritResis;

		private int _DamagePlus;

		private int _DamageMinus;

		private int _PlayerSkillID;

		private int _ManaCost;

		private float _CoolDown;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Slot", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Slot
		{
			get
			{
				return this._Slot;
			}
			set
			{
				this._Slot = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "InfoID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int InfoID
		{
			get
			{
				return this._InfoID;
			}
			set
			{
				this._InfoID = value;
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

		[ProtoMember(4, IsRequired = false, Name = "Further", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Further
		{
			get
			{
				return this._Further;
			}
			set
			{
				this._Further = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "MaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "Attack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "PhysicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(8, IsRequired = false, Name = "MagicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(9, IsRequired = false, Name = "Hit", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(10, IsRequired = false, Name = "Dodge", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(11, IsRequired = false, Name = "Crit", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(12, IsRequired = false, Name = "CritResis", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CritResis
		{
			get
			{
				return this._CritResis;
			}
			set
			{
				this._CritResis = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "DamagePlus", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(14, IsRequired = false, Name = "DamageMinus", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(15, IsRequired = false, Name = "PlayerSkillID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(16, IsRequired = false, Name = "ManaCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(17, IsRequired = false, Name = "CoolDown", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
