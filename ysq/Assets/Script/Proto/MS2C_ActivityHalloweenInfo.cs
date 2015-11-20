using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ActivityHalloweenInfo")]
	[Serializable]
	public class MS2C_ActivityHalloweenInfo : IExtensible
	{
		private int _Result;

		private int _Score;

		private int _RewardTimestamp;

		private readonly List<int> _FreeContractIDs = new List<int>();

		private int _FireEndTimestamp;

		private int _PlayerScore;

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

		[ProtoMember(2, IsRequired = false, Name = "Score", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "RewardTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardTimestamp
		{
			get
			{
				return this._RewardTimestamp;
			}
			set
			{
				this._RewardTimestamp = value;
			}
		}

		[ProtoMember(4, Name = "FreeContractIDs", DataFormat = DataFormat.TwosComplement)]
		public List<int> FreeContractIDs
		{
			get
			{
				return this._FreeContractIDs;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "FireEndTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FireEndTimestamp
		{
			get
			{
				return this._FireEndTimestamp;
			}
			set
			{
				this._FireEndTimestamp = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "PlayerScore", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PlayerScore
		{
			get
			{
				return this._PlayerScore;
			}
			set
			{
				this._PlayerScore = value;
			}
		}

		[ProtoMember(7, Name = "ScoreRewardID", DataFormat = DataFormat.TwosComplement)]
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
