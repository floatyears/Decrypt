using NtUniSdk.Unity3d;
using System;
using UnityEngine;

public class CombatMainOptionalLayer : MonoBehaviour
{
	private GameObject mGoldNumGo;

	private int mGoldNum;

	private UILabel mGoldNumTxt;

	private GameObject mChatBtn;

	private GameObject mBoxNoticePrefab;

	private TweenScale mChatTweenScale;

	private float timerRefresh;

	private int mCurState;

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mGoldNumGo = base.transform.Find("goldInfo").gameObject;
		this.mGoldNumTxt = this.mGoldNumGo.transform.FindChild("goldNum").GetComponent<UILabel>();
		this.mChatBtn = base.transform.Find("chatBtnBg").gameObject;
		GameObject gameObject = this.mChatBtn.transform.Find("chatBtn").gameObject;
		this.mChatTweenScale = gameObject.GetComponent<TweenScale>();
		UIEventListener expr_83 = UIEventListener.Get(gameObject);
		expr_83.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_83.onClick, new UIEventListener.VoidDelegate(this.OnChatBtnClicked));
		GameObject gameObject2 = base.transform.FindChild("pauseBtnBg/pauseBtn").gameObject;
		UIEventListener expr_C0 = UIEventListener.Get(gameObject2);
		expr_C0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C0.onClick, new UIEventListener.VoidDelegate(this.OnPauseBtnClicked));
		GamePadMgr.RegClickDelegate(8192, new GamePadMgr.VoidDelegate(this.ProcessPauseBtnClick));
		this.mBoxNoticePrefab = Res.LoadGUI("GUI/IngameBoxNotice");
	}

	private void OnDestroy()
	{
		GamePadMgr.UnRegClickDelegate(8192, new GamePadMgr.VoidDelegate(this.ProcessPauseBtnClick));
	}

	public void SetState(int nState)
	{
		this.mCurState = nState;
		switch (nState)
		{
		case 0:
		case 6:
			this.mGoldNumGo.SetActive(true);
			break;
		case 1:
		case 3:
		case 5:
		case 7:
			this.mGoldNumGo.SetActive(false);
			break;
		case 2:
		case 4:
		case 8:
		case 9:
			this.mGoldNumGo.SetActive(false);
			this.mChatBtn.SetActive(false);
			break;
		}
	}

	private void OnChatBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIChatWindowV2.TryShowMe();
	}

	private void OnPauseBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mCurState == 2 || this.mCurState == 8 || this.mCurState == 9)
		{
			global::Debug.Log(new object[]
			{
				"OnPauseBtnClicked"
			});
		}
		else
		{
			GameUIManager.mInstance.ShowGameUIOptionPopUp();
		}
	}

	private void ProcessPauseBtnClick()
	{
		GameUIOptionPopUp gameUIOptionPopUp;
		if ((gameUIOptionPopUp = GameUIManager.mInstance.GetGameUIOptionPopUp()) != null)
		{
			gameUIOptionPopUp.OnCancelClick(null);
		}
		else
		{
			this.OnPauseBtnClicked(null);
		}
	}

	public void OnLootMoneyEvent(ActorController actor, int money)
	{
		if (this.mBoxNoticePrefab != null)
		{
			GameObject gameObject = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, this.mBoxNoticePrefab);
			gameObject.AddComponent<UIIngameBoxNotice>().SetInfoLabel(actor, Singleton<StringManager>.Instance.GetString("lootNotice", new object[]
			{
				money
			}));
			Vector3 localPosition = gameObject.transform.localPosition;
			localPosition.z += 5000f;
			gameObject.transform.localPosition = localPosition;
		}
		this.mGoldNum += money;
		this.mGoldNumTxt.text = this.mGoldNum.ToString();
	}

	private void RefreshChatBtn()
	{
		this.mChatTweenScale.enabled = Globals.Instance.Player.ShowChatBtnAnim;
		if (!this.mChatTweenScale.enabled)
		{
			this.mChatTweenScale.transform.localScale = Vector3.one;
		}
	}

	private void Update()
	{
		if (Time.time - this.timerRefresh > 0.5f && Globals.Instance && Globals.Instance.Player != null)
		{
			this.timerRefresh = Time.time;
			this.RefreshChatBtn();
		}
	}
}
