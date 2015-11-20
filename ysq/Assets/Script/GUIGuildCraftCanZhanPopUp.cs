using System;
using UnityEngine;

public class GUIGuildCraftCanZhanPopUp : GameUIBasePopup
{
	public static void ShowMe()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildCraftCanZhanPopUp, false, null, null);
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
	}

	private void OnCloseClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
