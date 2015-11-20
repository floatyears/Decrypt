using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIGuildCraftResultScene : GameUISession
{
	private UISprite mTitleSp;

	private GameObject mBackBtn;

	private GameObject mTimeGo;

	private GameObject mTxt0Go;

	private GameObject mLineGo;

	private GameObject mJiShaGo;

	private GameObject mJiShaScoreGo;

	private UILabel mTimeNum;

	private UILabel mJiShaNum;

	private UILabel mJiShaScore;

	private static bool ArenaResultWin;

	private static MS2C_GuildWarFightEnd ArenaResultData;

	public static void ShowMe(bool win, MS2C_GuildWarFightEnd data)
	{
		GUIGuildCraftResultScene.ArenaResultWin = win;
		GUIGuildCraftResultScene.ArenaResultData = data;
		GameUIManager.mInstance.ChangeSession<GUIGuildCraftResultScene>(null, false, false);
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		if (GUIGuildCraftResultScene.ArenaResultWin)
		{
			this.mTitleSp.spriteName = "Victory_Txt";
			Globals.Instance.BackgroundMusicMgr.StopWarmingSound();
			Globals.Instance.BackgroundMusicMgr.ClearGameBGM();
			Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
			Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("ui/ui_004b", false);
		}
		else
		{
			this.mTitleSp.spriteName = "Failure";
			Globals.Instance.BackgroundMusicMgr.StopWarmingSound();
			Globals.Instance.BackgroundMusicMgr.ClearGameBGM();
			Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
			Globals.Instance.BackgroundMusicMgr.PlayGameOverSound();
		}
		this.mTitleSp.gameObject.SetActive(true);
		base.StartCoroutine(this.ShowContentAnimation(GUIGuildCraftResultScene.ArenaResultData));
	}

	protected override void OnPreDestroyGUI()
	{
		GUIGuildCraftResultScene.ArenaResultData = null;
		base.StopAllCoroutines();
		Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
	}

	private void CreateObjects()
	{
		this.mTitleSp = base.transform.Find("result").GetComponent<UISprite>();
		this.mTitleSp.gameObject.SetActive(false);
		this.mBackBtn = base.transform.Find("backMenu").gameObject;
		UIEventListener expr_52 = UIEventListener.Get(this.mBackBtn);
		expr_52.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_52.onClick, new UIEventListener.VoidDelegate(this.OnBackBtnClick));
		this.mBackBtn.SetActive(false);
		Transform transform = base.transform.Find("infoBg");
		this.mTxt0Go = transform.Find("txt0").gameObject;
		this.mTxt0Go.SetActive(false);
		this.mLineGo = transform.Find("line1").gameObject;
		this.mLineGo.SetActive(false);
		this.mTimeGo = transform.Find("time").gameObject;
		this.mTimeNum = this.mTimeGo.transform.Find("num").GetComponent<UILabel>();
		this.mTimeGo.SetActive(false);
		this.mJiShaGo = transform.Find("txt1").gameObject;
		this.mJiShaNum = this.mJiShaGo.transform.Find("num").GetComponent<UILabel>();
		this.mJiShaGo.SetActive(false);
		this.mJiShaScoreGo = transform.Find("txt2").gameObject;
		this.mJiShaScore = this.mJiShaScoreGo.transform.Find("num").GetComponent<UILabel>();
		this.mJiShaScoreGo.SetActive(false);
	}

	[DebuggerHidden]
	private IEnumerator ShowContentAnimation(MS2C_GuildWarFightEnd resultData)
	{
        return null;
        //GUIGuildCraftResultScene.<ShowContentAnimation>c__Iterator58 <ShowContentAnimation>c__Iterator = new GUIGuildCraftResultScene.<ShowContentAnimation>c__Iterator58();
        //<ShowContentAnimation>c__Iterator.resultData = resultData;
        //<ShowContentAnimation>c__Iterator.<$>resultData = resultData;
        //<ShowContentAnimation>c__Iterator.<>f__this = this;
        //return <ShowContentAnimation>c__Iterator;
	}

	private void OnBackBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.PopBackSessionType();
		GameUIManager.mInstance.ChangeSession<GUIGuildCraftSetScene>(null, false, true);
	}
}
