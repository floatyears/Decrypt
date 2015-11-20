using Holoville.HOTween;
using System;
using UnityEngine;

public class CommonCombatMainBattleCD : MonoBehaviour
{
	private UILabel mCDTimeTxt;

	private UISprite mCDSprite;

	private float timerRefresh;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mCDSprite = base.transform.Find("cdSprite").GetComponent<UISprite>();
		this.mCDSprite.spriteName = "Countdown";
		if (HOTween.IsTweening(this.mCDSprite))
		{
			HOTween.Kill(this.mCDSprite);
		}
		this.mCDTimeTxt = base.transform.Find("cdTimeTxt").GetComponent<UILabel>();
		this.mCDTimeTxt.color = new Color(0.9843137f, 0.996078432f, 0.266666681f, 1f);
	}

	private void Update()
	{
		if (Time.time - this.timerRefresh > 1f)
		{
			this.timerRefresh = Time.time;
			this.Refresh();
		}
	}

	private void Refresh()
	{
		if (Globals.Instance.ActorMgr.CurScene == null)
		{
			return;
		}
		int num = (int)Globals.Instance.ActorMgr.CurScene.GetCombatTimer();
		this.mCDTimeTxt.text = UIEnergyTooltip.FormatTime2(num);
		if (num == 10)
		{
			this.mCDTimeTxt.color = Color.red;
			this.mCDSprite.spriteName = "Countdown_red";
			HOTween.To(this.mCDSprite, 0.5f, new TweenParms().Prop("color", new Color(this.mCDSprite.color.r, this.mCDSprite.color.g, this.mCDSprite.color.b, 0.5f)).Loops(-1, LoopType.Yoyo));
		}
	}
}
