using ProtoBuf;
using System;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "QualityInfo")]
	[Serializable]
	public class QualityInfo : IExtensible
	{
		private int _ID;

		private int _MaxHP;

		private int _MaxHPInc;

		private int _Attack;

		private int _AttackInc;

		private int _PhysicDefense;

		private int _PhysicDefenseInc;

		private int _MagicDefense;

		private int _MagicDefenseInc;

		private uint _PetValue;

		private uint _PetExp;

		private uint _EquipValue;

		private uint _EquipMoney;

		private uint _TrinketExp;

		private int _SceneCount;

		private int _PillageRate;

		private int _PillageCount;

		private int _AwakeCreateMoney;

		private int _AwakeBreakupValue;

		private int _SceneCount2;

		private uint _EquipValue2;

		private uint _LopetValue;

		private int _SceneCount3;

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

		[ProtoMember(2, IsRequired = false, Name = "MaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "MaxHPInc", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(4, IsRequired = false, Name = "Attack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "AttackInc", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "PhysicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "PhysicDefenseInc", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(9, IsRequired = false, Name = "MagicDefenseInc", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(10, IsRequired = false, Name = "PetValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint PetValue
		{
			get
			{
				return this._PetValue;
			}
			set
			{
				this._PetValue = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "PetExp", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint PetExp
		{
			get
			{
				return this._PetExp;
			}
			set
			{
				this._PetExp = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "EquipValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint EquipValue
		{
			get
			{
				return this._EquipValue;
			}
			set
			{
				this._EquipValue = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "EquipMoney", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint EquipMoney
		{
			get
			{
				return this._EquipMoney;
			}
			set
			{
				this._EquipMoney = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "TrinketExp", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint TrinketExp
		{
			get
			{
				return this._TrinketExp;
			}
			set
			{
				this._TrinketExp = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "SceneCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneCount
		{
			get
			{
				return this._SceneCount;
			}
			set
			{
				this._SceneCount = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "PillageRate", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PillageRate
		{
			get
			{
				return this._PillageRate;
			}
			set
			{
				this._PillageRate = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "PillageCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PillageCount
		{
			get
			{
				return this._PillageCount;
			}
			set
			{
				this._PillageCount = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "AwakeCreateMoney", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AwakeCreateMoney
		{
			get
			{
				return this._AwakeCreateMoney;
			}
			set
			{
				this._AwakeCreateMoney = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "AwakeBreakupValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int AwakeBreakupValue
		{
			get
			{
				return this._AwakeBreakupValue;
			}
			set
			{
				this._AwakeBreakupValue = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "SceneCount2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneCount2
		{
			get
			{
				return this._SceneCount2;
			}
			set
			{
				this._SceneCount2 = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "EquipValue2", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint EquipValue2
		{
			get
			{
				return this._EquipValue2;
			}
			set
			{
				this._EquipValue2 = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "LopetValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint LopetValue
		{
			get
			{
				return this._LopetValue;
			}
			set
			{
				this._LopetValue = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "SceneCount3", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneCount3
		{
			get
			{
				return this._SceneCount3;
			}
			set
			{
				this._SceneCount3 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
