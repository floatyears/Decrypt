using Att;
using System;
using UnityEngine;

public class AwakeItemDetailLayer : MonoBehaviour
{
	public GUIAwakeItemInfoPopUp mBaseScene;

	private bool initFlag;

	private ItemInfo mItemInfo;

	private AwakeItemListPanel mListPanel;

	private AwakeItemCreateInfoLayer mCreateInfoLayer;

	private AwakeItemSourceInfoLayer mSourceInfoLayer;

	private UISprite mSourceBG;

	public void Init(GUIAwakeItemInfoPopUp basescene)
	{
		this.mBaseScene = basescene;
	}

	private void CreateObjects()
	{
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		this.mListPanel = GameUITools.FindGameObject("ListPanel", base.gameObject).AddComponent<AwakeItemListPanel>();
		this.mListPanel.Init(this);
		this.mCreateInfoLayer = GameUITools.FindGameObject("CreateInfo", base.gameObject).AddComponent<AwakeItemCreateInfoLayer>();
		this.mCreateInfoLayer.Init(this);
		this.mSourceInfoLayer = GameUITools.FindGameObject("SourceInfo", base.gameObject).AddComponent<AwakeItemSourceInfoLayer>();
		this.mSourceInfoLayer.Init();
		this.mSourceBG = GameUITools.FindUISprite("SourceBG", base.gameObject);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.mBaseScene.PlayAnim(false, false);
	}

	public void Refresh(ItemInfo info, bool source)
	{
		if (!this.initFlag)
		{
			this.CreateObjects();
			this.initFlag = true;
		}
		if (info == null)
		{
			return;
		}
		this.mItemInfo = info;
		this.mListPanel.Refresh(this.mItemInfo, null);
		if (source)
		{
			if (this.mItemInfo.Source == 0)
			{
				global::Debug.LogErrorFormat("iteminfo dose not have source", new object[0]);
				return;
			}
			this.mCreateInfoLayer.gameObject.SetActive(false);
			this.mSourceInfoLayer.gameObject.SetActive(true);
			this.mSourceBG.enabled = true;
			this.mSourceInfoLayer.Refresh(this.mItemInfo);
		}
		else
		{
			if (Globals.Instance.AttDB.AwakeRecipeDict.GetInfo(this.mItemInfo.ID) == null)
			{
				global::Debug.LogErrorFormat("iteminfo can not be create", new object[0]);
				return;
			}
			this.mCreateInfoLayer.gameObject.SetActive(true);
			this.mSourceInfoLayer.gameObject.SetActive(false);
			this.mSourceBG.enabled = false;
			this.mCreateInfoLayer.Refresh(this.mItemInfo, null);
		}
	}

	public void Refresh(ItemInfo info, ItemInfo parentInfo, bool ifEnough = false)
	{
		if (ifEnough)
		{
			this.mListPanel.CheckIfEnough(info);
		}
		else
		{
			this.mListPanel.Refresh(info, parentInfo);
			if (Globals.Instance.AttDB.AwakeRecipeDict.GetInfo(info.ID) == null)
			{
				this.mCreateInfoLayer.gameObject.SetActive(false);
				this.mSourceInfoLayer.gameObject.SetActive(true);
				this.mSourceBG.enabled = true;
				this.mSourceInfoLayer.Refresh(info);
			}
			else
			{
				this.mCreateInfoLayer.gameObject.SetActive(true);
				this.mSourceInfoLayer.gameObject.SetActive(false);
				this.mSourceBG.enabled = false;
				this.mCreateInfoLayer.Refresh(info, parentInfo);
			}
		}
	}
}
