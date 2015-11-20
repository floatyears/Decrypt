using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityHalloweenExt")]
	[Serializable]
	public class ActivityHalloweenExt : IExtensible
	{
		private int _FireScore;

		private int _DiamondToScore;

		private int _FireReturnMin;

		private int _FireReturnMax;

		private int _FirstRewardTime;

		private int _SecondRewardTime;

		private int _ThirdRewardTime;

		private readonly List<ActivityHalloweenItem> _Data = new List<ActivityHalloweenItem>();

		private readonly List<ActivityHalloweenScoreReward> _ScoreReward = new List<ActivityHalloweenScoreReward>();

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "FireScore", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FireScore
		{
			get
			{
				return this._FireScore;
			}
			set
			{
				this._FireScore = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "DiamondToScore", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DiamondToScore
		{
			get
			{
				return this._DiamondToScore;
			}
			set
			{
				this._DiamondToScore = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "FireReturnMin", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FireReturnMin
		{
			get
			{
				return this._FireReturnMin;
			}
			set
			{
				this._FireReturnMin = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "FireReturnMax", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FireReturnMax
		{
			get
			{
				return this._FireReturnMax;
			}
			set
			{
				this._FireReturnMax = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "FirstRewardTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FirstRewardTime
		{
			get
			{
				return this._FirstRewardTime;
			}
			set
			{
				this._FirstRewardTime = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "SecondRewardTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SecondRewardTime
		{
			get
			{
				return this._SecondRewardTime;
			}
			set
			{
				this._SecondRewardTime = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "ThirdRewardTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ThirdRewardTime
		{
			get
			{
				return this._ThirdRewardTime;
			}
			set
			{
				this._ThirdRewardTime = value;
			}
		}

		[ProtoMember(8, Name = "Data", DataFormat = DataFormat.Default)]
		public List<ActivityHalloweenItem> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(9, Name = "ScoreReward", DataFormat = DataFormat.Default)]
		public List<ActivityHalloweenScoreReward> ScoreReward
		{
			get
			{
				return this._ScoreReward;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
