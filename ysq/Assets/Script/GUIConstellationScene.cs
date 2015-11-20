using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIConstellationScene : GameUISession
{
	public GUIRightInfo mGUIRightInfo;

	private GUILeftInfo mGUILeftInfo;

	public GUIXingZuoPage mGUIXingZuoPage;

	private int mconLv;

	private GameObject mEffect;

	private GUIAttributeTip mGUIAttributeTip;

	private void CreateObjects()
	{
		this.mEffect = base.transform.Find("ui64").gameObject;
		this.mEffect.AddComponent<GameRenderQueue>().renderQueue = 3050;
		Transform transform = base.transform.Find("UIMiddle");
		this.mGUIRightInfo = transform.Find("Panel/infoBgRight").gameObject.AddComponent<GUIRightInfo>();
		this.mGUIRightInfo.InitWithBaseScene(this);
		this.mGUILeftInfo = transform.Find("Panel/infoBgLeft").gameObject.AddComponent<GUILeftInfo>();
		this.mGUILeftInfo.InitWithBaseScene();
		this.mGUIXingZuoPage = transform.Find("Panel/xingZuoPage").gameObject.AddComponent<GUIXingZuoPage>();
		this.mGUIXingZuoPage.InitWithBaseScene(this);
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		this.mGUIXingZuoPage.mUIXingZuoItem.mWaitTimeToHide = 0f;
		this.mconLv = Globals.Instance.Player.Data.ConstellationLevel;
		this.mGUIXingZuoPage.Refresh(this.mconLv);
		this.mGUIRightInfo.Refresh(this.mconLv);
		this.mGUILeftInfo.Refresh(this.mconLv);
		GameUIManager.mInstance.GetTopGoods().Show("xingZuoLb");
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		Globals.Instance.CliSession.Register(204, new ClientSession.MsgHandler(this.OnMsgConstellationLevelup));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		GameUIManager.mInstance.GetTopGoods().Hide();
		if (this.mGUIAttributeTip != null)
		{
			this.mGUIAttributeTip.DestroySelf();
		}
		Globals.Instance.CliSession.Unregister(204, new ClientSession.MsgHandler(this.OnMsgConstellationLevelup));
	}

	public void OnMsgConstellationLevelup(MemoryStream stream)
	{
		MS2C_ConstellationLevelup mS2C_ConstellationLevelup = Serializer.NonGeneric.Deserialize(typeof(MS2C_ConstellationLevelup), stream) as MS2C_ConstellationLevelup;
		if (mS2C_ConstellationLevelup.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_ConstellationLevelup.Result);
			return;
		}
		this.mconLv = Globals.Instance.Player.Data.ConstellationLevel;
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		this.mGUIXingZuoPage.mUIXingZuoItem.mWaitTimeToHide = 1.1f;
		this.mGUIXingZuoPage.mUIXingZuoItem.RefreshShowIcon();
		this.mGUIRightInfo.Refresh(this.mconLv);
		this.mGUIXingZuoPage.mUIXingZuoItem.RefreshEffect();
		base.StartCoroutine(this.WaitShowAttribute());
		if (Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(103)) >= GUIRightInfo.GetCost())
		{
			base.StartCoroutine(this.EffectSound());
		}
		int constellationLevel = Globals.Instance.Player.Data.ConstellationLevel;
		if (constellationLevel == 10 || constellationLevel == 30)
		{
			GameUIManager.mInstance.ShowPetQualityUp(constellationLevel);
		}
		if (constellationLevel > 0 && constellationLevel % 5 == 0 && (constellationLevel != 10 & constellationLevel != 30))
		{
			base.StartCoroutine(this.WaitShowBaoXiang());
		}
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	[DebuggerHidden]
	private IEnumerator WaitShowAttribute()
	{
        return null;
        //GUIConstellationScene.<WaitShowAttribute>c__Iterator3E <WaitShowAttribute>c__Iterator3E = new GUIConstellationScene.<WaitShowAttribute>c__Iterator3E();
        //<WaitShowAttribute>c__Iterator3E.<>f__this = this;
        //return <WaitShowAttribute>c__Iterator3E;
	}

	[DebuggerHidden]
	private IEnumerator WaitShowBaoXiang()
	{
        return null;
        //return new GUIConstellationScene.<WaitShowBaoXiang>c__Iterator3F();
	}

	[DebuggerHidden]
	private IEnumerator EffectSound()
	{
        return null;
        //return new GUIConstellationScene.<EffectSound>c__Iterator40();
	}
}
