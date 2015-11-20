using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_GuildWarHold")]
	[Serializable]
	public class MC2S_GuildWarHold : IExtensible
	{
		private EGuildWarId _WarID;

		private EGuildWarTeamId _TeamID = EGuildWarTeamId.EGWTI_None;

		private int _StrongholdID;

		private int _SlotIndex;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "WarID", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarId.EGWI_None)]
		public EGuildWarId WarID
		{
			get
			{
				return this._WarID;
			}
			set
			{
				this._WarID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "TeamID", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarTeamId.EGWTI_None)]
		public EGuildWarTeamId TeamID
		{
			get
			{
				return this._TeamID;
			}
			set
			{
				this._TeamID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "StrongholdID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int StrongholdID
		{
			get
			{
				return this._StrongholdID;
			}
			set
			{
				this._StrongholdID = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "SlotIndex", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SlotIndex
		{
			get
			{
				return this._SlotIndex;
			}
			set
			{
				this._SlotIndex = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
