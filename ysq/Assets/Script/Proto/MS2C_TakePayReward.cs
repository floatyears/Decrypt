using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_TakePayReward")]
	[Serializable]
	public class MS2C_TakePayReward : IExtensible
	{
		private int _Result;

		private int _ActivityID;

		private int _ProductID;

		private int _RewardCount;

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

		[ProtoMember(2, IsRequired = false, Name = "ActivityID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ActivityID
		{
			get
			{
				return this._ActivityID;
			}
			set
			{
				this._ActivityID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "ProductID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ProductID
		{
			get
			{
				return this._ProductID;
			}
			set
			{
				this._ProductID = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "RewardCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardCount
		{
			get
			{
				return this._RewardCount;
			}
			set
			{
				this._RewardCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
