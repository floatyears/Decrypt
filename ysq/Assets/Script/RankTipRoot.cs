using Holoville.HOTween.Core;
using System;
using UnityEngine;

public abstract class RankTipRoot : MonoBehaviour
{
	protected GameUICommonBillboardPopUp mBaseScene;

	protected CommonRankItemBase mRankBase;

	public virtual void InitWithBaseScene(GameUICommonBillboardPopUp baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	public virtual void ShowRankTip(CommonRankItemBase rankBase)
	{
		this.mRankBase = rankBase;
	}

	protected virtual void CreateObjects()
	{
		Transform transform = base.transform.Find("rankTip");
		UIEventListener expr_1C = UIEventListener.Get(transform.gameObject);
		expr_1C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C.onClick, new UIEventListener.VoidDelegate(this.OnTipBgClick));
	}

	private void OnTipAnimEnd()
	{
		base.gameObject.SetActive(false);
	}

	private void OnTipBgClick(GameObject go)
	{
		GameUITools.PlayCloseWindowAnim(base.transform, new TweenDelegate.TweenCallback(this.OnTipAnimEnd), true);
	}

	public abstract void Refresh();
}
