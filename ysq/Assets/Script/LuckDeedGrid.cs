using System;
using UnityEngine;

public class LuckDeedGrid : UICustomGrid
{
	private UnityEngine.Object MyDeedItemPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private LuckyDeedItem AddOneTragetItem()
	{
		if (this.MyDeedItemPrefab == null)
		{
			this.MyDeedItemPrefab = Res.LoadGUI("GUI/HallowmasLuckyDeedItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.MyDeedItemPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		LuckyDeedItem luckyDeedItem = gameObject.AddComponent<LuckyDeedItem>();
		luckyDeedItem.Init();
		gameObject.AddComponent<UIDragScrollView>();
		return luckyDeedItem;
	}
}
