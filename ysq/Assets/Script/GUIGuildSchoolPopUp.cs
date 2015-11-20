using Att;
using Proto;
using System;
using UnityEngine;

public class GUIGuildSchoolPopUp : GameUIBasePopup
{
	private GUIGuildSchoolItemTable mSchoolTable;

	private GuildSchoolItemTip mGuildSchoolItemTip;

	private void Awake()
	{
		this.CreateObjects();
		this.InitSchoolItems();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBg");
		GameObject gameObject = transform.Find("closeBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		this.mSchoolTable = transform.Find("contentsBg/contentsPanel/contents").gameObject.AddComponent<GUIGuildSchoolItemTable>();
		this.mSchoolTable.maxPerLine = 1;
		this.mSchoolTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mSchoolTable.cellWidth = 880f;
		this.mSchoolTable.cellHeight = 120f;
		this.mSchoolTable.InitWithBaseScene(this);
		this.mGuildSchoolItemTip = transform.Find("itemTip").gameObject.AddComponent<GuildSchoolItemTip>();
		this.mGuildSchoolItemTip.InitWithBaseScene();
		this.mGuildSchoolItemTip.gameObject.SetActive(false);
		GameObject gameObject2 = transform.Find("ruleBtn").gameObject;
		UIEventListener expr_F6 = UIEventListener.Get(gameObject2);
		expr_F6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_F6.onClick, new UIEventListener.VoidDelegate(this.OnRuleClick));
		GameObject gameObject3 = transform.Find("recordBtn").gameObject;
		UIEventListener expr_12E = UIEventListener.Get(gameObject3);
		expr_12E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_12E.onClick, new UIEventListener.VoidDelegate(this.OnRecordBtnClick));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	public void UpdateTable()
	{
		this.mSchoolTable.repositionNow = true;
	}

	private ulong GetOpenedGuildBossId()
	{
		GuildData guild = Globals.Instance.Player.GuildSystem.Guild;
		if (guild != null)
		{
			for (int i = 1; i <= 10; i++)
			{
				GuildBossData guildBossData = Globals.Instance.Player.GuildSystem.GetGuildBossData(i);
				if (guildBossData != null && i == guild.AttackAcademyID1)
				{
					return (ulong)((long)i);
				}
			}
		}
		return 1uL;
	}

	public void InitSchoolItems()
	{
		this.mSchoolTable.ClearData();
		this.mSchoolTable.focusID = this.GetOpenedGuildBossId();
		for (int i = 1; i <= 10; i++)
		{
			GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(i);
			if (info != null && !string.IsNullOrEmpty(info.Academy))
			{
				this.mSchoolTable.AddData(new GUIGuildSchoolItemData(i, info));
			}
		}
	}

	private void OnRuleClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUIRuleInfoPopUp, false, null, null);
		GameUIRuleInfoPopUp gameUIRuleInfoPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUIRuleInfoPopUp;
		gameUIRuleInfoPopUp.Refresh(Singleton<StringManager>.Instance.GetString("guildSchool50"), Singleton<StringManager>.Instance.GetString("guildSchool51"));
	}

	private void OnRecordBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIGuildSchoolTargetPopUp.ShowMe();
	}

	public void ShowSchoolItemTip(int schoolId)
	{
		this.mGuildSchoolItemTip.ShowSchoolTip(schoolId);
	}
}
