using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIGuildMinesRewardDescPopUp : GameUIBasePopup
{
	private GUIGuildMinesRewardDescTable mContent;

	public static void Show()
	{
		if (!Globals.Instance.Player.GuildSystem.HasGuild())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("PlayerR_19", 0f, 0f);
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildMinesRewardDescPopUp, false, null, null);
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		GameObject parent = GameUITools.FindGameObject("Window", base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), parent);
		UIToggle component = GameUITools.FindGameObject("TabTarget", parent).GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_5D = UIEventListener.Get(component.gameObject);
		expr_5D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_5D.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		component = GameUITools.FindGameObject("TabBillboard", parent).GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		UIEventListener expr_B2 = UIEventListener.Get(component.gameObject);
		expr_B2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B2.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mContent = GameUITools.FindGameObject("Panel/Content", parent).AddComponent<GUIGuildMinesRewardDescTable>();
		this.mContent.maxPerLine = 1;
		this.mContent.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContent.cellWidth = 610f;
		this.mContent.cellHeight = 90f;
		this.mContent.gapHeight = 6f;
		this.mContent.gapWidth = 8f;
		this.mContent.bgScrollView = GameUITools.FindGameObject("PanelBG", parent).AddComponent<UIDragScrollView>();
		this.mContent.Init(610, false);
		if (Globals.Instance.Player.GuildSystem.GuildMines != null)
		{
			GameUITools.FindUILabel("Window/Mines/Value", base.gameObject).text = Globals.Instance.Player.GuildSystem.GuildMines.OreAmount.ToString();
		}
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	public void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			string name = UIToggle.current.gameObject.name;
			if (name != null)
			{
                //if (GUIGuildMinesRewardDescPopUp.<>f__switch$mapC == null)
                //{
                //    GUIGuildMinesRewardDescPopUp.<>f__switch$mapC = new Dictionary<string, int>(2)
                //    {
                //        {
                //            "TabTarget",
                //            0
                //        },
                //        {
                //            "TabBillboard",
                //            1
                //        }
                //    };
                //}
                //int num;
                //if (GUIGuildMinesRewardDescPopUp.<>f__switch$mapC.TryGetValue(name, out num))
                //{
                //    if (num != 0)
                //    {
                //        if (num == 1)
                //        {
                //            this.RefreshBilboards();
                //        }
                //    }
                //    else
                //    {
                //        this.RefreshTargets();
                //    }
                //}
			}
		}
	}

	private void RefreshTargets()
	{
		this.mContent.ClearData();
		this.mContent.SetDragAmount(0f, 0f);
		foreach (OreInfo current in Globals.Instance.AttDB.OreDict.Values)
		{
			if (current.OreAmount > 0)
			{
				this.mContent.AddData(new GUIGuildMinesRewardDescData(true, current, null));
			}
		}
	}

	private void RefreshBilboards()
	{
		this.mContent.ClearData();
		this.mContent.SetDragAmount(0f, 0f);
		foreach (OreInfo current in Globals.Instance.AttDB.OreDict.Values)
		{
			if (current.DayRewardType[0] > 0 && current.DayRewardType[0] < 20)
			{
				this.mContent.AddData(new GUIGuildMinesRewardDescData(false, current, null));
			}
		}
	}

	private void OnCloseClick(GameObject go)
	{
		base.OnButtonBlockerClick();
	}
}
