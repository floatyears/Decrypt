using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameUIPopupManager : MonoBehaviour
{
	public enum eSTATE
	{
		None = -1,
		GameUIOptionPopUp,
		GameUItakeName,
		GameUICommonBillboardPopUp,
		GameUIRuleInfoPopUp,
		GUIShortcutBuyItem,
		GUIPvpRecordPopUp,
		GUIHighestRankPopUp,
		GameUIPVPRuleInfoPopUp,
		GUIDayEnergyEvent,
		GUIHowGetPetItemPopUp,
		GUIGuildSetPopUp,
		GUIGuildChangeNamePopUp,
		GUIGuildMyJobPopUp,
		GUIGuildImpeachPopUp,
		GUIGuildLogPopUp,
		GUIGuildMemberPopUp,
		GUIGuildMagicPopUp,
		GUIGuildSchoolPopUp,
		GUIEquipLvlUpPopUp,
		GUIVIPLevelUpPopUp,
		GUIGuildCraftKillLog,
		GUIUnlockPopUp,
		GUIBillboardPopUp,
		GUIInvitePopUp,
		GUIExchangeAcNumPopUp,
		GUIPropsInfoPopUp,
		GUIEquipInfoPopUp,
		GUIAwakeItemInfoPopUp,
		GUIEquipMasterInfoPopUp,
		GUIRecycleGetItemsPopUp,
		GUITrialRewardPopUp,
		GameUIPhoneBindPopUp,
		GameUISharePopUp,
		GUILongLinRewardPopUp,
		GUIGuildSchoolTargetPopUp,
		GameUIFairyTalePopUp,
		GUILuckyDrawRulePopUp,
		GUIWBRuleInfoPopUp,
		GUIMultiUsePopUp,
		GUIAgreementInfoPopUp,
		GUIPetZhuWeiPopUp,
		GUIGuildMinesRewardDescPopUp,
		GUIGuildMinesRecordPopUp,
		GUIGuildMinesResultPopUp,
		GUIGuildCraftYuJiPopUp,
		GUIGuildCraftCanZhanPopUp,
		GUIGuildCraftRecord,
		GUIPeiYangNumPopUp,
		GUIGuildCraftYZHalfPop,
		GUIGuildCraftYZTopPop,
		GUIGroupBuyingRewardPopUp,
		Max
	}

	public delegate void PopClosedCallback();

	public GameUIPopupManager.PopClosedCallback mPopClosedCallbackEvent;

	public GameUIPopupManager.PopClosedCallback mPushShowedCallbackEvent;

	public GameUIPopupManager.PopClosedCallback mPopClosedCallbackEvent2;

	private StringBuilder mStringBuilder = new StringBuilder();

	private static GameUIPopupManager mInstance;

	private GameObject mButtonBlocker;

	private GameUIBasePopup curPopup;

	private Stack<GameUIPopupManager.eSTATE> mStateStack = new Stack<GameUIPopupManager.eSTATE>();

	private GameUIBasePopup mCurPopup
	{
		get
		{
			return this.curPopup;
		}
		set
		{
			this.curPopup = value;
		}
	}

	public static GameUIPopupManager GetInstance()
	{
		if (GameUIPopupManager.mInstance == null)
		{
			GameUIPopupManager.CreateInstance();
			UnityEngine.Object.DontDestroyOnLoad(GameUIPopupManager.mInstance.gameObject);
		}
		return GameUIPopupManager.mInstance;
	}

	private static void CreateInstance()
	{
		GameUIPopupManager.mInstance = new GameObject("GameUIPopupManager").AddComponent<GameUIPopupManager>();
		Tools.Assert(GameUIPopupManager.mInstance, "GameUIPopupManager cant find.");
	}

	public int GetStackSize()
	{
		return this.mStateStack.Count;
	}

	private GameUIBasePopup CreatePopup(GameUIPopupManager.eSTATE eState)
	{
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
		this.mStringBuilder.Append("GUI/").Append(eState.ToString());
		GameObject gameObject = Res.LoadGUI(this.mStringBuilder.ToString());
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/MessageTip error"
			});
			return null;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild error"
			});
			return null;
		}
		GameUIBasePopup gameUIBasePopup = gameObject2.GetComponent(eState.ToString()) as GameUIBasePopup;
		if (gameUIBasePopup == null)
		{
			gameUIBasePopup = (gameObject2.AddComponent(eState.ToString()) as GameUIBasePopup);
		}
		gameUIBasePopup.gameObject.SetActive(true);
		GameObject gameObject3 = Res.LoadGUI("GUI/GameUIButtonBlocker");
		if (gameObject3 == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GameUIButtonBlocker error"
			});
			return null;
		}
		this.mButtonBlocker = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject3);
		UIEventListener expr_13A = UIEventListener.Get(this.mButtonBlocker);
		expr_13A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_13A.onClick, new UIEventListener.VoidDelegate(this.OnButtonBlockerClick));
		this.mCurPopup = gameUIBasePopup;
		this.SetCurrentPopupPanel(5400, 400, 3000);
		return gameUIBasePopup;
	}

	private void OnButtonBlockerClick(GameObject go)
	{
		if (this.mCurPopup != null)
		{
			this.mCurPopup.OnButtonBlockerClick();
		}
	}

	public GameUIBasePopup GetCurrentPopup()
	{
		return this.mCurPopup;
	}

	public void SetCurrentPopupPanel(int renderQueue, int depth, int z)
	{
		this.mCurPopup.transform.localPosition = new Vector3(0f, 0f, (float)z);
		UIPanel uIPanel = this.mCurPopup.transform.GetComponent<UIPanel>();
		if (uIPanel == null)
		{
			uIPanel = this.mCurPopup.gameObject.AddComponent<UIPanel>();
		}
		uIPanel.depth = depth;
		uIPanel.renderQueue = UIPanel.RenderQueue.StartAt;
		uIPanel.startingRenderQueue = renderQueue;
		this.mButtonBlocker.transform.localPosition = new Vector3(0f, 0f, (float)(z + 100));
		UIPanel uIPanel2 = this.mButtonBlocker.transform.GetComponent<UIPanel>();
		if (uIPanel2 == null)
		{
			uIPanel2 = this.mButtonBlocker.gameObject.AddComponent<UIPanel>();
		}
		uIPanel2.depth = depth - 10;
		uIPanel2.renderQueue = UIPanel.RenderQueue.StartAt;
		uIPanel2.startingRenderQueue = renderQueue - 5;
	}

	public GameUIPopupManager.eSTATE GetState()
	{
		if (this.mStateStack.Count == 0)
		{
			return GameUIPopupManager.eSTATE.None;
		}
		return this.mStateStack.Peek();
	}

	public GameUIPopupManager.eSTATE GetStateByIndex(int index)
	{
		if (index < this.mStateStack.Count)
		{
			GameUIPopupManager.eSTATE[] array = this.mStateStack.ToArray();
			return array[index];
		}
		return GameUIPopupManager.eSTATE.None;
	}

	public void PopState(bool isCloseNow = false, GameUIPopupManager.PopClosedCallback cb = null)
	{
		this.mPopClosedCallbackEvent = cb;
		if (this.mStateStack.Count <= 1)
		{
			this.SetStateNone(isCloseNow);
		}
		else
		{
			this.EndAnimation(isCloseNow);
		}
	}

	public void PushState(GameUIPopupManager.eSTATE eState, bool isCloseNow = false, GameUIPopupManager.PopClosedCallback cb = null, GameUIPopupManager.PopClosedCallback cb2 = null)
	{
		this.mPushShowedCallbackEvent = cb;
		this.mPopClosedCallbackEvent2 = cb2;
		if (this.GetState() != eState)
		{
			this.mStateStack.Push(eState);
			if (eState == GameUIPopupManager.eSTATE.None)
			{
				this.SetStateNone(isCloseNow);
			}
			else
			{
				if (this.mCurPopup != null)
				{
					this.mCurPopup.gameObject.SetActive(false);
					UnityEngine.Object.Destroy(this.mCurPopup.gameObject);
					this.mButtonBlocker.gameObject.SetActive(false);
					UnityEngine.Object.Destroy(this.mButtonBlocker.gameObject);
				}
				this.CreatePopup(eState);
				this.StartAnimation();
			}
		}
	}

	private void SetStateNone(bool isCloseNow)
	{
		if (this.mCurPopup != null)
		{
			if (!isCloseNow)
			{
				this.StateNoneAnimation();
			}
			else
			{
				this.OnStateNoneAnimationComplete();
			}
		}
	}

	private void StartAnimation()
	{
		GameUITools.PlayOpenWindowAnim(this.mCurPopup.gameObject.transform, new TweenDelegate.TweenCallback(this.OnShowEndAnimComplete), true);
	}

	private void OnShowEndAnimComplete()
	{
		if (this.mPushShowedCallbackEvent != null)
		{
			this.mPushShowedCallbackEvent();
		}
	}

	private void EndAnimation(bool isCloseNow)
	{
		if (!HOTween.IsTweening(this.mCurPopup.gameObject.transform))
		{
			this.mCurPopup.OnPopUpClosing();
		}
		if (!isCloseNow)
		{
			GameUITools.PlayCloseWindowAnim(this.mCurPopup.gameObject.transform, new TweenDelegate.TweenCallback(this.OnEndAnimComplete), true);
		}
		else
		{
			this.OnEndAnimComplete();
		}
	}

	private void StateNoneAnimation()
	{
		if (!HOTween.IsTweening(this.mCurPopup.gameObject.transform))
		{
			this.mCurPopup.OnPopUpClosing();
			HOTween.To(this.mCurPopup.gameObject.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.zero).Ease(EaseType.EaseInBack).OnComplete(new TweenDelegate.TweenCallback(this.OnStateNoneAnimationComplete)).UpdateType(UpdateType.TimeScaleIndependentUpdate));
		}
	}

	private void OnStateNoneAnimationComplete()
	{
		this.mCurPopup.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(this.mCurPopup.gameObject);
		this.mButtonBlocker.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(this.mButtonBlocker.gameObject);
		this.mCurPopup = null;
		this.mButtonBlocker = null;
		this.mStateStack.Clear();
		if (this.mPopClosedCallbackEvent != null)
		{
			this.mPopClosedCallbackEvent();
		}
		if (this.mPopClosedCallbackEvent2 != null)
		{
			this.mPopClosedCallbackEvent2();
		}
	}

	private void OnEndAnimComplete()
	{
		this.mStateStack.Pop();
		GameUIPopupManager.eSTATE eState = this.mStateStack.Peek();
		this.mCurPopup.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(this.mCurPopup.gameObject);
		this.mButtonBlocker.gameObject.SetActive(false);
		UnityEngine.Object.Destroy(this.mButtonBlocker.gameObject);
		if (this.mPopClosedCallbackEvent != null)
		{
			this.mPopClosedCallbackEvent();
		}
		if (this.mPopClosedCallbackEvent2 != null)
		{
			this.mPopClosedCallbackEvent2();
		}
		this.CreatePopup(eState);
		this.mCurPopup.gameObject.SetActive(true);
		this.StartAnimation();
	}
}
