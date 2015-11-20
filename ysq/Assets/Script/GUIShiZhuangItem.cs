using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIShiZhuangItem : UICustomGridItem
{
	private GUIShiZhuangSceneV2 mBaseScene;

	private GUIShiZhuangItemTable mBaseLayer;

	private ShiZhuangItemData mShiZhuangItemData;

	private UISprite mItemIcon;

	private UISprite mQualityMask;

	private UILabel mName;

	private UILabel mTimeDesc;

	private GameObject mEquipedTag;

	private GameObject mHowGetBtn;

	private GameObject mEquipBtn;

	private GameObject mSelectedGo;

	private GameObject mMask;

	private string mShiZhuangTxt2;

	private string mShiZhuangTxt3;

	private string mTimeTxt3;

	private StringBuilder mSb = new StringBuilder(42);

	public void InitWithBaseScene(GUIShiZhuangSceneV2 baseScene, GUIShiZhuangItemTable baseLayer)
	{
		this.mBaseScene = baseScene;
		this.mBaseLayer = baseLayer;
		this.CreateObjects();
		this.mShiZhuangTxt2 = Singleton<StringManager>.Instance.GetString("shiZhuangTxt2");
		this.mShiZhuangTxt3 = Singleton<StringManager>.Instance.GetString("shiZhuangTxt3");
		this.mTimeTxt3 = Singleton<StringManager>.Instance.GetString("timeTxt3");
	}

	private void CreateObjects()
	{
		this.mName = base.transform.Find("title").GetComponent<UILabel>();
		Transform transform = base.transform.Find("item0");
		this.mItemIcon = transform.Find("icon").GetComponent<UISprite>();
		this.mQualityMask = transform.Find("qualityMask").GetComponent<UISprite>();
		this.mEquipedTag = base.transform.Find("equiping").gameObject;
		this.mHowGetBtn = base.transform.Find("howGetBtn").gameObject;
		UIEventListener expr_99 = UIEventListener.Get(this.mHowGetBtn);
		expr_99.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_99.onClick, new UIEventListener.VoidDelegate(this.OnHowGetBtnClick));
		this.mEquipBtn = base.transform.Find("equipBtn").gameObject;
		UIEventListener expr_E0 = UIEventListener.Get(this.mEquipBtn);
		expr_E0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_E0.onClick, new UIEventListener.VoidDelegate(this.OnEquipBtnClick));
		UIEventListener expr_10C = UIEventListener.Get(base.gameObject);
		expr_10C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_10C.onClick, new UIEventListener.VoidDelegate(this.OnSelectClick));
		this.mSelectedGo = base.transform.Find("selected").gameObject;
		this.mTimeDesc = transform.Find("txt0").GetComponent<UILabel>();
		this.mTimeDesc.text = string.Empty;
		this.mMask = transform.Find("mask").gameObject;
		this.mMask.SetActive(false);
	}

	public override void Refresh(object data)
	{
		if (this.mShiZhuangItemData != data)
		{
			this.mShiZhuangItemData = (ShiZhuangItemData)data;
			this.Refresh();
		}
	}

	public void Refresh()
	{
		ItemSubSystem itemSystem = Globals.Instance.Player.ItemSystem;
		if (itemSystem != null && this.mShiZhuangItemData != null && this.mShiZhuangItemData.mFashionInfo != null)
		{
			this.mSb.Remove(0, this.mSb.Length).Append(this.mShiZhuangItemData.mFashionInfo.Name);
			if (itemSystem.IsShiXiaoFashion(this.mShiZhuangItemData.mFashionInfo))
			{
				this.mSb.Append("(").AppendFormat(this.mTimeTxt3, this.mShiZhuangItemData.mFashionInfo.Day).Append(")");
			}
			this.mName.text = this.mSb.ToString();
			this.mItemIcon.spriteName = this.mShiZhuangItemData.mFashionInfo.Icon;
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mShiZhuangItemData.mFashionInfo.Quality);
			if (itemSystem.HasFashion(this.mShiZhuangItemData.mFashionInfo.ID))
			{
				if (itemSystem.IsShiXiaoFashion(this.mShiZhuangItemData.mFashionInfo))
				{
					this.mTimeDesc.gameObject.SetActive(true);
					if (itemSystem.IsFashionGuoqi(this.mShiZhuangItemData.mFashionInfo.ID))
					{
						this.mTimeDesc.text = this.mShiZhuangTxt3;
						this.mMask.SetActive(true);
						this.mEquipBtn.SetActive(false);
						this.mHowGetBtn.SetActive(true);
						this.mEquipedTag.SetActive(false);
					}
					else
					{
						int num = itemSystem.GetFashionTime(this.mShiZhuangItemData.mFashionInfo.ID) - Globals.Instance.Player.GetTimeStamp();
						int num2 = Mathf.Max(num / 86400, 1);
						this.mTimeDesc.text = this.mSb.Remove(0, this.mSb.Length).AppendFormat(this.mShiZhuangTxt2, num2).ToString();
						this.mMask.SetActive(false);
						if (this.mShiZhuangItemData.mFashionInfo.ID == Globals.Instance.Player.GetCurFashionID())
						{
							this.mEquipBtn.SetActive(false);
							this.mHowGetBtn.SetActive(false);
							this.mEquipedTag.SetActive(true);
						}
						else
						{
							this.mEquipBtn.SetActive(true);
							this.mHowGetBtn.SetActive(false);
							this.mEquipedTag.SetActive(false);
						}
					}
				}
				else
				{
					this.mTimeDesc.gameObject.SetActive(false);
					this.mMask.SetActive(false);
					if (this.mShiZhuangItemData.mFashionInfo.ID == Globals.Instance.Player.GetCurFashionID())
					{
						this.mEquipBtn.SetActive(false);
						this.mHowGetBtn.SetActive(false);
						this.mEquipedTag.SetActive(true);
					}
					else
					{
						this.mEquipBtn.SetActive(true);
						this.mHowGetBtn.SetActive(false);
						this.mEquipedTag.SetActive(false);
					}
				}
			}
			else
			{
				this.mEquipBtn.SetActive(false);
				this.mHowGetBtn.SetActive(true);
				this.mEquipedTag.SetActive(false);
				this.mTimeDesc.gameObject.SetActive(false);
				this.mMask.SetActive(false);
			}
		}
		this.mSelectedGo.SetActive(this.mShiZhuangItemData.mIsSelected);
	}

	private void OnSelectClick(GameObject go)
	{
		if (this.mShiZhuangItemData != null && !this.mShiZhuangItemData.mIsSelected)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
			this.mBaseLayer.SetCurSelectData(this.mShiZhuangItemData.mFashionInfo.ID);
			this.mBaseScene.CreateModel();
		}
	}

	private void OnHowGetBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mShiZhuangItemData != null && this.mShiZhuangItemData.mFashionInfo != null)
		{
			GUIHowGetPetItemPopUp.ShowThis(this.mShiZhuangItemData.mFashionInfo);
		}
	}

	private void OnEquipBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		ItemSubSystem itemSystem = Globals.Instance.Player.ItemSystem;
		if (itemSystem != null && this.mShiZhuangItemData != null && this.mShiZhuangItemData.mFashionInfo != null && itemSystem.HasFashion(this.mShiZhuangItemData.mFashionInfo.ID))
		{
			SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(0);
			if (socket != null && this.mShiZhuangItemData.mFashionInfo.ID != socket.GetFashionID())
			{
				MC2S_ChangeFashion mC2S_ChangeFashion = new MC2S_ChangeFashion();
				mC2S_ChangeFashion.FashionID = this.mShiZhuangItemData.mFashionInfo.ID;
				Globals.Instance.CliSession.Send(201, mC2S_ChangeFashion);
			}
		}
	}

	public int GetFashionID()
	{
		if (this.mShiZhuangItemData != null && this.mShiZhuangItemData.mFashionInfo != null)
		{
			return this.mShiZhuangItemData.mFashionInfo.ID;
		}
		return 0;
	}
}
