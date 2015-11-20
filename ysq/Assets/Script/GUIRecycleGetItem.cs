using System;

public class GUIRecycleGetItem : UICustomGridItem
{
	private RecycleGetItemData mData;

	private CommonIconItem mIconItem;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIconItem = base.gameObject.GetComponent<CommonIconItem>();
	}

	public override void Refresh(object data)
	{
		if (this.mData == data)
		{
			return;
		}
		this.mData = (RecycleGetItemData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mData == null)
		{
			return;
		}
		this.mIconItem.Refresh(this.mData.data, false, true, false);
	}
}
