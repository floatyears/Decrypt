using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "SkillLog")]
	[Serializable]
	public class SkillLog : IExtensible
	{
		private int _SkillID;

		private int _Count;

		private long _Damage;

		private long _Heal;

		private long _HighDamage;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "SkillID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SkillID
		{
			get
			{
				return this._SkillID;
			}
			set
			{
				this._SkillID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				this._Count = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Damage", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(4, IsRequired = false, Name = "Heal", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(5, IsRequired = false, Name = "HighDamage", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long HighDamage
		{
			get
			{
				return this._HighDamage;
			}
			set
			{
				this._HighDamage = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
