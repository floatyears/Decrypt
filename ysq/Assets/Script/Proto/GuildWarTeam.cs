using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarTeam")]
	[Serializable]
	public class GuildWarTeam : IExtensible
	{
		private ulong _GuildID;

		private readonly List<GuildWarTeamMember> _Members = new List<GuildWarTeamMember>();

		private int _Score;

		private uint _KillNum;

		private uint _KilledNum;

		private readonly List<GuildWarSupporter> _Supporters = new List<GuildWarSupporter>();

		private int _SupportDiamond;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "GuildID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong GuildID
		{
			get
			{
				return this._GuildID;
			}
			set
			{
				this._GuildID = value;
			}
		}

		[ProtoMember(2, Name = "Members", DataFormat = DataFormat.Default)]
		public List<GuildWarTeamMember> Members
		{
			get
			{
				return this._Members;
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

		[ProtoMember(4, IsRequired = false, Name = "KillNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(5, IsRequired = false, Name = "KilledNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
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

		[ProtoMember(6, Name = "Supporters", DataFormat = DataFormat.Default)]
		public List<GuildWarSupporter> Supporters
		{
			get
			{
				return this._Supporters;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "SupportDiamond", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SupportDiamond
		{
			get
			{
				return this._SupportDiamond;
			}
			set
			{
				this._SupportDiamond = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
