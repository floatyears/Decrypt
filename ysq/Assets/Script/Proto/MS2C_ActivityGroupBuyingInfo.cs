using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ActivityGroupBuyingInfo")]
	[Serializable]
	public class MS2C_ActivityGroupBuyingInfo : IExtensible
	{
		private int _Result;

		private readonly List<ActivityGroupBuyingCount> _BuyingData = new List<ActivityGroupBuyingCount>();

		private int _Score;

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

		[ProtoMember(2, Name = "BuyingData", DataFormat = DataFormat.Default)]
		public List<ActivityGroupBuyingCount> BuyingData
		{
			get
			{
				return this._BuyingData;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Score", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				this._Score = value;
			}
		}

		[ProtoMember(4, Name = "ScoreRewardID", DataFormat = DataFormat.TwosComplement)]
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
