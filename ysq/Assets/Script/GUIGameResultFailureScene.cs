using Att;
using System;
using UnityEngine;

public class GUIGameResultFailureScene : GameUISession
{
	private GameObject btnGroup;

	private GameObject btnRechallenge;

	private GameObject btnLast;

	private GameObject btnBackPveMenu;

	private void CreateObjects()
	{
		this.btnGroup = base.transform.Find("ButtonGroup").gameObject;
		this.btnRechallenge = base.RegisterClickEvent("resurrect", new UIEventListener.VoidDelegate(this.OnRechallengeClick), this.btnGroup);
		UILabel component = this.btnRechallenge.transform.Find("Label").GetComponent<UILabel>();
		component.overflowMethod = UILabel.Overflow.ShrinkContent;
		component.width = 170;
		this.btnLast = base.RegisterClickEvent("Last", new UIEventListener.VoidDelegate(this.OnBtnLastClick), this.btnGroup);
		this.btnBackPveMenu = base.RegisterClickEvent("backPveMenu", new UIEventListener.VoidDelegate(this.OnReturnClick), this.btnGroup);
		UILabel component2 = this.btnBackPveMenu.transform.Find("Label").GetComponent<UILabel>();
		component2.overflowMethod = UILabel.Overflow.ShrinkContent;
		component2.width = 176;
		this.btnGroup.SetActive(false);
		if (GameUIManager.mInstance.uiState.AdventureSceneInfo != null && GameUIManager.mInstance.uiState.AdventureSceneInfo.Type == 6)
		{
			this.btnBackPveMenu.transform.localPosition = new Vector3(0f, this.btnBackPveMenu.transform.localPosition.y, this.btnBackPveMenu.transform.localPosition.z);
			this.btnRechallenge.SetActive(false);
			this.btnLast.SetActive(false);
		}
		else
		{
			this.btnRechallenge.SetActive(true);
			this.btnLast.SetActive(true);
		}
		if (GameUIManager.mInstance.uiState.CurSceneInfo != null && GameUIManager.mInstance.uiState.CurSceneInfo.Type == 0 && GameUIManager.mInstance.uiState.CurSceneInfo.Difficulty == 2 && !GameUIManager.mInstance.uiState.CurSceneInfo.DayReset)
		{
			this.btnLast.SetActive(false);
		}
		GameObject gameObject = GameUITools.FindGameObject("Failure", base.gameObject);
		gameObject.AddComponent<GUIFailureTipsGroup>();
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		this.OpenBtnGroup();
		Globals.Instance.BackgroundMusicMgr.StopWarmingSound();
		Globals.Instance.BackgroundMusicMgr.ClearGameBGM();
		Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
		Globals.Instance.BackgroundMusicMgr.PlayGameOverSound();
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnPreDestroyGUI()
	{
		Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
	}

	private void OnRechallengeClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (GameUIManager.mInstance.uiState.CurSceneInfo.Type == 0 && GameUIManager.mInstance.uiState.CurSceneInfo.Difficulty == 2)
		{
			GameUIManager.mInstance.uiState.ResultSceneInfo = GameUIManager.mInstance.uiState.CurSceneInfo;
			GameUIManager.mInstance.ChangeSession<GUIAwakeRoadSceneV2>(null, true, true);
		}
		else
		{
			GameUIManager.mInstance.uiState.ResultSceneInfo = GameUIManager.mInstance.uiState.CurSceneInfo;
			GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, true, true);
		}
	}

	private void OnBtnLastClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (GameUIManager.mInstance.uiState.CurSceneInfo.Type == 0 && GameUIManager.mInstance.uiState.CurSceneInfo.Difficulty == 2)
		{
			GameUIManager.mInstance.ChangeSession<GUIAwakeRoadSceneV2>(null, true, true);
		}
		else
		{
			GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, true, true);
		}
	}

	public void OnReturnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.ResultSceneInfo = null;
		if (GameUIManager.mInstance.uiState.CurSceneInfo.Type == 0 && GameUIManager.mInstance.uiState.CurSceneInfo.Difficulty == 2)
		{
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
		if (curSceneInfo == null)
		{
			return;
		}
		SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(curSceneInfo.PreID);
		if (info != null)
		{
			this.OpenBtnLevel(info);
		}
		else
		{
			this.btnLast.SetActive(false);
		}
	}

	private void OpenBtnLevel(SceneInfo sceneInfo)
	{
		GameUIManager.mInstance.uiState.ResultSceneInfo = sceneInfo;
		if (GameUIManager.mInstance.uiState.AdventureSceneInfo != null && GameUIManager.mInstance.uiState.AdventureSceneInfo.Type != 6)
		{
			this.btnLast.SetActive(true);
		}
	}
}
