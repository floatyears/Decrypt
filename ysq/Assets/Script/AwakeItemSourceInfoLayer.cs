using Att;
using System;
using UnityEngine;

public class AwakeItemSourceInfoLayer : MonoBehaviour
{
	private AwakeItemSourceUITable mContentsTable;

	private UILabel mTips;

	private UILabel mName;

	public void Init()
	{
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mContentsTable = GameUITools.FindGameObject("Panel/Content", base.gameObject).AddComponent<AwakeItemSourceUITable>();
		this.mContentsTable.maxPerLine = 1;
		this.mContentsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContentsTable.cellWidth = 370f;
		this.mContentsTable.cellHeight = 76f;
		this.mContentsTable.gapHeight = 6f;
		this.mContentsTable.gapWidth = 0f;
		this.mTips = GameUITools.FindUILabel("Panel/Tips", base.gameObject);
	}

	public void Refresh(ItemInfo info)
	{
		GUIHowGetPetItemPopUp.InitSourceItems(info, this.mContentsTable);
		this.mName.text = info.Name;
		if (this.mContentsTable.mDatas.Count > 0)
		{
			this.mTips.enabled = false;
		}
		else
		{
			this.mTips.enabled = true;
		}
	}
}
