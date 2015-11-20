using Att;
using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIRecycleGetItemsPopUp : GameUIBasePopup
{
	public delegate void VoidCallBack(List<RewardData> tempDataList);

	public GUIRecycleGetItemsPopUp.VoidCallBack OKEvent;

	private UILabel mTitle;

	private RecycleRewardItemsUITable mContentsTable;

	private UILabel mCost;

	private UILabel mTips;

	private Dictionary<ERewardType, RewardData> rewardDataDict = new Dictionary<ERewardType, RewardData>();

	private Dictionary<int, int> itemDataDict = new Dictionary<int, int>();

	private Dictionary<int, int> petDataDict = new Dictionary<int, int>();

	private Dictionary<int, int> lopetDataDict = new Dictionary<int, int>();

	private List<RewardData> tempDataList = new List<RewardData>();

	public static void ShowThis(GUIRecycleScene.ERecycleT type, GUIRecycleGetItemsPopUp.VoidCallBack cb)
	{
		if (cb == null)
		{
			global::Debug.LogError(new object[]
			{
				"params is null"
			});
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIRecycleGetItemsPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(type, cb);
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseBtnClick), base.gameObject);
		GameUITools.RegisterClickEvent("OKBtn", new UIEventListener.VoidDelegate(this.OnOKClick), base.gameObject);
		GameUITools.RegisterClickEvent("CancelBtn", new UIEventListener.VoidDelegate(this.OnCancelClick), base.gameObject);
		this.mTitle = GameUITools.FindUILabel("Title", base.gameObject);
		this.mCost = GameUITools.FindUILabel("Cost", base.gameObject);
		this.mTips = GameUITools.FindUILabel("Tips", base.gameObject);
		this.mContentsTable = GameUITools.FindGameObject("Panel/Contents", base.gameObject).AddComponent<RecycleRewardItemsUITable>();
		this.mContentsTable.maxPerLine = 5;
		this.mContentsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContentsTable.cellWidth = 90f;
		this.mContentsTable.cellHeight = 90f;
		this.mContentsTable.gapHeight = 18f;
		this.mContentsTable.gapWidth = 14f;
		this.mContentsTable.bgScrollView = GameUITools.FindGameObject("PanelBG", base.gameObject).GetComponent<UIDragScrollView>();
	}

	public override void InitPopUp(GUIRecycleScene.ERecycleT type, GUIRecycleGetItemsPopUp.VoidCallBack cb)
	{
		this.OKEvent = cb;
		bool flag = false;
		switch (type)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetBreak:
			this.mCost.gameObject.SetActive(false);
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("recycle21");
			foreach (ulong current in GameUIManager.mInstance.uiState.PetBreakUpData.PetID)
			{
				PetDataEx pet = Globals.Instance.Player.PetSystem.GetPet(current);
				if (pet != null)
				{
					if (!flag && pet.Info.Quality >= 2)
					{
						flag = true;
					}
					uint num;
					uint num2;
					uint num3;
					uint num4;
					uint[] array;
					uint num5;
					uint num6;
					uint num7;
					pet.GetBreakData(out num, out num2, out num3, out num4, out array, out num5, out num6, out num7);
					if (num > 0u)
					{
						this.AddRewardData(ERewardType.EReward_MagicSoul, (int)num);
					}
					if (num2 > 0u)
					{
						this.AddRewardData(ERewardType.EReward_Money, (int)num2);
					}
					if (num3 > 0u)
					{
						this.AddItemData(GameConst.GetInt32(100), (int)num3);
					}
					if (num4 > 0u)
					{
						this.AddItemData(GameConst.GetInt32(101), (int)num4);
					}
					if (num5 > 0u)
					{
						this.AddRewardData(ERewardType.EReward_StarSoul, (int)num5);
					}
					if (num6 > 0u)
					{
						this.AddItemData(GameConst.GetInt32(118), (int)num6);
					}
					if (num7 > 0u)
					{
						this.AddItemData(GameConst.GetInt32(178), (int)num7);
					}
					int num8 = 0;
					while (num8 < array.Length && num8 < GameConst.PET_EXP_ITEM_ID.Length)
					{
						if (array[num8] > 0u)
						{
							this.AddItemData(GameConst.PET_EXP_ITEM_ID[num8], (int)array[num8]);
						}
						num8++;
					}
				}
			}
			if (flag)
			{
				this.mTips.text = Singleton<StringManager>.Instance.GetString("recycle36", new object[]
				{
					Singleton<StringManager>.Instance.GetString("teamate")
				});
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak:
			this.mCost.gameObject.SetActive(false);
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("recycle21");
			foreach (ulong current2 in GameUIManager.mInstance.uiState.EquipBreakUpData.EquipID)
			{
				ItemDataEx item = Globals.Instance.Player.ItemSystem.GetItem(current2);
				if (item != null)
				{
					if (!flag && item.Info.Quality >= 2)
					{
						flag = true;
					}
					uint num9;
					uint num10;
					uint num11;
					uint[] array2;
					item.GetEquipBreakValue(out num9, out num10, out num11, out array2);
					if (num11 > 0u)
					{
						this.AddRewardData(ERewardType.EReward_Money, (int)num11);
					}
					if (num9 > 0u)
					{
						this.AddRewardData(ERewardType.EReward_MagicCrystal, (int)num9);
					}
					if (num10 > 0u)
					{
						this.AddRewardData(ERewardType.EReward_Emblem, (int)num10);
					}
					int num12 = 0;
					while (num12 < array2.Length && num12 < GameConst.EQUIP_REFINE_ITEM_ID.Length)
					{
						if (array2[num12] > 0u)
						{
							this.AddItemData(GameConst.EQUIP_REFINE_ITEM_ID[num12], (int)array2[num12]);
						}
						num12++;
					}
				}
			}
			if (flag)
			{
				this.mTips.text = Singleton<StringManager>.Instance.GetString("recycle36", new object[]
				{
					Singleton<StringManager>.Instance.GetString("equipLb")
				});
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_PetReborn:
		{
			this.mCost.gameObject.SetActive(true);
			this.mCost.text = GameConst.GetInt32(67).ToString();
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("recycle22");
			PetDataEx pet2 = Globals.Instance.Player.PetSystem.GetPet(GameUIManager.mInstance.uiState.PetRebornData.PetID);
			if (pet2 == null)
			{
				return;
			}
			List<OpenLootData> list = new List<OpenLootData>();
			uint num13;
			uint num14;
			uint num15;
			uint num16;
			uint[] array3;
			uint num17;
			uint num18;
			pet2.GetRebornData(out num13, out num14, out num15, out num16, out array3, out num17, ref list, out num18, false);
			if (num13 > 0u)
			{
				this.AddPetData(pet2.Info.ID, (int)num13);
			}
			if (num14 > 0u)
			{
				this.AddRewardData(ERewardType.EReward_Money, (int)num14);
			}
			if (num15 > 0u)
			{
				this.AddItemData(GameConst.GetInt32(100), (int)num15);
			}
			if (num16 > 0u)
			{
				this.AddItemData(GameConst.GetInt32(101), (int)num16);
			}
			if (num17 > 0u)
			{
				this.AddItemData(GameConst.GetInt32(118), (int)num17);
			}
			if (num18 > 0u)
			{
				this.AddItemData(GameConst.GetInt32(178), (int)num18);
			}
			int num19 = 0;
			while (num19 < array3.Length && num19 < GameConst.PET_EXP_ITEM_ID.Length)
			{
				if (array3[num19] > 0u)
				{
					this.AddItemData(GameConst.PET_EXP_ITEM_ID[num19], (int)array3[num19]);
				}
				num19++;
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.AddItemData(list[i].InfoID, (int)list[i].Count);
			}
			this.mTips.text = string.Empty;
			break;
		}
		case GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn:
		{
			this.mCost.gameObject.SetActive(true);
			this.mCost.text = GameConst.GetInt32(67).ToString();
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("recycle22");
			ItemDataEx item2 = Globals.Instance.Player.ItemSystem.GetItem(GameUIManager.mInstance.uiState.TrinketRebornData.TrinketID);
			if (item2 == null)
			{
				return;
			}
			uint num20;
			uint num21;
			uint num22;
			uint[] array4;
			uint num23;
			item2.GetTrinketRebornValue(out num20, out num21, out num22, out array4, out num23);
			if (num20 > 0u)
			{
				this.AddRewardData(ERewardType.EReward_Money, (int)num20);
			}
			if (num21 > 0u)
			{
				this.AddItemData(item2.Info.ID, (int)num21);
			}
			if (num22 > 0u)
			{
				this.AddItemData(GameConst.GetInt32(102), (int)num22);
			}
			int num24 = 0;
			while (num24 < array4.Length && num24 < GameConst.TRINKET_ENHANCE_EXP_ITEM_ID.Length)
			{
				if (array4[num24] > 0u)
				{
					this.AddItemData(GameConst.TRINKET_ENHANCE_EXP_ITEM_ID[num24], (int)array4[num24]);
				}
				num24++;
			}
			if (num23 > 0u)
			{
				ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(161));
				if (info == null)
				{
					global::Debug.LogError(new object[]
					{
						"ItemDict get info error , ID : {0}",
						GameConst.GetInt32(161)
					});
				}
				else
				{
					this.AddItemData(info.ID, (int)num23);
				}
			}
			this.mTips.text = string.Empty;
			break;
		}
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak:
			this.mCost.gameObject.SetActive(true);
			this.mCost.text = GameConst.GetInt32(216).ToString();
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("recycle21");
			foreach (ulong current3 in GameUIManager.mInstance.uiState.LopetBreakData.LopetID)
			{
				LopetDataEx lopet = Globals.Instance.Player.LopetSystem.GetLopet(current3);
				if (lopet != null)
				{
					if (!flag && lopet.Info.Quality >= 2)
					{
						flag = true;
					}
					uint num25;
					uint num26;
					uint num27;
					uint[] array5;
					lopet.GetBreakData(out num25, out num26, out num27, out array5);
					if (num25 > 0u)
					{
						this.AddRewardData(ERewardType.EReward_LopetSoul, (int)num25);
					}
					if (num26 > 0u)
					{
						this.AddRewardData(ERewardType.EReward_Money, (int)num26);
					}
					if (num27 > 0u)
					{
						this.AddItemData(GameConst.GetInt32(205), (int)num27);
					}
					int num28 = 0;
					while (num28 < array5.Length && num28 < GameConst.LOPET_EXP_ITEM_ID.Length)
					{
						if (array5[num28] > 0u)
						{
							this.AddItemData(GameConst.LOPET_EXP_ITEM_ID[num28], (int)array5[num28]);
						}
						num28++;
					}
				}
			}
			if (flag)
			{
				this.mTips.text = Singleton<StringManager>.Instance.GetString("recycle36", new object[]
				{
					Singleton<StringManager>.Instance.GetString("teamate")
				});
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn:
		{
			this.mCost.gameObject.SetActive(true);
			this.mCost.text = GameConst.GetInt32(216).ToString();
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("recycle22");
			LopetDataEx lopet2 = Globals.Instance.Player.LopetSystem.GetLopet(GameUIManager.mInstance.uiState.LopetRebornData.LopetID);
			if (lopet2 == null)
			{
				return;
			}
			uint num29;
			uint num30;
			uint num31;
			uint[] array6;
			lopet2.GetRebornData(out num29, out num30, out num31, out array6);
			if (num29 > 0u)
			{
				this.AddLopetData(lopet2.Info.ID, (int)num29);
			}
			if (num30 > 0u)
			{
				this.AddRewardData(ERewardType.EReward_Money, (int)num30);
			}
			if (num31 > 0u)
			{
				this.AddItemData(GameConst.GetInt32(205), (int)num31);
			}
			int num32 = 0;
			while (num32 < array6.Length && num32 < GameConst.PET_EXP_ITEM_ID.Length)
			{
				if (array6[num32] > 0u)
				{
					this.AddItemData(GameConst.LOPET_EXP_ITEM_ID[num32], (int)array6[num32]);
				}
				num32++;
			}
			this.mTips.text = string.Empty;
			break;
		}
		}
		this.mContentsTable.ClearData();
		foreach (KeyValuePair<ERewardType, RewardData> current4 in this.rewardDataDict)
		{
			this.mContentsTable.AddData(new RecycleGetItemData(current4.Value));
			this.tempDataList.Add(current4.Value);
		}
		foreach (KeyValuePair<int, int> current5 in this.itemDataDict)
		{
			if (Globals.Instance.AttDB.ItemDict.GetInfo(current5.Key) == null)
			{
				global::Debug.LogErrorFormat("ItemDict.GetInfo error, id = {0}", new object[]
				{
					current5.Key
				});
			}
			else
			{
				RewardData rewardData = new RewardData();
				rewardData.RewardType = 3;
				rewardData.RewardValue1 = current5.Key;
				rewardData.RewardValue2 = current5.Value;
				this.mContentsTable.AddData(new RecycleGetItemData(rewardData));
				this.tempDataList.Add(rewardData);
			}
		}
		foreach (KeyValuePair<int, int> current6 in this.petDataDict)
		{
			if (Globals.Instance.AttDB.PetDict.GetInfo(current6.Key) == null)
			{
				global::Debug.LogErrorFormat("PetDict.GetInfo error, id = {0}", new object[]
				{
					current6.Key
				});
			}
			else
			{
				RewardData rewardData = new RewardData();
				rewardData.RewardType = 4;
				rewardData.RewardValue1 = current6.Key;
				rewardData.RewardValue2 = current6.Value;
				this.mContentsTable.AddData(new RecycleGetItemData(rewardData));
				this.tempDataList.Add(rewardData);
			}
		}
		foreach (KeyValuePair<int, int> current7 in this.lopetDataDict)
		{
			if (Globals.Instance.AttDB.LopetDict.GetInfo(current7.Key) == null)
			{
				global::Debug.LogErrorFormat("LopetDict get info error , id = {0}", new object[]
				{
					current7.Key
				});
			}
			else
			{
				RewardData rewardData = new RewardData();
				rewardData.RewardType = 16;
				rewardData.RewardValue1 = current7.Key;
				rewardData.RewardValue2 = current7.Value;
				this.mContentsTable.AddData(new RecycleGetItemData(rewardData));
				this.tempDataList.Add(rewardData);
			}
		}
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private void AddItemData(int id, int count)
	{
		if (this.itemDataDict.ContainsKey(id))
		{
			Dictionary<int, int> dictionary;
			Dictionary<int, int> expr_17 = dictionary = this.itemDataDict;
			int num = dictionary[id];
			expr_17[id] = num + count;
		}
		else
		{
			this.itemDataDict.Add(id, count);
		}
	}

	private void AddPetData(int id, int count)
	{
		if (this.petDataDict.ContainsKey(id))
		{
			Dictionary<int, int> dictionary;
			Dictionary<int, int> expr_17 = dictionary = this.petDataDict;
			int num = dictionary[id];
			expr_17[id] = num + count;
		}
		else
		{
			this.petDataDict.Add(id, count);
		}
	}

	private void AddLopetData(int id, int count)
	{
		if (this.lopetDataDict.ContainsKey(id))
		{
			Dictionary<int, int> dictionary;
			Dictionary<int, int> expr_17 = dictionary = this.lopetDataDict;
			int num = dictionary[id];
			expr_17[id] = num + count;
		}
		else
		{
			this.lopetDataDict.Add(id, count);
		}
	}

	private void AddRewardData(ERewardType type, int count)
	{
		if (this.rewardDataDict.ContainsKey(type))
		{
			this.rewardDataDict[type].RewardValue1 += count;
		}
		else
		{
			RewardData rewardData = new RewardData();
			rewardData.RewardType = (int)type;
			rewardData.RewardValue1 = count;
			this.rewardDataDict.Add(type, rewardData);
		}
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	public void OnOKClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIPopupManager.GetInstance().PopState(false, null);
		if (this.OKEvent != null)
		{
			this.OKEvent(this.tempDataList);
		}
	}

	private void OnCancelClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	public override void OnButtonBlockerClick()
	{
	}
}
