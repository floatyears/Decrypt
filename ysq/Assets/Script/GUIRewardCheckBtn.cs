using Proto;
using System;
using UnityEngine;

public class GUIRewardCheckBtn : MonoBehaviour
{
	public delegate void CheckChangeCallback(bool isCheck);

	public GUIRewardCheckBtn.CheckChangeCallback CheckChangeCallbackEvent;

	private GameObject mNewMark;

	private UISprite mNewActivityMark;

	private UISprite mCheckSp;

	private UILabel mTabName;

	private UISprite mTabIcon;

	private UISprite mTabIcon2;

	public static GUIRewardCheckBtn mCurrent;

	public EActivityType ActivityType = EActivityType.EAT_Value;

	private BaseActivityData mBaseActivityData;

	private bool mIsChecked;

	private bool mIsShowMark;

	private bool mIgnornClickEvent;

	public GameObject ActivateObj
	{
		get;
		set;
	}

	public ActivityAchievementData AAData
	{
		get;
		private set;
	}

	public ActivityValueData AVData
	{
		get;
		private set;
	}

	public ActivityShopData ASData
	{
		get;
		private set;
	}

	public ActivityPayShopData APSData
	{
		get;
		private set;
	}

	public ActivitySpecifyPayData ASPData
	{
		get;
		private set;
	}

	public ActivityGroupBuyingData AGBData
	{
		get;
		private set;
	}

	public bool IsChecked
	{
		get
		{
			return this.mIsChecked;
		}
		set
		{
			if (this.mIsChecked != value)
			{
				this.mIsChecked = value;
				this.mCheckSp.spriteName = ((!this.mIsChecked) ? "btnBg3" : "btn2");
				if (this.mIsChecked)
				{
					if (GUIRewardCheckBtn.mCurrent != null && GUIRewardCheckBtn.mCurrent != this)
					{
						GUIRewardCheckBtn.mCurrent.IsChecked = false;
					}
					GUIRewardCheckBtn.mCurrent = this;
					if (this.mBaseActivityData != null && GameCache.AddConfigurableActivityData(this.mBaseActivityData.ID, this.mBaseActivityData.RewardTimeStamp))
					{
						this.mNewActivityMark.enabled = false;
						this.IsShowMark = this.mIsShowMark;
					}
				}
				if (this.ActivateObj != null)
				{
					this.ActivateObj.SetActive(value);
				}
				if (this.CheckChangeCallbackEvent != null)
				{
					this.CheckChangeCallbackEvent(this.mIsChecked);
				}
			}
		}
	}

	public bool IsShowMark
	{
		get
		{
			return base.gameObject.activeSelf && this.mIsShowMark;
		}
		set
		{
			if (this.mBaseActivityData != null)
			{
				this.mNewActivityMark.enabled = GameCache.ShowNewActivityMark(this.mBaseActivityData.ID, this.mBaseActivityData.RewardTimeStamp);
			}
			this.mIsShowMark = value;
			if (this.mNewActivityMark.enabled)
			{
				this.mNewMark.SetActive(false);
			}
			else
			{
				this.mNewMark.SetActive(this.mIsShowMark);
			}
		}
	}

	public bool IgnornClickEvent
	{
		get
		{
			return this.mIgnornClickEvent;
		}
		set
		{
			this.mIgnornClickEvent = value;
		}
	}

	public string Text
	{
		get
		{
			return this.mTabName.text;
		}
		set
		{
			this.mTabName.text = value;
		}
	}

	public string Icon
	{
		get
		{
			if (this.mTabIcon2.gameObject.activeInHierarchy)
			{
				return this.mTabIcon2.spriteName;
			}
			return this.mTabIcon.spriteName;
		}
		set
		{
			if (value == "groupBuy")
			{
				this.mTabIcon2.gameObject.SetActive(true);
				this.mTabIcon2.spriteName = value;
				this.mTabIcon2.MakePixelPerfect();
				this.mTabIcon2.height = 80;
			}
			else
			{
				this.mTabIcon.gameObject.SetActive(true);
				this.mTabIcon.spriteName = value;
				this.mTabIcon.MakePixelPerfect();
				if (this.mTabIcon.spriteName == "Share")
				{
					this.mTabIcon.height = 58;
				}
				else
				{
					this.mTabIcon.height = 80;
				}
			}
		}
	}

	public void InitWithBaseScene(ActivityValueData data)
	{
		this.AVData = data;
		this.mIsChecked = false;
		this.CreateObjects();
		this.Text = data.Base.Name;
		this.Icon = "ar";
		base.gameObject.name = string.Format("cv{0}", data.Base.ID);
	}

	public void InitWithBaseScene(ActivityAchievementData data)
	{
		this.AAData = data;
		this.mIsChecked = false;
		this.CreateObjects();
		this.Text = data.Base.Name;
		this.Icon = "aa";
		base.gameObject.name = string.Format("cb{0}", data.Base.ID);
	}

	public void InitWithBaseScene(ActivityGroupBuyingData data)
	{
		this.AGBData = data;
		this.mIsChecked = false;
		this.CreateObjects();
		this.Text = data.Base.Name;
		this.Icon = "groupBuy";
		base.gameObject.name = string.Format("ca{0}", data.Base.ID);
	}

	public void InitWithBaseScene(ActivityShopData data)
	{
		this.ASData = data;
		this.mIsChecked = false;
		this.CreateObjects();
		this.Text = data.Base.Name;
		this.Icon = "FlashSale";
		base.gameObject.name = string.Format("ca{0}", data.Base.ID);
	}

	public void InitWithBaseScene(ActivityPayShopData data)
	{
		this.APSData = data;
		this.mIsChecked = false;
		this.CreateObjects();
		this.Text = data.Base.Name;
		this.Icon = "FlashSale";
		base.gameObject.name = string.Format("cc{0}", data.Base.ID);
	}

	public void InitWithBaseScene(bool isCheck, EActivityType type)
	{
		this.ActivityType = type;
		this.mIsChecked = isCheck;
		this.CreateObjects();
		base.gameObject.name = string.Format("b{0}", (int)type);
	}

	public void InitWithBaseScene(bool isCheck, string sortName, string iconName)
	{
		this.mIsChecked = isCheck;
		this.CreateObjects();
		this.Icon = iconName;
		base.gameObject.name = sortName;
	}

	public void InitWithBaseScene(ActivitySpecifyPayData data)
	{
		this.ASPData = data;
		this.mIsChecked = false;
		this.CreateObjects();
		this.Text = data.Base.Name;
		this.Icon = "aa";
		base.gameObject.name = string.Format("cd{0}", data.Base.ID);
	}

	private void CreateObjects()
	{
		this.mNewMark = base.transform.Find("new").gameObject;
		this.mNewActivityMark = base.transform.Find("newActivity").GetComponent<UISprite>();
		this.mNewActivityMark.enabled = false;
		this.mCheckSp = base.transform.GetComponent<UISprite>();
		this.mCheckSp.spriteName = ((!this.mIsChecked) ? "btnBg3" : "btn2");
		this.mTabName = base.transform.Find("tabTxt0").GetComponent<UILabel>();
		this.mTabIcon = base.transform.Find("icon").GetComponent<UISprite>();
		this.mTabIcon2 = base.transform.Find("icon2").GetComponent<UISprite>();
		this.mTabIcon.gameObject.SetActive(false);
		this.mTabIcon2.gameObject.SetActive(false);
		if (this.ActivityType != (EActivityType)0)
		{
			UISprite uISprite = GameUITools.FindUISprite("icon", base.gameObject);
			EActivityType activityType = this.ActivityType;
			switch (activityType)
			{
			case EActivityType.EAT_Dart:
				this.mTabIcon.gameObject.SetActive(true);
				uISprite.spriteName = "Dart";
				break;
			case EActivityType.EAT_LuckyDraw:
				this.mTabIcon.gameObject.SetActive(true);
				uISprite.spriteName = "LuckyDraw";
				break;
			case EActivityType.EAT_FlashSale:
				this.mTabIcon.gameObject.SetActive(true);
				uISprite.spriteName = "FlashSale";
				break;
			case EActivityType.EAT_ScratchOff:
				this.mTabIcon.gameObject.SetActive(true);
				uISprite.spriteName = "ScratchOff";
				break;
			default:
				if (activityType == EActivityType.EAT_RollEquip)
				{
					this.mTabIcon.gameObject.SetActive(true);
					uISprite.spriteName = "Tree";
					uISprite.keepAspectRatio = UIWidget.AspectRatioSource.Free;
					uISprite.MakePixelPerfect();
					uISprite.transform.localPosition = new Vector3(-94f, 0f, 0f);
					this.mBaseActivityData = Globals.Instance.Player.ActivitySystem.REData.Base;
				}
				break;
			}
		}
		if (this.AAData != null)
		{
			this.mBaseActivityData = this.AAData.Base;
		}
		else if (this.AVData != null)
		{
			this.mBaseActivityData = this.AVData.Base;
		}
		else if (this.ASData != null)
		{
			this.mBaseActivityData = this.ASData.Base;
		}
		else if (this.APSData != null)
		{
			this.mBaseActivityData = this.APSData.Base;
		}
		else if (this.ASPData != null)
		{
			this.mBaseActivityData = this.ASPData.Base;
		}
		else if (this.AGBData != null)
		{
			this.mBaseActivityData = this.AGBData.Base;
		}
		UIEventListener expr_2EF = UIEventListener.Get(base.gameObject);
		expr_2EF.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2EF.onClick, new UIEventListener.VoidDelegate(this.OnTabBtnClick));
		GameUITools.UpdateUIBoxCollider(base.transform, 22f, false);
	}

	private void OnTabBtnClick(GameObject go)
	{
		if (!this.IgnornClickEvent)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
			this.IsChecked = true;
		}
	}
}
