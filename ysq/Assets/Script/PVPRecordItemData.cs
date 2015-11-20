using Proto;
using System;

public class PVPRecordItemData : BaseData
{
	public PvpRecord RecordData
	{
		get;
		private set;
	}

	public PVPRecordItemData(PvpRecord aInfo)
	{
		this.RecordData = aInfo;
	}

	public override ulong GetID()
	{
		return this.RecordData.PlayerID;
	}
}
