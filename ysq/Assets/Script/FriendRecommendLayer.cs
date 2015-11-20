using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FriendRecommendLayer : MonoBehaviour
{
	private FriendCommonGrid mFriendRecommendTable;

	private GameObject changeBtn;

	private GameObject BtnRefresh;

	private UILabel BtnRefreshText;

	private static List<FriendData> RecommendFriends = new List<FriendData>();

	private static float RecommendTimestamp = 0f;

	private float BtnRefreshTextTimer;

	public void Init()
	{
		this.CreateObjects();
	}

	public void RefreshLayer()
	{
		this.mFriendRecommendTable.repositionNow = true;
	}

	public void OnOpenTable()
	{
		if (this.BtnRefreshTextTimer - Time.realtimeSinceStartup < 0f && (FriendRecommendLayer.RecommendFriends.Count == 0 || Time.realtimeSinceStartup - FriendRecommendLayer.RecommendTimestamp > 180f))
		{
			this.SendRecommendFriendRequest();
		}
		else
		{
			this.RefreshRecommendFriend(FriendRecommendLayer.RecommendFriends);
			this.RefreshLayer();
		}
	}

	public void AddFriendItem(FriendData data)
	{
		if (data == null || data.GUID == 0uL)
		{
			return;
		}
		for (int i = 0; i < FriendRecommendLayer.RecommendFriends.Count; i++)
		{
			if (FriendRecommendLayer.RecommendFriends[i].GUID == data.GUID)
			{
				FriendRecommendLayer.RecommendFriends.RemoveAt(i);
				this.mFriendRecommendTable.RemoveData(data.GUID);
				this.RefreshLayer();
				break;
			}
		}
	}

	public void RefreshRecommendFriend(List<FriendData> datas)
	{
		FriendRecommendLayer.RecommendTimestamp = Time.realtimeSinceStartup;
		FriendRecommendLayer.RecommendFriends = datas;
		this.mFriendRecommendTable.ClearData();
		for (int i = 0; i < datas.Count; i++)
		{
			if (datas[i].GUID != 0uL)
			{
				this.mFriendRecommendTable.AddData(new FriendDataEx(datas[i], EUITableLayers.ESL_FriendRecommend, delegate(FriendDataEx friend)
				{
					this.AddFriendItem(friend.FriendData);
				}, null));
			}
		}
		this.RefreshLayer();
	}

	private void CreateObjects()
	{
		this.changeBtn = GameUITools.RegisterClickEvent("changeBtn", new UIEventListener.VoidDelegate(this.OnChangeBtnClick), base.gameObject);
		this.BtnRefresh = base.transform.FindChild("refresh").gameObject;
		this.BtnRefreshText = this.BtnRefresh.transform.FindChild("Label").GetComponent<UILabel>();
		this.changeBtn.SetActive(true);
		this.BtnRefresh.gameObject.SetActive(false);
		this.mFriendRecommendTable = base.transform.FindChild("friendRecommendPanel/friendContents").gameObject.AddComponent<FriendCommonGrid>();
		this.mFriendRecommendTable.maxPerLine = 2;
		this.mFriendRecommendTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mFriendRecommendTable.cellWidth = 450f;
		this.mFriendRecommendTable.cellHeight = 120f;
		this.mFriendRecommendTable.gapHeight = 2f;
		this.mFriendRecommendTable.gapWidth = 2f;
		this.mFriendRecommendTable.focusID = GameUIManager.mInstance.uiState.SelectFriendID;
	}

	private void OnChangeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.SendRecommendFriendRequest();
	}

	private void SendRecommendFriendRequest()
	{
		this.changeBtn.SetActive(false);
		this.BtnRefresh.gameObject.SetActive(true);
		this.BtnRefreshTextTimer = Time.realtimeSinceStartup + 10f;
		FriendRecommendLayer.RecommendTimestamp = Time.realtimeSinceStartup;
		MC2S_RecommendFriend ojb = new MC2S_RecommendFriend();
		Globals.Instance.CliSession.Send(307, ojb);
	}

	private void FixedUpdate()
	{
		if (!this.changeBtn.activeInHierarchy)
		{
			float num = this.BtnRefreshTextTimer - Time.realtimeSinceStartup;
			if (num < 0f)
			{
				this.changeBtn.SetActive(true);
				this.BtnRefresh.gameObject.SetActive(false);
			}
			else
			{
				this.BtnRefreshText.text = string.Format("{0}({1})", Singleton<StringManager>.Instance.GetString("Pillage17"), (int)(num + 1f));
			}
		}
	}

	public static void ClearRecommendFriends()
	{
		if (FriendRecommendLayer.RecommendFriends != null)
		{
			FriendRecommendLayer.RecommendFriends.Clear();
		}
		FriendRecommendLayer.RecommendTimestamp = 0f;
	}
}
