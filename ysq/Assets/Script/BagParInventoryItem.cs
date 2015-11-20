using Proto;
using System;
using UnityEngine;

public class BagParInventoryItem : UICustomGridItem
{
	public ItemDataEx mItemDataEx;

	private UISprite mParBg;

	private UISprite mChip;

	private UISprite mInfoBg;

	private UISprite mMengBan;

	private GameObject mComposeBtn;

	private GameObject mGetBtn;

	private UILabel mName;

	private UILabel mCount;

	private UILabel mGet;

	private UILabel mHeCheng;

	private UILabel mCountNeed;

	public void InitItemData(GUIPartnerManageScene baseScene)
	{
		this.CreateObjects();
	}

	public void CreateObjects()
	{
		this.mParBg = base.transform.Find("parBg").GetComponent<UISprite>();
		this.mChip = this.mParBg.transform.Find("chip").GetComponent<UISprite>();
		this.mInfoBg = base.transform.Find("InfoBg").GetComponent<UISprite>();
		this.mCount = this.mInfoBg.transform.Find("num").GetComponent<UILabel>();
		this.mCountNeed = this.mInfoBg.transform.Find("countNeed").GetComponent<UILabel>();
		this.mComposeBtn = base.transform.Find("ComposeBtn").gameObject;
		this.mGetBtn = base.transform.Find("GetBtn").gameObject;
		this.mMengBan = this.mParBg.transform.Find("mengban").GetComponent<UISprite>();
		this.mGet = this.mGetBtn.GetComponent<UISprite>().transform.Find("get").GetComponent<UILabel>();
		this.mHeCheng = this.mComposeBtn.GetComponent<UISprite>().transform.Find("com").GetComponent<UILabel>();
		this.mName = this.mInfoBg.transform.Find("name").GetComponent<UILabel>();
		UIEventListener expr_161 = UIEventListener.Get(this.mComposeBtn);
		expr_161.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_161.onClick, new UIEventListener.VoidDelegate(this.OnComposeBtnClicked));
		UIEventListener expr_18D = UIEventListener.Get(this.mGetBtn);
		expr_18D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_18D.onClick, new UIEventListener.VoidDelegate(this.OnGetBtnClicked));
		UIEventListener expr_1BE = UIEventListener.Get(this.mParBg.gameObject);
		expr_1BE.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1BE.onClick, new UIEventListener.VoidDelegate(this.OnItemBtnClicked));
	}

	private void OnGetBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.SelectItemID = this.mItemDataEx.Data.ID;
		GUIHowGetPetItemPopUp.ShowThis(this.mItemDataEx.Info);
	}

	private void OnComposeBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mItemDataEx == null || !this.mItemDataEx.CanCreate())
		{
			return;
		}
		if (Tools.IsPetBagFull())
		{
			return;
		}
		MC2S_SummonPet mC2S_SummonPet = new MC2S_SummonPet();
		mC2S_SummonPet.ItemID = this.mItemDataEx.Data.ID;
		Globals.Instance.CliSession.Send(504, mC2S_SummonPet);
		this.Refresh();
	}

	public void OnItemBtnClicked(GameObject go)
	{
		if (this.mItemDataEx != null)
		{
			GameUIManager.mInstance.ShowPetSliceInfo(this.mItemDataEx.Info);
		}
	}

	public override void Refresh(object data)
	{
		if (this.mItemDataEx == data)
		{
			return;
		}
		this.mItemDataEx = (ItemDataEx)data;
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.mItemDataEx != null)
		{
			this.mName.text = this.mItemDataEx.Info.Name;
			this.mName.color = Tools.GetItemQualityColor(this.mItemDataEx.Info.Quality);
			this.mChip.spriteName = this.mItemDataEx.Info.Icon;
			this.mMengBan.gameObject.SetActive(true);
			if (!this.mItemDataEx.CanCreate())
			{
				this.mComposeBtn.SetActive(false);
				this.mGetBtn.SetActive(true);
				this.mCount.text = Singleton<StringManager>.Instance.GetString("PetClipCount");
				this.mCountNeed.text = Singleton<StringManager>.Instance.GetString("PetClipCountNeed", new object[]
				{
					this.mItemDataEx.GetCount(),
					this.mItemDataEx.Info.Value1
				});
				this.mGet.text = Singleton<StringManager>.Instance.GetString("PetGet");
				this.mCountNeed.color = Color.red;
			}
			else
			{
				this.mComposeBtn.SetActive(true);
				this.mGetBtn.SetActive(false);
				this.mHeCheng.text = Singleton<StringManager>.Instance.GetString("PetComposite");
				this.mCount.text = Singleton<StringManager>.Instance.GetString("PetClipCount");
				this.mCountNeed.text = Singleton<StringManager>.Instance.GetString("PetClipCountNeed", new object[]
				{
					this.mItemDataEx.GetCount(),
					this.mItemDataEx.Info.Value1
				});
				this.mCountNeed.color = Tools.GetItemQualityColor(this.mItemDataEx.Info.Quality);
			}
			this.mParBg.spriteName = Tools.GetItemQualityIcon(this.mItemDataEx.Info.Quality);
		}
		else
		{
			NGUITools.SetActive(this.mInfoBg.gameObject, false);
			this.mInfoBg.spriteName = string.Empty;
			this.mParBg.spriteName = string.Empty;
			this.mChip.spriteName = string.Empty;
		}
	}
}
