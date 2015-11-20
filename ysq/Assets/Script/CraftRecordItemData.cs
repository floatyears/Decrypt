using Proto;
using System;

public class CraftRecordItemData : BaseData
{
	public GuildWarBattleRecord RecordData
	{
		get;
		private set;
	}

	public CraftRecordItemData(GuildWarBattleRecord ai)
	{
		this.RecordData = ai;
	}

	public override ulong GetID()
	{
		return (this.RecordData == null) ? 0uL : this.RecordData.PlayerID;
	}
}
