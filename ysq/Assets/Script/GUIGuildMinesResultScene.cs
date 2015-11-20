using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIGuildMinesResultScene : GameUISession
{
	private static bool MinesResultWin;

	private static MS2C_OrePillageResult MinesResultData;

	private GameObject backMenu;

	private UISprite result;

	private UISprite mBG;

	private GameObject mInfoBG;

	private GameObject time;

	private GameObject flowMines;

	private GameObject fixedMines;

	private UILabel timeLabel;

	private UILabel flowLabel;

	private UILabel fixedLabel;

	private GameObject failure;

	public static void Show(bool win, MS2C_OrePillageResult data)
	{
		GUIGuildMinesResultScene.MinesResultWin = win;
		GUIGuildMinesResultScene.MinesResultData = data;
		GameUIManager.mInstance.ChangeSession<GUIGuildMinesResultScene>(null, false, false);
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		if (GUIGuildMinesResultScene.MinesResultWin)
		{
			this.mBG.height = 228;
			this.mInfoBG.transform.localPosition = new Vector3(25f, -26f, 0f);
		}
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		if (GUIGuildMinesResultScene.MinesResultWin)
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
		base.StartCoroutine(this.ShowContentAnimation(GUIGuildMinesResultScene.MinesResultWin, GUIGuildMinesResultScene.MinesResultData));
	}

	protected override void OnPreDestroyGUI()
	{
		GUIGuildMinesResultScene.MinesResultData = null;
		base.StopAllCoroutines();
		Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
	}

	private void CreateObjects()
	{
		this.backMenu = base.RegisterClickEvent("backMenu", new UIEventListener.VoidDelegate(this.OnOKClick), base.gameObject);
		this.backMenu.SetActive(false);
		this.result = GameUITools.FindUISprite("result", base.gameObject);
		this.result.gameObject.SetActive(true);
		this.result.spriteName = ((!GUIGuildMinesResultScene.MinesResultWin) ? "Failure" : "Victory_Txt");
		this.mBG = GameUITools.FindUISprite("BG", base.gameObject);
		this.mInfoBG = GameUITools.FindGameObject("infoBg", base.gameObject);
		this.time = GameUITools.FindGameObject("time", this.mInfoBG);
		this.time.SetActive(false);
		this.flowMines = GameUITools.FindGameObject("flowMines", this.mInfoBG);
		this.flowMines.SetActive(false);
		this.fixedMines = GameUITools.FindGameObject("fixedMines", this.mInfoBG);
		this.fixedMines.SetActive(false);
		this.timeLabel = GameUITools.FindUILabel("num", this.time);
		this.flowLabel = GameUITools.FindUILabel("num", this.flowMines);
		this.fixedLabel = GameUITools.FindUILabel("num", this.fixedMines);
		this.failure = GameUITools.FindGameObject("Failure", base.gameObject);
		this.failure.AddComponent<GUIFailureTipsGroup>();
		this.failure.SetActive(false);
	}

	[DebuggerHidden]
	private IEnumerator ShowContentAnimation(bool win, MS2C_OrePillageResult data)
	{
        return null;
        //GUIGuildMinesResultScene.<ShowContentAnimation>c__Iterator5F <ShowContentAnimation>c__Iterator5F = new GUIGuildMinesResultScene.<ShowContentAnimation>c__Iterator5F();
        //<ShowContentAnimation>c__Iterator5F.win = win;
        //<ShowContentAnimation>c__Iterator5F.data = data;
        //<ShowContentAnimation>c__Iterator5F.<$>win = win;
        //<ShowContentAnimation>c__Iterator5F.<$>data = data;
        //<ShowContentAnimation>c__Iterator5F.<>f__this = this;
        //return <ShowContentAnimation>c__Iterator5F;
	}

	private void OnOKClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIGuildMinesScene.Show(false);
	}
}
