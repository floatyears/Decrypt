using System;

public class BillboardInfoData : BaseData
{
	private ulong ID;

	public object userData
	{
		get;
		private set;
	}

	public BillboardInfoData(object userData, int id)
	{
		this.userData = userData;
		this.ID = (ulong)((long)id);
	}

	public override ulong GetID()
	{
		return this.ID;
	}
}
