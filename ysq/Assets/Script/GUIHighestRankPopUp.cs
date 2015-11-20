using Proto;
using System;
using UnityEngine;

public class GUIHighestRankPopUp : GameUIBasePopup
{
	private UILabel mHighestRank;

	private UILabel mCurRank;

	private UILabel mUpdateRank;

	private UILabel mDiamond;

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("BG").gameObject;
		this.mHighestRank = gameObject.transform.Find("highestRank/num").GetComponent<UILabel>();
		this.mCurRank = gameObject.transform.Find("curRank/num").GetComponent<UILabel>();
		this.mUpdateRank = gameObject.transform.Find("curRank/arrow/num").GetComponent<UILabel>();
		this.mDiamond = gameObject.transform.Find("reward/num").GetComponent<UILabel>();
		GameObject gameObject2 = gameObject.transform.Find("sureBtn").gameObject;
		UIEventListener expr_9E = UIEventListener.Get(gameObject2);
		expr_9E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_9E.onClick, new UIEventListener.VoidDelegate(this.OnSureBtnClick));
	}

	public override void InitPopUp(MS2C_PvpArenaResult resultData)
	{
		GameUIState uiState = GameUIManager.mInstance.uiState;
		this.mHighestRank.text = uiState.ArenaHighestRank.ToString();
		this.mCurRank.text = resultData.HighestRank.ToString();
		int num = uiState.ArenaHighestRank - resultData.HighestRank;
		if (num > 0)
		{
			this.mUpdateRank.text = num.ToString();
		}
		else
		{
			this.mUpdateRank.text = resultData.UpdateRank.ToString();
		}
		this.mDiamond.text = resultData.Diamond.ToString();
	}

	private void OnSureBtnClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
