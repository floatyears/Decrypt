using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "PetData")]
	[Serializable]
	public class PetData : IExtensible
	{
		private ulong _ID;

		private int _InfoID;

		private uint _Exp;

		private uint _Level;

		private uint _Further;

		private uint _SkillLevel;

		private uint _Awake;

		private uint _ItemFlag;

		private int _CultivateCount;

		private int _Attack;

		private int _PhysicDefense;

		private int _MagicDefense;

		private int _MaxHP;

		private int _AttackPreview;

		private int _PhysicDefensePreview;

		private int _MagicDefensePreview;

		private int _MaxHPPreview;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong ID
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

		[ProtoMember(3, IsRequired = false, Name = "Exp", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Exp
		{
			get
			{
				return this._Exp;
			}
			set
			{
				this._Exp = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(5, IsRequired = false, Name = "Further", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Further
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

		[ProtoMember(6, IsRequired = false, Name = "SkillLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SkillLevel
		{
			get
			{
				return this._SkillLevel;
			}
			set
			{
				this._SkillLevel = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Awake", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint Awake
		{
			get
			{
				return this._Awake;
			}
			set
			{
				this._Awake = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "ItemFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ItemFlag
		{
			get
			{
				return this._ItemFlag;
			}
			set
			{
				this._ItemFlag = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "CultivateCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CultivateCount
		{
			get
			{
				return this._CultivateCount;
			}
			set
			{
				this._CultivateCount = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "Attack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(11, IsRequired = false, Name = "PhysicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(12, IsRequired = false, Name = "MagicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(13, IsRequired = false, Name = "MaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(14, IsRequired = false, Name = "AttackPreview", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AttackPreview
		{
			get
			{
				return this._AttackPreview;
			}
			set
			{
				this._AttackPreview = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "PhysicDefensePreview", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PhysicDefensePreview
		{
			get
			{
				return this._PhysicDefensePreview;
			}
			set
			{
				this._PhysicDefensePreview = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "MagicDefensePreview", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MagicDefensePreview
		{
			get
			{
				return this._MagicDefensePreview;
			}
			set
			{
				this._MagicDefensePreview = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "MaxHPPreview", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxHPPreview
		{
			get
			{
				return this._MaxHPPreview;
			}
			set
			{
				this._MaxHPPreview = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
