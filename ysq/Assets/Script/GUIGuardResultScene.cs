using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIGuardResultScene : GameUISession
{
	private static MS2C_PveResult PveResultData;

	private GameObject backMenu;

	private UISprite result;

	private GameObject time;

	private GameObject gold;

	private UILabel timeLb;

	private UILabel goldLb;

	private GameObject failure;

	private GameObject victory;

	private GameObject rewardItem;

	public static void ShowResult(MS2C_PveResult data)
	{
		GUIGuardResultScene.PveResultData = data;
		GameUIManager.mInstance.ChangeSession<GUIGuardResultScene>(null, false, false);
	}

	protected override void OnPostLoadGUI()
	{
		this.backMenu = base.RegisterClickEvent("backMenu", new UIEventListener.VoidDelegate(this.OnReturnClick), base.gameObject);
		this.backMenu.SetActive(false);
		this.result = GameUITools.FindUISprite("result", base.gameObject);
		this.result.gameObject.SetActive(true);
		this.result.spriteName = ((GUIGuardResultScene.PveResultData == null) ? "Failure" : "Victory_Txt");
		this.failure = GameUITools.FindGameObject("Failure", base.gameObject);
		this.failure.AddComponent<GUIFailureTipsGroup>();
		this.failure.SetActive(false);
		this.victory = GameUITools.FindGameObject("Victory", base.gameObject);
		this.victory.SetActive(false);
		GameObject gameObject = GameUITools.FindGameObject("infoBg", this.victory);
		this.time = gameObject.transform.FindChild("Time").gameObject;
		this.time.SetActive(false);
		this.gold = gameObject.transform.FindChild("Gold").gameObject;
		this.gold.SetActive(false);
		this.timeLb = this.time.GetComponent<UILabel>();
		this.goldLb = this.gold.transform.Find("num").GetComponent<UILabel>();
		this.rewardItem = GameUITools.FindGameObject("Reward", this.victory);
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		if (GUIGuardResultScene.PveResultData != null)
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
		base.StartCoroutine(this.ShowContentAnimation(GUIGuardResultScene.PveResultData));
	}

	protected override void OnPreDestroyGUI()
	{
		GUIGuardResultScene.PveResultData = null;
		base.StopAllCoroutines();
		Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
	}

	[DebuggerHidden]
	private IEnumerator ShowContentAnimation(MS2C_PveResult data)
	{
        return null;
        //GUIGuardResultScene.<ShowContentAnimation>c__Iterator7C <ShowContentAnimation>c__Iterator7C = new GUIGuardResultScene.<ShowContentAnimation>c__Iterator7C();
        //<ShowContentAnimation>c__Iterator7C.data = data;
        //<ShowContentAnimation>c__Iterator7C.<$>data = data;
        //<ShowContentAnimation>c__Iterator7C.<>f__this = this;
        //return <ShowContentAnimation>c__Iterator7C;
	}

	private void OnReturnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIGuardScene.Show(false);
	}
}
