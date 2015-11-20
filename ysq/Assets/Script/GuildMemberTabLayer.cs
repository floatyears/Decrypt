using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GuildMemberTabLayer : MonoBehaviour
{
	private GuildMemberItemsTable mMemberItemsTable;

	private UIButton mSendOnce;

	private UIButton mReceiveOnce;

	private UILabel mLeftNum;

	private UnityEngine.Object mMemberItemOriginal;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
		this.DoInitMemberItems();
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("title/level").gameObject;
		UIEventListener expr_1C = UIEventListener.Get(gameObject);
		expr_1C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C.onClick, new UIEventListener.VoidDelegate(this.OnLvlSortClick));
		GameObject gameObject2 = base.transform.Find("title/reputation").gameObject;
		UIEventListener expr_59 = UIEventListener.Get(gameObject2);
		expr_59.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_59.onClick, new UIEventListener.VoidDelegate(this.OnReputationSortClick));
		GameObject gameObject3 = base.transform.Find("title/job").gameObject;
		UIEventListener expr_96 = UIEventListener.Get(gameObject3);
		expr_96.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_96.onClick, new UIEventListener.VoidDelegate(this.OnJobSortClick));
		GameObject gameObject4 = base.transform.Find("title/lastTime").gameObject;
		UIEventListener expr_D3 = UIEventListener.Get(gameObject4);
		expr_D3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D3.onClick, new UIEventListener.VoidDelegate(this.OnLastTimeSortClick));
		this.mMemberItemsTable = base.transform.Find("contentsPanel/recordContents").gameObject.AddComponent<GuildMemberItemsTable>();
		this.mMemberItemsTable.maxPerLine = 1;
		this.mMemberItemsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mMemberItemsTable.cellWidth = 884f;
		this.mMemberItemsTable.cellHeight = 62f;
		this.mMemberItemsTable.scrollBar = base.transform.Find("contentsScrollBar").GetComponent<UIScrollBar>();
		GameObject gameObject5 = base.transform.Find("myPower").gameObject;
		UIEventListener expr_18A = UIEventListener.Get(gameObject5);
		expr_18A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_18A.onClick, new UIEventListener.VoidDelegate(this.OnMyPowerClick));
		this.mSendOnce = base.transform.Find("sendOnce").GetComponent<UIButton>();
		UIEventListener expr_1D6 = UIEventListener.Get(this.mSendOnce.gameObject);
		expr_1D6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1D6.onClick, new UIEventListener.VoidDelegate(this.OnSendOnceClick));
		this.mReceiveOnce = base.transform.Find("receiveOnce").GetComponent<UIButton>();
		UIEventListener expr_222 = UIEventListener.Get(this.mReceiveOnce.gameObject);
		expr_222.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_222.onClick, new UIEventListener.VoidDelegate(this.OnReceiveOnceClick));
		this.mLeftNum = base.transform.Find("txt1").GetComponent<UILabel>();
		this.mLeftNum.text = string.Empty;
	}

	public void DoInitMemberItems()
	{
		this.mMemberItemsTable.ClearData();
		this.InitMemberItems();
		this.RefreshBtnState();
		this.RefreshLeftNum();
	}

	private void RefreshLeftNum()
	{
		int num = Mathf.Max(0, GameConst.GetInt32(166) - Globals.Instance.Player.Data.TakeGuildGift);
		this.mLeftNum.color = ((num != 0) ? Color.green : Color.red);
		this.mLeftNum.text = num.ToString();
	}

	private void RefreshBtnState()
	{
		this.mSendOnce.isEnabled = !Tools.IsAllGiftSended();
		this.mReceiveOnce.isEnabled = !Tools.IsAllGiftReceived(false);
		UIButton[] components = this.mSendOnce.GetComponents<UIButton>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].SetState((!this.mSendOnce.isEnabled) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
		}
		UIButton[] components2 = this.mReceiveOnce.GetComponents<UIButton>();
		for (int j = 0; j < components2.Length; j++)
		{
			components2[j].SetState((!this.mReceiveOnce.isEnabled) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
		}
	}

	public void Refresh(ulong playerId = 0uL)
	{
		if (this.mMemberItemsTable == null)
		{
			return;
		}
		for (int i = 0; i < this.mMemberItemsTable.transform.childCount; i++)
		{
			GuildMemberItem component = this.mMemberItemsTable.transform.GetChild(i).GetComponent<GuildMemberItem>();
			if (component != null)
			{
				component.Refresh();
			}
		}
		this.RefreshBtnState();
		this.RefreshLeftNum();
	}

	public void ResetSBPos()
	{
		base.StartCoroutine(this.UpdateScrollBar());
	}

	[DebuggerHidden]
	public IEnumerator UpdateScrollBar()
	{
        return null;
        //GuildMemberTabLayer.<UpdateScrollBar>c__Iterator5E <UpdateScrollBar>c__Iterator5E = new GuildMemberTabLayer.<UpdateScrollBar>c__Iterator5E();
        //<UpdateScrollBar>c__Iterator5E.<>f__this = this;
        //return <UpdateScrollBar>c__Iterator5E;
	}

	private void InitMemberItems()
	{
		this.mMemberItemsTable.ClearData();
		List<GuildMember> members = Globals.Instance.Player.GuildSystem.Members;
		if (members != null && members.Count > 0)
		{
			for (int i = 0; i < members.Count; i++)
			{
				this.mMemberItemsTable.AddData(new GuildMemberItemData(members[i]));
			}
		}
		if (base.gameObject.activeInHierarchy)
		{
			this.ResetSBPos();
		}
	}

	public void RemoveMemberItem(ulong memberId)
	{
		this.mMemberItemsTable.RemoveData(memberId);
	}

	public void AddMemberItem(GuildMember member)
	{
		this.mMemberItemsTable.AddData(new GuildMemberItemData(member));
	}

	private void OnMyPowerClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildMyJobPopUp, false, null, null);
	}

	private void OnSendOnceClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Tools.IsAllGiftSended())
		{
			return;
		}
		MC2S_GiveGift mC2S_GiveGift = new MC2S_GiveGift();
		mC2S_GiveGift.PlayerID = 0uL;
		Globals.Instance.CliSession.Send(950, mC2S_GiveGift);
	}

	private void OnReceiveOnceClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Tools.IsAllGiftReceived(true))
		{
			return;
		}
		MC2S_TakeGift mC2S_TakeGift = new MC2S_TakeGift();
		mC2S_TakeGift.PlayerID = 0uL;
		Globals.Instance.CliSession.Send(948, mC2S_TakeGift);
	}

	private void OnJobSortClick(GameObject go)
	{
		this.mMemberItemsTable.SetSortType(0);
	}

	private void OnLvlSortClick(GameObject go)
	{
		this.mMemberItemsTable.SetSortType(2);
	}

	private void OnReputationSortClick(GameObject go)
	{
		this.mMemberItemsTable.SetSortType(1);
	}

	private void OnLastTimeSortClick(GameObject go)
	{
		this.mMemberItemsTable.SetSortType(3);
	}
}
