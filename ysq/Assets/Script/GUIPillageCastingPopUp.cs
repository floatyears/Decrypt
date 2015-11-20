using Att;
using Holoville.HOTween.Core;
using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GUIPillageCastingPopUp : MonoBehaviour
{
	private class CastItem : MonoBehaviour
	{
		public delegate void SelectCallback(GUIPillageCastingPopUp.CastItem item);

		public GUIPillageCastingPopUp.CastItem.SelectCallback OnSelectEvent;

		private GameObject mSelected;

		public ItemInfo mInfo;

		public void Init(ItemInfo info, GUIPillageCastingPopUp.CastItem.SelectCallback cb)
		{
			if (info == null)
			{
				base.gameObject.SetActive(false);
			}
			base.gameObject.SetActive(true);
			this.OnSelectEvent = cb;
			this.mInfo = info;
			UISprite uISprite = base.gameObject.GetComponent<UISprite>();
			uISprite.spriteName = this.mInfo.Icon;
			uISprite = GameUITools.FindUISprite("Quality", base.gameObject);
			uISprite.spriteName = Tools.GetItemQualityIcon(this.mInfo.Quality);
			this.mSelected = GameUITools.FindGameObject("Selected", base.gameObject);
			this.mSelected.gameObject.SetActive(false);
		}

		public void SetStatus(bool status)
		{
			this.mSelected.gameObject.SetActive(status);
		}

		private void OnClick()
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
			if (this.OnSelectEvent != null)
			{
				this.OnSelectEvent(this);
			}
		}
	}

	private class CastTrinketItem : MonoBehaviour
	{
		public delegate void VoidCallback();

		public GUIPillageCastingPopUp.CastTrinketItem.VoidCallback OnClickEvent;

		private UISprite mQuality;

		private UISprite mIcon;

		private UISprite mAdd;

		private GameObject mUI59_1;

		private ItemDataEx mData;

		public bool IsNull
		{
			get
			{
				return this.mData == null;
			}
			private set
			{
			}
		}

		public ulong GetID()
		{
			if (this.mData != null)
			{
				return this.mData.GetID();
			}
			return 0uL;
		}

		public void Init(GUIPillageCastingPopUp.CastTrinketItem.VoidCallback cb)
		{
			this.OnClickEvent = cb;
			this.mQuality = GameUITools.FindUISprite("Quality", base.gameObject);
			this.mIcon = GameUITools.FindUISprite("Icon", base.gameObject);
			this.mAdd = GameUITools.FindUISprite("Add", base.gameObject);
			this.mUI59_1 = GameUITools.FindGameObject("ui59_1", base.gameObject);
			Tools.SetParticleRenderQueue(this.mUI59_1, 3300, 0.68f);
			this.mUI59_1.gameObject.SetActive(false);
			this.mQuality.enabled = false;
			this.mIcon.enabled = false;
			this.mAdd.enabled = true;
		}

		public void Refresh(ItemDataEx data)
		{
			this.mData = data;
			if (this.mData != null)
			{
				if (this.mData.Info == null)
				{
					this.Clear();
				}
				else
				{
					this.mIcon.enabled = true;
					this.mQuality.enabled = true;
					this.mAdd.enabled = false;
					this.mIcon.spriteName = this.mData.Info.Icon;
					this.mQuality.spriteName = Tools.GetItemQualityIcon(this.mData.Info.Quality);
				}
			}
			else
			{
				this.Clear();
			}
		}

		private void Clear()
		{
			this.mIcon.enabled = false;
			this.mQuality.enabled = false;
			this.mAdd.enabled = true;
		}

		private void OnClick()
		{
			if (this.OnClickEvent != null)
			{
				this.OnClickEvent();
			}
		}

		public void PlaySfx()
		{
			this.mUI59_1.gameObject.SetActive(false);
			this.mUI59_1.gameObject.SetActive(true);
		}

		public void StopSfx()
		{
			this.mUI59_1.gameObject.SetActive(false);
		}
	}

	private GameObject mBG;

	private UILabel mName;

	private UILabel mValue0;

	private UILabel mValue1;

	private GameObject mRecipeContent;

	private GameObject mUI59;

	private UISprite mRightQuality;

	private UISprite mRightIcon;

	private UILabel mRightTrinketName;

	private UILabel mRightTips;

	private UILabel mRightName;

	[NonSerialized]
	public int CastItemCount;

	private GUIPillageCastingPopUp.CastTrinketItem[] castTrinketItems;

	private GUIPillageCastingPopUp.CastItem mCurItem;

	private ItemInfo tInfo;

	private SelectTrinketPopUp mSelectTrinketPopUp;

	private UnityEngine.Object castTrinketItemPrefab;

	public void Init()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.CastItemCount = 3;
		this.castTrinketItems = new GUIPillageCastingPopUp.CastTrinketItem[this.CastItemCount];
		this.mSelectTrinketPopUp = GameUITools.FindGameObject("SelectTrinketPopUp", base.gameObject).AddComponent<SelectTrinketPopUp>();
		this.mSelectTrinketPopUp.Init(this);
		this.mSelectTrinketPopUp.Hide();
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		this.mBG = GameUITools.FindGameObject("BG", base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mBG);
		GameObject gameObject = GameUITools.FindGameObject("Desc", this.mBG);
		this.mName = GameUITools.FindUILabel("Name", gameObject);
		this.mValue0 = GameUITools.FindUILabel("Value0", gameObject);
		this.mValue1 = GameUITools.FindUILabel("Value1", gameObject);
		gameObject = GameUITools.FindGameObject("Right", this.mBG);
		this.mRecipeContent = GameUITools.FindGameObject("RecipeContent", gameObject);
		this.mUI59 = GameUITools.FindGameObject("ui59", this.mRecipeContent);
		this.mUI59.gameObject.SetActive(false);
		Tools.SetParticleRenderQueue(this.mUI59, 3300, 0.68f);
		this.mRightQuality = GameUITools.RegisterClickEvent("Trinket", new UIEventListener.VoidDelegate(this.OnTrinketClick), this.mRecipeContent).GetComponent<UISprite>();
		this.mRightIcon = GameUITools.FindUISprite("Icon", this.mRightQuality.gameObject);
		this.mRightTrinketName = GameUITools.FindUILabel("Name", this.mRightQuality.gameObject);
		this.mRightTips = GameUITools.FindUILabel("Tips", gameObject);
		this.mRightName = GameUITools.FindUILabel("Name", this.mRightTips.gameObject);
		GameUITools.RegisterClickEvent("AutoAddBtn", new UIEventListener.VoidDelegate(this.OnAutoAddBtnClick), gameObject);
		GameUITools.RegisterClickEvent("CastingBtn", new UIEventListener.VoidDelegate(this.OnCastingBtnClick), gameObject);
		gameObject = GameUITools.FindGameObject("Items", this.mBG);
		int num = Mathf.Min(gameObject.transform.childCount, 4) - 1;
		int i = 0;
		foreach (ItemInfo current in Globals.Instance.AttDB.ItemDict.Values)
		{
			if (current.Type == 1 && current.Quality == 4)
			{
				if (i > num)
				{
					break;
				}
				GUIPillageCastingPopUp.CastItem castItem = gameObject.transform.GetChild(i).gameObject.AddComponent<GUIPillageCastingPopUp.CastItem>();
				castItem.Init(current, new GUIPillageCastingPopUp.CastItem.SelectCallback(this.OnSelect));
				if (i == 0)
				{
					this.OnSelect(castItem);
				}
				i++;
			}
		}
		while (i <= num)
		{
			gameObject.transform.GetChild(i).gameObject.SetActive(false);
			i++;
		}
	}

	private void OnSelect(GUIPillageCastingPopUp.CastItem item)
	{
		if (this.mCurItem == item)
		{
			return;
		}
		if (this.mCurItem != null)
		{
			this.mCurItem.SetStatus(false);
		}
		this.mCurItem = item;
		this.mCurItem.SetStatus(true);
		this.mName.text = item.mInfo.Name + Singleton<StringManager>.Instance.GetString("Colon0");
		this.mValue0.text = Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
		{
			Tools.GetTrinketAEStr(new ItemDataEx(null, item.mInfo), 0),
			item.mInfo.Value1
		});
		this.mValue1.text = Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
		{
			Tools.GetTrinketAEStr(new ItemDataEx(null, item.mInfo), 1),
			Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
			{
				((float)item.mInfo.Value2 / 100f).ToString("0.0")
			})
		});
		this.RefreshRight();
	}

	private void RefreshRight()
	{
		if (this.mCurItem == null)
		{
			return;
		}
		this.tInfo = null;
		foreach (ItemInfo current in Globals.Instance.AttDB.ItemDict.Values)
		{
			if (current.Type == this.mCurItem.mInfo.Type && current.SubType == this.mCurItem.mInfo.SubType && current.Quality == this.mCurItem.mInfo.Quality - 1 && current.Value5 == this.mCurItem.mInfo.Value5)
			{
				this.tInfo = current;
				break;
			}
		}
		if (this.tInfo == null)
		{
			this.mRightTips.gameObject.SetActive(false);
		}
		else
		{
			this.mRightTips.gameObject.SetActive(true);
			this.mRightName.text = this.tInfo.Name;
			Vector3 localPosition = this.mRightTips.transform.localPosition;
			localPosition.x = (float)(-(float)(this.mRightTips.width + this.mRightName.width) / 2);
			this.mRightTips.transform.localPosition = localPosition;
		}
		this.RefreshCastTrinketItems();
	}

	private void RefreshCastTrinketItems()
	{
		if (this.mCurItem == null)
		{
			return;
		}
		int castItemCount = this.CastItemCount;
		if (castItemCount == 0)
		{
			return;
		}
		this.mRightQuality.spriteName = Tools.GetItemQualityIcon(this.mCurItem.mInfo.Quality);
		this.mRightIcon.spriteName = this.mCurItem.mInfo.Icon;
		this.mRightTrinketName.text = this.mCurItem.mInfo.Name;
		this.mRightTrinketName.color = Tools.GetItemQualityColor(this.mCurItem.mInfo.Quality);
		float num = 120f;
		float num2 = 3.14159274f / (float)castItemCount * 2f;
		float num3 = (float)((castItemCount + 1) % 2) * 3.14159274f / (float)castItemCount;
		for (int i = 0; i < castItemCount; i++)
		{
			float f = num2 * (float)i + num3;
			if (this.castTrinketItems[i] == null)
			{
				if (this.castTrinketItemPrefab == null)
				{
					this.castTrinketItemPrefab = Res.LoadGUI("GUI/CastTrinketItem");
				}
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.castTrinketItemPrefab);
				gameObject.transform.parent = this.mRecipeContent.transform;
				gameObject.transform.localPosition = new Vector3(num * Mathf.Sin(f), num * Mathf.Cos(f), 0f);
				gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
				GUIPillageCastingPopUp.CastTrinketItem castTrinketItem = gameObject.AddComponent<GUIPillageCastingPopUp.CastTrinketItem>();
				castTrinketItem.Init(new GUIPillageCastingPopUp.CastTrinketItem.VoidCallback(this.OnCastTrinketItemClick));
				castTrinketItem.Refresh(null);
				this.castTrinketItems[i] = castTrinketItem;
			}
			else
			{
				this.castTrinketItems[i].Refresh(null);
				this.castTrinketItems[i].transform.localPosition = new Vector3(num * Mathf.Sin(f), num * Mathf.Cos(f), 0f);
			}
			this.castTrinketItems[i].gameObject.SetActive(true);
		}
		for (int j = castItemCount; j < this.castTrinketItems.Length; j++)
		{
			if (this.castTrinketItems[j] != null)
			{
				this.castTrinketItems[j].gameObject.SetActive(false);
			}
		}
	}

	private void OnTrinketClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mCurItem != null)
		{
			GameUIManager.mInstance.ShowItemInfo(this.mCurItem.mInfo);
		}
	}

	private void OnCastTrinketItemClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mCurItem == null && this.tInfo == null)
		{
			return;
		}
		List<ulong> list = new List<ulong>();
		for (int i = 0; i < this.castTrinketItems.Length; i++)
		{
			if (this.castTrinketItems[i].gameObject.activeInHierarchy && !this.castTrinketItems[i].IsNull)
			{
				list.Add(this.castTrinketItems[i].GetID());
			}
		}
		this.mSelectTrinketPopUp.Open(this.tInfo.ID, list);
	}

	public void Open()
	{
		base.gameObject.SetActive(true);
		GameUITools.PlayOpenWindowAnim(this.mBG.transform, null, true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
		this.RefreshCastTrinketItems();
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.mBG.transform, new TweenDelegate.TweenCallback(this.Hide), true);
	}

	private void OnAutoAddBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mCurItem == null && this.tInfo == null)
		{
			return;
		}
		int num = 0;
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (current.Info.ID == this.tInfo.ID && current.IsTrinketAndCastItem())
			{
				this.castTrinketItems[num].Refresh(current);
				num++;
				if (num >= this.castTrinketItems.Length)
				{
					break;
				}
				if (!this.castTrinketItems[num].gameObject.activeInHierarchy)
				{
					break;
				}
			}
		}
		if (num == 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove83", 0f, 0f);
		}
	}

	private void OnCastingBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_TrinketCompound mC2S_TrinketCompound = new MC2S_TrinketCompound();
		for (int i = 0; i < this.castTrinketItems.Length; i++)
		{
			if (this.castTrinketItems[i].gameObject.activeInHierarchy)
			{
				if (this.castTrinketItems[i].IsNull)
				{
					GameUIManager.mInstance.ShowMessageTipByKey("equipImprove84", 0f, 0f);
					return;
				}
				mC2S_TrinketCompound.ItemID.Add(this.castTrinketItems[i].GetID());
			}
		}
		if (mC2S_TrinketCompound.ItemID.Count < this.CastItemCount)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove84", 0f, 0f);
			return;
		}
		mC2S_TrinketCompound.InfoID = this.mCurItem.mInfo.ID;
		Globals.Instance.CliSession.Send(542, mC2S_TrinketCompound);
	}

	public void AddCastTrinketItems(List<ItemDataEx> datas)
	{
		int i = 0;
		if (datas != null)
		{
			foreach (ItemDataEx current in datas)
			{
				if (current.Info.ID == this.tInfo.ID && current.IsTrinketAndCastItem())
				{
					this.castTrinketItems[i].Refresh(current);
					i++;
					if (i >= this.castTrinketItems.Length)
					{
						break;
					}
					if (!this.castTrinketItems[i].gameObject.activeInHierarchy)
					{
						break;
					}
				}
			}
		}
		while (i < this.castTrinketItems.Length)
		{
			this.castTrinketItems[i].Refresh(null);
			i++;
		}
	}

	public void PlayCompoundAnim(ulong id)
	{
		base.StartCoroutine(this.PlayCastingSfx(id));
	}

	[DebuggerHidden]
	private IEnumerator PlayCastingSfx(ulong itemID)
	{
        return null;
        //GUIPillageCastingPopUp.<PlayCastingSfx>c__Iterator83 <PlayCastingSfx>c__Iterator = new GUIPillageCastingPopUp.<PlayCastingSfx>c__Iterator83();
        //<PlayCastingSfx>c__Iterator.itemID = itemID;
        //<PlayCastingSfx>c__Iterator.<$>itemID = itemID;
        //<PlayCastingSfx>c__Iterator.<>f__this = this;
        //return <PlayCastingSfx>c__Iterator;
	}
}
