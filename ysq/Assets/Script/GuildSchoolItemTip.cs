using Att;
using Proto;
using System;
using UnityEngine;

public class GuildSchoolItemTip : MonoBehaviour
{
	private UISprite mTipBg;

	private UnityEngine.Object mOrignalItem;

	private UIGrid mItemGrid;

	private UILabel mSchoolName;

	private int mSchoolId;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mTipBg = base.transform.Find("itemTipBg").GetComponent<UISprite>();
		GameObject gameObject = this.mTipBg.transform.Find("closeBtn").gameObject;
		UIEventListener expr_3C = UIEventListener.Get(gameObject);
		expr_3C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3C.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mSchoolName = this.mTipBg.transform.Find("guildName").GetComponent<UILabel>();
		this.mItemGrid = this.mTipBg.transform.Find("contents").GetComponent<UIGrid>();
	}

	private void OnEnable()
	{
		GuildSubSystem expr_0F = Globals.Instance.Player.GuildSystem;
		expr_0F.SchoolLootDatasInitEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_0F.SchoolLootDatasInitEvent, new GuildSubSystem.VoidCallback(this.OnSchoolLootDatasUpdateEvent));
	}

	private void OnDisable()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		GuildSubSystem expr_20 = Globals.Instance.Player.GuildSystem;
		expr_20.SchoolLootDatasInitEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_20.SchoolLootDatasInitEvent, new GuildSubSystem.VoidCallback(this.OnSchoolLootDatasUpdateEvent));
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		base.gameObject.SetActive(false);
	}

	public void ShowSchoolTip(int schoolId)
	{
		this.mSchoolId = schoolId;
		base.gameObject.SetActive(true);
		GuildSubSystem guildSystem = Globals.Instance.Player.GuildSystem;
		MS2C_GetLootReward mS2C_GetLootReward;
		if (!guildSystem.mSchoolLootDataCaches.TryGetValue(schoolId, out mS2C_GetLootReward))
		{
			guildSystem.DoSendGetLootRequest(schoolId);
		}
		this.Refresh();
	}

	private void OnSchoolLootDatasUpdateEvent()
	{
		this.Refresh();
	}

	private void Refresh()
	{
		for (int i = this.mItemGrid.transform.childCount; i > 0; i--)
		{
			Transform child = this.mItemGrid.transform.GetChild(i - 1);
			UnityEngine.Object.Destroy(child.gameObject);
		}
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(this.mSchoolId);
		if (info != null && !string.IsNullOrEmpty(info.Academy))
		{
			this.mSchoolName.text = Singleton<StringManager>.Instance.GetString("guildSchool0", new object[]
			{
				this.mSchoolId,
				info.Academy
			});
		}
		MS2C_GetLootReward mS2C_GetLootReward;
		if (Globals.Instance.Player.GuildSystem.mSchoolLootDataCaches.TryGetValue(this.mSchoolId, out mS2C_GetLootReward) && mS2C_GetLootReward.LootReward != null)
		{
			for (int j = 0; j < mS2C_GetLootReward.LootReward.Count; j++)
			{
				RewardData rewardData = mS2C_GetLootReward.LootReward[j];
				if (rewardData != null)
				{
					this.AddLootItem(rewardData);
				}
			}
		}
		this.mItemGrid.repositionNow = true;
		this.mTipBg.height = ((mS2C_GetLootReward == null) ? 180 : (180 + 70 * Mathf.Max(0, mS2C_GetLootReward.LootReward.Count / 11)));
	}

	private void AddLootItem(RewardData lootData)
	{
		if (this.mOrignalItem == null)
		{
			this.mOrignalItem = Res.LoadGUI("GUI/guildSchoolTipRewardItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.mOrignalItem);
		gameObject.name = this.mOrignalItem.name;
		gameObject.transform.parent = this.mItemGrid.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUIGuildLootItemBase gUIGuildLootItemBase = gameObject.AddComponent<GUIGuildLootItemBase>();
		gUIGuildLootItemBase.InitWithBaseScene(lootData);
	}
}
