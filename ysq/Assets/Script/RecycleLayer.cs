using Holoville.HOTween;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class RecycleLayer : MonoBehaviour
{
	private GUIRecycleScene mBaseScene;

	private GameObject mPoints;

	private int tempCurrency;

	private UILabel mCurrency;

	private UISprite mCurrencyIcon;

	private UILabel mCurrencyName;

	public UIButton mShop;

	private UILabel mShopName;

	private RecycleItem mRebornItem;

	public RecycleItem[] mBreakItems = new RecycleItem[5];

	private UILabel mRules;

	private GameObject mAutoAddBtn;

	public GameObject mBreakBtn;

	private UILabel mCost;

	private GameObject mRebornBtn;

	private GameObject ui60;

	private bool isBreaking;

	private Sequence mBreakSeq;

	private List<RewardData> tempRewardDatas;

	public void InitWithBaseScene(GUIRecycleScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.ui60 = GameUITools.FindGameObject("ui60", base.gameObject);
		Tools.SetParticleRQWithUIScale(this.ui60, 3010);
		this.ui60.gameObject.SetActive(false);
		Tools.SetParticleRQWithUIScale(GameUITools.FindGameObject("ui60_1", base.gameObject), 3002);
		this.mPoints = GameUITools.FindGameObject("Points", base.gameObject);
		this.mCurrency = GameUITools.FindUILabel("Currency/Value", base.gameObject);
		this.mCurrencyIcon = GameUITools.FindUISprite("Icon", this.mCurrency.transform.parent.gameObject);
		this.mCurrencyName = GameUITools.FindUILabel("Name", this.mCurrency.transform.parent.gameObject);
		this.mShop = GameUITools.RegisterClickEvent("Shop", new UIEventListener.VoidDelegate(this.OnShopClick), base.gameObject).GetComponent<UIButton>();
		this.mShopName = GameUITools.FindUILabel("Name", this.mShop.gameObject);
		this.mRebornItem = GameUITools.FindGameObject("RebornItem", base.gameObject).AddComponent<RecycleItem>();
		this.mRebornItem.InitWithBaseScene(this.mBaseScene);
		Transform transform = GameUITools.FindGameObject("BreakItems", base.gameObject).transform;
		if (transform.childCount != 5)
		{
			global::Debug.LogError(new object[]
			{
				"BreakItems count error"
			});
			this.mBreakItems = new RecycleItem[transform.childCount];
		}
		int num = 0;
		while (num < this.mBreakItems.Length && num < transform.childCount)
		{
			this.mBreakItems[num] = transform.GetChild(num).gameObject.AddComponent<RecycleItem>();
			this.mBreakItems[num].InitWithBaseScene(this.mBaseScene);
			num++;
		}
		this.mRules = GameUITools.RegisterClickEvent("Rules", new UIEventListener.VoidDelegate(this.OnRulesClick), base.gameObject).GetComponent<UILabel>();
		this.mAutoAddBtn = GameUITools.RegisterClickEvent("AutoAddBtn", new UIEventListener.VoidDelegate(this.OnAutoAddBtnClick), base.gameObject);
		this.mBreakBtn = GameUITools.RegisterClickEvent("BreakBtn", new UIEventListener.VoidDelegate(this.OnBreakBtnClick), base.gameObject);
		this.mCost = GameUITools.FindUILabel("Cost", base.gameObject);
		this.mRebornBtn = GameUITools.RegisterClickEvent("RebornBtn", new UIEventListener.VoidDelegate(this.OnRebornBtnClick), base.gameObject);
	}

	public void PreDestroy()
	{
		HOTween.Kill(this.mBreakSeq);
		base.StopCoroutine("PlayBreakAnim");
		base.StopCoroutine("PlayAnimUI60");
		base.StopCoroutine("ShowRebornBox");
	}

	public void OnShopClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.EquipBreakUpData = null;
		GameUIManager.mInstance.uiState.TrinketRebornData = null;
		GameUIManager.mInstance.uiState.PetBreakUpData = null;
		GameUIManager.mInstance.uiState.PetRebornData = null;
		GameUIManager.mInstance.uiState.LopetBreakData = null;
		GameUIManager.mInstance.uiState.LopetRebornData = null;
		GameUIManager.mInstance.uiState.RecycleType = this.mBaseScene.mCurType;
		switch (this.mBaseScene.mCurType)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetBreak:
			GUIShopScene.TryOpen(EShopType.EShop_Common2);
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak:
			GUIShopScene.TryOpen(EShopType.EShop_Trial);
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_PetReborn:
			GUIShopScene.TryOpen(EShopType.EShop_Common2);
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak:
			GUIShopScene.TryOpen(EShopType.EShop_Lopet);
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn:
			GUIShopScene.TryOpen(EShopType.EShop_Lopet);
			break;
		}
	}

	private void OnRulesClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		switch (this.mBaseScene.mCurType)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetBreak:
			GameUIRuleInfoPopUp.ShowThis("recycle14", (!Tools.CanPlay(GameConst.GetInt32(24), true)) ? "recycle10" : "recycle100");
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak:
			GameUIRuleInfoPopUp.ShowThis("recycle15", "recycle11");
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_PetReborn:
			GameUIRuleInfoPopUp.ShowThis("recycle16", (!Tools.CanPlay(GameConst.GetInt32(24), true)) ? "recycle12" : "recycle120");
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn:
			GameUIRuleInfoPopUp.ShowThis("recycle17", "recycle13");
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak:
			GameUIRuleInfoPopUp.ShowThis("recycle45", "recycle43");
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn:
			GameUIRuleInfoPopUp.ShowThis("recycle46", "recycle44");
			break;
		}
	}

	public void OnAutoAddBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.isBreaking)
		{
			return;
		}
		GUIRecycleScene.ERecycleT mCurType = this.mBaseScene.mCurType;
		if (mCurType != GUIRecycleScene.ERecycleT.ERecycleT_PetBreak)
		{
			if (mCurType == GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak)
			{
				bool flag = false;
				List<ItemDataEx> list;
				Globals.Instance.Player.ItemSystem.GetAllEquip2BreakUp(out list, out flag);
				if (list.Count > 0)
				{
					list.Sort(new Comparison<ItemDataEx>(EquipBreakSelectItemBagUITable.Sort));
					MC2S_EquipBreakUp equipBreakUpData = GameUIManager.mInstance.uiState.EquipBreakUpData;
					equipBreakUpData.EquipID.Clear();
					int num = 0;
					while (num < 5 && num < list.Count)
					{
						equipBreakUpData.EquipID.Add(list[num].GetID());
						num++;
					}
					this.InitData();
				}
				else if (flag)
				{
					GameUIManager.mInstance.ShowMessageTipByKey("recycle34", 0f, 0f);
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTipByKey("recycle30", 0f, 0f);
				}
			}
		}
		else
		{
			List<PetDataEx> list2;
			bool flag2;
			Globals.Instance.Player.PetSystem.GetAllPet2BreakUp(out list2, out flag2);
			if (list2.Count > 0)
			{
				list2.Sort(new Comparison<PetDataEx>(GUILvlUpSelectItemTable.SortByPetDatas2));
				MC2S_PetBreakUp petBreakUpData = GameUIManager.mInstance.uiState.PetBreakUpData;
				petBreakUpData.PetID.Clear();
				int num2 = 0;
				while (num2 < 5 && num2 < list2.Count)
				{
					petBreakUpData.PetID.Add(list2[num2].GetID());
					num2++;
				}
				this.InitData();
			}
			else if (flag2)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle33", 0f, 0f);
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle29", 0f, 0f);
			}
		}
	}

	public void OnBreakBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		switch (this.mBaseScene.mCurType)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetBreak:
			if (GameUIManager.mInstance.uiState.PetBreakUpData.PetID.Count <= 0)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle23", 0f, 0f);
				return;
			}
			GUIRecycleGetItemsPopUp.ShowThis(this.mBaseScene.mCurType, new GUIRecycleGetItemsPopUp.VoidCallBack(this.SendPetBreakUpMsg));
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak:
			if (GameUIManager.mInstance.uiState.EquipBreakUpData.EquipID.Count <= 0)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle24", 0f, 0f);
				return;
			}
			GUIRecycleGetItemsPopUp.ShowThis(this.mBaseScene.mCurType, new GUIRecycleGetItemsPopUp.VoidCallBack(this.SendEquipBreakUpMsg));
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak:
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(216), 0))
			{
				return;
			}
			if (GameUIManager.mInstance.uiState.LopetBreakData.LopetID.Count <= 0)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle37", 0f, 0f);
				return;
			}
			GUIRecycleGetItemsPopUp.ShowThis(this.mBaseScene.mCurType, new GUIRecycleGetItemsPopUp.VoidCallBack(this.SendLopetBreakMsg));
			break;
		}
	}

	private void PlayEffectAnim()
	{
		this.mBreakItems[0].transform.localPosition = new Vector3(228f, -150f, 0f);
		this.mBreakItems[0].transform.localScale = Vector3.one;
		this.mBreakItems[1].transform.localPosition = new Vector3(94f, 104f, 0f);
		this.mBreakItems[1].transform.localScale = Vector3.one;
		this.mBreakItems[2].transform.localPosition = new Vector3(379f, 104f, 0f);
		this.mBreakItems[2].transform.localScale = Vector3.one;
		this.mBreakItems[3].transform.localPosition = new Vector3(-38f, -40f, 0f);
		this.mBreakItems[3].transform.localScale = Vector3.one;
		this.mBreakItems[4].transform.localPosition = new Vector3(511f, -40f, 0f);
		this.mBreakItems[4].transform.localScale = Vector3.one;
		this.PlayAnim();
	}

	[DebuggerHidden]
	private IEnumerator PlayBreakAnim()
	{
        return null;
        //RecycleLayer.<PlayBreakAnim>c__Iterator47 <PlayBreakAnim>c__Iterator = new RecycleLayer.<PlayBreakAnim>c__Iterator47();
        //<PlayBreakAnim>c__Iterator.<>f__this = this;
        //return <PlayBreakAnim>c__Iterator;
	}

	private void PlayAnim()
	{
		GameUIManager.mInstance.uiState.PetBreakUpData.PetID.Clear();
		GameUIManager.mInstance.uiState.EquipBreakUpData.EquipID.Clear();
		GameUIManager.mInstance.uiState.TrinketRebornData.TrinketID = 0uL;
		GameUIManager.mInstance.uiState.PetRebornData.PetID = 0uL;
		GameUIManager.mInstance.uiState.LopetBreakData.LopetID.Clear();
		GameUIManager.mInstance.uiState.LopetRebornData.LopetID = 0uL;
		this.InitData();
		base.StartCoroutine("PlayAnimUI60");
	}

	[DebuggerHidden]
	private IEnumerator PlayAnimUI60()
	{
        return null;
        //RecycleLayer.<PlayAnimUI60>c__Iterator48 <PlayAnimUI60>c__Iterator = new RecycleLayer.<PlayAnimUI60>c__Iterator48();
        //<PlayAnimUI60>c__Iterator.<>f__this = this;
        //return <PlayAnimUI60>c__Iterator;
	}

	private void ShowRewardsBox()
	{
		GameUIManager.mInstance.uiState.PetBreakUpData.PetID.Clear();
		GameUIManager.mInstance.uiState.EquipBreakUpData.EquipID.Clear();
		GameUIManager.mInstance.uiState.TrinketRebornData.TrinketID = 0uL;
		GameUIManager.mInstance.uiState.PetRebornData.PetID = 0uL;
		GameUIManager.mInstance.uiState.LopetBreakData.LopetID.Clear();
		GameUIManager.mInstance.uiState.LopetRebornData.LopetID = 0uL;
		this.InitData();
		switch (this.mBaseScene.mCurType)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetBreak:
		case GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak:
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak:
			GUIRewardPanel.Show(this.tempRewardDatas, Singleton<StringManager>.Instance.GetString("recycle27"), false, true, null, false);
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_PetReborn:
		case GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn:
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn:
			GUIRewardPanel.Show(this.tempRewardDatas, Singleton<StringManager>.Instance.GetString("recycle28"), false, true, null, false);
			break;
		}
		this.isBreaking = false;
	}

	private void SendEquipBreakUpMsg(List<RewardData> datas)
	{
		Globals.Instance.CliSession.Send(530, GameUIManager.mInstance.uiState.EquipBreakUpData);
		this.tempRewardDatas = datas;
	}

	public void OnMsgEquipBreakUp(MemoryStream stream)
	{
		MS2C_EquipBreakUp mS2C_EquipBreakUp = Serializer.NonGeneric.Deserialize(typeof(MS2C_EquipBreakUp), stream) as MS2C_EquipBreakUp;
		if (mS2C_EquipBreakUp.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_EquipBreakUp.Result);
			return;
		}
		this.mBaseScene.RefreshEquipBreakRed();
		base.StartCoroutine("PlayBreakAnim");
	}

	private void SendPetBreakUpMsg(List<RewardData> datas)
	{
		Globals.Instance.CliSession.Send(413, GameUIManager.mInstance.uiState.PetBreakUpData);
		this.tempRewardDatas = datas;
	}

	public void OnPetBreakUpEvent()
	{
		if (Globals.Instance.TutorialMgr.IsNull)
		{
			Tutorial_Recycle.PassThis();
		}
		this.mBaseScene.RefreshPetBreakRed();
		base.StartCoroutine("PlayBreakAnim");
	}

	private void OnRebornBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		switch (this.mBaseScene.mCurType)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetReborn:
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(67), 0))
			{
				return;
			}
			if (GameUIManager.mInstance.uiState.PetRebornData.PetID <= 0uL)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle25", 0f, 0f);
				return;
			}
			if (Tools.IsPetBagFull())
			{
				return;
			}
			GUIRecycleGetItemsPopUp.ShowThis(this.mBaseScene.mCurType, new GUIRecycleGetItemsPopUp.VoidCallBack(this.SendPetRebornMsg));
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn:
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(67), 0))
			{
				return;
			}
			if (GameUIManager.mInstance.uiState.TrinketRebornData.TrinketID <= 0uL)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle26", 0f, 0f);
				return;
			}
			if (Tools.IsTrinketBagFull())
			{
				return;
			}
			GUIRecycleGetItemsPopUp.ShowThis(this.mBaseScene.mCurType, new GUIRecycleGetItemsPopUp.VoidCallBack(this.SendTrinketRebornMsg));
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn:
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(216), 0))
			{
				return;
			}
			if (GameUIManager.mInstance.uiState.LopetRebornData.LopetID <= 0uL)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("recycle38", 0f, 0f);
				return;
			}
			GUIRecycleGetItemsPopUp.ShowThis(this.mBaseScene.mCurType, new GUIRecycleGetItemsPopUp.VoidCallBack(this.SendLopetRebornMsg));
			break;
		}
	}

	private void SendTrinketRebornMsg(List<RewardData> datas)
	{
		Globals.Instance.CliSession.Send(532, GameUIManager.mInstance.uiState.TrinketRebornData);
		this.tempRewardDatas = datas;
	}

	public void OnMsgTrinketReborn(MemoryStream stream)
	{
		MS2C_TrinketReborn mS2C_TrinketReborn = Serializer.NonGeneric.Deserialize(typeof(MS2C_TrinketReborn), stream) as MS2C_TrinketReborn;
		if (mS2C_TrinketReborn.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_TrinketReborn.Result);
			return;
		}
		base.StartCoroutine("ShowRebornBox");
	}

	[DebuggerHidden]
	private IEnumerator ShowRebornBox()
	{
        return null;
        //RecycleLayer.<ShowRebornBox>c__Iterator49 <ShowRebornBox>c__Iterator = new RecycleLayer.<ShowRebornBox>c__Iterator49();
        //<ShowRebornBox>c__Iterator.<>f__this = this;
        //return <ShowRebornBox>c__Iterator;
	}

	private void SendPetRebornMsg(List<RewardData> datas)
	{
		Globals.Instance.CliSession.Send(415, GameUIManager.mInstance.uiState.PetRebornData);
		this.tempRewardDatas = datas;
	}

	public void OnMsgPetReborn(MemoryStream stream)
	{
		MS2C_PetReborn mS2C_PetReborn = Serializer.NonGeneric.Deserialize(typeof(MS2C_PetReborn), stream) as MS2C_PetReborn;
		if (mS2C_PetReborn.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PetR", mS2C_PetReborn.Result);
			return;
		}
		base.StartCoroutine("ShowRebornBox");
	}

	private void SendLopetBreakMsg(List<RewardData> datas)
	{
		Globals.Instance.CliSession.Send(1066, GameUIManager.mInstance.uiState.LopetBreakData);
		this.tempRewardDatas = datas;
	}

	public void OnLopetBreakUpEvent()
	{
		base.StartCoroutine("ShowRebornBox");
	}

	private void SendLopetRebornMsg(List<RewardData> datas)
	{
		Globals.Instance.CliSession.Send(1068, GameUIManager.mInstance.uiState.LopetRebornData);
		this.tempRewardDatas = datas;
	}

	public void OnLopetRebornEvent()
	{
		base.StartCoroutine("ShowRebornBox");
	}

	public void InitData()
	{
		switch (this.mBaseScene.mCurType)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetBreak:
		{
			MC2S_PetBreakUp petBreakUpData = GameUIManager.mInstance.uiState.PetBreakUpData;
			int i = 0;
			foreach (ulong current in petBreakUpData.PetID)
			{
				PetDataEx pet = Globals.Instance.Player.PetSystem.GetPet(current);
				if (i >= this.mBreakItems.Length)
				{
					break;
				}
				this.mBreakItems[i].Refresh(pet);
				i++;
			}
			while (i < this.mBreakItems.Length)
			{
				this.mBreakItems[i].Clear();
				i++;
			}
			break;
		}
		case GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak:
		{
			MC2S_EquipBreakUp equipBreakUpData = GameUIManager.mInstance.uiState.EquipBreakUpData;
			int j = 0;
			foreach (ulong current2 in equipBreakUpData.EquipID)
			{
				ItemDataEx item = Globals.Instance.Player.ItemSystem.GetItem(current2);
				if (j >= this.mBreakItems.Length)
				{
					break;
				}
				this.mBreakItems[j].Refresh(item);
				j++;
			}
			while (j < this.mBreakItems.Length)
			{
				this.mBreakItems[j].Clear();
				j++;
			}
			break;
		}
		case GUIRecycleScene.ERecycleT.ERecycleT_PetReborn:
			this.mRebornItem.Refresh(Globals.Instance.Player.PetSystem.GetPet(GameUIManager.mInstance.uiState.PetRebornData.PetID));
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn:
		{
			MC2S_TrinketReborn trinketRebornData = GameUIManager.mInstance.uiState.TrinketRebornData;
			this.mRebornItem.Refresh(Globals.Instance.Player.ItemSystem.GetItem(trinketRebornData.TrinketID));
			break;
		}
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak:
		{
			MC2S_LopetBreakUp lopetBreakData = GameUIManager.mInstance.uiState.LopetBreakData;
			int k = 0;
			foreach (ulong current3 in lopetBreakData.LopetID)
			{
				LopetDataEx lopet = Globals.Instance.Player.LopetSystem.GetLopet(current3);
				if (k >= 1)
				{
					break;
				}
				this.mRebornItem.Refresh(lopet);
				k++;
			}
			while (k < 1)
			{
				this.mRebornItem.Clear();
				k++;
			}
			break;
		}
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn:
			this.mRebornItem.Refresh(Globals.Instance.Player.LopetSystem.GetLopet(GameUIManager.mInstance.uiState.LopetRebornData.LopetID));
			break;
		}
	}

	private void ClearBreakItemsData()
	{
		for (int i = 0; i < this.mBreakItems.Length; i++)
		{
			this.mBreakItems[i].Clear();
		}
		GameUIManager.mInstance.uiState.PetBreakUpData.PetID.Clear();
		GameUIManager.mInstance.uiState.EquipBreakUpData.EquipID.Clear();
	}

	private void ClearRebornItemData()
	{
		this.mRebornItem.Clear();
		GameUIManager.mInstance.uiState.PetRebornData.PetID = 0uL;
		GameUIManager.mInstance.uiState.TrinketRebornData.TrinketID = 0uL;
		GameUIManager.mInstance.uiState.LopetBreakData.LopetID.Clear();
		GameUIManager.mInstance.uiState.LopetRebornData.LopetID = 0uL;
	}

	private void SetBreakItemsState(bool value)
	{
		for (int i = 0; i < this.mBreakItems.Length; i++)
		{
			this.mBreakItems[i].gameObject.SetActive(!value);
			this.mBreakItems[i].gameObject.SetActive(value);
		}
	}

	private void SetRebornItemState(bool value)
	{
		this.mRebornItem.gameObject.SetActive(!value);
		this.mRebornItem.gameObject.SetActive(value);
	}

	public void ClearData()
	{
		this.ClearBreakItemsData();
		this.ClearRebornItemData();
	}

	public void Refresh()
	{
		if (this.mBreakSeq != null)
		{
			this.mBreakSeq.Kill();
			this.mBreakSeq = null;
			this.PlayEffectAnim();
			this.isBreaking = false;
		}
		base.StopCoroutine("ShowRebornBox");
		base.StopCoroutine("PlayAnimUI60");
		this.ui60.SetActive(false);
		switch (this.mBaseScene.mCurType)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetBreak:
			this.mPoints.gameObject.SetActive(true);
			this.mCurrency.transform.parent.gameObject.SetActive(true);
			this.tempCurrency = Globals.Instance.Player.Data.MagicSoul;
			this.mCurrency.text = Tools.FormatCurrency(this.tempCurrency);
			this.mCurrencyIcon.spriteName = "magicSoul";
			this.mCurrencyName.text = Singleton<StringManager>.Instance.GetString("recycle2") + Singleton<StringManager>.Instance.GetString("Colon0");
			this.mShop.gameObject.SetActive(true);
			this.mShop.normalSprite = "petShopIcon";
			this.mShopName.text = Singleton<StringManager>.Instance.GetString("ShopN0");
			this.SetBreakItemsState(true);
			this.SetRebornItemState(false);
			this.mRules.text = Singleton<StringManager>.Instance.GetString("recycle6");
			this.mAutoAddBtn.gameObject.SetActive(true);
			this.mBreakBtn.gameObject.SetActive(true);
			this.mCost.gameObject.SetActive(false);
			this.mRebornBtn.gameObject.SetActive(false);
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak:
			this.mPoints.gameObject.SetActive(true);
			this.mCurrency.transform.parent.gameObject.SetActive(true);
			this.tempCurrency = Globals.Instance.Player.Data.MagicCrystal;
			this.mCurrency.text = Tools.FormatCurrency(this.tempCurrency);
			this.mCurrencyIcon.spriteName = "magicCrystal";
			this.mCurrencyName.text = Singleton<StringManager>.Instance.GetString("recycle3") + Singleton<StringManager>.Instance.GetString("Colon0");
			this.mShop.gameObject.SetActive(true);
			this.mShop.normalSprite = "equipShopIcon";
			this.mShopName.text = Singleton<StringManager>.Instance.GetString("ShopN4");
			this.SetBreakItemsState(true);
			this.SetRebornItemState(false);
			this.mRules.text = Singleton<StringManager>.Instance.GetString("recycle7");
			this.mAutoAddBtn.gameObject.SetActive(true);
			this.mBreakBtn.gameObject.SetActive(true);
			this.mCost.gameObject.SetActive(false);
			this.mRebornBtn.gameObject.SetActive(false);
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_PetReborn:
			this.mPoints.gameObject.SetActive(false);
			this.mCurrency.transform.parent.gameObject.SetActive(false);
			this.mShop.gameObject.SetActive(true);
			this.mShop.normalSprite = "petShopIcon";
			this.mShopName.text = Singleton<StringManager>.Instance.GetString("ShopN0");
			this.SetBreakItemsState(false);
			this.SetRebornItemState(true);
			this.mRules.text = Singleton<StringManager>.Instance.GetString("recycle8");
			this.mAutoAddBtn.gameObject.SetActive(false);
			this.mBreakBtn.gameObject.SetActive(false);
			this.mCost.gameObject.SetActive(true);
			this.mCost.text = GameConst.GetInt32(67).ToString();
			this.mRebornBtn.gameObject.SetActive(true);
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_TrinketReborn:
			this.mPoints.gameObject.SetActive(false);
			this.mCurrency.transform.parent.gameObject.SetActive(false);
			this.mShop.gameObject.SetActive(false);
			this.SetBreakItemsState(false);
			this.SetRebornItemState(true);
			this.mRules.text = Singleton<StringManager>.Instance.GetString("recycle9");
			this.mAutoAddBtn.gameObject.SetActive(false);
			this.mBreakBtn.gameObject.SetActive(false);
			this.mCost.gameObject.SetActive(true);
			this.mCost.text = GameConst.GetInt32(67).ToString();
			this.mRebornBtn.gameObject.SetActive(true);
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak:
			this.mPoints.gameObject.SetActive(false);
			this.mCurrency.transform.parent.gameObject.SetActive(true);
			this.tempCurrency = Globals.Instance.Player.Data.LopetSoul;
			this.mCurrency.text = Tools.FormatCurrency(this.tempCurrency);
			this.mCurrencyIcon.spriteName = "lopetSoul";
			this.mCurrencyName.text = Singleton<StringManager>.Instance.GetString("lopetSoul") + Singleton<StringManager>.Instance.GetString("Colon0");
			this.mShop.gameObject.SetActive(true);
			this.mShop.normalSprite = "lopetShop";
			this.SetBreakItemsState(false);
			this.SetRebornItemState(true);
			this.mRules.text = Singleton<StringManager>.Instance.GetString("recycle41");
			this.mAutoAddBtn.gameObject.SetActive(false);
			this.mBreakBtn.gameObject.SetActive(true);
			this.mCost.gameObject.SetActive(true);
			this.mCost.text = GameConst.GetInt32(216).ToString();
			this.mRebornBtn.gameObject.SetActive(false);
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn:
			this.mPoints.gameObject.SetActive(false);
			this.mCurrency.transform.parent.gameObject.SetActive(true);
			this.tempCurrency = Globals.Instance.Player.Data.LopetSoul;
			this.mCurrency.text = Tools.FormatCurrency(this.tempCurrency);
			this.mCurrencyIcon.spriteName = "lopetSoul";
			this.mCurrencyName.text = Singleton<StringManager>.Instance.GetString("lopetSoul") + Singleton<StringManager>.Instance.GetString("Colon0");
			this.mShop.gameObject.SetActive(true);
			this.mShop.normalSprite = "lopetShop";
			this.SetBreakItemsState(false);
			this.SetRebornItemState(true);
			this.mRules.text = Singleton<StringManager>.Instance.GetString("recycle42");
			this.mAutoAddBtn.gameObject.SetActive(false);
			this.mBreakBtn.gameObject.SetActive(false);
			this.mCost.gameObject.SetActive(true);
			this.mCost.text = GameConst.GetInt32(216).ToString();
			this.mRebornBtn.gameObject.SetActive(true);
			break;
		}
	}

	public List<PetDataEx> GetPetBreakDatas()
	{
		List<PetDataEx> list = new List<PetDataEx>();
		RecycleItem[] array = this.mBreakItems;
		for (int i = 0; i < array.Length; i++)
		{
			RecycleItem recycleItem = array[i];
			if (recycleItem.petData != null)
			{
				list.Add(recycleItem.petData);
			}
		}
		return list;
	}

	public List<ItemDataEx> GetEquipBreakDatas()
	{
		List<ItemDataEx> list = new List<ItemDataEx>();
		RecycleItem[] array = this.mBreakItems;
		for (int i = 0; i < array.Length; i++)
		{
			RecycleItem recycleItem = array[i];
			if (recycleItem.itemData != null)
			{
				list.Add(recycleItem.itemData);
			}
		}
		return list;
	}

	public PetDataEx GetPetRebornData()
	{
		return this.mRebornItem.petData;
	}

	public ItemDataEx GetTrinketRebornData()
	{
		return this.mRebornItem.itemData;
	}

	public void OnPlayerUpdateEvent()
	{
		switch (this.mBaseScene.mCurType)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetBreak:
			if (this.tempCurrency != Globals.Instance.Player.Data.MagicSoul)
			{
				this.PlayCurrencyAnim();
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak:
			if (this.tempCurrency != Globals.Instance.Player.Data.MagicCrystal)
			{
				this.PlayCurrencyAnim();
			}
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak:
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn:
			if (this.tempCurrency != Globals.Instance.Player.Data.LopetSoul)
			{
				this.PlayCurrencyAnim();
			}
			break;
		}
	}

	private void PlayCurrencyAnim()
	{
		this.RefreshCurrencyValue();
		Sequence sequence = new Sequence();
		sequence.Append(HOTween.To(this.mCurrency.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
		sequence.Append(HOTween.To(this.mCurrency.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
		sequence.Play();
	}

	private void RefreshCurrencyValue()
	{
		switch (this.mBaseScene.mCurType)
		{
		case GUIRecycleScene.ERecycleT.ERecycleT_PetBreak:
			this.tempCurrency = Globals.Instance.Player.Data.MagicSoul;
			this.mCurrency.text = this.tempCurrency.ToString();
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak:
			this.tempCurrency = Globals.Instance.Player.Data.MagicCrystal;
			this.mCurrency.text = this.tempCurrency.ToString();
			break;
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak:
		case GUIRecycleScene.ERecycleT.ERecycleT_LopetReborn:
			this.tempCurrency = Globals.Instance.Player.Data.LopetSoul;
			this.mCurrency.text = this.tempCurrency.ToString();
			break;
		}
	}
}
