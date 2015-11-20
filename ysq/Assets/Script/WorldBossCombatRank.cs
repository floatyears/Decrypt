using Proto;
using System;
using UnityEngine;

public class WorldBossCombatRank : MonoBehaviour
{
	private static WorldBossCombatRank mInstance;

	private Transform mWinBg;

	private UISprite mBg;

	private UIPanel mRankPanel;

	private UIScrollView mRankScrollView;

	private UITable mBossRankTable;

	private Transform expandBtn;

	private WorldBossCombatRankItem[] items = new WorldBossCombatRankItem[31];

	private UILabel mPlayerName;

	private UILabel mRankTxt;

	private UnityEngine.Object mRankItemOriginal;

	private bool expand;

	private bool mHideExpand;

	private Vector4 mAnchorValue;

	private string mUIState;

	public void Init(Transform parent, Vector4 anchor, bool hideExpand = false, string uiState = "worldBoss")
	{
		this.mUIState = uiState;
		this.CreateObjects();
		Transform transform = base.transform;
		transform.parent = parent;
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		this.mAnchorValue = anchor;
		this.mBg.leftAnchor.absolute = (int)this.mAnchorValue.x;
		this.mBg.rightAnchor.absolute = (int)this.mAnchorValue.y;
		this.mBg.topAnchor.absolute = (int)this.mAnchorValue.z;
		this.mBg.SetAnchor(GameUIManager.mInstance.uiCamera.transform);
		this.expand = false;
		this.mHideExpand = hideExpand;
		this.Refresh();
		if (this.mUIState == "worldBoss")
		{
			WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
			WorldBossSubSystem expr_E1 = worldBossSystem;
			expr_E1.DamageRankEvent = (WorldBossSubSystem.VoidCallback)Delegate.Combine(expr_E1.DamageRankEvent, new WorldBossSubSystem.VoidCallback(this.OnDamageRankEvent));
		}
		else if (this.mUIState == "guildBoss")
		{
			GuildSubSystem expr_12B = Globals.Instance.Player.GuildSystem;
			expr_12B.GuildBossDamageRankEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_12B.GuildBossDamageRankEvent, new GuildSubSystem.VoidCallback(this.OnGuildBossDamageRank));
		}
	}

	private void OnDestroy()
	{
		this.mRankItemOriginal = null;
		if (Globals.Instance == null)
		{
			return;
		}
		if (this.mUIState == "worldBoss")
		{
			WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
			WorldBossSubSystem expr_3E = worldBossSystem;
			expr_3E.DamageRankEvent = (WorldBossSubSystem.VoidCallback)Delegate.Remove(expr_3E.DamageRankEvent, new WorldBossSubSystem.VoidCallback(this.OnDamageRankEvent));
		}
		else if (this.mUIState == "guildBoss")
		{
			GuildSubSystem expr_88 = Globals.Instance.Player.GuildSystem;
			expr_88.GuildBossDamageRankEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_88.GuildBossDamageRankEvent, new GuildSubSystem.VoidCallback(this.OnGuildBossDamageRank));
		}
	}

	private void CreateObjects()
	{
		this.mWinBg = base.transform.Find("winBg");
		this.mBg = this.mWinBg.GetComponent<UISprite>();
		this.expandBtn = this.mWinBg.Find("ExpandBtn");
		UIEventListener expr_4D = UIEventListener.Get(this.expandBtn.gameObject);
		expr_4D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4D.onClick, new UIEventListener.VoidDelegate(this.OnExpandClick));
		this.mRankPanel = this.mWinBg.Find("itemsPanel").GetComponent<UIPanel>();
		this.mRankScrollView = this.mRankPanel.gameObject.GetComponent<UIScrollView>();
		this.mBossRankTable = this.mRankPanel.transform.Find("itemsContents").gameObject.AddComponent<UITable>();
		this.mBossRankTable.columns = 1;
		this.mBossRankTable.direction = UITable.Direction.Down;
		this.mBossRankTable.sorting = UITable.Sorting.None;
		this.mBossRankTable.hideInactive = true;
		this.mBossRankTable.keepWithinPanel = true;
		this.mBossRankTable.padding = new Vector2(0f, 0f);
		this.mPlayerName = this.mWinBg.Find("Name").GetComponent<UILabel>();
		this.mRankTxt = this.mPlayerName.transform.Find("Label").GetComponent<UILabel>();
	}

	public void ShowCombatRank()
	{
		this.mWinBg.gameObject.SetActive(true);
	}

	public void HideCombatRank()
	{
		this.mWinBg.gameObject.SetActive(false);
	}

	public static WorldBossCombatRank GetInstance()
	{
		if (WorldBossCombatRank.mInstance == null)
		{
			GameObject original = Res.LoadGUI("GUI/WorldBossCombatRank");
			GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
			WorldBossCombatRank.mInstance = gameObject.AddComponent<WorldBossCombatRank>();
		}
		return WorldBossCombatRank.mInstance;
	}

	public WorldBossCombatRankItem AddRewardItem()
	{
		if (this.mRankItemOriginal == null)
		{
			this.mRankItemOriginal = Res.LoadGUI("GUI/BossCombatRankItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.mRankItemOriginal);
		gameObject.transform.parent = this.mBossRankTable.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		WorldBossCombatRankItem worldBossCombatRankItem = gameObject.AddComponent<WorldBossCombatRankItem>();
		worldBossCombatRankItem.InitWithBaseScene(this);
		return worldBossCombatRankItem;
	}

	private void OnExpandClick(GameObject go)
	{
		this.expand = !this.expand;
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.expand)
		{
			this.expandBtn.localRotation = Quaternion.Euler(0f, 0f, -180f);
			this.mBg.bottomAnchor.absolute = (int)this.mAnchorValue.w;
			this.mRankScrollView.SetDragAmount(0f, 0f, false);
			Vector3 localPosition = this.mRankPanel.transform.localPosition;
			Vector4 baseClipRegion = this.mRankPanel.baseClipRegion;
			baseClipRegion.w = 224f;
			this.mRankPanel.baseClipRegion = baseClipRegion;
			this.mRankPanel.clipOffset = new Vector2(0f, 371.5563f);
			this.mRankPanel.transform.localPosition = localPosition;
		}
		else
		{
			this.expandBtn.localRotation = Quaternion.identity;
			int num = 144;
			this.mBg.bottomAnchor.absolute = (int)this.mAnchorValue.w + num;
			this.mRankScrollView.SetDragAmount(0f, 0f, false);
			Vector3 localPosition2 = this.mRankPanel.transform.localPosition;
			Vector4 baseClipRegion2 = this.mRankPanel.baseClipRegion;
			baseClipRegion2.w = (float)(224 - num);
			this.mRankPanel.baseClipRegion = baseClipRegion2;
			this.mRankPanel.clipOffset = new Vector2(0f, 371.5563f + (float)(num / 2));
			this.mRankPanel.transform.localPosition = localPosition2;
		}
		this.RefreshItems();
	}

	public void RefreshItems()
	{
		int num = ((!this.expand) ? 3 : 30) + 1;
		int num2 = 0;
		for (int i = 1; i < num; i++)
		{
			RankData rankData = null;
			if (this.mUIState == "worldBoss")
			{
				rankData = Globals.Instance.Player.WorldBossSystem.GetRankData(i);
			}
			else if (this.mUIState == "guildBoss")
			{
				rankData = Globals.Instance.Player.GuildSystem.GetCurSchoolBossDamageRankData(i);
			}
			if (rankData != null)
			{
				if (this.items[i - 1] == null)
				{
					this.items[i - 1] = this.AddRewardItem();
				}
				this.items[i - 1].Refresh(rankData);
				this.items[i - 1].gameObject.SetActive(true);
				num2++;
			}
		}
		for (int j = num; j < 31; j++)
		{
			WorldBossCombatRankItem worldBossCombatRankItem = this.items[j - 1];
			if (!(worldBossCombatRankItem != null))
			{
				break;
			}
			worldBossCombatRankItem.gameObject.SetActive(false);
		}
		if (this.mUIState == "worldBoss")
		{
			RankData localData = Globals.Instance.Player.WorldBossSystem.GetLocalData();
			if (localData != null)
			{
				WorldBossCombatRankItem.RefreshRankItem(this.mPlayerName, this.mRankTxt, localData);
			}
		}
		else if (this.mUIState == "guildBoss")
		{
			RankData curSchoolBossDamageRankData = Globals.Instance.Player.GuildSystem.GetCurSchoolBossDamageRankData(0);
			if (curSchoolBossDamageRankData != null && this.mPlayerName != null && this.mRankTxt != null)
			{
				WorldBossCombatRankItem.RefreshRankItem(this.mPlayerName, this.mRankTxt, curSchoolBossDamageRankData);
			}
		}
		this.mBossRankTable.repositionNow = true;
		this.expandBtn.gameObject.SetActive(!this.mHideExpand && num2 >= 3);
	}

	public void OnDamageRankEvent()
	{
		this.RefreshItems();
	}

	public void OnGuildBossDamageRank()
	{
		this.RefreshItems();
	}
}
