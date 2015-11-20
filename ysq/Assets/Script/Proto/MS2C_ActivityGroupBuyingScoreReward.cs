using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ActivityGroupBuyingScoreReward")]
	[Serializable]
	public class MS2C_ActivityGroupBuyingScoreReward : IExtensible
	{
		private int _Result;

		private readonly List<int> _ScoreRewardID = new List<int>();

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

		[ProtoMember(2, Name = "ScoreRewardID", DataFormat = DataFormat.TwosComplement)]
		public List<int> ScoreRewardID
		{
			get
			{
				return this._ScoreRewardID;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
