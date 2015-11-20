using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIGuildCraftTeamInfoPop : MonoBehaviour
{
	private static GUIGuildCraftTeamInfoPop mInstance;

	private GameObject mTopCDGo;

	private UILabel mTopCDLb;

	private UILabel mTopCDDesc;

	private UILabel mTopOneLb;

	private UILabel mTwo1Lb;

	private UILabel mTwo2Lb;

	private UILabel mFour1Lb;

	private UILabel mFour2Lb;

	private UILabel mFour3Lb;

	private UILabel mFour4Lb;

	private UIButton mTwo1Sp;

	private UIButton mTwo2Sp;

	private UISprite mFour1Sp;

	private UISprite mFour2Sp;

	private UISprite mFour3Sp;

	private UISprite mFour4Sp;

	private UILabel mTipTxt;

	private GameObject mBattleMark41;

	private GameObject mBattleMark42;

	private GameObject mBattleMarkTopOne;

	private UISprite mX0;

	private UISprite mX1;

	private UISprite mY0;

	private UISprite mY1;

	private UISprite mY2;

	private UISprite mZ0;

	private UISprite mZ1;

	private UISprite mZ2;

	private GameObject mTopEffectGo;

	private GameObject mTwo1EffectGo;

	private GameObject mTwo2EffectGo;

	private GameObject mTopLiuGo;

	private GameObject mTwo1LiuGo;

	private GameObject mTwo2LiuGo;

	private GameObject mJinBeiEffectGo;

	private string mGuildCraft10Str;

	private string mGuildCraft11Str;

	private string mGuildCraft12Str;

	private string mGuildCraft13Str;

	private string mGuildCraft14Str;

	private string mGuildCraft16Str;

	private string mGuildCraft17Str;

	private string mGuildCraft20Str;

	private string mGuildCraft21Str;

	private string mGuildCraft69Str;

	private float mRefreshTimer;

	private StringBuilder mSb = new StringBuilder(42);

	public static void ShowMe()
	{
		if (GUIGuildCraftTeamInfoPop.mInstance == null)
		{
			GUIGuildCraftTeamInfoPop.CreateInstance();
		}
		GUIGuildCraftTeamInfoPop.mInstance.Refresh();
	}

	public static void CloseMe()
	{
		if (GUIGuildCraftTeamInfoPop.mInstance != null)
		{
			UnityEngine.Object.Destroy(GUIGuildCraftTeamInfoPop.mInstance.gameObject);
			GUIGuildCraftTeamInfoPop.mInstance = null;
		}
	}

	private static void CreateInstance()
	{
		if (GUIGuildCraftTeamInfoPop.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIGuildCraftTeamInfoPop");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIGuildCraftTeamInfoPop error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIGuildCraftTeamInfoPop error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 1000f);
		GUIGuildCraftTeamInfoPop.mInstance = gameObject2.AddComponent<GUIGuildCraftTeamInfoPop>();
	}

	private void Awake()
	{
		this.CreateObjects();
		this.mRefreshTimer = Time.time;
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		GuildSubSystem expr_20 = Globals.Instance.Player.GuildSystem;
		expr_20.GetWarStateInfoEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_20.GetWarStateInfoEvent, new GuildSubSystem.VoidCallback(this.OnGetWarStateInfoEvent));
		GuildSubSystem expr_50 = Globals.Instance.Player.GuildSystem;
		expr_50.GuildWarEnterEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_50.GuildWarEnterEvent, new GuildSubSystem.VoidCallback(this.OnGuildWarEnter));
		GuildSubSystem expr_80 = Globals.Instance.Player.GuildSystem;
		expr_80.CastleUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_80.CastleUpdateEvent, new GuildSubSystem.VoidCallback(this.OnCastleUpdate));
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBG");
		GameObject gameObject = transform.Find("closeBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mJinBeiEffectGo = transform.Find("jinBei/ui86").gameObject;
		NGUITools.SetActive(this.mJinBeiEffectGo, true);
		Tools.SetParticleRenderQueue2(this.mJinBeiEffectGo, 3450);
		this.mTopCDGo = transform.Find("time").gameObject;
		this.mTopCDLb = transform.Find("time").GetComponent<UILabel>();
		this.mTopCDDesc = this.mTopCDLb.transform.Find("txt").GetComponent<UILabel>();
		GameObject gameObject2 = transform.Find("topOne").gameObject;
		UIEventListener expr_DE = UIEventListener.Get(gameObject2);
		expr_DE.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_DE.onClick, new UIEventListener.VoidDelegate(this.OnTopOneClick));
		this.mTopOneLb = gameObject2.transform.Find("Label").GetComponent<UILabel>();
		this.mTopEffectGo = gameObject2.transform.Find("ui85").gameObject;
		NGUITools.SetActive(this.mTopEffectGo, false);
		Tools.SetParticleRenderQueue2(this.mTopEffectGo, 3450);
		this.mTopLiuGo = gameObject2.transform.Find("Effect").gameObject;
		this.mTopLiuGo.SetActive(false);
		GameObject gameObject3 = transform.Find("two1").gameObject;
		this.mTwo1Sp = gameObject3.GetComponent<UIButton>();
		UIEventListener expr_19B = UIEventListener.Get(gameObject3);
		expr_19B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_19B.onClick, new UIEventListener.VoidDelegate(this.OnTwo1Click));
		this.mTwo1Lb = gameObject3.transform.Find("Label").GetComponent<UILabel>();
		this.mTwo1EffectGo = gameObject3.transform.Find("ui85").gameObject;
		Tools.SetParticleRenderQueue2(this.mTwo1EffectGo, 3450);
		NGUITools.SetActive(this.mTwo1EffectGo, false);
		this.mTwo1LiuGo = gameObject3.transform.Find("Effect").gameObject;
		this.mTwo1LiuGo.SetActive(false);
		GameObject gameObject4 = transform.Find("two2").gameObject;
		this.mTwo2Sp = gameObject4.GetComponent<UIButton>();
		UIEventListener expr_25B = UIEventListener.Get(gameObject4);
		expr_25B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_25B.onClick, new UIEventListener.VoidDelegate(this.OnTwo2Click));
		this.mTwo2Lb = gameObject4.transform.Find("Label").GetComponent<UILabel>();
		this.mTwo2EffectGo = gameObject4.transform.Find("ui85").gameObject;
		Tools.SetParticleRenderQueue2(this.mTwo2EffectGo, 3450);
		NGUITools.SetActive(this.mTwo2EffectGo, false);
		this.mTwo2LiuGo = gameObject4.transform.Find("Effect").gameObject;
		this.mTwo2LiuGo.SetActive(false);
		GameObject gameObject5 = transform.Find("four1").gameObject;
		this.mFour1Sp = gameObject5.GetComponent<UISprite>();
		this.mFour1Lb = gameObject5.transform.Find("Label").GetComponent<UILabel>();
		GameObject gameObject6 = transform.Find("four2").gameObject;
		this.mFour2Sp = gameObject6.GetComponent<UISprite>();
		this.mFour2Lb = gameObject6.transform.Find("Label").GetComponent<UILabel>();
		GameObject gameObject7 = transform.Find("four3").gameObject;
		this.mFour3Sp = gameObject7.GetComponent<UISprite>();
		this.mFour3Lb = gameObject7.transform.Find("Label").GetComponent<UILabel>();
		GameObject gameObject8 = transform.Find("four4").gameObject;
		this.mFour4Sp = gameObject8.GetComponent<UISprite>();
		this.mFour4Lb = gameObject8.transform.Find("Label").GetComponent<UILabel>();
		this.mBattleMarkTopOne = transform.Find("topOneMark").gameObject;
		this.mBattleMarkTopOne.SetActive(false);
		this.mBattleMark41 = transform.Find("four1Mark").gameObject;
		this.mBattleMark41.SetActive(false);
		this.mBattleMark42 = transform.Find("four2Mark").gameObject;
		this.mBattleMark42.SetActive(false);
		this.mTipTxt = transform.Find("tipTxt").GetComponent<UILabel>();
		this.mX0 = transform.Find("x0").GetComponent<UISprite>();
		this.mX1 = transform.Find("x1").GetComponent<UISprite>();
		this.mY0 = transform.Find("y0").GetComponent<UISprite>();
		this.mY1 = transform.Find("y1").GetComponent<UISprite>();
		this.mY2 = transform.Find("y2").GetComponent<UISprite>();
		this.mZ0 = transform.Find("z0").GetComponent<UISprite>();
		this.mZ1 = transform.Find("z1").GetComponent<UISprite>();
		this.mZ2 = transform.Find("z2").GetComponent<UISprite>();
		this.mGuildCraft10Str = Singleton<StringManager>.Instance.GetString("guildCraft10");
		this.mGuildCraft11Str = Singleton<StringManager>.Instance.GetString("guildCraft11");
		this.mGuildCraft12Str = Singleton<StringManager>.Instance.GetString("guildCraft12");
		this.mGuildCraft13Str = Singleton<StringManager>.Instance.GetString("guildCraft13");
		this.mGuildCraft14Str = Singleton<StringManager>.Instance.GetString("guildCraft14");
		this.mGuildCraft16Str = Singleton<StringManager>.Instance.GetString("guildCraft16");
		this.mGuildCraft17Str = Singleton<StringManager>.Instance.GetString("guildCraft17");
		this.mGuildCraft20Str = Singleton<StringManager>.Instance.GetString("guildCraft20");
		this.mGuildCraft21Str = Singleton<StringManager>.Instance.GetString("guildCraft21");
		this.mGuildCraft69Str = Singleton<StringManager>.Instance.GetString("guildCraft69");
		GuildSubSystem expr_5F1 = Globals.Instance.Player.GuildSystem;
		expr_5F1.GetWarStateInfoEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_5F1.GetWarStateInfoEvent, new GuildSubSystem.VoidCallback(this.OnGetWarStateInfoEvent));
		GuildSubSystem expr_621 = Globals.Instance.Player.GuildSystem;
		expr_621.GuildWarEnterEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_621.GuildWarEnterEvent, new GuildSubSystem.VoidCallback(this.OnGuildWarEnter));
		GuildSubSystem expr_651 = Globals.Instance.Player.GuildSystem;
		expr_651.CastleUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_651.CastleUpdateEvent, new GuildSubSystem.VoidCallback(this.OnCastleUpdate));
	}

	private void SetEffectDisable()
	{
		NGUITools.SetActive(this.mTopEffectGo, false);
		NGUITools.SetActive(this.mTwo1EffectGo, false);
		NGUITools.SetActive(this.mTwo2EffectGo, false);
		NGUITools.SetActive(this.mTopLiuGo, false);
		NGUITools.SetActive(this.mTwo1LiuGo, false);
		NGUITools.SetActive(this.mTwo2LiuGo, false);
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GUIGuildCraftTeamInfoPop.CloseMe();
	}

	private bool SlefCanCanZhan()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return false;
		}
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null && (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour1 || guildWarClient.WarID == EGuildWarId.EGWI_FinalFour2) && (iD == guildWarClient.Red.GuildID || iD == guildWarClient.Blue.GuildID))
			{
				return true;
			}
		}
		return false;
	}

	private void RequestWarEnter(GuildWarClient wClient)
	{
		MC2S_GuildWarEnter mC2S_GuildWarEnter = new MC2S_GuildWarEnter();
		mC2S_GuildWarEnter.WarID = wClient.WarID;
		Globals.Instance.CliSession.Send(979, mC2S_GuildWarEnter);
	}

	private void OnTopOneClick(GameObject go)
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
		{
			for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
			{
				GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
				if (guildWarClient != null && guildWarClient.WarID == EGuildWarId.EGWI_Final)
				{
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						return;
					}
					Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
					if (this.IsTwoTeamFull(guildWarClient))
					{
						if (this.IsSelfTwoTeamFull(guildWarClient))
						{
							this.RequestWarEnter(guildWarClient);
						}
						else
						{
							GameUIManager.mInstance.ShowMessageTipByKey("guildCraft50", 0f, 0f);
						}
					}
					else
					{
						GameUIManager.mInstance.ShowMessageTipByKey("guildCraft69", 0f, 0f);
					}
				}
			}
		}
		else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
		{
			for (int j = 0; j < mWarStateInfo.mWarDatas.Count; j++)
			{
				GuildWarClient guildWarClient2 = mWarStateInfo.mWarDatas[j];
				if (guildWarClient2 != null && guildWarClient2.WarID == EGuildWarId.EGWI_Final)
				{
					if (guildWarClient2.Red.GuildID == 0uL && guildWarClient2.Blue.GuildID == 0uL)
					{
						return;
					}
					Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
					if (this.IsTwoTeamFull(guildWarClient2))
					{
						if (this.IsSelfTwoTeamFull(guildWarClient2))
						{
							this.RequestWarEnter(guildWarClient2);
						}
						else
						{
							this.RequestWarEnter(guildWarClient2);
						}
					}
					else
					{
						GameUIManager.mInstance.ShowMessageTipByKey("guildCraft69", 0f, 0f);
					}
				}
			}
		}
		else if (mWarStateInfo.mWarState != EGuildWarState.EGWS_FinalEnd)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guildCraft60", 0f, 0f);
		}
	}

	private void OnTwo1Click(GameObject go)
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare)
		{
			for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
			{
				GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
				if (guildWarClient != null && guildWarClient.WarID == EGuildWarId.EGWI_FinalFour1)
				{
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						return;
					}
					Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
					if (this.IsTwoTeamFull(guildWarClient))
					{
						if (this.IsSelfTwoTeamFull(guildWarClient))
						{
							this.RequestWarEnter(guildWarClient);
						}
						else
						{
							GameUIManager.mInstance.ShowMessageTipByKey("guildCraft50", 0f, 0f);
						}
					}
					else
					{
						GameUIManager.mInstance.ShowMessageTipByKey("guildCraft70", 0f, 0f);
					}
				}
			}
		}
		else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing)
		{
			for (int j = 0; j < mWarStateInfo.mWarDatas.Count; j++)
			{
				GuildWarClient guildWarClient2 = mWarStateInfo.mWarDatas[j];
				if (guildWarClient2 != null && guildWarClient2.WarID == EGuildWarId.EGWI_FinalFour1)
				{
					if (guildWarClient2.Red.GuildID == 0uL && guildWarClient2.Blue.GuildID == 0uL)
					{
						return;
					}
					Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
					if (this.IsTwoTeamFull(guildWarClient2))
					{
						if (this.IsSelfTwoTeamFull(guildWarClient2))
						{
							this.RequestWarEnter(guildWarClient2);
						}
						else if (!this.SlefCanCanZhan())
						{
							this.RequestWarEnter(guildWarClient2);
						}
						else
						{
							GameUIManager.mInstance.ShowMessageTip("EGR", 117);
						}
					}
					else
					{
						GameUIManager.mInstance.ShowMessageTipByKey("guildCraft70", 0f, 0f);
					}
				}
			}
		}
		else if (mWarStateInfo.mWarState != EGuildWarState.EGWS_FinalFourEnd)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guildCraft60", 0f, 0f);
		}
	}

	private void OnTwo2Click(GameObject go)
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare)
		{
			for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
			{
				GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
				if (guildWarClient != null && guildWarClient.WarID == EGuildWarId.EGWI_FinalFour2)
				{
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						return;
					}
					Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
					if (this.IsTwoTeamFull(guildWarClient))
					{
						if (this.IsSelfTwoTeamFull(guildWarClient))
						{
							this.RequestWarEnter(guildWarClient);
						}
						else
						{
							GameUIManager.mInstance.ShowMessageTipByKey("guildCraft50", 0f, 0f);
						}
					}
					else
					{
						GameUIManager.mInstance.ShowMessageTipByKey("guildCraft70", 0f, 0f);
					}
				}
			}
		}
		else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing)
		{
			for (int j = 0; j < mWarStateInfo.mWarDatas.Count; j++)
			{
				GuildWarClient guildWarClient2 = mWarStateInfo.mWarDatas[j];
				if (guildWarClient2 != null && guildWarClient2.WarID == EGuildWarId.EGWI_FinalFour2)
				{
					if (guildWarClient2.Red.GuildID == 0uL && guildWarClient2.Blue.GuildID == 0uL)
					{
						return;
					}
					Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
					if (this.IsTwoTeamFull(guildWarClient2))
					{
						if (this.IsSelfTwoTeamFull(guildWarClient2))
						{
							this.RequestWarEnter(guildWarClient2);
						}
						else if (!this.SlefCanCanZhan())
						{
							this.RequestWarEnter(guildWarClient2);
						}
						else
						{
							GameUIManager.mInstance.ShowMessageTip("EGR", 117);
						}
					}
					else
					{
						GameUIManager.mInstance.ShowMessageTipByKey("guildCraft70", 0f, 0f);
					}
				}
			}
		}
		else if (mWarStateInfo.mWarState != EGuildWarState.EGWS_FinalFourEnd)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guildCraft60", 0f, 0f);
		}
	}

	private bool IsSelfBattleOneEmpty()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return false;
		}
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null && guildWarClient.WarID == EGuildWarId.EGWI_Final && ((iD == guildWarClient.Red.GuildID && guildWarClient.Blue.GuildID == 0uL) || (iD == guildWarClient.Blue.GuildID && guildWarClient.Red.GuildID == 0uL)))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsSelfBattleFourEmpty()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return false;
		}
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null && (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour1 || guildWarClient.WarID == EGuildWarId.EGWI_FinalFour2) && ((iD == guildWarClient.Red.GuildID && guildWarClient.Blue.GuildID == 0uL) || (iD == guildWarClient.Blue.GuildID && guildWarClient.Red.GuildID == 0uL)))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsCanJoinBattleFour()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return false;
		}
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null && (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour1 || guildWarClient.WarID == EGuildWarId.EGWI_FinalFour2) && (iD == guildWarClient.Red.GuildID || iD == guildWarClient.Blue.GuildID))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsCanJoinBattleOne()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return false;
		}
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null && guildWarClient.WarID == EGuildWarId.EGWI_Final && (iD == guildWarClient.Red.GuildID || iD == guildWarClient.Blue.GuildID))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsTwoTeamFull(GuildWarClient wClient)
	{
		return wClient != null && wClient.Red.GuildID != 0uL && wClient.Blue.GuildID != 0uL;
	}

	private bool IsSelfTwoTeamFull(GuildWarClient wClient)
	{
		if (wClient == null)
		{
			return false;
		}
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		return this.IsTwoTeamFull(wClient) && (iD == wClient.Red.GuildID || iD == wClient.Blue.GuildID);
	}

	private void OnEnterStateSelectFourTeam()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		this.mTopCDDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft52");
		this.mTwo1Sp.hoverSprite = "nameBg1";
		this.mTwo1Sp.normalSprite = "nameBg1";
		this.mTwo1Sp.pressedSprite = "nameBg1";
		this.mTwo2Sp.hoverSprite = "nameBg1";
		this.mTwo2Sp.normalSprite = "nameBg1";
		this.mTwo2Sp.pressedSprite = "nameBg1";
		this.mFour1Sp.spriteName = "nameBg1";
		this.mFour2Sp.spriteName = "nameBg1";
		this.mFour3Sp.spriteName = "nameBg1";
		this.mFour4Sp.spriteName = "nameBg1";
		this.mX0.spriteName = "wan0";
		this.mX1.spriteName = "wan0";
		this.mY0.spriteName = "zhi0";
		this.mY1.spriteName = "wan0";
		this.mY2.spriteName = "wan0";
		this.mZ0.spriteName = "zhi0";
		this.mZ1.spriteName = "wan0";
		this.mZ2.spriteName = "wan0";
		this.mTopOneLb.text = string.Empty;
		this.mBattleMarkTopOne.SetActive(false);
		this.mTwo1Lb.text = string.Empty;
		this.mBattleMark41.SetActive(false);
		this.mTwo2Lb.text = string.Empty;
		this.mBattleMark42.SetActive(false);
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null)
			{
				if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour1)
				{
					this.mFour1Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour1Lb.color = Color.green;
					}
					else
					{
						this.mFour1Lb.color = Color.white;
					}
					this.mFour2Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour2Lb.color = Color.green;
					}
					else
					{
						this.mFour2Lb.color = Color.white;
					}
				}
				else if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour2)
				{
					this.mFour3Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour3Lb.color = Color.green;
					}
					else
					{
						this.mFour3Lb.color = Color.white;
					}
					this.mFour4Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour4Lb.color = Color.green;
					}
					else
					{
						this.mFour4Lb.color = Color.white;
					}
				}
			}
		}
		this.mSb.Remove(0, this.mSb.Length).Append(this.mGuildCraft10Str);
		if (this.IsCanJoinBattleFour())
		{
			this.mSb.Append(this.mGuildCraft11Str);
			if (this.IsSelfBattleFourEmpty())
			{
				this.mSb.AppendLine().Append(this.mGuildCraft69Str);
			}
			else
			{
				this.mSb.AppendLine().Append(this.mGuildCraft12Str);
			}
		}
		else
		{
			this.mSb.Append(this.mGuildCraft13Str).AppendLine().Append(this.mGuildCraft14Str);
		}
		this.mTipTxt.gameObject.SetActive(true);
		this.mTipTxt.text = this.mSb.ToString();
	}

	private void OnEnterStateFinalFourPrepare()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		this.mTopCDDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft53");
		this.mTwo1Sp.hoverSprite = "nameBg1";
		this.mTwo1Sp.normalSprite = "nameBg1";
		this.mTwo1Sp.pressedSprite = "nameBg1";
		this.mTwo2Sp.hoverSprite = "nameBg1";
		this.mTwo2Sp.normalSprite = "nameBg1";
		this.mTwo2Sp.pressedSprite = "nameBg1";
		this.mFour1Sp.spriteName = "nameBg1";
		this.mFour2Sp.spriteName = "nameBg1";
		this.mFour3Sp.spriteName = "nameBg1";
		this.mFour4Sp.spriteName = "nameBg1";
		this.mX0.spriteName = "wan0";
		this.mX1.spriteName = "wan0";
		this.mY0.spriteName = "zhi0";
		this.mY1.spriteName = "wan0";
		this.mY2.spriteName = "wan0";
		this.mZ0.spriteName = "zhi0";
		this.mZ1.spriteName = "wan0";
		this.mZ2.spriteName = "wan0";
		this.mTopOneLb.text = string.Empty;
		this.mBattleMarkTopOne.SetActive(false);
		this.mBattleMark41.SetActive(false);
		this.mBattleMark42.SetActive(false);
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null)
			{
				if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour1)
				{
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTwo1Lb.text = string.Empty;
						this.mBattleMark41.SetActive(false);
					}
					else
					{
						if (this.IsSelfTwoTeamFull(guildWarClient))
						{
							this.mTwo1Lb.text = this.mGuildCraft16Str;
							NGUITools.SetActive(this.mTwo1EffectGo, false);
							NGUITools.SetActive(this.mTwo1EffectGo, true);
						}
						else
						{
							this.mTwo1Lb.text = string.Empty;
						}
						this.mBattleMark41.SetActive(true);
					}
					this.mFour1Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour1Lb.color = Color.green;
					}
					else
					{
						this.mFour1Lb.color = Color.white;
					}
					this.mFour2Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour2Lb.color = Color.green;
					}
					else
					{
						this.mFour2Lb.color = Color.white;
					}
				}
				else if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour2)
				{
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTwo2Lb.text = string.Empty;
						this.mBattleMark42.SetActive(false);
					}
					else
					{
						if (this.IsSelfTwoTeamFull(guildWarClient))
						{
							this.mTwo2Lb.text = this.mGuildCraft16Str;
							NGUITools.SetActive(this.mTwo2EffectGo, false);
							NGUITools.SetActive(this.mTwo2EffectGo, true);
						}
						else
						{
							this.mTwo2Lb.text = string.Empty;
						}
						this.mBattleMark42.SetActive(true);
					}
					this.mFour3Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour3Lb.color = Color.green;
					}
					else
					{
						this.mFour3Lb.color = Color.white;
					}
					this.mFour4Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour4Lb.color = Color.green;
					}
					else
					{
						this.mFour4Lb.color = Color.white;
					}
				}
			}
		}
		this.mSb.Remove(0, this.mSb.Length).Append(this.mGuildCraft10Str);
		if (this.IsCanJoinBattleFour())
		{
			this.mSb.Append(this.mGuildCraft11Str);
			if (this.IsSelfBattleFourEmpty())
			{
				this.mSb.AppendLine().Append(this.mGuildCraft69Str);
			}
			else
			{
				this.mSb.AppendLine().Append(this.mGuildCraft12Str);
			}
		}
		else
		{
			this.mSb.Append(this.mGuildCraft13Str).AppendLine().Append(this.mGuildCraft14Str);
		}
		this.mTipTxt.gameObject.SetActive(true);
		this.mTipTxt.text = this.mSb.ToString();
	}

	private void OnEnterStateFinalFourGoing()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		this.mTopCDDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft54");
		this.mTwo1Sp.hoverSprite = "nameBg1";
		this.mTwo1Sp.normalSprite = "nameBg1";
		this.mTwo1Sp.pressedSprite = "nameBg1";
		this.mTwo2Sp.hoverSprite = "nameBg1";
		this.mTwo2Sp.normalSprite = "nameBg1";
		this.mTwo2Sp.pressedSprite = "nameBg1";
		this.mFour1Sp.spriteName = "nameBg1";
		this.mFour2Sp.spriteName = "nameBg1";
		this.mFour3Sp.spriteName = "nameBg1";
		this.mFour4Sp.spriteName = "nameBg1";
		this.mX0.spriteName = "wan0";
		this.mX1.spriteName = "wan0";
		this.mY0.spriteName = "zhi0";
		this.mY1.spriteName = "wan0";
		this.mY2.spriteName = "wan0";
		this.mZ0.spriteName = "zhi0";
		this.mZ1.spriteName = "wan0";
		this.mZ2.spriteName = "wan0";
		this.mTopOneLb.text = string.Empty;
		this.mBattleMarkTopOne.SetActive(false);
		this.mBattleMark41.SetActive(false);
		this.mBattleMark42.SetActive(false);
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null)
			{
				if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour1)
				{
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTwo1Lb.text = string.Empty;
						this.mBattleMark41.SetActive(false);
					}
					else
					{
						if (this.IsTwoTeamFull(guildWarClient))
						{
							if (this.IsSelfTwoTeamFull(guildWarClient))
							{
								this.mTwo1Lb.text = this.mGuildCraft16Str;
								NGUITools.SetActive(this.mTwo1EffectGo, false);
								NGUITools.SetActive(this.mTwo1EffectGo, true);
							}
							else
							{
								this.mTwo1Lb.text = this.mGuildCraft17Str;
								this.mTwo1LiuGo.SetActive(true);
							}
						}
						else
						{
							this.mTwo1Lb.text = string.Empty;
						}
						this.mBattleMark41.SetActive(true);
					}
					this.mFour1Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour1Lb.color = Color.green;
					}
					else
					{
						this.mFour1Lb.color = Color.white;
					}
					this.mFour2Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour2Lb.color = Color.green;
					}
					else
					{
						this.mFour2Lb.color = Color.white;
					}
				}
				else if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour2)
				{
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTwo2Lb.text = string.Empty;
						this.mBattleMark42.SetActive(false);
					}
					else
					{
						if (this.IsTwoTeamFull(guildWarClient))
						{
							if (this.IsSelfTwoTeamFull(guildWarClient))
							{
								this.mTwo2Lb.text = this.mGuildCraft16Str;
								NGUITools.SetActive(this.mTwo2EffectGo, false);
								NGUITools.SetActive(this.mTwo2EffectGo, true);
							}
							else
							{
								this.mTwo2Lb.text = this.mGuildCraft17Str;
								this.mTwo2LiuGo.SetActive(true);
							}
						}
						else
						{
							this.mTwo2Lb.text = string.Empty;
						}
						this.mBattleMark42.SetActive(true);
					}
					this.mFour3Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour3Lb.color = Color.green;
					}
					else
					{
						this.mFour3Lb.color = Color.white;
					}
					this.mFour4Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour4Lb.color = Color.green;
					}
					else
					{
						this.mFour4Lb.color = Color.white;
					}
				}
			}
		}
		this.mSb.Remove(0, this.mSb.Length).Append(this.mGuildCraft10Str);
		if (this.IsCanJoinBattleFour())
		{
			this.mSb.Append(this.mGuildCraft11Str);
			if (this.IsSelfBattleFourEmpty())
			{
				this.mSb.AppendLine().Append(this.mGuildCraft69Str);
			}
			else
			{
				this.mSb.AppendLine().Append(this.mGuildCraft12Str);
			}
		}
		else
		{
			this.mSb.Append(this.mGuildCraft13Str).AppendLine().Append(this.mGuildCraft14Str);
		}
		this.mTipTxt.gameObject.SetActive(true);
		this.mTipTxt.text = this.mSb.ToString();
	}

	private void OnEnterStateFinalFourEnd()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		this.mTopCDDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft55");
		this.mX0.spriteName = "wan0";
		this.mX1.spriteName = "wan0";
		this.mTopOneLb.text = string.Empty;
		this.mBattleMarkTopOne.SetActive(false);
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null)
			{
				if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour1)
				{
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTwo1Lb.text = string.Empty;
						this.mTwo1Sp.hoverSprite = "nameBg1";
						this.mTwo1Sp.normalSprite = "nameBg1";
						this.mTwo1Sp.pressedSprite = "nameBg1";
						this.mFour1Sp.spriteName = "nameBg1";
						this.mFour2Sp.spriteName = "nameBg1";
						this.mY0.spriteName = "zhi0";
						this.mY1.spriteName = "wan0";
						this.mY2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Red)
					{
						this.mTwo1Lb.text = guildWarClient.Red.GuildName;
						if (iD == guildWarClient.Red.GuildID)
						{
							this.mTwo1Lb.color = Color.green;
						}
						else
						{
							this.mTwo1Lb.color = Color.white;
						}
						this.mTwo1Sp.hoverSprite = "nameBg0";
						this.mTwo1Sp.normalSprite = "nameBg0";
						this.mTwo1Sp.pressedSprite = "nameBg0";
						this.mFour1Sp.spriteName = "nameBg0";
						this.mFour2Sp.spriteName = "nameBg1";
						this.mY0.spriteName = "zhi1";
						this.mY1.spriteName = "wan1";
						this.mY2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Blue)
					{
						this.mTwo1Lb.text = guildWarClient.Blue.GuildName;
						if (iD == guildWarClient.Blue.GuildID)
						{
							this.mTwo1Lb.color = Color.green;
						}
						else
						{
							this.mTwo1Lb.color = Color.white;
						}
						this.mTwo1Sp.hoverSprite = "nameBg0";
						this.mTwo1Sp.normalSprite = "nameBg0";
						this.mTwo1Sp.pressedSprite = "nameBg0";
						this.mFour1Sp.spriteName = "nameBg1";
						this.mFour2Sp.spriteName = "nameBg0";
						this.mY0.spriteName = "zhi1";
						this.mY1.spriteName = "wan0";
						this.mY2.spriteName = "wan1";
					}
					else
					{
						this.mTwo1Lb.text = string.Empty;
						this.mTwo1Sp.hoverSprite = "nameBg1";
						this.mTwo1Sp.normalSprite = "nameBg1";
						this.mTwo1Sp.pressedSprite = "nameBg1";
						this.mFour1Sp.spriteName = "nameBg1";
						this.mFour2Sp.spriteName = "nameBg1";
						this.mY0.spriteName = "zhi0";
						this.mY1.spriteName = "wan0";
						this.mY2.spriteName = "wan0";
					}
					this.mFour1Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour1Lb.color = Color.green;
					}
					else
					{
						this.mFour1Lb.color = Color.white;
					}
					this.mFour2Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour2Lb.color = Color.green;
					}
					else
					{
						this.mFour2Lb.color = Color.white;
					}
				}
				else if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour2)
				{
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTwo2Lb.text = string.Empty;
						this.mTwo2Sp.hoverSprite = "nameBg1";
						this.mTwo2Sp.normalSprite = "nameBg1";
						this.mTwo2Sp.pressedSprite = "nameBg1";
						this.mFour3Sp.spriteName = "nameBg1";
						this.mFour4Sp.spriteName = "nameBg1";
						this.mZ0.spriteName = "zhi0";
						this.mZ1.spriteName = "wan0";
						this.mZ2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Red)
					{
						this.mTwo2Lb.text = guildWarClient.Red.GuildName;
						if (iD == guildWarClient.Red.GuildID)
						{
							this.mTwo2Lb.color = Color.green;
						}
						else
						{
							this.mTwo2Lb.color = Color.white;
						}
						this.mTwo2Sp.hoverSprite = "nameBg0";
						this.mTwo2Sp.normalSprite = "nameBg0";
						this.mTwo2Sp.pressedSprite = "nameBg0";
						this.mFour3Sp.spriteName = "nameBg0";
						this.mFour4Sp.spriteName = "nameBg1";
						this.mZ0.spriteName = "zhi1";
						this.mZ1.spriteName = "wan1";
						this.mZ2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Blue)
					{
						this.mTwo2Lb.text = guildWarClient.Blue.GuildName;
						if (iD == guildWarClient.Blue.GuildID)
						{
							this.mTwo2Lb.color = Color.green;
						}
						else
						{
							this.mTwo2Lb.color = Color.white;
						}
						this.mTwo2Sp.hoverSprite = "nameBg0";
						this.mTwo2Sp.normalSprite = "nameBg0";
						this.mTwo2Sp.pressedSprite = "nameBg0";
						this.mFour3Sp.spriteName = "nameBg1";
						this.mFour4Sp.spriteName = "nameBg0";
						this.mZ0.spriteName = "zhi1";
						this.mZ1.spriteName = "wan0";
						this.mZ2.spriteName = "wan1";
					}
					else
					{
						this.mTwo2Lb.text = string.Empty;
						this.mTwo2Sp.hoverSprite = "nameBg1";
						this.mTwo2Sp.normalSprite = "nameBg1";
						this.mTwo2Sp.pressedSprite = "nameBg1";
						this.mFour3Sp.spriteName = "nameBg1";
						this.mFour4Sp.spriteName = "nameBg1";
						this.mZ0.spriteName = "zhi0";
						this.mZ1.spriteName = "wan0";
						this.mZ2.spriteName = "wan0";
					}
					this.mFour3Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour3Lb.color = Color.green;
					}
					else
					{
						this.mFour3Lb.color = Color.white;
					}
					this.mFour4Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour4Lb.color = Color.green;
					}
					else
					{
						this.mFour4Lb.color = Color.white;
					}
				}
			}
		}
		this.mBattleMark41.SetActive(false);
		this.mBattleMark42.SetActive(false);
		this.mSb.Remove(0, this.mSb.Length).Append(this.mGuildCraft10Str);
		if (this.IsCanJoinBattleOne())
		{
			this.mSb.Append(this.mGuildCraft20Str);
			if (this.IsSelfBattleFourEmpty())
			{
				this.mSb.AppendLine().Append(this.mGuildCraft69Str);
			}
			else
			{
				this.mSb.AppendLine().Append(this.mGuildCraft12Str);
			}
		}
		else
		{
			this.mSb.Append(this.mGuildCraft21Str).AppendLine().Append(this.mGuildCraft14Str);
		}
		this.mTipTxt.gameObject.SetActive(true);
		this.mTipTxt.text = this.mSb.ToString();
	}

	private void OnEnterStateFinalPrepare()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		this.mTopCDDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft56");
		this.mX0.spriteName = "wan0";
		this.mX1.spriteName = "wan0";
		this.mBattleMarkTopOne.SetActive(false);
		this.mBattleMark41.SetActive(false);
		this.mBattleMark42.SetActive(false);
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null)
			{
				if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour1)
				{
					this.mFour1Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour1Lb.color = Color.green;
					}
					else
					{
						this.mFour1Lb.color = Color.white;
					}
					this.mFour2Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour2Lb.color = Color.green;
					}
					else
					{
						this.mFour2Lb.color = Color.white;
					}
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTwo1Sp.hoverSprite = "nameBg1";
						this.mTwo1Sp.normalSprite = "nameBg1";
						this.mTwo1Sp.pressedSprite = "nameBg1";
						this.mFour1Sp.spriteName = "nameBg1";
						this.mFour2Sp.spriteName = "nameBg1";
						this.mY0.spriteName = "zhi0";
						this.mY1.spriteName = "wan0";
						this.mY2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Red)
					{
						this.mTwo1Sp.hoverSprite = "nameBg0";
						this.mTwo1Sp.normalSprite = "nameBg0";
						this.mTwo1Sp.pressedSprite = "nameBg0";
						this.mFour1Sp.spriteName = "nameBg0";
						this.mFour2Sp.spriteName = "nameBg1";
						this.mY0.spriteName = "zhi1";
						this.mY1.spriteName = "wan1";
						this.mY2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Blue)
					{
						this.mTwo1Sp.hoverSprite = "nameBg0";
						this.mTwo1Sp.normalSprite = "nameBg0";
						this.mTwo1Sp.pressedSprite = "nameBg0";
						this.mFour1Sp.spriteName = "nameBg1";
						this.mFour2Sp.spriteName = "nameBg0";
						this.mY0.spriteName = "zhi1";
						this.mY1.spriteName = "wan0";
						this.mY2.spriteName = "wan1";
					}
					else
					{
						this.mTwo1Sp.hoverSprite = "nameBg1";
						this.mTwo1Sp.normalSprite = "nameBg1";
						this.mTwo1Sp.pressedSprite = "nameBg1";
						this.mFour1Sp.spriteName = "nameBg1";
						this.mFour2Sp.spriteName = "nameBg1";
						this.mY0.spriteName = "zhi0";
						this.mY1.spriteName = "wan0";
						this.mY2.spriteName = "wan0";
					}
				}
				else if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour2)
				{
					this.mFour3Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour3Lb.color = Color.green;
					}
					else
					{
						this.mFour3Lb.color = Color.white;
					}
					this.mFour4Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour4Lb.color = Color.green;
					}
					else
					{
						this.mFour4Lb.color = Color.white;
					}
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTwo2Sp.hoverSprite = "nameBg1";
						this.mTwo2Sp.normalSprite = "nameBg1";
						this.mTwo2Sp.pressedSprite = "nameBg1";
						this.mFour3Sp.spriteName = "nameBg1";
						this.mFour4Sp.spriteName = "nameBg1";
						this.mZ0.spriteName = "zhi0";
						this.mZ1.spriteName = "wan0";
						this.mZ2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Red)
					{
						this.mTwo2Sp.hoverSprite = "nameBg0";
						this.mTwo2Sp.normalSprite = "nameBg0";
						this.mTwo2Sp.pressedSprite = "nameBg0";
						this.mFour3Sp.spriteName = "nameBg0";
						this.mFour4Sp.spriteName = "nameBg1";
						this.mZ0.spriteName = "zhi1";
						this.mZ1.spriteName = "wan1";
						this.mZ2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Blue)
					{
						this.mTwo2Sp.hoverSprite = "nameBg0";
						this.mTwo2Sp.normalSprite = "nameBg0";
						this.mTwo2Sp.pressedSprite = "nameBg0";
						this.mFour3Sp.spriteName = "nameBg1";
						this.mFour4Sp.spriteName = "nameBg0";
						this.mZ0.spriteName = "zhi1";
						this.mZ1.spriteName = "wan0";
						this.mZ2.spriteName = "wan1";
					}
					else
					{
						this.mTwo2Sp.hoverSprite = "nameBg1";
						this.mTwo2Sp.normalSprite = "nameBg1";
						this.mTwo2Sp.pressedSprite = "nameBg1";
						this.mFour3Sp.spriteName = "nameBg1";
						this.mFour4Sp.spriteName = "nameBg1";
						this.mZ0.spriteName = "zhi0";
						this.mZ1.spriteName = "wan0";
						this.mZ2.spriteName = "wan0";
					}
				}
				else if (guildWarClient.WarID == EGuildWarId.EGWI_Final)
				{
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTopOneLb.text = string.Empty;
						this.mBattleMarkTopOne.SetActive(false);
					}
					else
					{
						if (this.IsSelfTwoTeamFull(guildWarClient))
						{
							this.mTopOneLb.text = this.mGuildCraft16Str;
							NGUITools.SetActive(this.mTopEffectGo, false);
							NGUITools.SetActive(this.mTopEffectGo, true);
						}
						else
						{
							this.mTopOneLb.text = string.Empty;
						}
						this.mBattleMarkTopOne.SetActive(true);
					}
					this.mTwo1Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mTwo1Lb.color = Color.green;
					}
					else
					{
						this.mTwo1Lb.color = Color.white;
					}
					this.mTwo2Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mTwo2Lb.color = Color.green;
					}
					else
					{
						this.mTwo2Lb.color = Color.white;
					}
				}
			}
		}
		this.mSb.Remove(0, this.mSb.Length).Append(this.mGuildCraft10Str);
		if (this.IsCanJoinBattleOne())
		{
			this.mSb.Append(this.mGuildCraft20Str);
			if (this.IsSelfBattleOneEmpty())
			{
				this.mSb.AppendLine().Append(this.mGuildCraft69Str);
			}
			else
			{
				this.mSb.AppendLine().Append(this.mGuildCraft12Str);
			}
		}
		else
		{
			this.mSb.Append(this.mGuildCraft21Str).AppendLine().Append(this.mGuildCraft14Str);
		}
		this.mTipTxt.gameObject.SetActive(true);
		this.mTipTxt.text = this.mSb.ToString();
	}

	private void OnEnterStateFinalGoing()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		this.mTopCDDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft57");
		this.mX0.spriteName = "wan0";
		this.mX1.spriteName = "wan0";
		this.mBattleMark41.SetActive(false);
		this.mBattleMark42.SetActive(false);
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null)
			{
				if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour1)
				{
					this.mFour1Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour1Lb.color = Color.green;
					}
					else
					{
						this.mFour1Lb.color = Color.white;
					}
					this.mFour2Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour2Lb.color = Color.green;
					}
					else
					{
						this.mFour2Lb.color = Color.white;
					}
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTwo1Sp.hoverSprite = "nameBg1";
						this.mTwo1Sp.normalSprite = "nameBg1";
						this.mTwo1Sp.pressedSprite = "nameBg1";
						this.mFour1Sp.spriteName = "nameBg1";
						this.mFour2Sp.spriteName = "nameBg1";
						this.mY0.spriteName = "zhi0";
						this.mY1.spriteName = "wan0";
						this.mY2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Red)
					{
						this.mTwo1Sp.hoverSprite = "nameBg0";
						this.mTwo1Sp.normalSprite = "nameBg0";
						this.mTwo1Sp.pressedSprite = "nameBg0";
						this.mFour1Sp.spriteName = "nameBg0";
						this.mFour2Sp.spriteName = "nameBg1";
						this.mY0.spriteName = "zhi1";
						this.mY1.spriteName = "wan1";
						this.mY2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Blue)
					{
						this.mTwo1Sp.hoverSprite = "nameBg0";
						this.mTwo1Sp.normalSprite = "nameBg0";
						this.mTwo1Sp.pressedSprite = "nameBg0";
						this.mFour1Sp.spriteName = "nameBg1";
						this.mFour2Sp.spriteName = "nameBg0";
						this.mY0.spriteName = "zhi1";
						this.mY1.spriteName = "wan0";
						this.mY2.spriteName = "wan1";
					}
					else
					{
						this.mTwo1Sp.hoverSprite = "nameBg1";
						this.mTwo1Sp.normalSprite = "nameBg1";
						this.mTwo1Sp.pressedSprite = "nameBg1";
						this.mFour1Sp.spriteName = "nameBg1";
						this.mFour2Sp.spriteName = "nameBg1";
						this.mY0.spriteName = "zhi0";
						this.mY1.spriteName = "wan0";
						this.mY2.spriteName = "wan0";
					}
				}
				else if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour2)
				{
					this.mFour3Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour3Lb.color = Color.green;
					}
					else
					{
						this.mFour3Lb.color = Color.white;
					}
					this.mFour4Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour4Lb.color = Color.green;
					}
					else
					{
						this.mFour4Lb.color = Color.white;
					}
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTwo2Sp.hoverSprite = "nameBg1";
						this.mTwo2Sp.normalSprite = "nameBg1";
						this.mTwo2Sp.pressedSprite = "nameBg1";
						this.mFour3Sp.spriteName = "nameBg1";
						this.mFour4Sp.spriteName = "nameBg1";
						this.mZ0.spriteName = "zhi0";
						this.mZ1.spriteName = "wan0";
						this.mZ2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Red)
					{
						this.mTwo2Sp.hoverSprite = "nameBg0";
						this.mTwo2Sp.normalSprite = "nameBg0";
						this.mTwo2Sp.pressedSprite = "nameBg0";
						this.mFour3Sp.spriteName = "nameBg0";
						this.mFour4Sp.spriteName = "nameBg1";
						this.mZ0.spriteName = "zhi1";
						this.mZ1.spriteName = "wan1";
						this.mZ2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Blue)
					{
						this.mTwo2Sp.hoverSprite = "nameBg0";
						this.mTwo2Sp.normalSprite = "nameBg0";
						this.mTwo2Sp.pressedSprite = "nameBg0";
						this.mFour3Sp.spriteName = "nameBg1";
						this.mFour4Sp.spriteName = "nameBg0";
						this.mZ0.spriteName = "zhi1";
						this.mZ1.spriteName = "wan0";
						this.mZ2.spriteName = "wan1";
					}
					else
					{
						this.mTwo2Sp.hoverSprite = "nameBg1";
						this.mTwo2Sp.normalSprite = "nameBg1";
						this.mTwo2Sp.pressedSprite = "nameBg1";
						this.mFour3Sp.spriteName = "nameBg1";
						this.mFour4Sp.spriteName = "nameBg1";
						this.mZ0.spriteName = "zhi0";
						this.mZ1.spriteName = "wan0";
						this.mZ2.spriteName = "wan0";
					}
				}
				else if (guildWarClient.WarID == EGuildWarId.EGWI_Final)
				{
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTopOneLb.text = string.Empty;
						this.mBattleMarkTopOne.SetActive(false);
					}
					else
					{
						if (this.IsTwoTeamFull(guildWarClient))
						{
							if (this.IsSelfTwoTeamFull(guildWarClient))
							{
								this.mTopOneLb.text = this.mGuildCraft16Str;
								NGUITools.SetActive(this.mTopEffectGo, false);
								NGUITools.SetActive(this.mTopEffectGo, true);
							}
							else
							{
								this.mTopOneLb.text = this.mGuildCraft17Str;
								this.mTopLiuGo.SetActive(true);
							}
						}
						else
						{
							this.mTopOneLb.text = string.Empty;
						}
						this.mBattleMarkTopOne.SetActive(true);
					}
					this.mTwo1Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mTwo1Lb.color = Color.green;
					}
					else
					{
						this.mTwo1Lb.color = Color.white;
					}
					this.mTwo2Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mTwo2Lb.color = Color.green;
					}
					else
					{
						this.mTwo2Lb.color = Color.white;
					}
				}
			}
		}
		this.mSb.Remove(0, this.mSb.Length).Append(this.mGuildCraft10Str);
		if (this.IsCanJoinBattleOne())
		{
			this.mSb.Append(this.mGuildCraft20Str);
			if (this.IsSelfBattleOneEmpty())
			{
				this.mSb.AppendLine().Append(this.mGuildCraft69Str);
			}
			else
			{
				this.mSb.AppendLine().Append(this.mGuildCraft12Str);
			}
		}
		else
		{
			this.mSb.Append(this.mGuildCraft21Str).AppendLine().Append(this.mGuildCraft14Str);
		}
		this.mTipTxt.gameObject.SetActive(true);
		this.mTipTxt.text = this.mSb.ToString();
	}

	private void OnEnterStateFinalEnd()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		this.mBattleMarkTopOne.SetActive(false);
		this.mBattleMark41.SetActive(false);
		this.mBattleMark42.SetActive(false);
		ulong iD = Globals.Instance.Player.GuildSystem.Guild.ID;
		for (int i = 0; i < mWarStateInfo.mWarDatas.Count; i++)
		{
			GuildWarClient guildWarClient = mWarStateInfo.mWarDatas[i];
			if (guildWarClient != null)
			{
				if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour1)
				{
					this.mFour1Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour1Lb.color = Color.green;
					}
					else
					{
						this.mFour1Lb.color = Color.white;
					}
					this.mFour2Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour2Lb.color = Color.green;
					}
					else
					{
						this.mFour2Lb.color = Color.white;
					}
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTwo1Sp.hoverSprite = "nameBg1";
						this.mTwo1Sp.normalSprite = "nameBg1";
						this.mTwo1Sp.pressedSprite = "nameBg1";
						this.mFour1Sp.spriteName = "nameBg1";
						this.mFour2Sp.spriteName = "nameBg1";
						this.mY0.spriteName = "zhi0";
						this.mY1.spriteName = "wan0";
						this.mY2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Red)
					{
						this.mTwo1Sp.hoverSprite = "nameBg0";
						this.mTwo1Sp.normalSprite = "nameBg0";
						this.mTwo1Sp.pressedSprite = "nameBg0";
						this.mFour1Sp.spriteName = "nameBg0";
						this.mFour2Sp.spriteName = "nameBg1";
						this.mY0.spriteName = "zhi1";
						this.mY1.spriteName = "wan1";
						this.mY2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Blue)
					{
						this.mTwo1Sp.hoverSprite = "nameBg0";
						this.mTwo1Sp.normalSprite = "nameBg0";
						this.mTwo1Sp.pressedSprite = "nameBg0";
						this.mFour1Sp.spriteName = "nameBg1";
						this.mFour2Sp.spriteName = "nameBg0";
						this.mY0.spriteName = "zhi1";
						this.mY1.spriteName = "wan0";
						this.mY2.spriteName = "wan1";
					}
					else
					{
						this.mTwo1Sp.hoverSprite = "nameBg1";
						this.mTwo1Sp.normalSprite = "nameBg1";
						this.mTwo1Sp.pressedSprite = "nameBg1";
						this.mFour1Sp.spriteName = "nameBg1";
						this.mFour2Sp.spriteName = "nameBg1";
						this.mY0.spriteName = "zhi0";
						this.mY1.spriteName = "wan0";
						this.mY2.spriteName = "wan0";
					}
				}
				else if (guildWarClient.WarID == EGuildWarId.EGWI_FinalFour2)
				{
					this.mFour3Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mFour3Lb.color = Color.green;
					}
					else
					{
						this.mFour3Lb.color = Color.white;
					}
					this.mFour4Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mFour4Lb.color = Color.green;
					}
					else
					{
						this.mFour4Lb.color = Color.white;
					}
					if (guildWarClient.Red.GuildID == 0uL && guildWarClient.Blue.GuildID == 0uL)
					{
						this.mTwo2Sp.hoverSprite = "nameBg1";
						this.mTwo2Sp.normalSprite = "nameBg1";
						this.mTwo2Sp.pressedSprite = "nameBg1";
						this.mFour3Sp.spriteName = "nameBg1";
						this.mFour4Sp.spriteName = "nameBg1";
						this.mZ0.spriteName = "zhi0";
						this.mZ1.spriteName = "wan0";
						this.mZ2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Red)
					{
						this.mTwo2Sp.hoverSprite = "nameBg0";
						this.mTwo2Sp.normalSprite = "nameBg0";
						this.mTwo2Sp.pressedSprite = "nameBg0";
						this.mFour3Sp.spriteName = "nameBg0";
						this.mFour4Sp.spriteName = "nameBg1";
						this.mZ0.spriteName = "zhi1";
						this.mZ1.spriteName = "wan1";
						this.mZ2.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Blue)
					{
						this.mTwo2Sp.hoverSprite = "nameBg0";
						this.mTwo2Sp.normalSprite = "nameBg0";
						this.mTwo2Sp.pressedSprite = "nameBg0";
						this.mFour3Sp.spriteName = "nameBg1";
						this.mFour4Sp.spriteName = "nameBg0";
						this.mZ0.spriteName = "zhi1";
						this.mZ1.spriteName = "wan0";
						this.mZ2.spriteName = "wan1";
					}
					else
					{
						this.mTwo2Sp.hoverSprite = "nameBg1";
						this.mTwo2Sp.normalSprite = "nameBg1";
						this.mTwo2Sp.pressedSprite = "nameBg1";
						this.mFour3Sp.spriteName = "nameBg1";
						this.mFour4Sp.spriteName = "nameBg1";
						this.mZ0.spriteName = "zhi0";
						this.mZ1.spriteName = "wan0";
						this.mZ2.spriteName = "wan0";
					}
				}
				else if (guildWarClient.WarID == EGuildWarId.EGWI_Final)
				{
					if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Red)
					{
						this.mTopOneLb.text = guildWarClient.Red.GuildName;
						if (iD == guildWarClient.Red.GuildID)
						{
							this.mTopOneLb.color = Color.green;
						}
						else
						{
							this.mTopOneLb.color = Color.white;
						}
						this.mX0.spriteName = "wan1";
						this.mX1.spriteName = "wan0";
					}
					else if (guildWarClient.Winner == EGuildWarTeamId.EGWTI_Blue)
					{
						this.mTopOneLb.text = guildWarClient.Blue.GuildName;
						if (iD == guildWarClient.Blue.GuildID)
						{
							this.mTopOneLb.color = Color.green;
						}
						else
						{
							this.mTopOneLb.color = Color.white;
						}
						this.mX0.spriteName = "wan0";
						this.mX1.spriteName = "wan1";
					}
					else
					{
						this.mTopOneLb.text = string.Empty;
						this.mX0.spriteName = "wan0";
						this.mX1.spriteName = "wan0";
					}
					this.mTwo1Lb.text = guildWarClient.Red.GuildName;
					if (iD == guildWarClient.Red.GuildID)
					{
						this.mTwo1Lb.color = Color.green;
					}
					else
					{
						this.mTwo1Lb.color = Color.white;
					}
					this.mTwo2Lb.text = guildWarClient.Blue.GuildName;
					if (iD == guildWarClient.Blue.GuildID)
					{
						this.mTwo2Lb.color = Color.green;
					}
					else
					{
						this.mTwo2Lb.color = Color.white;
					}
				}
			}
		}
		this.mTipTxt.gameObject.SetActive(false);
	}

	public void Refresh()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo != null)
		{
			this.SetEffectDisable();
			if (mWarStateInfo.mWarState == EGuildWarState.EGWS_SelectFourTeam)
			{
				this.OnEnterStateSelectFourTeam();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare)
			{
				this.OnEnterStateFinalFourPrepare();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing)
			{
				this.OnEnterStateFinalFourGoing();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourEnd)
			{
				this.OnEnterStateFinalFourEnd();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
			{
				this.OnEnterStateFinalPrepare();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
			{
				this.OnEnterStateFinalGoing();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalEnd)
			{
				this.OnEnterStateFinalEnd();
			}
			this.RefreshCDTime();
		}
	}

	private void RefreshCDTime()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if (mWarStateInfo.mWarState == EGuildWarState.EGWS_SelectFourTeam)
		{
			int num = mWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (num < 0)
			{
				num = 0;
			}
			string value = Tools.FormatTime(num);
			this.mTopCDGo.SetActive(true);
			this.mTopCDLb.text = this.mSb.Remove(0, this.mSb.Length).Append(value).ToString();
		}
		else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare)
		{
			int num2 = mWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (num2 < 0)
			{
				num2 = 0;
			}
			string value2 = Tools.FormatTime(num2);
			this.mTopCDGo.SetActive(true);
			this.mTopCDLb.text = this.mSb.Remove(0, this.mSb.Length).Append(value2).ToString();
		}
		else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing)
		{
			int num3 = mWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (num3 < 0)
			{
				num3 = 0;
			}
			string value3 = Tools.FormatTime(num3);
			this.mTopCDGo.SetActive(true);
			this.mTopCDLb.text = this.mSb.Remove(0, this.mSb.Length).Append(value3).ToString();
		}
		else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourEnd)
		{
			int num4 = mWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (num4 < 0)
			{
				num4 = 0;
			}
			string value4 = Tools.FormatTime(num4);
			this.mTopCDGo.SetActive(true);
			this.mTopCDLb.text = this.mSb.Remove(0, this.mSb.Length).Append(value4).ToString();
		}
		else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
		{
			int num5 = mWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (num5 < 0)
			{
				num5 = 0;
			}
			string value5 = Tools.FormatTime(num5);
			this.mTopCDGo.SetActive(true);
			this.mTopCDLb.text = this.mSb.Remove(0, this.mSb.Length).Append(value5).ToString();
		}
		else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
		{
			int num6 = mWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (num6 < 0)
			{
				num6 = 0;
			}
			string value6 = Tools.FormatTime(num6);
			this.mTopCDGo.SetActive(true);
			this.mTopCDLb.text = this.mSb.Remove(0, this.mSb.Length).Append(value6).ToString();
		}
		else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalEnd)
		{
			this.mTopCDGo.SetActive(false);
		}
	}

	private void OnGuildWarEnter()
	{
		GameUIManager.mInstance.ChangeSession<GUIGuildCraftSetScene>(null, false, true);
		GUIGuildCraftTeamInfoPop.CloseMe();
	}

	private void OnCastleUpdate()
	{
		this.Refresh();
	}

	private void OnGetWarStateInfoEvent()
	{
		this.Refresh();
	}

	private void Update()
	{
		if (Time.time - this.mRefreshTimer >= 1f)
		{
			this.mRefreshTimer = Time.time;
			this.RefreshCDTime();
		}
	}
}
