using Att;
using Proto;
using System;
using UnityEngine;

public sealed class UIVIPGiftPacks : MonoBehaviour
{
	private VipLevelInfo vipLevelInfo;

	private UILabel title;

	private UILabel mCondition;

	private UILabel mPrice;

	private UILabel mOffPrice;

	private UIButton mReceiveBtn;

	private UILabel mReceiveBtnLabel;

	private UIButton[] mReceiveBtns;

	private Transform[] mRewardParent = new Transform[4];

	private GameObject[] mRewards = new GameObject[4];

	private void Awake()
	{
		GameObject gameObject = GameUITools.FindGameObject("GiftBG", base.gameObject);
		this.title = GameUITools.FindUILabel("Title", gameObject);
		this.mCondition = GameUITools.FindUILabel("Condition", gameObject);
		this.mPrice = gameObject.transform.Find("price").GetComponent<UILabel>();
		this.mPrice.text = string.Empty;
		this.mOffPrice = gameObject.transform.Find("offprice").GetComponent<UILabel>();
		this.mOffPrice.text = string.Empty;
		this.mReceiveBtn = GameUITools.FindGameObject("ReceiveBtn", gameObject).GetComponent<UIButton>();
		this.mReceiveBtnLabel = GameUITools.FindUILabel("Label", this.mReceiveBtn.gameObject);
		this.mReceiveBtns = this.mReceiveBtn.GetComponents<UIButton>();
		for (int i = 0; i < this.mRewardParent.Length; i++)
		{
			this.mRewardParent[i] = gameObject.transform.Find(string.Format("Reward{0}", i));
		}
		UIEventListener expr_117 = UIEventListener.Get(this.mReceiveBtn.gameObject);
		expr_117.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_117.onClick, new UIEventListener.VoidDelegate(this.OnReceiveBtnClick));
	}

	public void Refresh(VipLevelInfo vipInfo)
	{
		this.vipLevelInfo = vipInfo;
		if (this.vipLevelInfo == null)
		{
			return;
		}
		this.title.text = Singleton<StringManager>.Instance.GetString("VIPTitle3", new object[]
		{
			this.vipLevelInfo.ID
		});
		this.RefreshRewards();
		this.mPrice.text = this.vipLevelInfo.Price.ToString();
		this.mOffPrice.text = this.vipLevelInfo.OffPrice.ToString();
		this.mOffPrice.color = ((Globals.Instance.Player.Data.Diamond >= this.vipLevelInfo.OffPrice) ? Color.white : Color.red);
		if ((ulong)Globals.Instance.Player.Data.VipLevel < (ulong)((long)this.vipLevelInfo.ID))
		{
			this.mCondition.text = Singleton<StringManager>.Instance.GetString("VIPDes14", new object[]
			{
				this.vipLevelInfo.ID
			});
			NGUITools.SetActive(this.mCondition.gameObject, true);
			NGUITools.SetActive(this.mReceiveBtn.gameObject, false);
		}
		else
		{
			NGUITools.SetActive(this.mReceiveBtn.gameObject, true);
			NGUITools.SetActive(this.mCondition.gameObject, false);
		}
		bool flag = Globals.Instance.Player.IsPayVipRewardTaken(this.vipLevelInfo);
		if (flag)
		{
			this.mReceiveBtn.isEnabled = false;
			for (int i = 0; i < this.mReceiveBtns.Length; i++)
			{
				this.mReceiveBtns[i].SetState(UIButtonColor.State.Disabled, true);
			}
			this.mReceiveBtnLabel.text = Singleton<StringManager>.Instance.GetString("VIPDes21");
		}
		else
		{
			this.mReceiveBtn.isEnabled = true;
			for (int j = 0; j < this.mReceiveBtns.Length; j++)
			{
				this.mReceiveBtns[j].SetState(UIButtonColor.State.Normal, true);
			}
			this.mReceiveBtnLabel.text = Singleton<StringManager>.Instance.GetString("VIPDes20");
		}
	}

	private void RefreshRewards()
	{
		for (int i = 0; i < this.mRewards.Length; i++)
		{
			UnityEngine.Object.Destroy(this.mRewards[i]);
			this.mRewards[i] = null;
		}
		int num = 0;
		int num2 = 0;
		while (num < this.mRewards.Length && num2 < this.vipLevelInfo.RewardType.Count)
		{
			if (this.vipLevelInfo.RewardType[num2] != 0)
			{
				GameObject gameObject = GameUITools.CreateReward(this.vipLevelInfo.RewardType[num2], this.vipLevelInfo.RewardValue1[num2], this.vipLevelInfo.RewardValue2[num2], this.mRewardParent[num], true, true, (float)((num2 % 2 != 0) ? -100 : 36), -7f, -2000f, 87f, 64f, 31f, 0);
				if (gameObject != null)
				{
					gameObject.AddComponent<UIDragScrollView>();
					this.mRewards[num] = gameObject;
					num++;
				}
			}
			num2++;
		}
	}

	private void OnReceiveBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.vipLevelInfo == null)
		{
			return;
		}
		UIVIPGiftPacks.RequestBuyVipReward(this.vipLevelInfo);
	}

	public static void RequestBuyVipReward(VipLevelInfo vipInfo)
	{
		if (vipInfo == null)
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		int vipLevel = Tools.GetVipLevel(vipInfo);
		if ((ulong)player.Data.VipLevel < (ulong)((long)vipLevel))
		{
			GameMessageBox.ShowRechargeMessageBox(string.Format(Singleton<StringManager>.Instance.GetString("VIPTip2"), vipLevel), null);
			return;
		}
		if (Globals.Instance.Player.IsPayVipRewardTaken(vipInfo))
		{
			return;
		}
		if (player.Data.Diamond < vipInfo.OffPrice)
		{
			Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, vipInfo.OffPrice, 0);
			return;
		}
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("VIPTitle4", new object[]
		{
			vipInfo.OffPrice,
			Tools.GetVIPPayRewardTitle(vipInfo)
		}), MessageBox.Type.OKCancel, vipInfo);
		GameMessageBox expr_BE = gameMessageBox;
		expr_BE.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_BE.OkClick, new MessageBox.MessageDelegate(UIVIPGiftPacks.OnPayVIPPayRewardChecked));
	}

	private static void OnPayVIPPayRewardChecked(object obj)
	{
		VipLevelInfo vipLevelInfo = (VipLevelInfo)obj;
		if (vipLevelInfo == null)
		{
			return;
		}
		MC2S_BuyVipReward mC2S_BuyVipReward = new MC2S_BuyVipReward();
		mC2S_BuyVipReward.VipInfoID = vipLevelInfo.ID;
		mC2S_BuyVipReward.Type = 0;
		Globals.Instance.CliSession.Send(232, mC2S_BuyVipReward);
	}

	public static void RequestBuyVipWeekReward(VipLevelInfo vipInfo)
	{
		if (vipInfo == null)
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		int vipLevel = Tools.GetVipLevel(vipInfo);
		if ((ulong)player.Data.VipLevel < (ulong)((long)vipLevel))
		{
			GameMessageBox.ShowRechargeMessageBox(string.Format(Singleton<StringManager>.Instance.GetString("VIPTip2"), vipLevel), null);
			return;
		}
		if (Globals.Instance.Player.IsVipWeekRewardBuy(vipInfo))
		{
			return;
		}
		if (player.Data.Diamond < vipInfo.WeekOffPrice)
		{
			Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, vipInfo.WeekOffPrice, 0);
			return;
		}
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("VIPTitle4", new object[]
		{
			vipInfo.WeekOffPrice,
			Tools.GetVIPWeekRewardTitle(vipInfo)
		}), MessageBox.Type.OKCancel, vipInfo);
		GameMessageBox expr_BE = gameMessageBox;
		expr_BE.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_BE.OkClick, new MessageBox.MessageDelegate(UIVIPGiftPacks.OnPayVIPWeekRewardChecked));
	}

	private static void OnPayVIPWeekRewardChecked(object obj)
	{
		VipLevelInfo vipLevelInfo = (VipLevelInfo)obj;
		if (vipLevelInfo == null)
		{
			return;
		}
		MC2S_BuyVipReward mC2S_BuyVipReward = new MC2S_BuyVipReward();
		mC2S_BuyVipReward.VipInfoID = vipLevelInfo.ID;
		mC2S_BuyVipReward.Type = 1;
		Globals.Instance.CliSession.Send(232, mC2S_BuyVipReward);
	}
}
