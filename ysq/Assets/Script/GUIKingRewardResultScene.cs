using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIKingRewardResultScene : GameUISession
{
	private GameObject backMenu;

	private UISprite result;

	private GameObject time;

	private GameObject exp;

	private UILabel timeLb;

	private UILabel expLabel;

	private GUILevelExpUpAnimation levelAnimation;

	private GameObject Failure;

	private GameObject Victory;

	private GameObject RewardItem;

	private static MS2C_PveResult PveResultData;

	public static void ShowKingRewardResult(MS2C_PveResult data)
	{
		GUIKingRewardResultScene.PveResultData = data;
		GameUIManager.mInstance.ChangeSession<GUIKingRewardResultScene>(null, false, false);
	}

	protected override void OnPostLoadGUI()
	{
		this.backMenu = base.RegisterClickEvent("backMenu", new UIEventListener.VoidDelegate(this.OnReturnClick), base.gameObject);
		this.backMenu.gameObject.SetActive(false);
		this.result = base.transform.Find("result").GetComponent<UISprite>();
		this.result.gameObject.SetActive(true);
		this.result.spriteName = ((GUIKingRewardResultScene.PveResultData == null) ? "Failure" : "Victory_Txt");
		this.Failure = GameUITools.FindGameObject("Failure", base.gameObject);
		this.Failure.AddComponent<GUIFailureTipsGroup>();
		this.Failure.SetActive(false);
		this.Victory = GameUITools.FindGameObject("Victory", base.gameObject);
		this.Victory.SetActive(false);
		GameObject gameObject = GameUITools.FindGameObject("infoBg", this.Victory);
		this.time = gameObject.transform.FindChild("Time").gameObject;
		this.time.SetActive(false);
		this.exp = gameObject.transform.FindChild("exp").gameObject;
		this.exp.SetActive(false);
		this.timeLb = this.time.GetComponent<UILabel>();
		this.expLabel = this.exp.transform.Find("num").GetComponent<UILabel>();
		GameObject gameObject2 = gameObject.transform.FindChild("Level").gameObject;
		this.levelAnimation = gameObject2.AddComponent<GUILevelExpUpAnimation>();
		this.levelAnimation.Init();
		this.RewardItem = GameUITools.FindGameObject("Reward", this.Victory);
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		if (GUIKingRewardResultScene.PveResultData != null)
		{
			Globals.Instance.BackgroundMusicMgr.StopWarmingSound();
			Globals.Instance.BackgroundMusicMgr.ClearGameBGM();
			Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
			Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("ui/ui_004b", false);
		}
		else
		{
			Globals.Instance.BackgroundMusicMgr.StopWarmingSound();
			Globals.Instance.BackgroundMusicMgr.ClearGameBGM();
			Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
			Globals.Instance.BackgroundMusicMgr.PlayGameOverSound();
		}
		base.StartCoroutine(this.ShowContentAnimation(GUIKingRewardResultScene.PveResultData));
	}

	protected override void OnPreDestroyGUI()
	{
		GUIKingRewardResultScene.PveResultData = null;
		base.StopAllCoroutines();
		Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
	}

	[DebuggerHidden]
	private IEnumerator ShowContentAnimation(MS2C_PveResult data)
	{
        return null;
        //GUIKingRewardResultScene.<ShowContentAnimation>c__Iterator4F <ShowContentAnimation>c__Iterator4F = new GUIKingRewardResultScene.<ShowContentAnimation>c__Iterator4F();
        //<ShowContentAnimation>c__Iterator4F.data = data;
        //<ShowContentAnimation>c__Iterator4F.<$>data = data;
        //<ShowContentAnimation>c__Iterator4F.<>f__this = this;
        //return <ShowContentAnimation>c__Iterator4F;
	}

	private void OnReturnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIKingRewardScene>(null, false, true);
	}
}
