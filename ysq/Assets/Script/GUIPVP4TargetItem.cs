using Att;
using Proto;
using System;
using UnityEngine;

public class GUIPVP4TargetItem : UICustomGridItem
{
	private const int NUM_ICON = 4;

	private UISprite back;

	private UISprite backTop;

	private UILabel Level;

	private UILabel Name;

	private UILabel CombatValue;

	private UILabel RankValue;

	private UISprite[] Icon = new UISprite[4];

	private UISprite[] Quality = new UISprite[4];

	private UIButton view;

	public UIButton pk;

	private UILabel mViewLb;

	public PVPTargetDaata data;

	public void Init()
	{
		this.back = base.transform.Find("bkbottom").GetComponent<UISprite>();
		this.backTop = base.transform.Find("bktop").GetComponent<UISprite>();
		this.Level = base.transform.FindChild("Level").GetComponent<UILabel>();
		this.Name = base.transform.FindChild("Name").GetComponent<UILabel>();
		this.CombatValue = base.transform.FindChild("CombatValue/num").GetComponent<UILabel>();
		this.RankValue = base.transform.FindChild("RankValue/num").GetComponent<UILabel>();
		Transform transform = base.transform.FindChild("Team");
		for (int i = 0; i < 4; i++)
		{
			this.Icon[i] = transform.FindChild(string.Format("Icon{0}", i)).GetComponent<UISprite>();
			this.Quality[i] = this.Icon[i].transform.FindChild("Quality").GetComponent<UISprite>();
			this.Icon[i].gameObject.SetActive(false);
		}
		this.view = transform.FindChild("view").GetComponent<UIButton>();
		this.mViewLb = GameUITools.FindUILabel("Label", this.view.gameObject);
		this.pk = transform.FindChild("pk").GetComponent<UIButton>();
		UIEventListener expr_176 = UIEventListener.Get(this.view.gameObject);
		expr_176.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_176.onClick, new UIEventListener.VoidDelegate(this.OnViewTraget));
		UIEventListener expr_1A7 = UIEventListener.Get(this.pk.gameObject);
		expr_1A7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1A7.onClick, new UIEventListener.VoidDelegate(this.OnPkTraget));
		this.view.gameObject.SetActive(false);
		this.pk.gameObject.SetActive(false);
	}

	public override void Refresh(object _data)
	{
		if (this.data == _data)
		{
			return;
		}
		this.data = (PVPTargetDaata)_data;
		this.ShowRankData();
	}

	public void ShowRankData()
	{
		LocalPlayer player = Globals.Instance.Player;
		bool flag = player.Data.ID == this.data.RankData.Data.GUID;
		this.back.spriteName = ((!flag) ? "info1" : "info3");
		this.backTop.spriteName = ((!flag) ? "info2" : "info4");
		this.view.gameObject.SetActive(!flag);
		if (flag)
		{
			this.pk.gameObject.SetActive(false);
		}
		else if (this.data.RankData.Rank > 0 && this.data.RankData.Rank <= 10)
		{
			this.pk.gameObject.SetActive(player.PvpSystem.Rank <= GameConst.GetInt32(68));
		}
		else
		{
			if (this.data.IsFarmRebot)
			{
				this.mViewLb.text = Singleton<StringManager>.Instance.GetString("pvp4Txt20");
			}
			else
			{
				this.mViewLb.text = Singleton<StringManager>.Instance.GetString("pvp4Txt21");
			}
			this.pk.gameObject.SetActive(true);
		}
		this.Level.text = string.Format("Lv {0}", this.data.RankData.Data.Level);
		this.Name.text = this.data.RankData.Data.Name;
		this.CombatValue.text = this.data.RankData.Data.CombatValue.ToString();
		if (flag && (player.Data.DataFlag & 64) == 0)
		{
			this.RankValue.transform.parent.gameObject.SetActive(false);
		}
		else if (this.data.IsFarmRebot)
		{
			this.RankValue.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			this.RankValue.transform.parent.gameObject.SetActive(true);
			this.RankValue.text = this.data.RankData.Rank.ToString();
		}
		FashionInfo info = Globals.Instance.AttDB.FashionDict.GetInfo(this.data.RankData.Data.FashionID);
		if (info != null)
		{
			this.Icon[0].spriteName = info.Icon;
		}
		this.Quality[0].spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(this.data.RankData.Data.ConstellationLevel));
		this.Icon[0].gameObject.SetActive(true);
		int i = 1;
		int num = 0;
		while (i < 4)
		{
			if (num >= this.data.RankData.Data.PetInfoID.Count)
			{
				this.Icon[i].gameObject.SetActive(false);
				num++;
				i++;
			}
			else if (this.data.RankData.Data.PetInfoID[num] == 0)
			{
				num++;
			}
			else
			{
				PetInfo info2 = Globals.Instance.AttDB.PetDict.GetInfo(this.data.RankData.Data.PetInfoID[num]);
				if (info2 == null)
				{
					global::Debug.LogErrorFormat("can not find anra target pet info {0}", new object[]
					{
						this.data.RankData.Data.PetInfoID[num]
					});
					num++;
				}
				else
				{
					this.Icon[i].spriteName = info2.Icon;
					this.Quality[i].spriteName = Tools.GetItemQualityIcon(info2.Quality);
					this.Icon[i].gameObject.SetActive(true);
					num++;
					i++;
				}
			}
		}
	}

	private void OnViewTraget(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.data == null || this.data.RankData == null)
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (player.Data.ID == this.data.RankData.Data.GUID)
		{
			return;
		}
		if (this.data.IsFarmRebot)
		{
			if (player.Data.Stamina < GameConst.GetInt32(36))
			{
				GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Stamina);
				return;
			}
			this.SendFarmMsg(5);
		}
		else
		{
			MC2S_QueryRemotePlayer mC2S_QueryRemotePlayer = new MC2S_QueryRemotePlayer();
			mC2S_QueryRemotePlayer.PlayerID = this.data.RankData.Data.GUID;
			Globals.Instance.CliSession.Send(286, mC2S_QueryRemotePlayer);
		}
	}

	private void SendFarmMsg(int times)
	{
		MC2S_PvpFarm mC2S_PvpFarm = new MC2S_PvpFarm();
		mC2S_PvpFarm.TargetID = this.data.RankData.Data.GUID;
		mC2S_PvpFarm.Count = times;
		Globals.Instance.CliSession.Send(824, mC2S_PvpFarm);
		GameUIState uiState = GameUIManager.mInstance.uiState;
		LocalPlayer player = Globals.Instance.Player;
		uiState.PlayerLevel = player.Data.Level;
		uiState.PlayerEnergy = player.Data.Energy;
		uiState.PlayerExp = player.Data.Exp;
		uiState.PlayerMoney = player.Data.Money;
		uiState.SetOldFurtherData(Globals.Instance.Player.TeamSystem.GetPet(0));
	}

	public void OnPkTraget(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.data == null || this.data.RankData == null)
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		bool flag = player.Data.ID == this.data.RankData.Data.GUID;
		if (flag)
		{
			return;
		}
		if (player.Data.Stamina < GameConst.GetInt32(36))
		{
			GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Stamina);
			return;
		}
		if (this.data.IsFarmRebot)
		{
			this.SendFarmMsg(1);
			return;
		}
		if (player.PvpSystem.Rank > GameConst.GetInt32(68) && this.data.RankData.Rank <= 10)
		{
			GameUIManager.mInstance.ShowMessageTip(string.Format(Singleton<StringManager>.Instance.GetString("pvp4Top"), GameConst.GetInt32(68)), 0f, 0f);
			return;
		}
		Globals.Instance.Player.PvpSystem.SetArenaTargetID(this.data.RankData.Data.GUID);
		MC2S_PvpArenaStart mC2S_PvpArenaStart = new MC2S_PvpArenaStart();
		mC2S_PvpArenaStart.TargetID = this.data.RankData.Data.GUID;
		mC2S_PvpArenaStart.Rank = this.data.RankData.Rank;
		Globals.Instance.CliSession.Send(803, mC2S_PvpArenaStart);
		GameUIState uiState = GameUIManager.mInstance.uiState;
		uiState.PlayerLevel = player.Data.Level;
		uiState.PlayerEnergy = player.Data.Energy;
		uiState.PlayerExp = player.Data.Exp;
		uiState.PlayerMoney = player.Data.Money;
		uiState.ArenaHighestRank = player.Data.ArenaHighestRank;
		uiState.SetOldFurtherData(player.TeamSystem.GetPet(0));
	}
}
