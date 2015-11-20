using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MailContentsUITable : UICustomGrid
{
	private GUIMailScene mBaseScene;

	public void InitWithBaseScene(GUIMailScene baseScene)
	{
		this.mBaseScene = baseScene;
	}

	public static bool IsReadMailFlag(MailData mailData)
	{
		return mailData.Read && mailData.AffixType.Count == 0;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/mailItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		MailItem mailItem = gameObject.AddComponent<MailItem>();
		mailItem.InitItem(this.mBaseScene);
		return mailItem;
	}

	public int SortMailItem(BaseData a, BaseData b)
	{
		MailItemData mailItemData = (MailItemData)a;
		MailItemData mailItemData2 = (MailItemData)b;
		if (mailItemData == null || mailItemData2 == null)
		{
			return 0;
		}
		if (MailContentsUITable.IsReadMailFlag(mailItemData.mMailData) && !MailContentsUITable.IsReadMailFlag(mailItemData2.mMailData))
		{
			return 1;
		}
		if (!MailContentsUITable.IsReadMailFlag(mailItemData.mMailData) && MailContentsUITable.IsReadMailFlag(mailItemData2.mMailData))
		{
			return -1;
		}
		return mailItemData2.mMailData.TimeStamp - mailItemData.mMailData.TimeStamp;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortMailItem));
	}
}
