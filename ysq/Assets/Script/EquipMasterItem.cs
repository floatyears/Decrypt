using System;
using UnityEngine;

public class EquipMasterItem : MonoBehaviour
{
	private GUIEquipMasterInfoPopUp mBaseScene;

	private ItemDataEx mData;

	private bool isEnhance;

	private int masterCurLevel;

	private CommonIconItem mEquipIconItem;

	private UILabel mName;

	private UISlider mSlider;

	private UILabel mValue;

	private int fullLevel;

	private int curLevel;

	public void Init(GUIEquipMasterInfoPopUp baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mEquipIconItem = CommonIconItem.Create(base.gameObject, new Vector3(-161f, 38f, 0f), null, false, 0.8f, null);
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mSlider = GameUITools.FindGameObject("Bar", base.gameObject).GetComponent<UISlider>();
		this.mValue = GameUITools.FindUILabel("Value", this.mSlider.gameObject);
	}

	public void Refresh(ItemDataEx data, bool enhance, int masterLevel)
	{
		this.mData = data;
		this.isEnhance = enhance;
		this.masterCurLevel = masterLevel;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mData == null)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.gameObject.SetActive(true);
			this.mEquipIconItem.Refresh(this.mData, false, false, false);
			this.mName.text = this.mData.Info.Name;
			this.mName.color = Tools.GetItemQualityColor(this.mData.Info.Quality);
			if (this.mData.Info.Type == 0)
			{
				if (this.isEnhance)
				{
					this.curLevel = this.mData.GetEquipEnhanceLevel();
					this.fullLevel = Master.GetEquipEnhanceLevel(this.masterCurLevel + 1);
				}
				else
				{
					this.curLevel = this.mData.GetEquipRefineLevel();
					this.fullLevel = Master.GetEquipRefineLevel(this.masterCurLevel + 1);
				}
			}
			else if (this.mData.Info.Type == 1)
			{
				if (this.isEnhance)
				{
					this.curLevel = this.mData.GetTrinketEnhanceLevel();
					this.fullLevel = Master.GetTrinketEnhanceLevel(this.masterCurLevel + 1);
				}
				else
				{
					this.curLevel = this.mData.GetTrinketRefineLevel();
					this.fullLevel = Master.GetTrinketRefineLevel(this.masterCurLevel + 1);
				}
			}
			else
			{
				global::Debug.LogErrorFormat("item type should be Equip or Trinket , ID : {0} , Type : {1} ", new object[]
				{
					this.mData.GetID(),
					this.mData.Info.Type
				});
			}
			this.mValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				this.curLevel,
				this.fullLevel
			});
			this.mSlider.value = ((this.fullLevel != 0) ? ((float)this.curLevel / (float)this.fullLevel) : 1f);
		}
	}

	private void OnClick()
	{
		if (this.mData.Info.Type == 0)
		{
			if (this.isEnhance)
			{
				if (this.mData.CanEnhance())
				{
					GUIEquipUpgradeScene.Change2This(this.mData, GUIEquipUpgradeScene.EUpgradeType.EUT_Enhance, this.mBaseScene.mSocketData.GetPet().GetSocketSlot());
					this.mBaseScene.CloseImmediate();
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTipByKey("equipImprove28", 0f, 0f);
				}
			}
			else if (Globals.Instance.Player.ItemSystem.CanEquipRefine())
			{
				if (this.mData.CanRefine())
				{
					GameUIManager.mInstance.uiState.SelectItemID = this.mData.Data.ID;
					GUIEquipUpgradeScene.Change2This(this.mData, GUIEquipUpgradeScene.EUpgradeType.EUT_Refine, this.mBaseScene.mSocketData.GetPet().GetSocketSlot());
					this.mBaseScene.CloseImmediate();
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTipByKey("equipImprove27", 0f, 0f);
				}
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("equipImprove43", new object[]
				{
					GameConst.GetInt32(11)
				}), 0f, 0f);
			}
		}
		else if (this.mData.Info.Type == 1)
		{
			if (this.isEnhance)
			{
				if (this.mData.CanEnhance())
				{
					GUITrinketUpgradeScene.Change2This(this.mData, GUITrinketUpgradeScene.EUpgradeType.EUT_Enhance, this.mBaseScene.mSocketData.GetPet().GetSocketSlot());
					this.mBaseScene.CloseImmediate();
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTipByKey("equipImprove28", 0f, 0f);
				}
			}
			else if (Globals.Instance.Player.ItemSystem.CanTrinketRefine())
			{
				if (this.mData.CanRefine())
				{
					GUITrinketUpgradeScene.Change2This(this.mData, GUITrinketUpgradeScene.EUpgradeType.EUT_Refine, this.mBaseScene.mSocketData.GetPet().GetSocketSlot());
					this.mBaseScene.CloseImmediate();
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTipByKey("equipImprove27", 0f, 0f);
				}
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("equipImprove44", new object[]
				{
					GameConst.GetInt32(13)
				}), 0f, 0f);
			}
		}
		else
		{
			global::Debug.LogErrorFormat("item type should be Equip or Trinket , ID : {0} , Type : {1} ", new object[]
			{
				this.mData.GetID(),
				this.mData.Info.Type
			});
		}
	}
}
