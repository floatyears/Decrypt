using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GUIBossReadyScene : GameUISession
{
	private GameObject mTakenBtn;

	private GameObject mStartBtn;

	private GameObject mTakenBtnRedFlag;

	private GameObject readyPanel;

	private UILabel timeLb;

	private UILabel topPlayersLb;

	private UILabel mFDSNum;

	private UILabel startTimeLb;

	private bool updateTime;

	private int frameIndex;

	private int sendCount;

	public static void TryOpen()
	{
		if (Globals.Instance.Player.PetSystem.Values.Count == 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("pvpTxt18", 0f, 0f);
			return;
		}
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(1)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("pvpTxt1", new object[]
			{
				GameConst.GetInt32(1)
			}), 0f, 0f);
			return;
		}
		GameUIManager.mInstance.ChangeSession<GUIBossReadyScene>(null, false, true);
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("worldBossTxt5");
		Transform transform = base.transform.Find("winBg");
		UIEventListener expr_4B = UIEventListener.Get(transform.Find("rulesBtn").gameObject);
		expr_4B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4B.onClick, new UIEventListener.VoidDelegate(this.OnRulesBtnClicked));
		this.mStartBtn = transform.Find("StartBtn").gameObject;
		this.mStartBtn.SetActive(false);
		UIEventListener expr_99 = UIEventListener.Get(this.mStartBtn);
		expr_99.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_99.onClick, new UIEventListener.VoidDelegate(this.OnStartBtnClicked));
		this.startTimeLb = this.mStartBtn.transform.FindChild("tipTxt").GetComponent<UILabel>();
		this.readyPanel = transform.FindChild("readyPanel").gameObject;
		this.timeLb = this.readyPanel.transform.FindChild("Time").GetComponent<UILabel>();
		this.timeLb.gameObject.SetActive(false);
		this.mTakenBtn = transform.Find("takenBtn").gameObject;
		UIEventListener expr_142 = UIEventListener.Get(this.mTakenBtn);
		expr_142.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_142.onClick, new UIEventListener.VoidDelegate(this.OnTakenBtnClicked));
		this.mTakenBtnRedFlag = this.mTakenBtn.transform.Find("redFlag").gameObject;
		this.mTakenBtnRedFlag.SetActive(false);
		UILabel component = this.mTakenBtn.transform.Find("Label").GetComponent<UILabel>();
		component.overflowMethod = UILabel.Overflow.ShrinkContent;
		component.width = 158;
		Transform transform2 = this.readyPanel.transform.FindChild("TopPlayers");
		this.topPlayersLb = transform2.FindChild("Label").GetComponent<UILabel>();
		this.topPlayersLb.enabled = false;
		UIEventListener expr_209 = UIEventListener.Get(transform2.Find("billBoardBtn").gameObject);
		expr_209.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_209.onClick, new UIEventListener.VoidDelegate(this.OnBillBoardBtnClicked));
		this.mFDSNum = transform.Find("txt/Sprite/num").GetComponent<UILabel>();
		this.readyPanel.SetActive(false);
		LocalPlayer expr_256 = Globals.Instance.Player;
		expr_256.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_256.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdate));
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		WorldBossSubSystem expr_28A = worldBossSystem;
		expr_28A.GetBossDataEvent = (WorldBossSubSystem.VoidCallback)Delegate.Combine(expr_28A.GetBossDataEvent, new WorldBossSubSystem.VoidCallback(this.OnGetBossDataEvent));
		WorldBossSubSystem expr_2AD = worldBossSystem;
		expr_2AD.BossDeadEvent = (WorldBossSubSystem.BossDeadCallback)Delegate.Combine(expr_2AD.BossDeadEvent, new WorldBossSubSystem.BossDeadCallback(this.OnBossDeadEvent));
		Globals.Instance.CliSession.Register(648, new ClientSession.MsgHandler(this.OnMsgTakeFDSReward));
		Globals.Instance.CliSession.Register(651, new ClientSession.MsgHandler(this.OnMsgTakeKWBReward));
		this.updateTime = false;
		this.frameIndex = Time.frameCount;
		this.sendCount++;
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		GUIBossReadyScene.SendGetBossDataMsg();
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_WorldBossFirst, 0f);
	}

	protected override void OnPreDestroyGUI()
	{
		this.updateTime = false;
		GameUIManager.mInstance.GetTopGoods().Hide();
		LocalPlayer expr_20 = Globals.Instance.Player;
		expr_20.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_20.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdate));
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		WorldBossSubSystem expr_52 = worldBossSystem;
		expr_52.GetBossDataEvent = (WorldBossSubSystem.VoidCallback)Delegate.Remove(expr_52.GetBossDataEvent, new WorldBossSubSystem.VoidCallback(this.OnGetBossDataEvent));
		WorldBossSubSystem expr_74 = worldBossSystem;
		expr_74.BossDeadEvent = (WorldBossSubSystem.BossDeadCallback)Delegate.Remove(expr_74.BossDeadEvent, new WorldBossSubSystem.BossDeadCallback(this.OnBossDeadEvent));
		Globals.Instance.CliSession.Unregister(648, new ClientSession.MsgHandler(this.OnMsgTakeFDSReward));
		Globals.Instance.CliSession.Unregister(651, new ClientSession.MsgHandler(this.OnMsgTakeKWBReward));
	}

	public static void SendGetBossDataMsg()
	{
		MC2S_GetBossData mC2S_GetBossData = new MC2S_GetBossData();
		mC2S_GetBossData.Status = Globals.Instance.Player.WorldBossSystem.Status;
		mC2S_GetBossData.TimeStamp = Globals.Instance.Player.WorldBossSystem.TimeStamp;
		Globals.Instance.CliSession.Send(614, mC2S_GetBossData);
	}

	private void Update()
	{
		if (this.updateTime && this.timeLb != null && this.timeLb.gameObject.activeInHierarchy)
		{
			int timeStamp = Globals.Instance.Player.GetTimeStamp();
			int timeStamp2 = Globals.Instance.Player.WorldBossSystem.TimeStamp;
			if (timeStamp < timeStamp2)
			{
				this.timeLb.text = UIEnergyTooltip.FormatTime(timeStamp2 - timeStamp);
			}
			else
			{
				this.timeLb.text = UIEnergyTooltip.FormatTime(0);
				if (this.sendCount < 5 && Time.frameCount - this.frameIndex > 100)
				{
					this.updateTime = false;
					this.frameIndex = Time.frameCount;
					this.sendCount++;
					GUIBossReadyScene.SendGetBossDataMsg();
				}
			}
		}
		if (this.updateTime && this.startTimeLb != null && this.startTimeLb.gameObject.activeInHierarchy)
		{
			int timeStamp3 = Globals.Instance.Player.GetTimeStamp();
			int timeStamp4 = Globals.Instance.Player.WorldBossSystem.TimeStamp;
			int num = 0;
			if (timeStamp3 < timeStamp4)
			{
				num = GUIBossReadyScene.GetMinute(timeStamp4 - timeStamp3);
			}
			else
			{
				GameUIManager.mInstance.uiState.WorldBossIsOver = true;
				if (this.sendCount < 5 && Time.frameCount - this.frameIndex > 100)
				{
					this.updateTime = false;
					this.frameIndex = Time.frameCount;
					this.sendCount++;
					GUIBossReadyScene.SendGetBossDataMsg();
				}
			}
			this.startTimeLb.text = Singleton<StringManager>.Instance.GetString("worldBossTxt10", new object[]
			{
				30 - num
			});
		}
	}

	private int GetKillTime()
	{
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		return GUIBossReadyScene.GetMinute(worldBossSystem.KillElapsedTime);
	}

	public static int GetMinute(int second)
	{
		return second / 60 % 60;
	}

	public void OnRulesBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIWBRuleInfoPopUp.ShowMe();
	}

	public void OnTakenBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUILongLinRewardPopUp.ShowMe();
	}

	public void OnStartBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIBossMapScene>(null, false, false);
	}

	public void OnBillBoardBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.ShowBillBoard();
	}

	private void ShowBillBoard()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUICommonBillboardPopUp, false, null, null);
		GameUICommonBillboardPopUp gameUICommonBillboardPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUICommonBillboardPopUp;
		gameUICommonBillboardPopUp.InitBillboard("WorldBossRankKillItem");
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		RankData killerData = worldBossSystem.GetKillerData();
		bool flag = killerData != null;
		List<object> list = new List<object>();
		if (flag)
		{
			list.Add(killerData);
		}
		for (int i = 1; i < 31; i++)
		{
			RankData rankData = worldBossSystem.GetRankData(i);
			if (rankData != null)
			{
				list.Add(rankData);
			}
		}
		gameUICommonBillboardPopUp.InitItems(list, 3, (!flag) ? 1 : 0);
		string @string;
		if (worldBossSystem.Rank <= 31 && 0 < worldBossSystem.Rank)
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				worldBossSystem.Rank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				Singleton<StringManager>.Instance.GetString("Billboard0")
			});
		}
		gameUICommonBillboardPopUp.Refresh(Singleton<StringManager>.Instance.GetString("worldBossTxt8"), (!flag) ? null : string.Format(Singleton<StringManager>.Instance.GetString("worldBossTxt9"), this.GetKillTime(), killerData.Data.Name), @string, null, string.Format("[9e865a]{0}[-]{1}", Singleton<StringManager>.Instance.GetString("worldBossTxt3"), worldBossSystem.TotalDamage));
	}

	public void OnGetBossDataEvent()
	{
		this.Refresh();
	}

	public void OnBossDeadEvent(int slot, MonsterInfo info, string playerName)
	{
		if (slot == 5)
		{
			GameUIManager.mInstance.uiState.WorldBossIsOver = true;
			this.updateTime = false;
			GUIBossReadyScene.SendGetBossDataMsg();
		}
	}

	private void Refresh()
	{
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		if (worldBossSystem.Status == 2)
		{
			this.mStartBtn.SetActive(false);
			this.readyPanel.SetActive(true);
			this.timeLb.gameObject.SetActive(Tools.ServerDateTime(Globals.Instance.Player.GetTimeStamp()).Hour < 19);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("worldBossTxt6"));
			for (int i = 1; i <= 3; i++)
			{
				RankData rankData = worldBossSystem.GetRankData(i);
				if (rankData != null)
				{
					if (i > 1)
					{
						stringBuilder.Append(Singleton<StringManager>.Instance.GetString("Comma"));
					}
					stringBuilder.Append(rankData.Data.Name);
				}
			}
			RankData killerData = worldBossSystem.GetKillerData();
			if (killerData != null)
			{
				stringBuilder.Append(string.Format(Singleton<StringManager>.Instance.GetString("worldBossTxt7", new object[]
				{
					this.GetKillTime(),
					killerData.Data.Name
				}), new object[0]));
			}
			this.topPlayersLb.text = stringBuilder.ToString();
			this.topPlayersLb.enabled = true;
			if (GameUIManager.mInstance.uiState.WorldBossIsOver)
			{
				GameUIManager.mInstance.uiState.WorldBossIsOver = false;
				this.ShowBillBoard();
			}
		}
		else
		{
			this.mStartBtn.SetActive(true);
			this.readyPanel.SetActive(false);
		}
		this.RefreshFDSNum();
		this.updateTime = true;
	}

	private void RefreshFDSNum()
	{
		this.mFDSNum.text = Tools.FormatValue(Globals.Instance.Player.Data.FireDragonScale);
		this.mTakenBtnRedFlag.SetActive(Tools.IsFDSCanTaken() || Tools.IsWBRewardCanTaken());
	}

	private void OnPlayerUpdate()
	{
		this.RefreshFDSNum();
	}

	private void OnMsgTakeFDSReward(MemoryStream stream)
	{
		MS2C_TakeFDSReward mS2C_TakeFDSReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeFDSReward), stream) as MS2C_TakeFDSReward;
		if (mS2C_TakeFDSReward.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TakeFDSReward.Result == 0)
		{
			this.RefreshFDSNum();
		}
	}

	private void OnMsgTakeKWBReward(MemoryStream stream)
	{
		MS2C_TakeKillWorldBossReward mS2C_TakeKillWorldBossReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeKillWorldBossReward), stream) as MS2C_TakeKillWorldBossReward;
		if (mS2C_TakeKillWorldBossReward.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TakeKillWorldBossReward.Result == 0)
		{
			Globals.Instance.Player.WorldBossSystem.SetWBRewardFlag(mS2C_TakeKillWorldBossReward.HasReward);
			this.RefreshFDSNum();
		}
	}
}
