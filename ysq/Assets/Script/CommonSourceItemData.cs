using Att;
using System;

public class CommonSourceItemData : BaseData
{
	public EItemSource mSourceType;

	public int mSceneID;

	public ItemInfo mItemInfo;

	public FashionInfo mFashionInfo;

	private ulong id;

	public CommonSourceItemData(EItemSource type, ItemInfo itemInfo, ulong index)
	{
		this.mSourceType = type;
		this.mItemInfo = itemInfo;
		this.id = index;
	}

	public CommonSourceItemData(EItemSource type, ulong index, FashionInfo fInfo = null)
	{
		if (type == EItemSource.EISource_SceneLoot)
		{
			Debug.LogError(new object[]
			{
				"SceneID is null"
			});
		}
		this.mSourceType = type;
		this.id = index;
		this.mFashionInfo = fInfo;
	}

	public CommonSourceItemData(int sceneID, ItemInfo itemInfo, ulong index)
	{
		this.mSourceType = EItemSource.EISource_SceneLoot;
		this.mSceneID = sceneID;
		this.mItemInfo = itemInfo;
		this.id = index;
	}

	public override ulong GetID()
	{
		return this.id;
	}
}
