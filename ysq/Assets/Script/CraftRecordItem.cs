using Proto;
using System;
using System.Text;
using UnityEngine;

public sealed class CraftRecordItem : UICustomGridItem
{
	private UISprite mIsWinSp;

	private UILabel mIsWinLb;

	private UILabel mLvlName;

	private UILabel mTimeNum;

	private UISprite mHeadIcon;

	private UISprite mHeadIconFrame;

	private GameObject viewBtn;

	private StringBuilder mSB = new StringBuilder(42);

	public CraftRecordItemData mRecordData
	{
		get;
		private set;
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mIsWinSp = base.transform.Find("flagBg").GetComponent<UISprite>();
		this.mIsWinLb = this.mIsWinSp.transform.Find("txt").GetComponent<UILabel>();
		this.mLvlName = base.transform.Find("lvlName").GetComponent<UILabel>();
		this.mTimeNum = base.transform.Find("timeNum").GetComponent<UILabel>();
		this.mHeadIcon = base.transform.Find("headIcon").GetComponent<UISprite>();
		this.mHeadIconFrame = GameUITools.FindUISprite("Frame", this.mHeadIcon.gameObject);
		this.viewBtn = base.transform.Find("view").gameObject;
		UIEventListener expr_CD = UIEventListener.Get(this.viewBtn);
		expr_CD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_CD.onClick, new UIEventListener.VoidDelegate(this.OnViewBtnClick));
	}

	public override void Refresh(object data)
	{
		if (this.mRecordData == data)
		{
			return;
		}
		this.mRecordData = (CraftRecordItemData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mRecordData == null)
		{
			return;
		}
		if (this.mRecordData.RecordData == null)
		{
			return;
		}
		if (this.mRecordData.RecordData.Attack)
		{
			if (this.mRecordData.RecordData.Win)
			{
				this.mIsWinSp.spriteName = "Flag_G";
				this.mIsWinLb.text = Singleton<StringManager>.Instance.GetString("guildCraft44");
			}
			else
			{
				this.mIsWinSp.spriteName = "Flag_R";
				this.mIsWinLb.text = Singleton<StringManager>.Instance.GetString("guildCraft45");
			}
		}
		else if (this.mRecordData.RecordData.Win)
		{
			this.mIsWinSp.spriteName = "Flag_R";
			this.mIsWinLb.text = Singleton<StringManager>.Instance.GetString("guildCraft43");
		}
		else
		{
			this.mIsWinSp.spriteName = "Flag_G";
			this.mIsWinLb.text = Singleton<StringManager>.Instance.GetString("guildCraft42");
		}
		this.mLvlName.text = this.mSB.Remove(0, this.mSB.Length).Append("Lv").Append(this.mRecordData.RecordData.Level).Append(" ").Append(this.mRecordData.RecordData.Name).ToString();
		this.mTimeNum.text = GameUITools.FormatPvpRecordTime(Globals.Instance.Player.GetTimeStamp() - this.mRecordData.RecordData.TimeStamp);
		this.mHeadIcon.spriteName = Tools.GetPlayerIcon(this.mRecordData.RecordData.FashionID);
		this.mHeadIconFrame.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(this.mRecordData.RecordData.ConstellationLevel));
	}

	private void OnViewBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		if (this.mRecordData == null || this.mRecordData.RecordData == null)
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (player.Data.ID == this.mRecordData.RecordData.PlayerID)
		{
			return;
		}
		MC2S_QueryRemotePlayer mC2S_QueryRemotePlayer = new MC2S_QueryRemotePlayer();
		mC2S_QueryRemotePlayer.PlayerID = this.mRecordData.RecordData.PlayerID;
		Globals.Instance.CliSession.Send(286, mC2S_QueryRemotePlayer);
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
