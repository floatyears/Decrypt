using Holoville.HOTween;
using System;
using UnityEngine;

public class CombatMainPvp4TopInfo : MonoBehaviour
{
	private GUICombatMain mBaseScene;

	private UILabel mSelfName;

	private UILabel mTargetName;

	private UILabel mCDTimeTxt;

	private UISprite mCDSprite;

	private float timerRefresh;

	private GameObject mCDGo;

	private GameObject mReplayTxtGo;

	public void InitWithBaseScene(GUICombatMain baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	public void RefreshUIState()
	{
		this.mCDGo.gameObject.SetActive(!this.mBaseScene.IsReplay);
		this.mReplayTxtGo.gameObject.SetActive(this.mBaseScene.IsReplay);
	}

	private void CreateObjects()
	{
		this.mReplayTxtGo = base.transform.Find("txt").gameObject;
		this.mSelfName = base.transform.Find("selfName").GetComponent<UILabel>();
		this.mTargetName = base.transform.Find("targetName").GetComponent<UILabel>();
		this.mCDGo = base.transform.Find("battleCD").gameObject;
		this.mCDSprite = this.mCDGo.transform.Find("cdSprite").GetComponent<UISprite>();
		this.mCDSprite.spriteName = "Countdown";
		if (HOTween.IsTweening(this.mCDSprite))
		{
			HOTween.Kill(this.mCDSprite);
		}
		this.mCDTimeTxt = this.mCDGo.transform.Find("cdTimeTxt").GetComponent<UILabel>();
		this.mCDTimeTxt.color = new Color(0.9843137f, 0.996078432f, 0.266666681f, 1f);
		this.mSelfName.text = Globals.Instance.Player.Data.Name;
		this.mTargetName.text = Globals.Instance.Player.TeamSystem.GetRemoteName();
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
