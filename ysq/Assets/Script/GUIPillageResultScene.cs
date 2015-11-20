using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIPillageResultScene : GameUISession
{
	private GameObject backMenu;

	private UISprite result;

	private GameObject time;

	private GameObject gold;

	private GameObject exp;

	private UILabel timeLabel;

	private UILabel goldLabel;

	private UILabel expLabel;

	private GUILevelExpUpAnimation levelAnimation;

	private GameObject Failure;

	private GameObject Victory;

	private UILabel VictoryTips;

	private GameObject RewardItem;

	private static bool PillageResultWin;

	private static MS2C_PvpPillageResult PillageResultData;

	public static void ShowPillageResult(bool win, MS2C_PvpPillageResult data)
	{
		GUIPillageResultScene.PillageResultWin = win;
		GUIPillageResultScene.PillageResultData = data;
		GameUIManager.mInstance.ChangeSession<GUIPillageResultScene>(null, false, false);
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		if (GUIPillageResultScene.PillageResultWin)
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
		base.StartCoroutine(this.ShowContentAnimation(GUIPillageResultScene.PillageResultWin, GUIPillageResultScene.PillageResultData));
	}

	protected override void OnPreDestroyGUI()
	{
		GUIPillageResultScene.PillageResultData = null;
		base.StopAllCoroutines();
		Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
	}

	private void CreateObjects()
	{
		this.backMenu = base.RegisterClickEvent("backMenu", new UIEventListener.VoidDelegate(this.OnReturnClick), base.gameObject);
		this.backMenu.gameObject.SetActive(false);
		this.result = base.transform.Find("result").GetComponent<UISprite>();
		this.result.gameObject.SetActive(true);
		this.result.spriteName = ((!GUIPillageResultScene.PillageResultWin) ? "Failure" : "Victory_Txt");
		GameObject gameObject = GameUITools.FindGameObject("infoBg", base.gameObject);
		this.time = gameObject.transform.FindChild("time").gameObject;
		this.time.SetActive(false);
		this.gold = gameObject.transform.FindChild("gold").gameObject;
		this.gold.SetActive(false);
		this.exp = gameObject.transform.FindChild("exp").gameObject;
		this.exp.SetActive(false);
		this.timeLabel = this.time.transform.Find("num").GetComponent<UILabel>();
		this.goldLabel = this.gold.transform.Find("num").GetComponent<UILabel>();
		this.expLabel = this.exp.transform.Find("num").GetComponent<UILabel>();
		GameObject gameObject2 = gameObject.transform.FindChild("Level").gameObject;
		this.levelAnimation = gameObject2.AddComponent<GUILevelExpUpAnimation>();
		gameObject2.SetActive(false);
		this.levelAnimation.Init();
		this.Failure = GameUITools.FindGameObject("Failure", base.gameObject);
		this.Failure.AddComponent<GUIFailureTipsGroup>();
		this.Failure.SetActive(false);
		this.Victory = GameUITools.FindGameObject("Victory", base.gameObject);
		this.Victory.SetActive(false);
		this.VictoryTips = this.Victory.transform.Find("Tips").GetComponent<UILabel>();
		this.RewardItem = GameUITools.FindGameObject("Reward", base.gameObject);
		this.RewardItem.SetActive(false);
	}

	[DebuggerHidden]
	private IEnumerator ShowContentAnimation(bool win, MS2C_PvpPillageResult data)
	{
        return null;
        //GUIPillageResultScene.<ShowContentAnimation>c__Iterator85 <ShowContentAnimation>c__Iterator = new GUIPillageResultScene.<ShowContentAnimation>c__Iterator85();
        //<ShowContentAnimation>c__Iterator.data = data;
        //<ShowContentAnimation>c__Iterator.win = win;
        //<ShowContentAnimation>c__Iterator.<$>data = data;
        //<ShowContentAnimation>c__Iterator.<$>win = win;
        //<ShowContentAnimation>c__Iterator.<>f__this = this;
        //return <ShowContentAnimation>c__Iterator;
	}

	private void OnReturnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIPillageScene.TryOpen(false);
	}
}
