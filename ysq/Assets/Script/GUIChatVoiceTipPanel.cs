using System;
using UnityEngine;

public class GUIChatVoiceTipPanel : MonoBehaviour
{
	public enum UITipState
	{
		ETS_State0,
		ETS_State1,
		ETS_State2
	}

	private static GUIChatVoiceTipPanel mInstance;

	private GUIChatVoiceTipPanel.UITipState mTipState;

	private UISprite mTipBg;

	private UILabel mTipDesc;

	private UISprite mYinLiangSp;

	private GameObject mRedBg;

	private string mChatTxt30Str;

	private string mChatTxt31Str;

	private string mChatTxt32Str;

	private bool mIsDestroying;

	private float mState2StartTime;

	private float mRefreshTimer;

	public static void SetState(int state)
	{
		if (GUIChatVoiceTipPanel.mInstance == null)
		{
			GUIChatVoiceTipPanel.CreateInstance();
		}
		GUIChatVoiceTipPanel.mInstance.ShowState(state);
	}

	public static void TryDestroyMe()
	{
		global::Debug.Log(new object[]
		{
			"GUIChatVoiceTipPanel.TryDestroyMe"
		});
		if (GUIChatVoiceTipPanel.mInstance != null)
		{
			GUIChatVoiceTipPanel.mInstance.DestroyMe();
		}
	}

	public static int GetState()
	{
		if (GUIChatVoiceTipPanel.mInstance == null)
		{
			return -1;
		}
		return GUIChatVoiceTipPanel.mInstance.GetTipState();
	}

	private static void CreateInstance()
	{
		if (GUIChatVoiceTipPanel.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/chatVoiceTipPanel");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/chatVoiceTipPanel error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIPetTypeInfoPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 1000f);
		GUIChatVoiceTipPanel.mInstance = gameObject2.AddComponent<GUIChatVoiceTipPanel>();
	}

	public int GetTipState()
	{
		return (int)this.mTipState;
	}

	private void Awake()
	{
		this.mTipBg = base.transform.Find("tipBg").GetComponent<UISprite>();
		this.mTipDesc = base.transform.Find("txt").GetComponent<UILabel>();
		this.mYinLiangSp = base.transform.Find("yinLiang").GetComponent<UISprite>();
		this.mYinLiangSp.gameObject.SetActive(false);
		this.mRedBg = base.transform.Find("redBg").gameObject;
		this.mRedBg.SetActive(false);
		this.mChatTxt30Str = Singleton<StringManager>.Instance.GetString("chatTxt30");
		this.mChatTxt31Str = Singleton<StringManager>.Instance.GetString("chatTxt31");
		this.mChatTxt32Str = Singleton<StringManager>.Instance.GetString("chatTxt32");
		this.mRefreshTimer = Time.time;
	}

	private void OnEnterState0()
	{
		this.mTipBg.spriteName = "tipBg0";
		this.mTipBg.MakePixelPerfect();
		this.mTipDesc.text = this.mChatTxt30Str;
		this.mYinLiangSp.gameObject.SetActive(true);
		this.mYinLiangSp.fillAmount = 0f;
		this.mRedBg.SetActive(false);
	}

	private void OnEnterState1()
	{
		this.mTipBg.spriteName = "tipBg1";
		this.mTipBg.MakePixelPerfect();
		this.mTipDesc.text = this.mChatTxt31Str;
		this.mYinLiangSp.gameObject.SetActive(false);
		this.mRedBg.SetActive(true);
	}

	private void OnEnterState2()
	{
		this.mTipBg.spriteName = "tipBg2";
		this.mTipBg.MakePixelPerfect();
		this.mTipDesc.text = this.mChatTxt32Str;
		this.mYinLiangSp.gameObject.SetActive(false);
		this.mRedBg.SetActive(false);
		this.mState2StartTime = Time.time;
	}

	public void DestroyMe()
	{
		this.mIsDestroying = true;
		if (GUIChatVoiceTipPanel.mInstance != null)
		{
			UnityEngine.Object.Destroy(GUIChatVoiceTipPanel.mInstance.gameObject);
			GUIChatVoiceTipPanel.mInstance = null;
		}
	}

	public void ShowState(int state)
	{
		global::Debug.Log(new object[]
		{
			"GUIChatVoiceTipPanel.ShowState:" + state
		});
		this.mTipState = (GUIChatVoiceTipPanel.UITipState)state;
		if (state == -1)
		{
			this.DestroyMe();
		}
		else
		{
			if (!base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(true);
			}
			if (state == 1)
			{
				this.OnEnterState1();
			}
			else if (state == 2)
			{
				this.OnEnterState2();
			}
			else
			{
				this.OnEnterState0();
			}
		}
	}

	private void Update()
	{
		if (Time.time - this.mRefreshTimer >= 0.1f && this.mTipState == GUIChatVoiceTipPanel.UITipState.ETS_State0)
		{
			this.mYinLiangSp.fillAmount = Mathf.Clamp01(Globals.Instance.VoiceMgr.GetPowerForRecordF());
		}
		if (this.mTipState == GUIChatVoiceTipPanel.UITipState.ETS_State2 && base.gameObject.activeInHierarchy && !this.mIsDestroying && Time.time - this.mState2StartTime > 1f)
		{
			this.DestroyMe();
		}
	}
}
