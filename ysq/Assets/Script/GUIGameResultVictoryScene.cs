using Att;
using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIGameResultVictoryScene : GameUISession
{
	public enum EAnimState
	{
		EAS_Null,
		EAS_Star,
		EAS_Exp
	}

	private GameObject fadeBG;

	private GameObject btnGroup;

	private GameObject btnLevel;

	private GameObject mResurrectBtn;

	private GameObject victoryPanel;

	private GameObject victoryContent;

	private GameObject starGroup;

	private GameObject[] starSprite = new GameObject[3];

	private GameObject content;

	private UILabel timeLb;

	private UILabel goldLb;

	private UILabel expLb;

	private GameObject levelGrp;

	private GUILevelExpUpAnimation levelAnimation;

	private RewardPet[] mPetItems;

	private UITweener goldDouble;

	private UITweener expDouble;

	private GameObject victoryFirst;

	private GameObject treasureBox;

	private GameObject cardGroup;

	private float combatTime;

	private GUIGameResultVictoryScene.EAnimState curAnimState;

	private void CreateObjects()
	{
		this.fadeBG = base.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnFadeBGClick), null);
		this.fadeBG.SetActive(false);
		this.btnGroup = base.transform.Find("ButtonGroup").gameObject;
		this.mResurrectBtn = base.RegisterClickEvent("resurrect", new UIEventListener.VoidDelegate(this.OnRechallengeClick), this.btnGroup);
		this.btnLevel = base.RegisterClickEvent("Last", new UIEventListener.VoidDelegate(this.OnBtnLevelClick), this.btnGroup);
		base.RegisterClickEvent("backPveMenu", new UIEventListener.VoidDelegate(this.OnReturnClick), this.btnGroup);
		this.btnGroup.SetActive(false);
		this.victoryPanel = base.transform.Find("VictoryPanel").gameObject;
		this.victoryPanel.SetActive(false);
		this.victoryContent = this.victoryPanel.transform.Find("victory").gameObject;
		this.starGroup = this.victoryContent.transform.Find("StartGroup").gameObject;
		for (int i = 0; i < 3; i++)
		{
			this.starSprite[i] = this.starGroup.transform.Find(string.Format("start{0}", i)).gameObject;
			this.starSprite[i].SetActive(false);
		}
		TweenScale component = this.victoryContent.transform.Find("text").GetComponent<TweenScale>();
		this.content = component.gameObject;
		this.content.SetActive(false);
		this.timeLb = component.transform.Find("Time").GetComponent<UILabel>();
		this.timeLb.gameObject.SetActive(false);
		this.goldLb = component.transform.Find("Gold").GetComponent<UILabel>();
		this.goldLb.gameObject.SetActive(false);
		this.expLb = component.transform.Find("Exp").GetComponent<UILabel>();
		this.expLb.gameObject.SetActive(false);
		this.levelGrp = component.transform.Find("Level").gameObject;
		this.levelGrp.SetActive(false);
		this.levelAnimation = this.levelGrp.AddComponent<GUILevelExpUpAnimation>();
		this.levelAnimation.Init();
		this.goldDouble = this.goldLb.transform.Find("Label").GetComponent<UITweener>();
		this.goldDouble.gameObject.SetActive(false);
		this.expDouble = this.expLb.transform.Find("Label").GetComponent<UITweener>();
		this.expDouble.gameObject.SetActive(false);
		this.victoryFirst = this.victoryContent.transform.Find("victory_first").gameObject;
		this.victoryFirst.SetActive(false);
		this.treasureBox = this.victoryPanel.transform.Find("Treasure-Box").gameObject;
		this.treasureBox.SetActive(false);
		this.cardGroup = this.victoryPanel.transform.Find("CardGroup").gameObject;
		this.cardGroup.SetActive(false);
	}

	public static void CacheOldData()
	{
		GameUIState uiState = GameUIManager.mInstance.uiState;
		uiState.SetOldFurtherData(Globals.Instance.Player.TeamSystem.GetPet(0));
		GameUIQuestInformation.SaveOldData();
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		base.StartCoroutine(this.PlayVictoryPanelAnim());
	}

	protected override void OnPreDestroyGUI()
	{
		base.StopAllCoroutines();
		Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
		GameUIManager.mInstance.uiState.PveResult = null;
	}

	public void OnRechallengeClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.ResultSceneInfo = GameUIManager.mInstance.uiState.CurSceneInfo;
		this.BackToWorldMap();
	}

	public void OnBtnLevelClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.BackToWorldMap();
	}

	public void OnReturnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (GameUIManager.mInstance.uiState.LastScore == 0)
		{
			GameUIManager.mInstance.uiState.ResultSceneInfo2 = GameUIManager.mInstance.uiState.ResultSceneInfo;
		}
		GameUIManager.mInstance.uiState.ResultSceneInfo = null;
		this.BackToWorldMap();
	}

	private void BackToWorldMap()
	{
		if (GameUIManager.mInstance.uiState.CurSceneInfo.Type == 0 && GameUIManager.mInstance.uiState.CurSceneInfo.Difficulty == 2)
		{
			GameUIManager.mInstance.uiState.AdventureSceneInfo = null;
			GameUIManager.mInstance.uiState.ResultSceneInfo2 = null;
			GameUIManager.mInstance.ChangeSession<GUIAwakeRoadSceneV2>(null, true, true);
		}
		else
		{
			GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, true, true);
		}
	}

	private void OpenBtnGroup()
	{
		this.btnGroup.SetActive(true);
		SceneInfo curSceneInfo = GameUIManager.mInstance.uiState.CurSceneInfo;
		SceneInfo sceneInfo;
		if (curSceneInfo.ID / 100000 == 6)
		{
			sceneInfo = Tools.GetNextAwakeMapSceneInfo(curSceneInfo);
		}
		else
		{
			sceneInfo = Tools.GetNextMapSceneInfo(curSceneInfo);
		}
		if (sceneInfo != null)
		{
			this.OpenBtnLevel(sceneInfo);
		}
		else
		{
			this.btnLevel.SetActive(false);
		}
		if (curSceneInfo.ID / 100000 == 6 && curSceneInfo.ID % 100000 % 1000 == 6)
		{
			this.mResurrectBtn.SetActive(false);
		}
		else
		{
			this.mResurrectBtn.SetActive(true);
		}
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private void OpenBtnLevel(SceneInfo sceneInfo)
	{
		GameUIManager.mInstance.uiState.AdventureSceneInfo = sceneInfo;
		GameUIManager.mInstance.uiState.ResultSceneInfo = sceneInfo;
		this.btnLevel.SetActive(true);
	}

	private void OpenLevelupPanel()
	{
		GameUILevelupPanel.GetInstance().Init(base.transform, new GameUIPopupManager.PopClosedCallback(this.OnCloseLevelupPanel));
	}

	[DebuggerHidden]
	private IEnumerator PlayCardAnim()
	{
        return null;
        //GUIGameResultVictoryScene.<PlayCardAnim>c__Iterator4C <PlayCardAnim>c__Iterator4C = new GUIGameResultVictoryScene.<PlayCardAnim>c__Iterator4C();
        //<PlayCardAnim>c__Iterator4C.<>f__this = this;
        //return <PlayCardAnim>c__Iterator4C;
	}

	private void OnCloseLevelupPanel()
	{
		base.StartCoroutine(this.PlayCardAnim());
	}

	[DebuggerHidden]
	private IEnumerator StartCombatTime()
	{
        return null;
        //GUIGameResultVictoryScene.<StartCombatTime>c__Iterator4D <StartCombatTime>c__Iterator4D = new GUIGameResultVictoryScene.<StartCombatTime>c__Iterator4D();
        //<StartCombatTime>c__Iterator4D.<>f__this = this;
        //return <StartCombatTime>c__Iterator4D;
	}

	[DebuggerHidden]
	private IEnumerator PlayVictoryPanelAnim()
	{
        return null;
        //GUIGameResultVictoryScene.<PlayVictoryPanelAnim>c__Iterator4E <PlayVictoryPanelAnim>c__Iterator4E = new GUIGameResultVictoryScene.<PlayVictoryPanelAnim>c__Iterator4E();
        //<PlayVictoryPanelAnim>c__Iterator4E.<>f__this = this;
        //return <PlayVictoryPanelAnim>c__Iterator4E;
	}

	private void UpdateTimeText(int time)
	{
		this.timeLb.text = Tools.UpdateTimeText(time);
	}

	public void OnFadeBGClick(GameObject go)
	{
		if (this.curAnimState == GUIGameResultVictoryScene.EAnimState.EAS_Null)
		{
			return;
		}
		GUIGameResultVictoryScene.EAnimState eAnimState = this.curAnimState;
		this.curAnimState = GUIGameResultVictoryScene.EAnimState.EAS_Null;
		int num = (int)eAnimState;
		if (num != 1)
		{
			if (num == 2)
			{
				base.StopAllCoroutines();
				this.timeLb.gameObject.SetActive(true);
				this.UpdateTimeText((int)this.combatTime);
				this.goldLb.gameObject.SetActive(true);
				MS2C_PveResult pveResult = GameUIManager.mInstance.uiState.PveResult;
				int lootMoney = pveResult.LootMoney;
				this.goldLb.text = lootMoney.ToString();
				this.expLb.gameObject.SetActive(true);
				int lootExp = pveResult.LootExp;
				this.expLb.text = lootExp.ToString();
				this.levelGrp.SetActive(true);
				GameUIState uiState = GameUIManager.mInstance.uiState;
				ObscuredStats data = Globals.Instance.Player.Data;
				this.levelAnimation.RefreshExpItem(uiState.PlayerLevel, uiState.PlayerExp, data.Level, data.Exp, (float)lootExp, new GUILevelExpUpAnimation.ExpCallback(GUILevelExpUpAnimation.PlayerExpCallback), null);
				if (this.levelAnimation.IsLevelup)
				{
					this.OpenLevelupPanel();
				}
				else
				{
					base.StartCoroutine(this.PlayCardAnim());
				}
			}
		}
		else
		{
			base.StopAllCoroutines();
			this.starGroup.animation[this.starGroup.animation.clip.name].normalizedTime = 1f;
			this.victoryContent.animation[this.victoryContent.animation.clip.name].normalizedTime = 1f;
			for (int i = 0; i < Globals.Instance.ActorMgr.Score; i++)
			{
				this.starSprite[i].transform.GetChild(0).gameObject.SetActive(false);
			}
			base.StartCoroutine(this.StartCombatTime());
		}
	}
}
