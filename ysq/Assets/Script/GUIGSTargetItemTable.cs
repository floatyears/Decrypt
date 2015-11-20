using System;
using UnityEngine;

public class GUIGSTargetItemTable : UICustomGrid
{
	private GUIGuildSchoolTargetPopUp mBaseScene;

	public void InitWithBaseScene(GUIGuildSchoolTargetPopUp baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/guildSchoolTargetItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUIGSTargetItem gUIGSTargetItem = gameObject.AddComponent<GUIGSTargetItem>();
		gUIGSTargetItem.InitWithBaseScene(this.mBaseScene);
		return gUIGSTargetItem;
	}

	public int GetCurSelectedID()
	{
		int result = 0;
		for (int i = 0; i < this.mDatas.Count; i++)
		{
			GUIGSTargetItemData gUIGSTargetItemData = (GUIGSTargetItemData)this.mDatas[i];
			if (gUIGSTargetItemData != null && gUIGSTargetItemData.IsSelected)
			{
				result = gUIGSTargetItemData.SchoolId;
				break;
			}
		}
		return result;
	}

	public void SetCurSelectID(int id)
	{
		for (int i = 0; i < this.mDatas.Count; i++)
		{
			GUIGSTargetItemData gUIGSTargetItemData = (GUIGSTargetItemData)this.mDatas[i];
			if (gUIGSTargetItemData != null)
			{
				gUIGSTargetItemData.IsSelected = (gUIGSTargetItemData.SchoolId == id);
			}
		}
		this.Refresh();
	}

	private void Refresh()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			GUIGSTargetItem component = base.transform.GetChild(i).GetComponent<GUIGSTargetItem>();
			if (component != null)
			{
				component.Refresh();
			}
		}
	}
}
