using Proto;
using System;
using System.Text;
using UnityEngine;

public sealed class PVPRecordItemPVP4 : UICustomGridItem
{
	private UISprite mIsWinSp;

	private UISprite mPvp4Info;

	private UILabel mIsWinLb;

	private UILabel mPvp4Rank;

	private UILabel mLvlName;

	private UILabel mTimeNum;

	private UISprite mHeadIcon;

	private UISprite mHeadIconFrame;

	private GameObject viewBtn;

	private StringBuilder mStringBuilder = new StringBuilder();

	public PVPRecordItemData mRecordData
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
		this.mPvp4Info = base.transform.Find("arrow").GetComponent<UISprite>();
		this.mPvp4Rank = this.mPvp4Info.transform.Find("num").GetComponent<UILabel>();
		this.mLvlName = base.transform.Find("lvlName").GetComponent<UILabel>();
		this.mTimeNum = base.transform.Find("timeNum").GetComponent<UILabel>();
		this.mHeadIcon = base.transform.Find("headIcon").GetComponent<UISprite>();
		this.mHeadIconFrame = GameUITools.FindUISprite("Frame", this.mHeadIcon.gameObject);
		GameObject gameObject = base.transform.Find("replayBtn").gameObject;
		UIEventListener expr_FE = UIEventListener.Get(gameObject);
		expr_FE.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_FE.onClick, new UIEventListener.VoidDelegate(this.OnReplayBtnClick));
		gameObject.SetActive(false);
		this.viewBtn = base.transform.Find("view").gameObject;
		UIEventListener expr_14C = UIEventListener.Get(this.viewBtn);
		expr_14C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_14C.onClick, new UIEventListener.VoidDelegate(this.OnViewBtnClick));
	}

	public override void Refresh(object data)
	{
		if (this.mRecordData == data)
		{
			return;
		}
		this.mRecordData = (PVPRecordItemData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mRecordData == null)
		{
			return;
		}
		if (this.mRecordData.RecordData.Win)
		{
			this.mIsWinSp.spriteName = "Flag_G";
			this.mIsWinLb.text = Singleton<StringManager>.Instance.GetString("pvpTxt4");
		}
		else
		{
			this.mIsWinSp.spriteName = "Flag_R";
			this.mIsWinLb.text = Singleton<StringManager>.Instance.GetString("pvpTxt5");
		}
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append("Lv").Append(this.mRecordData.RecordData.Level).Append(" ").Append(this.mRecordData.RecordData.Name);
		this.mLvlName.text = this.mStringBuilder.ToString();
		this.mTimeNum.text = GameUITools.FormatPvpRecordTime(Globals.Instance.Player.GetTimeStamp() - this.mRecordData.RecordData.TimeStamp);
		this.mHeadIcon.spriteName = Tools.GetPlayerIcon(this.mRecordData.RecordData.FashionID);
		this.mHeadIconFrame.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(this.mRecordData.RecordData.ConstellationLevel));
		if (this.mRecordData.RecordData.ValueUpdate == 0)
		{
			this.mPvp4Info.gameObject.SetActive(false);
		}
		else
		{
			this.mPvp4Info.gameObject.SetActive(true);
			this.mPvp4Info.spriteName = ((!this.mRecordData.RecordData.Win) ? "Arrow_R" : "Arrow_G");
			this.mPvp4Rank.color = ((!this.mRecordData.RecordData.Win) ? Color.red : Color.green);
			this.mPvp4Rank.text = this.mRecordData.RecordData.ValueUpdate.ToString();
		}
	}

	private void OnReplayBtnClick(GameObject go)
	{
		GameUIManager.mInstance.ShowMessageTipByKey("notRealizeTxt", 0f, 0f);
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
