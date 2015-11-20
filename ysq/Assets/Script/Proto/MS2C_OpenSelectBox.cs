using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_OpenSelectBox")]
	[Serializable]
	public class MS2C_OpenSelectBox : IExtensible
	{
		private int _Result;

		private int _RewardType;

		private int _RewardValue1;

		private int _RewardValue2;

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

		[ProtoMember(3, IsRequired = false, Name = "RewardValue1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardValue1
		{
			get
			{
				return this._RewardValue1;
			}
			set
			{
				this._RewardValue1 = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "RewardValue2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardValue2
		{
			get
			{
				return this._RewardValue2;
			}
			set
			{
				this._RewardValue2 = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
