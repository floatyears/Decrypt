using Att;
using Holoville.HOTween.Core;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GameUIMapReward : MonoBehaviour
{
	private GUIWorldMap mBaseScene;

	private GUIAwakeRoadSceneV2 mBaseScene2;

	private int mapID;

	private Transform mWinBG;

	private GameObject mCloseBtn;

	private GameUIToolTip mToolTip;

	private GameObject[] mRewardItems = new GameObject[3];

	private UIButton[] mBtnChecked = new UIButton[3];

	private UIButtonColor[] mLabelColor = new UIButtonColor[3];

	private GameObject[] mHaveReceived = new GameObject[3];

	public void Init(GUIWorldMap baseScene, int mapID)
	{
		this.mBaseScene = baseScene;
		this.mBaseScene2 = null;
		this.mapID = mapID;
		this.DoInit();
	}

	public void Init(GUIAwakeRoadSceneV2 baseScene, int mapID)
	{
		this.mBaseScene = null;
		this.mBaseScene2 = baseScene;
		this.mapID = mapID;
		this.DoInit();
	}

	private void DoInit()
	{
		this.mWinBG = base.transform.Find("winBG");
		this.mCloseBtn = this.mWinBG.Find("closeBtn").gameObject;
		MapInfo info = Globals.Instance.AttDB.MapDict.GetInfo(this.mapID);
		if (info == null)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			Transform transform = this.mWinBG.FindChild(string.Format("group{0}", i + 1));
			GameObject gameObject = transform.transform.FindChild("Item").gameObject;
			GameObject gameObject2 = GameUITools.CreateReward(info.RewardType[i], info.RewardValue1[i], info.RewardValue2[i], gameObject.transform, true, true, 0f, 0f, 0f, 255f, 255f, 255f, 0);
			if (gameObject2 == null)
			{
				global::Debug.LogErrorFormat("GameUITools.CreateReward Error {0}", new object[]
				{
					info.RewardValue1[i]
				});
			}
			this.mRewardItems[i] = gameObject2;
			transform.transform.FindChild("star0/label").GetComponent<UILabel>().text = info.NeedStar[i].ToString();
			this.mBtnChecked[i] = transform.transform.FindChild("BtnChecked").GetComponent<UIButton>();
			this.mBtnChecked[i].name = i.ToString();
			UIEventListener expr_18D = UIEventListener.Get(this.mBtnChecked[i].gameObject);
			expr_18D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_18D.onClick, new UIEventListener.VoidDelegate(this.OnGroupClicked));
			this.mLabelColor[i] = this.mBtnChecked[i].transform.FindChild("Label").GetComponent<UIButtonColor>();
			this.mHaveReceived[i] = transform.transform.FindChild("get").gameObject;
			this.RefreshBtnCheck(i, info.NeedStar[i]);
		}
		Globals.Instance.CliSession.Register(610, new ClientSession.MsgHandler(this.OnMsgTakeMapReward));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		UIEventListener expr_249 = UIEventListener.Get(this.mCloseBtn);
		expr_249.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_249.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
		UIEventListener expr_284 = UIEventListener.Get(base.transform.Find("FadeBG").gameObject);
		expr_284.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_284.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
	}

	private void OnDestroy()
	{
		UIEventListener expr_0B = UIEventListener.Get(this.mCloseBtn);
		expr_0B.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(expr_0B.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
		if (Globals.Instance == null)
		{
			return;
		}
		Globals.Instance.CliSession.Unregister(610, new ClientSession.MsgHandler(this.OnMsgTakeMapReward));
	}

	public void OnCloseBtnClicked(GameObject go)
	{
		this.CloseSelf();
	}

	private void CloseSelf()
	{
		GameUITools.PlayCloseWindowAnim(this.mWinBG, new TweenDelegate.TweenCallback(this.OnCloseAnimEnd), true);
	}

	private void OnCloseAnimEnd()
	{
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}

	private void RefreshBtnCheck(int index, int needStar)
	{
		int num = (!(this.mBaseScene != null)) ? ((!(this.mBaseScene2 != null)) ? 0 : this.mBaseScene2.totalScore) : this.mBaseScene.totalScore;
		if (num >= needStar)
		{
			if ((Globals.Instance.Player.GetMapRewardMask(this.mapID) & 1 << index) != 0)
			{
				this.mHaveReceived[index].SetActive(true);
				this.mBtnChecked[index].gameObject.SetActive(false);
			}
			else
			{
				this.mHaveReceived[index].SetActive(false);
				this.mBtnChecked[index].gameObject.SetActive(true);
				this.mBtnChecked[index].isEnabled = true;
				this.mLabelColor[index].SetState(UIButtonColor.State.Normal, false);
			}
		}
		else
		{
			this.mHaveReceived[index].SetActive(false);
			this.mBtnChecked[index].gameObject.SetActive(true);
			this.mBtnChecked[index].isEnabled = false;
			this.mLabelColor[index].SetState(UIButtonColor.State.Disabled, false);
		}
	}

	public void OnGroupClicked(GameObject go)
	{
		MC2S_TakeMapReward mC2S_TakeMapReward = new MC2S_TakeMapReward();
		mC2S_TakeMapReward.MapID = this.mapID;
		mC2S_TakeMapReward.Index = Convert.ToInt32(go.name);
		Globals.Instance.CliSession.Send(609, mC2S_TakeMapReward);
	}

	public void OnMsgTakeMapReward(MemoryStream stream)
	{
		MS2C_TakeMapReward mS2C_TakeMapReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeMapReward), stream) as MS2C_TakeMapReward;
		if (mS2C_TakeMapReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_TakeMapReward.Result);
			return;
		}
		if (mS2C_TakeMapReward.RewardType == 2 && mS2C_TakeMapReward.Value1 > 0)
		{
			GameAnalytics.OnReward((double)mS2C_TakeMapReward.Value1, GameAnalytics.VirtualCurrencyReward.MapReward);
		}
		MapInfo info = Globals.Instance.AttDB.MapDict.GetInfo(this.mapID);
		if (info == null)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			this.RefreshBtnCheck(i, info.NeedStar[i]);
		}
		if (this.mBaseScene != null)
		{
			this.mBaseScene.RefreshBoxEffect(info);
		}
		else if (this.mBaseScene2 != null)
		{
			this.mBaseScene2.RefreshBoxEffect(info);
		}
		base.StartCoroutine(this.PlayCardAnim(mS2C_TakeMapReward));
	}

	[DebuggerHidden]
	private IEnumerator PlayCardAnim(MS2C_TakeMapReward reply)
	{
        return null;
        //GameUIMapReward.<PlayCardAnim>c__IteratorA3 <PlayCardAnim>c__IteratorA = new GameUIMapReward.<PlayCardAnim>c__IteratorA3();
        //<PlayCardAnim>c__IteratorA.reply = reply;
        //<PlayCardAnim>c__IteratorA.<$>reply = reply;
        //return <PlayCardAnim>c__IteratorA;
	}
}
