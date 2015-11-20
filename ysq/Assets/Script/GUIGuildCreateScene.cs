using System;
using UnityEngine;

public class GUIGuildCreateScene : GameUISession
{
	private UIToggle mTab0;

	private UIToggle mTab1;

	private UIToggle mTab2;

	private GuildJoinTabLayer mGuildJoinTabLayer;

	private GuildCreateTabLayer mGuildCreateTabLayer;

	private GuildSearchTabLayer mGuildSearchTabLayer;

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle/WindowBg");
		this.mGuildJoinTabLayer = transform.transform.Find("Page0").gameObject.AddComponent<GuildJoinTabLayer>();
		this.mGuildJoinTabLayer.InitWithBaseScene();
		this.mGuildCreateTabLayer = transform.transform.Find("Page1").gameObject.AddComponent<GuildCreateTabLayer>();
		this.mGuildCreateTabLayer.InitWithBaseScene();
		this.mGuildSearchTabLayer = transform.transform.Find("Page2").gameObject.AddComponent<GuildSearchTabLayer>();
		this.mGuildSearchTabLayer.InitWithBaseScene();
		this.mTab0 = transform.Find("tab0").GetComponent<UIToggle>();
		EventDelegate.Add(this.mTab0.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		this.mTab1 = transform.Find("tab1").GetComponent<UIToggle>();
		EventDelegate.Add(this.mTab1.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		this.mTab2 = transform.Find("tab2").GetComponent<UIToggle>();
		EventDelegate.Add(this.mTab2.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		this.mTab0.value = true;
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("guild0");
		GuildSubSystem expr_2B = Globals.Instance.Player.GuildSystem;
		expr_2B.GuildListUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_2B.GuildListUpdateEvent, new GuildSubSystem.VoidCallback(this.OnGuildListUpdateEvent));
		GuildSubSystem expr_5B = Globals.Instance.Player.GuildSystem;
		expr_5B.GuildSearchListUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_5B.GuildSearchListUpdateEvent, new GuildSubSystem.VoidCallback(this.OnGuildSearchListUpdateEvent));
		GuildSubSystem expr_8B = Globals.Instance.Player.GuildSystem;
		expr_8B.GuildApplyEvent = (GuildSubSystem.GuildApplyCallback)Delegate.Combine(expr_8B.GuildApplyEvent, new GuildSubSystem.GuildApplyCallback(this.OnGuildApplyEvent));
		GuildSubSystem expr_BB = Globals.Instance.Player.GuildSystem;
		expr_BB.GuildInitDataEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_BB.GuildInitDataEvent, new GuildSubSystem.VoidCallback(this.OnGuildInitDataEvent));
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Hide();
		GuildSubSystem expr_1E = Globals.Instance.Player.GuildSystem;
		expr_1E.GuildListUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_1E.GuildListUpdateEvent, new GuildSubSystem.VoidCallback(this.OnGuildListUpdateEvent));
		GuildSubSystem expr_4E = Globals.Instance.Player.GuildSystem;
		expr_4E.GuildSearchListUpdateEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_4E.GuildSearchListUpdateEvent, new GuildSubSystem.VoidCallback(this.OnGuildSearchListUpdateEvent));
		GuildSubSystem expr_7E = Globals.Instance.Player.GuildSystem;
		expr_7E.GuildApplyEvent = (GuildSubSystem.GuildApplyCallback)Delegate.Remove(expr_7E.GuildApplyEvent, new GuildSubSystem.GuildApplyCallback(this.OnGuildApplyEvent));
		GuildSubSystem expr_AE = Globals.Instance.Player.GuildSystem;
		expr_AE.GuildInitDataEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_AE.GuildInitDataEvent, new GuildSubSystem.VoidCallback(this.OnGuildInitDataEvent));
	}

	private void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			if (!(UIToggle.current == this.mTab1))
			{
				if (UIToggle.current == this.mTab2)
				{
				}
			}
		}
	}

	private void OnGuildListUpdateEvent()
	{
		this.mGuildJoinTabLayer.DoRefreshJoinItems();
	}

	private void OnGuildSearchListUpdateEvent()
	{
		this.mGuildSearchTabLayer.DoRefreshJoinItems();
	}

	private void OnGuildApplyEvent(ulong applyerId)
	{
		this.mGuildJoinTabLayer.Refresh(applyerId);
		this.mGuildSearchTabLayer.Refresh(applyerId);
	}

	private void OnGuildInitDataEvent()
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			GameUIManager.mInstance.ChangeSession<GUIGuildManageScene>(null, false, true);
		}
	}
}
