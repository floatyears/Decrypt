using Proto;
using System;
using UnityEngine;

public class GUIFundRewardInfo : MonoBehaviour
{
	private UIToggle[] tabPage = new UIToggle[2];

	private GameObject[] pageNewFlag = new GameObject[2];

	private FundRewardGrid[] mFundRewardTable = new FundRewardGrid[2];

	private int curSelectTab;

	private UILabel diamondReward;

	private UILabel num;

	private GameObject btnBuyFund;

	private GameObject btnHasBuy;

	public void Init()
	{
		for (int i = 0; i < 2; i++)
		{
			this.mFundRewardTable[i] = base.transform.FindChild(string.Format("bagPanel{0}/bagContents", i)).gameObject.AddComponent<FundRewardGrid>();
			this.mFundRewardTable[i].maxPerLine = 1;
			this.mFundRewardTable[i].arrangement = UICustomGrid.Arrangement.Vertical;
			this.mFundRewardTable[i].cellWidth = 662f;
			this.mFundRewardTable[i].cellHeight = 100f;
			this.tabPage[i] = base.transform.Find(string.Format("bag{0}", i)).GetComponent<UIToggle>();
			EventDelegate.Add(this.tabPage[i].onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
			this.pageNewFlag[i] = this.tabPage[i].transform.Find("new").gameObject;
			this.pageNewFlag[i].SetActive(false);
		}
		this.diamondReward = base.transform.Find("diamond").GetComponent<UILabel>();
		this.num = base.transform.Find("num").GetComponent<UILabel>();
		this.btnBuyFund = base.transform.Find("ToGet").gameObject;
		UIEventListener expr_159 = UIEventListener.Get(this.btnBuyFund.gameObject);
		expr_159.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_159.onClick, new UIEventListener.VoidDelegate(this.OnBuyFundClick));
		this.btnHasBuy = base.transform.Find("Buy").gameObject;
		uint @int = (uint)GameConst.GetInt32(77);
		UILabel component = base.transform.Find("FundTips").GetComponent<UILabel>();
		component.text = Singleton<StringManager>.Instance.GetString("activityFund5", new object[]
		{
			@int,
			@int * 10u
		});
		LocalPlayer player = Globals.Instance.Player;
		for (int j = 0; j < player.ActivitySystem.FundRewards.Count; j++)
		{
			FundRewardData fundRewardData = player.ActivitySystem.FundRewards[j];
			this.mFundRewardTable[(!fundRewardData.IsWelfare) ? 0 : 1].AddData(fundRewardData);
		}
	}

	public void Refresh()
	{
		this.mFundRewardTable[this.curSelectTab].repositionNow = true;
		bool active;
		bool active2;
		GUIFundRewardInfo.CanTakeRewardEx(out active, out active2);
		this.pageNewFlag[0].gameObject.SetActive(active);
		this.pageNewFlag[1].gameObject.SetActive(active2);
		LocalPlayer player = Globals.Instance.Player;
		if (player.ActivitySystem.HasBuyFund())
		{
			this.btnHasBuy.SetActive(true);
			this.btnBuyFund.SetActive(false);
		}
		else
		{
			this.btnHasBuy.SetActive(false);
			this.btnBuyFund.SetActive(true);
		}
		int num;
		int num2;
		GUIFundRewardInfo.GetFundRewardDiamond(out num, out num2);
		this.diamondReward.text = Singleton<StringManager>.Instance.GetString("activityFund4", new object[]
		{
			num2,
			num - num2
		});
		this.num.text = string.Format("{0:D4}", player.ActivitySystem.BuyFundNum % 10000);
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
				Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
				this.curSelectTab = i;
				this.mFundRewardTable[this.curSelectTab].repositionNow = true;
			}
		}
	}

	private void OnBuyFundClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		LocalPlayer player = Globals.Instance.Player;
		if (player.ActivitySystem.HasBuyFund())
		{
			return;
		}
		int @int = GameConst.GetInt32(52);
		if ((ulong)player.Data.VipLevel < (ulong)((long)@int))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("activityFund3", new object[]
			{
				@int
			}), 0f, 0f);
			return;
		}
		int int2 = GameConst.GetInt32(77);
		if (player.Data.Diamond < int2)
		{
			Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, int2, 0);
			return;
		}
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("VIPTitle5"), MessageBox.Type.OK, null);
		GameMessageBox expr_BA = gameMessageBox;
		expr_BA.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_BA.OkClick, new MessageBox.MessageDelegate(this.OnBuyFundChecked));
	}

	private void OnBuyFundChecked(object go)
	{
		MC2S_BuyFund ojb = new MC2S_BuyFund();
		Globals.Instance.CliSession.Send(735, ojb);
	}

	public void OnBuyFundNumUpdateEvent()
	{
		if (base.gameObject.activeSelf)
		{
			this.Refresh();
		}
	}

	public static void GetFundRewardDiamond(out int total, out int taken)
	{
		total = 0;
		taken = 0;
		LocalPlayer player = Globals.Instance.Player;
		for (int i = 0; i < player.ActivitySystem.FundRewards.Count; i++)
		{
			FundRewardData fundRewardData = player.ActivitySystem.FundRewards[i];
			if (!fundRewardData.IsWelfare)
			{
				total += fundRewardData.Info.FundDiamond;
				if (fundRewardData.IsTakeReward())
				{
					taken += fundRewardData.Info.FundDiamond;
				}
			}
		}
	}

	public static void CanTakeRewardEx(out bool newFund, out bool newWelfare)
	{
		newFund = false;
		newWelfare = false;
		LocalPlayer player = Globals.Instance.Player;
		for (int i = 0; i < player.ActivitySystem.FundRewards.Count; i++)
		{
			FundRewardData fundRewardData = player.ActivitySystem.FundRewards[i];
			if (fundRewardData.IsComplete() && !fundRewardData.IsTakeReward())
			{
				if (fundRewardData.IsWelfare)
				{
					newWelfare = true;
				}
				else
				{
					newFund = true;
				}
				if (newWelfare && newFund)
				{
					break;
				}
			}
		}
	}

	public static bool CanTakeReward()
	{
		LocalPlayer player = Globals.Instance.Player;
		for (int i = 0; i < player.ActivitySystem.FundRewards.Count; i++)
		{
			FundRewardData fundRewardData = player.ActivitySystem.FundRewards[i];
			if (fundRewardData.IsComplete() && !fundRewardData.IsTakeReward())
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsOpen()
	{
		return true;
	}
}
