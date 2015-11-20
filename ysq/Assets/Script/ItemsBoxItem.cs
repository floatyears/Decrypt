using System;

public class ItemsBoxItem : UICustomGridItem
{
	private ItemsBox mBaseBox;

	private ItemsBoxItemData mData;

	private CommonIconItem mIconItem;

	public void InitWithBaseScene(ItemsBox basebox)
	{
		this.mBaseBox = basebox;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIconItem = base.gameObject.GetComponent<CommonIconItem>();
		this.mIconItem.SetNameStyle(4, UILabel.Overflow.ResizeHeight);
		this.mIconItem.ShowItemInfo = true;
	}

	public override void Refresh(object data)
	{
		if (this.mData == data)
		{
			return;
		}
		this.mData = (ItemsBoxItemData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mData == null)
		{
			return;
		}
		this.mIconItem.Refresh(this.mData.data, true, this.mBaseBox.ShowNum, false);
	}
}
