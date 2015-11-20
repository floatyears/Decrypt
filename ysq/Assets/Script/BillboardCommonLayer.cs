using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCommonLayer : MonoBehaviour
{
	public enum BillboardType
	{
		BT_CombatValue,
		BT_Guild,
		BT_PVP4,
		BT_PVEStars,
		BT_Level,
		BT_WorldBoss,
		BT_Trial,
		BT_Rose,
		BT_Turtle
	}

	private GUIBillboard mBaseScene;

	private UILabel mLabel1;

	private UILabel mLabel2;

	private UILabel mFighting;

	private UILabel mValue;

	private CommonRankItemTable mItemsTable;

	public int specialNum
	{
		get;
		private set;
	}

	public void InitBillboard(GUIBillboard baseScene)
	{
		this.mBaseScene = baseScene;
		this.mItemsTable = GameUITools.FindGameObject("RankPanel/Contents", base.gameObject).gameObject.AddComponent<CommonRankItemTable>();
		this.mItemsTable.maxPerLine = 1;
		this.mItemsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mItemsTable.cellWidth = 598f;
		this.mItemsTable.cellHeight = 87f;
		this.mItemsTable.scrollBar = GameUITools.FindGameObject("ScrollBar", base.gameObject).GetComponent<UIScrollBar>();
		this.mItemsTable.InitWithBaseScene(this);
		this.mLabel1 = GameUITools.FindUILabel("Label1", base.gameObject);
		this.mLabel2 = GameUITools.FindUILabel("Label2", base.gameObject);
		this.mFighting = GameUITools.FindUILabel("Fighting", base.gameObject);
		this.mValue = GameUITools.FindUILabel("Value", base.gameObject);
		this.mLabel1.gameObject.SetActive(false);
		this.mLabel2.gameObject.SetActive(false);
		this.mFighting.gameObject.SetActive(false);
		this.mValue.gameObject.SetActive(false);
	}

	public void Refresh(BillboardCommonLayer.BillboardType type, int param1 = -1)
	{
		this.mBaseScene.isWaitingMessageReply = false;
		this.mItemsTable.scrollBar.value = 0f;
		switch (type)
		{
		case BillboardCommonLayer.BillboardType.BT_CombatValue:
			this.RefreshCombatValue();
			break;
		case BillboardCommonLayer.BillboardType.BT_Guild:
			this.RefreshGuild(param1);
			break;
		case BillboardCommonLayer.BillboardType.BT_PVP4:
			this.RefreshPVP4();
			break;
		case BillboardCommonLayer.BillboardType.BT_PVEStars:
			this.RefreshPVEStars();
			break;
		case BillboardCommonLayer.BillboardType.BT_Level:
			this.RefreshLevel();
			break;
		case BillboardCommonLayer.BillboardType.BT_WorldBoss:
			this.RefreshWorldBoss();
			break;
		case BillboardCommonLayer.BillboardType.BT_Trial:
			this.RefreshTrial();
			break;
		case BillboardCommonLayer.BillboardType.BT_Rose:
			this.RefreshRose();
			break;
		case BillboardCommonLayer.BillboardType.BT_Turtle:
			this.RefreshTurtle();
			break;
		}
	}

	private void RefreshWorldBoss()
	{
		this.mItemsTable.className = "WorldBossRankKillItem";
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		RankData killerData = worldBossSystem.GetKillerData();
		bool flag = killerData != null;
		List<object> list = new List<object>();
		if (flag)
		{
			list.Add(killerData);
		}
		for (int i = 1; i < 31; i++)
		{
			RankData rankData = worldBossSystem.GetRankData(i);
			if (rankData != null)
			{
				list.Add(rankData);
			}
		}
		this.InitItems(list, 3, (!flag) ? 1 : 0);
		string @string;
		if (worldBossSystem.Rank <= 31 && 0 < worldBossSystem.Rank)
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				worldBossSystem.Rank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				Singleton<StringManager>.Instance.GetString("Billboard0")
			});
		}
		this.Refresh(@string, null, string.Format("[9e865a]{0}[-]", Singleton<StringManager>.Instance.GetString("worldBossTxt3")), worldBossSystem.TotalDamage.ToString());
	}

	private void RefreshLevel()
	{
		BillboardSubSystem billboardSystem = Globals.Instance.Player.BillboardSystem;
		this.mItemsTable.className = "LevelRankItem";
		List<object> list = new List<object>();
		for (int i = 0; i < billboardSystem.LevelRankData.Count; i++)
		{
			RankData rankData = billboardSystem.LevelRankData[i];
			if (rankData != null)
			{
				list.Add(rankData);
			}
		}
		this.InitItems(list, 3, 0);
		string @string;
		if (billboardSystem.LevelRank > 0u)
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				billboardSystem.LevelRank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				Singleton<StringManager>.Instance.GetString("Billboard0")
			});
		}
		this.Refresh(@string, null, string.Format("[9e865a]{0}[-]", Singleton<StringManager>.Instance.GetString("BillboardLevelCharacterTxt")), Globals.Instance.Player.Data.Level.ToString());
	}

	private void RefreshGuild(int selfRank)
	{
		GuildSubSystem guildSystem = Globals.Instance.Player.GuildSystem;
		this.mItemsTable.className = "GUIGuildRankItem";
		List<object> list = new List<object>();
		for (int i = 0; i < guildSystem.GuildRankDataList.Count; i++)
		{
			if (guildSystem.GuildRankDataList[i] != null)
			{
				list.Add(guildSystem.GuildRankDataList[i]);
			}
		}
		this.InitItems(list, 3, 0);
		string @string;
		if (selfRank <= 100 && 0 < selfRank)
		{
			@string = Singleton<StringManager>.Instance.GetString("guild23", new object[]
			{
				selfRank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("guild23", new object[]
			{
				Singleton<StringManager>.Instance.GetString("Billboard0")
			});
		}
		this.Refresh(@string, null, null, null);
	}

	private void RefreshRose()
	{
		BillboardSubSystem billboardSystem = Globals.Instance.Player.BillboardSystem;
		this.mItemsTable.className = "RoseRankItem";
		List<object> list = new List<object>();
		for (int i = 0; i < billboardSystem.RoseRankData.Count; i++)
		{
			RankData rankData = billboardSystem.RoseRankData[i];
			if (rankData != null)
			{
				list.Add(rankData);
			}
		}
		this.InitItems(list, 3, 0);
		string @string;
		if (billboardSystem.RoseRank > 0u)
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				billboardSystem.RoseRank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				Singleton<StringManager>.Instance.GetString("Billboard0")
			});
		}
		this.Refresh(@string, null, string.Format("[9e865a]{0}[-]", Singleton<StringManager>.Instance.GetString("BillboardRoseNum")), billboardSystem.RoseValue.ToString());
	}

	private void RefreshTurtle()
	{
		BillboardSubSystem billboardSystem = Globals.Instance.Player.BillboardSystem;
		this.mItemsTable.className = "TurtleRankItem";
		List<object> list = new List<object>();
		for (int i = 0; i < billboardSystem.TurtleRankData.Count; i++)
		{
			RankData rankData = billboardSystem.TurtleRankData[i];
			if (rankData != null)
			{
				list.Add(rankData);
			}
		}
		this.InitItems(list, 3, 0);
		string @string;
		if (billboardSystem.TurtleRank > 0u)
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				billboardSystem.TurtleRank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				Singleton<StringManager>.Instance.GetString("Billboard0")
			});
		}
		this.Refresh(@string, null, string.Format("[9e865a]{0}[-]", Singleton<StringManager>.Instance.GetString("BillboardTurtleNum")), billboardSystem.TurtleValue.ToString());
	}

	private void RefreshCombatValue()
	{
		BillboardSubSystem billboardSystem = Globals.Instance.Player.BillboardSystem;
		this.mItemsTable.className = "CombatValueRankItem";
		List<object> list = new List<object>();
		for (int i = 0; i < billboardSystem.CombatValueRankData.Count; i++)
		{
			RankData rankData = billboardSystem.CombatValueRankData[i];
			if (rankData != null)
			{
				list.Add(rankData);
			}
		}
		this.InitItems(list, 3, 0);
		string @string;
		if (billboardSystem.CombatValueRank > 0u)
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				billboardSystem.CombatValueRank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				Singleton<StringManager>.Instance.GetString("Billboard0")
			});
		}
		long num = 0L;
		if (billboardSystem.CombatValueRank != 0u && billboardSystem.CombatValueRank <= 50u)
		{
			for (int j = 0; j < billboardSystem.CombatValueRankData.Count; j++)
			{
				RankData rankData2 = billboardSystem.CombatValueRankData[j];
				if (rankData2 != null && rankData2.Data.GUID == Globals.Instance.Player.Data.ID)
				{
					num = rankData2.Value;
				}
			}
		}
		else
		{
			num = (long)Globals.Instance.Player.TeamSystem.GetCombatValue();
		}
		this.Refresh(@string, null, string.Format("[9e865a]{0}[-]", Singleton<StringManager>.Instance.GetString("pvp4Txt12")), num.ToString());
	}

	private void RefreshPVEStars()
	{
		BillboardSubSystem billboardSystem = Globals.Instance.Player.BillboardSystem;
		this.mItemsTable.className = "PVEStarsRankItem";
		List<object> list = new List<object>();
		for (int i = 0; i < billboardSystem.PVEStarsRankData.Count; i++)
		{
			RankData rankData = billboardSystem.PVEStarsRankData[i];
			if (rankData != null)
			{
				list.Add(rankData);
			}
		}
		this.InitItems(list, 3, 0);
		string @string;
		if (billboardSystem.PVEStarsRank > 0u)
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				billboardSystem.PVEStarsRank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				Singleton<StringManager>.Instance.GetString("Billboard0")
			});
		}
		this.Refresh(@string, null, string.Format("[9e865a]{0}[-]", Singleton<StringManager>.Instance.GetString("BillboardPVEStarsTxt")), Tools.GetPVEStars(0).ToString());
	}

	private void RefreshPVP4()
	{
		PvpSubSystem pvpSystem = Globals.Instance.Player.PvpSystem;
		this.mItemsTable.className = "PVP4RankItem";
		List<object> list = new List<object>();
		for (int i = 0; i < pvpSystem.ArenaRank.Count; i++)
		{
			RankData rankData = pvpSystem.ArenaRank[i];
			if (rankData != null)
			{
				list.Add(rankData);
			}
		}
		this.InitItems(list, 3, 0);
		string @string;
		if (Globals.Instance.Player.PvpSystem.ShowSelfRank() && pvpSystem.Rank <= 15000 && 0 < pvpSystem.Rank)
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				pvpSystem.Rank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				Singleton<StringManager>.Instance.GetString("Billboard0")
			});
		}
		this.Refresh(@string, null, null, null);
	}

	private void RefreshTrial()
	{
		BillboardSubSystem billboardSystem = Globals.Instance.Player.BillboardSystem;
		this.mItemsTable.className = "TrailRankItem";
		List<object> list = new List<object>();
		for (int i = 0; i < billboardSystem.TrialRankData.Count; i++)
		{
			RankData rankData = billboardSystem.TrialRankData[i];
			if (rankData != null)
			{
				list.Add(rankData);
			}
		}
		this.InitItems(list, 3, 0);
		string @string;
		if (billboardSystem.TrialRank > 0u)
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				billboardSystem.TrialRank
			});
		}
		else
		{
			@string = Singleton<StringManager>.Instance.GetString("trailTower14", new object[]
			{
				Singleton<StringManager>.Instance.GetString("Billboard0")
			});
		}
		this.Refresh(@string, null, string.Format("[9e865a]{0}[-]", Singleton<StringManager>.Instance.GetString("BillboardTrialFloors")), Globals.Instance.Player.Data.TrialMaxWave.ToString());
	}

	public void InitItems(List<object> datas, int num = 3, int startIndex = 0)
	{
		if (this.mItemsTable.gridItems != null)
		{
			for (int i = 0; i < this.mItemsTable.gridItems.Length; i++)
			{
				if (this.mItemsTable.gridItems[i] != null)
				{
					GameObject gameObject = this.mItemsTable.gridItems[i].gameObject;
					UnityEngine.Object.Destroy(this.mItemsTable.gridItems[i]);
					CommonRankItemBase commonRankItemBase = gameObject.AddComponent(this.mItemsTable.className) as CommonRankItemBase;
					commonRankItemBase.InitWithBaseScene(this);
					this.mItemsTable.gridItems[i] = commonRankItemBase;
				}
			}
		}
		this.mItemsTable.ClearData();
		this.specialNum = num;
		for (int j = startIndex; j < datas.Count + startIndex; j++)
		{
			this.mItemsTable.AddData(new BillboardInfoData(datas[j - startIndex], j));
		}
		this.mItemsTable.repositionNow = true;
	}

	public void Refresh(string txt1, string txt2, string txt3, string txt4)
	{
		if (!string.IsNullOrEmpty(txt1))
		{
			this.mLabel1.text = txt1;
			this.mLabel1.gameObject.SetActive(true);
		}
		else
		{
			this.mLabel1.gameObject.SetActive(false);
		}
		if (!string.IsNullOrEmpty(txt2))
		{
			this.mLabel2.text = txt2;
			this.mLabel2.gameObject.SetActive(true);
		}
		else
		{
			this.mLabel2.gameObject.SetActive(false);
		}
		if (!string.IsNullOrEmpty(txt3))
		{
			this.mFighting.text = txt3;
			this.mFighting.gameObject.SetActive(true);
		}
		else
		{
			this.mFighting.gameObject.SetActive(false);
		}
		if (!string.IsNullOrEmpty(txt4))
		{
			this.mValue.text = txt4;
			this.mValue.gameObject.SetActive(true);
		}
		else
		{
			this.mValue.gameObject.SetActive(false);
		}
	}
}
