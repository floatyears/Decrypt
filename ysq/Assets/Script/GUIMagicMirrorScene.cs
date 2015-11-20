using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIMagicMirrorScene : GameUISession
{
	private MagicMirrorSelectPetPopUp mSelectPop;

	private UIButton mChangeBtn;

	private UIButton[] mChangeBtns;

	private GameObject mChangeBtnEffect;

	private CommonIconItem mInIcon;

	private GameObject mInAdd;

	private GameObject mInTips;

	private GameObject mInInfo;

	private UILabel mInName;

	private UILabel mInLevel;

	private GUIStars mInStars;

	private GUIAttributeValue mInValues;

	private GUIPetSkills mInSkills;

	private CommonIconItem mOutIcon;

	private GameObject mOutAdd;

	private GameObject mOutTips;

	private GameObject mOutInfo;

	private UILabel mOutName;

	private UILabel mOutLevel;

	private GUIStars mOutStars;

	private GUIAttributeValue mOutValues;

	private GUIPetSkills mOutSkills;

	private GameObject mCost;

	private UILabel mDiamond;

	private UILabel mMagicSoul;

	private PetDataEx mCurPetData;

	private PetDataEx mTargetPet;

	[NonSerialized]
	public bool setIn = true;

	private int diamond;

	private int magicSoul;

	protected override void OnPostLoadGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Show("MirrorLb");
		this.CreateObjects();
		LocalPlayer expr_24 = Globals.Instance.Player;
		expr_24.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_24.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		Globals.Instance.CliSession.Register(426, new ClientSession.MsgHandler(this.OnMsgPetExchange));
	}

	protected override void OnPreDestroyGUI()
	{
		LocalPlayer expr_0A = Globals.Instance.Player;
		expr_0A.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_0A.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		Globals.Instance.CliSession.Unregister(426, new ClientSession.MsgHandler(this.OnMsgPetExchange));
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	private void CreateObjects()
	{
		this.mSelectPop = GameUITools.FindGameObject("SelectPetPopUp", base.gameObject).AddComponent<MagicMirrorSelectPetPopUp>();
		this.mSelectPop.Init(this);
		this.mSelectPop.Hide();
		GameObject parent = GameUITools.FindGameObject("Window", base.gameObject);
		GameObject parent2 = GameUITools.RegisterClickEvent("InAdd", new UIEventListener.VoidDelegate(this.OnInAddClick), parent);
		this.mInAdd = GameUITools.RegisterClickEvent("Add", new UIEventListener.VoidDelegate(this.OnInAddClick), parent2);
		this.mInTips = GameUITools.FindGameObject("Tips", parent2);
		this.mInInfo = GameUITools.FindGameObject("Info", parent2);
		this.mInName = GameUITools.FindUILabel("Name", this.mInInfo);
		this.mInLevel = GameUITools.FindUILabel("Level", this.mInInfo);
		this.mInStars = GameUITools.FindGameObject("Stars", this.mInInfo).AddComponent<GUIStars>();
		this.mInStars.Init(5);
		this.mInValues = GameUITools.FindGameObject("Values", this.mInInfo).AddComponent<GUIAttributeValue>();
		this.mInSkills = GameUITools.FindGameObject("Skill", this.mInInfo).AddComponent<GUIPetSkills>();
		parent2 = GameUITools.RegisterClickEvent("OutAdd", new UIEventListener.VoidDelegate(this.OnOutAddClick), parent);
		this.mOutAdd = GameUITools.RegisterClickEvent("Add", new UIEventListener.VoidDelegate(this.OnOutAddClick), parent2);
		this.mOutTips = GameUITools.FindGameObject("Tips", parent2);
		this.mOutInfo = GameUITools.FindGameObject("Info", parent2);
		this.mOutName = GameUITools.FindUILabel("Name", this.mOutInfo);
		this.mOutLevel = GameUITools.FindUILabel("Level", this.mOutInfo);
		this.mOutStars = GameUITools.FindGameObject("Stars", this.mOutInfo).AddComponent<GUIStars>();
		this.mOutStars.Init(5);
		this.mOutValues = GameUITools.FindGameObject("Values", this.mOutInfo).AddComponent<GUIAttributeValue>();
		this.mOutSkills = GameUITools.FindGameObject("Skill", this.mOutInfo).AddComponent<GUIPetSkills>();
		GameUITools.RegisterClickEvent("RulesBtn", new UIEventListener.VoidDelegate(this.OnRulesClick), parent);
		this.mCost = GameUITools.FindGameObject("Cost", parent);
		this.mDiamond = GameUITools.FindUILabel("Diamond", this.mCost);
		this.mMagicSoul = GameUITools.FindUILabel("MagicSoul", this.mCost);
		this.mChangeBtn = GameUITools.RegisterClickEvent("ChangeBtn", new UIEventListener.VoidDelegate(this.OnChangeClick), parent).GetComponent<UIButton>();
		this.mChangeBtns = this.mChangeBtn.GetComponents<UIButton>();
		this.mChangeBtnEffect = GameUITools.FindGameObject("Effect", this.mChangeBtn.gameObject);
		this.RefreshInInfo();
	}

	private void RefreshInInfo()
	{
		if (this.mCurPetData == null)
		{
			this.mInAdd.SetActive(true);
			this.mInTips.SetActive(true);
			this.mInInfo.gameObject.SetActive(false);
			if (this.mInIcon != null)
			{
				this.mInIcon.gameObject.SetActive(false);
			}
		}
		else
		{
			this.mInAdd.SetActive(false);
			this.mInTips.SetActive(false);
			this.mInInfo.gameObject.SetActive(true);
			if (this.mInIcon == null)
			{
				this.mInIcon = CommonIconItem.Create(this.mInInfo, new Vector3(-153f, 120f, 0f), new CommonIconItem.VoidCallBack(this.OnInAddClick), true, 0.8f, null);
			}
			if (this.mInIcon != null)
			{
				this.mInIcon.gameObject.SetActive(true);
			}
			this.mInIcon.Refresh(this.mCurPetData, false, false, false);
			if (this.mCurPetData.Data.Further > 0u)
			{
				this.mInName.text = Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
				{
					Tools.GetPetName(this.mCurPetData.Info),
					this.mCurPetData.Data.Further
				});
			}
			else
			{
				this.mInName.text = Tools.GetPetName(this.mCurPetData.Info);
			}
			this.mInName.color = Tools.GetItemQualityColor(this.mCurPetData.Info.Quality);
			this.mInLevel.text = Singleton<StringManager>.Instance.GetString("equipImprove16", new object[]
			{
				this.mCurPetData.Data.Level
			});
			uint num = 0u;
			this.mInStars.Refresh((int)Tools.GetPetStarAndLvl(this.mCurPetData.Data.Awake, out num));
			this.mInValues.Refresh(this.mCurPetData, true);
			this.mInSkills.Refresh(this.mCurPetData, true);
		}
		this.RefreshOutInfo();
	}

	private void RefreshOutInfo()
	{
		if (this.mCurPetData == null)
		{
			this.mOutAdd.SetActive(false);
			this.mOutTips.SetActive(false);
			this.mOutInfo.SetActive(false);
			if (this.mOutIcon != null)
			{
				this.mOutIcon.gameObject.SetActive(false);
			}
		}
		else if (this.mTargetPet == null)
		{
			this.mOutAdd.SetActive(true);
			this.mOutTips.SetActive(true);
			this.mOutInfo.SetActive(false);
			if (this.mOutIcon != null)
			{
				this.mOutIcon.gameObject.SetActive(false);
			}
		}
		else
		{
			this.mOutAdd.SetActive(false);
			this.mOutTips.SetActive(false);
			this.mOutInfo.SetActive(true);
			if (this.mOutIcon == null)
			{
				this.mOutIcon = CommonIconItem.Create(this.mOutInfo, new Vector3(-153f, 120f, 0f), new CommonIconItem.VoidCallBack(this.OnOutAddClick), true, 0.8f, null);
			}
			if (this.mOutIcon != null)
			{
				this.mOutIcon.gameObject.SetActive(true);
			}
			this.mOutIcon.Refresh(this.mTargetPet, false, false, false);
			if (this.mCurPetData.Data.Further > 0u)
			{
				this.mOutName.text = Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
				{
					Tools.GetPetName(this.mTargetPet.Info),
					this.mCurPetData.Data.Further
				});
			}
			else
			{
				this.mOutName.text = Tools.GetPetName(this.mTargetPet.Info);
			}
			this.mOutName.color = Tools.GetItemQualityColor(this.mTargetPet.Info.Quality);
			this.mOutLevel.text = Singleton<StringManager>.Instance.GetString("equipImprove16", new object[]
			{
				this.mCurPetData.Data.Level
			});
			uint num = 0u;
			this.mOutStars.Refresh((int)Tools.GetPetStarAndLvl(this.mCurPetData.Data.Awake, out num));
			this.mOutValues.Refresh(new PetDataEx(this.mCurPetData.Data, this.mTargetPet.Info), true);
			this.mOutSkills.Refresh(new PetDataEx(this.mCurPetData.Data, this.mTargetPet.Info), false);
		}
		this.RefreshCost();
		this.RefreshChangeBtn();
	}

	private void RefreshCost()
	{
		if (this.mCurPetData == null || this.mTargetPet == null)
		{
			this.mCost.SetActive(false);
		}
		else
		{
			this.mCost.SetActive(true);
			List<OpenLootData> list = new List<OpenLootData>();
			uint num;
			uint num2;
			uint num3;
			uint num4;
			uint[] array;
			uint num5;
			uint num6;
			this.mCurPetData.GetRebornData(out num, out num2, out num3, out num4, out array, out num5, ref list, out num6, false);
			this.diamond = (int)((long)GameConst.GetInt32(199) * (long)((ulong)num));
			this.magicSoul = (int)((long)GameConst.GetInt32(200) * (long)((ulong)num));
			if (Tools.GetCurrencyMoney(ECurrencyType.ECurrencyT_Diamond, 0) < this.diamond)
			{
				this.mDiamond.color = Color.red;
			}
			else
			{
				this.mDiamond.color = Color.white;
			}
			if (Tools.GetCurrencyMoney(ECurrencyType.ECurrencyT_MagicSoul, 0) < this.magicSoul)
			{
				this.mMagicSoul.color = Color.red;
			}
			else
			{
				this.mMagicSoul.color = Color.white;
			}
			this.mDiamond.text = this.diamond.ToString();
			this.mMagicSoul.text = this.magicSoul.ToString();
		}
	}

	private void RefreshChangeBtn()
	{
		if (this.mCurPetData == null)
		{
			this.mChangeBtn.isEnabled = false;
			for (int i = 0; i < this.mChangeBtns.Length; i++)
			{
				this.mChangeBtns[i].SetState(UIButtonColor.State.Disabled, true);
			}
			this.mChangeBtnEffect.SetActive(false);
		}
		else
		{
			this.mChangeBtn.isEnabled = true;
			for (int j = 0; j < this.mChangeBtns.Length; j++)
			{
				this.mChangeBtns[j].SetState(UIButtonColor.State.Normal, true);
			}
			this.mChangeBtnEffect.SetActive(this.mTargetPet != null);
		}
	}

	private void OnPlayerUpdateEvent()
	{
		this.RefreshCost();
	}

	public void AddInPet(PetDataEx data)
	{
		this.mCurPetData = data;
		this.mTargetPet = null;
		this.mSelectPop.Close();
		this.RefreshInInfo();
	}

	public void AddOutPet(PetDataEx pet)
	{
		if (pet == null)
		{
			return;
		}
		this.mTargetPet = pet;
		this.mSelectPop.Close();
		this.RefreshOutInfo();
	}

	private void OnRulesClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIRuleInfoPopUp.ShowThis("MirrorLb", "Mirror4");
	}

	private void OnInAddClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mSelectPop.Open(null);
		this.setIn = true;
	}

	private void OnOutAddClick(GameObject go)
	{
		if (this.mCurPetData == null)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mSelectPop.Open(this.mCurPetData);
		this.setIn = false;
	}

	private void OnChangeClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mCurPetData == null)
		{
			return;
		}
		if (this.mTargetPet == null)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("Mirror3", 0f, 0f);
			return;
		}
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.diamond, 0))
		{
			return;
		}
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_MagicSoul, this.magicSoul, 0))
		{
			return;
		}
		MC2S_PetExchange mC2S_PetExchange = new MC2S_PetExchange();
		mC2S_PetExchange.PetID1 = this.mCurPetData.Data.ID;
		mC2S_PetExchange.PetID2 = this.mTargetPet.Data.ID;
		Globals.Instance.CliSession.Send(425, mC2S_PetExchange);
	}

	private void OnMsgPetExchange(MemoryStream stream)
	{
		MS2C_PetExchange mS2C_PetExchange = Serializer.NonGeneric.Deserialize(typeof(MS2C_PetExchange), stream) as MS2C_PetExchange;
		if (mS2C_PetExchange.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PetR", mS2C_PetExchange.Result);
			return;
		}
		GUIMagicMirrorExchangeSuccess.Show(this.mTargetPet);
		this.mCurPetData = null;
		this.mTargetPet = null;
		this.RefreshInInfo();
	}
}
