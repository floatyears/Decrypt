using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIPVPResultScene : GameUISession
{
	private GameObject backMenu;

	private UISprite result;

	private GameObject time;

	private GameObject gold;

	private GameObject honor;

	private GameObject exp;

	private GameObject line1;

	private UILabel timeLabel;

	private UILabel goldLabel;

	private UILabel honorLabel;

	private UISprite honorHot;

	private UILabel expLabel;

	private GUILevelExpUpAnimation levelAnimation;

	private GameObject Failure;

	private GameObject Victory;

	private static bool ArenaResultWin;

	private static MS2C_PvpArenaResult ArenaResultData;

	public static void ShowArenaResult(bool win, MS2C_PvpArenaResult data)
	{
		GUIPVPResultScene.ArenaResultWin = win;
		GUIPVPResultScene.ArenaResultData = data;
		GameUIManager.mInstance.ChangeSession<GUIPVPResultScene>(null, false, false);
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		if (GUIPVPResultScene.ArenaResultWin)
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
		base.StartCoroutine(this.ShowContentAnimation(GUIPVPResultScene.ArenaResultWin, GUIPVPResultScene.ArenaResultData));
	}

	protected override void OnPreDestroyGUI()
	{
		GUIPVPResultScene.ArenaResultData = null;
		base.StopAllCoroutines();
		Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
	}

	private void CreateObjects()
	{
		this.backMenu = base.RegisterClickEvent("backMenu", new UIEventListener.VoidDelegate(this.OnReturnClick), base.gameObject);
		this.backMenu.gameObject.SetActive(false);
		this.result = base.transform.Find("result").GetComponent<UISprite>();
		this.result.gameObject.SetActive(true);
		this.result.spriteName = ((!GUIPVPResultScene.ArenaResultWin) ? "Failure" : "Victory_Txt");
		GameObject gameObject = GameUITools.FindGameObject("infoBg", base.gameObject);
		this.time = gameObject.transform.FindChild("time").gameObject;
		this.time.SetActive(false);
		this.gold = gameObject.transform.FindChild("gold").gameObject;
		this.gold.SetActive(false);
		this.honor = gameObject.transform.FindChild("honor").gameObject;
		this.honor.SetActive(false);
		this.exp = gameObject.transform.FindChild("exp").gameObject;
		this.exp.SetActive(false);
		this.line1 = gameObject.transform.FindChild("line1").gameObject;
		this.line1.SetActive(false);
		this.timeLabel = this.time.transform.Find("num").GetComponent<UILabel>();
		this.goldLabel = this.gold.transform.Find("num").GetComponent<UILabel>();
		this.honorLabel = this.honor.transform.Find("num").GetComponent<UILabel>();
		this.honorHot = this.honor.transform.Find("Hot").GetComponent<UISprite>();
		this.honorHot.gameObject.SetActive(false);
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
	}

	[DebuggerHidden]
	private IEnumerator ShowContentAnimation(bool win, MS2C_PvpArenaResult data)
	{
        return null;
        //GUIPVPResultScene.<ShowContentAnimation>c__Iterator82 <ShowContentAnimation>c__Iterator = new GUIPVPResultScene.<ShowContentAnimation>c__Iterator82();
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
		GUIPVP4ReadyScene.TryOpen();
	}
}
