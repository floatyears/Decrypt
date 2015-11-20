using Proto;
using System;

public class GUIGuildMinesRecordData : BaseData
{
	public OrePillageRecord mRecord;

	public GUIGuildMinesRecordData(OrePillageRecord record)
	{
		this.mRecord = record;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.mRecord.Timestamp);
	}
}
