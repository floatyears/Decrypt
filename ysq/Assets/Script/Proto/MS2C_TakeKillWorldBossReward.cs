using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TakeKillWorldBossReward")]
	[Serializable]
	public class MS2C_TakeKillWorldBossReward : IExtensible
	{
		private int _Result;

		private int _HasReward;

		private int _Slot;

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

		[ProtoMember(2, IsRequired = false, Name = "HasReward", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HasReward
		{
			get
			{
				return this._HasReward;
			}
			set
			{
				this._HasReward = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Slot", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
