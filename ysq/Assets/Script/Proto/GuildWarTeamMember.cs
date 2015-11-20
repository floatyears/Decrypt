using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarTeamMember")]
	[Serializable]
	public class GuildWarTeamMember : IExtensible
	{
		private ulong _PlayerID;

		private EGuardWarTeamMemState _Status = EGuardWarTeamMemState.EGWTMS_Empty;

		private uint _KillNum;

		private uint _KilledNum;

		private int _Para1;

		private int _HealthPct;

		private int _RecoverTimes;

		private uint _ContinueKillNum;

		private int _StrongholdId;

		private int _KillerTimestamp;

		private int _SlotId;

		private int _Score;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "PlayerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong PlayerID
		{
			get
			{
				return this._PlayerID;
			}
			set
			{
				this._PlayerID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Status", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuardWarTeamMemState.EGWTMS_Empty)]
		public EGuardWarTeamMemState Status
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

		[ProtoMember(3, IsRequired = false, Name = "KillNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint KillNum
		{
			get
			{
				return this._KillNum;
			}
			set
			{
				this._KillNum = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "KilledNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint KilledNum
		{
			get
			{
				return this._KilledNum;
			}
			set
			{
				this._KilledNum = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Para1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Para1
		{
			get
			{
				return this._Para1;
			}
			set
			{
				this._Para1 = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "HealthPct", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int HealthPct
		{
			get
			{
				return this._HealthPct;
			}
			set
			{
				this._HealthPct = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "RecoverTimes", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RecoverTimes
		{
			get
			{
				return this._RecoverTimes;
			}
			set
			{
				this._RecoverTimes = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "ContinueKillNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint ContinueKillNum
		{
			get
			{
				return this._ContinueKillNum;
			}
			set
			{
				this._ContinueKillNum = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "StrongholdId", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StrongholdId
		{
			get
			{
				return this._StrongholdId;
			}
			set
			{
				this._StrongholdId = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "KillerTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int KillerTimestamp
		{
			get
			{
				return this._KillerTimestamp;
			}
			set
			{
				this._KillerTimestamp = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "SlotId", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SlotId
		{
			get
			{
				return this._SlotId;
			}
			set
			{
				this._SlotId = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "Score", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
