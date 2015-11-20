using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIAchievementScene : GameUISession
{
	private const float totalScore = 140f;

	private bool RefreshPlayerDataFlag;

	private AchievementGrid[] mAchievementTable = new AchievementGrid[2];

	private UIToggle[] tabPage = new UIToggle[2];

	private GameObject[] newFlag = new GameObject[2];

	private int curSelectTab;

	private UISlider Slider;

	private UILabel DailyScore;

	private UILabel DailyScoreDesc;

	private UISprite[] chest = new UISprite[4];

	private UILabel[] chestScore = new UILabel[4];

	private GameObject[] chestSFX = new GameObject[4];

	private UITweener[] chestTween = new UITweener[4];

	public static MiscInfo[] miscInfo = new MiscInfo[4];

	public static int[] scoreValue = new int[]
	{
		30,
		60,
		90,
		120
	};

	private DailyScoreRewardWnd DailyScoreWnd;

	public static bool HasNewScore()
	{
		LocalPlayer player = Globals.Instance.Player;
		for (int i = 0; i < 4; i++)
		{
			if (Globals.Instance.AttDB.MiscDict.GetInfo(12 + i) != null)
			{
				int dailyRewardFlag = player.Data.DailyRewardFlag;
				if ((dailyRewardFlag & 1 << i) == 0 && player.Data.DailyScore >= GUIAchievementScene.scoreValue[i])
				{
					return true;
				}
			}
		}
		return false;
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("questAH");
		this.DailyScoreWnd = base.transform.Find("DayReward").gameObject.AddComponent<DailyScoreRewardWnd>();
		this.DailyScoreWnd.Init();
		this.DailyScoreWnd.gameObject.SetActive(false);
		Transform transform = base.transform.Find("WindowBg");
		for (int i = 0; i < 2; i++)
		{
			this.mAchievementTable[i] = transform.FindChild(string.Format("bagPanel{0}/bagContents", i)).gameObject.AddComponent<AchievementGrid>();
			this.mAchievementTable[i].maxPerLine = 1;
			this.mAchievementTable[i].arrangement = UICustomGrid.Arrangement.Vertical;
			this.mAchievementTable[i].cellWidth = 740f;
			this.mAchievementTable[i].cellHeight = 98f;
			this.tabPage[i] = transform.transform.Find(string.Format("tab{0}", i)).GetComponent<UIToggle>();
			EventDelegate.Add(this.tabPage[i].onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
			UIEventListener expr_13A = UIEventListener.Get(this.tabPage[i].gameObject);
			expr_13A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_13A.onClick, new UIEventListener.VoidDelegate(this.OnTabClicked));
			this.newFlag[i] = this.tabPage[i].transform.Find("new").gameObject;
			this.newFlag[i].SetActive(false);
		}
		this.Slider = transform.transform.Find("Slider").GetComponent<UISlider>();
		this.DailyScore = this.Slider.transform.Find("num").GetComponent<UILabel>();
		for (int j = 0; j < 4; j++)
		{
			this.chest[j] = this.Slider.transform.Find(string.Format("c{0}/chest", j)).GetComponent<UISprite>();
			this.chestTween[j] = this.chest[j].GetComponent<UITweener>();
			this.chestScore[j] = this.chest[j].transform.parent.Find("num").GetComponent<UILabel>();
			this.chestSFX[j] = this.chest[j].transform.parent.Find("ui65").gameObject;
			this.chestScore[j].text = GUIAchievementScene.scoreValue[j].ToString();
			UIEventListener expr_29D = UIEventListener.Get(this.chest[j].gameObject);
			expr_29D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_29D.onClick, new UIEventListener.VoidDelegate(this.OnDailyScoreClicked));
			GUIAchievementScene.miscInfo[j] = Globals.Instance.AttDB.MiscDict.GetInfo(12 + j);
		}
		this.DailyScoreDesc = this.Slider.transform.Find("texttip").GetComponent<UILabel>();
		this.DailyScoreDesc.text = Singleton<StringManager>.Instance.GetString("DailyScoreDesc");
		LocalPlayer expr_32C = Globals.Instance.Player;
		expr_32C.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_32C.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		AchievementSubSystem expr_35C = Globals.Instance.Player.AchievementSystem;
		expr_35C.AchievementUpdateEvent = (AchievementSubSystem.AchievementUpdateCallback)Delegate.Combine(expr_35C.AchievementUpdateEvent, new AchievementSubSystem.AchievementUpdateCallback(this.OnAchievementUpdateEvent));
		AchievementSubSystem expr_38C = Globals.Instance.Player.AchievementSystem;
		expr_38C.AchievementTakeRewardEvent = (AchievementSubSystem.AchievementUpdateCallback)Delegate.Combine(expr_38C.AchievementTakeRewardEvent, new AchievementSubSystem.AchievementUpdateCallback(this.OnAchievementTakeRewardEvent));
		Globals.Instance.CliSession.Register(249, new ClientSession.MsgHandler(this.OnMsgTakeDailyScoreReward));
		Globals.Instance.CliSession.Register(235, new ClientSession.MsgHandler(this.OnMsgTakeCardDiamond));
		Globals.Instance.CliSession.Register(237, new ClientSession.MsgHandler(this.OnMsgTakeSuperCardDiamond));
		this.RefreshPlayerDataFlag = true;
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		LocalPlayer player = Globals.Instance.Player;
		if (!GUIAchievementScene.HasNewScore() && !player.AchievementSystem.HasTakeReward(true) && player.AchievementSystem.HasTakeReward(false))
		{
			this.tabPage[1].value = true;
		}
		this.RebuildAchievementGrid();
	}

	protected override void OnPreDestroyGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Hide();
		LocalPlayer expr_1B = Globals.Instance.Player;
		expr_1B.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_1B.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		AchievementSubSystem expr_4B = Globals.Instance.Player.AchievementSystem;
		expr_4B.AchievementUpdateEvent = (AchievementSubSystem.AchievementUpdateCallback)Delegate.Remove(expr_4B.AchievementUpdateEvent, new AchievementSubSystem.AchievementUpdateCallback(this.OnAchievementUpdateEvent));
		AchievementSubSystem expr_7B = Globals.Instance.Player.AchievementSystem;
		expr_7B.AchievementTakeRewardEvent = (AchievementSubSystem.AchievementUpdateCallback)Delegate.Remove(expr_7B.AchievementTakeRewardEvent, new AchievementSubSystem.AchievementUpdateCallback(this.OnAchievementTakeRewardEvent));
		Globals.Instance.CliSession.Unregister(249, new ClientSession.MsgHandler(this.OnMsgTakeDailyScoreReward));
		Globals.Instance.CliSession.Unregister(235, new ClientSession.MsgHandler(this.OnMsgTakeCardDiamond));
		Globals.Instance.CliSession.Unregister(237, new ClientSession.MsgHandler(this.OnMsgTakeSuperCardDiamond));
	}

	private void OnPlayerUpdateEvent()
	{
		this.RefreshPlayerDataFlag = true;
	}

	private void Update()
	{
		if (this.RefreshPlayerDataFlag)
		{
			this._RefreshDaliyInfo();
		}
	}

	private void _RefreshDaliyInfo()
	{
		this.RefreshPlayerDataFlag = false;
		LocalPlayer player = Globals.Instance.Player;
		for (int i = 0; i < 4; i++)
		{
			if (GUIAchievementScene.miscInfo[i] != null)
			{
				int dailyRewardFlag = player.Data.DailyRewardFlag;
				if ((dailyRewardFlag & 1 << i) != 0)
				{
					this.chest[i].spriteName = "chest_open";
					this.chestSFX[i].SetActive(false);
					this.chestTween[i].enabled = false;
					this.chest[i].transform.localRotation = Quaternion.identity;
				}
				else
				{
					this.chest[i].spriteName = "chest";
					this.chestSFX[i].SetActive(player.Data.DailyScore >= GUIAchievementScene.scoreValue[i]);
					this.chestTween[i].enabled = (player.Data.DailyScore >= GUIAchievementScene.scoreValue[i]);
				}
			}
		}
		this.Slider.value = (float)player.Data.DailyScore / 140f;
		this.DailyScore.text = player.Data.DailyScore.ToString();
	}

	private void OnTabClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
	}

	private void OnTabCheckChanged()
	{
		if (!UIToggle.current.value)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			if (UIToggle.current == this.tabPage[i] && this.curSelectTab != i)
			{
				this.curSelectTab = i;
				this.mAchievementTable[this.curSelectTab].repositionNow = true;
				this.Slider.gameObject.SetActive(this.curSelectTab == 0);
			}
		}
	}

	private void OnAchievementUpdateEvent(AchievementInfo info)
	{
		this.mAchievementTable[this.curSelectTab].repositionNow = true;
		LocalPlayer player = Globals.Instance.Player;
		if (info.Daily)
		{
			this.newFlag[0].SetActive(player.AchievementSystem.HasTakeReward(true) || GUIAchievementScene.HasNewScore());
		}
		else
		{
			this.newFlag[1].SetActive(player.AchievementSystem.HasTakeReward(false));
		}
	}

	private void OnAchievementTakeRewardEvent(AchievementInfo info)
	{
		List<RewardData> list = new List<RewardData>();
		for (int i = 0; i < info.RewardType.Count; i++)
		{
			if (info.RewardType[i] != 0 && info.RewardType[i] != 20)
			{
				list.Add(new RewardData
				{
					RewardType = info.RewardType[i],
					RewardValue1 = info.RewardValue1[i],
					RewardValue2 = info.RewardValue2[i]
				});
			}
		}
		GUIRewardPanel.Show(list, string.Format("{0}{1}", Singleton<StringManager>.Instance.GetString("QuestFinish"), info.Name), false, true, null, false);
		this.OnTakeAchievementRewardChecked(null);
		this.mAchievementTable[this.curSelectTab].repositionNow = true;
		LocalPlayer player = Globals.Instance.Player;
		if (info.Daily)
		{
			this.newFlag[0].SetActive(player.AchievementSystem.HasTakeReward(true));
		}
		else
		{
			this.newFlag[1].SetActive(player.AchievementSystem.HasTakeReward(false));
		}
	}

	private void OnTakeAchievementRewardChecked(GameObject go)
	{
		LocalPlayer player = Globals.Instance.Player;
		if (GameUIManager.mInstance.uiState.PlayerLevel < player.Data.Level)
		{
			GameUILevelupPanel instance = GameUILevelupPanel.GetInstance();
			instance.Init(GameUIManager.mInstance.uiCamera.transform, null);
		}
	}

	private void RebuildAchievementGrid()
	{
		this.mAchievementTable[0].ClearData();
		this.mAchievementTable[1].ClearData();
		bool flag = false;
		bool flag2 = false;
		AchievementSubSystem achievementSystem = Globals.Instance.Player.AchievementSystem;
		for (int i = 0; i < 67; i++)
		{
			AchievementDataEx curAchievement = achievementSystem.GetCurAchievement(i, true);
			if (curAchievement != null && curAchievement.Info != null && curAchievement.IsShowUI())
			{
				if (!flag && !curAchievement.Data.TakeReward && curAchievement.IsComplete())
				{
					flag = true;
				}
				this.mAchievementTable[0].AddData(curAchievement);
			}
		}
		for (int j = 0; j < 67; j++)
		{
			AchievementDataEx curAchievement = achievementSystem.GetCurAchievement(j, false);
			if (curAchievement != null && curAchievement.Info != null && curAchievement.IsShowUI())
			{
				if (!flag2 && !curAchievement.Data.TakeReward && curAchievement.IsComplete())
				{
					flag2 = true;
				}
				this.mAchievementTable[1].AddData(curAchievement);
			}
		}
		this.newFlag[0].SetActive(flag);
		this.newFlag[1].SetActive(flag2);
	}

	private void OnDailyScoreClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		int num = -1;
		for (int i = 0; i < this.chest.Length; i++)
		{
			if (this.chest[i].gameObject == go)
			{
				num = i;
				break;
			}
		}
		if (this.DailyScoreWnd != null)
		{
			this.DailyScoreWnd.Show(num);
		}
		else
		{
			GUIAchievementScene.RequestTalkDailyScoreReward(num);
		}
	}

	public void OnMsgTakeDailyScoreReward(MemoryStream stream)
	{
		try
		{
			MS2C_TakeDailyScoreReward mS2C_TakeDailyScoreReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeDailyScoreReward), stream) as MS2C_TakeDailyScoreReward;
			if (mS2C_TakeDailyScoreReward.Result == 0)
			{
				if (mS2C_TakeDailyScoreReward.Index >= 0 && mS2C_TakeDailyScoreReward.Index < GUIAchievementScene.scoreValue.Length)
				{
					MiscInfo miscInfo = GUIAchievementScene.miscInfo[mS2C_TakeDailyScoreReward.Index];
					if (miscInfo != null)
					{
						List<RewardData> list = new List<RewardData>(miscInfo.Day7RewardType.Count);
						for (int i = 0; i < miscInfo.Day7RewardType.Count; i++)
						{
							if (miscInfo.Day7RewardType[i] != 0 && miscInfo.Day7RewardType[i] != 20)
							{
								list.Add(new RewardData
								{
									RewardType = miscInfo.Day7RewardType[i],
									RewardValue1 = miscInfo.Day7RewardValue1[i],
									RewardValue2 = miscInfo.Day7RewardValue2[i]
								});
							}
						}
						GUIRewardPanel.Show(list, null, false, true, null, false);
					}
				}
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogException("MS2C_TakeDailyScoreReward Error", ex);
		}
	}

	private void OnMsgTakeCardDiamond(MemoryStream stream)
	{
		MS2C_TakeCardDiamond mS2C_TakeCardDiamond = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeCardDiamond), stream) as MS2C_TakeCardDiamond;
		if (mS2C_TakeCardDiamond.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_TakeCardDiamond.Result);
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (this.curSelectTab == 0)
		{
			this.mAchievementTable[this.curSelectTab].repositionNow = true;
		}
		this.newFlag[0].SetActive(player.AchievementSystem.HasTakeReward(true));
		AchievementDataEx curAchievement = Globals.Instance.Player.AchievementSystem.GetCurAchievement(16, true);
		if (curAchievement != null && curAchievement.Info != null && curAchievement.IsShowUI())
		{
			this.ShowAchievementReward(curAchievement);
		}
	}

	private void OnMsgTakeSuperCardDiamond(MemoryStream stream)
	{
		MS2C_TakeSuperCardDiamond mS2C_TakeSuperCardDiamond = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeSuperCardDiamond), stream) as MS2C_TakeSuperCardDiamond;
		if (mS2C_TakeSuperCardDiamond.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_TakeSuperCardDiamond.Result);
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (this.curSelectTab == 0)
		{
			this.mAchievementTable[this.curSelectTab].repositionNow = true;
		}
		this.newFlag[0].SetActive(player.AchievementSystem.HasTakeReward(true));
		AchievementDataEx curAchievement = Globals.Instance.Player.AchievementSystem.GetCurAchievement(17, true);
		if (curAchievement != null && curAchievement.Info != null && curAchievement.IsShowUI())
		{
			this.ShowAchievementReward(curAchievement);
		}
	}

	private void ShowAchievementReward(AchievementDataEx data)
	{
		List<RewardData> list = new List<RewardData>(1);
		for (int i = 0; i < data.Info.RewardType.Count; i++)
		{
			if (data.Info.RewardType[i] != 0 && data.Info.RewardType[i] != 7)
			{
				list.Add(new RewardData
				{
					RewardType = data.Info.RewardType[i],
					RewardValue1 = data.Info.RewardValue1[i],
					RewardValue2 = data.Info.RewardValue2[i]
				});
			}
		}
		if (list.Count > 0)
		{
			GUIRewardPanel.Show(list, string.Format("{0}{1}", Singleton<StringManager>.Instance.GetString("QuestFinish"), data.Info.Name), false, true, null, false);
		}
	}

	public static void RequestTalkDailyScoreReward(int index)
	{
		if (index == -1)
		{
			return;
		}
		if (GUIAchievementScene.miscInfo[index] == null)
		{
			global::Debug.LogErrorFormat("Daily Score config error {0}", new object[]
			{
				index
			});
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (player.Data.DailyScore < GUIAchievementScene.scoreValue[index])
		{
			return;
		}
		int dailyRewardFlag = player.Data.DailyRewardFlag;
		if ((dailyRewardFlag & 1 << index) != 0)
		{
			return;
		}
		MC2S_TakeDailyScoreReward mC2S_TakeDailyScoreReward = new MC2S_TakeDailyScoreReward();
		mC2S_TakeDailyScoreReward.Index = index;
		Globals.Instance.CliSession.Send(248, mC2S_TakeDailyScoreReward);
	}

	public static void RequestTakeAchievementReward(AchievementDataEx data)
	{
		if (data == null)
		{
			return;
		}
		if (!data.IsComplete() || !data.IsShowUI() || data.Data.TakeReward)
		{
			global::Debug.LogErrorFormat("Take Achievement Reward Error.", new object[0]);
			return;
		}
		if (data.Info.ConditionType == 16)
		{
			MC2S_TakeCardDiamond ojb = new MC2S_TakeCardDiamond();
			Globals.Instance.CliSession.Send(234, ojb);
		}
		else if (data.Info.ConditionType == 17)
		{
			MC2S_TakeSuperCardDiamond ojb2 = new MC2S_TakeSuperCardDiamond();
			Globals.Instance.CliSession.Send(236, ojb2);
		}
		else
		{
			MC2S_TakeAchievementReward mC2S_TakeAchievementReward = new MC2S_TakeAchievementReward();
			mC2S_TakeAchievementReward.AchievementID = data.Data.AchievementID;
			Globals.Instance.CliSession.Send(246, mC2S_TakeAchievementReward);
		}
		LocalPlayer player = Globals.Instance.Player;
		GameUIState uiState = GameUIManager.mInstance.uiState;
		uiState.PlayerLevel = player.Data.Level;
		uiState.PlayerEnergy = player.Data.Energy;
		uiState.PlayerExp = player.Data.Exp;
		uiState.PlayerMoney = player.Data.Money;
		uiState.SetOldFurtherData(Globals.Instance.Player.TeamSystem.GetPet(0));
	}
}
