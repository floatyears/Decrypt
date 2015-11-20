using System;
using UnityEngine;

public class MyDeedGrid : UICustomGrid
{
	private UnityEngine.Object MyDeedItemPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private MyDeedItem AddOneTragetItem()
	{
		if (this.MyDeedItemPrefab == null)
		{
			this.MyDeedItemPrefab = Res.LoadGUI("GUI/MyDeedItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.MyDeedItemPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		MyDeedItem myDeedItem = gameObject.AddComponent<MyDeedItem>();
		myDeedItem.Init();
		gameObject.AddComponent<UIDragScrollView>();
		return myDeedItem;
	}
}
