using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GetBossData")]
	[Serializable]
	public class MS2C_GetBossData : IExtensible
	{
		private int _Status;

		private int _TimeStamp;

		private readonly List<BossData> _Data = new List<BossData>();

		private readonly List<RankData> _RData = new List<RankData>();

		private int _Rank;

		private long _TotalDamage;

		private int _TotalCount;

		private int _ChallengeCD;

		private int _KillElapsedTime;

		private int _DeadTimestamp;

		private int _HasReward;

		private int _Result;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Status", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				this._Status = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "TimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TimeStamp
		{
			get
			{
				return this._TimeStamp;
			}
			set
			{
				this._TimeStamp = value;
			}
		}

		[ProtoMember(3, Name = "Data", DataFormat = DataFormat.Default)]
		public List<BossData> Data
		{
			get
			{
				return this._Data;
			}
		}

		[ProtoMember(4, Name = "RData", DataFormat = DataFormat.Default)]
		public List<RankData> RData
		{
			get
			{
				return this._RData;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Rank", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Rank
		{
			get
			{
				return this._Rank;
			}
			set
			{
				this._Rank = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "TotalDamage", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public long TotalDamage
		{
			get
			{
				return this._TotalDamage;
			}
			set
			{
				this._TotalDamage = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "TotalCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TotalCount
		{
			get
			{
				return this._TotalCount;
			}
			set
			{
				this._TotalCount = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "ChallengeCD", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ChallengeCD
		{
			get
			{
				return this._ChallengeCD;
			}
			set
			{
				this._ChallengeCD = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "KillElapsedTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int KillElapsedTime
		{
			get
			{
				return this._KillElapsedTime;
			}
			set
			{
				this._KillElapsedTime = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "DeadTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DeadTimestamp
		{
			get
			{
				return this._DeadTimestamp;
			}
			set
			{
				this._DeadTimestamp = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "HasReward", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(14, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
