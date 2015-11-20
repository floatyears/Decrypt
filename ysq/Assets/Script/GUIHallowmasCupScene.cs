using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIHallowmasCupScene : GameUISession
{
	private ActivityHalloweenData mData;

	private UISprite mCupJinDu;

	private UISprite mDeedJinDu;

	private UISprite mZheZhaoJinDu;

	private UILabel mJinDuCount;

	private UILabel mJinDuCountDiamond;

	private UILabel mJinDuCountDiamondTxt;

	public UILabel[] num = new UILabel[4];

	private GameObject[] mScoreIcon = new GameObject[4];

	private GameObject[] mScoreEffect = new GameObject[4];

	private UILabel mTime;

	private UISprite[] mDiamondIcon = new UISprite[3];

	private UILabel[] mDeedTxt = new UILabel[3];

	private float RefreshTimeFlag = 0.1f;

	private float RefreshTimeFlag1 = 0.1f;

	private UILabel[] mBtnName = new UILabel[3];

	public int mCupScore;

	public int mLuckyNum;

	private GameObject mEffect;

	private HallowmasScore DailyScoreWnd;

	private float mXiShu = 0.9f;

	private UILabel mCurDiamond;

	private UILabel mCurScoreShow;

	private int lastTime = -1;

	private int lastTime1 = -1;

	private List<string> tempStrs = new List<string>();

	private void CreateObjects()
	{
		GameUITools.FindGameObject("Texture", base.gameObject).GetComponent<UITexture>().mainTexture = Res.Load<Texture>("MainBg/HallowmasCup", false);
		GameObject gameObject = base.transform.Find("slider").gameObject;
		this.mDeedJinDu = gameObject.transform.Find("Fg").GetComponent<UISprite>();
		for (int i = 0; i < 4; i++)
		{
			this.num[i] = gameObject.transform.Find(string.Format("p{0}", i)).GetComponent<UILabel>();
			this.mScoreEffect[i] = gameObject.transform.Find(string.Format("p{0}/ui65", i)).gameObject;
			this.mScoreIcon[i] = gameObject.transform.Find(string.Format("p{0}/icon", i)).gameObject;
			UIEventListener expr_E7 = UIEventListener.Get(this.mScoreIcon[i].gameObject);
			expr_E7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_E7.onClick, new UIEventListener.VoidDelegate(this.OnDailyScoreClicked));
		}
		GameObject gameObject2 = base.transform.Find("cup").gameObject;
		this.mCupJinDu = gameObject2.transform.Find("full").GetComponent<UISprite>();
		this.mJinDuCount = base.transform.Find("openNum").GetComponent<UILabel>();
		this.mCurDiamond = base.transform.Find("txt").GetComponent<UILabel>();
		this.mCurScoreShow = base.transform.Find("curScore").GetComponent<UILabel>();
		this.mJinDuCountDiamond = base.transform.Find("openDiamond").GetComponent<UILabel>();
		this.mJinDuCountDiamondTxt = base.transform.Find("openDiamond/txt").GetComponent<UILabel>();
		this.mZheZhaoJinDu = gameObject2.transform.Find("Panel/full2").GetComponent<UISprite>();
		for (int j = 0; j < 3; j++)
		{
			this.mDeedTxt[j] = base.transform.Find(string.Format("DeedBtn{0}/txt", j)).GetComponent<UILabel>();
			this.mDiamondIcon[j] = base.transform.Find(string.Format("DeedBtn{0}/txt/Sprite", j)).GetComponent<UISprite>();
			this.mBtnName[j] = base.transform.Find(string.Format("DeedBtn{0}/Label", j)).GetComponent<UILabel>();
		}
		this.mEffect = base.transform.Find("ui102_1").gameObject;
		this.mEffect.SetActive(false);
		this.mTime = base.transform.Find("time").GetComponent<UILabel>();
		this.DailyScoreWnd = base.transform.Find("DayReward").gameObject.AddComponent<HallowmasScore>();
		this.DailyScoreWnd.Init();
		this.DailyScoreWnd.gameObject.SetActive(false);
		GameUITools.RegisterClickEvent("rulesBtn", new UIEventListener.VoidDelegate(this.OnRulesBtnClick), base.gameObject);
		GameUITools.RegisterClickEvent("myDeedBtn", new UIEventListener.VoidDelegate(this.OnMyDeedBtnClick), base.gameObject);
		GameUITools.RegisterClickEvent("luckyDeedBtn", new UIEventListener.VoidDelegate(this.OnLuckyDeedBtnClick), base.gameObject);
		GameUITools.RegisterClickEvent("DeedBtn0", new UIEventListener.VoidDelegate(this.OnEquipDeedBtnClick), base.gameObject);
		GameUITools.RegisterClickEvent("DeedBtn1", new UIEventListener.VoidDelegate(this.OnPetDeedBtnClick), base.gameObject);
		GameUITools.RegisterClickEvent("DeedBtn2", new UIEventListener.VoidDelegate(this.OnItemDeedBtnClick), base.gameObject);
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("groupBuy_2");
		ActivitySubSystem expr_2B = Globals.Instance.Player.ActivitySystem;
		expr_2B.GetHalloweenDataEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_2B.GetHalloweenDataEvent, new ActivitySubSystem.VoidCallback(this.OnGetHalloweenDataEvent));
		ActivitySubSystem expr_5B = Globals.Instance.Player.ActivitySystem;
		expr_5B.ActivityHalloweenEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_5B.ActivityHalloweenEvent, new ActivitySubSystem.VoidCallback(this.OnActivityHalloweenUpdataEvent));
		ActivitySubSystem expr_8B = Globals.Instance.Player.ActivitySystem;
		expr_8B.GetHalloweenDiamondEvent = (ActivitySubSystem.AHBCallBack)Delegate.Combine(expr_8B.GetHalloweenDiamondEvent, new ActivitySubSystem.AHBCallBack(this.GetHalloweenDiamondEvent));
		ActivitySubSystem expr_BB = Globals.Instance.Player.ActivitySystem;
		expr_BB.GetHalloweenRewardScoreEvent = (ActivitySubSystem.AHBCallBack)Delegate.Combine(expr_BB.GetHalloweenRewardScoreEvent, new ActivitySubSystem.AHBCallBack(this.OnGetHalloweenRewardScoreEvent));
		this.Refresh();
		MC2S_ActivityHalloweenInfo ojb = new MC2S_ActivityHalloweenInfo();
		Globals.Instance.CliSession.Send(781, ojb);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Hide();
		ActivitySubSystem expr_1E = Globals.Instance.Player.ActivitySystem;
		expr_1E.ActivityHalloweenEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_1E.ActivityHalloweenEvent, new ActivitySubSystem.VoidCallback(this.OnActivityHalloweenUpdataEvent));
		ActivitySubSystem expr_4E = Globals.Instance.Player.ActivitySystem;
		expr_4E.GetHalloweenDataEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_4E.GetHalloweenDataEvent, new ActivitySubSystem.VoidCallback(this.OnGetHalloweenDataEvent));
		ActivitySubSystem expr_7E = Globals.Instance.Player.ActivitySystem;
		expr_7E.GetHalloweenDiamondEvent = (ActivitySubSystem.AHBCallBack)Delegate.Remove(expr_7E.GetHalloweenDiamondEvent, new ActivitySubSystem.AHBCallBack(this.GetHalloweenDiamondEvent));
		ActivitySubSystem expr_AE = Globals.Instance.Player.ActivitySystem;
		expr_AE.GetHalloweenRewardScoreEvent = (ActivitySubSystem.AHBCallBack)Delegate.Remove(expr_AE.GetHalloweenRewardScoreEvent, new ActivitySubSystem.AHBCallBack(this.OnGetHalloweenRewardScoreEvent));
	}

	private void Refresh()
	{
		this.mData = Globals.Instance.Player.ActivitySystem.HData;
		int hScore = Globals.Instance.Player.ActivitySystem.HScore;
		List<ActivityHalloweenScoreReward> scoreReward = Globals.Instance.Player.ActivitySystem.HData.Ext.ScoreReward;
		if (Globals.Instance.Player.ActivitySystem.PlayerScore > 0)
		{
			this.mCurScoreShow.text = Singleton<StringManager>.Instance.GetString("festival28", new object[]
			{
				Globals.Instance.Player.ActivitySystem.PlayerScore
			});
		}
		else
		{
			this.mCurScoreShow.text = string.Empty;
		}
		for (int i = 0; i < scoreReward.Count; i++)
		{
			UITweener[] components = this.mScoreIcon[i].GetComponents<UITweener>();
			this.num[i].text = Globals.Instance.Player.ActivitySystem.HData.Ext.ScoreReward[i].Score.ToString();
			if (Globals.Instance.Player.ActivitySystem.PlayerScore < scoreReward[i].Score)
			{
				this.mScoreEffect[i].SetActive(false);
				for (int j = 0; j < components.Length; j++)
				{
					components[j].enabled = false;
				}
			}
			else if (GUIHallowmasCupScene.IsGet(i))
			{
				this.mScoreEffect[i].SetActive(false);
				for (int k = 0; k < components.Length; k++)
				{
					components[k].enabled = false;
				}
				this.mScoreIcon[i].transform.localRotation = Quaternion.identity;
			}
			else
			{
				for (int l = 0; l < components.Length; l++)
				{
					components[l].enabled = true;
				}
				this.mScoreEffect[i].SetActive(true);
			}
		}
		float num = 0f;
		for (int m = 0; m < scoreReward.Count; m++)
		{
			if (Globals.Instance.Player.ActivitySystem.PlayerScore <= scoreReward[m].Score)
			{
				float num2 = 0f;
				if (m > 0)
				{
					num2 = (float)scoreReward[m - 1].Score;
				}
				num += ((float)Globals.Instance.Player.ActivitySystem.PlayerScore - num2) / ((float)scoreReward[m].Score - num2);
				break;
			}
			num += 1f;
		}
		this.mDeedJinDu.fillAmount = num * (1f / (float)scoreReward.Count);
		this.mZheZhaoJinDu.fillAmount = 1f - this.mXiShu * (float)hScore / (float)this.mData.Ext.FireScore;
		if (Globals.Instance.Player.ActivitySystem.FireRewardTimestamp - Globals.Instance.Player.GetTimeStamp() > 0)
		{
			Globals.Instance.Player.ActivitySystem.HData.Fire = true;
			this.mEffect.SetActive(true);
		}
		else
		{
			this.mEffect.SetActive(false);
		}
		if (Globals.Instance.Player.ActivitySystem.FireRewardTimestamp - Globals.Instance.Player.GetTimeStamp() > 0)
		{
			this.mBtnName[0].text = Singleton<StringManager>.Instance.GetString("festival21");
			this.mBtnName[1].text = Singleton<StringManager>.Instance.GetString("festival22");
			this.mBtnName[2].text = Singleton<StringManager>.Instance.GetString("festival23");
			this._RefreshEndTime();
			this.mJinDuCountDiamond.gameObject.SetActive(false);
			this.mCurDiamond.gameObject.SetActive(true);
			this.mCurDiamond.text = Singleton<StringManager>.Instance.GetString("festival27", new object[]
			{
				Globals.Instance.Player.ActivitySystem.HScore
			});
		}
		else
		{
			this.mCurDiamond.gameObject.SetActive(false);
			this.mBtnName[0].text = Singleton<StringManager>.Instance.GetString("festival24");
			this.mBtnName[1].text = Singleton<StringManager>.Instance.GetString("festival25");
			this.mBtnName[2].text = Singleton<StringManager>.Instance.GetString("festival26");
			this.mJinDuCount.text = string.Empty;
			this.mJinDuCountDiamond.gameObject.SetActive(true);
			this.mJinDuCountDiamond.text = Singleton<StringManager>.Instance.GetString("festival19", new object[]
			{
				hScore,
				this.mData.Ext.FireScore
			});
			this.mJinDuCountDiamondTxt.text = Singleton<StringManager>.Instance.GetString("festival16");
		}
		this.mCupJinDu.fillAmount = this.mXiShu * (float)hScore / (float)this.mData.Ext.FireScore;
		List<int> hFreeContractIDs = Globals.Instance.Player.ActivitySystem.HFreeContractIDs;
		List<ActivityHalloweenItem> data = this.mData.Ext.Data;
		for (int n = 0; n < data.Count; n++)
		{
			bool flag = false;
			if (hFreeContractIDs != null)
			{
				for (int num3 = 0; num3 < hFreeContractIDs.Count; num3++)
				{
					if (hFreeContractIDs[num3] == data[n].ID)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				this.mDiamondIcon[n].gameObject.SetActive(false);
				this.mDeedTxt[n].text = Singleton<StringManager>.Instance.GetString("festival5");
				this.mDeedTxt[n].transform.localPosition = new Vector3(0f, -41f, 0f);
			}
			else
			{
				this.mDiamondIcon[n].gameObject.SetActive(true);
				this.mDeedTxt[n].text = Singleton<StringManager>.Instance.GetString("festival1", new object[]
				{
					data[n].Cost
				});
				this.mDeedTxt[n].transform.localPosition = new Vector3(15f, -41f, 0f);
			}
		}
	}

	private void _RefreshWarFreeTime()
	{
		if (Globals.Instance.Player == null)
		{
			return;
		}
		this.RefreshTimeFlag = 1f;
		int num = (Globals.Instance.Player.ActivitySystem.HRewardTimestamp <= 0) ? 0 : (Globals.Instance.Player.ActivitySystem.HRewardTimestamp - Globals.Instance.Player.GetTimeStamp());
		if (num == 0 && this.lastTime == num)
		{
			return;
		}
		this.lastTime = num;
		if (num > 0)
		{
			this.mTime.text = Singleton<StringManager>.Instance.GetString("festival3", new object[]
			{
				Tools.FormatTime(num)
			});
		}
		else
		{
			this.mTime.text = string.Empty;
		}
	}

	private void _RefreshEndTime()
	{
		if (Globals.Instance.Player == null)
		{
			return;
		}
		this.RefreshTimeFlag1 = 1f;
		int num = (Globals.Instance.Player.ActivitySystem.FireRewardTimestamp <= 0) ? 0 : (Globals.Instance.Player.ActivitySystem.FireRewardTimestamp - Globals.Instance.Player.GetTimeStamp());
		if (num == 0 && this.lastTime1 == num)
		{
			return;
		}
		this.lastTime1 = num;
		this.mData = Globals.Instance.Player.ActivitySystem.HData;
		if (num > 0)
		{
			this.mJinDuCount.text = Singleton<StringManager>.Instance.GetString("festival15", new object[]
			{
				Tools.FormatTime(num)
			});
		}
	}

	private void Update()
	{
		this.RefreshTimeFlag -= Time.fixedDeltaTime;
		if (this.RefreshTimeFlag < 0f)
		{
			this._RefreshWarFreeTime();
		}
		this.RefreshTimeFlag1 -= Time.fixedDeltaTime;
		if (this.RefreshTimeFlag1 < 0f)
		{
			this._RefreshEndTime();
		}
	}

	private bool IsFree(int index)
	{
		List<int> hFreeContractIDs = Globals.Instance.Player.ActivitySystem.HFreeContractIDs;
		List<ActivityHalloweenItem> data = Globals.Instance.Player.ActivitySystem.HData.Ext.Data;
		if (hFreeContractIDs != null)
		{
			for (int i = 0; i < hFreeContractIDs.Count; i++)
			{
				if (hFreeContractIDs[i] == data[index].ID)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsGet(int index)
	{
		List<int> hGetedIDs = Globals.Instance.Player.ActivitySystem.HGetedIDs;
		List<ActivityHalloweenScoreReward> scoreReward = Globals.Instance.Player.ActivitySystem.HData.Ext.ScoreReward;
		if (hGetedIDs != null)
		{
			for (int i = 0; i < hGetedIDs.Count; i++)
			{
				if (hGetedIDs[i] == scoreReward[index].ID)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void SendDeedMsg(int index)
	{
		MC2S_ActivityHalloweenBuy mC2S_ActivityHalloweenBuy = new MC2S_ActivityHalloweenBuy();
		for (int i = 0; i < Globals.Instance.Player.ActivitySystem.HData.Ext.Data.Count; i++)
		{
			mC2S_ActivityHalloweenBuy.ID = Globals.Instance.Player.ActivitySystem.HData.Ext.Data[index].ID;
		}
		Globals.Instance.CliSession.Send(783, mC2S_ActivityHalloweenBuy);
	}

	private void OnItemDeedBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.SendDeedMsg(2);
	}

	private void OnPetDeedBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.SendDeedMsg(1);
	}

	private void OnEquipDeedBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.SendDeedMsg(0);
	}

	private void OnLuckyDeedBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIHallowmasDeedPopUp.Show(1);
	}

	private void OnMyDeedBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIHallowmasDeedPopUp.Show(0);
	}

	private void OnRulesBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIRuleInfoPopUp.ShowThis("groupBuy", "festival17");
	}

	private void OnDailyScoreClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		int num = -1;
		for (int i = 0; i < this.mScoreIcon.Length; i++)
		{
			if (this.mScoreIcon[i].gameObject == go)
			{
				num = i;
				break;
			}
		}
		List<ActivityHalloweenScoreReward> scoreReward = Globals.Instance.Player.ActivitySystem.HData.Ext.ScoreReward;
		if (Globals.Instance.Player.ActivitySystem.PlayerScore > scoreReward[num].Score && !GUIHallowmasCupScene.IsGet(num))
		{
			GUIHallowmasCupScene.RequestTalkScoreReward(num);
		}
		else if (this.DailyScoreWnd != null)
		{
			this.DailyScoreWnd.Show(num);
		}
	}

	public void OnGetHalloweenDataEvent()
	{
		this.Refresh();
	}

	public void OnActivityHalloweenUpdataEvent()
	{
		this.Refresh();
	}

	public void GetHalloweenDiamondEvent(int diamonds)
	{
		if (diamonds < 0)
		{
			this.tempStrs.Clear();
			this.tempStrs.Add(Singleton<StringManager>.Instance.GetString("festival18", new object[]
			{
				-diamonds
			}));
			GameUIManager.mInstance.ShowAttributeTip(this.tempStrs, 2f, 0.4f, 0f, 200f);
		}
		LocalPlayer player = Globals.Instance.Player;
		if (player.ActivitySystem.HRewardData != null && player.ActivitySystem.HRewardData.Count > 0)
		{
			GUIRewardPanel.Show(player.ActivitySystem.HRewardData, Singleton<StringManager>.Instance.GetString("festival20"), false, true, null, false);
		}
		this.Refresh();
	}

	public void OnGetHalloweenRewardScoreEvent(int id)
	{
		LocalPlayer player = Globals.Instance.Player;
		this.DailyScoreWnd.IsTakeBtnTrue(id - 1);
		this.Refresh();
		if (player.ActivitySystem.HData.Ext.ScoreReward.Count >= id && id > 0 && player.ActivitySystem.HData.Ext.ScoreReward[id - 1].Rewards.Count > 0)
		{
			GUIRewardPanel.Show(player.ActivitySystem.HData.Ext.ScoreReward[id - 1].Rewards, Singleton<StringManager>.Instance.GetString("festival20"), false, true, null, false);
		}
	}

	public static void RequestTalkScoreReward(int index)
	{
		LocalPlayer player = Globals.Instance.Player;
		if (index == -1)
		{
			return;
		}
		if (player.ActivitySystem.HData.Ext.ScoreReward[index] == null)
		{
			global::Debug.LogErrorFormat("Daily Score config error {0}", new object[]
			{
				index
			});
			return;
		}
		if (player.ActivitySystem.PlayerScore < player.ActivitySystem.HData.Ext.ScoreReward[index].Score)
		{
			return;
		}
		if (GUIHallowmasCupScene.IsGet(index))
		{
			return;
		}
		MC2S_ActivityHalloweenScoreReward mC2S_ActivityHalloweenScoreReward = new MC2S_ActivityHalloweenScoreReward();
		mC2S_ActivityHalloweenScoreReward.ID = player.ActivitySystem.HData.Ext.ScoreReward[index].ID;
		Globals.Instance.CliSession.Send(787, mC2S_ActivityHalloweenScoreReward);
	}
}
