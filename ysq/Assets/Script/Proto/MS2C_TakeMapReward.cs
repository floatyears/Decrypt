using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TakeMapReward")]
	[Serializable]
	public class MS2C_TakeMapReward : IExtensible
	{
		private int _Result;

		private int _RewardType;

		private int _Value1;

		private int _Value2;

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

		[ProtoMember(2, IsRequired = false, Name = "RewardType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardType
		{
			get
			{
				return this._RewardType;
			}
			set
			{
				this._RewardType = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Value1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value1
		{
			get
			{
				return this._Value1;
			}
			set
			{
				this._Value1 = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Value2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Value2
		{
			get
			{
				return this._Value2;
			}
			set
			{
				this._Value2 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
