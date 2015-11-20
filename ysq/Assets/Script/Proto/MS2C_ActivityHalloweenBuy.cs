using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ActivityHalloweenBuy")]
	[Serializable]
	public class MS2C_ActivityHalloweenBuy : IExtensible
	{
		private int _Result;

		private int _ID;

		private int _Score;

		private int _Number;

		private int _FireEndTimestamp;

		private int _Diamond;

		private int _PlayerScore;

		private readonly List<RewardData> _Rewards = new List<RewardData>();

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

		[ProtoMember(2, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
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

		[ProtoMember(4, IsRequired = false, Name = "Number", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Number
		{
			get
			{
				return this._Number;
			}
			set
			{
				this._Number = value;
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

		[ProtoMember(6, IsRequired = false, Name = "Diamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "PlayerScore", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(8, Name = "Rewards", DataFormat = DataFormat.Default)]
		public List<RewardData> Rewards
		{
			get
			{
				return this._Rewards;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
