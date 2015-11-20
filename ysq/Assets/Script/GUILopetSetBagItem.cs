using System;
using UnityEngine;

public class GUILopetSetBagItem : UICustomGridItem
{
	private CommonIconItem[] mIcons;

	public void Init()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIcons = new CommonIconItem[3];
	}

	public override void Refresh(object data)
	{
		this.Refresh();
	}

	private void Refresh()
	{
		int num = 0;
		while (num < this.mIcons.Length && num < 3)
		{
			if (this.mIcons[num] == null)
			{
				this.mIcons[num] = CommonIconItem.Create(base.gameObject, new Vector3((float)(16 + num * 72), -9f, 0f), null, true, 0.7f, null);
				this.mIcons[num].ShowItemInfo = true;
			}
			num++;
		}
	}
}
