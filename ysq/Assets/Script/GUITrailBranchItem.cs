using System;
using UnityEngine;

public class GUITrailBranchItem : UICustomGridItem
{
	private const int mLeafNum = 5;

	private GUITrailTowerSceneV2 mBaseScene;

	private GUITrailBranchLeafItem[] mLeafItems = new GUITrailBranchLeafItem[5];

	private GameObject mBottom;

	private UITexture mBgTt;

	private GUITrailBranchItemData mGUITrailBranchItemData;

	public void InitWithBaseScene(GUITrailTowerSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBgTt = base.transform.GetComponent<UITexture>();
		for (int i = 0; i < 5; i++)
		{
			this.mLeafItems[i] = base.transform.Find(string.Format("l{0}", i)).gameObject.AddComponent<GUITrailBranchLeafItem>();
			this.mLeafItems[i].InitWithBaseScene(this.mBaseScene, i);
		}
		this.mBottom = base.transform.Find("bottom").gameObject;
		this.mBottom.SetActive(false);
	}

	public override void Refresh(object data)
	{
		if (this.mGUITrailBranchItemData != data)
		{
			this.mGUITrailBranchItemData = (GUITrailBranchItemData)data;
			this.Refresh();
		}
	}

	public void Refresh()
	{
		if (this.mGUITrailBranchItemData != null)
		{
			for (int i = 0; i < 5; i++)
			{
				this.mLeafItems[i].Refresh(this.mGUITrailBranchItemData.StartIndex);
			}
			this.mBottom.SetActive(this.mGUITrailBranchItemData.StartIndex == 1);
			this.mBgTt.height = ((this.mGUITrailBranchItemData.StartIndex != this.mBaseScene.mMaxBranchIndex) ? 584 : 864);
		}
	}
}
