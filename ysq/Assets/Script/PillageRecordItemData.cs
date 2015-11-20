using Proto;
using System;

public class PillageRecordItemData : BaseData
{
	public PillageRecord RecordData
	{
		get;
		private set;
	}

	public PillageRecordItemData(PillageRecord data)
	{
		this.RecordData = data;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.GetHashCode());
	}
}
