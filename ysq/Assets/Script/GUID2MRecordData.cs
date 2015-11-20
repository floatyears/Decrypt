using Proto;
using System;

public class GUID2MRecordData : BaseData
{
	public D2MData mData
	{
		get;
		private set;
	}

	public GUID2MRecordData(D2MData da)
	{
		this.mData = da;
	}

	public override ulong GetID()
	{
		return (ulong)((long)this.mData.Money);
	}
}
