using System;
using UnityEngine;

public class GUIGuildMinesRecordItem : UICustomGridItem
{
	private UILabel mContent;

	private UILabel mMines;

	private UILabel mTime;

	private GUIGuildMinesRecordData mData;

	private GUIGuildMinesRecordPopUp mBaseScene;

	public void Init(GUIGuildMinesRecordPopUp basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mContent = GameUITools.FindUILabel("Content", base.gameObject);
		this.mMines = GameUITools.FindUILabel("Mines", base.gameObject);
		this.mTime = GameUITools.FindUILabel("Time", base.gameObject);
		GameUITools.RegisterClickEvent("Go", new UIEventListener.VoidDelegate(this.OnGoClick), base.gameObject);
	}

	private void OnGoClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!Globals.Instance.Player.GuildSystem.IsGuildMinesOpen())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("ActivityR_1", 0f, 0f);
			return;
		}
		this.mBaseScene.OnOrePillageClick(this.mData);
	}

	public override void Refresh(object data)
	{
		if (this.mData == data)
		{
			return;
		}
		this.mData = (GUIGuildMinesRecordData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mData == null)
		{
			return;
		}
		this.mContent.text = Singleton<StringManager>.Instance.GetString("guildMines19", new object[]
		{
			Singleton<StringManager>.Instance.GetString("guildMines18", new object[]
			{
				this.mData.mRecord.Amount
			}),
			this.mData.mRecord.Name
		});
		this.mMines.text = Singleton<StringManager>.Instance.GetString("guildMines17", new object[]
		{
			this.mData.mRecord.PillageCount
		});
		int num = Globals.Instance.Player.GetTimeStamp() - this.mData.mRecord.Timestamp;
		if (num <= 0)
		{
			num = 1;
		}
		this.mTime.text = GameUITools.FormatPvpRecordTime(num);
	}
}
