using Proto;
using System;
using UnityEngine;

public class GUIGuildCraftYZHalfPop : GameUIBasePopup
{
	private GUIGuildCraftYZTeam mGUIGuildCraftYZTeam1;

	private GUIGuildCraftYZTeam mGUIGuildCraftYZTeam2;

	public static void ShowMe()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildCraftYZHalfPop, false, null, null);
	}

	private void Awake()
	{
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBG");
		GameObject gameObject = transform.Find("closeBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		this.mGUIGuildCraftYZTeam1 = transform.Find("one").gameObject.AddComponent<GUIGuildCraftYZTeam>();
		this.mGUIGuildCraftYZTeam1.InitWithBaseScene(EGuildWarId.EGWI_FinalFour1);
		this.mGUIGuildCraftYZTeam2 = transform.Find("two").gameObject.AddComponent<GUIGuildCraftYZTeam>();
		this.mGUIGuildCraftYZTeam2.InitWithBaseScene(EGuildWarId.EGWI_FinalFour2);
		GuildSubSystem expr_A6 = Globals.Instance.Player.GuildSystem;
		expr_A6.GuildWarSupportEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_A6.GuildWarSupportEvent, new GuildSubSystem.VoidCallback(this.OnWarSupportEvent));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		GuildSubSystem expr_20 = Globals.Instance.Player.GuildSystem;
		expr_20.GuildWarSupportEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_20.GuildWarSupportEvent, new GuildSubSystem.VoidCallback(this.OnWarSupportEvent));
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnWarSupportEvent()
	{
		this.Refresh();
	}

	private void Refresh()
	{
		if (Globals.Instance.Player.GuildSystem.BattleSupportInfo == null)
		{
			return;
		}
		this.mGUIGuildCraftYZTeam1.Refresh();
		this.mGUIGuildCraftYZTeam2.Refresh();
	}
}
