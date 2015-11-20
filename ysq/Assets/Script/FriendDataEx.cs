using Proto;
using System;

public class FriendDataEx : BaseData
{
	public Action<FriendDataEx> FriendDataEvent1;

	public Action<FriendDataEx> FriendDataEvent2;

	public EUITableLayers FriendType
	{
		get;
		private set;
	}

	public FriendData FriendData
	{
		get;
		private set;
	}

	public FriendDataEx(FriendData fdata, EUITableLayers type, Action<FriendDataEx> event1 = null, Action<FriendDataEx> event2 = null)
	{
		this.FriendData = fdata;
		this.FriendType = type;
		this.FriendDataEvent1 = event1;
		this.FriendDataEvent2 = event2;
	}

	public override ulong GetID()
	{
		return this.FriendData.GUID;
	}

	public void DoAddFriend()
	{
		if (this.FriendType != EUITableLayers.ESL_FriendRecommend)
		{
			return;
		}
		if (this.FriendData.GUID == Globals.Instance.Player.Data.ID)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("friend_30", 0f, 0f);
			return;
		}
		MC2S_RequestFriend mC2S_RequestFriend = new MC2S_RequestFriend();
		mC2S_RequestFriend.GUID = this.FriendData.GUID;
		Globals.Instance.CliSession.Send(309, mC2S_RequestFriend);
		if (this.FriendDataEvent1 != null)
		{
			this.FriendDataEvent1(this);
		}
	}

	public void DoSendApplyRequest(bool agree)
	{
		if (this.FriendType != EUITableLayers.ESL_FriendRequest)
		{
			return;
		}
		if (agree)
		{
			MC2S_ReplyFriend mC2S_ReplyFriend = new MC2S_ReplyFriend();
			mC2S_ReplyFriend.GUID = this.FriendData.GUID;
			mC2S_ReplyFriend.Agree = true;
			Globals.Instance.CliSession.Send(311, mC2S_ReplyFriend);
			if (this.FriendDataEvent1 != null)
			{
				this.FriendDataEvent1(this);
			}
		}
		else
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("friend_31", new object[]
			{
				this.FriendData.Name
			}), MessageBox.Type.OKCancel, agree);
			GameMessageBox expr_94 = gameMessageBox;
			expr_94.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_94.OkClick, new MessageBox.MessageDelegate(delegate(object obj)
			{
				MC2S_ReplyFriend mC2S_ReplyFriend2 = new MC2S_ReplyFriend();
				mC2S_ReplyFriend2.GUID = this.FriendData.GUID;
				mC2S_ReplyFriend2.Agree = false;
				Globals.Instance.CliSession.Send(311, mC2S_ReplyFriend2);
			}));
			if (this.FriendDataEvent2 != null)
			{
				this.FriendDataEvent2(this);
			}
		}
	}
}
