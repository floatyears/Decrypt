using Proto;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GUIFriendScene : GameUISession
{
	private FriendLayer mFriendLayer;

	private FriendRecommendLayer mFriendRecommendLayer;

	private FriendBlackListLayer mFriendBlackListLayer;

	private FriendRequestLayer mFriendRequestLayer;

	private UIToggle[] mPlayerInfoToggle = new UIToggle[4];

	public EUITableLayers currentTable = EUITableLayers.ESL_MAX;

	private GameObject mTab0Mark;

	private GameObject mTab3Mark;

	private static EUITableLayers lastTable = EUITableLayers.ESL_MAX;

	public static void TryOpen(EUITableLayers type)
	{
		GUIFriendScene gUIFriendScene = GameUIManager.mInstance.CurUISession as GUIFriendScene;
		if (gUIFriendScene == null)
		{
			if (type == EUITableLayers.ESL_MAX)
			{
				if (Globals.Instance.Player.FriendSystem.applyList.Count > 0)
				{
					type = EUITableLayers.ESL_FriendRequest;
				}
				else if (Globals.Instance.Player.FriendSystem.friends.Count == 0)
				{
					type = EUITableLayers.ESL_FriendRecommend;
				}
				else
				{
					type = EUITableLayers.ESL_Friend;
				}
			}
			GameUIManager.mInstance.ChangeSession<GUIFriendScene>(delegate(GUIFriendScene friend)
			{
				friend.SelectTab(type);
			}, false, true);
		}
		else
		{
			gUIFriendScene.SelectTab(type);
		}
	}

	public void SelectTab(EUITableLayers layer)
	{
		if (layer >= (EUITableLayers)this.mPlayerInfoToggle.Length)
		{
			return;
		}
		this.mPlayerInfoToggle[(int)layer].value = true;
		this.currentTable = layer;
	}

	protected override void OnPostLoadGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("friendLb");
		this.CreateObjects();
		FriendSubSystem expr_2B = Globals.Instance.Player.FriendSystem;
		expr_2B.AddFriendDataEvent = (FriendSubSystem.FriendDataCallback)Delegate.Combine(expr_2B.AddFriendDataEvent, new FriendSubSystem.FriendDataCallback(this.OnAddFriendDataEvent));
		FriendSubSystem expr_5B = Globals.Instance.Player.FriendSystem;
		expr_5B.RemoveFriendEvent = (FriendSubSystem.FriendIDCallback)Delegate.Combine(expr_5B.RemoveFriendEvent, new FriendSubSystem.FriendIDCallback(this.OnRemoveFriendEvent));
		FriendSubSystem expr_8B = Globals.Instance.Player.FriendSystem;
		expr_8B.UpdateFriendEvent = (FriendSubSystem.FriendIDCallback)Delegate.Combine(expr_8B.UpdateFriendEvent, new FriendSubSystem.FriendIDCallback(this.OnUpdateFriendEvent));
		LocalPlayer expr_B6 = Globals.Instance.Player;
		expr_B6.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_B6.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		Globals.Instance.CliSession.Register(308, new ClientSession.MsgHandler(this.OnMsgRecommendFriend));
	}

	protected override void OnPreDestroyGUI()
	{
		GUIFriendScene.lastTable = this.currentTable;
		GameUIManager.mInstance.GetTopGoods().Hide();
		FriendSubSystem expr_29 = Globals.Instance.Player.FriendSystem;
		expr_29.AddFriendDataEvent = (FriendSubSystem.FriendDataCallback)Delegate.Remove(expr_29.AddFriendDataEvent, new FriendSubSystem.FriendDataCallback(this.OnAddFriendDataEvent));
		FriendSubSystem expr_59 = Globals.Instance.Player.FriendSystem;
		expr_59.RemoveFriendEvent = (FriendSubSystem.FriendIDCallback)Delegate.Remove(expr_59.RemoveFriendEvent, new FriendSubSystem.FriendIDCallback(this.OnRemoveFriendEvent));
		FriendSubSystem expr_89 = Globals.Instance.Player.FriendSystem;
		expr_89.UpdateFriendEvent = (FriendSubSystem.FriendIDCallback)Delegate.Remove(expr_89.UpdateFriendEvent, new FriendSubSystem.FriendIDCallback(this.OnUpdateFriendEvent));
		LocalPlayer expr_B4 = Globals.Instance.Player;
		expr_B4.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_B4.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		Globals.Instance.CliSession.Unregister(308, new ClientSession.MsgHandler(this.OnMsgRecommendFriend));
	}

	protected override void OnLoadedFinished()
	{
		base.OnLoadedFinished();
		if (this.currentTable == EUITableLayers.ESL_MAX)
		{
			if (GUIFriendScene.lastTable == EUITableLayers.ESL_MAX)
			{
				if (Globals.Instance.Player.FriendSystem.applyList.Count > 0)
				{
					GUIFriendScene.lastTable = EUITableLayers.ESL_FriendRequest;
				}
				else if (Globals.Instance.Player.FriendSystem.friends.Count == 0)
				{
					GUIFriendScene.lastTable = EUITableLayers.ESL_FriendRecommend;
				}
				else
				{
					GUIFriendScene.lastTable = EUITableLayers.ESL_Friend;
				}
			}
			this.SelectTab(GUIFriendScene.lastTable);
		}
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("bg").gameObject;
		this.mFriendLayer = gameObject.transform.Find("page0").gameObject.AddComponent<FriendLayer>();
		this.mFriendLayer.Init();
		this.mFriendRecommendLayer = gameObject.transform.Find("page1").gameObject.AddComponent<FriendRecommendLayer>();
		this.mFriendRecommendLayer.Init();
		this.mFriendBlackListLayer = gameObject.transform.Find("page2").gameObject.AddComponent<FriendBlackListLayer>();
		this.mFriendBlackListLayer.Init();
		this.mFriendRequestLayer = gameObject.transform.Find("page3").gameObject.AddComponent<FriendRequestLayer>();
		this.mFriendRequestLayer.Init();
		for (int i = 0; i < 4; i++)
		{
			this.mPlayerInfoToggle[i] = base.RegisterClickEvent(string.Format("tab{0}", i), new UIEventListener.VoidDelegate(this.OnTabClick), gameObject).GetComponent<UIToggle>();
			EventDelegate.Add(this.mPlayerInfoToggle[i].onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		}
		this.mTab0Mark = base.FindGameObject("new", this.mPlayerInfoToggle[0].gameObject);
		this.mTab3Mark = base.FindGameObject("new", this.mPlayerInfoToggle[3].gameObject);
		this.RefreshTabMark0();
		this.RefreshTabMark3();
	}

	private void RefreshTabMark0()
	{
		this.mTab0Mark.SetActive(this.mFriendLayer != null && Globals.Instance.Player.FriendSystem.friends.Count > 0 && (this.mFriendLayer.CanAllEnergyGet() || Globals.Instance.Player.FriendSystem.PendingGiveFriendEnergy > 0));
	}

	private void RefreshTabMark3()
	{
		this.mTab3Mark.SetActive(this.mFriendRequestLayer != null && Globals.Instance.Player.FriendSystem.applyList.Count > 0);
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		for (int i = 0; i < 4; i++)
		{
			if (this.mPlayerInfoToggle[i].gameObject == go)
			{
				this.currentTable = (EUITableLayers)i;
				break;
			}
		}
	}

	private void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			if (UIToggle.current == this.mPlayerInfoToggle[0])
			{
				this.mFriendLayer.RefreshLayer();
			}
			else if (UIToggle.current == this.mPlayerInfoToggle[1])
			{
				this.mFriendRecommendLayer.OnOpenTable();
			}
			else if (UIToggle.current == this.mPlayerInfoToggle[2])
			{
				this.mFriendBlackListLayer.RefreshLayer();
			}
			else if (UIToggle.current == this.mPlayerInfoToggle[3])
			{
				this.mFriendRequestLayer.RefreshLayer();
			}
		}
	}

	private void OnPlayerUpdateEvent()
	{
		this.mFriendLayer.RefreshEngeryCount();
	}

	private void OnUpdateFriendEvent(ulong id)
	{
		if (id == 0uL)
		{
			if (this.currentTable == EUITableLayers.ESL_Friend)
			{
				this.mFriendLayer.RefreshLayer();
				this.RefreshTabMark0();
			}
			else if (this.currentTable == EUITableLayers.ESL_BlackList)
			{
				this.mFriendBlackListLayer.RefreshLayer();
			}
			else if (this.currentTable == EUITableLayers.ESL_FriendRequest)
			{
				this.mFriendRequestLayer.RefreshLayer();
				this.RefreshTabMark3();
			}
		}
		else if (this.currentTable == EUITableLayers.ESL_Friend)
		{
			this.mFriendLayer.UpdateFriendItem(id);
			this.RefreshTabMark0();
		}
		else if (this.currentTable == EUITableLayers.ESL_BlackList)
		{
			this.mFriendBlackListLayer.UpdateFriendItem(id);
		}
		else if (this.currentTable == EUITableLayers.ESL_FriendRequest)
		{
			this.mFriendRequestLayer.UpdateFriendItem(id);
			this.RefreshTabMark3();
		}
	}

	private void OnRemoveFriendEvent(ulong id)
	{
		this.mFriendLayer.RemoveFriendItem(id);
		this.RefreshTabMark0();
		this.mFriendBlackListLayer.RemoveFriendItem(id);
		this.mFriendRequestLayer.RemoveFriendItem(id);
		this.RefreshTabMark3();
	}

	private void OnAddFriendDataEvent(FriendData data)
	{
		if (data.FriendType == 1)
		{
			this.mFriendLayer.AddFriendItem(data);
			this.RefreshTabMark0();
		}
		if (data.FriendType == 2)
		{
			this.mFriendBlackListLayer.AddFriendItem(data);
		}
		if (data.FriendType == 3)
		{
			this.mFriendRequestLayer.AddFriendItem(data);
			this.RefreshTabMark3();
		}
		this.mFriendRecommendLayer.AddFriendItem(data);
	}

	public void OnMsgRecommendFriend(MemoryStream stream)
	{
		if (this.mFriendRecommendLayer != null)
		{
			MS2C_RecommendFriend mS2C_RecommendFriend = Serializer.NonGeneric.Deserialize(typeof(MS2C_RecommendFriend), stream) as MS2C_RecommendFriend;
			this.mFriendRecommendLayer.RefreshRecommendFriend(mS2C_RecommendFriend.Data);
		}
	}
}
