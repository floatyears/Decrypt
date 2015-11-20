using Att;
using System;

public class PetViewItemData : BaseData
{
	public PetInfo info;

	public PetViewItemData(PetInfo info)
	{
		this.info = info;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.info.ID);
	}
}
