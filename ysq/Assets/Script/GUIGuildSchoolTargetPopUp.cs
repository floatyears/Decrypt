using Att;
using Proto;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GUIGuildSchoolTargetPopUp : GameUIBasePopup
{
	private GUIGSTargetItemTable mGUIGSTargetItemTable;

	public static void ShowMe()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildSchoolTargetPopUp, false, null, null);
	}

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
		this.mGUIGSTargetItemTable = transform.Find("contentsBg/contentsPanel/contents").gameObject.AddComponent<GUIGSTargetItemTable>();
		this.mGUIGSTargetItemTable.maxPerLine = 1;
		this.mGUIGSTargetItemTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mGUIGSTargetItemTable.cellWidth = 616f;
		this.mGUIGSTargetItemTable.cellHeight = 96f;
		this.mGUIGSTargetItemTable.InitWithBaseScene(this);
		Globals.Instance.CliSession.Register(947, new ClientSession.MsgHandler(this.OnMsgSetAttackTarget));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		Globals.Instance.CliSession.Unregister(947, new ClientSession.MsgHandler(this.OnMsgSetAttackTarget));
	}

	private void DoClosePop()
	{
		int attackAcademyID = Globals.Instance.Player.GuildSystem.Guild.AttackAcademyID2;
		int curSelectedID = this.mGUIGSTargetItemTable.GetCurSelectedID();
		if (attackAcademyID != curSelectedID)
		{
			MC2S_SetAttackTarget mC2S_SetAttackTarget = new MC2S_SetAttackTarget();
			mC2S_SetAttackTarget.id = curSelectedID;
			Globals.Instance.CliSession.Send(946, mC2S_SetAttackTarget);
		}
		else
		{
			GameUIPopupManager.GetInstance().PopState(false, null);
		}
	}

	private void OnCloseClick(GameObject go)
	{
		this.DoClosePop();
	}

	public override void OnButtonBlockerClick()
	{
		this.DoClosePop();
	}

	private void InitSchoolItems()
	{
		this.mGUIGSTargetItemTable.ClearData();
		int attackAcademyID = Globals.Instance.Player.GuildSystem.Guild.AttackAcademyID2;
		for (int i = 1; i <= 10; i++)
		{
			GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(i);
			if (info != null && !string.IsNullOrEmpty(info.Academy))
			{
				this.mGUIGSTargetItemTable.AddData(new GUIGSTargetItemData(i, info, i == attackAcademyID));
			}
		}
	}

	public void SelectSchool(int id)
	{
		this.mGUIGSTargetItemTable.SetCurSelectID(id);
	}

	private void OnMsgSetAttackTarget(MemoryStream stream)
	{
		MS2C_SetAttackTarget mS2C_SetAttackTarget = Serializer.NonGeneric.Deserialize(typeof(MS2C_SetAttackTarget), stream) as MS2C_SetAttackTarget;
		if (mS2C_SetAttackTarget.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_SetAttackTarget.Result);
			return;
		}
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
