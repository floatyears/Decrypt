using Att;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CombatMainPvp4TargetInfo : MonoBehaviour
{
	private class ActorUIInfo
	{
		public ActorController Actor;

		public UIGrid BuffGrid;

		public Dictionary<int, GameObject> BuffIcons = new Dictionary<int, GameObject>();

		public GameObject OriginalBuffItem;

		public UISlider HPBar;

		public UISprite Icon;

		public void OnIconClick(GameObject go)
		{
			Globals.Instance.CameraMgr.dynamicCam = false;
			Globals.Instance.CameraMgr.SelectActor = this.Actor;
		}
	}

	private class SummmonInfo : CombatMainPvp4TargetInfo.ActorUIInfo
	{
		public GameObject InfoRoot;

		public UISprite MaskImage;

		public UISprite Quality;
	}

	public enum ActorIndex
	{
		player,
		pet1,
		pet2,
		pet3,
		Max
	}

	private UILabel hpBarTxt;

	private UILabel mpBarTxt;

	private UISlider mpBar;

	private CombatMainPvp4TargetInfo.ActorUIInfo[] actorUIInfos = new CombatMainPvp4TargetInfo.ActorUIInfo[4];

	private StringBuilder sb = new StringBuilder();

	public void UpdatePlayerHPMP()
	{
		if (this.actorUIInfos[0] == null || this.actorUIInfos[0].Actor == null)
		{
			return;
		}
		long num = this.actorUIInfos[0].Actor.MaxHP;
		long num2 = this.actorUIInfos[0].Actor.CurHP;
		this.actorUIInfos[0].HPBar.value = (float)num2 / (float)num;
		this.sb.Remove(0, this.sb.Length);
		this.sb.Append(num2).Append("/").Append(num);
		this.hpBarTxt.text = this.sb.ToString();
		num = this.actorUIInfos[0].Actor.MaxMP;
		num2 = this.actorUIInfos[0].Actor.CurMP;
		this.mpBar.value = (float)num2 / (float)num;
		this.sb.Remove(0, this.sb.Length);
		this.sb.Append(num2).Append("/").Append(num);
		this.mpBarTxt.text = this.sb.ToString();
	}

	public void UpdateHPBar(int slot)
	{
		if (slot < 1 || slot >= 4 || this.actorUIInfos[slot] == null)
		{
			return;
		}
		if (this.actorUIInfos[slot].Actor == null)
		{
			return;
		}
		this.actorUIInfos[slot].HPBar.value = (float)this.actorUIInfos[slot].Actor.CurHP / (float)this.actorUIInfos[slot].Actor.MaxHP;
	}

	public void SetSummonDeath(int slot, bool bDeath)
	{
		if (slot < 1 || slot >= 4 || this.actorUIInfos[slot] == null)
		{
			return;
		}
		((CombatMainPvp4TargetInfo.SummmonInfo)this.actorUIInfos[slot]).MaskImage.gameObject.SetActive(bDeath);
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.actorUIInfos[0] = new CombatMainPvp4TargetInfo.SummmonInfo();
		this.actorUIInfos[0].Icon = base.transform.Find("stat_bm/icon").GetComponent<UISprite>();
		UIEventListener expr_46 = UIEventListener.Get(this.actorUIInfos[0].Icon.gameObject);
		expr_46.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_46.onClick, new UIEventListener.VoidDelegate(this.actorUIInfos[0].OnIconClick));
		SocketDataEx remoteSocket = Globals.Instance.Player.TeamSystem.GetRemoteSocket(0);
		this.actorUIInfos[0].Icon.spriteName = remoteSocket.GetIcon();
		this.actorUIInfos[0].BuffGrid = base.transform.Find("stat_bm/herobuff").GetComponent<UIGrid>();
		this.actorUIInfos[0].BuffGrid.pivot = UIWidget.Pivot.TopRight;
		this.actorUIInfos[0].OriginalBuffItem = this.actorUIInfos[0].BuffGrid.transform.Find("buffItem").gameObject;
		this.actorUIInfos[0].OriginalBuffItem.SetActive(false);
		UILabel component = base.transform.Find("stat_bm/headBg2/headBg/stat_lv").GetComponent<UILabel>();
		component.text = Globals.Instance.Player.TeamSystem.GetRemoteLevel().ToString();
		this.hpBarTxt = base.transform.Find("stat_bm/headBg2/hpMpBg/hpHero/hptxt").GetComponent<UILabel>();
		this.mpBarTxt = base.transform.Find("stat_bm/headBg2/hpMpBg/mpHero/mptxt").GetComponent<UILabel>();
		this.actorUIInfos[0].HPBar = base.transform.Find("stat_bm/headBg2/hpMpBg/hpHero").GetComponent<UISlider>();
		this.mpBar = base.transform.Find("stat_bm/headBg2/hpMpBg/mpHero").GetComponent<UISlider>();
		this.actorUIInfos[0].HPBar.value = 1f;
		this.mpBar.value = 1f;
		this.actorUIInfos[0].Actor = Globals.Instance.ActorMgr.GetRemoteActor(0);
		int i = 1;
		for (int j = 0; j < 3; j++)
		{
			ActorController remoteActor = Globals.Instance.ActorMgr.GetRemoteActor(j + 1);
			if (!(remoteActor == null))
			{
				CombatMainPvp4TargetInfo.SummmonInfo summmonInfo = new CombatMainPvp4TargetInfo.SummmonInfo();
				summmonInfo.InfoRoot = base.transform.Find(string.Format("pet{0}", i)).gameObject;
				summmonInfo.Icon = summmonInfo.InfoRoot.transform.Find("pet_pic").GetComponent<UISprite>();
				UIEventListener expr_293 = UIEventListener.Get(summmonInfo.Icon.gameObject);
				expr_293.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_293.onClick, new UIEventListener.VoidDelegate(summmonInfo.OnIconClick));
				summmonInfo.Icon.spriteName = remoteActor.petInfo.Icon;
				summmonInfo.BuffGrid = summmonInfo.InfoRoot.transform.Find("petbuff").GetComponent<UIGrid>();
				summmonInfo.OriginalBuffItem = summmonInfo.BuffGrid.transform.Find("buffItem").gameObject;
				summmonInfo.OriginalBuffItem.SetActive(false);
				summmonInfo.HPBar = summmonInfo.InfoRoot.transform.Find("pet_pic/hp").GetComponent<UISlider>();
				summmonInfo.HPBar.value = 1f;
				summmonInfo.MaskImage = summmonInfo.InfoRoot.transform.Find("death_mask").GetComponent<UISprite>();
				summmonInfo.MaskImage.gameObject.SetActive(false);
				summmonInfo.Quality = summmonInfo.InfoRoot.GetComponent<UISprite>();
				summmonInfo.Quality.spriteName = Tools.GetItemQualityIcon(remoteActor.petInfo.Quality);
				summmonInfo.Actor = remoteActor;
				this.actorUIInfos[j + 1] = summmonInfo;
				i++;
			}
		}
		while (i < 4)
		{
			GameObject gameObject = base.transform.Find(string.Format("pet{0}", i)).gameObject;
			gameObject.SetActive(false);
			i++;
		}
		for (int k = 0; k < 4; k++)
		{
			if (this.actorUIInfos[k] != null && !(this.actorUIInfos[k].Actor == null))
			{
				for (int l = 0; l < this.actorUIInfos[k].Actor.Buffs.Count; l++)
				{
					Buff buff = this.actorUIInfos[k].Actor.Buffs[l];
					if (buff != null)
					{
						this.OnBuffAddEvent(k, buff.SerialID, buff.Info, buff.Duration, buff.StackCount);
					}
				}
			}
		}
	}

	public void OnBuffAddEvent(int slot, int serialID, BuffInfo info, float duration, int stackCount)
	{
		if (slot >= 4 || this.actorUIInfos[slot] == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(info.Icon))
		{
			return;
		}
		GameObject gameObject = null;
		if (!this.actorUIInfos[slot].BuffIcons.TryGetValue(serialID, out gameObject))
		{
			this.actorUIInfos[slot].OriginalBuffItem.SetActive(true);
			gameObject = Tools.AddChild(this.actorUIInfos[slot].BuffGrid.gameObject, this.actorUIInfos[slot].OriginalBuffItem);
			this.actorUIInfos[slot].OriginalBuffItem.SetActive(false);
			UISprite component = gameObject.transform.Find("buffImage").GetComponent<UISprite>();
			component.spriteName = info.Icon;
			UISprite component2 = component.gameObject.transform.Find("buffCD").GetComponent<UISprite>();
			if (info.MaxDuration <= 0f)
			{
				component2.fillAmount = 0f;
			}
			else
			{
				component2.gameObject.AddComponent<GameUIAutoCoolTimer>().PlayCool(component2, info.MaxDuration - duration, info.MaxDuration);
			}
			this.actorUIInfos[slot].BuffIcons.Add(serialID, gameObject);
			this.actorUIInfos[slot].BuffGrid.repositionNow = true;
		}
		else
		{
			Transform transform = gameObject.transform.Find("buffImage/buffCD");
			if (transform != null)
			{
				GameUIAutoCoolTimer component3 = transform.GetComponent<GameUIAutoCoolTimer>();
				if (component3 != null)
				{
					component3.PlayCool(transform.GetComponent<UISprite>(), info.MaxDuration - duration, info.MaxDuration);
				}
			}
		}
	}

	public void OnBuffRemoveEvent(int slot, int serialID)
	{
		if (slot >= 4 || this.actorUIInfos[slot] == null)
		{
			return;
		}
		GameObject obj = null;
		if (this.actorUIInfos[slot].BuffIcons.TryGetValue(serialID, out obj))
		{
			this.actorUIInfos[slot].BuffIcons.Remove(serialID);
			NGUITools.Destroy(obj);
			this.actorUIInfos[slot].BuffGrid.repositionNow = true;
		}
	}

	private void Update()
	{
		this.UpdatePlayerHPMP();
		for (int i = 1; i < 4; i++)
		{
			if (this.actorUIInfos[i] != null)
			{
				if (this.actorUIInfos[i].Actor == null || this.actorUIInfos[i].Actor.IsDead)
				{
					this.actorUIInfos[i].HPBar.value = 0f;
					this.SetSummonDeath(i, true);
				}
				else
				{
					this.UpdateHPBar(i);
				}
			}
		}
	}
}
