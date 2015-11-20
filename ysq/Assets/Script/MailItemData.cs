using Proto;
using System;

public class MailItemData : BaseData
{
	public MailData mMailData
	{
		get;
		private set;
	}

	public MailItemData(MailData aInfo)
	{
		this.mMailData = aInfo;
	}

	public override ulong GetID()
	{
		return (ulong)this.mMailData.MailID;
	}
}
