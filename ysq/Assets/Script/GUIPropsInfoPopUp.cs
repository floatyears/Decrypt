using Att;
using System;
using UnityEngine;

public class GUIPropsInfoPopUp : GameUIBasePopup
{
	private CommonIconItem mEquipIconItem;

	private UILabel mName;

	private UILabel mNumValue;

	private UILabel mDesc;

	public static void Show(ItemDataEx data)
	{
		if (data == null || data.Info == null)
		{
			global::Debug.LogError(new object[]
			{
				"ItemDataEx is null"
			});
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIPropsInfoPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(data);
	}

	public static void Show(ItemInfo info)
	{
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				"ItemInfo is null"
			});
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIPropsInfoPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(info);
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mEquipIconItem = CommonIconItem.Create(base.gameObject, new Vector3(-171f, 75f), null, false, 0.8f, null);
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mNumValue = GameUITools.FindUILabel("Num/Value", base.gameObject);
		this.mDesc = GameUITools.FindUILabel("Desc", base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseBtnClick), base.gameObject);
	}

	public override void InitPopUp(ItemDataEx data)
	{
		if (data == null)
		{
			return;
		}
		this.Refresh(data.Info);
	}

	public override void InitPopUp(ItemInfo info)
	{
		if (info == null)
		{
			return;
		}
		this.Refresh(info);
	}

	private void Refresh(ItemInfo info)
	{
		this.mEquipIconItem.Refresh(info, false, false, false);
		this.mName.text = info.Name;
		this.mName.color = Tools.GetItemQualityColor(info.Quality);
		this.mNumValue.text = Globals.Instance.Player.ItemSystem.GetItemCount(info.ID).ToString();
		this.mDesc.text = info.Desc;
	}

	private void OnCloseBtnClick(GameObject go)
	{
		base.OnButtonBlockerClick();
	}
}
