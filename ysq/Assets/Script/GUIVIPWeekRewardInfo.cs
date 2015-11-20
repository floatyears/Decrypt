using Att;
using Holoville.HOTween;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIVIPWeekRewardInfo : MonoBehaviour
{
	private UILabel ResetTimer;

	private UILabel Title;

	private Transform Reward;

	private GameObject[] RewardItem = new GameObject[4];

	private UILabel Price;

	private UILabel OffPrice;

	private UILabel BuyVIPLevel;

	private UILabel CurVipLevel;

	private Transform ToBuy;

	private Transform Buyed;

	private UIScrollView rewardScrollView;

	private UITable rewardTable;

	private UIScrollBar mTabBtnScrollBar;

	private GameObject leftBtn;

	private GameObject rightBtn;

	private static int CurViewVIPLevel;

	private VipLevelInfo curVipLevelInfo;

	private UISprite[] VIPLevel = new UISprite[16];

	private float time1Flag = 0.2f;

	public void Init()
	{
		this.ResetTimer = base.transform.Find("time1").GetComponent<UILabel>();
		Transform transform = base.transform.Find("RewardBK");
		this.Title = transform.transform.Find("Title").GetComponent<UILabel>();
		this.Reward = transform.transform.Find("Reward");
		this.Price = transform.transform.Find("Price").GetComponent<UILabel>();
		this.OffPrice = transform.transform.Find("OffPrice").GetComponent<UILabel>();
		this.BuyVIPLevel = transform.transform.Find("BuyVIPLevel").GetComponent<UILabel>();
		this.CurVipLevel = transform.transform.Find("CurVipLevel").GetComponent<UILabel>();
		this.ToBuy = transform.transform.Find("GoBtn");
		this.Buyed = transform.transform.Find("Buy");
		UIEventListener expr_105 = UIEventListener.Get(this.ToBuy.gameObject);
		expr_105.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_105.onClick, new UIEventListener.VoidDelegate(this.OnBuyVipWeekReward));
		this.rewardScrollView = base.transform.Find("rewardPanel").GetComponent<UIScrollView>();
		this.rewardTable = this.rewardScrollView.transform.Find("rewardContents").GetComponent<UITable>();
		for (int i = 0; i <= 15; i++)
		{
			this.VIPLevel[i] = this.rewardTable.transform.Find(string.Format("{0:D2}", i)).GetComponent<UISprite>();
			UIEventListener expr_1A7 = UIEventListener.Get(this.VIPLevel[i].gameObject);
			expr_1A7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1A7.onClick, new UIEventListener.VoidDelegate(this.OnVIPLevelClicked));
		}
		this.mTabBtnScrollBar = base.transform.Find("scrollBar").GetComponent<UIScrollBar>();
		EventDelegate.Add(this.mTabBtnScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBarValueChange));
		this.leftBtn = base.transform.Find("LeftBtn").gameObject;
		this.rightBtn = base.transform.Find("RightBtn").gameObject;
		UIEventListener expr_24D = UIEventListener.Get(this.leftBtn);
		expr_24D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_24D.onClick, new UIEventListener.VoidDelegate(this.OnLeftBtnClicked));
		UIEventListener expr_279 = UIEventListener.Get(this.rightBtn);
		expr_279.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_279.onClick, new UIEventListener.VoidDelegate(this.OnRightBtnClicked));
		this.RefreshCurLevelToggle();
		this.RefreshCurLevelInfo();
	}

	public void Refresh()
	{
		if (Globals.Instance.Player != null)
		{
			GameCache.Data.VIPWeekRewardStamp = Globals.Instance.Player.GetTimeStamp();
			GameCache.UpdateNow = true;
		}
		this.RefreshCurLevelToggle();
		this.RefreshCurLevelInfo();
	}

	private void Update()
	{
		this.RefreshTime();
	}

	public void OnMsgBuyVipReward(MemoryStream stream)
	{
		MS2C_BuyVipReward mS2C_BuyVipReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_BuyVipReward), stream) as MS2C_BuyVipReward;
		if (mS2C_BuyVipReward.Type != 1)
		{
			return;
		}
		if (mS2C_BuyVipReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_BuyVipReward.Result);
			return;
		}
		base.StartCoroutine(this.DoReward(mS2C_BuyVipReward.VipInfoID));
		this.RefreshCurLevelInfo();
	}

	[DebuggerHidden]
	private IEnumerator DoReward(int VipInfoID)
	{
        return null;
        //GUIVIPWeekRewardInfo.<DoReward>c__Iterator32 <DoReward>c__Iterator = new GUIVIPWeekRewardInfo.<DoReward>c__Iterator32();
        //<DoReward>c__Iterator.VipInfoID = VipInfoID;
        //<DoReward>c__Iterator.<$>VipInfoID = VipInfoID;
        //return <DoReward>c__Iterator;
	}

	private void RefreshTime()
	{
		this.time1Flag -= Time.deltaTime;
		if (this.ResetTimer != null && this.time1Flag < 0f)
		{
			int num = Globals.Instance.Player.Data.WeekTimestamp - Globals.Instance.Player.GetTimeStamp();
			if (num <= 0)
			{
				this.ResetTimer.text = string.Empty;
				this.time1Flag = 3.40282347E+38f;
			}
			else
			{
				this.ResetTimer.text = Singleton<StringManager>.Instance.GetString("VIPTip5", new object[]
				{
					Tools.FormatTimeStr2(num, false, false)
				});
				this.time1Flag = 1f;
			}
		}
	}

	private void OnBuyVipWeekReward(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.curVipLevelInfo == null)
		{
			return;
		}
		UIVIPGiftPacks.RequestBuyVipWeekReward(this.curVipLevelInfo);
	}

	private void OnVIPLevelClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		for (int i = 0; i < this.VIPLevel.Length; i++)
		{
			if (this.VIPLevel[i].gameObject == go)
			{
				if (GUIVIPWeekRewardInfo.CurViewVIPLevel != i)
				{
					GUIVIPWeekRewardInfo.CurViewVIPLevel = i;
					this.RefreshCurLevelToggle();
					this.RefreshCurLevelInfo();
				}
				break;
			}
		}
	}

	private void RefreshCurLevelToggle()
	{
		for (int i = 0; i < this.VIPLevel.Length; i++)
		{
			if (GUIVIPWeekRewardInfo.CurViewVIPLevel == i)
			{
				this.VIPLevel[i].spriteName = "btnBg2";
			}
			else
			{
				this.VIPLevel[i].spriteName = "btnBg1";
			}
		}
		this.rewardTable.repositionNow = true;
	}

	private void RefreshCurLevelInfo()
	{
		GUIVIPWeekRewardInfo.CurViewVIPLevel = Mathf.Clamp(GUIVIPWeekRewardInfo.CurViewVIPLevel, 0, 15);
		this.curVipLevelInfo = Tools.GetVipLevelInfo(GUIVIPWeekRewardInfo.CurViewVIPLevel);
		if (this.curVipLevelInfo == null)
		{
			global::Debug.LogErrorFormat("Error Vip level {0}", new object[]
			{
				GUIVIPWeekRewardInfo.CurViewVIPLevel
			});
			return;
		}
		this.Title.text = Tools.GetVIPWeekRewardTitle(this.curVipLevelInfo);
		for (int i = 0; i < this.RewardItem.Length; i++)
		{
			if (this.RewardItem[i] != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem[i]);
				this.RewardItem[i] = null;
			}
		}
		int num = 0;
		for (int j = 0; j < this.curVipLevelInfo.WeekRewardType.Count; j++)
		{
			if (this.curVipLevelInfo.WeekRewardType[j] != 0 && this.curVipLevelInfo.WeekRewardType[j] != 20)
			{
				this.RewardItem[num] = GameUITools.CreateReward(this.curVipLevelInfo.WeekRewardType[j], this.curVipLevelInfo.WeekRewardValue1[j], this.curVipLevelInfo.WeekRewardValue2[j], this.Reward, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
				if (this.RewardItem[num] != null)
				{
					this.RewardItem[num].gameObject.AddComponent<UIDragScrollView>();
					this.RewardItem[num].transform.localPosition = new Vector3((float)(num * 106), 0f, 0f);
					num++;
				}
				if (num >= this.RewardItem.Length)
				{
					break;
				}
			}
		}
		this.Price.text = this.curVipLevelInfo.WeekPrice.ToString();
		this.OffPrice.text = this.curVipLevelInfo.WeekOffPrice.ToString();
		this.BuyVIPLevel.text = Singleton<StringManager>.Instance.GetString("VIPTip3", new object[]
		{
			GUIVIPWeekRewardInfo.CurViewVIPLevel
		});
		this.CurVipLevel.text = Singleton<StringManager>.Instance.GetString("VIPTip4", new object[]
		{
			Globals.Instance.Player.Data.VipLevel
		});
		if (Globals.Instance.Player.IsVipWeekRewardBuy(this.curVipLevelInfo))
		{
			this.ToBuy.gameObject.SetActive(false);
			this.Buyed.gameObject.SetActive(true);
		}
		else
		{
			this.ToBuy.gameObject.SetActive(true);
			this.Buyed.gameObject.SetActive(false);
		}
	}

	private void OnLeftBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		HOTween.To(this.mTabBtnScrollBar, 0.25f, new TweenParms().Prop("value", this.mTabBtnScrollBar.value - 4f / (float)(this.VIPLevel.Length - 5)));
	}

	private void OnRightBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		HOTween.To(this.mTabBtnScrollBar, 0.25f, new TweenParms().Prop("value", this.mTabBtnScrollBar.value + 4f / (float)(this.VIPLevel.Length - 5)));
	}

	private void OnScrollBarValueChange()
	{
		if (this.leftBtn.activeInHierarchy)
		{
			if (this.mTabBtnScrollBar.value <= 0.01f)
			{
				this.leftBtn.SetActive(false);
			}
		}
		else if (this.mTabBtnScrollBar.value > 0.01f)
		{
			this.leftBtn.SetActive(true);
		}
		if (this.rightBtn.activeInHierarchy)
		{
			if (this.mTabBtnScrollBar.value >= 0.99f)
			{
				this.rightBtn.SetActive(false);
			}
		}
		else if (this.mTabBtnScrollBar.value < 0.99f)
		{
			this.rightBtn.SetActive(true);
		}
	}

	public static bool CanBuyRewardMark()
	{
		if (GameCache.Data != null && GameCache.Data.VIPWeekRewardStamp != 0 && Time.time - (float)GameCache.Data.VIPWeekRewardStamp < 86400f)
		{
			return false;
		}
		LocalPlayer player = Globals.Instance.Player;
		foreach (VipLevelInfo current in Globals.Instance.AttDB.VipLevelDict.Values)
		{
			if (current != null)
			{
				if ((ulong)player.Data.VipLevel >= (ulong)((long)Tools.GetVipLevel(current)) && !player.IsVipWeekRewardBuy(current))
				{
					return true;
				}
			}
		}
		return false;
	}
}
