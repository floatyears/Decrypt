using Att;
using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class GUIPetTrainSceneV2 : GameUISession
{
	public enum EUILopetTabs
	{
		E_UIBaseInfo,
		E_UILvlUp,
		E_UIAwake,
		E_UIMax
	}

	public enum EUITabBtns
	{
		ETB_UIBaseInfo,
		ETB_UILvlUp,
		ETB_UIJinJie,
		ETB_UISkill,
		ETB_UIPetYang,
		ETB_UIJueXing,
		ETB_UIMax
	}

	private const int mStatNum = 4;

	public UIAtlasDynamic mPetsIconAtlas;

	public UIAtlasDynamic mItemsIconAtlas;

	private GameObject mRightInfoGo;

	private GameObject mState2;

	private GameObject[] mLopetTabs = new GameObject[3];

	private GameObject[] mLopetFTabs = new GameObject[3];

	private GameObject mLopetLvlUpNewMark;

	private GameObject mLopetAwakeNewMark;

	private GameObject mState0;

	private GameObject[] mTab0s = new GameObject[6];

	private GameObject mLvlUpNewMark;

	private GameObject mSkillUpNewMark;

	private GameObject mJinJieNewMark;

	private GameObject mJinJieNewMarkPlayer;

	private GameObject mJueXingNewMark;

	private GameObject mJueXingNewMarkPlayer;

	private GameObject mPeiYangNewMark;

	private GameObject mPeiYangNewMarkPlayer;

	private GameObject[] mTab1s = new GameObject[6];

	private GameObject mState1;

	private GameObject[] mTab10s = new GameObject[4];

	private GameObject[] mTab11s = new GameObject[4];

	private GameObject mCardModel;

	private GameObject mModelTmp;

	private GameObject mModelLvlupEffect;

	private GameObject mModelLvlupEffect2;

	private GameObject mModelEffect75;

	private GameObject mAwakeShopBtn;

	private Transform mTmpTransform;

	private UIActorController mUIActorController;

	private GUIPetTitleInfo mGUIPetTitleInfo;

	private GUILopetTitleInfo mGUILopetTitleInfo;

	private GUILopetTrainBaseInfo mGUILopetTrainBaseInfo;

	private GUILopetTrainLvlUpInfo mGUILopetTrainLvlUpInfo;

	private GUILopetTrainAwakeInfo mGUILopetTrainAwakeInfo;

	private GUIPetTrainBaseInfo mGUIPetTrainBaseInfo;

	private GUIPetTrainLvlUpInfo mGUIPetTrainLvlUpInfo;

	private GUIPetTrainJinjieInfo mGUIPetTrainJinjieInfo;

	private GUIPetTrainSkillInfo mGUIPetTrainSkillInfo;

	private GUIPetTrainJueXingInfo mGUIPetTrainJueXingInfo;

	private GUIPetTrainPeiYangInfo mGUIPetTrainPeiYangInfo;

	private List<PetDataEx> mPetDatas = new List<PetDataEx>();

	private GameObject mLeftBtn;

	private GameObject mRightBtn;

	private int mPageIndex;

	private int mCurrentPetDataIndex;

	private List<PetDataEx> mBattleingPetDatas = new List<PetDataEx>();

	private LopetDataEx mBattlingLopetData;

	private GUISimpleSM<string, string> mGUISimpleSM;

	private GUISimpleSM<string, string>.TriggerWithParameters<int> mLvlUpTrigger;

	private StringBuilder mSb = new StringBuilder(42);

	private uint mOldLvl;

	private int mOldHpNum;

	private int mOldAttackNum;

	private int mOldWuFangNum;

	private int mOlfFaFangNum;

	private PetDataEx mCurPetDataEx;

	private LopetDataEx mCurLopetDataEx;

	private ResourceEntity asyncEntiry;

	public bool ShowLopetLvlUpNewMark
	{
		set
		{
			this.mLopetLvlUpNewMark.gameObject.SetActive(value);
		}
	}

	public bool ShowLopetAwakeNewMark
	{
		set
		{
			this.mLopetAwakeNewMark.gameObject.SetActive(value);
		}
	}

	public bool ShowPetLvlUpNewMark
	{
		set
		{
			this.mLvlUpNewMark.SetActive(value);
		}
	}

	public bool ShowPetSkillUpNewMark
	{
		set
		{
			this.mSkillUpNewMark.SetActive(value);
		}
	}

	public bool ShowPetJinJieNewMark
	{
		set
		{
			this.mJinJieNewMark.SetActive(value);
			this.mJinJieNewMarkPlayer.SetActive(value);
		}
	}

	public bool ShowPetJueXingNewMark
	{
		set
		{
			this.mJueXingNewMark.SetActive(value);
			this.mJueXingNewMarkPlayer.SetActive(value);
		}
	}

	public bool ShowPetPeiYangNewMark
	{
		set
		{
			this.mPeiYangNewMark.SetActive(value);
			this.mPeiYangNewMarkPlayer.SetActive(value);
		}
	}

	public GameObject[] Tab0s
	{
		get
		{
			return this.mTab0s;
		}
	}

	public GameObject[] Tab10s
	{
		get
		{
			return this.mTab10s;
		}
	}

	public GUIPetTrainLvlUpInfo PetTrainLvlUpInfo
	{
		get
		{
			return this.mGUIPetTrainLvlUpInfo;
		}
		set
		{
			this.mGUIPetTrainLvlUpInfo = value;
		}
	}

	public GUIPetTrainJinjieInfo PetTrainJinjieInfo
	{
		get
		{
			return this.mGUIPetTrainJinjieInfo;
		}
		set
		{
			this.mGUIPetTrainJinjieInfo = value;
		}
	}

	public GUIPetTrainSkillInfo PetTrainSkillInfo
	{
		get
		{
			return this.mGUIPetTrainSkillInfo;
		}
		set
		{
			this.mGUIPetTrainSkillInfo = value;
		}
	}

	public GUIPetTrainJueXingInfo PetTrainJueXingInfo
	{
		get
		{
			return this.mGUIPetTrainJueXingInfo;
		}
	}

	public GUIPetTrainPeiYangInfo PetTrainPeiYangInfo
	{
		get
		{
			return this.mGUIPetTrainPeiYangInfo;
		}
	}

	public PetDataEx CurPetDataEx
	{
		get
		{
			return this.mCurPetDataEx;
		}
		set
		{
			this.mCurPetDataEx = value;
			if (this.mCurPetDataEx == null)
			{
				return;
			}
			if (this.mCurLopetDataEx != null)
			{
				this.mCurLopetDataEx = null;
			}
			GameUIManager.mInstance.GetTopGoods().BackLabelText = Singleton<StringManager>.Instance.GetString("petTrain");
			this.InitPetDatas();
			if (this.CurPetIsNotPlayer())
			{
				this.mState0.SetActive(true);
				this.mState1.SetActive(false);
				this.mState2.SetActive(false);
			}
			else
			{
				this.mState0.SetActive(false);
				this.mState1.SetActive(true);
				this.mState2.SetActive(false);
			}
			this.mGUIPetTitleInfo.Refresh(this.mCurPetDataEx, true, this.mCurPetDataEx.GetSocketSlot() == 0);
			if (this.mGUIPetTrainBaseInfo != null)
			{
				this.mGUIPetTrainBaseInfo.Refresh();
			}
			this.mGUIPetTrainLvlUpInfo.Refresh();
			this.mGUIPetTrainJinjieInfo.Refresh();
			if (this.mGUIPetTrainPeiYangInfo != null)
			{
				this.mGUIPetTrainPeiYangInfo.Refresh();
			}
			if (this.mGUIPetTrainJueXingInfo != null)
			{
				this.mGUIPetTrainJueXingInfo.Refresh();
			}
			if (this.mCurPetDataEx.GetSocketSlot() != 0)
			{
				this.mGUIPetTrainSkillInfo.Refresh();
				this.mGUIPetTrainSkillInfo.SelectCurItem(this.mGUIPetTrainSkillInfo.GetCurSelectIndex());
			}
			this.RefreshModal();
		}
	}

	public LopetDataEx CurLopetDataEx
	{
		get
		{
			return this.mCurLopetDataEx;
		}
		set
		{
			this.mCurLopetDataEx = value;
			if (this.mCurLopetDataEx == null)
			{
				return;
			}
			if (this.mCurPetDataEx != null)
			{
				this.mCurPetDataEx = null;
			}
			GameUIManager.mInstance.GetTopGoods().BackLabelText = Singleton<StringManager>.Instance.GetString("lopetTrain");
			this.mState0.gameObject.SetActive(false);
			this.mState1.gameObject.SetActive(false);
			this.mState2.gameObject.SetActive(true);
			this.Refresh();
			this.RefreshModal();
		}
	}

	public static void Show(LopetDataEx lopetData, GUIPetTrainSceneV2.EUILopetTabs type = GUIPetTrainSceneV2.EUILopetTabs.E_UIBaseInfo)
	{
		if (lopetData == null)
		{
			return;
		}
		GameUIManager.mInstance.uiState.mLopetTrainCurLopetDataEx = lopetData;
		GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = (int)type;
		GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
	}

	public void ModelEffect75()
	{
		NGUITools.SetActive(this.mModelEffect75, false);
		NGUITools.SetActive(this.mModelEffect75, true);
	}

	public Transform GetCardModelTransform()
	{
		return this.mTmpTransform;
	}

	public Vector3 GetStarPosition(int index)
	{
		if (this.mGUIPetTitleInfo != null)
		{
			return this.mGUIPetTitleInfo.GetStarPosition(index);
		}
		return Vector3.zero;
	}

	public void PlayStarEffect(int index)
	{
		if (this.mGUIPetTitleInfo != null)
		{
			this.mGUIPetTitleInfo.PlayStarEffect(index);
		}
	}

	public List<PetDataEx> GetPetDatas()
	{
		return this.mPetDatas;
	}

	public uint GetOldLvl()
	{
		return this.mOldLvl;
	}

	public void SetOldAttrNum()
	{
		if (this.mCurPetDataEx != null)
		{
			this.mOldLvl = this.mCurPetDataEx.Data.Level;
			this.mCurPetDataEx.GetAttribute(ref this.mOldHpNum, ref this.mOldAttackNum, ref this.mOldWuFangNum, ref this.mOlfFaFangNum);
		}
		else if (this.mCurLopetDataEx != null)
		{
			this.mOldLvl = this.mCurLopetDataEx.Data.Level;
			this.mCurLopetDataEx.GetAttribute(ref this.mOldHpNum, ref this.mOldAttackNum, ref this.mOldWuFangNum, ref this.mOlfFaFangNum);
		}
	}

	public void SetCurSelectItem(int index, int lvlPageIndex = 0)
	{
		switch (index)
		{
		case 0:
			this.mGUISimpleSM.Fire("onBaseInfo");
			break;
		case 1:
			if (this.CurPetIsNotPlayer() || this.mCurLopetDataEx != null)
			{
				this.mGUISimpleSM.Fire<int>(this.mLvlUpTrigger, lvlPageIndex);
			}
			else
			{
				this.mGUISimpleSM.Fire("onBaseInfo");
			}
			break;
		case 2:
			this.mGUISimpleSM.Fire("onJinJie");
			break;
		case 3:
			if (this.CurPetIsNotPlayer())
			{
				this.mGUISimpleSM.Fire("onSkill");
			}
			else
			{
				this.mGUISimpleSM.Fire("onBaseInfo");
			}
			break;
		case 4:
			this.mGUISimpleSM.Fire("onPeiYang");
			break;
		case 5:
			this.mGUISimpleSM.Fire("onJueXing");
			break;
		default:
			this.mGUISimpleSM.Fire("onBaseInfo");
			break;
		}
	}

	public int GetCurPageIndex()
	{
		if (this.mGUISimpleSM.State == "baseInfo")
		{
			return 0;
		}
		if (this.mGUISimpleSM.State == "lvlUp")
		{
			return 1;
		}
		if (this.mGUISimpleSM.State == "jinJie")
		{
			return 2;
		}
		if (this.mGUISimpleSM.State == "skill")
		{
			return 3;
		}
		if (this.mGUISimpleSM.State == "peiYang")
		{
			return 4;
		}
		if (this.mGUISimpleSM.State == "jueXing")
		{
			return 5;
		}
		return 0;
	}

	public int GetCurLvlPageIndex()
	{
		return this.mGUIPetTrainLvlUpInfo.GetCurPageIndex();
	}

	public void SetTuiShiItems(List<PetDataEx> petDatas)
	{
		this.mGUIPetTrainLvlUpInfo.SetTuiShiItems(petDatas);
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle");
		Transform transform2 = transform.Find("winBg");
		this.mCardModel = transform.Find("flower/modelPos").gameObject;
		this.mModelLvlupEffect = this.mCardModel.transform.Find("ui56").gameObject;
		Tools.SetParticleRenderQueue2(this.mModelLvlupEffect, 4500);
		NGUITools.SetActive(this.mModelLvlupEffect, false);
		this.mTmpTransform = this.mModelLvlupEffect.transform;
		this.mModelLvlupEffect2 = this.mCardModel.transform.Find("ui56_3").gameObject;
		Tools.SetParticleRenderQueue2(this.mModelLvlupEffect2, 4500);
		NGUITools.SetActive(this.mModelLvlupEffect2, false);
		this.mModelEffect75 = this.mCardModel.transform.Find("ui75").gameObject;
		Tools.SetParticleRenderQueue2(this.mModelEffect75, 4500);
		NGUITools.SetActive(this.mModelEffect75, false);
		this.mState2 = transform2.Find("state2").gameObject;
		for (int i = 0; i < 3; i++)
		{
			this.mLopetTabs[i] = this.mState2.transform.Find(string.Format("tab{0}", i)).gameObject;
			UIEventListener expr_14F = UIEventListener.Get(this.mLopetTabs[i]);
			expr_14F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_14F.onClick, new UIEventListener.VoidDelegate(this.OnTab2Click));
			this.mLopetFTabs[i] = this.mState2.transform.Find(string.Format("tabF{0}", i)).gameObject;
		}
		this.mLopetLvlUpNewMark = this.mLopetTabs[1].transform.Find("newMark1").gameObject;
		this.mLopetLvlUpNewMark.SetActive(false);
		this.mLopetAwakeNewMark = this.mLopetTabs[2].transform.Find("newMark1").gameObject;
		this.mLopetAwakeNewMark.SetActive(false);
		this.mState0 = transform2.Find("state0").gameObject;
		Transform transform3 = this.mState0.transform;
		for (int j = 0; j < 6; j++)
		{
			this.mTab0s[j] = transform3.Find(string.Format("tab{0}", j)).gameObject;
			UIEventListener expr_261 = UIEventListener.Get(this.mTab0s[j]);
			expr_261.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_261.onClick, new UIEventListener.VoidDelegate(this.OnTab0Click));
			this.mTab1s[j] = transform3.Find(string.Format("tabF{0}", j)).gameObject;
		}
		this.mLvlUpNewMark = this.mTab0s[1].transform.Find("newMark1").gameObject;
		this.mLvlUpNewMark.SetActive(false);
		this.mJinJieNewMark = this.mTab0s[2].transform.Find("newMark1").gameObject;
		this.mJinJieNewMark.SetActive(false);
		this.mSkillUpNewMark = this.mTab0s[3].transform.Find("newMark1").gameObject;
		this.mSkillUpNewMark.SetActive(false);
		this.mPeiYangNewMark = this.mTab0s[4].transform.Find("newMark1").gameObject;
		this.mPeiYangNewMark.SetActive(false);
		this.mJueXingNewMark = this.mTab0s[5].transform.Find("newMark1").gameObject;
		this.mJueXingNewMark.SetActive(false);
		this.mState1 = transform2.Find("state1").gameObject;
		Transform transform4 = this.mState1.transform;
		for (int k = 0; k < 4; k++)
		{
			this.mTab10s[k] = transform4.Find(string.Format("tab{0}", k)).gameObject;
			UIEventListener expr_3FA = UIEventListener.Get(this.mTab10s[k]);
			expr_3FA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3FA.onClick, new UIEventListener.VoidDelegate(this.OnTab10Click));
			this.mTab11s[k] = transform4.Find(string.Format("tabF{0}", k)).gameObject;
		}
		this.mJinJieNewMarkPlayer = this.mTab10s[1].transform.Find("newMark1").gameObject;
		this.mJinJieNewMarkPlayer.SetActive(false);
		this.mPeiYangNewMarkPlayer = this.mTab10s[2].transform.Find("newMark1").gameObject;
		this.mPeiYangNewMarkPlayer.SetActive(false);
		this.mJueXingNewMarkPlayer = this.mTab10s[3].transform.Find("newMark1").gameObject;
		this.mJueXingNewMarkPlayer.SetActive(false);
		Transform transform5 = transform.Find("topInfoPanel");
		this.mGUIPetTitleInfo = transform5.Find("topInfo").gameObject.AddComponent<GUIPetTitleInfo>();
		this.mGUIPetTitleInfo.InitWithBaseScene();
		this.mGUILopetTitleInfo = transform5.Find("lopetTopInfo").gameObject.AddComponent<GUILopetTitleInfo>();
		this.mAwakeShopBtn = transform5.Find("awakeShop").gameObject;
		UIEventListener expr_54B = UIEventListener.Get(this.mAwakeShopBtn);
		expr_54B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_54B.onClick, new UIEventListener.VoidDelegate(this.OnAwakeShopBtnClick));
		GameObject gameObject = this.mAwakeShopBtn.transform.Find("ui77").gameObject;
		Tools.SetParticleRenderQueue2(gameObject, 4000);
		Transform transform6 = transform2.Find("rightInfo");
		this.mRightInfoGo = transform6.gameObject;
		this.mGUIPetTrainLvlUpInfo = transform6.Find("lvlInfo").gameObject.AddComponent<GUIPetTrainLvlUpInfo>();
		this.mGUIPetTrainLvlUpInfo.InitWithBaseScene(this);
		this.mGUIPetTrainJinjieInfo = transform6.Find("jinJieInfo").gameObject.AddComponent<GUIPetTrainJinjieInfo>();
		this.mGUIPetTrainJinjieInfo.InitWithBaseScene(this);
		this.mGUIPetTrainSkillInfo = transform6.Find("jiNengInfo").gameObject.AddComponent<GUIPetTrainSkillInfo>();
		this.mGUIPetTrainSkillInfo.InitWithBaseScene(this);
		this.mLeftBtn = transform.Find("leftBtn").gameObject;
		UIEventListener expr_647 = UIEventListener.Get(this.mLeftBtn);
		expr_647.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_647.onClick, new UIEventListener.VoidDelegate(this.OnLeftBtnClick));
		this.mRightBtn = transform.Find("rightBtn").gameObject;
		UIEventListener expr_689 = UIEventListener.Get(this.mRightBtn);
		expr_689.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_689.onClick, new UIEventListener.VoidDelegate(this.OnRightBtnClick));
		this.mGUISimpleSM = new GUISimpleSM<string, string>("init");
		this.mLvlUpTrigger = this.mGUISimpleSM.SetTriggerParameters<int>("onLvlUp");
		this.mGUISimpleSM.Configure("init").Permit("onBaseInfo", "baseInfo").PermitIf("onLvlUp", "lvlUp", new Func<bool>(this.CurCanLvlUp)).Permit("onJinJie", "jinJie").PermitIf("onSkill", "skill", new Func<bool>(this.CurPetIsNotPlayer)).Permit("onPeiYang", "peiYang").Permit("onJueXing", "jueXing");
		this.mGUISimpleSM.Configure("baseInfo").PermitIf("onLvlUp", "lvlUp", new Func<bool>(this.CurCanLvlUp)).PermitIf("onSkill", "skill", new Func<bool>(this.CurPetIsNotPlayer)).Permit("onJinJie", "jinJie").Permit("onPeiYang", "peiYang").Permit("onJueXing", "jueXing").PermitReentry("onBaseInfo").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterBaseInfo();
		});
		this.mGUISimpleSM.Configure("lvlUp").Permit("onBaseInfo", "baseInfo").Permit("onJinJie", "jinJie").PermitIf("onSkill", "skill", new Func<bool>(this.CurPetIsNotPlayer)).Permit("onPeiYang", "peiYang").Permit("onJueXing", "jueXing").PermitReentry("onLvlUp").OnEntryFrom<int>(this.mLvlUpTrigger, delegate(int index)
		{
			this.OnEnterLvlUp(index);
		});
		this.mGUISimpleSM.Configure("jinJie").Permit("onBaseInfo", "baseInfo").PermitIf("onLvlUp", "lvlUp", new Func<bool>(this.CurCanLvlUp)).PermitIf("onSkill", "skill", new Func<bool>(this.CurPetIsNotPlayer)).Permit("onPeiYang", "peiYang").Permit("onJueXing", "jueXing").PermitReentry("onJinJie").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterJinJie();
		});
		this.mGUISimpleSM.Configure("skill").Permit("onBaseInfo", "baseInfo").PermitIf("onLvlUp", "lvlUp", new Func<bool>(this.CurCanLvlUp)).Permit("onJinJie", "jinJie").Permit("onPeiYang", "peiYang").Permit("onJueXing", "jueXing").Ignore("onSkill").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterSkill();
		});
		this.mGUISimpleSM.Configure("jueXing").Permit("onBaseInfo", "baseInfo").PermitIf("onLvlUp", "lvlUp", new Func<bool>(this.CurCanLvlUp)).Permit("onJinJie", "jinJie").PermitIf("onSkill", "skill", new Func<bool>(this.CurPetIsNotPlayer)).Permit("onPeiYang", "peiYang").PermitReentry("onJueXing").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterJueXing();
		});
		this.mGUISimpleSM.Configure("peiYang").Permit("onBaseInfo", "baseInfo").PermitIf("onLvlUp", "lvlUp", new Func<bool>(this.CurCanLvlUp)).Permit("onJinJie", "jinJie").PermitIf("onSkill", "skill", new Func<bool>(this.CurPetIsNotPlayer)).Permit("onJueXing", "jueXing").PermitReentry("onPeiYang").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterPeiYang();
		});
	}

	private bool CurCanLvlUp()
	{
		return this.CurPetIsNotPlayer() || this.CurLopetDataEx != null;
	}

	private bool CurPetIsNotPlayer()
	{
		return this.mCurPetDataEx != null && this.mCurPetDataEx.GetSocketSlot() != 0;
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("petTrain");
		topGoods.transform.localPosition = new Vector3(topGoods.transform.localPosition.x, topGoods.transform.localPosition.y, topGoods.transform.localPosition.z - 1000f);
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		PetSubSystem expr_9B = Globals.Instance.Player.PetSystem;
		expr_9B.LevelupPetEvent = (PetSubSystem.UpdatePetCallback)Delegate.Combine(expr_9B.LevelupPetEvent, new PetSubSystem.UpdatePetCallback(this.OnLevelupPetEvent));
		PetSubSystem expr_CB = Globals.Instance.Player.PetSystem;
		expr_CB.FurtherPetEvent = (PetSubSystem.UpdatePetCallback)Delegate.Combine(expr_CB.FurtherPetEvent, new PetSubSystem.UpdatePetCallback(this.OnFurtherPetEvent));
		PetSubSystem expr_FB = Globals.Instance.Player.PetSystem;
		expr_FB.SkillPetEvent = (PetSubSystem.UpdatePetCallback)Delegate.Combine(expr_FB.SkillPetEvent, new PetSubSystem.UpdatePetCallback(this.OnSkillPetEvent));
		PetSubSystem expr_12B = Globals.Instance.Player.PetSystem;
		expr_12B.AwakeLevelupEvent = (PetSubSystem.UpdatePetCallback)Delegate.Combine(expr_12B.AwakeLevelupEvent, new PetSubSystem.UpdatePetCallback(this.OnAwakeLevelUpEvent));
		PetSubSystem expr_15B = Globals.Instance.Player.PetSystem;
		expr_15B.AwakeItemEvent = (PetSubSystem.AwakeItemCallback)Delegate.Combine(expr_15B.AwakeItemEvent, new PetSubSystem.AwakeItemCallback(this.OnAwakeItemEvent));
		PetSubSystem expr_18B = Globals.Instance.Player.PetSystem;
		expr_18B.PetCultivateEvent = (PetSubSystem.UpdatePetCallback)Delegate.Combine(expr_18B.PetCultivateEvent, new PetSubSystem.UpdatePetCallback(this.OnPetCultivateEvent));
		PetSubSystem expr_1BB = Globals.Instance.Player.PetSystem;
		expr_1BB.PetCultivateAckEvent = (PetSubSystem.UpdatePetCallback)Delegate.Combine(expr_1BB.PetCultivateAckEvent, new PetSubSystem.UpdatePetCallback(this.OnPetCultivateAckEvent));
		LocalPlayer expr_1E6 = Globals.Instance.Player;
		expr_1E6.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_1E6.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		LocalPlayer expr_211 = Globals.Instance.Player;
		expr_211.DataInitEvent = (LocalPlayer.DataInitCallback)Delegate.Combine(expr_211.DataInitEvent, new LocalPlayer.DataInitCallback(this.OnDataInitEvent));
		LopetSubSystem expr_241 = Globals.Instance.Player.LopetSystem;
		expr_241.LevelupLopetEvent = (LopetSubSystem.UpdateLopetCallback)Delegate.Combine(expr_241.LevelupLopetEvent, new LopetSubSystem.UpdateLopetCallback(this.OnLevelupLopetEvent));
		LopetSubSystem expr_271 = Globals.Instance.Player.LopetSystem;
		expr_271.AwakeLopetEvent = (LopetSubSystem.UpdateLopetCallback)Delegate.Combine(expr_271.AwakeLopetEvent, new LopetSubSystem.UpdateLopetCallback(this.OnAwakeLopetEvent));
		Globals.Instance.CliSession.Register(537, new ClientSession.MsgHandler(this.OnMsgAwakeItemCreate));
		this.InitBattelingPetDatas();
		this.CurPetDataEx = GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx;
		this.CurLopetDataEx = GameUIManager.mInstance.uiState.mLopetTrainCurLopetDataEx;
		if (this.CurPetDataEx != null)
		{
			if (!GameUIManager.mInstance.uiState.IsShowPetZhuWeiPopUp && !GameUIManager.mInstance.uiState.IsShowPetZhuWei)
			{
				this.mLeftBtn.SetActive((this.mBattleingPetDatas.Count > 1 || this.mBattlingLopetData != null) && this.CurPetDataEx.IsBattling());
				this.mRightBtn.SetActive((this.mBattleingPetDatas.Count > 1 || this.mBattlingLopetData != null) && this.CurPetDataEx.IsBattling());
			}
			else
			{
				this.mLeftBtn.SetActive(this.mBattleingPetDatas.Count > 1 && this.CurPetDataEx.IsPetAssisting());
				this.mRightBtn.SetActive(this.mBattleingPetDatas.Count > 1 && this.CurPetDataEx.IsPetAssisting());
			}
		}
		else if (this.CurLopetDataEx != null)
		{
			if (this.CurLopetDataEx.IsBattling())
			{
				this.mLeftBtn.SetActive(true);
				this.mRightBtn.SetActive(true);
			}
			else
			{
				this.mLeftBtn.SetActive(false);
				this.mRightBtn.SetActive(false);
			}
		}
		this.SetCurBattleingPetIndex();
		this.SetCurSelectItem(GameUIManager.mInstance.uiState.mPetTrainCurPageIndex, GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex);
		this.mPageIndex = GameUIManager.mInstance.uiState.mPetTrainCurPageIndex;
		this.RefreshPeiYangNewMark();
		this.RefreshJueXingNewMark();
	}

	private void InitBattelingPetDatas()
	{
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (!GameUIManager.mInstance.uiState.IsShowPetZhuWeiPopUp && !GameUIManager.mInstance.uiState.IsShowPetZhuWei)
			{
				if (current.IsBattling())
				{
					this.mBattleingPetDatas.Add(current);
				}
			}
			else if (current.IsPetAssisting())
			{
				this.mBattleingPetDatas.Add(current);
			}
		}
		this.mBattleingPetDatas.Sort(delegate(PetDataEx a, PetDataEx b)
		{
			int num = Tools.ComparePetSlot(a, b);
			if (num != 0)
			{
				return num;
			}
			if (a.Info.Quality > b.Info.Quality)
			{
				return -1;
			}
			if (a.Info.Quality < b.Info.Quality)
			{
				return 1;
			}
			return a.Info.ID - b.Info.ID;
		});
		this.mBattlingLopetData = Globals.Instance.Player.LopetSystem.GetCurLopet(true);
	}

	private void SetCurBattleingPetIndex()
	{
		this.mCurrentPetDataIndex = -1;
		if (this.mCurPetDataEx != null)
		{
			for (int i = 0; i < this.mBattleingPetDatas.Count; i++)
			{
				if (this.mCurPetDataEx == this.mBattleingPetDatas[i])
				{
					this.mCurrentPetDataIndex = i;
					break;
				}
			}
		}
		if (this.mCurrentPetDataIndex == -1 && this.mBattlingLopetData != null)
		{
			this.mCurrentPetDataIndex = this.mBattleingPetDatas.Count;
		}
	}

	private void OnLeftBtnClick(GameObject go)
	{
		if (this.mCurPetDataEx != null || this.mCurLopetDataEx != null)
		{
			if (this.mCurrentPetDataIndex <= 0)
			{
				this.mCurrentPetDataIndex = this.mBattleingPetDatas.Count - ((this.mBattlingLopetData != null) ? 0 : 1);
			}
			else
			{
				this.mCurrentPetDataIndex--;
			}
			if (this.mCurrentPetDataIndex >= this.mBattleingPetDatas.Count)
			{
				this.CurLopetDataEx = this.mBattlingLopetData;
			}
			else
			{
				this.CurPetDataEx = this.mBattleingPetDatas[this.mCurrentPetDataIndex];
			}
			this.SetCurSelectItem(this.mPageIndex, 0);
		}
	}

	private void OnRightBtnClick(GameObject go)
	{
		if (this.mCurPetDataEx != null || this.mCurLopetDataEx != null)
		{
			if (this.mCurrentPetDataIndex + ((this.mBattlingLopetData != null) ? 0 : 1) >= this.mBattleingPetDatas.Count)
			{
				this.mCurrentPetDataIndex = 0;
			}
			else
			{
				this.mCurrentPetDataIndex++;
			}
			if (this.mCurrentPetDataIndex >= this.mBattleingPetDatas.Count)
			{
				this.CurLopetDataEx = this.mBattlingLopetData;
			}
			else
			{
				this.CurPetDataEx = this.mBattleingPetDatas[this.mCurrentPetDataIndex];
			}
			this.SetCurSelectItem(this.mPageIndex, 0);
		}
	}

	private void InitPetDatas()
	{
		this.mPetDatas.Clear();
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (!current.IsBattling() && !current.IsPetAssisting() && current.Info.Quality <= 1 && this.mCurPetDataEx.Data.ID != current.Data.ID)
			{
				this.mPetDatas.Add(current);
			}
		}
		this.mPetDatas.Sort(delegate(PetDataEx a, PetDataEx b)
		{
			if (a.Info.Quality > b.Info.Quality)
			{
				return 1;
			}
			if (a.Info.Quality < b.Info.Quality)
			{
				return -1;
			}
			if (a.Info.SubQuality > b.Info.SubQuality)
			{
				return 1;
			}
			if (a.Info.SubQuality < b.Info.SubQuality)
			{
				return -1;
			}
			if (a.Data.Level > b.Data.Level)
			{
				return 1;
			}
			if (a.Data.Level < b.Data.Level)
			{
				return -1;
			}
			return a.Info.ID - b.Info.ID;
		});
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		this.ClearModel();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.transform.localPosition = new Vector3(topGoods.transform.localPosition.x, topGoods.transform.localPosition.y, topGoods.transform.localPosition.z + 1000f);
		topGoods.Hide();
		PetSubSystem expr_7A = Globals.Instance.Player.PetSystem;
		expr_7A.LevelupPetEvent = (PetSubSystem.UpdatePetCallback)Delegate.Remove(expr_7A.LevelupPetEvent, new PetSubSystem.UpdatePetCallback(this.OnLevelupPetEvent));
		PetSubSystem expr_AA = Globals.Instance.Player.PetSystem;
		expr_AA.FurtherPetEvent = (PetSubSystem.UpdatePetCallback)Delegate.Remove(expr_AA.FurtherPetEvent, new PetSubSystem.UpdatePetCallback(this.OnFurtherPetEvent));
		PetSubSystem expr_DA = Globals.Instance.Player.PetSystem;
		expr_DA.SkillPetEvent = (PetSubSystem.UpdatePetCallback)Delegate.Remove(expr_DA.SkillPetEvent, new PetSubSystem.UpdatePetCallback(this.OnSkillPetEvent));
		PetSubSystem expr_10A = Globals.Instance.Player.PetSystem;
		expr_10A.AwakeLevelupEvent = (PetSubSystem.UpdatePetCallback)Delegate.Remove(expr_10A.AwakeLevelupEvent, new PetSubSystem.UpdatePetCallback(this.OnAwakeLevelUpEvent));
		PetSubSystem expr_13A = Globals.Instance.Player.PetSystem;
		expr_13A.AwakeItemEvent = (PetSubSystem.AwakeItemCallback)Delegate.Remove(expr_13A.AwakeItemEvent, new PetSubSystem.AwakeItemCallback(this.OnAwakeItemEvent));
		PetSubSystem expr_16A = Globals.Instance.Player.PetSystem;
		expr_16A.PetCultivateEvent = (PetSubSystem.UpdatePetCallback)Delegate.Remove(expr_16A.PetCultivateEvent, new PetSubSystem.UpdatePetCallback(this.OnPetCultivateEvent));
		PetSubSystem expr_19A = Globals.Instance.Player.PetSystem;
		expr_19A.PetCultivateAckEvent = (PetSubSystem.UpdatePetCallback)Delegate.Remove(expr_19A.PetCultivateAckEvent, new PetSubSystem.UpdatePetCallback(this.OnPetCultivateAckEvent));
		LocalPlayer expr_1C5 = Globals.Instance.Player;
		expr_1C5.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_1C5.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		LocalPlayer expr_1F0 = Globals.Instance.Player;
		expr_1F0.DataInitEvent = (LocalPlayer.DataInitCallback)Delegate.Remove(expr_1F0.DataInitEvent, new LocalPlayer.DataInitCallback(this.OnDataInitEvent));
		LopetSubSystem expr_220 = Globals.Instance.Player.LopetSystem;
		expr_220.LevelupLopetEvent = (LopetSubSystem.UpdateLopetCallback)Delegate.Remove(expr_220.LevelupLopetEvent, new LopetSubSystem.UpdateLopetCallback(this.OnLevelupLopetEvent));
		LopetSubSystem expr_250 = Globals.Instance.Player.LopetSystem;
		expr_250.AwakeLopetEvent = (LopetSubSystem.UpdateLopetCallback)Delegate.Remove(expr_250.AwakeLopetEvent, new LopetSubSystem.UpdateLopetCallback(this.OnAwakeLopetEvent));
		Globals.Instance.CliSession.Unregister(537, new ClientSession.MsgHandler(this.OnMsgAwakeItemCreate));
	}

	public void OnBackClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		if (GameUIManager.mInstance.PreSessionType == typeof(GUITeamManageSceneV2))
		{
			GameUIManager.mInstance.uiState.IsLocalPlayer = true;
			if (this.CurPetDataEx != null)
			{
				GameUIManager.mInstance.uiState.CombatPetSlot = this.CurPetDataEx.GetSocketSlot();
			}
			else if (this.CurLopetDataEx != null)
			{
				GameUIManager.mInstance.uiState.CombatPetSlot = 4;
			}
		}
		Type type = GameUIManager.mInstance.GobackSession();
		if (type == typeof(GUITeamManageSceneV2) && GameUIManager.mInstance.uiState.CombatPetSlot > 3)
		{
			if (GameUIManager.mInstance.uiState.IsShowPetZhuWeiPopUp)
			{
				GameUIManager.mInstance.uiState.IsShowPetZhuWeiPopUp = false;
				GUIPetZhuWeiPopUp.ShowMe();
			}
			if (GameUIManager.mInstance.uiState.IsShowPetZhuWei)
			{
				GameUIManager.mInstance.uiState.IsShowPetZhuWei = false;
			}
		}
	}

	private void OnTab2Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		for (int i = 0; i < this.mLopetTabs.Length; i++)
		{
			if (go == this.mLopetTabs[i])
			{
				this.SetCurSelectItem(i, 0);
				this.mPageIndex = i;
				break;
			}
		}
	}

	public void OnTab0Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (go == this.mTab0s[1])
		{
			this.SetCurSelectItem(1, 0);
			this.mPageIndex = 1;
		}
		else if (go == this.mTab0s[2])
		{
			this.SetCurSelectItem(2, 0);
			this.mPageIndex = 2;
		}
		else if (go == this.mTab0s[3])
		{
			this.SetCurSelectItem(3, 0);
			this.mPageIndex = 3;
		}
		else if (go == this.mTab0s[4])
		{
			if (!Tools.CanPlay(GameConst.GetInt32(122), true))
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WBTip1", new object[]
				{
					GameConst.GetInt32(122)
				}), 0f, 0f);
				return;
			}
			this.SetCurSelectItem(4, 0);
			this.mPageIndex = 4;
		}
		else if (go == this.mTab0s[5])
		{
			if (!Tools.CanPlay(GameConst.GetInt32(24), true))
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WBTip1", new object[]
				{
					GameConst.GetInt32(24)
				}), 0f, 0f);
				return;
			}
			this.SetCurSelectItem(5, 0);
			this.mPageIndex = 5;
		}
		else
		{
			this.SetCurSelectItem(0, 0);
			this.mPageIndex = 0;
		}
	}

	public void OnTab10Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (go == this.mTab10s[1])
		{
			this.SetCurSelectItem(2, 0);
			this.mPageIndex = 2;
		}
		else if (go == this.mTab10s[2])
		{
			if (!Tools.CanPlay(GameConst.GetInt32(122), true))
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WBTip1", new object[]
				{
					GameConst.GetInt32(122)
				}), 0f, 0f);
				return;
			}
			this.SetCurSelectItem(4, 0);
			this.mPageIndex = 4;
		}
		else if (go == this.mTab10s[3])
		{
			if (!Tools.CanPlay(GameConst.GetInt32(24), true))
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WBTip1", new object[]
				{
					GameConst.GetInt32(24)
				}), 0f, 0f);
				return;
			}
			this.SetCurSelectItem(5, 0);
			this.mPageIndex = 5;
		}
		else
		{
			this.SetCurSelectItem(0, 0);
			this.mPageIndex = 0;
		}
	}

	private void SetTabStates(int index)
	{
		if (this.mCurPetDataEx != null)
		{
			this.mGUIPetTitleInfo.gameObject.SetActive(true);
			this.mGUILopetTitleInfo.gameObject.SetActive(false);
			this.HideLopetInfos();
			if (this.CurPetIsNotPlayer())
			{
				for (int i = 0; i < 6; i++)
				{
					this.mTab0s[i].SetActive(i != index);
					this.mTab1s[i].SetActive(i == index);
				}
				if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(122)))
				{
					this.mTab0s[4].SetActive(false);
					this.mTab1s[4].SetActive(false);
				}
				if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(26)))
				{
					this.mTab0s[5].SetActive(false);
					this.mTab1s[5].SetActive(false);
				}
				if (index == 0)
				{
					if (this.mGUIPetTrainBaseInfo == null)
					{
						this.InitBaseInfo();
					}
					this.mGUIPetTrainBaseInfo.gameObject.SetActive(true);
					this.mGUIPetTrainLvlUpInfo.gameObject.SetActive(false);
					this.mGUIPetTrainJinjieInfo.gameObject.SetActive(false);
					this.mGUIPetTrainSkillInfo.gameObject.SetActive(false);
					if (this.mGUIPetTrainPeiYangInfo != null)
					{
						this.mGUIPetTrainPeiYangInfo.gameObject.SetActive(false);
					}
					if (this.mGUIPetTrainJueXingInfo != null)
					{
						this.mGUIPetTrainJueXingInfo.gameObject.SetActive(false);
					}
				}
				else if (index == 1)
				{
					if (this.mGUIPetTrainBaseInfo != null)
					{
						this.mGUIPetTrainBaseInfo.gameObject.SetActive(false);
					}
					this.mGUIPetTrainLvlUpInfo.gameObject.SetActive(true);
					this.mGUIPetTrainJinjieInfo.gameObject.SetActive(false);
					this.mGUIPetTrainSkillInfo.gameObject.SetActive(false);
					if (this.mGUIPetTrainPeiYangInfo != null)
					{
						this.mGUIPetTrainPeiYangInfo.gameObject.SetActive(false);
					}
					if (this.mGUIPetTrainJueXingInfo != null)
					{
						this.mGUIPetTrainJueXingInfo.gameObject.SetActive(false);
					}
				}
				else if (index == 2)
				{
					if (this.mGUIPetTrainBaseInfo != null)
					{
						this.mGUIPetTrainBaseInfo.gameObject.SetActive(false);
					}
					this.mGUIPetTrainLvlUpInfo.gameObject.SetActive(false);
					this.mGUIPetTrainJinjieInfo.gameObject.SetActive(true);
					this.mGUIPetTrainSkillInfo.gameObject.SetActive(false);
					if (this.mGUIPetTrainPeiYangInfo != null)
					{
						this.mGUIPetTrainPeiYangInfo.gameObject.SetActive(false);
					}
					if (this.mGUIPetTrainJueXingInfo != null)
					{
						this.mGUIPetTrainJueXingInfo.gameObject.SetActive(false);
					}
				}
				else if (index == 3)
				{
					if (this.mGUIPetTrainBaseInfo != null)
					{
						this.mGUIPetTrainBaseInfo.gameObject.SetActive(false);
					}
					this.mGUIPetTrainLvlUpInfo.gameObject.SetActive(false);
					this.mGUIPetTrainJinjieInfo.gameObject.SetActive(false);
					this.mGUIPetTrainSkillInfo.gameObject.SetActive(true);
					if (this.mGUIPetTrainPeiYangInfo != null)
					{
						this.mGUIPetTrainPeiYangInfo.gameObject.SetActive(false);
					}
					if (this.mGUIPetTrainJueXingInfo != null)
					{
						this.mGUIPetTrainJueXingInfo.gameObject.SetActive(false);
					}
				}
				else if (index == 4)
				{
					if (this.mGUIPetTrainBaseInfo != null)
					{
						this.mGUIPetTrainBaseInfo.gameObject.SetActive(false);
					}
					this.mGUIPetTrainLvlUpInfo.gameObject.SetActive(false);
					this.mGUIPetTrainJinjieInfo.gameObject.SetActive(false);
					this.mGUIPetTrainSkillInfo.gameObject.SetActive(false);
					if (this.mGUIPetTrainPeiYangInfo == null)
					{
						this.InitPeiYangInfo();
					}
					this.mGUIPetTrainPeiYangInfo.gameObject.SetActive(true);
					if (this.mGUIPetTrainJueXingInfo != null)
					{
						this.mGUIPetTrainJueXingInfo.gameObject.SetActive(false);
					}
				}
				else if (index == 5)
				{
					if (this.mGUIPetTrainBaseInfo != null)
					{
						this.mGUIPetTrainBaseInfo.gameObject.SetActive(false);
					}
					this.mGUIPetTrainLvlUpInfo.gameObject.SetActive(false);
					this.mGUIPetTrainJinjieInfo.gameObject.SetActive(false);
					this.mGUIPetTrainSkillInfo.gameObject.SetActive(false);
					if (this.mGUIPetTrainPeiYangInfo != null)
					{
						this.mGUIPetTrainPeiYangInfo.gameObject.SetActive(false);
					}
					if (this.mGUIPetTrainJueXingInfo == null)
					{
						this.InitJueXingInfo();
					}
					this.mGUIPetTrainJueXingInfo.gameObject.SetActive(true);
				}
			}
			else
			{
				if (index == 0)
				{
					this.mTab10s[0].SetActive(false);
					this.mTab11s[0].SetActive(true);
					this.mTab10s[1].SetActive(true);
					this.mTab11s[1].SetActive(false);
					this.mTab10s[2].SetActive(true);
					this.mTab11s[2].SetActive(false);
					this.mTab10s[3].SetActive(true);
					this.mTab11s[3].SetActive(false);
					if (this.mGUIPetTrainBaseInfo == null)
					{
						this.InitBaseInfo();
					}
					this.mGUIPetTrainBaseInfo.gameObject.SetActive(true);
					this.mGUIPetTrainJinjieInfo.gameObject.SetActive(false);
					if (this.mGUIPetTrainPeiYangInfo != null)
					{
						this.mGUIPetTrainPeiYangInfo.gameObject.SetActive(false);
					}
					if (this.mGUIPetTrainJueXingInfo != null)
					{
						this.mGUIPetTrainJueXingInfo.gameObject.SetActive(false);
					}
				}
				else if (index == 2)
				{
					this.mTab10s[0].SetActive(true);
					this.mTab11s[0].SetActive(false);
					this.mTab10s[1].SetActive(false);
					this.mTab11s[1].SetActive(true);
					this.mTab10s[2].SetActive(true);
					this.mTab11s[2].SetActive(false);
					this.mTab10s[3].SetActive(true);
					this.mTab11s[3].SetActive(false);
					if (this.mGUIPetTrainBaseInfo != null)
					{
						this.mGUIPetTrainBaseInfo.gameObject.SetActive(false);
					}
					this.mGUIPetTrainJinjieInfo.gameObject.SetActive(true);
					if (this.mGUIPetTrainPeiYangInfo != null)
					{
						this.mGUIPetTrainPeiYangInfo.gameObject.SetActive(false);
					}
					if (this.mGUIPetTrainJueXingInfo != null)
					{
						this.mGUIPetTrainJueXingInfo.gameObject.SetActive(false);
					}
				}
				else if (index == 4)
				{
					this.mTab10s[0].SetActive(true);
					this.mTab11s[0].SetActive(false);
					this.mTab10s[1].SetActive(true);
					this.mTab11s[1].SetActive(false);
					this.mTab10s[2].SetActive(false);
					this.mTab11s[2].SetActive(true);
					this.mTab10s[3].SetActive(true);
					this.mTab11s[3].SetActive(false);
					if (this.mGUIPetTrainBaseInfo != null)
					{
						this.mGUIPetTrainBaseInfo.gameObject.SetActive(false);
					}
					this.mGUIPetTrainJinjieInfo.gameObject.SetActive(false);
					if (this.mGUIPetTrainPeiYangInfo == null)
					{
						this.InitPeiYangInfo();
					}
					this.mGUIPetTrainPeiYangInfo.gameObject.SetActive(true);
					if (this.mGUIPetTrainJueXingInfo != null)
					{
						this.mGUIPetTrainJueXingInfo.gameObject.SetActive(false);
					}
				}
				else if (index == 5)
				{
					this.mTab10s[0].SetActive(true);
					this.mTab11s[0].SetActive(false);
					this.mTab10s[1].SetActive(true);
					this.mTab11s[1].SetActive(false);
					this.mTab10s[2].SetActive(true);
					this.mTab11s[2].SetActive(false);
					this.mTab10s[3].SetActive(false);
					this.mTab11s[3].SetActive(true);
					if (this.mGUIPetTrainBaseInfo != null)
					{
						this.mGUIPetTrainBaseInfo.gameObject.SetActive(false);
					}
					this.mGUIPetTrainJinjieInfo.gameObject.SetActive(false);
					if (this.mGUIPetTrainPeiYangInfo != null)
					{
						this.mGUIPetTrainPeiYangInfo.gameObject.SetActive(false);
					}
					if (this.mGUIPetTrainJueXingInfo == null)
					{
						this.InitJueXingInfo();
					}
					this.mGUIPetTrainJueXingInfo.gameObject.SetActive(true);
				}
				if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(122)))
				{
					this.mTab10s[2].SetActive(false);
					this.mTab11s[2].SetActive(false);
				}
				if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(26)))
				{
					this.mTab10s[3].SetActive(false);
					this.mTab11s[3].SetActive(false);
				}
				this.mGUIPetTrainLvlUpInfo.gameObject.SetActive(false);
				this.mGUIPetTrainSkillInfo.gameObject.SetActive(false);
			}
		}
		else if (this.mCurLopetDataEx != null)
		{
			this.HidePetInfos();
			this.HideLopetInfos();
			this.mGUIPetTitleInfo.gameObject.SetActive(false);
			this.mGUILopetTitleInfo.gameObject.SetActive(true);
			if (index >= 3)
			{
				index = 0;
			}
			for (int j = 0; j < 3; j++)
			{
				this.mLopetTabs[j].SetActive(j != index);
				this.mLopetFTabs[j].SetActive(j == index);
			}
			switch (index)
			{
			case 0:
				if (this.mGUILopetTrainBaseInfo == null)
				{
					this.InitLopetBaseInfo();
				}
				this.mGUILopetTrainBaseInfo.gameObject.SetActive(true);
				break;
			case 1:
				if (this.mGUILopetTrainLvlUpInfo == null)
				{
					this.InitLopetLvlUpInfo();
				}
				this.mGUILopetTrainLvlUpInfo.gameObject.SetActive(true);
				break;
			case 2:
				if (this.mGUILopetTrainAwakeInfo == null)
				{
					this.InitLopetAwakeInfo();
				}
				this.mGUILopetTrainAwakeInfo.gameObject.SetActive(true);
				break;
			}
		}
	}

	private void HidePetInfos()
	{
		if (this.mGUIPetTrainBaseInfo != null)
		{
			this.mGUIPetTrainBaseInfo.gameObject.SetActive(false);
		}
		this.mGUIPetTrainLvlUpInfo.gameObject.SetActive(false);
		this.mGUIPetTrainJinjieInfo.gameObject.SetActive(false);
		this.mGUIPetTrainSkillInfo.HideSkillUpEffect();
		this.mGUIPetTrainSkillInfo.gameObject.SetActive(false);
		if (this.mGUIPetTrainJueXingInfo != null)
		{
			this.mGUIPetTrainJueXingInfo.HideEffects();
			this.mGUIPetTrainJueXingInfo.gameObject.SetActive(false);
		}
		if (this.mGUIPetTrainPeiYangInfo != null)
		{
			this.mGUIPetTrainPeiYangInfo.HidePeiYangEffects();
			this.mGUIPetTrainPeiYangInfo.gameObject.SetActive(false);
		}
	}

	private void HideLopetInfos()
	{
		if (this.mGUILopetTrainBaseInfo != null)
		{
			this.mGUILopetTrainBaseInfo.gameObject.SetActive(false);
		}
		if (this.mGUILopetTrainLvlUpInfo != null)
		{
			this.mGUILopetTrainLvlUpInfo.gameObject.SetActive(false);
		}
		if (this.mGUILopetTrainAwakeInfo != null)
		{
			this.mGUILopetTrainAwakeInfo.gameObject.SetActive(false);
		}
	}

	private void OnEnterBaseInfo()
	{
		this.SetTabStates(0);
		if (this.mCurPetDataEx != null)
		{
			this.mGUIPetTrainSkillInfo.HideSkillUpEffect();
			if (this.mGUIPetTrainPeiYangInfo != null)
			{
				this.mGUIPetTrainPeiYangInfo.HidePeiYangEffects();
			}
			if (this.mGUIPetTrainJueXingInfo != null)
			{
				this.mGUIPetTrainJueXingInfo.HideEffects();
			}
			this.mAwakeShopBtn.SetActive(false);
			Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		}
		else if (this.mCurLopetDataEx != null)
		{
			this.mAwakeShopBtn.SetActive(false);
		}
	}

	private void OnEnterLvlUp(int lvlIndex)
	{
		this.SetTabStates(1);
		if (this.mCurPetDataEx != null)
		{
			this.mGUIPetTrainLvlUpInfo.SelectCurItem(lvlIndex);
			this.mGUIPetTrainSkillInfo.HideSkillUpEffect();
			if (this.mGUIPetTrainPeiYangInfo != null)
			{
				this.mGUIPetTrainPeiYangInfo.HidePeiYangEffects();
			}
			if (this.mGUIPetTrainJueXingInfo != null)
			{
				this.mGUIPetTrainJueXingInfo.HideEffects();
			}
			this.mAwakeShopBtn.SetActive(false);
			Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		}
		else if (this.mCurLopetDataEx != null)
		{
			this.mAwakeShopBtn.SetActive(false);
		}
	}

	private void OnEnterJinJie()
	{
		this.SetTabStates(2);
		if (this.mCurPetDataEx != null)
		{
			this.mGUIPetTrainSkillInfo.HideSkillUpEffect();
			if (this.mGUIPetTrainPeiYangInfo != null)
			{
				this.mGUIPetTrainPeiYangInfo.HidePeiYangEffects();
			}
			if (this.mGUIPetTrainJueXingInfo != null)
			{
				this.mGUIPetTrainJueXingInfo.HideEffects();
			}
			this.mAwakeShopBtn.SetActive(false);
			Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		}
		else if (this.mCurLopetDataEx != null)
		{
			this.mAwakeShopBtn.SetActive(false);
		}
	}

	private void OnEnterSkill()
	{
		this.SetTabStates(3);
		if (this.mCurPetDataEx != null)
		{
			if (this.mGUIPetTrainPeiYangInfo != null)
			{
				this.mGUIPetTrainPeiYangInfo.HidePeiYangEffects();
			}
			if (this.mGUIPetTrainJueXingInfo != null)
			{
				this.mGUIPetTrainJueXingInfo.HideEffects();
			}
			this.mAwakeShopBtn.SetActive(false);
			Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		}
		else if (this.mCurLopetDataEx != null)
		{
			this.mAwakeShopBtn.SetActive(false);
		}
	}

	private void OnEnterJueXing()
	{
		this.SetTabStates(5);
		if (this.mCurPetDataEx != null)
		{
			this.mAwakeShopBtn.SetActive(true);
			if (this.mGUIPetTrainPeiYangInfo != null)
			{
				this.mGUIPetTrainPeiYangInfo.HidePeiYangEffects();
			}
			this.mGUIPetTrainSkillInfo.HideSkillUpEffect();
		}
		else if (this.mCurLopetDataEx != null)
		{
			this.mAwakeShopBtn.SetActive(false);
		}
	}

	private void OnEnterPeiYang()
	{
		this.SetTabStates(4);
		if (this.mCurPetDataEx != null)
		{
			this.mGUIPetTrainSkillInfo.HideSkillUpEffect();
			if (this.mGUIPetTrainJueXingInfo != null)
			{
				this.mGUIPetTrainJueXingInfo.HideEffects();
			}
			this.mAwakeShopBtn.SetActive(false);
		}
		else if (this.mCurLopetDataEx != null)
		{
			this.mAwakeShopBtn.SetActive(false);
		}
	}

	private void ClearModel()
	{
		if (this.asyncEntiry != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntiry);
			this.asyncEntiry = null;
		}
		if (this.mModelTmp != null)
		{
			this.mUIActorController = null;
			UnityEngine.Object.DestroyImmediate(this.mModelTmp);
			this.mModelTmp = null;
		}
	}

	private void CreateModel()
	{
		this.ClearModel();
		if (this.mCurPetDataEx != null)
		{
			if (this.mCurPetDataEx.GetSocketSlot() == 0)
			{
				this.asyncEntiry = ActorManager.CreateLocalUIActor(0, 450, true, true, this.mCardModel, 1.15f, delegate(GameObject go)
				{
					this.asyncEntiry = null;
					this.mModelTmp = go;
					if (this.mModelTmp != null)
					{
						this.mUIActorController = this.mModelTmp.GetComponent<UIActorController>();
						this.mUIActorController.PlayIdleAnimationAndVoice();
						this.mModelTmp.transform.localPosition = new Vector3(0f, 0f, -500f);
						Tools.SetMeshRenderQueue(this.mModelTmp, 3500);
					}
				});
			}
			else
			{
				this.asyncEntiry = ActorManager.CreateUIPet(this.mCurPetDataEx.Info.ID, 450, true, true, this.mCardModel, 1.15f, 2, delegate(GameObject go)
				{
					this.mModelTmp = go;
					if (this.mModelTmp != null)
					{
						this.mUIActorController = this.mModelTmp.GetComponent<UIActorController>();
						this.mUIActorController.PlayIdleAnimationAndVoice();
						this.mModelTmp.transform.localPosition = new Vector3(0f, 0f, -500f);
						Tools.SetMeshRenderQueue(this.mModelTmp, 3500);
					}
				});
			}
		}
		else if (this.mCurLopetDataEx != null)
		{
			this.asyncEntiry = ActorManager.CreateUILopet(this.mCurLopetDataEx.Info, 0, true, true, this.mCardModel, 1f, delegate(GameObject go)
			{
				this.asyncEntiry = null;
				this.mModelTmp = go;
				if (this.mModelTmp != null)
				{
					Vector3 localPosition = this.mModelTmp.transform.localPosition;
					localPosition.z -= 500f;
					this.mModelTmp.transform.localPosition = localPosition;
					Tools.SetMeshRenderQueue(this.mModelTmp, 3900);
					this.mUIActorController = this.mModelTmp.GetComponent<UIActorController>();
				}
			});
		}
	}

	private void RefreshModal()
	{
		this.CreateModel();
	}

	private void Refresh()
	{
		if (this.mCurPetDataEx != null)
		{
			this.mGUIPetTitleInfo.Refresh(this.mCurPetDataEx, true, this.mCurPetDataEx.GetSocketSlot() == 0);
			if (this.mGUIPetTrainBaseInfo != null)
			{
				this.mGUIPetTrainBaseInfo.Refresh();
			}
			this.mGUIPetTrainJinjieInfo.Refresh();
			if (this.mGUIPetTrainJueXingInfo != null)
			{
				this.mGUIPetTrainJueXingInfo.Refresh();
			}
			if (this.mGUIPetTrainPeiYangInfo != null)
			{
				this.mGUIPetTrainPeiYangInfo.Refresh();
			}
			if (this.mCurPetDataEx.GetSocketSlot() != 0)
			{
				this.mGUIPetTrainLvlUpInfo.Refresh();
				this.mGUIPetTrainSkillInfo.Refresh();
			}
		}
		else if (this.mCurLopetDataEx != null)
		{
			this.mGUILopetTitleInfo.Refresh(this.mCurLopetDataEx);
			if (this.mGUILopetTrainBaseInfo != null)
			{
				this.mGUILopetTrainBaseInfo.Refresh();
			}
			if (this.mGUILopetTrainLvlUpInfo != null)
			{
				this.mGUILopetTrainLvlUpInfo.Refresh();
			}
			else
			{
				this.RefreshLopetLvlUpNewMark();
			}
			if (this.mGUILopetTrainAwakeInfo != null)
			{
				this.mGUILopetTrainAwakeInfo.Refresh();
			}
			else
			{
				this.RefreshLopetAwakeNewMark();
			}
		}
	}

	public void PlayLvlUpMsgTip()
	{
		if (this.mCurPetDataEx != null)
		{
			this.InitPetDatas();
			this.SetTuiShiItems(null);
			this.Refresh();
			if (this.mCurPetDataEx.Data.Level > this.mOldLvl)
			{
				this.mSb.Remove(0, this.mSb.Length);
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				this.mCurPetDataEx.GetAttribute(ref num, ref num2, ref num3, ref num4);
				if (num > this.mOldHpNum)
				{
					this.mSb.Append(Singleton<StringManager>.Instance.GetString("EAID_1")).Append(" +").Append(num - this.mOldHpNum);
				}
				if (num2 > this.mOldAttackNum)
				{
					this.mSb.Append("\n").Append(Singleton<StringManager>.Instance.GetString("EAID_2")).Append(" +").Append(num2 - this.mOldAttackNum);
				}
				if (num3 > this.mOldWuFangNum)
				{
					this.mSb.Append("\n").Append(Singleton<StringManager>.Instance.GetString("EAID_3")).Append(" +").Append(num3 - this.mOldWuFangNum);
				}
				if (num4 > this.mOlfFaFangNum)
				{
					this.mSb.Append("\n").Append(Singleton<StringManager>.Instance.GetString("EAID_4")).Append(" +").Append(num4 - this.mOlfFaFangNum);
				}
				GUIUpgradeTipPopUp.ShowThis(Singleton<StringManager>.Instance.GetString("petTrainTxt3", new object[]
				{
					this.mCurPetDataEx.Data.Level
				}), this.mSb.ToString(), string.Empty, string.Empty, 5f, 1f);
			}
		}
		else if (this.mCurLopetDataEx != null)
		{
			this.Refresh();
			if (this.mCurLopetDataEx.Data.Level > this.mOldLvl)
			{
				this.mSb.Remove(0, this.mSb.Length);
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				int num8 = 0;
				this.mCurLopetDataEx.GetAttribute(ref num5, ref num6, ref num7, ref num8);
				if (num5 > this.mOldHpNum)
				{
					this.mSb.Append(Singleton<StringManager>.Instance.GetString("EAID_1")).Append(" +").Append(num5 - this.mOldHpNum);
				}
				if (num6 > this.mOldAttackNum)
				{
					this.mSb.Append("\n").Append(Singleton<StringManager>.Instance.GetString("EAID_2")).Append(" +").Append(num6 - this.mOldAttackNum);
				}
				if (num7 > this.mOldWuFangNum)
				{
					this.mSb.Append("\n").Append(Singleton<StringManager>.Instance.GetString("EAID_3")).Append(" +").Append(num7 - this.mOldWuFangNum);
				}
				if (num8 > this.mOlfFaFangNum)
				{
					this.mSb.Append("\n").Append(Singleton<StringManager>.Instance.GetString("EAID_4")).Append(" +").Append(num8 - this.mOlfFaFangNum);
				}
				GUIUpgradeTipPopUp.ShowThis(Singleton<StringManager>.Instance.GetString("petTrainTxt3", new object[]
				{
					this.mCurLopetDataEx.Data.Level
				}), this.mSb.ToString(), string.Empty, string.Empty, 5f, 1f);
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator DoPlayAwakeLvlUpMsg(uint oldAwake)
	{
        return null;
        //GUIPetTrainSceneV2.<DoPlayAwakeLvlUpMsg>c__Iterator8B <DoPlayAwakeLvlUpMsg>c__Iterator8B = new GUIPetTrainSceneV2.<DoPlayAwakeLvlUpMsg>c__Iterator8B();
        //<DoPlayAwakeLvlUpMsg>c__Iterator8B.oldAwake = oldAwake;
        //<DoPlayAwakeLvlUpMsg>c__Iterator8B.<$>oldAwake = oldAwake;
        //<DoPlayAwakeLvlUpMsg>c__Iterator8B.<>f__this = this;
        //return <DoPlayAwakeLvlUpMsg>c__Iterator8B;
	}

	public void PlayAwakeLvlUpMsgTip(uint oldAwake)
	{
		this.Refresh();
		base.StartCoroutine(this.DoPlayAwakeLvlUpMsg(oldAwake));
	}

	private void OnLevelupPetEvent(PetDataEx pdEx)
	{
		if (pdEx != null)
		{
			this.mCurPetDataEx = pdEx;
			this.mGUIPetTrainLvlUpInfo.PlayLvlUpEffectAnimation();
		}
	}

	private void OnFurtherPetEvent(PetDataEx pdEx)
	{
		if (pdEx != null)
		{
			this.mCurPetDataEx = pdEx;
			this.Refresh();
			GameUIManager.mInstance.ShowPetFurtherSucV2(pdEx);
		}
	}

	private void OnSkillPetEvent(PetDataEx pdEx)
	{
		if (pdEx != null)
		{
			this.mCurPetDataEx = pdEx;
			this.Refresh();
			this.mGUIPetTrainSkillInfo.PlaySkillUpEffect();
			Globals.Instance.TutorialMgr.InitializationCompleted(this.mGUIPetTrainSkillInfo, null);
		}
	}

	private void OnAwakeLevelUpEvent(PetDataEx pdEx)
	{
		if (pdEx != null)
		{
			this.mCurPetDataEx = pdEx;
			if (this.mGUIPetTrainJueXingInfo != null)
			{
				this.mGUIPetTrainJueXingInfo.PlayAwakeLvlUpEffect();
			}
			Globals.Instance.EffectSoundMgr.Play("ui/ui_027a");
		}
	}

	private void OnAwakeItemEvent(PetDataEx pdEx, int slot)
	{
		if (pdEx != null)
		{
			GUIAwakeItemInfoPopUp.TryClose();
			this.mCurPetDataEx = pdEx;
			this.Refresh();
			if (slot >= 0 && slot < 4)
			{
				if (this.mGUIPetTrainJueXingInfo != null)
				{
					this.mGUIPetTrainJueXingInfo.PlayEquipEffect(slot);
				}
				this.PlayAwakeItemMsgTip(slot);
			}
		}
	}

	public void PlayAwakeItemMsgTip(int slot)
	{
		int awakeItemID = this.CurPetDataEx.GetAwakeItemID(slot);
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(awakeItemID);
		if (info != null)
		{
			this.mSb.Remove(0, this.mSb.Length);
			this.mSb.Append(Tools.GetEAttIDName(1)).Append(" +").Append(Tools.GetEAttIDValue(1, info.Value3));
			this.mSb.Append("\n").Append(Tools.GetEAttIDName(2)).Append(" +").Append(Tools.GetEAttIDValue(2, info.Value1));
			this.mSb.Append("\n").Append(Tools.GetEAttIDName(20)).Append(" +").Append(Tools.GetEAttIDValue(20, info.Value2));
			if (this.mSb.Length > 0)
			{
				GUIUpgradeTipPopUp.ShowThis(Singleton<StringManager>.Instance.GetString("petJueXing8"), this.mSb.ToString(), string.Empty, string.Empty, 5f, 1f);
			}
		}
	}

	private void OnMsgAwakeItemCreate(MemoryStream stream)
	{
		if (this.mGUIPetTrainJueXingInfo != null)
		{
			this.mGUIPetTrainJueXingInfo.Refresh();
		}
	}

	public void PlayModelEffect()
	{
		if (this.mCurPetDataEx != null)
		{
			this.mGUIPetTrainLvlUpInfo.PlayExpBarEffect();
			if (this.mCurPetDataEx.Data.Level > this.mOldLvl)
			{
				Globals.Instance.EffectSoundMgr.Play("ui/ui_014");
				NGUITools.SetActive(this.mModelLvlupEffect, false);
				NGUITools.SetActive(this.mModelLvlupEffect, true);
			}
			else
			{
				NGUITools.SetActive(this.mModelLvlupEffect2, false);
				NGUITools.SetActive(this.mModelLvlupEffect2, true);
			}
		}
		else if (this.mCurLopetDataEx != null)
		{
			this.mGUILopetTrainLvlUpInfo.PlayExpBarEffect();
			if (this.mCurLopetDataEx.Data.Level > this.mOldLvl)
			{
				Globals.Instance.EffectSoundMgr.Play("ui/ui_014");
				NGUITools.SetActive(this.mModelLvlupEffect, false);
				NGUITools.SetActive(this.mModelLvlupEffect, true);
			}
			else
			{
				NGUITools.SetActive(this.mModelLvlupEffect2, false);
				NGUITools.SetActive(this.mModelLvlupEffect2, true);
			}
		}
	}

	private void OnPlayerUpdateEvent()
	{
		this.Refresh();
	}

	private void OnDataInitEvent(bool versionInit, bool newPlayer)
	{
		int curPageIndex = this.GetCurPageIndex();
		if (curPageIndex == 4)
		{
			this.CurPetDataEx = Globals.Instance.Player.PetSystem.GetPet(this.CurPetDataEx.Data.ID);
		}
	}

	private void OnAwakeShopBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.CurPetDataEx;
		GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = this.GetCurPageIndex();
		GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
		GUIShopScene.TryOpen(EShopType.EShop_Awaken);
	}

	private void InitPeiYangInfo()
	{
		GameObject gameObject = Res.LoadGUI("GUI/peiYangInfo");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.LoadGUI GUI/peiYangInfo error"
			});
			return;
		}
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
		gameObject2.SetActive(true);
		GameUITools.AddChild(this.mRightInfoGo, gameObject2);
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localScale = Vector3.one;
		this.mGUIPetTrainPeiYangInfo = gameObject2.AddComponent<GUIPetTrainPeiYangInfo>();
		this.mGUIPetTrainPeiYangInfo.InitWithBaseScene(this);
		this.mGUIPetTrainPeiYangInfo.Refresh();
	}

	private void InitBaseInfo()
	{
		GameObject gameObject = Res.LoadGUI("GUI/petTrainBaseInfo");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.LoadGUI GUI/petTrainBaseInfo error"
			});
			return;
		}
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
		gameObject2.SetActive(true);
		GameUITools.AddChild(this.mRightInfoGo, gameObject2);
		gameObject2.transform.localPosition = new Vector3(0f, -6f, 0f);
		gameObject2.transform.localScale = Vector3.one;
		this.mGUIPetTrainBaseInfo = gameObject2.AddComponent<GUIPetTrainBaseInfo>();
		this.mGUIPetTrainBaseInfo.InitWithBaseScene(this);
		this.mGUIPetTrainBaseInfo.Refresh();
	}

	public void RefreshPeiYangNewMark()
	{
		this.ShowPetPeiYangNewMark = false;
		if (Tools.CanPlay(GameConst.GetInt32(122), true))
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(178));
			if (info != null)
			{
				int itemCount = Globals.Instance.Player.ItemSystem.GetItemCount(info.ID);
				int @int = GameConst.GetInt32(179);
				this.ShowPetPeiYangNewMark = (itemCount >= @int);
			}
		}
	}

	public void RefreshJueXingNewMark()
	{
		this.ShowPetJueXingNewMark = Tools.CanShowJueXingMark(this.CurPetDataEx);
	}

	private void InitJueXingInfo()
	{
		GameObject gameObject = Res.LoadGUI("GUI/petJueXingInfo");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.LoadGUI GUI/petJueXingInfo error"
			});
			return;
		}
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
		gameObject2.SetActive(true);
		GameUITools.AddChild(this.mRightInfoGo, gameObject2);
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localScale = Vector3.one;
		this.mGUIPetTrainJueXingInfo = gameObject2.AddComponent<GUIPetTrainJueXingInfo>();
		this.mGUIPetTrainJueXingInfo.InitWithBaseScene(this);
		this.mGUIPetTrainJueXingInfo.Refresh();
	}

	private void OnPetCultivateEvent(PetDataEx pdEx)
	{
		if (this.mGUIPetTrainPeiYangInfo != null)
		{
			this.mGUIPetTrainPeiYangInfo.Refresh();
			this.mGUIPetTrainPeiYangInfo.RefreshPeiYangAni();
		}
	}

	private void OnPetCultivateAckEvent(PetDataEx pdEx)
	{
		if (this.mGUIPetTrainPeiYangInfo != null)
		{
			this.mGUIPetTrainPeiYangInfo.Refresh();
			this.mGUIPetTrainPeiYangInfo.PlayPeiYangEffect();
			this.mGUIPetTrainPeiYangInfo.ClosePeiYangAni();
		}
	}

	public void SetPeiYangNum(int num)
	{
		if (this.mGUIPetTrainPeiYangInfo != null)
		{
			this.mGUIPetTrainPeiYangInfo.SetCiShuNum(num);
		}
	}

	public void RefreshLopetLvlUpNewMark()
	{
		this.ShowLopetLvlUpNewMark = Tools.CanLopetLevelUp(this.mCurLopetDataEx);
	}

	public void RefreshLopetAwakeNewMark()
	{
		this.ShowLopetAwakeNewMark = Tools.CanLopetAwake(this.mCurLopetDataEx);
	}

	private void OnLevelupLopetEvent(LopetDataEx data)
	{
		if (data != null)
		{
			this.mCurLopetDataEx = data;
			this.mGUILopetTrainLvlUpInfo.PlayLvlUpEffectAnimation();
		}
	}

	private void OnAwakeLopetEvent(LopetDataEx data)
	{
		if (data != null)
		{
			this.mCurLopetDataEx = data;
			this.Refresh();
			GUILopetAwakeSuccess.Show(data);
		}
	}

	private void InitLopetBaseInfo()
	{
		GameObject gameObject = Res.LoadGUI("GUI/lopetTrainBaseInfo");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.LoadGUI GUI/lopetTrainBaseInfo error"
			});
			return;
		}
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
		gameObject2.SetActive(true);
		GameUITools.AddChild(this.mRightInfoGo, gameObject2);
		gameObject2.transform.localPosition = new Vector3(0f, -6f, 0f);
		gameObject2.transform.localScale = Vector3.one;
		this.mGUILopetTrainBaseInfo = gameObject2.AddComponent<GUILopetTrainBaseInfo>();
		this.mGUILopetTrainBaseInfo.InitWithBaseScene(this);
		this.mGUILopetTrainBaseInfo.Refresh();
	}

	private void InitLopetLvlUpInfo()
	{
		GameObject gameObject = Res.LoadGUI("GUI/lopetTrainLvlUpInfo");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.LoadGUI GUI/lopetTrainLvlUpInfo error"
			});
			return;
		}
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
		gameObject2.SetActive(true);
		GameUITools.AddChild(this.mRightInfoGo, gameObject2);
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject2.transform.localScale = Vector3.one;
		this.mGUILopetTrainLvlUpInfo = gameObject2.AddComponent<GUILopetTrainLvlUpInfo>();
		this.mGUILopetTrainLvlUpInfo.InitWithBaseScene(this);
		this.mGUILopetTrainLvlUpInfo.Refresh();
	}

	private void InitLopetAwakeInfo()
	{
		GameObject gameObject = Res.LoadGUI("GUI/lopetTrainAwakeInfo");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.LoadGUI GUI/lopetTrainAwakeInfo error"
			});
			return;
		}
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
		gameObject2.SetActive(true);
		GameUITools.AddChild(this.mRightInfoGo, gameObject2);
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
		gameObject2.transform.localScale = Vector3.one;
		this.mGUILopetTrainAwakeInfo = gameObject2.AddComponent<GUILopetTrainAwakeInfo>();
		this.mGUILopetTrainAwakeInfo.InitWithBaseScene(this);
		this.mGUILopetTrainAwakeInfo.Refresh();
	}
}
