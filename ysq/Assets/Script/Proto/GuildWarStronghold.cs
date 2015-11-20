using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarStronghold")]
	[Serializable]
	public class GuildWarStronghold : IExtensible
	{
		private int _ID;

		private EGuildWarStrongholdState _Status = EGuildWarStrongholdState.EGWPS_Neutrality;

		private EGuildWarTeamId _OwnerId = EGuildWarTeamId.EGWTI_None;

		private readonly List<GuildWarStrongholdSlot> _Slots = new List<GuildWarStrongholdSlot>();

		private int _Para1;

		private int _Para2;

		private int _DefenceNum;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(2, IsRequired = false, Name = "Status", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarStrongholdState.EGWPS_Neutrality)]
		public EGuildWarStrongholdState Status
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

		[ProtoMember(3, IsRequired = false, Name = "OwnerId", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildWarTeamId.EGWTI_None)]
		public EGuildWarTeamId OwnerId
		{
			get
			{
				return this._OwnerId;
			}
			set
			{
				this._OwnerId = value;
			}
		}

		[ProtoMember(4, Name = "Slots", DataFormat = DataFormat.Default)]
		public List<GuildWarStrongholdSlot> Slots
		{
			get
			{
				return this._Slots;
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

		[ProtoMember(6, IsRequired = false, Name = "Para2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Para2
		{
			get
			{
				return this._Para2;
			}
			set
			{
				this._Para2 = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "DefenceNum", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DefenceNum
		{
			get
			{
				return this._DefenceNum;
			}
			set
			{
				this._DefenceNum = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
