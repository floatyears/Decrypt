using System;
using UnityEngine;

public class GUIShiZhuangItemTable : UICustomGrid
{
	private GUIShiZhuangSceneV2 mBaseScene;

	public void InitWithBaseScene(GUIShiZhuangSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/shiZhuangItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUIShiZhuangItem gUIShiZhuangItem = gameObject.AddComponent<GUIShiZhuangItem>();
		gUIShiZhuangItem.InitWithBaseScene(this.mBaseScene, this);
		return gUIShiZhuangItem;
	}

	public ShiZhuangItemData GetCurSelectData()
	{
		ShiZhuangItemData shiZhuangItemData = null;
		for (int i = 0; i < this.mDatas.Count; i++)
		{
			ShiZhuangItemData shiZhuangItemData2 = (ShiZhuangItemData)this.mDatas[i];
			if (shiZhuangItemData2 != null && shiZhuangItemData2.mIsSelected)
			{
				shiZhuangItemData = shiZhuangItemData2;
				break;
			}
		}
		if (shiZhuangItemData == null)
		{
			this.SetCurSelectData(Globals.Instance.Player.GetCurFashionID());
			for (int j = 0; j < this.mDatas.Count; j++)
			{
				ShiZhuangItemData shiZhuangItemData3 = (ShiZhuangItemData)this.mDatas[j];
				if (shiZhuangItemData3 != null && shiZhuangItemData3.mIsSelected)
				{
					shiZhuangItemData = shiZhuangItemData3;
					break;
				}
			}
		}
		return shiZhuangItemData;
	}

	public void SetCurSelectData(int infoId)
	{
		if (infoId != 0)
		{
			for (int i = 0; i < this.mDatas.Count; i++)
			{
				ShiZhuangItemData shiZhuangItemData = (ShiZhuangItemData)this.mDatas[i];
				if (shiZhuangItemData != null && shiZhuangItemData.mFashionInfo.ID == infoId)
				{
					shiZhuangItemData.mIsSelected = true;
				}
				else
				{
					shiZhuangItemData.mIsSelected = false;
				}
			}
			for (int j = 0; j < base.transform.childCount; j++)
			{
				GUIShiZhuangItem component = base.transform.GetChild(j).GetComponent<GUIShiZhuangItem>();
				if (component != null)
				{
					component.Refresh();
				}
			}
		}
	}
}
