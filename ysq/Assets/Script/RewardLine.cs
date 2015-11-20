using Att;
using Proto;
using System;
using UnityEngine;

public class RewardLine : MonoBehaviour
{
	public enum ERewardItemType
	{
		ERIT_Level,
		ERIT_Vip
	}

	public const int MAX_REWARD_NUM = 4;

	private RewardLine.ERewardItemType type;

	public MiscInfo mInfo;

	public VipLevelInfo vInfo;

	private GameObject get;

	private UISprite bg;

	private GameObject level;

	private GameObject vip;

	private GameObject toGet;

	private GameObject NotGet;

	private GameObject[] rewardSlot = new GameObject[4];

	private UIScrollView scrollView;

	public void Init(RewardLine.ERewardItemType _type, MiscInfo _mInfo, VipLevelInfo _vInfo, UIScrollView sw)
	{
		this.type = _type;
		this.mInfo = _mInfo;
		this.vInfo = _vInfo;
		this.scrollView = sw;
		this.bg = GameUITools.FindGameObject("bg", base.gameObject).GetComponent<UISprite>();
		this.get = GameUITools.FindGameObject("Get", this.bg.gameObject);
		this.level = GameUITools.FindGameObject("Level", this.bg.gameObject);
		this.vip = GameUITools.FindGameObject("VIP", this.bg.gameObject);
		this.toGet = GameUITools.RegisterClickEvent("ToGet", new UIEventListener.VoidDelegate(this.OnGetClicked), this.bg.gameObject);
		this.NotGet = GameUITools.FindGameObject("NotGet", this.bg.gameObject);
		if (this.type == RewardLine.ERewardItemType.ERIT_Level)
		{
			this.InitLevelReward();
		}
		else if (this.type == RewardLine.ERewardItemType.ERIT_Vip)
		{
			this.InitVipReward();
		}
		GameUITools.UpdateUIBoxCollider(this.bg.transform, 4f, false);
	}

	private void InitLevelReward()
	{
		if (this.mInfo == null)
		{
			return;
		}
		this.level.SetActive(true);
		this.vip.SetActive(false);
		UILabel uILabel = GameUITools.FindUILabel("Label", this.level);
		uILabel.text = Singleton<StringManager>.Instance.GetString("levelReward", new object[]
		{
			this.mInfo.Level
		});
		int num = 0;
		while (num < 4 && num < this.mInfo.RewardType.Count)
		{
			if (this.mInfo.RewardType[num] == 0)
			{
				break;
			}
			this.rewardSlot[num] = GameUITools.FindGameObject(string.Format("Slot{0}", num), this.level.transform.parent.gameObject);
			if (this.rewardSlot[num] != null)
			{
				GameObject gameObject = GameUITools.CreateReward(this.mInfo.RewardType[num], this.mInfo.RewardValue1[num], this.mInfo.RewardValue2[num], this.rewardSlot[num].transform, true, true, -30f, -20f, -2000f, 87f, 64f, 31f, 0);
				if (!(gameObject == null))
				{
					gameObject.AddComponent<UIDragScrollView>().scrollView = this.scrollView;
					if (this.mInfo.RewardType[num] == 3 || this.mInfo.RewardType[num] == 4)
					{
						gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
					}
					else
					{
						gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 1f);
					}
				}
			}
			num++;
		}
		this.RefreshGetFlag();
	}

	private void InitVipReward()
	{
		this.level.SetActive(false);
		this.vip.SetActive(true);
		UILabel uILabel = GameUITools.FindUILabel("Label", this.vip);
		uILabel.text = Singleton<StringManager>.Instance.GetString("vipReward", new object[]
		{
			this.vInfo.ID
		});
		int num = 0;
		while (num < 4 && num < this.vInfo.RewardType.Count)
		{
			if (this.vInfo.RewardType[num] == 0)
			{
				break;
			}
			this.rewardSlot[num] = GameUITools.FindGameObject(string.Format("Slot{0}", num), this.vip.transform.parent.gameObject);
			if (this.rewardSlot[num] != null)
			{
				GameObject gameObject = GameUITools.CreateReward(this.vInfo.RewardType[num], this.vInfo.RewardValue1[num], this.vInfo.RewardValue2[num], this.rewardSlot[num].transform, true, true, 36f, -7f, -2000f, 87f, 64f, 31f, 0);
				if (gameObject != null)
				{
					gameObject.AddComponent<UIDragScrollView>().scrollView = this.scrollView;
					if (this.vInfo.RewardType[num] == 3 || this.vInfo.RewardType[num] == 4)
					{
						gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
					}
					else
					{
						gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 1f);
					}
				}
			}
			num++;
		}
		this.RefreshGetFlag();
	}

	public int GetInfoID()
	{
		if (this.type == RewardLine.ERewardItemType.ERIT_Level)
		{
			return this.mInfo.ID;
		}
		if (this.type == RewardLine.ERewardItemType.ERIT_Vip)
		{
			return this.vInfo.ID;
		}
		return 0;
	}

	public void RefreshGetFlag()
	{
		if (this.type == RewardLine.ERewardItemType.ERIT_Level)
		{
			if (Globals.Instance.Player.IsLevelRewardTaken(this.mInfo.ID))
			{
				this.bg.spriteName = "Price_bg";
				NGUITools.SetActive(this.get, true);
				NGUITools.SetActive(this.toGet, false);
				NGUITools.SetActive(this.NotGet, false);
			}
			else
			{
				this.bg.spriteName = "gold_bg";
				NGUITools.SetActive(this.get, false);
				if ((ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)this.mInfo.Level))
				{
					NGUITools.SetActive(this.toGet, true);
					NGUITools.SetActive(this.NotGet, false);
				}
				else
				{
					NGUITools.SetActive(this.toGet, false);
					NGUITools.SetActive(this.NotGet, true);
				}
			}
		}
	}

	public void OnGetClicked(GameObject go)
	{
		if (this.type == RewardLine.ERewardItemType.ERIT_Level)
		{
			if (Globals.Instance.Player.IsLevelRewardTaken(this.mInfo.ID) || (ulong)Globals.Instance.Player.Data.Level < (ulong)((long)this.mInfo.Level))
			{
				return;
			}
			MC2S_TakeLevelReward mC2S_TakeLevelReward = new MC2S_TakeLevelReward();
			mC2S_TakeLevelReward.Index = this.mInfo.ID;
			Globals.Instance.CliSession.Send(228, mC2S_TakeLevelReward);
		}
	}
}
