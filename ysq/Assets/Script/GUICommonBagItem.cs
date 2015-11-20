using Holoville.HOTween;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public abstract class GUICommonBagItem : UICustomGridItem
{
	protected object mOriginal;

	public ItemDataEx mData;

	public PetDataEx mPetData;

	public LopetDataEx mLopetData;

	protected UISprite mBG;

	private CommonIconItem mIconItem;

	private UILabel mLevel;

	protected UILabel mName;

	protected GameObject mInfoPanel;

	protected UISprite mInfo;

	protected UILabel mPoint0;

	protected UILabel mPoint0Value;

	protected UILabel mPoint1;

	private UILabel mPoint1Value;

	protected UILabel mRefineLevel;

	private UISprite mRefineLevelBG;

	private UILabel mDesc;

	private UILabel mRelation;

	protected GUIStars mStars;

	protected GameObject mList;

	private GameObject mEnhanceBtn;

	private UILabel mEnhanceLb;

	private UILabel mEnhanceMax;

	public GameObject mRefineBtn;

	private UILabel mRefineLb;

	private UILabel mRefineMax;

	protected UISprite mRefineLock;

	protected GameObject mListGoBtn;

	private UILabel mOwner;

	private UILabel mPrice;

	private UILabel mPriceName;

	private UISprite mPriceIcon;

	protected CommonBagItemToggle mSelectToggle;

	protected GameObject mListBtn;

	protected UISprite mClickableBtnSprite;

	private UIButton mClickableBtn;

	private UIButton[] mClickableBtns;

	private UILabel mClickableBtnTxt;

	private GameObject mClickableBtnEffect;

	private string tempStr;

	private int tempInt;

	private PetDataEx tempPetData;

	private bool tempBool;

	private static GUICommonBagItem mCurBagItem;

	private static ItemDataEx mCurBagItemData;

	private static PetDataEx mCurBagItemPetData;

	private static LopetDataEx mCurBagItemLopetData;

	public void InitWithBaseScene(object scene)
	{
		this.mOriginal = scene;
		this.CreateObjects();
	}

	protected void CreateObjects()
	{
		this.mBG = base.gameObject.GetComponent<UISprite>();
		this.mIconItem = CommonIconItem.Create(base.gameObject, new Vector3(11f, -9f, 0f), new CommonIconItem.VoidCallBack(this.OnIconClick), true, 0.9f, null);
		this.mLevel = GameUITools.FindUILabel("Level", base.gameObject);
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mInfoPanel = GameUITools.FindGameObject("InfoPanel", base.gameObject);
		this.mInfo = GameUITools.FindUISprite("Info", this.mInfoPanel.gameObject);
		this.mList = GameUITools.FindGameObject("List", this.mInfoPanel.gameObject);
		this.mPoint0 = GameUITools.FindUILabel("Point0", this.mInfo.gameObject);
		this.mPoint0Value = GameUITools.FindUILabel("Value", this.mPoint0.gameObject);
		this.mPoint1 = GameUITools.FindUILabel("Point1", this.mInfo.gameObject);
		this.mPoint1Value = GameUITools.FindUILabel("Value", this.mPoint1.gameObject);
		this.mRefineLevel = GameUITools.FindUILabel("RefineLevel", this.mInfo.gameObject);
		this.mRefineLevelBG = GameUITools.FindUISprite("BG", this.mRefineLevel.gameObject);
		this.mDesc = GameUITools.FindUILabel("Desc", this.mInfo.gameObject);
		this.mRelation = GameUITools.FindUILabel("Relation", this.mInfo.gameObject);
		this.mStars = GameUITools.FindGameObject("Stars", this.mInfo.gameObject).AddComponent<GUIStars>();
		this.mStars.Init(5);
		this.mOwner = GameUITools.FindUILabel("Owner", base.gameObject);
		this.mOwner.gameObject.SetActive(true);
		this.mPrice = GameUITools.FindUILabel("Price", base.gameObject);
		this.mPriceName = GameUITools.FindUILabel("Name", this.mPrice.gameObject);
		this.mPriceIcon = GameUITools.FindUISprite("Icon", this.mPrice.gameObject);
		this.mSelectToggle = GameUITools.FindGameObject("SelectToggle", base.gameObject).AddComponent<CommonBagItemToggle>();
		CommonBagItemToggle expr_25A = this.mSelectToggle;
		expr_25A.ValueChangeEvent = (CommonBagItemToggle.ValueChangeCallBack)Delegate.Combine(expr_25A.ValueChangeEvent, new CommonBagItemToggle.ValueChangeCallBack(this.OnSelectToggleChange));
		CommonBagItemToggle expr_282 = this.mSelectToggle;
		expr_282.PreValueChangeEvent = (CommonBagItemToggle.PreValueChangeCallBack)Delegate.Combine(expr_282.PreValueChangeEvent, new CommonBagItemToggle.PreValueChangeCallBack(this.OnPreSelectToggleChange));
		this.mEnhanceBtn = GameUITools.RegisterClickEvent("Enhance", new UIEventListener.VoidDelegate(this.OnEnhanceClick), this.mList);
		this.mEnhanceLb = GameUITools.FindUILabel("Label", this.mEnhanceBtn);
		this.mEnhanceMax = GameUITools.FindUILabel("Max", this.mEnhanceBtn);
		this.mRefineBtn = GameUITools.RegisterClickEvent("Refine", new UIEventListener.VoidDelegate(this.OnRefineClick), this.mList);
		this.mRefineLb = GameUITools.FindUILabel("Label", this.mRefineBtn);
		this.mRefineMax = GameUITools.FindUILabel("Max", this.mRefineBtn);
		this.mRefineLock = GameUITools.FindUISprite("Lock", this.mRefineBtn);
		this.mListGoBtn = GameUITools.RegisterClickEvent("ListGo", new UIEventListener.VoidDelegate(this.OnListGoBtnClick), this.mList);
		this.mListBtn = GameUITools.RegisterClickEvent("ListBtn", new UIEventListener.VoidDelegate(this.OnListBtnClick), base.gameObject);
		this.mClickableBtnSprite = GameUITools.RegisterClickEvent("ClickableBtn", new UIEventListener.VoidDelegate(this.OnClickableBtnClick), base.gameObject).GetComponent<UISprite>();
		this.mClickableBtn = this.mClickableBtnSprite.GetComponent<UIButton>();
		this.mClickableBtns = this.mClickableBtnSprite.GetComponents<UIButton>();
		this.mClickableBtnTxt = GameUITools.FindUILabel("Txt", this.mClickableBtnSprite.gameObject);
		this.mClickableBtnEffect = GameUITools.FindGameObject("Effect", this.mClickableBtnSprite.gameObject);
		this.mClickableBtnEffect.gameObject.SetActive(false);
		if (this.IsShowPanel())
		{
			this.mListBtn.SetActive(true);
		}
		else
		{
			this.mListBtn.SetActive(false);
			UIWidget component = this.mInfo.GetComponent<UIWidget>();
			component.bottomAnchor.target = null;
			component.topAnchor.target = null;
			component.leftAnchor.target = null;
			component.rightAnchor.target = null;
		}
		if (this.ShowSelect())
		{
			this.mSelectToggle.gameObject.SetActive(true);
		}
		else
		{
			this.mSelectToggle.gameObject.SetActive(false);
		}
		this.InitObjects();
		GameUITools.UpdateUIBoxCollider(base.transform, 8f, true);
	}

	protected abstract void InitObjects();

	protected abstract bool IsShowPanel();

	public override void Refresh(object data)
	{
		if (this.mData == data)
		{
			return;
		}
		if (data is ItemDataEx)
		{
			this.mData = (ItemDataEx)data;
		}
		else if (data is PetDataEx)
		{
			this.mPetData = (PetDataEx)data;
		}
		else
		{
			if (!(data is LopetDataEx))
			{
				return;
			}
			this.mLopetData = (LopetDataEx)data;
		}
		this.Refresh();
	}

	protected abstract string GetName();

	protected virtual string GetClickableBtnTxt()
	{
		return string.Empty;
	}

	protected virtual bool EnableClickableBtn()
	{
		return true;
	}

	protected virtual bool ShowClickableBtnEffect()
	{
		return false;
	}

	protected virtual bool ShowIconGray()
	{
		return false;
	}

	protected virtual bool ShowIconLeftTopTag()
	{
		return false;
	}

	protected virtual string GetIconLeftTopTagLb()
	{
		return string.Empty;
	}

	protected virtual string GetLevel()
	{
		return string.Empty;
	}

	protected virtual string GetPoint0()
	{
		return string.Empty;
	}

	protected virtual string GetPoint0Value()
	{
		return string.Empty;
	}

	protected virtual string GetPoint1()
	{
		return string.Empty;
	}

	protected virtual string GetPoint1Value()
	{
		return string.Empty;
	}

	protected virtual int GetRefineLevel()
	{
		return 0;
	}

	protected virtual string GetDesc()
	{
		return string.Empty;
	}

	protected virtual string GetTip()
	{
		return string.Empty;
	}

	protected virtual string GetRelation()
	{
		return string.Empty;
	}

	protected virtual bool ShowStars()
	{
		return false;
	}

	protected virtual int GetStarsNum()
	{
		return 0;
	}

	protected virtual PetDataEx GetPetDataEx()
	{
		if (this.mData != null)
		{
			return this.mData.GetEquipPet();
		}
		if (this.mPetData != null)
		{
			return this.mPetData;
		}
		return null;
	}

	protected virtual bool ShowSelect()
	{
		return false;
	}

	protected virtual int GetPrice(out string name, out bool showIcon)
	{
		name = string.Empty;
		showIcon = true;
		return 0;
	}

	protected virtual bool ShowListGoBtn()
	{
		return false;
	}

	protected virtual string GetEnhanceLb()
	{
		return Singleton<StringManager>.Instance.GetString("improve");
	}

	protected virtual string GetRefineLb()
	{
		return Singleton<StringManager>.Instance.GetString("refine");
	}

	protected virtual string GetEnhanceMax()
	{
		return string.Empty;
	}

	protected virtual string GetRefineMax()
	{
		return string.Empty;
	}

	public void Refresh()
	{
		if (this.mData != null)
		{
			this.mIconItem.Refresh(this.mData, false, false, false);
			this.mIconItem.SetMask = this.ShowIconGray();
			this.tempStr = this.GetLevel();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mLevel.enabled = false;
			}
			else
			{
				this.mLevel.enabled = true;
				this.mLevel.text = this.tempStr;
			}
			this.tempStr = this.GetName();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mName.enabled = false;
			}
			else
			{
				this.mName.enabled = true;
				this.mName.color = Tools.GetItemQualityColor(this.mData.Info.Quality);
				this.mName.text = this.tempStr;
			}
			this.mPoint0.text = this.GetPoint0();
			this.mPoint0Value.text = this.GetPoint0Value();
			this.mPoint1.text = this.GetPoint1();
			this.mPoint1Value.text = this.GetPoint1Value();
			this.tempInt = this.GetRefineLevel();
			if (this.tempInt > 0)
			{
				this.mRefineLevel.gameObject.SetActive(true);
				this.mRefineLevel.text = Singleton<StringManager>.Instance.GetString("equipImprove1", new object[]
				{
					this.tempInt
				});
				this.mRefineLevelBG.enabled = true;
			}
			else
			{
				this.tempStr = this.GetTip();
				if (string.IsNullOrEmpty(this.tempStr))
				{
					this.mRefineLevel.gameObject.SetActive(false);
				}
				else
				{
					this.mRefineLevel.gameObject.SetActive(true);
					this.mRefineLevel.text = this.tempStr;
					this.mRefineLevelBG.enabled = false;
				}
			}
			this.tempStr = this.GetDesc();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mDesc.enabled = false;
			}
			else
			{
				this.mDesc.enabled = true;
				this.mDesc.text = this.tempStr;
			}
			this.tempStr = this.GetRelation();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mRelation.enabled = false;
			}
			else
			{
				this.mRelation.enabled = true;
				this.mRelation.text = this.tempStr;
			}
			if (this.ShowStars())
			{
				this.mStars.Show();
				this.tempInt = this.GetStarsNum();
				this.mStars.Refresh(this.tempInt);
			}
			else
			{
				this.mStars.Hide();
			}
			this.tempPetData = this.GetPetDataEx();
			if (this.tempPetData == null)
			{
				this.mOwner.enabled = false;
			}
			else
			{
				this.mOwner.enabled = true;
				if (this.tempPetData.IsPlayerBattling())
				{
					this.mOwner.text = Singleton<StringManager>.Instance.GetString("equipImprove10", new object[]
					{
						Globals.Instance.Player.Data.Name
					});
				}
				else
				{
					this.mOwner.text = Singleton<StringManager>.Instance.GetString("equipImprove10", new object[]
					{
						this.tempPetData.Info.FirstName
					});
				}
			}
			if (this.ShowSelect())
			{
				this.mSelectToggle.SetCheckValue(this.mData.IsSelected);
			}
			this.tempInt = this.GetPrice(out this.tempStr, out this.tempBool);
			if (this.tempInt > 0)
			{
				this.mPrice.gameObject.SetActive(true);
				this.mPrice.text = this.tempInt.ToString();
				this.mPriceIcon.enabled = this.tempBool;
				this.mPriceName.text = this.tempStr;
			}
			else
			{
				this.mPrice.gameObject.SetActive(false);
			}
			if (this.mListBtn.activeInHierarchy)
			{
				if (this.mData.BtnsVisible)
				{
					this.mInfo.gameObject.SetActive(false);
					this.mList.SetActive(true);
					this.mListBtn.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);
					this.mRefineBtn.gameObject.SetActive(true);
					this.mEnhanceBtn.gameObject.SetActive(true);
					if (GUICommonBagItem.mCurBagItem != this)
					{
						GUICommonBagItem.mCurBagItem = this;
						GUICommonBagItem.mCurBagItemData = this.mData;
					}
				}
				else
				{
					this.mInfo.gameObject.SetActive(true);
					this.mList.SetActive(false);
					this.mListBtn.transform.localRotation = Quaternion.identity;
					this.mRefineBtn.gameObject.SetActive(false);
					this.mEnhanceBtn.gameObject.SetActive(false);
				}
				this.tempStr = this.GetEnhanceMax();
				if (string.IsNullOrEmpty(this.tempStr))
				{
					this.mEnhanceMax.enabled = false;
				}
				else
				{
					this.mEnhanceMax.enabled = true;
					this.mEnhanceMax.text = this.tempStr;
				}
				this.tempStr = this.GetRefineMax();
				if (string.IsNullOrEmpty(this.tempStr))
				{
					this.mRefineMax.enabled = false;
				}
				else
				{
					this.mRefineMax.enabled = true;
					this.mRefineMax.text = this.tempStr;
				}
				if (this.ShowListGoBtn())
				{
					this.mListGoBtn.SetActive(true);
				}
				else
				{
					this.mListGoBtn.SetActive(false);
				}
			}
			else
			{
				this.mListBtn.SetActive(false);
				this.tempStr = this.GetClickableBtnTxt();
				if (string.IsNullOrEmpty(this.tempStr))
				{
					this.mClickableBtnSprite.gameObject.SetActive(false);
				}
				else
				{
					this.mClickableBtnSprite.gameObject.SetActive(true);
					this.mClickableBtnTxt.text = this.tempStr;
					NGUITools.SetActive(this.mClickableBtnEffect, this.ShowClickableBtnEffect());
				}
			}
		}
		else if (this.mPetData != null)
		{
			this.mIconItem.Refresh(this.mPetData, false, false, false);
			this.mIconItem.SetMask = this.ShowIconGray();
			this.mIconItem.EnableLeftTopTag = this.ShowIconLeftTopTag();
			this.mIconItem.SetLeftTopTag(this.GetIconLeftTopTagLb());
			this.tempStr = this.GetLevel();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mLevel.enabled = false;
			}
			else
			{
				this.mLevel.enabled = true;
				this.mLevel.text = this.tempStr;
			}
			this.tempStr = this.GetName();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mName.enabled = false;
			}
			else
			{
				this.mName.enabled = true;
				this.mName.color = Tools.GetItemQualityColor(this.mPetData.Info.Quality);
				this.mName.text = this.tempStr;
			}
			this.mPoint0.text = this.GetPoint0();
			this.mPoint0Value.text = this.GetPoint0Value();
			this.mPoint1.text = this.GetPoint1();
			this.mPoint1Value.text = this.GetPoint1Value();
			this.tempStr = this.GetTip();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mRefineLevel.gameObject.SetActive(false);
			}
			else
			{
				this.mRefineLevel.gameObject.SetActive(true);
				this.mRefineLevel.text = this.tempStr;
				this.mRefineLevelBG.enabled = false;
			}
			this.tempStr = this.GetDesc();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mDesc.enabled = false;
			}
			else
			{
				this.mDesc.enabled = true;
				this.mDesc.text = this.tempStr;
			}
			this.tempStr = this.GetRelation();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mRelation.enabled = false;
			}
			else
			{
				this.mRelation.enabled = true;
				this.mRelation.text = this.tempStr;
			}
			if (this.ShowStars())
			{
				this.mStars.Show();
				this.tempInt = this.GetStarsNum();
				this.mStars.Refresh(this.tempInt);
			}
			else
			{
				this.mStars.Hide();
			}
			this.mOwner.enabled = false;
			this.mPrice.gameObject.SetActive(false);
			if (this.mListBtn.activeInHierarchy)
			{
				if (this.mPetData.State == 1)
				{
					this.mInfo.gameObject.SetActive(false);
					this.mList.SetActive(true);
					this.mListBtn.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);
					this.mRefineBtn.gameObject.SetActive(true);
					this.mEnhanceBtn.gameObject.SetActive(true);
					if (GUICommonBagItem.mCurBagItem != this)
					{
						GUICommonBagItem.mCurBagItem = this;
						GUICommonBagItem.mCurBagItemPetData = this.mPetData;
					}
				}
				else
				{
					this.mInfo.gameObject.SetActive(true);
					this.mList.SetActive(false);
					this.mListBtn.transform.localRotation = Quaternion.identity;
					this.mRefineBtn.gameObject.SetActive(false);
					this.mEnhanceBtn.gameObject.SetActive(false);
				}
				if (this.ShowListGoBtn())
				{
					this.mListGoBtn.SetActive(true);
				}
				else
				{
					this.mListGoBtn.SetActive(false);
				}
			}
			else
			{
				this.mListBtn.SetActive(false);
				this.tempStr = this.GetClickableBtnTxt();
				if (string.IsNullOrEmpty(this.tempStr))
				{
					this.mClickableBtnSprite.gameObject.SetActive(false);
				}
				else
				{
					this.mClickableBtnSprite.gameObject.SetActive(true);
					this.mClickableBtnTxt.text = this.tempStr;
					NGUITools.SetActive(this.mClickableBtnEffect, this.ShowClickableBtnEffect());
				}
				this.tempBool = this.EnableClickableBtn();
				if (this.tempBool)
				{
					this.mClickableBtn.isEnabled = true;
					for (int i = 0; i < this.mClickableBtns.Length; i++)
					{
						this.mClickableBtns[i].SetState(UIButtonColor.State.Normal, true);
					}
				}
				else
				{
					this.mClickableBtn.isEnabled = false;
					for (int j = 0; j < this.mClickableBtns.Length; j++)
					{
						this.mClickableBtns[j].SetState(UIButtonColor.State.Disabled, true);
					}
				}
			}
		}
		else if (this.mLopetData != null)
		{
			this.mIconItem.Refresh(this.mLopetData, false, false, false);
			this.mIconItem.SetMask = this.ShowIconGray();
			this.mIconItem.EnableLeftTopTag = this.ShowIconLeftTopTag();
			this.mIconItem.SetLeftTopTag(this.GetIconLeftTopTagLb());
			this.tempStr = this.GetLevel();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mLevel.enabled = false;
			}
			else
			{
				this.mLevel.enabled = true;
				this.mLevel.text = this.tempStr;
			}
			this.tempStr = this.GetName();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mName.enabled = false;
			}
			else
			{
				this.mName.enabled = true;
				this.mName.color = Tools.GetItemQualityColor(this.mLopetData.Info.Quality);
				this.mName.text = this.tempStr;
			}
			this.mPoint0.text = this.GetPoint0();
			this.mPoint0Value.text = this.GetPoint0Value();
			this.mPoint1.text = this.GetPoint1();
			this.mPoint1Value.text = this.GetPoint1Value();
			this.mEnhanceLb.text = this.GetEnhanceLb();
			this.mRefineLb.text = this.GetRefineLb();
			this.tempStr = this.GetTip();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mRefineLevel.gameObject.SetActive(false);
			}
			else
			{
				this.mRefineLevel.gameObject.SetActive(true);
				this.mRefineLevel.text = this.tempStr;
				this.mRefineLevelBG.enabled = false;
			}
			this.tempStr = this.GetDesc();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mDesc.enabled = false;
			}
			else
			{
				this.mDesc.enabled = true;
				this.mDesc.text = this.tempStr;
			}
			this.tempStr = this.GetRelation();
			if (string.IsNullOrEmpty(this.tempStr))
			{
				this.mRelation.enabled = false;
			}
			else
			{
				this.mRelation.enabled = true;
				this.mRelation.text = this.tempStr;
			}
			if (this.ShowStars())
			{
				this.mStars.Show();
				this.tempInt = this.GetStarsNum();
				this.mStars.Refresh(this.tempInt);
			}
			else
			{
				this.mStars.Hide();
			}
			this.mOwner.enabled = false;
			this.mPrice.gameObject.SetActive(false);
			if (this.mListBtn.activeInHierarchy)
			{
				if (this.mLopetData.State == 1)
				{
					this.mInfo.gameObject.SetActive(false);
					this.mList.SetActive(true);
					this.mListBtn.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);
					this.mRefineBtn.gameObject.SetActive(true);
					this.mEnhanceBtn.gameObject.SetActive(true);
					if (GUICommonBagItem.mCurBagItem != this)
					{
						GUICommonBagItem.mCurBagItem = this;
						GUICommonBagItem.mCurBagItemLopetData = this.mLopetData;
					}
				}
				else
				{
					this.mInfo.gameObject.SetActive(true);
					this.mList.SetActive(false);
					this.mListBtn.transform.localRotation = Quaternion.identity;
					this.mRefineBtn.gameObject.SetActive(false);
					this.mEnhanceBtn.gameObject.SetActive(false);
				}
				this.tempStr = this.GetEnhanceMax();
				if (string.IsNullOrEmpty(this.tempStr))
				{
					this.mEnhanceMax.enabled = false;
				}
				else
				{
					this.mEnhanceMax.enabled = true;
					this.mEnhanceMax.text = this.tempStr;
				}
				this.tempStr = this.GetRefineMax();
				if (string.IsNullOrEmpty(this.tempStr))
				{
					this.mRefineMax.enabled = false;
				}
				else
				{
					this.mRefineMax.enabled = true;
					this.mRefineMax.text = this.tempStr;
				}
				if (this.ShowListGoBtn())
				{
					this.mListGoBtn.SetActive(true);
				}
				else
				{
					this.mListGoBtn.SetActive(false);
				}
			}
			else
			{
				this.mListBtn.SetActive(false);
				this.tempStr = this.GetClickableBtnTxt();
				if (string.IsNullOrEmpty(this.tempStr))
				{
					this.mClickableBtnSprite.gameObject.SetActive(false);
				}
				else
				{
					this.mClickableBtnSprite.gameObject.SetActive(true);
					this.mClickableBtnTxt.text = this.tempStr;
					NGUITools.SetActive(this.mClickableBtnEffect, this.ShowClickableBtnEffect());
				}
			}
		}
	}

	public void OnListBtnClick(GameObject go)
	{
		if (HOTween.IsTweening(this.mListBtn.transform))
		{
			if (!(go == null))
			{
				return;
			}
			HOTween.Kill(this.mListBtn.transform);
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mData != null)
		{
			if (this.mData.BtnsVisible)
			{
				this.mData.BtnsVisible = false;
				base.StartCoroutine(this.HideListBtns());
				if (GUICommonBagItem.mCurBagItemData == this.mData)
				{
					GUICommonBagItem.mCurBagItemData = null;
					GUICommonBagItem.mCurBagItem = null;
				}
			}
			else
			{
				this.mData.BtnsVisible = true;
				base.StartCoroutine(this.ShowListBtns());
				if (GUICommonBagItem.mCurBagItemData != null && GUICommonBagItem.mCurBagItem != null && GUICommonBagItem.mCurBagItemData != this.mData)
				{
					if (!(GUICommonBagItem.mCurBagItem == null))
					{
						if (GUICommonBagItem.mCurBagItemData == GUICommonBagItem.mCurBagItem.mData)
						{
							GUICommonBagItem.mCurBagItem.OnListBtnClick(null);
						}
						else
						{
							GUICommonBagItem.mCurBagItemData.BtnsVisible = false;
						}
					}
				}
				GUICommonBagItem.mCurBagItemData = this.mData;
				GUICommonBagItem.mCurBagItem = this;
			}
		}
		else if (this.mLopetData != null)
		{
			if (this.mLopetData.State == 1)
			{
				this.mLopetData.State = 0;
				base.StartCoroutine(this.HideListBtns());
				if (GUICommonBagItem.mCurBagItemLopetData == this.mLopetData)
				{
					GUICommonBagItem.mCurBagItemLopetData = null;
					GUICommonBagItem.mCurBagItem = null;
				}
			}
			else
			{
				this.mLopetData.State = 1;
				base.StartCoroutine(this.ShowListBtns());
				if (GUICommonBagItem.mCurBagItemLopetData != null && GUICommonBagItem.mCurBagItem != null && GUICommonBagItem.mCurBagItemLopetData != this.mLopetData)
				{
					if (!(GUICommonBagItem.mCurBagItem == null))
					{
						if (GUICommonBagItem.mCurBagItemLopetData == GUICommonBagItem.mCurBagItem.mLopetData)
						{
							GUICommonBagItem.mCurBagItem.OnListBtnClick(null);
						}
						else
						{
							GUICommonBagItem.mCurBagItemLopetData.State = 0;
						}
					}
				}
				GUICommonBagItem.mCurBagItemLopetData = this.mLopetData;
				GUICommonBagItem.mCurBagItem = this;
			}
		}
		else if (this.mPetData != null)
		{
			if (this.mPetData.State == 1)
			{
				this.mPetData.State = 0;
				base.StartCoroutine(this.HideListBtns());
				if (GUICommonBagItem.mCurBagItemPetData == this.mPetData)
				{
					GUICommonBagItem.mCurBagItemPetData = null;
					GUICommonBagItem.mCurBagItem = null;
				}
			}
			else
			{
				this.mPetData.State = 1;
				base.StartCoroutine(this.ShowListBtns());
				if (GUICommonBagItem.mCurBagItemPetData != null && GUICommonBagItem.mCurBagItem != null && GUICommonBagItem.mCurBagItemPetData != this.mPetData)
				{
					if (!(GUICommonBagItem.mCurBagItem == null))
					{
						if (GUICommonBagItem.mCurBagItemPetData == GUICommonBagItem.mCurBagItem.mPetData)
						{
							GUICommonBagItem.mCurBagItem.OnListBtnClick(null);
						}
						else
						{
							GUICommonBagItem.mCurBagItemPetData.State = 0;
						}
					}
				}
				GUICommonBagItem.mCurBagItemPetData = this.mPetData;
				GUICommonBagItem.mCurBagItem = this;
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator ShowListBtns()
	{
        return null;
        //GUICommonBagItem.<ShowListBtns>c__Iterator42 <ShowListBtns>c__Iterator = new GUICommonBagItem.<ShowListBtns>c__Iterator42();
        //<ShowListBtns>c__Iterator.<>f__this = this;
        //return <ShowListBtns>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator HideListBtns()
	{
        return null;
        //GUICommonBagItem.<HideListBtns>c__Iterator43 <HideListBtns>c__Iterator = new GUICommonBagItem.<HideListBtns>c__Iterator43();
        //<HideListBtns>c__Iterator.<>f__this = this;
        //return <HideListBtns>c__Iterator;
	}

	public virtual void OnClickableBtnClick(GameObject go)
	{
	}

	protected virtual void OnEnhanceClick(GameObject go)
	{
	}

	public virtual void OnRefineClick(GameObject go)
	{
	}

	protected virtual void OnListGoBtnClick(GameObject go)
	{
	}

	protected virtual bool OnPreSelectToggleChange(bool isCheck)
	{
		return true;
	}

	protected virtual void OnSelectToggleChange(bool isCheck)
	{
	}

	protected abstract void OnIconClick(GameObject go);
}
