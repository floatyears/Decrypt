using System;
using UnityEngine;

public class SystemSettingNotifyLayer : MonoBehaviour
{
	private UITable mBtnsTable;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
		this.mBtnsTable.repositionNow = true;
	}

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("contentsPanel/contents").gameObject;
		this.mBtnsTable = gameObject.GetComponent<UITable>();
		this.mBtnsTable.columns = 1;
		this.mBtnsTable.direction = UITable.Direction.Down;
		this.mBtnsTable.sorting = UITable.Sorting.Alphabetic;
		this.mBtnsTable.hideInactive = true;
		this.mBtnsTable.keepWithinPanel = true;
		this.mBtnsTable.padding = new Vector2(0f, 6f);
		GUICommonSwitchBtn gUICommonSwitchBtn = gameObject.transform.Find("set0/takeKey12Switch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn.InitSwithBtn(GameSetting.Data.TwelveEnergy);
		GUICommonSwitchBtn expr_A4 = gUICommonSwitchBtn;
		expr_A4.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_A4.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnTakeKey12SwitchChanged));
		GUICommonSwitchBtn gUICommonSwitchBtn2 = gameObject.transform.Find("set0/takeKey18Switch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn2.InitSwithBtn(GameSetting.Data.EighteenEnergy);
		GUICommonSwitchBtn expr_F1 = gUICommonSwitchBtn2;
		expr_F1.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_F1.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnTakeKey18SwitchChanged));
		GUICommonSwitchBtn gUICommonSwitchBtn3 = gameObject.transform.Find("set1/keyFullSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn3.InitSwithBtn(GameSetting.Data.EnergyFull);
		GUICommonSwitchBtn expr_13E = gUICommonSwitchBtn3;
		expr_13E.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_13E.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnKeyFullSwitchChanged));
		GUICommonSwitchBtn gUICommonSwitchBtn4 = gameObject.transform.Find("set1/takeKey21Switch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn4.InitSwithBtn(GameSetting.Data.TwentyOneEnergy);
		GUICommonSwitchBtn expr_18E = gUICommonSwitchBtn4;
		expr_18E.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_18E.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnTakeKey21SwitchChanged));
		GUICommonSwitchBtn gUICommonSwitchBtn5 = gameObject.transform.Find("set2/petShopSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn5.InitSwithBtn(GameSetting.Data.PetShopRefresh);
		GUICommonSwitchBtn expr_1DE = gUICommonSwitchBtn5;
		expr_1DE.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_1DE.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnPetShopSwitchChanged));
		GUICommonSwitchBtn gUICommonSwitchBtn6 = gameObject.transform.Find("set2/jueXingShopSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn6.InitSwithBtn(GameSetting.Data.JueXingShopRefresh);
		GUICommonSwitchBtn expr_22E = gUICommonSwitchBtn6;
		expr_22E.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_22E.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnJueXingShopSwitchChanged));
		gameObject.transform.Find("set2/jueXingShop").gameObject.SetActive(Tools.CanPlay(GameConst.GetInt32(24), true));
		gUICommonSwitchBtn6.gameObject.SetActive(Tools.CanPlay(GameConst.GetInt32(24), true));
		GUICommonSwitchBtn gUICommonSwitchBtn7 = gameObject.transform.Find("set3/worldBossSwitch").gameObject.AddComponent<GUICommonSwitchBtn>();
		gUICommonSwitchBtn7.InitSwithBtn(GameSetting.Data.WorldBossNotify);
		GUICommonSwitchBtn expr_2BE = gUICommonSwitchBtn7;
		expr_2BE.BtnSwithCallbackEvent = (GUICommonSwitchBtn.BtnSwithCallback)Delegate.Combine(expr_2BE.BtnSwithCallbackEvent, new GUICommonSwitchBtn.BtnSwithCallback(this.OnWorldBossSwitchChanged));
	}

	private void OnWorldBossSwitchChanged(bool isOpen)
	{
		if (GameSetting.Data.WorldBossNotify != isOpen)
		{
			GameSetting.Data.WorldBossNotify = isOpen;
			GameSetting.UpdateNow = true;
		}
	}

	private void OnPetShopSwitchChanged(bool isOpen)
	{
		if (GameSetting.Data.PetShopRefresh != isOpen)
		{
			GameSetting.Data.PetShopRefresh = isOpen;
			GameSetting.UpdateNow = true;
		}
	}

	private void OnJueXingShopSwitchChanged(bool isOpen)
	{
		if (GameSetting.Data.JueXingShopRefresh != isOpen)
		{
			GameSetting.Data.JueXingShopRefresh = isOpen;
			GameSetting.UpdateNow = true;
		}
	}

	private void OnTakeKey12SwitchChanged(bool isOpen)
	{
		if (GameSetting.Data.TwelveEnergy != isOpen)
		{
			GameSetting.Data.TwelveEnergy = isOpen;
			GameSetting.UpdateNow = true;
		}
	}

	private void OnTakeKey18SwitchChanged(bool isOpen)
	{
		if (GameSetting.Data.EighteenEnergy != isOpen)
		{
			GameSetting.Data.EighteenEnergy = isOpen;
			GameSetting.UpdateNow = true;
		}
	}

	private void OnTakeKey21SwitchChanged(bool isOpen)
	{
		if (GameSetting.Data.TwentyOneEnergy != isOpen)
		{
			GameSetting.Data.TwentyOneEnergy = isOpen;
			GameSetting.UpdateNow = true;
		}
	}

	private void OnKeyFullSwitchChanged(bool isOpen)
	{
		if (GameSetting.Data.EnergyFull != isOpen)
		{
			GameSetting.Data.EnergyFull = isOpen;
			GameSetting.UpdateNow = true;
		}
	}
}
