using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "TinyLevelInfo")]
	[Serializable]
	public class TinyLevelInfo : IExtensible
	{
		private int _ID;

		private readonly List<int> _MaxHPFurther = new List<int>();

		private readonly List<int> _AttackFurther = new List<int>();

		private readonly List<int> _PhysicDefenseFurther = new List<int>();

		private readonly List<int> _MagicDefenseFurther = new List<int>();

		private readonly List<int> _PetMaxHPFurther = new List<int>();

		private readonly List<int> _PetAttackFurther = new List<int>();

		private readonly List<int> _PetPhysicDefenseFurther = new List<int>();

		private readonly List<int> _PetMagicDefenseFurther = new List<int>();

		private uint _PFurtherItemCount;

		private uint _PFurtherCost;

		private readonly List<uint> _FurtherCost = new List<uint>();

		private readonly List<uint> _FurtherPetCount = new List<uint>();

		private readonly List<uint> _FurtherItemCount = new List<uint>();

		private readonly List<uint> _RefineCost = new List<uint>();

		private readonly List<uint> _RefineTrinketCount = new List<uint>();

		private readonly List<uint> _RefineItemCount = new List<uint>();

		private uint _SkillItemCount;

		private uint _SkillCost;

		private int _FashionMaxHP;

		private int _FashionAttack;

		private int _AssistMinLevel;

		private int _ALAttack;

		private int _ALDefense;

		private int _ALMaxHP;

		private int _AssistMinFurther;

		private int _AFAttack;

		private int _AFDefense;

		private int _AFMaxHP;

		private readonly List<uint> _LopetAwakeCost = new List<uint>();

		private readonly List<uint> _LopetAwakeCount = new List<uint>();

		private readonly List<uint> _LopetAwakeItemCount = new List<uint>();

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

		[ProtoMember(2, Name = "MaxHPFurther", DataFormat = DataFormat.TwosComplement)]
		public List<int> MaxHPFurther
		{
			get
			{
				return this._MaxHPFurther;
			}
		}

		[ProtoMember(3, Name = "AttackFurther", DataFormat = DataFormat.TwosComplement)]
		public List<int> AttackFurther
		{
			get
			{
				return this._AttackFurther;
			}
		}

		[ProtoMember(4, Name = "PhysicDefenseFurther", DataFormat = DataFormat.TwosComplement)]
		public List<int> PhysicDefenseFurther
		{
			get
			{
				return this._PhysicDefenseFurther;
			}
		}

		[ProtoMember(5, Name = "MagicDefenseFurther", DataFormat = DataFormat.TwosComplement)]
		public List<int> MagicDefenseFurther
		{
			get
			{
				return this._MagicDefenseFurther;
			}
		}

		[ProtoMember(6, Name = "PetMaxHPFurther", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetMaxHPFurther
		{
			get
			{
				return this._PetMaxHPFurther;
			}
		}

		[ProtoMember(7, Name = "PetAttackFurther", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetAttackFurther
		{
			get
			{
				return this._PetAttackFurther;
			}
		}

		[ProtoMember(8, Name = "PetPhysicDefenseFurther", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetPhysicDefenseFurther
		{
			get
			{
				return this._PetPhysicDefenseFurther;
			}
		}

		[ProtoMember(9, Name = "PetMagicDefenseFurther", DataFormat = DataFormat.TwosComplement)]
		public List<int> PetMagicDefenseFurther
		{
			get
			{
				return this._PetMagicDefenseFurther;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "PFurtherItemCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint PFurtherItemCount
		{
			get
			{
				return this._PFurtherItemCount;
			}
			set
			{
				this._PFurtherItemCount = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "PFurtherCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint PFurtherCost
		{
			get
			{
				return this._PFurtherCost;
			}
			set
			{
				this._PFurtherCost = value;
			}
		}

		[ProtoMember(12, Name = "FurtherCost", DataFormat = DataFormat.TwosComplement)]
		public List<uint> FurtherCost
		{
			get
			{
				return this._FurtherCost;
			}
		}

		[ProtoMember(13, Name = "FurtherPetCount", DataFormat = DataFormat.TwosComplement)]
		public List<uint> FurtherPetCount
		{
			get
			{
				return this._FurtherPetCount;
			}
		}

		[ProtoMember(14, Name = "FurtherItemCount", DataFormat = DataFormat.TwosComplement)]
		public List<uint> FurtherItemCount
		{
			get
			{
				return this._FurtherItemCount;
			}
		}

		[ProtoMember(15, Name = "RefineCost", DataFormat = DataFormat.TwosComplement)]
		public List<uint> RefineCost
		{
			get
			{
				return this._RefineCost;
			}
		}

		[ProtoMember(16, Name = "RefineTrinketCount", DataFormat = DataFormat.TwosComplement)]
		public List<uint> RefineTrinketCount
		{
			get
			{
				return this._RefineTrinketCount;
			}
		}

		[ProtoMember(17, Name = "RefineItemCount", DataFormat = DataFormat.TwosComplement)]
		public List<uint> RefineItemCount
		{
			get
			{
				return this._RefineItemCount;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "SkillItemCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SkillItemCount
		{
			get
			{
				return this._SkillItemCount;
			}
			set
			{
				this._SkillItemCount = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "SkillCost", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SkillCost
		{
			get
			{
				return this._SkillCost;
			}
			set
			{
				this._SkillCost = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "FashionMaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FashionMaxHP
		{
			get
			{
				return this._FashionMaxHP;
			}
			set
			{
				this._FashionMaxHP = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "FashionAttack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FashionAttack
		{
			get
			{
				return this._FashionAttack;
			}
			set
			{
				this._FashionAttack = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "AssistMinLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AssistMinLevel
		{
			get
			{
				return this._AssistMinLevel;
			}
			set
			{
				this._AssistMinLevel = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "ALAttack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ALAttack
		{
			get
			{
				return this._ALAttack;
			}
			set
			{
				this._ALAttack = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "ALDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ALDefense
		{
			get
			{
				return this._ALDefense;
			}
			set
			{
				this._ALDefense = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "ALMaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ALMaxHP
		{
			get
			{
				return this._ALMaxHP;
			}
			set
			{
				this._ALMaxHP = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "AssistMinFurther", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AssistMinFurther
		{
			get
			{
				return this._AssistMinFurther;
			}
			set
			{
				this._AssistMinFurther = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "AFAttack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AFAttack
		{
			get
			{
				return this._AFAttack;
			}
			set
			{
				this._AFAttack = value;
			}
		}

		[ProtoMember(28, IsRequired = false, Name = "AFDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AFDefense
		{
			get
			{
				return this._AFDefense;
			}
			set
			{
				this._AFDefense = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "AFMaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AFMaxHP
		{
			get
			{
				return this._AFMaxHP;
			}
			set
			{
				this._AFMaxHP = value;
			}
		}

		[ProtoMember(30, Name = "LopetAwakeCost", DataFormat = DataFormat.TwosComplement)]
		public List<uint> LopetAwakeCost
		{
			get
			{
				return this._LopetAwakeCost;
			}
		}

		[ProtoMember(31, Name = "LopetAwakeCount", DataFormat = DataFormat.TwosComplement)]
		public List<uint> LopetAwakeCount
		{
			get
			{
				return this._LopetAwakeCount;
			}
		}

		[ProtoMember(32, Name = "LopetAwakeItemCount", DataFormat = DataFormat.TwosComplement)]
		public List<uint> LopetAwakeItemCount
		{
			get
			{
				return this._LopetAwakeItemCount;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
