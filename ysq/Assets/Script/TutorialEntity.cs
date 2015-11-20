using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class TutorialEntity : MonoBehaviour
{
	public enum ETutorialHandDirection
	{
		ETHD_Null,
		ETHD_LeftDown,
		ETHD_LeftUp,
		ETHD_RightUp,
		ETHD_RightDown,
		ETHD_Left,
		ETHD_Right
	}

	protected delegate int IntCallback();

	private static Color32 FadeColor = new Color32(14, 8, 5, 130);

	private static Color32 TransparentColor = new Color32(255, 255, 255, 1);

	public GameObject guideMask;

	protected GameObject guideArea;

	public UISprite hand;

	protected UISprite area;

	protected UISprite fadeBG;

	protected UISprite halo;

	protected GameObject targetObj;

	protected GameObject targetCloneObj;

	protected GameObject clone;

	public GameObject ui38;

	protected UILabel tips;

	public GameObject guideAnimation;

	protected GUIMainMenuScene mainMenuScene;

	protected GUIWorldMap worldMap;

	protected GameUIAdventureReady adventureReady;

	protected GameUIQuestInformation questInformation;

	protected RewardMessagebox rewardMessage;

	protected GUICombatMain combatMain;

	protected GUIUnlockPopUp unlockPopUp;

	protected GUITeamManageSceneV2 teamManageSceneV2;

	protected GUIGameResultFailureScene gameResultFailureScene;

	private GUIGameResultVictoryScene gameResultVictoryScene;

	protected GUIMysteryScene mysteryScene;

	protected int afterDialogNextStep = -1;

	protected int dialogIndex = -1;

	protected int unlockPopStep = -1;

	public GUIPlotDialog.FinishCallback FinishEvent;

	protected UIPanel maskPanel;

	protected UIPanel animationPanel;

	private UIWidget mWidget;

	protected int intervalFrame;

	protected float waitTime;

	protected UIEventListener.VoidDelegate step_BackCB;

	protected TutorialEntity.IntCallback PlotEndStep;

	private bool hasShowPlot;

	private void OnDestroy()
	{
		if (this.guideMask != null)
		{
			UnityEngine.Object.Destroy(this.guideMask);
		}
		if (this.guideAnimation != null)
		{
			UnityEngine.Object.Destroy(this.guideAnimation);
		}
	}

	protected void SetParRQ()
	{
		base.StartCoroutine(this.SetParticlesRenderQ());
	}

	[DebuggerHidden]
	protected IEnumerator SetParticlesRenderQ()
	{
        return null;
        //TutorialEntity.<SetParticlesRenderQ>c__Iterator18 <SetParticlesRenderQ>c__Iterator = new TutorialEntity.<SetParticlesRenderQ>c__Iterator18();
        //<SetParticlesRenderQ>c__Iterator.<>f__this = this;
        //return <SetParticlesRenderQ>c__Iterator;
	}

	protected void InitGuideMask(TutorialInitParams param)
	{
		if (param.hasNullNecessaryParam())
		{
			return;
		}
		this.CreateGuideMask();
		this.maskPanel = this.guideMask.GetComponent<UIPanel>();
		this.animationPanel = this.guideAnimation.GetComponent<UIPanel>();
		if (!param.IsRemovePanel && !param.FreeTutorial)
		{
			if (this.maskPanel == null)
			{
				this.maskPanel = this.guideMask.AddComponent<UIPanel>();
			}
			this.maskPanel.enabled = true;
			this.maskPanel.depth = 2500;
			this.maskPanel.renderQueue = UIPanel.RenderQueue.StartAt;
			this.maskPanel.startingRenderQueue = ((param.PanelRenderQueue != 0) ? param.PanelRenderQueue : 6200);
			if (this.animationPanel == null)
			{
				this.animationPanel = this.guideAnimation.AddComponent<UIPanel>();
			}
			this.animationPanel.enabled = true;
			this.animationPanel.depth = 3000;
			this.animationPanel.renderQueue = UIPanel.RenderQueue.StartAt;
			this.animationPanel.startingRenderQueue = ((param.PanelRenderQueue != 0) ? (param.PanelRenderQueue + 300) : 6500);
		}
		else if (param.FreeTutorial)
		{
			if (this.maskPanel != null)
			{
				UnityEngine.Object.Destroy(this.maskPanel);
				this.maskPanel = null;
			}
			if (param.IsRemovePanel)
			{
				UnityEngine.Object.Destroy(this.animationPanel);
				this.animationPanel = null;
			}
			else
			{
				if (this.animationPanel == null)
				{
					this.animationPanel = this.guideAnimation.AddComponent<UIPanel>();
				}
				this.animationPanel.enabled = true;
				this.animationPanel.depth = 3000;
				this.animationPanel.renderQueue = UIPanel.RenderQueue.StartAt;
				this.animationPanel.startingRenderQueue = ((param.PanelRenderQueue != 0) ? (param.PanelRenderQueue + 300) : 6500);
			}
		}
		else if (param.IsRemovePanel)
		{
			if (this.maskPanel != null)
			{
				UnityEngine.Object.Destroy(this.maskPanel);
				this.maskPanel = null;
			}
			if (this.animationPanel != null)
			{
				UnityEngine.Object.Destroy(this.animationPanel);
				this.animationPanel = null;
			}
		}
		GameUITools.AddChild(param.MaskParent, this.guideMask);
		if (param.Depth != 0)
		{
			GameUITools.IncreaseObjectsDepth(this.guideMask, param.Depth);
		}
		this.guideAnimation.SetActive(false);
		this.guideMask.SetActive(false);
		this.guideMask.SetActive(true);
		this.area.enabled = false;
		base.StartCoroutine(this.InitGuideAnimation(param));
	}

	[DebuggerHidden]
	private IEnumerator InitGuideAnimation(TutorialInitParams param)
	{
        return null;
        //TutorialEntity.<InitGuideAnimation>c__Iterator19 <InitGuideAnimation>c__Iterator = new TutorialEntity.<InitGuideAnimation>c__Iterator19();
        //<InitGuideAnimation>c__Iterator.param = param;
        //<InitGuideAnimation>c__Iterator.<$>param = param;
        //<InitGuideAnimation>c__Iterator.<>f__this = this;
        //return <InitGuideAnimation>c__Iterator;
	}

	protected void SetTips(string strName)
	{
		if (this.tips == null)
		{
			return;
		}
		string @string = Singleton<StringManager>.Instance.GetString(strName);
		if (!string.IsNullOrEmpty(@string))
		{
			this.tips.gameObject.SetActive(true);
			this.tips.text = @string;
		}
		else
		{
			this.tips.gameObject.SetActive(false);
		}
	}

	protected void SetHalo(GameObject parent, Vector3 pos, int width, int height, List<GameObject> children)
	{
		UIPanel component = this.halo.transform.parent.gameObject.GetComponent<UIPanel>();
		component.depth = 3000;
		component.renderQueue = UIPanel.RenderQueue.StartAt;
		component.startingRenderQueue = 4000;
		this.halo.transform.parent.parent = parent.transform;
		this.halo.transform.parent.localPosition = Vector3.zero;
		this.halo.transform.localPosition = pos;
		this.halo.width = width;
		this.halo.height = height;
		this.halo.transform.parent.gameObject.SetActive(true);
		foreach (GameObject current in children)
		{
			current.transform.parent = this.halo.transform;
			current.transform.localScale = Vector3.one;
			GameUITools.IncreaseObjectsDepth(current, 1100);
		}
	}

	private void RestoreGuideMask()
	{
		this.targetCloneObj.SetActive(true);
		this.guideAnimation.SetActive(true);
		this.area.enabled = true;
		this.fadeBG.color = TutorialEntity.FadeColor;
	}

	protected void OnMaskAreaClickEnd(GameObject obj)
	{
		if (this.guideMask != null)
		{
			this.guideMask.SetActive(false);
			UIEventListener.Get(this.area.gameObject).onClick = null;
			UIEventListener expr_43 = UIEventListener.Get(this.area.gameObject);
			expr_43.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_43.onClick, new UIEventListener.VoidDelegate(this.OnMaskAreaClickEnd));
		}
		this.ResetGuideMask();
		if (this.clone != null)
		{
			this.clone.SetActive(false);
			UnityEngine.Object.Destroy(this.clone);
			this.clone = null;
		}
	}

	protected void ResetGuideMask()
	{
		this.guideArea.transform.parent = this.guideMask.transform;
		this.fadeBG.transform.parent = this.guideMask.transform;
		this.halo.transform.parent.parent = this.guideMask.transform;
		this.targetCloneObj.transform.parent = this.guideArea.transform;
		this.guideAnimation.transform.parent = this.guideArea.transform;
		this.area.transform.parent = this.guideArea.transform;
		this.hand.transform.parent = this.guideAnimation.transform;
		this.guideMask.transform.localPosition = Vector3.zero;
		this.fadeBG.transform.localPosition = new Vector3(0f, 0f, -500f);
		this.guideArea.transform.localPosition = Vector3.zero;
		this.guideAnimation.transform.localPosition = Vector3.zero;
		this.hand.transform.localPosition = Vector3.zero;
		this.ui38.transform.localPosition = new Vector3(0f, 0f, -1000f);
		this.targetCloneObj.transform.localPosition = new Vector3(0f, 0f, -1000f);
		this.area.transform.localPosition = new Vector3(0f, 0f, -1000f);
		this.SetDepth(this.fadeBG.gameObject, 1000);
		this.SetDepth(this.halo.gameObject, 1200);
		this.halo.transform.parent.gameObject.SetActive(false);
		this.SetDepth(this.guideArea.gameObject, 0);
		this.SetDepth(this.hand.gameObject, 1002);
		this.SetDepth(this.area.gameObject, 1001);
	}

	private void SetDepth(GameObject go, int value)
	{
		this.mWidget = go.GetComponent<UIWidget>();
		if (this.mWidget != null)
		{
			this.mWidget.depth = value;
		}
	}

	protected void CreateGuideMask()
	{
		if (this.guideMask == null)
		{
			this.guideMask = Tools.InstantiateGUIPrefab("GUI/GuideMask");
			this.guideMask.SetActive(false);
			this.guideArea = GameUITools.FindGameObject("GuideArea", this.guideMask);
			this.fadeBG = GameUITools.FindUISprite("FadeBG", this.guideMask);
			this.halo = GameUITools.FindUISprite("Halo/Halo", this.guideMask);
			this.halo.transform.parent.gameObject.SetActive(false);
			this.targetCloneObj = GameUITools.FindGameObject("TargetClone", this.guideArea);
			this.area = GameUITools.FindUISprite("Area", this.guideArea);
			this.guideAnimation = GameUITools.FindGameObject("Animation", this.guideArea);
			this.ui38 = GameUITools.FindGameObject("ui38", this.guideAnimation);
			this.hand = GameUITools.FindUISprite("Hand", this.guideAnimation);
			this.tips = GameUITools.FindUILabel("Tips", this.ui38);
			UIEventListener expr_11E = UIEventListener.Get(this.area.gameObject);
			expr_11E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_11E.onClick, new UIEventListener.VoidDelegate(this.OnMaskAreaClickEnd));
		}
	}

	public static void IncreaseObjectsPanelDepth(GameObject obj, UIPanel target)
	{
		if (target == null)
		{
			return;
		}
		UIPanel component = obj.GetComponent<UIPanel>();
		if (component != null && target != null)
		{
			component.depth += target.depth;
			if (component.renderQueue == UIPanel.RenderQueue.StartAt && target.renderQueue == UIPanel.RenderQueue.StartAt)
			{
				component.startingRenderQueue += target.startingRenderQueue;
			}
		}
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			TutorialEntity.IncreaseObjectsPanelDepth(obj.transform.GetChild(i).gameObject, target);
		}
	}

	protected void SetHandDirection(TutorialEntity.ETutorialHandDirection direction)
	{
		TweenPosition component = this.hand.GetComponent<TweenPosition>();
		switch (direction)
		{
		case TutorialEntity.ETutorialHandDirection.ETHD_LeftDown:
			this.hand.transform.localPosition = new Vector3(52f, 25f, -1000f);
			this.hand.transform.localRotation = Quaternion.Euler(0f, 0f, 45f);
			component.from = new Vector3(52f, 25f, -1000f);
			component.to = new Vector3(82f, 55f, -1000f);
			this.tips.pivot = UIWidget.Pivot.Left;
			this.tips.transform.localPosition = new Vector3(160f, 120f, 0f);
			break;
		case TutorialEntity.ETutorialHandDirection.ETHD_LeftUp:
			this.hand.transform.localPosition = new Vector3(30f, -55f, -1000f);
			this.hand.transform.localRotation = Quaternion.Euler(0f, 0f, -45f);
			component.from = new Vector3(30f, -55f, -1000f);
			component.to = new Vector3(60f, -85f, -1000f);
			this.tips.pivot = UIWidget.Pivot.Left;
			this.tips.transform.localPosition = new Vector3(140f, -120f, 0f);
			break;
		case TutorialEntity.ETutorialHandDirection.ETHD_RightUp:
			this.hand.transform.localPosition = new Vector3(-30f, -55f, -1000f);
			this.hand.transform.localRotation = Quaternion.Euler(0f, -180f, -45f);
			component.from = new Vector3(-30f, -55f, -1000f);
			component.to = new Vector3(-60f, -85f, -1000f);
			this.tips.pivot = UIWidget.Pivot.Right;
			this.tips.transform.localPosition = new Vector3(-160f, -120f, 0f);
			break;
		case TutorialEntity.ETutorialHandDirection.ETHD_RightDown:
			this.hand.transform.localPosition = new Vector3(-52f, 25f, -1000f);
			this.hand.transform.localRotation = Quaternion.Euler(0f, -180f, 45f);
			component.from = new Vector3(-52f, 25f, -1000f);
			component.to = new Vector3(-82f, 55f, -1000f);
			this.tips.pivot = UIWidget.Pivot.Right;
			this.tips.transform.localPosition = new Vector3(-160f, 120f, 0f);
			break;
		case TutorialEntity.ETutorialHandDirection.ETHD_Left:
			this.hand.transform.localPosition = new Vector3(60f, -20f, -1000f);
			this.hand.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			component.from = new Vector3(60f, -20f, -1000f);
			component.to = new Vector3(100f, -20f, -1000f);
			this.tips.pivot = UIWidget.Pivot.Left;
			this.tips.transform.localPosition = new Vector3(180f, -10f, 0f);
			break;
		case TutorialEntity.ETutorialHandDirection.ETHD_Right:
			this.hand.transform.localPosition = new Vector3(-60f, -20f, -1000f);
			this.hand.transform.localRotation = Quaternion.Euler(0f, -180f, 0f);
			component.from = new Vector3(-60f, -20f, -1000f);
			component.to = new Vector3(-100f, -20f, -1000f);
			this.tips.pivot = UIWidget.Pivot.Right;
			this.tips.transform.localPosition = new Vector3(-180f, -10f, 0f);
			break;
		}
		component.enabled = false;
	}

	public static void SetWidgetsColor2White(GameObject go)
	{
		if (go.GetComponent<UIWidget>() != null)
		{
			go.GetComponent<UIWidget>().color = Color.white;
		}
		foreach (Transform transform in go.transform)
		{
			TutorialEntity.SetWidgetsColor2White(transform.gameObject);
		}
	}

	public static void RemoveObjectsCollider(GameObject obj)
	{
		BoxCollider component = obj.GetComponent<BoxCollider>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			TutorialEntity.RemoveObjectsCollider(obj.transform.GetChild(i).gameObject);
		}
	}

	public static void SetObjectsCollider(GameObject obj, bool enabled)
	{
		BoxCollider component = obj.GetComponent<BoxCollider>();
		if (component != null)
		{
			component.enabled = enabled;
		}
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			TutorialEntity.SetObjectsCollider(obj.transform.GetChild(i).gameObject, enabled);
		}
	}

	public virtual bool SelectStep()
	{
		int currentTutorialStep = Globals.Instance.TutorialMgr.CurrentTutorialStep;
		if (currentTutorialStep >= 0)
		{
			string methodName = string.Format("Step_{0}", (currentTutorialStep <= 9) ? (0 + currentTutorialStep.ToString()) : currentTutorialStep.ToString());
			base.Invoke(methodName, 0f);
		}
		return false;
	}

	protected static T ConvertObject2UnityOrPrefab<T>() where T : MonoBehaviour
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is T)
		{
			return (T)((object)Globals.Instance.TutorialMgr.CurrentScene);
		}
		global::Debug.LogError(new object[]
		{
			string.Format("Parse Tutorial Scene or Prefab Error , {0}", typeof(T))
		});
		return (T)((object)null);
	}

	protected static void SetNextTutorialStep(int nextStep = -1, TutorialManager.ETutorialNum currentNum = TutorialManager.ETutorialNum.Tutorial_Null, bool isClearCurrentTutorialNum = false, bool isRefreshTutorial = false, bool isClearTutorial = false)
	{
		Globals.Instance.TutorialMgr.LastTutorialStep = Globals.Instance.TutorialMgr.CurrentTutorialStep;
		if (nextStep >= 0)
		{
			Globals.Instance.TutorialMgr.CurrentTutorialStep = nextStep;
		}
		if (currentNum != TutorialManager.ETutorialNum.Tutorial_Null && !Globals.Instance.TutorialMgr.IsPassTutorial(currentNum))
		{
			Globals.Instance.TutorialMgr.UpdateTutorialSteps(currentNum);
			TutorialEntity.SendTutorialNumToServer((int)currentNum);
		}
		if (isClearCurrentTutorialNum)
		{
			Globals.Instance.TutorialMgr.CurrentTutorialNum = TutorialManager.ETutorialNum.Tutorial_Null;
		}
		if (isClearTutorial)
		{
			Globals.Instance.TutorialMgr.ClearTutorial();
		}
		if (isRefreshTutorial)
		{
			Globals.Instance.TutorialMgr.InitializationCompleted(Globals.Instance.TutorialMgr.CurrentScene, null);
		}
	}

	public static void SendTutorialNumToServer(int index)
	{
		MC2S_SaveGuideSteps mC2S_SaveGuideSteps = new MC2S_SaveGuideSteps();
		mC2S_SaveGuideSteps.index = index;
		Globals.Instance.CliSession.SendPacket(244, mC2S_SaveGuideSteps);
	}

	protected static void FinishEarlierTutorialNum(TutorialManager.ETutorialNum num, int step, bool isUpdateTutorialSteps = true)
	{
		for (int i = 1; i < step; i++)
		{
			if (!Globals.Instance.TutorialMgr.IsPassTutorial(num + i))
			{
				if (isUpdateTutorialSteps)
				{
					Globals.Instance.TutorialMgr.UpdateTutorialSteps(num + i);
				}
				TutorialEntity.SendTutorialNumToServer((int)(num + i));
			}
		}
	}

	protected void Step_TeamBtn()
	{
		this.Step_TeamBtn(string.Empty);
	}

	protected void Step_TeamBtn(string tips)
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			this.mainMenuScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIMainMenuScene>();
		}
		if (this.mainMenuScene == null)
		{
			return;
		}
		TutorialInitParams tutorialInitParams = new TutorialInitParams();
		tutorialInitParams.MaskParent = this.mainMenuScene.transform.Find("UI_Edge").gameObject;
		tutorialInitParams.TargetName = "UI_Edge/PetBtn";
		tutorialInitParams.TargetParent = this.mainMenuScene.gameObject;
		tutorialInitParams.HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftDown;
		tutorialInitParams.HideTargetObj = true;
		if (string.IsNullOrEmpty(tips))
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString("tutorial5");
		}
		else
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString(tips);
		}
		this.InitGuideMask(tutorialInitParams);
		UIEventListener expr_D3 = UIEventListener.Get(this.area.gameObject);
		expr_D3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D3.onClick, new UIEventListener.VoidDelegate(this.OnStep_TeamBtnMaskAreaClick));
	}

	private void OnStep_TeamBtnMaskAreaClick(GameObject go)
	{
		this.mainMenuScene.OnPetBtnClick(null);
	}

	protected void Step_PVEBtnFree()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			this.mainMenuScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIMainMenuScene>();
		}
		if (this.mainMenuScene == null)
		{
			return;
		}
		this.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.mainMenuScene.transform.Find("UI_Edge").gameObject,
			TargetName = "UI_Edge/PveBtn",
			TargetParent = this.mainMenuScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			FreeTutorial = true,
			IsRemovePanel = true
		});
	}

	protected void Step_PVEBtn()
	{
		this.Step_PVEBtn(string.Empty);
	}

	protected void Step_PVEBtn(string tips)
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			this.mainMenuScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIMainMenuScene>();
		}
		if (this.mainMenuScene == null)
		{
			return;
		}
		TutorialInitParams tutorialInitParams = new TutorialInitParams();
		tutorialInitParams.MaskParent = this.mainMenuScene.transform.Find("UI_Edge").gameObject;
		tutorialInitParams.TargetName = "UI_Edge/PveBtn";
		tutorialInitParams.TargetParent = this.mainMenuScene.gameObject;
		tutorialInitParams.HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown;
		tutorialInitParams.HideTargetObj = true;
		if (string.IsNullOrEmpty(tips))
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString("tutorial9");
		}
		else
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString(tips);
		}
		this.InitGuideMask(tutorialInitParams);
		UIEventListener expr_D3 = UIEventListener.Get(this.area.gameObject);
		expr_D3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D3.onClick, new UIEventListener.VoidDelegate(this.OnStep_PVEBtnMaskAreaClick));
	}

	private void OnStep_PVEBtnMaskAreaClick(GameObject go)
	{
		this.mainMenuScene.OnPveBtnClick(null);
	}

	protected void Step_MysteryBtn()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			this.mainMenuScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIMainMenuScene>();
		}
		if (this.mainMenuScene == null)
		{
			return;
		}
		this.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.mainMenuScene.transform.Find("UI_Edge").gameObject,
			TargetName = "UI_Edge/mijingBtn",
			TargetParent = this.mainMenuScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			HideTargetObj = true
		});
		UIEventListener expr_9D = UIEventListener.Get(this.area.gameObject);
		expr_9D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_9D.onClick, new UIEventListener.VoidDelegate(this.OnStep_MysteryBtnMaskAreaClick));
	}

	private void OnStep_MysteryBtnMaskAreaClick(GameObject go)
	{
		this.mainMenuScene.OnMijingClick(null);
	}

	protected void Step_WorldTeam()
	{
		this.Step_WorldTeam(string.Empty);
	}

	protected void Step_WorldTeam(string tips)
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			this.worldMap = TutorialEntity.ConvertObject2UnityOrPrefab<GUIWorldMap>();
		}
		if (this.worldMap == null)
		{
			this.worldMap = GameUIManager.mInstance.GetSession<GUIWorldMap>();
		}
		if (this.worldMap == null)
		{
			return;
		}
		TutorialInitParams tutorialInitParams = new TutorialInitParams();
		tutorialInitParams.MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject;
		tutorialInitParams.TargetObj = GameUITools.FindGameObject("topPanel/PetBtn", this.worldMap.gameObject);
		tutorialInitParams.HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown;
		if (!string.IsNullOrEmpty(tips))
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString(tips);
		}
		this.InitGuideMask(tutorialInitParams);
		UIEventListener expr_C7 = UIEventListener.Get(this.area.gameObject);
		expr_C7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C7.onClick, new UIEventListener.VoidDelegate(this.OnStep_WorldTeamMaskAreaClick));
	}

	private void OnStep_WorldTeamMaskAreaClick(GameObject obj)
	{
		this.worldMap.OnPetBtnClick(null);
	}

	protected void Step_SceneBtnFree(int index, string tips)
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			this.worldMap = TutorialEntity.ConvertObject2UnityOrPrefab<GUIWorldMap>();
		}
		if (this.worldMap == null)
		{
			return;
		}
		TutorialInitParams tutorialInitParams = new TutorialInitParams();
		tutorialInitParams.MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject;
		tutorialInitParams.TargetObj = GameUITools.FindGameObject(string.Format("Level/{0}", index - 1), this.worldMap.gameObject);
		tutorialInitParams.HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_Left;
		tutorialInitParams.FreeTutorial = true;
		tutorialInitParams.IsRemovePanel = true;
		if (!string.IsNullOrEmpty(tips))
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString(tips);
		}
		this.InitGuideMask(tutorialInitParams);
	}

	protected void Step_SceneBtn(int index, string tips = null)
	{
		base.StartCoroutine(this.SceneBtn(index, tips));
	}

	[DebuggerHidden]
	private IEnumerator SceneBtn(int index, string tips = null)
	{
        return null;
        //TutorialEntity.<SceneBtn>c__Iterator1A <SceneBtn>c__Iterator1A = new TutorialEntity.<SceneBtn>c__Iterator1A();
        //<SceneBtn>c__Iterator1A.index = index;
        //<SceneBtn>c__Iterator1A.tips = tips;
        //<SceneBtn>c__Iterator1A.<$>index = index;
        //<SceneBtn>c__Iterator1A.<$>tips = tips;
        //<SceneBtn>c__Iterator1A.<>f__this = this;
        //return <SceneBtn>c__Iterator1A;
	}

	private void OnStep_SceneBtnMaskAreaClick(GameObject obj)
	{
		this.targetObj.GetComponent<SceneNode>().OnSceneNodeClicked(null);
	}

	protected void Step_StartSceneBtn()
	{
		this.Step_StartSceneBtn(string.Empty);
	}

	protected void Step_StartSceneBtn(string tips)
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GameUIAdventureReady)
		{
			this.adventureReady = TutorialEntity.ConvertObject2UnityOrPrefab<GameUIAdventureReady>();
		}
		if (this.adventureReady == null)
		{
			return;
		}
		TutorialInitParams tutorialInitParams = new TutorialInitParams();
		tutorialInitParams.MaskParent = this.adventureReady.gameObject;
		tutorialInitParams.Depth = 500;
		tutorialInitParams.TargetName = "winBG/RaidsBG/ReadyStartBtn";
		tutorialInitParams.TargetParent = this.adventureReady.gameObject;
		tutorialInitParams.HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown;
		if (string.IsNullOrEmpty(tips))
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString("tutorial10");
		}
		else
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString(tips);
		}
		this.InitGuideMask(tutorialInitParams);
		UIEventListener expr_C8 = UIEventListener.Get(this.area.gameObject);
		expr_C8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C8.onClick, new UIEventListener.VoidDelegate(this.OnStep_StartSceneBtnMaskAreaClick));
	}

	private void OnStep_StartSceneBtnMaskAreaClick(GameObject obj)
	{
		this.adventureReady.OnReadyStartBtnClicked(null);
	}

	protected void Step_FailureOKBtn()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultFailureScene)
		{
			this.gameResultFailureScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIGameResultFailureScene>();
		}
		if (this.gameResultFailureScene == null)
		{
			return;
		}
		this.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.gameResultFailureScene.gameObject,
			TargetName = "ButtonGroup/backPveMenu",
			TargetParent = this.gameResultFailureScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			HideTargetObj = true
		});
		UIEventListener expr_8E = UIEventListener.Get(this.area.gameObject);
		expr_8E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8E.onClick, new UIEventListener.VoidDelegate(this.OnStep_FailureOKBtnMaskAreaClick));
	}

	private void OnStep_FailureOKBtnMaskAreaClick(GameObject obj)
	{
		this.gameResultFailureScene.OnReturnClick(null);
	}

	protected void Step_VictoryOKBtn(UIEventListener.VoidDelegate beforeClick = null)
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultVictoryScene)
		{
			this.gameResultVictoryScene = TutorialEntity.ConvertObject2UnityOrPrefab<GUIGameResultVictoryScene>();
		}
		if (this.gameResultVictoryScene == null)
		{
			return;
		}
		if (GameUIPopupManager.GetInstance().GetState() == GameUIPopupManager.eSTATE.GUIPropsInfoPopUp)
		{
			GameUIPopupManager.GetInstance().PopState(true, null);
		}
		if (GameUIPopupManager.GetInstance().GetState() == GameUIPopupManager.eSTATE.GUIEquipInfoPopUp)
		{
			GameUIPopupManager.GetInstance().PopState(true, null);
		}
		GameUIManager.mInstance.DestroyPetInfoSceneV2();
		this.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.gameResultVictoryScene.gameObject,
			TargetName = "ButtonGroup/backPveMenu",
			TargetParent = this.gameResultVictoryScene.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown,
			Tips = Singleton<StringManager>.Instance.GetString("tutorial8")
		});
		if (beforeClick != null)
		{
			UIEventListener expr_E6 = UIEventListener.Get(this.area.gameObject);
			expr_E6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_E6.onClick, beforeClick);
		}
		UIEventListener expr_10C = UIEventListener.Get(this.area.gameObject);
		expr_10C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_10C.onClick, new UIEventListener.VoidDelegate(this.OnStep_VictoryOKBtnMaskAreaClick));
	}

	private void OnStep_VictoryOKBtnMaskAreaClick(GameObject obj)
	{
		this.gameResultVictoryScene.OnReturnClick(null);
	}

	protected void Step_QuestReceiveBtn()
	{
		this.Step_QuestReceiveBtn(string.Empty);
	}

	protected void Step_QuestReceiveBtn(string tips)
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GameUIQuestInformation)
		{
			this.questInformation = TutorialEntity.ConvertObject2UnityOrPrefab<GameUIQuestInformation>();
		}
		if (this.questInformation == null)
		{
			return;
		}
		TutorialInitParams tutorialInitParams = new TutorialInitParams();
		tutorialInitParams.MaskParent = this.questInformation.gameObject;
		tutorialInitParams.TargetName = "winBG/ReceiveBtn";
		tutorialInitParams.TargetParent = this.questInformation.gameObject;
		tutorialInitParams.HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftDown;
		if (string.IsNullOrEmpty(tips))
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString("tutorial14");
		}
		else
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString(tips);
		}
		this.InitGuideMask(tutorialInitParams);
		UIEventListener expr_BD = UIEventListener.Get(this.area.gameObject);
		expr_BD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_BD.onClick, new UIEventListener.VoidDelegate(this.OnStep_QuestReceiveBtnMaskAreaClick));
	}

	private void OnStep_QuestReceiveBtnMaskAreaClick(GameObject obj)
	{
		this.questInformation.OnQuestInfoReceiveBtnClicked(null);
	}

	protected void Step_QuestCloseBtn()
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GameUIQuestInformation)
		{
			this.questInformation = TutorialEntity.ConvertObject2UnityOrPrefab<GameUIQuestInformation>();
		}
		if (this.questInformation == null)
		{
			return;
		}
		this.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.questInformation.gameObject,
			TargetName = "winBG/closeBtn",
			TargetParent = this.questInformation.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftUp
		});
		UIEventListener expr_87 = UIEventListener.Get(this.area.gameObject);
		expr_87.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_87.onClick, new UIEventListener.VoidDelegate(this.OnStep_QuestCloseBtnMaskAreaClick));
	}

	private void OnStep_QuestCloseBtnMaskAreaClick(GameObject obj)
	{
		this.questInformation.OnCloseQuestInfoClicked(null);
		this.SelectStep();
	}

	protected void Step_QuestOKBtn(bool selectNext = true)
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is RewardMessagebox)
		{
			this.rewardMessage = TutorialEntity.ConvertObject2UnityOrPrefab<RewardMessagebox>();
		}
		if (this.rewardMessage == null)
		{
			return;
		}
		this.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.rewardMessage.gameObject,
			TargetName = "winBG/GoBtn",
			TargetParent = this.rewardMessage.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftDown
		});
		if (selectNext)
		{
			UIEventListener expr_8D = UIEventListener.Get(this.area.gameObject);
			expr_8D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_8D.onClick, new UIEventListener.VoidDelegate(this.OnStep_Step_QuestOKBtnMaskAreaClick));
		}
		else
		{
			UIEventListener expr_C3 = UIEventListener.Get(this.area.gameObject);
			expr_C3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C3.onClick, new UIEventListener.VoidDelegate(this.OnStep_Step_QuestOKBtn1MaskAreaClick));
		}
	}

	private void OnStep_Step_QuestOKBtnMaskAreaClick(GameObject obj)
	{
		this.rewardMessage.OnQuestMBOKClicked(null);
		this.SelectStep();
	}

	private void OnStep_Step_QuestOKBtn1MaskAreaClick(GameObject obj)
	{
		this.rewardMessage.OnQuestMBOKClicked(null);
	}

	protected void Step_BackBtnFree()
	{
		this.InitGuideMask(new TutorialInitParams
		{
			MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject.transform.Find("UIMiddle").gameObject,
			TargetName = "UIMiddle/UIBack",
			TargetParent = GameUIManager.mInstance.GetTopGoods().gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftUp,
			AnimationPosition = new Vector3(-30f, 0f, 0f),
			FreeTutorial = true,
			IsRemovePanel = true,
			CreateObjIntervalFrame = this.intervalFrame
		});
	}

	protected void Step_BackBtn()
	{
		this.Step_BackBtn(string.Empty);
	}

	protected void Step_BackBtn(string tips)
	{
		TutorialInitParams tutorialInitParams = new TutorialInitParams();
		tutorialInitParams.MaskParent = GameUIManager.mInstance.GetTopGoods().gameObject.transform.Find("UIMiddle").gameObject;
		tutorialInitParams.TargetName = "UIMiddle/UIBack";
		tutorialInitParams.TargetParent = GameUIManager.mInstance.GetTopGoods().gameObject;
		tutorialInitParams.HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftUp;
		tutorialInitParams.AnimationPosition = new Vector3(-30f, 0f, 0f);
		tutorialInitParams.HideGuideMask4Seconds = this.waitTime;
		this.waitTime = 0f;
		if (string.IsNullOrEmpty(tips))
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString("tutorial4");
		}
		else
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString(tips);
		}
		this.InitGuideMask(tutorialInitParams);
		UIEventListener expr_D4 = UIEventListener.Get(this.area.gameObject);
		expr_D4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D4.onClick, new UIEventListener.VoidDelegate(this.OnStep_BackBtnMaskAreaClick));
	}

	private void OnStep_BackBtnMaskAreaClick(GameObject obj)
	{
		if (this.step_BackCB != null)
		{
			UIEventListener.VoidDelegate voidDelegate = this.step_BackCB;
			this.step_BackCB = null;
			voidDelegate(obj);
		}
		else
		{
			GameUIManager.mInstance.GetTopGoods().OnBackClicked(null);
		}
	}

	protected void Step_Back2Main(string str = null)
	{
		this.Step_BackBtn(str);
		this.step_BackCB = delegate(GameObject obj)
		{
			GameUIManager.mInstance.ChangeSession<GUIMainMenuScene>(null, false, true);
		};
	}

	protected void Step_UnlockGoBtn()
	{
		this.Step_UnlockGoBtn(string.Empty);
	}

	protected void Step_UnlockGoBtn(string tips)
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIUnlockPopUp)
		{
			this.unlockPopUp = TutorialEntity.ConvertObject2UnityOrPrefab<GUIUnlockPopUp>();
		}
		if (this.unlockPopUp == null)
		{
			return;
		}
		TutorialInitParams tutorialInitParams = new TutorialInitParams();
		tutorialInitParams.MaskParent = this.unlockPopUp.gameObject;
		tutorialInitParams.TargetName = "Go";
		tutorialInitParams.TargetParent = this.unlockPopUp.gameObject;
		tutorialInitParams.HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_LeftDown;
		if (string.IsNullOrEmpty(tips))
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString("tutorial33");
		}
		else
		{
			tutorialInitParams.Tips = Singleton<StringManager>.Instance.GetString(tips);
		}
		this.InitGuideMask(tutorialInitParams);
		this.fadeBG.color = TutorialEntity.TransparentColor;
		UIEventListener expr_D2 = UIEventListener.Get(this.area.gameObject);
		expr_D2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D2.onClick, new UIEventListener.VoidDelegate(this.OnStep_UnlockGoBtnMaskAreaClick));
	}

	private void OnStep_UnlockGoBtnMaskAreaClick(GameObject obj)
	{
		this.fadeBG.color = TutorialEntity.FadeColor;
		this.unlockPopUp.OnGoClick(null);
	}

	protected void Step_CloseAdventureReady()
	{
		if (this.worldMap == null)
		{
			this.worldMap = GameUIManager.mInstance.GetSession<GUIWorldMap>();
		}
		if (this.worldMap == null)
		{
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GameUIAdventureReady)
		{
			this.adventureReady = TutorialEntity.ConvertObject2UnityOrPrefab<GameUIAdventureReady>();
		}
		if (this.adventureReady == null)
		{
			this.adventureReady = this.worldMap.transform.parent.FindChild("GameUIAdventureReady(Clone)").gameObject.GetComponent<GameUIAdventureReady>();
		}
		if (this.adventureReady == null)
		{
			return;
		}
		this.InitGuideMask(new TutorialInitParams
		{
			MaskParent = this.adventureReady.gameObject,
			Depth = 500,
			TargetName = "winBG/closeBtn",
			TargetParent = this.adventureReady.gameObject,
			HandDirection = TutorialEntity.ETutorialHandDirection.ETHD_RightDown
		});
		UIEventListener expr_100 = UIEventListener.Get(this.area.gameObject);
		expr_100.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_100.onClick, new UIEventListener.VoidDelegate(this.OnStep_CloseAdventureReadyMaskAreaClick));
	}

	protected void OnStep_CloseAdventureReadyMaskAreaClick(GameObject obj)
	{
		this.adventureReady.OnReadyCloseBtnClicked(null);
		this.adventureReady = null;
		Globals.Instance.TutorialMgr.InitializationCompleted(this.worldMap, null);
	}

	protected void ResetFadeBGArea()
	{
		if (this.fadeBG != null && this.area != null)
		{
			GameObject gameObject = GameUITools.FindGameObject("q", this.ui38);
			gameObject.SetActive(true);
			this.fadeBG.gameObject.SetActive(true);
			this.area.gameObject.SetActive(true);
			this.fadeBG.color = TutorialEntity.FadeColor;
			this.fadeBG.gameObject.SetActive(false);
			this.fadeBG.gameObject.SetActive(true);
			this.fadeBG.enabled = true;
			UIEventListener.Get(this.area.gameObject).onClick = null;
			UIEventListener expr_C5 = UIEventListener.Get(this.area.gameObject);
			expr_C5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C5.onClick, new UIEventListener.VoidDelegate(this.OnMaskAreaClickEnd));
		}
	}

	protected void StartNextTutorial()
	{
		TutorialEntity.SetNextTutorialStep(1, TutorialManager.ETutorialNum.Tutorial_Null, true, true, true);
	}

	protected void ShowFadeBG()
	{
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		base.StartCoroutine("HideBG");
	}

	protected void HideFadeBG()
	{
		GameUIManager.mInstance.HideFadeBG(false);
		base.StopCoroutine("HideBG");
	}

	[DebuggerHidden]
	private IEnumerator HideBG()
	{
        return null;
        //return new TutorialEntity.<HideBG>c__Iterator1B();
	}

	protected static bool IsLevelUpTutorialScene()
	{
		return TutorialEntity.IsLevelUpScene(Globals.Instance.TutorialMgr.CurrentScene);
	}

	public static bool IsLevelUpScene(UnityEngine.Object scene)
	{
		if (scene is GUIMainMenuScene)
		{
			return true;
		}
		if (scene is GUIGameResultVictoryScene)
		{
			return true;
		}
		if (scene is GUIPVP4ReadyScene)
		{
			return true;
		}
		if (scene is GameUIMapFarm)
		{
			return true;
		}
		if (scene is GUIPillageFarm)
		{
			return true;
		}
		if (scene is GUIPVP4FarmPopUp)
		{
			return true;
		}
		if (scene is GameUILevelupPanel)
		{
			if (GameUIManager.mInstance.GetSession<GUIAchievementScene>() != null)
			{
				return true;
			}
			if (GameUIManager.mInstance.GetSession<GUIPropsBagScene>() != null)
			{
				return true;
			}
			if (GameUIManager.mInstance.GetSession<GUIWorldMap>() != null)
			{
				return true;
			}
			if (GameUIManager.mInstance.GetSession<GUIQuestScene>() != null)
			{
				return true;
			}
		}
		return false;
	}

	protected void LevelUpFilter(int plotIndex, int mainMenuStep)
	{
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMainMenuScene)
		{
			TutorialEntity.SetNextTutorialStep(mainMenuStep, TutorialManager.ETutorialNum.Tutorial_Null, false, false, false);
			if (this.hasShowPlot)
			{
				this.InvokeSelectStep();
				this.hasShowPlot = false;
			}
			else
			{
				GameUIManager.mInstance.ShowPlotDialog(plotIndex, new GUIPlotDialog.FinishCallback(this.InvokeSelectStep), null);
			}
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIGameResultVictoryScene)
		{
			this.Step_VictoryOKBtn(null);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIWorldMap)
		{
			this.ShowPlotAndBack2Main(plotIndex);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GameUIQuestInformation)
		{
			this.questInformation = TutorialEntity.ConvertObject2UnityOrPrefab<GameUIQuestInformation>();
			this.questInformation.OnCloseQuestInfoAnimEnd();
			this.ShowPlotAndBack2Main(plotIndex);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPVP4ReadyScene)
		{
			this.ShowPlotAndBack2Main(plotIndex);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPillageFarm)
		{
			GameUIManager.mInstance.ShowPlotDialog(plotIndex, new GUIPlotDialog.FinishCallback(this.Step_BackBtn), null);
			this.step_BackCB = delegate(GameObject obj)
			{
				GameUIManager.mInstance.ChangeSession<GUIMysteryScene>(null, false, true);
			};
			this.hasShowPlot = true;
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIPVP4FarmPopUp)
		{
			GameUIManager.mInstance.ShowPlotDialog(plotIndex, new GUIPlotDialog.FinishCallback(this.Step_BackBtn), null);
			this.step_BackCB = delegate(GameObject obj)
			{
				GameUIManager.mInstance.ChangeSession<GUIMysteryScene>(null, false, true);
			};
			this.hasShowPlot = true;
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GUIMysteryScene)
		{
			this.Step_Back2Main(null);
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GameUIMapFarm)
		{
			this.Step_CloseAdventureReady();
			return;
		}
		if (Globals.Instance.TutorialMgr.CurrentScene is GameUILevelupPanel)
		{
			if (GameUIManager.mInstance.GetSession<GUIAchievementScene>() != null)
			{
				this.ShowPlotAndBack2Main(plotIndex);
				return;
			}
			if (GameUIManager.mInstance.GetSession<GUIPropsBagScene>() != null)
			{
				this.ShowPlotAndBack2Main(plotIndex);
				return;
			}
			if (GameUIManager.mInstance.GetSession<GUIWorldMap>() != null)
			{
				if (GameUITools.FindGameObject("GameUIMapFarm", GameUIManager.mInstance.GetSession<GUIWorldMap>().transform.parent.gameObject) != null)
				{
					return;
				}
				this.ShowPlotAndBack2Main(plotIndex);
				return;
			}
			else if (GameUIManager.mInstance.GetSession<GUIQuestScene>() != null)
			{
				this.ShowPlotAndBack2Main(plotIndex);
				return;
			}
		}
	}

	private void ShowPlotAndBack2Main(int plotIndex)
	{
		if (this.hasShowPlot)
		{
			this.Step_BackBtn();
		}
		else
		{
			GameUIManager.mInstance.ShowPlotDialog(plotIndex, new GUIPlotDialog.FinishCallback(this.Step_BackBtn), null);
			this.hasShowPlot = true;
		}
		this.step_BackCB = delegate(GameObject obj)
		{
			GameUIManager.mInstance.ChangeSession<GUIMainMenuScene>(null, false, true);
		};
	}

	protected void InvokeSelectStep()
	{
		this.SelectStep();
	}

	protected void PlaySound(string sound)
	{
		if (this == null)
		{
			return;
		}
		base.StartCoroutine(this._PlaySound(sound));
	}

	[DebuggerHidden]
	private IEnumerator _PlaySound(string sound)
	{
        return null;
        //TutorialEntity.<_PlaySound>c__Iterator1C <_PlaySound>c__Iterator1C = new TutorialEntity.<_PlaySound>c__Iterator1C();
        //<_PlaySound>c__Iterator1C.sound = sound;
        //<_PlaySound>c__Iterator1C.<$>sound = sound;
        //return <_PlaySound>c__Iterator1C;
	}
}
