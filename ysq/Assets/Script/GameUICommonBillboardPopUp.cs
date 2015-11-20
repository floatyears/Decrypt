using System;
using System.Collections.Generic;
using UnityEngine;

public class GameUICommonBillboardPopUp : GameUIBasePopup
{
	private UILabel mSelfRank;

	private UILabel mMayBeRank;

	private UILabel mScore;

	private UILabel mTitle;

	private UILabel mDesc;

	private CommonRankItemTable mItemsTable;

	public int specialNum
	{
		get;
		private set;
	}

	public void InitBillboard(string itemClassName)
	{
		Transform transform = base.transform.Find("WinBG");
		GameObject gameObject = transform.Find("CloseBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		this.mItemsTable = transform.Find("rankItems/itemsPanel/itemsContents").gameObject.AddComponent<CommonRankItemTable>();
		this.mItemsTable.maxPerLine = 1;
		this.mItemsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mItemsTable.cellWidth = 774f;
		this.mItemsTable.cellHeight = 87f;
		this.mItemsTable.scrollBar = transform.Find("rankItems/BgPanelScrollBar").GetComponent<UIScrollBar>();
		this.mItemsTable.className = itemClassName;
		this.mItemsTable.InitWithBaseScene(this);
		this.mSelfRank = transform.Find("selfRank").GetComponent<UILabel>();
		this.mMayBeRank = transform.Find("mayBeRank").GetComponent<UILabel>();
		this.mScore = transform.Find("scoreTxt").GetComponent<UILabel>();
		this.mTitle = transform.Find("TitleLabel").GetComponent<UILabel>();
		this.mDesc = transform.Find("tipTxt").GetComponent<UILabel>();
	}

	public void InitItems(List<object> datas, int num = 3, int startID = 0)
	{
		this.specialNum = num;
		for (int i = startID; i < datas.Count + startID; i++)
		{
			this.mItemsTable.AddData(new BillboardInfoData(datas[i - startID], i));
		}
		this.Refresh();
	}

	private void Refresh()
	{
		this.mItemsTable.repositionNow = true;
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	public void Refresh(string title, string desc, string selfRankTxt, string mayBeRankTxt, string scoreTxt)
	{
		this.mTitle.text = title;
		if (!string.IsNullOrEmpty(desc))
		{
			this.mDesc.text = desc;
			this.mDesc.gameObject.SetActive(true);
		}
		else
		{
			this.mDesc.gameObject.SetActive(false);
		}
		if (!string.IsNullOrEmpty(selfRankTxt))
		{
			this.mSelfRank.text = selfRankTxt;
			this.mSelfRank.gameObject.SetActive(true);
		}
		else
		{
			this.mSelfRank.gameObject.SetActive(false);
		}
		if (!string.IsNullOrEmpty(mayBeRankTxt))
		{
			this.mMayBeRank.text = Singleton<StringManager>.Instance.GetString("trailTowerTxt3", new object[]
			{
				mayBeRankTxt
			});
			this.mMayBeRank.gameObject.SetActive(true);
		}
		else
		{
			this.mMayBeRank.gameObject.SetActive(false);
		}
		if (!string.IsNullOrEmpty(scoreTxt))
		{
			this.mScore.text = scoreTxt;
			this.mScore.gameObject.SetActive(true);
		}
		else
		{
			this.mScore.gameObject.SetActive(false);
		}
	}
}
