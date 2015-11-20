using Proto;
using System;
using UnityEngine;

public class GuildActivityItem : UICustomGridItem
{
	private GuildActivityItemData mGuildActivityItemData;

	private UISprite mIcon;

	private UILabel mDesc;

	private GameObject mTipMark;

	private bool mTipMarkIsShow;

	public bool TipMarkIsShow
	{
		get
		{
			return this.mTipMarkIsShow;
		}
		private set
		{
			this.mTipMarkIsShow = value;
			this.mTipMark.SetActive(value);
		}
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
		this.TipMarkIsShow = false;
	}

	private void CreateObjects()
	{
		this.mIcon = base.transform.GetComponent<UISprite>();
		UIEventListener expr_1C = UIEventListener.Get(base.gameObject);
		expr_1C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C.onClick, new UIEventListener.VoidDelegate(this.OnIconClick));
		this.mDesc = base.transform.Find("desc").GetComponent<UILabel>();
		this.mDesc.text = string.Empty;
		this.mTipMark = base.transform.Find("new").gameObject;
	}

	public override void Refresh(object data)
	{
		if (this.mGuildActivityItemData == data)
		{
			return;
		}
		this.mGuildActivityItemData = (GuildActivityItemData)data;
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.mGuildActivityItemData != null)
		{
			switch (this.mGuildActivityItemData.mActivityType)
			{
			case GuildActivityItemData.EGAIType.EGAIGuildShop:
				this.mIcon.spriteName = "guildShop";
				this.mDesc.text = Singleton<StringManager>.Instance.GetString("guildActive0");
				this.TipMarkIsShow = false;
				break;
			case GuildActivityItemData.EGAIType.EGAIGuildMagic:
				this.mIcon.spriteName = "magicTag";
				this.mDesc.text = Singleton<StringManager>.Instance.GetString("guildActive1");
				this.TipMarkIsShow = this.mGuildActivityItemData.TipMarkIsShow;
				break;
			case GuildActivityItemData.EGAIType.EGAIGuildSchool:
				this.mIcon.spriteName = "guildSchool";
				this.mDesc.text = Singleton<StringManager>.Instance.GetString("guildActive2");
				this.TipMarkIsShow = this.mGuildActivityItemData.TipMarkIsShow;
				break;
			case GuildActivityItemData.EGAIType.EGAIGuildKuangShi:
				this.mIcon.spriteName = "kuangShi";
				this.mDesc.text = Singleton<StringManager>.Instance.GetString("guildActive3");
				this.TipMarkIsShow = this.mGuildActivityItemData.TipMarkIsShow;
				break;
			case GuildActivityItemData.EGAIType.EGAIGuildCraft:
				this.mIcon.spriteName = "guildCraft";
				this.mDesc.text = Singleton<StringManager>.Instance.GetString("guildActive4");
				this.TipMarkIsShow = this.mGuildActivityItemData.TipMarkIsShow;
				break;
			}
		}
	}

	private bool IsCanGuildSign(bool showMsg = false)
	{
		bool flag = false;
		for (int i = 0; i < Globals.Instance.Player.GuildSystem.Members.Count; i++)
		{
			GuildMember guildMember = Globals.Instance.Player.GuildSystem.Members[i];
			if (guildMember.ID == Globals.Instance.Player.Data.ID)
			{
				flag = ((guildMember.Flag & 4) == 0);
			}
		}
		if (!flag && showMsg)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guild15", 0f, 0f);
		}
		return flag;
	}

	public static void DoSendGuildBossRequest()
	{
		MC2S_GetGuildBoss ojb = new MC2S_GetGuildBoss();
		Globals.Instance.CliSession.Send(934, ojb);
	}

	private void OnIconClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mGuildActivityItemData != null)
		{
			switch (this.mGuildActivityItemData.mActivityType)
			{
			case GuildActivityItemData.EGAIType.EGAIGuildShop:
				GUIShopScene.TryOpen(EShopType.EShop_Guild);
				break;
			case GuildActivityItemData.EGAIType.EGAIGuildMagic:
				GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildMagicPopUp, false, null, null);
				break;
			case GuildActivityItemData.EGAIType.EGAIGuildSchool:
				GuildActivityItem.DoSendGuildBossRequest();
				break;
			case GuildActivityItemData.EGAIType.EGAIGuildKuangShi:
				GUIGuildMinesScene.Show(false);
				break;
			case GuildActivityItemData.EGAIType.EGAIGuildCraft:
				Globals.Instance.Player.GuildSystem.RequestQueryWarState();
				break;
			}
		}
	}
}
