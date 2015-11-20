using System;
using UnityEngine;

public class GUILopetTrainBaseInfo : MonoBehaviour
{
	private GUIPetTrainSceneV2 mBaseScene;

	private GUIAttributeValue mValues;

	private LopetInfoSkillLayer mSkills;

	private UISprite mLopetDescSp;

	private UILabel mDesc;

	private UITable mRightInfoTable;

	public void InitWithBaseScene(GUIPetTrainSceneV2 basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mRightInfoTable = base.transform.Find("contents").gameObject.AddComponent<UITable>();
		this.mRightInfoTable.columns = 1;
		this.mRightInfoTable.direction = UITable.Direction.Down;
		this.mRightInfoTable.sorting = UITable.Sorting.Alphabetic;
		this.mRightInfoTable.hideInactive = true;
		this.mRightInfoTable.keepWithinPanel = true;
		this.mRightInfoTable.padding = new Vector2(0f, 2f);
		this.mValues = GameUITools.FindGameObject("a", this.mRightInfoTable.gameObject).AddComponent<GUIAttributeValue>();
		this.mSkills = GameUITools.FindGameObject("b", this.mRightInfoTable.gameObject).AddComponent<LopetInfoSkillLayer>();
		this.mSkills.Init();
		this.mLopetDescSp = this.mRightInfoTable.transform.Find("e").GetComponent<UISprite>();
		this.mDesc = GameUITools.FindUILabel("desc", this.mLopetDescSp.gameObject);
	}

	public void Refresh()
	{
		LopetDataEx curLopetDataEx = this.mBaseScene.CurLopetDataEx;
		if (curLopetDataEx == null)
		{
			return;
		}
		this.mValues.Refresh(curLopetDataEx);
		this.mSkills.Refresh(curLopetDataEx);
		this.mDesc.text = curLopetDataEx.Info.Desc;
		this.mLopetDescSp.height = 50 + Mathf.RoundToInt(this.mDesc.printedSize.y);
		this.mRightInfoTable.repositionNow = true;
	}
}
