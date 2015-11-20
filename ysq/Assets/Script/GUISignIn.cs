using Att;
using Holoville.HOTween.Core;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUISignIn : GameUISession
{
	[NonSerialized]
	public List<GameObject> FlareList = new List<GameObject>();

	[NonSerialized]
	public UIScrollView mRewardScrollView;

	private UITable mRewardTable;

	private UILabel times;

	private GameObject mInstructionInfo;

	private GameObject needVIP;

	private GameObject RewardSignInPrefab;

	private List<RewardData> datas;

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		Globals.Instance.CliSession.Register(227, new ClientSession.MsgHandler(this.OnMsgSignIn));
	}

	private void CreateObjects()
	{
		GameObject parent = base.FindGameObject("WindowBG", null);
		this.times = GameUITools.FindUILabel("Times", parent);
		this.times.text = Singleton<StringManager>.Instance.GetString("signInTimes", new object[]
		{
			Globals.Instance.Player.Data.SignIn
		});
		UILabel uILabel = GameUITools.FindUILabel("Title/TitleLabel", parent);
		uILabel.text = Singleton<StringManager>.Instance.GetString("signInMonthLb");
		this.mRewardScrollView = base.FindGameObject("Rewards/RewardsPanel", parent).GetComponent<UIScrollView>();
		this.mRewardTable = base.FindGameObject("RewardsContents", this.mRewardScrollView.gameObject).GetComponent<UITable>();
		base.SetLabelLocalText("InstructionButton/RewardInstruction", "signInRewardInstruction", parent);
		this.mInstructionInfo = GameUITools.FindGameObject("SignInInstructionInformation", base.gameObject);
		base.SetLabelLocalText("Label", "signInInstructionLabel", this.mInstructionInfo);
		this.mInstructionInfo.transform.localPosition = Vector3.zero;
		base.RegisterClickEvent("BG", new UIEventListener.VoidDelegate(this.OnCloseInstructionInfoClicked), this.mInstructionInfo);
		base.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), parent);
		base.RegisterClickEvent("InstructionButton", new UIEventListener.VoidDelegate(this.OnInstructionClick), parent);
		base.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		this.InitRewardItems();
		this.mRewardScrollView.verticalScrollBar.value = (float)(Globals.Instance.Player.Data.SignIn / 5 - 1) / 3f;
		if (GameUIManager.mInstance.uiState.MaskTutorial)
		{
			GameUIManager.mInstance.uiState.MaskTutorial = false;
			this.RepositionRewardsTable();
		}
		else
		{
			GameUITools.PlayOpenWindowAnim(base.transform, new TweenDelegate.TweenCallback(this.RepositionRewardsTable), true);
		}
	}

	private void RepositionRewardsTable()
	{
		base.StartCoroutine(this.SetFlareActiveTrue());
	}

	[DebuggerHidden]
	private IEnumerator SetFlareActiveTrue()
	{
        return null;
        //GUISignIn.<SetFlareActiveTrue>c__Iterator73 <SetFlareActiveTrue>c__Iterator = new GUISignIn.<SetFlareActiveTrue>c__Iterator73();
        //<SetFlareActiveTrue>c__Iterator.<>f__this = this;
        //return <SetFlareActiveTrue>c__Iterator;
	}

	protected override void OnPreDestroyGUI()
	{
		Globals.Instance.CliSession.Unregister(227, new ClientSession.MsgHandler(this.OnMsgSignIn));
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(base.gameObject.transform, new TweenDelegate.TweenCallback(this.OnCloseAnimEnd), true);
	}

	private void OnCloseAnimEnd()
	{
		base.Close();
		Globals.Instance.TutorialMgr.InitializationCompleted(GameUIManager.mInstance.GetSession<GUIMainMenuScene>(), null);
	}

	public void OnInstructionClick(GameObject obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mInstructionInfo.SetActive(true);
		GameUITools.PlayOpenWindowAnim(this.mInstructionInfo.transform, null, true);
	}

	private void OnCloseInstructionInfoClicked(GameObject obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.mInstructionInfo.transform, new TweenDelegate.TweenCallback(this.OnCloseInstructionInfoAnimEnd), true);
	}

	private void OnCloseInstructionInfoAnimEnd()
	{
		this.mInstructionInfo.SetActive(false);
	}

	private void OnCancelBtn(GameObject obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.needVIP.transform, new TweenDelegate.TweenCallback(this.OnCancelNeedVIPAnimEnd), true);
	}

	private void OnCancelNeedVIPAnimEnd()
	{
		this.needVIP.SetActive(false);
	}

	private void OnVIPBtn(GameObject obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUITools.PlayCloseWindowAnim(this.needVIP.transform, new TweenDelegate.TweenCallback(this.OnOkNeedVIPAnimEnd), true);
	}

	private void OnOkNeedVIPAnimEnd()
	{
		this.needVIP.SetActive(false);
		base.Close();
		GameUIVip.OpenRecharge();
	}

	public void InitRewardItems()
	{
		UIWidget component = base.FindGameObject("Bg", this.mRewardScrollView.gameObject).GetComponent<UIWidget>();
		component.SetDimensions(654, 783);
		for (int i = 1; i < 31; i++)
		{
			SignInInfo info = Globals.Instance.AttDB.SignInDict.GetInfo(i);
			if (info == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("SignInDict.GetInfo error, ID = {0}", i)
				});
				return;
			}
			this.AddOneItem(info);
		}
		this.mRewardTable.repositionNow = true;
	}

	public void AddOneItem(SignInInfo info)
	{
		if (this.RewardSignInPrefab == null)
		{
			this.RewardSignInPrefab = Res.LoadGUI("GUI/RewardSignIn");
		}
		GameObject gameObject = Tools.InstantiateGUIPrefab(this.RewardSignInPrefab);
		GameUITools.AddChild(this.mRewardTable.gameObject, gameObject);
		gameObject.AddComponent<UIDragScrollView>().scrollView = this.mRewardScrollView;
		SignInRewardItem signInRewardItem = gameObject.AddComponent<SignInRewardItem>();
		signInRewardItem.InitItem(this, info);
	}

	private void RefreshItem(int index)
	{
		this.times.text = Singleton<StringManager>.Instance.GetString("signInTimes", new object[]
		{
			Globals.Instance.Player.Data.SignIn
		});
		Transform child = this.mRewardTable.transform.GetChild(index - 1);
		if (child == null)
		{
			global::Debug.LogError(new object[]
			{
				"SignInRewardItem.RefreshItem error, ID = {0}",
				index - 1
			});
			return;
		}
		child.GetComponent<SignInRewardItem>().RefreshItem();
	}

	public void FeatureCardClick()
	{
		if (this.datas != null && this.datas.Count > 0)
		{
			GUIRewardPanel.Show(this.datas, Singleton<StringManager>.Instance.GetString("signInTitle"), false, true, null, false);
		}
	}

	private void ShowSigInOK(int index, int flag)
	{
		SignInInfo info = Globals.Instance.AttDB.SignInDict.GetInfo(index);
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("SignInDict.GetInfo error, ID = {0}", index)
			});
			return;
		}
		GameAnalytics.SignInEvent(info, flag);
		this.datas = new List<RewardData>();
		switch (flag)
		{
		case 1:
			this.datas.Add(new RewardData
			{
				RewardType = info.RewardType,
				RewardValue1 = info.RewardValue1,
				RewardValue2 = info.RewardValue2
			});
			break;
		case 2:
			this.datas.Add(new RewardData
			{
				RewardType = info.RewardType,
				RewardValue1 = info.RewardValue1,
				RewardValue2 = info.RewardValue2
			});
			break;
		case 3:
			this.datas.Add(new RewardData
			{
				RewardType = info.RewardType,
				RewardValue1 = info.RewardValue1,
				RewardValue2 = info.RewardValue2
			});
			this.datas.Add(new RewardData
			{
				RewardType = info.RewardType,
				RewardValue1 = info.RewardValue1,
				RewardValue2 = info.RewardValue2
			});
			break;
		default:
			global::Debug.Log(new object[]
			{
				"MS2C_SignIn Error"
			});
			break;
		}
		if (info.RewardType == 4)
		{
			PetDataEx petByInfoID = Globals.Instance.Player.PetSystem.GetPetByInfoID(info.RewardValue1);
			if (petByInfoID == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("PetSystem.GetPetByInfoID, ID = {0}", info.RewardValue1)
				});
				return;
			}
			GetPetLayer.Show(petByInfoID, new GetPetLayer.VoidCallback(this.FeatureCardClick), GetPetLayer.EGPL_ShowNewsType.Null);
		}
		else if (this.datas != null && this.datas.Count > 0)
		{
			GUIRewardPanel.Show(this.datas, Singleton<StringManager>.Instance.GetString("signInTitle"), false, true, null, false);
		}
	}

	public void OnMsgSignIn(MemoryStream stream)
	{
		MS2C_SignIn mS2C_SignIn = Serializer.NonGeneric.Deserialize(typeof(MS2C_SignIn), stream) as MS2C_SignIn;
		if (mS2C_SignIn.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_SignIn.Result);
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_012");
		this.ShowSigInOK(mS2C_SignIn.Index, mS2C_SignIn.Flag);
		this.RefreshItem(mS2C_SignIn.Index);
	}

	public void ShowNeedVIP(int level)
	{
		this.needVIP = GameUITools.FindGameObject("NeedVIP", base.gameObject);
		UILabel uILabel = GameUITools.FindUILabel("Cancel/Label", this.needVIP);
		UILabel uILabel2 = GameUITools.FindUILabel("Vip/Label", this.needVIP);
		UILabel uILabel3 = GameUITools.FindUILabel("Label", this.needVIP);
		uILabel.text = Singleton<StringManager>.Instance.GetString("signInNeedVIPCancel");
		uILabel2.text = Singleton<StringManager>.Instance.GetString("signInOK");
		uILabel3.text = Singleton<StringManager>.Instance.GetString("signInNeedVIPText", new object[]
		{
			level
		});
		GameUITools.RegisterClickEvent("Cancel", new UIEventListener.VoidDelegate(this.OnCancelBtn), this.needVIP);
		GameUITools.RegisterClickEvent("Vip", new UIEventListener.VoidDelegate(this.OnVIPBtn), this.needVIP);
		this.needVIP.SetActive(true);
		GameUITools.PlayOpenWindowAnim(this.needVIP.transform, null, true);
	}
}
