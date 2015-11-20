using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TakeWorldBossDamageReward")]
	[Serializable]
	public class MS2C_TakeWorldBossDamageReward : IExtensible
	{
		private int _Result;

		private int _FireDragonScale;

		private long _Damage;

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

		[ProtoMember(2, IsRequired = false, Name = "FireDragonScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FireDragonScale
		{
			get
			{
				return this._FireDragonScale;
			}
			set
			{
				this._FireDragonScale = value;
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
