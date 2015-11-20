using Att;
using System;
using UnityEngine;

public class GUIMultiUsePopUp : GameUIBasePopup
{
	public delegate void UseItemCallBack(ItemDataEx data, int count);

	public GUIMultiUsePopUp.UseItemCallBack UseItemEvent;

	private ItemDataEx mData;

	private UILabel mTitle;

	private CommonIconItem mIcon;

	private UILabel mName;

	private UILabel mNum;

	private UISliderNumberInput mSliderInput;

	private UILabel mTips;

	private UISprite mTipsIcon;

	private UILabel mTipsValue;

	private UILabel mUseLabel;

	public static void Show(ItemDataEx data, GUIMultiUsePopUp.UseItemCallBack cb)
	{
		if (data == null || data.GetCount() <= 0)
		{
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIMultiUsePopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(data, cb);
	}

	public static void TryClose()
	{
		if (GameUIPopupManager.GetInstance().GetState() == GameUIPopupManager.eSTATE.GUIMultiUsePopUp)
		{
			GameUIPopupManager.GetInstance().PopState(true, null);
		}
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		this.mTitle = GameUITools.FindUILabel("Title", base.gameObject);
		this.mIcon = CommonIconItem.Create(base.gameObject, new Vector3(-170f, 80f, 0f), null, false, 0.8f, null);
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mNum = GameUITools.FindUILabel("Num", base.gameObject);
		this.mSliderInput = GameUITools.FindGameObject("SliderNumberInput", base.gameObject).AddComponent<UISliderNumberInput>();
		this.mSliderInput.Init(new EventDelegate.Callback(this.OnNumberInputChanged));
		this.mTips = GameUITools.FindUILabel("Tips", base.gameObject);
		this.mTipsIcon = GameUITools.FindUISprite("Icon", this.mTips.gameObject);
		this.mTipsValue = GameUITools.FindUILabel("Value", this.mTips.gameObject);
		this.mUseLabel = GameUITools.FindUILabel("Label", GameUITools.RegisterClickEvent("Use", new UIEventListener.VoidDelegate(this.OnUseClick), base.gameObject));
	}

	private void OnUseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		int num = this.mSliderInput.Number;
		if (num < 0)
		{
			num = 1;
		}
		if (this.UseItemEvent != null)
		{
			this.UseItemEvent(this.mData, num);
		}
	}

	private void OnNumberInputChanged()
	{
		if (this.mData == null || this.mData.GetCount() <= 0)
		{
			return;
		}
		this.RefreshTips();
	}

	private void RefreshTips()
	{
		int num = this.mSliderInput.Number;
		if (num < 0)
		{
			num = 1;
		}
		EItemType type = (EItemType)this.mData.Info.Type;
		if (type == EItemType.EIT_Awake)
		{
			QualityInfo info = Globals.Instance.AttDB.QualityDict.GetInfo(this.mData.Info.Quality + 1);
			if (info == null)
			{
				global::Debug.LogErrorFormat("QualityDict get info error , ID : {0} ", new object[]
				{
					this.mData.Info.Quality + 1
				});
				return;
			}
			this.mTipsValue.text = (num * info.AwakeBreakupValue).ToString();
		}
	}

	public override void InitPopUp(ItemDataEx data, GUIMultiUsePopUp.UseItemCallBack cb)
	{
		if (data == null || data.GetCount() < 0)
		{
			return;
		}
		this.mData = data;
		this.UseItemEvent = cb;
		this.mIcon.Refresh(this.mData.Info, false, false, false);
		this.mName.text = this.mData.Info.Name;
		this.mName.color = Tools.GetItemQualityColor(this.mData.Info.Quality);
		this.mNum.text = this.mData.GetCount().ToString();
		this.mSliderInput.SetValue(1f);
		this.mSliderInput.Max = this.mData.GetCount();
		switch (this.mData.Info.Type)
		{
		case 2:
		{
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("useitem");
			this.mUseLabel.text = Singleton<StringManager>.Instance.GetString("useitem");
			this.mTipsValue.transform.parent.gameObject.SetActive(false);
			ESubTypeConsumable subType = (ESubTypeConsumable)this.mData.Info.SubType;
			if (subType == ESubTypeConsumable.EConsumable_TrinketExpBox)
			{
				if (this.mSliderInput.Max > 5)
				{
					this.mSliderInput.Max = 5;
				}
			}
			break;
		}
		case 5:
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("PetFurther3") + Singleton<StringManager>.Instance.GetString("itemLb");
			this.mUseLabel.text = Singleton<StringManager>.Instance.GetString("PetFurther3");
			this.mTipsIcon.spriteName = Tools.GetRewardTypeIcon(ERewardType.EReward_StarSoul);
			break;
		}
		this.RefreshTips();
	}

	private void OnCloseClick(GameObject go)
	{
		base.OnButtonBlockerClick();
	}
}
