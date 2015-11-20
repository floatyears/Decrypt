using Att;
using Holoville.HOTween.Core;
using Proto;
using ProtoBuf;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class GameUIAdventureReady : MonoBehaviour
{
	private Transform readyWinBG;

	private UILabel readyTimesLb;

	private GameObject readyTimesBtn;

	private GameObject FarmOne;

	private GameObject FarmMutil;

	private UILabel FarmMutilText;

	private UILabel keyConst;

	private GameObject mMapFarm;

	private int[] mMonsterInfoId = new int[3];

	private StringBuilder mSb = new StringBuilder(42);

	public SceneInfo sceneInfo
	{
		get;
		set;
	}

	public void Init(SceneInfo sceneInfo)
	{
		this.sceneInfo = sceneInfo;
		base.transform.localPosition = new Vector3(0f, 0f, -400f);
		this.readyWinBG = base.transform.Find("winBG");
		GameObject gameObject = this.readyWinBG.Find("closeBtn").gameObject;
		UIEventListener expr_58 = UIEventListener.Get(gameObject);
		expr_58.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_58.onClick, new UIEventListener.VoidDelegate(this.OnReadyCloseBtnClicked));
		UIEventListener expr_93 = UIEventListener.Get(base.transform.Find("FadeBG").gameObject);
		expr_93.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_93.onClick, new UIEventListener.VoidDelegate(this.OnReadyCloseBtnClicked));
		UILabel component = this.readyWinBG.FindChild("Title").GetComponent<UILabel>();
		UILabel component2 = this.readyWinBG.FindChild("Difficulty").GetComponent<UILabel>();
		UILabel component3 = this.readyWinBG.FindChild("Label3").GetComponent<UILabel>();
		if (sceneInfo.ID / 100000 != 6)
		{
			component.text = string.Format("[ffdd77]{0}-{1,-6}[ffefbe]{2, -6}", sceneInfo.MapID % 100, sceneInfo.ID % 100, sceneInfo.Name);
			if (sceneInfo.Difficulty == 0)
			{
				component2.text = Singleton<StringManager>.Instance.GetString("pveReadyDiff");
				component2.color = new Color32(171, 254, 41, 255);
			}
			else if (sceneInfo.Difficulty == 1)
			{
				component2.text = Singleton<StringManager>.Instance.GetString("pveReadyDiff2");
				component2.color = new Color32(255, 44, 3, 255);
			}
			else
			{
				component2.text = Singleton<StringManager>.Instance.GetString("pveReadyDiff3");
				component2.color = new Color32(202, 47, 227, 255);
			}
			Transform transform = this.readyWinBG.Find("StarGroup");
			int sceneScore = Globals.Instance.Player.GetSceneScore(sceneInfo.ID);
			for (int i = 0; i < 3; i++)
			{
				UISprite component4 = transform.FindChild(string.Format("star{0}", i)).GetComponent<UISprite>();
				if (i < sceneScore)
				{
					component4.spriteName = "star";
				}
				else
				{
					component4.spriteName = "star0";
				}
			}
			if (sceneInfo.Difficulty == 9)
			{
				component3.text = Singleton<StringManager>.Instance.GetString("awakeRoad5");
			}
			else
			{
				component3.text = Singleton<StringManager>.Instance.GetString("awakeRoad4");
			}
		}
		else
		{
			component.text = this.mSb.Remove(0, this.mSb.Length).Append("[ffdd77]").Append(Singleton<StringManager>.Instance.GetString("awakeRoad1", new object[]
			{
				sceneInfo.MapID % 100
			})).Append("[-]      [ffefbe]").Append(sceneInfo.Name).Append("[-]").ToString();
			component2.text = Singleton<StringManager>.Instance.GetString("awakeRoad0");
			component2.color = new Color32(255, 44, 3, 255);
			Transform transform2 = this.readyWinBG.Find("StarGroup");
			if (sceneInfo.DayReset)
			{
				int sceneScore2 = Globals.Instance.Player.GetSceneScore(sceneInfo.ID);
				for (int j = 0; j < 3; j++)
				{
					UISprite component5 = transform2.FindChild(string.Format("star{0}", j)).GetComponent<UISprite>();
					if (j < sceneScore2)
					{
						component5.spriteName = "star";
					}
					else
					{
						component5.spriteName = "star0";
					}
				}
			}
			else
			{
				for (int k = 0; k < 3; k++)
				{
					UISprite component6 = transform2.FindChild(string.Format("star{0}", k)).GetComponent<UISprite>();
					component6.spriteName = string.Empty;
				}
			}
			component3.text = Singleton<StringManager>.Instance.GetString("awakeRoad5");
		}
		this.readyWinBG.FindChild("Desc").GetComponent<UILabel>().text = sceneInfo.Desc;
		Transform transform3 = this.readyWinBG.FindChild("RaidsBG");
		GameObject gameObject2 = transform3.Find("ReadyStartBtn").gameObject;
		UIEventListener expr_49C = UIEventListener.Get(gameObject2);
		expr_49C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_49C.onClick, new UIEventListener.VoidDelegate(this.OnReadyStartBtnClicked));
		this.keyConst = gameObject2.transform.FindChild("key/Label").GetComponent<UILabel>();
		this.keyConst.text = string.Format("{0}", this.sceneInfo.CostValue);
		this.readyTimesBtn = GameUITools.RegisterClickEvent("SceneTimes/BtnChecked", new UIEventListener.VoidDelegate(this.OnBuyTimesClicked), transform3.gameObject);
		this.readyTimesLb = transform3.FindChild("SceneTimes/Label").GetComponent<UILabel>();
		this.FarmOne = transform3.FindChild("BtnChecked2").gameObject;
		this.FarmOne.name = "1";
		this.FarmMutil = transform3.FindChild("BtnChecked1").gameObject;
		this.FarmMutilText = this.FarmMutil.transform.FindChild("Label").GetComponent<UILabel>();
		this.RefreshSceneTimes();
		UIEventListener expr_5A7 = UIEventListener.Get(this.FarmOne);
		expr_5A7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_5A7.onClick, new UIEventListener.VoidDelegate(this.OnFarmClicked));
		UIEventListener expr_5D3 = UIEventListener.Get(this.FarmMutil);
		expr_5D3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_5D3.onClick, new UIEventListener.VoidDelegate(this.OnFarmClicked));
		UISlider component7 = transform3.transform.Find("Exp").GetComponent<UISlider>();
		UISprite component8 = component7.transform.Find("ExpFg").GetComponent<UISprite>();
		UILabel component9 = component7.transform.Find("ExpText").GetComponent<UILabel>();
		UILabel component10 = transform3.transform.FindChild("RecommendCombat").GetComponent<UILabel>();
		component10.text = sceneInfo.CombatValue.ToString();
		UILabel component11 = transform3.transform.FindChild("TeamCombat").GetComponent<UILabel>();
		int combatValue = Globals.Instance.Player.TeamSystem.GetCombatValue();
		component11.text = combatValue.ToString();
		float num = (sceneInfo.CombatValue <= 0) ? 1f : ((float)combatValue / (float)sceneInfo.CombatValue);
		float[] expr_6CF = new float[4];
		expr_6CF[0] = 1f;
		expr_6CF[1] = 0.8f;
		expr_6CF[2] = 0.6f;
		float[] array = expr_6CF;
		Color[] array2 = new Color[]
		{
			new Color32(102, 254, 0, 255),
			new Color32(254, 217, 14, 255),
			new Color32(252, 141, 0, 255),
			new Color32(254, 1, 3, 255)
		};
		for (int l = 0; l < array.Length; l++)
		{
			if (num >= array[l])
			{
				component11.color = array2[l];
				component7.value = num;
				component8.spriteName = string.Format("lvlFg{0}", l);
				component9.text = Singleton<StringManager>.Instance.GetString(string.Format("AdvDifficulty{0}", l));
				component9.color = array2[l];
				break;
			}
		}
		Transform transform4 = transform3.FindChild("BtnTeam");
		UIEventListener expr_834 = UIEventListener.Get(transform4.gameObject);
		expr_834.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_834.onClick, new UIEventListener.VoidDelegate(this.OnBtnTeamClicked));
		Transform transform5 = this.readyWinBG.FindChild("Enemy");
		int num2 = 0;
		for (int m = 0; m < 3; m++)
		{
			this.mMonsterInfoId[m] = 0;
		}
		for (int n = 0; n < 3; n++)
		{
			if (sceneInfo.Enemy[n] != 0 && !this.IsMonsterIdExist(sceneInfo.Enemy[n]))
			{
				MonsterInfo info = Globals.Instance.AttDB.MonsterDict.GetInfo(sceneInfo.Enemy[n]);
				if (info != null)
				{
					Transform transform6 = transform5.transform.FindChild(string.Format("group{0}", num2 + 1));
					GameObject gameObject3 = transform6.FindChild("Sprite").gameObject;
					gameObject3.SetActive(info.BossType > 0);
					GameObject gameObject4 = transform6.FindChild("MasterItem").gameObject;
					UIMasterItem uIMasterItem = gameObject4.AddComponent<UIMasterItem>();
					uIMasterItem.Init(info, true);
					this.mMonsterInfoId[num2] = sceneInfo.Enemy[n];
					num2++;
				}
			}
		}
		for (int num3 = num2; num3 < 3; num3++)
		{
			transform5.transform.FindChild(string.Format("group{0}", num3 + 1)).gameObject.SetActive(false);
		}
		Transform transform7 = this.readyWinBG.FindChild("Label0");
		UILabel component12 = transform7.FindChild("Money/Value").GetComponent<UILabel>();
		component12.text = sceneInfo.RewardMoney.ToString();
		UILabel component13 = transform7.FindChild("Exp/Value").GetComponent<UILabel>();
		component13.text = sceneInfo.RewardExp.ToString();
		Transform transform8 = this.readyWinBG.FindChild("Reward");
		int num4 = 0;
		if (sceneInfo.Difficulty == 9)
		{
			GameObject gameObject5 = GameUITools.CreateReward(15, sceneInfo.RewardEmblem, 0, transform8, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
			GameObject gameObject6 = transform8.FindChild("FirstReward").gameObject;
			gameObject6.SetActive(true);
			gameObject5 = GameUITools.CreateReward(2, sceneInfo.RewardDiamond, 0, transform8, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
			if (gameObject5 != null)
			{
				gameObject5.transform.localPosition = new Vector3(446f, 0f, 0f);
			}
			num4++;
		}
		float num5 = 96f;
		float num6 = 8f;
		ActivityValueData valueMod = Globals.Instance.Player.ActivitySystem.GetValueMod(2);
		for (int num7 = 0; num7 < sceneInfo.LootItem.Count; num7++)
		{
			if (num4 >= 4)
			{
				break;
			}
			ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(sceneInfo.LootItem[num7]);
			if (info2 != null)
			{
				int num8 = sceneInfo.LootMinCount[num7];
				if (valueMod != null && info2.Type == 3)
				{
					num8 = num8 * valueMod.Value1 / 100;
				}
				GameObject gameObject7 = GameUITools.CreateReward(3, sceneInfo.LootItem[num7], num8, transform8, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
				if (gameObject7 != null)
				{
					gameObject7.transform.localPosition = new Vector3((float)num4 * (num5 + num6), 0f, 0f);
					num4++;
				}
			}
		}
		base.gameObject.SetActive(true);
		Globals.Instance.CliSession.Register(605, new ClientSession.MsgHandler(this.OnMsgFarm));
		Globals.Instance.CliSession.Register(613, new ClientSession.MsgHandler(this.OnMsgResetSceneCD));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		if (this.mMapFarm != null)
		{
			UnityEngine.Object.Destroy(this.mMapFarm);
		}
		Globals.Instance.CliSession.Unregister(605, new ClientSession.MsgHandler(this.OnMsgFarm));
		Globals.Instance.CliSession.Unregister(613, new ClientSession.MsgHandler(this.OnMsgResetSceneCD));
	}

	private bool IsMonsterIdExist(int infoId)
	{
		bool result = false;
		for (int i = 0; i < 3; i++)
		{
			if (this.mMonsterInfoId[i] != 0 && this.mMonsterInfoId[i] == infoId)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public void OnReadyCloseBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.CloseReadySelf();
	}

	private void CloseReadySelf()
	{
		GameUITools.PlayCloseWindowAnim(this.readyWinBG, new TweenDelegate.TweenCallback(this.OnCloseReadyAnimEnd), true);
	}

	private void OnCloseReadyAnimEnd()
	{
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}

	public void OnReadyStartBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.sceneInfo.Difficulty == 9 && Globals.Instance.Player.Data.NightmareCount >= GameConst.GetInt32(124))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("PveR_48", 0f, 0f);
			return;
		}
		if (this.sceneInfo.DayTimes - Globals.Instance.Player.GetSceneTimes(this.sceneInfo.ID) <= 0)
		{
			if (this.sceneInfo.Difficulty != 9)
			{
				this.BuySceneTimes();
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("PveR_18", 0f, 0f);
			}
			return;
		}
		if (Globals.Instance.Player.Data.Energy < this.sceneInfo.CostValue)
		{
			GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Energy);
			return;
		}
		GameUIManager.mInstance.uiState.AdventureSceneInfo = this.sceneInfo;
		GameUIManager.mInstance.uiState.PveSceneID = this.sceneInfo.ID;
		GameUIManager.mInstance.uiState.PveSceneValue = 0;
		MC2S_PveStart mC2S_PveStart = new MC2S_PveStart();
		mC2S_PveStart.SceneID = this.sceneInfo.ID;
		Globals.Instance.CliSession.Send(600, mC2S_PveStart);
	}

	private void OnFarmClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		LocalPlayer player = Globals.Instance.Player;
		if ((ulong)player.Data.Level < (ulong)((long)GameConst.GetInt32(14)))
		{
			string @string = Singleton<StringManager>.Instance.GetString("FarmMinLevel", new object[]
			{
				GameConst.GetInt32(14)
			});
			GameUIManager.mInstance.ShowMessageTip(@string, 0f, 0f);
			return;
		}
		int num = Convert.ToInt32(go.name);
		if (num <= 0)
		{
			return;
		}
		if (this.sceneInfo.DayTimes - player.GetSceneTimes(this.sceneInfo.ID) < num)
		{
			return;
		}
		GameUIAdventureReady.SendFarmMsg(this.sceneInfo.ID, num);
	}

	private static void SendFarmMsg(int sceneID, int times)
	{
		LocalPlayer player = Globals.Instance.Player;
		SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(sceneID);
		if (player.Data.Energy < info.CostValue)
		{
			GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Energy);
			return;
		}
		if (times > 1 && player.Data.Energy < info.CostValue * times)
		{
			times = (int)((long)player.Data.Energy / (long)((ulong)info.CostValue));
		}
		GUIGameResultVictoryScene.CacheOldData();
		MC2S_Farm mC2S_Farm = new MC2S_Farm();
		mC2S_Farm.SceneID = sceneID;
		mC2S_Farm.Times = times;
		Globals.Instance.CliSession.Send(604, mC2S_Farm);
	}

	private void OnBuyTimesClicked(GameObject go)
	{
		this.BuySceneTimes();
	}

	private void BuySceneTimes()
	{
		int vipLevel = (int)Globals.Instance.Player.Data.VipLevel;
		VipLevelInfo vipLevelInfo = Globals.Instance.Player.GetVipLevelInfo();
		if (vipLevelInfo == null)
		{
			return;
		}
		int iD = this.sceneInfo.ID;
		int resetCount = Globals.Instance.Player.GetSceneData(iD).ResetCount;
		int sceneResetCount = vipLevelInfo.SceneResetCount;
		if (resetCount >= sceneResetCount)
		{
			if (vipLevel >= 15)
			{
				GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("FarmMaxResetCount2"), MessageBox.Type.OK, null);
			}
			else
			{
				VipLevelInfo vipLevelInfo2 = null;
				for (VipLevelInfo info = Globals.Instance.AttDB.VipLevelDict.GetInfo((int)(Globals.Instance.Player.Data.VipLevel + 1u)); info != null; info = Globals.Instance.AttDB.VipLevelDict.GetInfo(info.ID + 1))
				{
					if (info.ID > 15)
					{
						break;
					}
					if (info.SceneResetCount > sceneResetCount)
					{
						vipLevelInfo2 = info;
						break;
					}
				}
				if (vipLevelInfo2 != null)
				{
					GameMessageBox.ShowPrivilegeMessageBox(string.Format(Singleton<StringManager>.Instance.GetString("FarmMaxResetCount"), vipLevelInfo2.ID, vipLevelInfo2.SceneResetCount));
				}
				else
				{
					GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("FarmMaxResetCount2"), MessageBox.Type.OK, null);
				}
			}
			return;
		}
		MiscInfo info2 = Globals.Instance.AttDB.MiscDict.GetInfo(resetCount + 1);
		if (info2 == null)
		{
			return;
		}
		int sceneResetCost = info2.SceneResetCost;
		GameMessageBox.ShowFarmSceneResetCountMessageBox(iD, sceneResetCount - resetCount, sceneResetCost);
	}

	public static void SendResetSceneCDMsg(int sceneID, int needDiamond)
	{
		if (needDiamond > Globals.Instance.Player.Data.Diamond)
		{
			GameMessageBox.ShowRechargeMessageBox();
			return;
		}
		MC2S_ResetSceneCD mC2S_ResetSceneCD = new MC2S_ResetSceneCD();
		mC2S_ResetSceneCD.SceneID = sceneID;
		Globals.Instance.CliSession.Send(612, mC2S_ResetSceneCD);
	}

	private void RefreshSceneTimes()
	{
		int num = this.sceneInfo.DayTimes - Globals.Instance.Player.GetSceneTimes(this.sceneInfo.ID);
		if (num > 0)
		{
			this.readyTimesBtn.SetActive(false);
			this.readyTimesLb.text = string.Format("{0} / {1}", num, this.sceneInfo.DayTimes);
			LocalPlayer player = Globals.Instance.Player;
			int sceneScore = player.GetSceneScore(this.sceneInfo.ID);
			if (sceneScore == 3)
			{
				this.FarmOne.SetActive(true);
				this.FarmMutil.SetActive(this.sceneInfo.DayTimes > 1);
				int num2 = Mathf.Clamp(num, 1, 10);
				this.FarmMutilText.text = string.Format(Singleton<StringManager>.Instance.GetString("FormTimes"), num2);
				this.FarmMutil.name = num2.ToString();
			}
			else
			{
				this.FarmOne.SetActive(false);
				this.FarmMutil.SetActive(false);
			}
		}
		else
		{
			if (this.sceneInfo.Difficulty == 9)
			{
				this.readyTimesBtn.SetActive(false);
			}
			else
			{
				this.readyTimesBtn.SetActive(true);
			}
			this.readyTimesLb.text = string.Format("[fe3231]{0} [-]/ {1}", num, this.sceneInfo.DayTimes);
			this.FarmOne.SetActive(false);
			this.FarmMutil.SetActive(false);
		}
	}

	public void OnMsgFarm(MemoryStream stream)
	{
		MS2C_Farm mS2C_Farm = Serializer.NonGeneric.Deserialize(typeof(MS2C_Farm), stream) as MS2C_Farm;
		if (mS2C_Farm.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_Farm.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_Farm.Result);
			return;
		}
		this.RefreshSceneTimes();
		GameObject gameObject = Res.LoadGUI("GUI/GameUIMapFarm");
		this.mMapFarm = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		Vector3 localPosition = this.mMapFarm.transform.localPosition;
		localPosition.z += 5000f;
		this.mMapFarm.transform.localPosition = localPosition;
		this.mMapFarm.name = gameObject.name;
		GameUIMapFarm gameUIMapFarm = this.mMapFarm.AddComponent<GameUIMapFarm>();
		GameUIMapFarm expr_D3 = gameUIMapFarm;
		expr_D3.MapFarmFinishEvent = (GameUIMapFarm.MapFarmFinishCallback)Delegate.Combine(expr_D3.MapFarmFinishEvent, new GameUIMapFarm.MapFarmFinishCallback(this.OnMapFarmFinishEvent));
		gameUIMapFarm.Init(this, mS2C_Farm.Data);
	}

	public void OnMapFarmFinishEvent()
	{
		GUIWorldMap gUIWorldMap = GameUIManager.mInstance.CurUISession as GUIWorldMap;
		if (gUIWorldMap != null)
		{
			gUIWorldMap.RefreshTeamBtnNewFlag();
		}
	}

	public void OnMsgResetSceneCD(MemoryStream stream)
	{
		MS2C_ResetSceneCD mS2C_ResetSceneCD = Serializer.NonGeneric.Deserialize(typeof(MS2C_ResetSceneCD), stream) as MS2C_ResetSceneCD;
		if (mS2C_ResetSceneCD.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_ResetSceneCD.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_ResetSceneCD.Result);
			return;
		}
		this.RefreshSceneTimes();
	}

	private void OnBtnTeamClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.IsLocalPlayer = true;
		GameUIManager.mInstance.uiState.CombatPetSlot = 0;
		GameUIManager.mInstance.ChangeSession<GUITeamManageSceneV2>(null, false, true);
	}
}
