using System;
using UnityEngine;

public class GUIRewardCardsInfo : MonoBehaviour
{
	public enum ECS
	{
		ECS_Null,
		ECS_GoBuy,
		ECS_GoTake,
		ECS_HasTaken,
		ECS_GoRenew
	}

	private GUIReward mBaseScene;

	private GUIRewardCheckBtn mCheckBtn;

	private GameObject mMonthBtn;

	private UILabel mMonthBtnLabel;

	private UILabel mMonthDesc;

	private UILabel mMonthContent;

	private GameObject mSuperBtn;

	private UILabel mSuperDesc;

	private GUIRewardCardsInfo.ECS monthStatus;

	private GUIRewardCardsInfo.ECS superStatus;

	public static bool IsVisible
	{
		get
		{
			return true;
		}
	}

	private static bool IsNew
	{
		get
		{
			return GameCache.Data == null || GameCache.Data.PayCardStamp == 0 || Time.time - (float)GameCache.Data.PayCardStamp >= 86400f;
		}
		set
		{
			if (Globals.Instance.Player)
			{
				GameCache.Data.PayCardStamp = Globals.Instance.Player.GetTimeStamp();
				GameCache.UpdateNow = true;
			}
			else
			{
				GameCache.Data.PayCardStamp = 0;
			}
		}
	}

	public void InitWithBaseScene(GUIReward baseScene, GUIRewardCheckBtn btn)
	{
		this.mBaseScene = baseScene;
		this.mCheckBtn = btn;
		this.CreateObjects();
	}

	protected void CreateObjects()
	{
		GameObject gameObject = GameUITools.FindGameObject("Cards/MonthCard", base.gameObject);
		this.mMonthBtn = GameUITools.RegisterClickEvent("TakeBtn", new UIEventListener.VoidDelegate(this.OnMonthTakeBtnClick), gameObject);
		this.mMonthBtnLabel = GameUITools.FindUILabel("Label", this.mMonthBtn);
		this.mMonthDesc = GameUITools.RegisterClickEvent("Desc", new UIEventListener.VoidDelegate(this.OnMonthDescClick), gameObject).GetComponent<UILabel>();
		this.mMonthContent = GameUITools.FindUILabel("Content", gameObject);
		gameObject = GameUITools.FindGameObject("SuperCard", gameObject.transform.parent.gameObject);
		this.mSuperBtn = GameUITools.RegisterClickEvent("TakeBtn", new UIEventListener.VoidDelegate(this.OnSuperTakeBtnClick), gameObject);
		this.mSuperDesc = GameUITools.RegisterClickEvent("Desc", new UIEventListener.VoidDelegate(this.OnSuperDescClick), gameObject).GetComponent<UILabel>();
	}

	public void OnTotalPayUpdateEvent()
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.Refresh();
		}
	}

	public static bool CanTakePartIn()
	{
		return GUIRewardCardsInfo.IsNew && (Globals.Instance.Player.IsCardExpire() || !Globals.Instance.Player.IsBuySuperCard());
	}

	public void Refresh()
	{
		GUIRewardCardsInfo.IsNew = false;
		this.mCheckBtn.IsShowMark = false;
		this.RefreshStatuses();
		switch (this.monthStatus)
		{
		case GUIRewardCardsInfo.ECS.ECS_Null:
			this.mMonthBtn.gameObject.SetActive(false);
			this.mMonthDesc.enabled = false;
			this.mMonthContent.text = Singleton<StringManager>.Instance.GetString("activityCardsMonthContent");
			break;
		case GUIRewardCardsInfo.ECS.ECS_GoBuy:
			this.mMonthDesc.enabled = false;
			this.mMonthBtn.gameObject.SetActive(true);
			this.mMonthBtnLabel.text = Singleton<StringManager>.Instance.GetString("activityCardsBtnLabelGoBuy");
			this.mMonthContent.text = Singleton<StringManager>.Instance.GetString("activityCardsMonthContent");
			break;
		case GUIRewardCardsInfo.ECS.ECS_GoTake:
			this.mMonthDesc.enabled = true;
			this.mMonthDesc.text = Singleton<StringManager>.Instance.GetString("activityCardsDescGoTake");
			this.mMonthBtn.gameObject.SetActive(false);
			this.mMonthContent.text = Singleton<StringManager>.Instance.GetString("activityCardsMonthContentRemain", new object[]
			{
				Globals.Instance.Player.GetCardRemainDays()
			});
			break;
		case GUIRewardCardsInfo.ECS.ECS_HasTaken:
			this.mMonthDesc.enabled = true;
			this.mMonthDesc.text = Singleton<StringManager>.Instance.GetString("activityCardsDescHasTaken");
			this.mMonthBtn.gameObject.SetActive(false);
			this.mMonthContent.text = Singleton<StringManager>.Instance.GetString("activityCardsMonthContentRemain", new object[]
			{
				Globals.Instance.Player.GetCardRemainDays()
			});
			break;
		case GUIRewardCardsInfo.ECS.ECS_GoRenew:
			this.mMonthDesc.enabled = false;
			this.mMonthBtn.gameObject.SetActive(true);
			this.mMonthBtnLabel.text = Singleton<StringManager>.Instance.GetString("activityCardsBtnLabelGoRenew");
			this.mMonthContent.text = Singleton<StringManager>.Instance.GetString("activityCardsMonthContentRemain", new object[]
			{
				Globals.Instance.Player.GetCardRemainDays()
			});
			break;
		}
		switch (this.superStatus)
		{
		case GUIRewardCardsInfo.ECS.ECS_Null:
			this.mSuperDesc.enabled = false;
			this.mSuperBtn.gameObject.SetActive(false);
			break;
		case GUIRewardCardsInfo.ECS.ECS_GoBuy:
			this.mSuperDesc.enabled = false;
			this.mSuperBtn.gameObject.SetActive(true);
			break;
		case GUIRewardCardsInfo.ECS.ECS_GoTake:
			this.mSuperDesc.enabled = true;
			this.mSuperDesc.text = Singleton<StringManager>.Instance.GetString("activityCardsDescGoTake");
			this.mSuperBtn.gameObject.SetActive(false);
			break;
		case GUIRewardCardsInfo.ECS.ECS_HasTaken:
			this.mSuperDesc.enabled = true;
			this.mSuperDesc.text = Singleton<StringManager>.Instance.GetString("activityCardsDescHasTaken");
			this.mSuperBtn.gameObject.SetActive(false);
			break;
		case GUIRewardCardsInfo.ECS.ECS_GoRenew:
			this.mSuperDesc.enabled = false;
			this.mSuperBtn.gameObject.SetActive(false);
			break;
		}
	}

	private void RefreshStatuses()
	{
		if (Globals.Instance.Player.IsCardExpire())
		{
			this.monthStatus = GUIRewardCardsInfo.ECS.ECS_GoBuy;
		}
		else if (Globals.Instance.Player.IsTodayCardDiamondTaken())
		{
			if (Globals.Instance.Player.GetCardRemainDays() > 0)
			{
				this.monthStatus = GUIRewardCardsInfo.ECS.ECS_HasTaken;
			}
			else
			{
				this.monthStatus = GUIRewardCardsInfo.ECS.ECS_GoRenew;
			}
		}
		else
		{
			this.monthStatus = GUIRewardCardsInfo.ECS.ECS_GoTake;
		}
		if (Globals.Instance.Player.IsBuySuperCard())
		{
			if (Globals.Instance.Player.IsTodaySuperCardDiamondTaken())
			{
				this.superStatus = GUIRewardCardsInfo.ECS.ECS_HasTaken;
			}
			else
			{
				this.superStatus = GUIRewardCardsInfo.ECS.ECS_GoTake;
			}
		}
		else
		{
			this.superStatus = GUIRewardCardsInfo.ECS.ECS_GoBuy;
		}
	}

	private void OnMonthTakeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.Refresh();
		switch (this.monthStatus)
		{
		case GUIRewardCardsInfo.ECS.ECS_GoBuy:
			GameUIManager.mInstance.uiState.ShowMonthCardHalo = 1;
			GameUIVip.OpenRecharge();
			break;
		case GUIRewardCardsInfo.ECS.ECS_GoTake:
			GUIReward.ActivityType = GUIReward.ERewardActivityType.ERAT_Cards;
			GameUIManager.mInstance.ChangeSession<GUIAchievementScene>(null, false, true);
			this.mBaseScene.Close();
			break;
		case GUIRewardCardsInfo.ECS.ECS_GoRenew:
			GameUIManager.mInstance.uiState.ShowMonthCardHalo = 1;
			GameUIVip.OpenRecharge();
			break;
		}
	}

	private void OnMonthDescClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.Refresh();
		switch (this.monthStatus)
		{
		case GUIRewardCardsInfo.ECS.ECS_GoTake:
			GUIReward.ActivityType = GUIReward.ERewardActivityType.ERAT_Cards;
			GameUIManager.mInstance.ChangeSession<GUIAchievementScene>(null, false, true);
			break;
		}
	}

	private void OnSuperTakeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.Refresh();
		switch (this.superStatus)
		{
		case GUIRewardCardsInfo.ECS.ECS_GoBuy:
			GameUIManager.mInstance.uiState.ShowMonthCardHalo = 2;
			GameUIVip.OpenRecharge();
			break;
		case GUIRewardCardsInfo.ECS.ECS_GoTake:
			GUIReward.ActivityType = GUIReward.ERewardActivityType.ERAT_Cards;
			GameUIManager.mInstance.ChangeSession<GUIAchievementScene>(null, false, true);
			this.mBaseScene.Close();
			break;
		}
	}

	private void OnSuperDescClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.Refresh();
		switch (this.superStatus)
		{
		case GUIRewardCardsInfo.ECS.ECS_GoTake:
			GUIReward.ActivityType = GUIReward.ERewardActivityType.ERAT_Cards;
			GameUIManager.mInstance.ChangeSession<GUIAchievementScene>(null, false, true);
			this.mBaseScene.Close();
			break;
		}
	}
}
