using Att;
using System;

public class RelationInfoData
{
	public RelationInfo mRelationInfo
	{
		get;
		private set;
	}

	public bool mIsActive
	{
		get;
		private set;
	}

	public RelationInfoData(RelationInfo rInfo, bool isActive)
	{
		this.mRelationInfo = rInfo;
		this.mIsActive = isActive;
	}
}
