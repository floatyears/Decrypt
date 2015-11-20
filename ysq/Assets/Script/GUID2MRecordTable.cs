using System;
using UnityEngine;

public class GUID2MRecordTable : UICustomGrid
{
	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/D2MRecord");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUID2MRecordItem gUID2MRecordItem = gameObject.AddComponent<GUID2MRecordItem>();
		gUID2MRecordItem.InitWithBaseScene();
		return gUID2MRecordItem;
	}
}
