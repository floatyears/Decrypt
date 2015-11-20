using Proto;
using System;

public class PillageFarmItemData : BaseData
{
	public MS2C_PvpPillageResult mData;

	private ulong index;

	public PillageFarmItemData(int index, MS2C_PvpPillageResult data)
	{
		this.index = (ulong)((long)index);
		this.mData = data;
	}

	public override ulong GetID()
	{
		return this.index;
	}
}
