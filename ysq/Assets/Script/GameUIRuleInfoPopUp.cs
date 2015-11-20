using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GameUIRuleInfoPopUp : GameUIBasePopup
{
	private UILabel mTitle;

	private UILabel mInfo;

	private UITable mContentTable;

	private UIScrollBar mScrollBar;

	public static void ShowThis(string title, string contents)
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUIRuleInfoPopUp, false, null, null);
		GameUIRuleInfoPopUp gameUIRuleInfoPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUIRuleInfoPopUp;
		gameUIRuleInfoPopUp.Refresh(Singleton<StringManager>.Instance.GetString(title), Singleton<StringManager>.Instance.GetString(contents));
	}

	public static void Show(string title, string contents)
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUIRuleInfoPopUp, false, null, null);
		GameUIRuleInfoPopUp gameUIRuleInfoPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUIRuleInfoPopUp;
		gameUIRuleInfoPopUp.Refresh(title, contents);
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("winBG").gameObject;
		this.mTitle = gameObject.transform.Find("title").GetComponent<UILabel>();
		this.mInfo = gameObject.transform.Find("contentsPanel/contents/info").GetComponent<UILabel>();
		this.mInfo.spaceIsNewLine = false;
		GameObject gameObject2 = gameObject.transform.Find("closeBtn").gameObject;
		UIEventListener expr_74 = UIEventListener.Get(gameObject2);
		expr_74.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_74.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mContentTable = gameObject.transform.Find("contentsPanel/contents").gameObject.AddComponent<UITable>();
		this.mContentTable.columns = 1;
		this.mContentTable.direction = UITable.Direction.Down;
		this.mContentTable.sorting = UITable.Sorting.None;
		this.mContentTable.hideInactive = true;
		this.mContentTable.keepWithinPanel = true;
		this.mContentTable.padding = new Vector2(0f, 2f);
		this.mScrollBar = gameObject.transform.Find("contentsScrollBar").GetComponent<UIScrollBar>();
	}

	private void OnCloseBtnClick(GameObject go)
	{
		base.OnButtonBlockerClick();
	}

	[DebuggerHidden]
	private IEnumerator UpdateScrollBar()
	{
        return null;
        //GameUIRuleInfoPopUp.<UpdateScrollBar>c__Iterator8F <UpdateScrollBar>c__Iterator8F = new GameUIRuleInfoPopUp.<UpdateScrollBar>c__Iterator8F();
        //<UpdateScrollBar>c__Iterator8F.<>f__this = this;
        //return <UpdateScrollBar>c__Iterator8F;
	}

	public void Refresh(string title, string desc)
	{
		if (!string.IsNullOrEmpty(title))
		{
			this.mTitle.text = title;
		}
		if (!string.IsNullOrEmpty(desc))
		{
			this.mInfo.text = desc;
		}
        base.StartCoroutine(this.UpdateScrollBar());
	}
}
