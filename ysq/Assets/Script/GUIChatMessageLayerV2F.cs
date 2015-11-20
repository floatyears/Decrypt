using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIChatMessageLayerV2F : MonoBehaviour
{
	private GUIChatMessageTableV2F mMsgChanelTable;

	private GUIChatWindowV2F mBaseLayer;

	public bool mNeedRefreshLine
	{
		get;
		set;
	}

	public void InitWorldChanel(GUIChatWindowV2F baseLayer)
	{
		this.mBaseLayer = baseLayer;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mMsgChanelTable = base.transform.Find("worldChatPanel/chatContents").gameObject.AddComponent<GUIChatMessageTableV2F>();
		this.mMsgChanelTable.maxPerLine = 1;
		this.mMsgChanelTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mMsgChanelTable.cellWidth = 890f;
		this.mMsgChanelTable.cellHeight = 110f;
		this.mMsgChanelTable.gapHeight = 5f;
		this.mMsgChanelTable.gapWidth = 0f;
		this.mMsgChanelTable.scrollBar = base.transform.Find("chatScrollBar").GetComponent<UIScrollBar>();
		this.mMsgChanelTable.bgScrollView = base.transform.parent.GetComponent<UIDragScrollView>();
		this.mMsgChanelTable.InitWithBaseScene(this.mBaseLayer);
	}

	public void ReInitChatDatas()
	{
		this.InitChatDatas();
	}

	private void InitChatDatas()
	{
		this.mMsgChanelTable.ClearData();
		switch (this.mBaseLayer.GetCurChanel())
		{
		case 1:
			foreach (ChatMessage current in Globals.Instance.Player.GuildMsgs)
			{
				this.DoAddChatMessage(current);
			}
			break;
		case 2:
			foreach (ChatMessage current2 in Globals.Instance.Player.WhisperMsgs)
			{
				this.DoAddChatMessage(current2);
			}
			break;
		case 3:
			foreach (ChatMessage current3 in Globals.Instance.Player.CostumePartyMsgs)
			{
				this.DoAddChatMessage(current3);
			}
			break;
		default:
			foreach (WorldMessageExtend current4 in Globals.Instance.Player.WorldMsgs)
			{
				if (current4 != null)
				{
					this.DoAddWorldMessage(current4, false);
				}
			}
			break;
		}
		this.mNeedRefreshLine = true;
		this.Refresh();
	}

	public void AddWorldMsg(WorldMessageExtend worldMsg, bool isSystem = false)
	{
		if (this.mBaseLayer.GetCurChanel() != 0)
		{
			return;
		}
		this.DoAddWorldMessage(worldMsg, isSystem);
	}

	public void AddGuildMsg(ChatMessage chatMsg)
	{
		if (this.mBaseLayer.GetCurChanel() != 1)
		{
			return;
		}
		this.DoAddChatMessage(chatMsg);
	}

	public void AddPersonalMsg(ChatMessage chatMsg)
	{
		if (this.mBaseLayer.GetCurChanel() != 2)
		{
			return;
		}
		this.DoAddChatMessage(chatMsg);
	}

	public void AddCostumePartyMsg(ChatMessage chatMsg)
	{
		if (this.mBaseLayer.GetCurChanel() != 3)
		{
			return;
		}
		this.DoAddChatMessage(chatMsg);
	}

	private void DoAddWorldCommonMessage(WorldMessageExtend msg)
	{
		this.mMsgChanelTable.AddData(new GUIChatMessageData(msg, null));
	}

	private void DoAddWorldCommonMessage1(WorldMessageExtend msg)
	{
		this.mMsgChanelTable.AddData(new GUIChatMessageData(msg, null));
	}

	private void DoAddWorldMessage(WorldMessageExtend worldMsg, bool issystem)
	{
		if (0.7f < this.mMsgChanelTable.scrollBar.value)
		{
			this.mNeedRefreshLine = true;
		}
		if (this.mMsgChanelTable.mDatas.Count >= 50)
		{
			int num = this.mMsgChanelTable.mDatas.Count - 50 + 1;
			for (int i = num - 1; i >= 0; i--)
			{
				this.mMsgChanelTable.mDatas.RemoveAt(i);
			}
		}
		if (worldMsg.mWM.SysEvent == null)
		{
			if (issystem)
			{
				this.DoAddWorldCommonMessage1(worldMsg);
			}
			else
			{
				this.DoAddWorldCommonMessage(worldMsg);
			}
		}
	}

	private void DoAddChatCommonMessage(ChatMessage msg)
	{
		this.mMsgChanelTable.AddData(new GUIChatMessageData(null, msg));
	}

	private void DoAddChatMessage(ChatMessage chatMsg)
	{
		if (this.mMsgChanelTable.scrollBar.value == 1f)
		{
			this.mNeedRefreshLine = true;
		}
		if (chatMsg.Channel == 3)
		{
			if (GameSetting.Data.ShieldPartyInvite && chatMsg.Type == 1u)
			{
				return;
			}
			if (GameSetting.Data.ShieldPartyInteraction && chatMsg.Type == 2u)
			{
				return;
			}
		}
		FriendData friendData = Globals.Instance.Player.FriendSystem.GetFriendData(chatMsg.PlayerID);
		bool flag = friendData != null && friendData.FriendType == 2;
		if (flag && chatMsg.PlayerID != Globals.Instance.Player.Data.ID)
		{
			return;
		}
		if (this.mMsgChanelTable.mDatas.Count >= 50)
		{
			int num = this.mMsgChanelTable.mDatas.Count - 50 + 1;
			for (int i = num - 1; i >= 0; i--)
			{
				this.mMsgChanelTable.mDatas.RemoveAt(i);
			}
		}
		this.DoAddChatCommonMessage(chatMsg);
	}

	[DebuggerHidden]
	private IEnumerator UpdateScrollBar()
	{
        return null;
        //GUIChatMessageLayerV2F.<UpdateScrollBar>c__Iterator36 <UpdateScrollBar>c__Iterator = new GUIChatMessageLayerV2F.<UpdateScrollBar>c__Iterator36();
        //<UpdateScrollBar>c__Iterator.<>f__this = this;
        //return <UpdateScrollBar>c__Iterator;
	}

	public void Refresh()
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.mMsgChanelTable.repositionNow = true;
			if (!this.mMsgChanelTable.scrollBar.gameObject.activeInHierarchy || this.mMsgChanelTable.scrollBar.alpha == 0f || this.mNeedRefreshLine)
			{
				this.mNeedRefreshLine = false;
				base.StartCoroutine(this.UpdateScrollBar());
			}
		}
	}
}
