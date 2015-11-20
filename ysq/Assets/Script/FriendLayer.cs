using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FriendLayer : MonoBehaviour
{
	private GameObject mTips;

	private UILabel mFriendCount;

	private UILabel mFriendCountBg;

	private UILabel mKeyBg;

	private UILabel mKeyCount;

	private UIButton mSendBtn;

	private UIButton mGetBtn;

	private FriendCommonGrid mCommonFriendTable;

	private FriendData mData;

	private bool IsInit;

	public void Init()
	{
		this.CreateObjects();
	}

	public void RefreshLayer()
	{
		if (!this.IsInit)
		{
			this.IsInit = true;
			this.InitInventoryItems();
		}
		this.mFriendCountBg.enabled = true;
		this.mFriendCount.enabled = true;
		this.mKeyBg.enabled = true;
		this.mKeyCount.enabled = true;
		this.mSendBtn.gameObject.SetActive(true);
		this.mGetBtn.gameObject.SetActive(true);
		this.mFriendCountBg.text = Singleton<StringManager>.Instance.GetString("friend_5");
		this.mFriendCount.text = Singleton<StringManager>.Instance.GetString("friend_9", new object[]
		{
			this.mCommonFriendTable.mDatas.Count,
			GameConst.GetInt32(194)
		});
		if (this.mCommonFriendTable.mDatas.Count > 0)
		{
			this.mFriendCount.color = Color.green;
		}
		else
		{
			this.mFriendCount.color = new Color(1f, 1f, 0.8392157f);
		}
		this.mKeyBg.text = Singleton<StringManager>.Instance.GetString("friend_6");
		this.RefreshEngeryCount();
		this.RefreshOneClick();
		this.RefreshTips();
		this.mCommonFriendTable.repositionNow = true;
	}

	public void RefreshOneClick()
	{
		bool flag = Globals.Instance.Player.FriendSystem.PendingGiveFriendEnergy > 0;
		if (flag != this.mSendBtn.isEnabled)
		{
			this.mSendBtn.isEnabled = flag;
			Tools.SetButtonState(this.mSendBtn.gameObject, flag);
		}
		bool flag2 = this.CanAllEnergyGet();
		if (flag2 != this.mGetBtn.isEnabled)
		{
			this.mGetBtn.isEnabled = flag2;
			Tools.SetButtonState(this.mGetBtn.gameObject, flag2);
		}
	}

	public void RefreshEngeryCount()
	{
		int num = GameConst.GetInt32(127) - Globals.Instance.Player.Data.TakeFriendEnergy;
		if (num > 0)
		{
			this.mKeyCount.text = Singleton<StringManager>.Instance.GetString("friend_15", new object[]
			{
				num
			});
			this.mKeyCount.color = Color.green;
		}
		else
		{
			this.mKeyCount.text = "0";
			this.mKeyCount.color = Color.red;
		}
	}

	public void AddFriendItem(FriendData data)
	{
		if (data.GUID == 0uL || data.FriendType != 1)
		{
			return;
		}
		this.mCommonFriendTable.AddData(new FriendDataEx(data, EUITableLayers.ESL_Friend, null, null));
		this.RefreshLayer();
	}

	public void RemoveFriendItem(ulong id)
	{
		if (this.mCommonFriendTable.RemoveData(id))
		{
			this.RefreshLayer();
		}
	}

	public void UpdateFriendItem(ulong id)
	{
		if (id == 0uL || this.mCommonFriendTable.GetData(id) != null)
		{
			this.RefreshLayer();
		}
	}

	private void RefreshTips()
	{
		this.mTips.gameObject.SetActive(this.mCommonFriendTable.mDatas.Count == 0);
	}

	private void CreateObjects()
	{
		this.mTips = base.transform.Find("friendPanel/tips").gameObject;
		this.mTips.SetActive(false);
		this.mSendBtn = base.transform.Find("sendBtn").GetComponent<UIButton>();
		this.mGetBtn = base.transform.Find("getBtn").GetComponent<UIButton>();
		this.mSendBtn.gameObject.SetActive(false);
		this.mGetBtn.gameObject.SetActive(false);
		this.mFriendCountBg = base.transform.Find("friendCount").GetComponent<UILabel>();
		this.mFriendCount = this.mFriendCountBg.transform.Find("Label").GetComponent<UILabel>();
		this.mKeyBg = base.transform.Find("keyCount").GetComponent<UILabel>();
		this.mKeyCount = this.mKeyBg.transform.Find("label").GetComponent<UILabel>();
		this.mFriendCountBg.enabled = false;
		this.mFriendCount.enabled = false;
		this.mKeyBg.enabled = false;
		this.mKeyCount.enabled = false;
		GameUITools.RegisterClickEvent("addBtn", new UIEventListener.VoidDelegate(this.OnAddBtnClick), base.gameObject);
		UIEventListener expr_152 = UIEventListener.Get(this.mSendBtn.gameObject);
		expr_152.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_152.onClick, new UIEventListener.VoidDelegate(this.OnSendBtnClick));
		UIEventListener expr_183 = UIEventListener.Get(this.mGetBtn.gameObject);
		expr_183.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_183.onClick, new UIEventListener.VoidDelegate(this.OnGetBtnClick));
		this.mCommonFriendTable = base.transform.FindChild("friendPanel/friendContents").gameObject.AddComponent<FriendCommonGrid>();
		this.mCommonFriendTable.maxPerLine = 2;
		this.mCommonFriendTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mCommonFriendTable.cellWidth = 450f;
		this.mCommonFriendTable.cellHeight = 120f;
		this.mCommonFriendTable.gapHeight = 2f;
		this.mCommonFriendTable.gapWidth = 2f;
		this.mCommonFriendTable.focusID = GameUIManager.mInstance.uiState.SelectFriendID;
	}

	public bool CanAllEnergyGet()
	{
		return Globals.Instance.Player.Data.TakeFriendEnergy < GameConst.GetInt32(127) && Globals.Instance.Player.FriendSystem.PendingGetFriendEnergy > 0;
	}

	private void OnSendBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.FriendSystem.PendingGiveFriendEnergy > 0)
		{
			MC2S_GiveFriendEnergy mC2S_GiveFriendEnergy = new MC2S_GiveFriendEnergy();
			mC2S_GiveFriendEnergy.GUID = 0uL;
			Globals.Instance.CliSession.Send(319, mC2S_GiveFriendEnergy);
		}
	}

	private void OnGetBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.CanAllEnergyGet())
		{
			MC2S_TakeFriendEnergy mC2S_TakeFriendEnergy = new MC2S_TakeFriendEnergy();
			mC2S_TakeFriendEnergy.GUID = 0uL;
			Globals.Instance.CliSession.Send(321, mC2S_TakeFriendEnergy);
		}
	}

	private void OnAddBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIAddFriendPopUp.Show();
	}

	public void InitInventoryItems()
	{
		this.mCommonFriendTable.ClearData();
		List<FriendData> friends = Globals.Instance.Player.FriendSystem.friends;
		for (int i = 0; i < friends.Count; i++)
		{
			if (friends[i].GUID != 0uL && friends[i].FriendType == 1)
			{
				this.mCommonFriendTable.AddData(new FriendDataEx(friends[i], EUITableLayers.ESL_Friend, null, null));
			}
		}
	}
}
