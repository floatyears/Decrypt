using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TakeGuildBossDamageReward")]
	[Serializable]
	public class MS2C_TakeGuildBossDamageReward : IExtensible
	{
		private int _Result;

		private long _Damage;

		private int _Reputation;

		private int _ExtraReputation;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				this._Result = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Damage", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(3, IsRequired = false, Name = "Reputation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Reputation
		{
			get
			{
				return this._Reputation;
			}
			set
			{
				this._Reputation = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "ExtraReputation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ExtraReputation
		{
			get
			{
				return this._ExtraReputation;
			}
			set
			{
				this._ExtraReputation = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
