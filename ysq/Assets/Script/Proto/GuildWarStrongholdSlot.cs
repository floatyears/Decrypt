using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarStrongholdSlot")]
	[Serializable]
	public class GuildWarStrongholdSlot : IExtensible
	{
		private EGuardWarStrongholdSlotState _Status = EGuardWarStrongholdSlotState.EGWPSS_Empty;

		private ulong _PlayerID;

		private ulong _AttackPlayerID;

		private int _CombatEndTimestamp;

		private int _ID;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Status", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuardWarStrongholdSlotState.EGWPSS_Empty)]
		public EGuardWarStrongholdSlotState Status
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

		[ProtoMember(2, IsRequired = false, Name = "PlayerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
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

		[ProtoMember(3, IsRequired = false, Name = "AttackPlayerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong AttackPlayerID
		{
			get
			{
				return this._AttackPlayerID;
			}
			set
			{
				this._AttackPlayerID = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "CombatEndTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CombatEndTimestamp
		{
			get
			{
				return this._CombatEndTimestamp;
			}
			set
			{
				this._CombatEndTimestamp = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
