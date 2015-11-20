using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActorCombatLog")]
	[Serializable]
	public class ActorCombatLog : IExtensible
	{
		private int _PetID;

		private int _HPPct;

		private readonly List<SkillLog> _Data = new List<SkillLog>();

		private long _DamageTaken;

		private int _DamageTakenCount;

		private long _HealTaken;

		private int _HealTakenCount;

		private long _Damage;

		private long _Heal;

		private int _SocketSlot;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "PetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PetID
		{
			get
			{
				return this._PetID;
			}
			set
			{
				this._PetID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "HPPct", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HPPct
		{
			get
			{
				return this._HPPct;
			}
			set
			{
				this._HPPct = value;
			}
		}

		[ProtoMember(3, Name = "Data", DataFormat = DataFormat.Default)]
		public List<SkillLog> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "DamageTaken", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long DamageTaken
		{
			get
			{
				return this._DamageTaken;
			}
			set
			{
				this._DamageTaken = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "DamageTakenCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DamageTakenCount
		{
			get
			{
				return this._DamageTakenCount;
			}
			set
			{
				this._DamageTakenCount = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "HealTaken", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long HealTaken
		{
			get
			{
				return this._HealTaken;
			}
			set
			{
				this._HealTaken = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "HealTakenCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HealTakenCount
		{
			get
			{
				return this._HealTakenCount;
			}
			set
			{
				this._HealTakenCount = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "Damage", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long Damage
		{
			get
			{
				return this._Damage;
			}
			set
			{
				this._Damage = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Heal", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long Heal
		{
			get
			{
				return this._Heal;
			}
			set
			{
				this._Heal = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "SocketSlot", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SocketSlot
		{
			get
			{
				return this._SocketSlot;
			}
			set
			{
				this._SocketSlot = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
