using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "CombatLog")]
	[Serializable]
	public class CombatLog : IExtensible
	{
		private readonly List<ActorCombatLog> _Data = new List<ActorCombatLog>();

		private int _StartTime;

		private int _EndTime;

		private int _PauseCount;

		private int _PauseTime;

		private long _HighestDamage;

		private long _HighestHeal;

		private int _KillMonsterCount;

		private int _Win;

		private int _RecvStartTime;

		private int _SendResultTime;

		private int _BossInfoID;

		private int _Attack;

		private int _PhysicDefense;

		private int _MagicDefense;

		private long _MaxHP;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Data", DataFormat = DataFormat.Default)]
		public List<ActorCombatLog> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "StartTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StartTime
		{
			get
			{
				return this._StartTime;
			}
			set
			{
				this._StartTime = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "EndTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int EndTime
		{
			get
			{
				return this._EndTime;
			}
			set
			{
				this._EndTime = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "PauseCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PauseCount
		{
			get
			{
				return this._PauseCount;
			}
			set
			{
				this._PauseCount = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "PauseTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PauseTime
		{
			get
			{
				return this._PauseTime;
			}
			set
			{
				this._PauseTime = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "HighestDamage", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long HighestDamage
		{
			get
			{
				return this._HighestDamage;
			}
			set
			{
				this._HighestDamage = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "HighestHeal", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long HighestHeal
		{
			get
			{
				return this._HighestHeal;
			}
			set
			{
				this._HighestHeal = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "KillMonsterCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int KillMonsterCount
		{
			get
			{
				return this._KillMonsterCount;
			}
			set
			{
				this._KillMonsterCount = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "Win", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Win
		{
			get
			{
				return this._Win;
			}
			set
			{
				this._Win = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "RecvStartTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RecvStartTime
		{
			get
			{
				return this._RecvStartTime;
			}
			set
			{
				this._RecvStartTime = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "SendResultTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SendResultTime
		{
			get
			{
				return this._SendResultTime;
			}
			set
			{
				this._SendResultTime = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "BossInfoID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int BossInfoID
		{
			get
			{
				return this._BossInfoID;
			}
			set
			{
				this._BossInfoID = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "Attack", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(15, IsRequired = false, Name = "PhysicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(16, IsRequired = false, Name = "MagicDefense", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(17, IsRequired = false, Name = "MaxHP", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long MaxHP
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
