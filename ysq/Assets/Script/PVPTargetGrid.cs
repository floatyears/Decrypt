using System;
using UnityEngine;

public class PVPTargetGrid : UICustomGrid
{
	private UnityEngine.Object PvpTargetItemPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddOneTragetItem();
	}

	private GUIPVP4TargetItem AddOneTragetItem()
	{
		if (this.PvpTargetItemPrefab == null)
		{
			this.PvpTargetItemPrefab = Res.LoadGUI("GUI/PvpTargetItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.PvpTargetItemPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUIPVP4TargetItem gUIPVP4TargetItem = gameObject.AddComponent<GUIPVP4TargetItem>();
		gUIPVP4TargetItem.Init();
		gameObject.AddComponent<UIDragScrollView>();
		return gUIPVP4TargetItem;
	}
}
