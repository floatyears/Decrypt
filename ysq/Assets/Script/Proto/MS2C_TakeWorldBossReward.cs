using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TakeWorldBossReward")]
	[Serializable]
	public class MS2C_TakeWorldBossReward : IExtensible
	{
		private int _Result;

		private int _FireDragonScale;

		private int _Diamond;

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

		[ProtoMember(3, IsRequired = false, Name = "Diamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Diamond
		{
			get
			{
				return this._Diamond;
			}
			set
			{
				this._Diamond = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
