using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class GUITeamManageSceneV2 : GameUISession
{
	private const int CanSelectItems = 6;

	private bool mIsLocalPlayer = true;

	private GUITeamManageSelectItem[] mSelectItems = new GUITeamManageSelectItem[6];

	private GameObject mTextureBgGo;

	private UITexture mTextureBg;

	private GameObject mLopetBG;

	private GUIPetTitleInfo mGUIPetTitleInfo;

	private GameObject mCommonPetGo;

	private GameObject mShiZhuangBtn;

	private GameObject mShiZhuangBtnEffect;

	private GameObject mChangePetBtn;

	private GameObject mChangePetBtnEffect;

	private GUITeamManageEquipItem[] mEquipItems = new GUITeamManageEquipItem[6];

	private UILabel mHpNum;

	private UILabel mAttackNum;

	private UILabel mWuFangNum;

	private UILabel mFaFangNum;

	private UILabel mYuanFenDesc;

	private UITable mRightInfoTable;

	private GameObject mSkillGo;

	private GUIPetInfoSkillLayer mSkillLayer;

	private UIGrid mModelTable;

	private UIScrollView mUIScrollView;

	public GUICenterModelItem mModelCenterChild;

	private GUITeamManageModelItem[] mTeamModelItems = new GUITeamManageModelItem[6];

	private Transform[] mModelWidgets = new Transform[6];

	private GameObject mLittlePet;

	private TeamLopetLayer mLopetLayer;

	private GUITeamManageAssitPetItem[] mAssistPets = new GUITeamManageAssitPetItem[6];

	private UILabel mPetName;

	private UITable mGUIPetYuanFenItemTable;

	private UIScrollView mPetYuanFenItemScrollView;

	private UnityEngine.Object mRecordItemOriginal;

	private GameObject mPetZhuWeiBtn;

	private GameObject mPetZhuWeiMark;

	private GUIAttributeTip mGUIAttributeTip;

	private GUIAttributeTip mGUITipForRelation;

	private uint mEnhanceTotalCost;

	private string mTeamManage6Str;

	private List<string> mTipContents = new List<string>();

	private GameObject mYangChengBtn;

	private GameObject mYangChengMark;

	private GameObject mEnhanceBtn;

	private GameObject mHuanZhuangBtn;

	private GameObject mEnhanceAllBtn;

	private GUISimpleSM<string, string> mGUISimpleSM;

	private StringBuilder mSb = new StringBuilder();

	private float mUpdateTimer;

	private bool mIsDragging;

	private int oldEquipEnhanceMasterLevel;

	private UIGrid mLeftContent;

	private UIScrollView mLeftScrollView;

	public bool IsLocalPlayer
	{
		get
		{
			return this.mIsLocalPlayer;
		}
		set
		{
			this.mIsLocalPlayer = value;
		}
	}

	public GUITeamManageEquipItem[] GetEquipItems()
	{
		return this.mEquipItems;
	}

	private void CreateObjects()
	{
		this.mTextureBgGo = base.transform.Find("TextureBG/Texture").gameObject;
		this.mLopetBG = base.transform.Find("LopetBG").gameObject;
		Transform transform = base.transform.Find("UIMiddle");
		this.mTextureBg = transform.Find("Texture").GetComponent<UITexture>();
		Material material = new Material(Shader.Find("Game/Transparent/Panel TextureMask"));
		this.mTextureBg.material = material;
		UIPanel component = transform.Find("commonPet/midInfo/modelsBg/contentsPanel").GetComponent<UIPanel>();
		Vector4 finalClipRegion = component.finalClipRegion;
		finalClipRegion.x -= 40f;
		finalClipRegion.z = (finalClipRegion.z + 200f) * 0.5f;
		finalClipRegion.w = (finalClipRegion.w + 1024f) * 0.5f;
		this.mTextureBg.material.SetVector("_ClipRange0", new Vector4(-finalClipRegion.x / finalClipRegion.z, -finalClipRegion.y / finalClipRegion.w, 1f / finalClipRegion.z, 1f / finalClipRegion.w));
		this.mSelectItems[0] = transform.Find("pet0").gameObject.AddComponent<GUITeamManageSelectItem>();
		this.mLeftScrollView = transform.Find("leftInfo").GetComponent<UIScrollView>();
		Transform transform2 = this.mLeftScrollView.transform.Find("Content");
		this.mLeftContent = transform2.GetComponent<UIGrid>();
		for (int i = 1; i < 6; i++)
		{
			this.mSelectItems[i] = transform2.Find(string.Format("pet{0}", i)).gameObject.AddComponent<GUITeamManageSelectItem>();
		}
		this.mCommonPetGo = transform.Find("commonPet").gameObject;
		this.mGUIPetTitleInfo = this.mCommonPetGo.transform.Find("topInfo").gameObject.AddComponent<GUIPetTitleInfo>();
		this.mGUIPetTitleInfo.InitWithBaseScene();
		Transform transform3 = this.mCommonPetGo.transform.Find("midInfo");
		this.mUIScrollView = transform3.Find("modelsBg/contentsPanel").GetComponent<UIScrollView>();
		UIScrollView expr_23E = this.mUIScrollView;
		expr_23E.onDragStarted = (UIScrollView.OnDragNotification)Delegate.Combine(expr_23E.onDragStarted, new UIScrollView.OnDragNotification(this.OnModelSWDragStarted));
		UIScrollView expr_265 = this.mUIScrollView;
		expr_265.onStoppedMoving = (UIScrollView.OnDragNotification)Delegate.Combine(expr_265.onStoppedMoving, new UIScrollView.OnDragNotification(this.OnModelSWDragFinished));
		this.mModelTable = transform3.Find("modelsBg/contentsPanel/contents").gameObject.AddComponent<UIGrid>();
		this.mModelTable.arrangement = UIGrid.Arrangement.Horizontal;
		this.mModelTable.cellWidth = 450f;
		this.mModelTable.cellHeight = 360f;
		this.mModelTable.animateSmoothly = true;
		this.mModelTable.hideInactive = true;
		this.mModelTable.keepWithinPanel = true;
		this.mModelTable.sorting = UIGrid.Sorting.Alphabetic;
		this.mModelTable.enabled = false;
		GameObject gameObject = transform3.Find("modelsBg/contentsPanel/contents").gameObject;
		this.mModelCenterChild = gameObject.AddComponent<GUICenterModelItem>();
		this.mModelCenterChild.Init();
		this.mModelCenterChild.nextPageThreshold = 120f;
		this.mModelCenterChild.springStrength = 8f;
		for (int j = 0; j < 6; j++)
		{
			this.mTeamModelItems[j] = gameObject.transform.Find(string.Format("teamManageModel{0}", j)).gameObject.AddComponent<GUITeamManageModelItem>();
			this.mTeamModelItems[j].InitWithBaseScene(this);
			this.mModelWidgets[j] = this.mTeamModelItems[j].transform;
		}
		if (Tools.CanPlay(GameConst.GetInt32(201), this.IsLocalPlayer))
		{
			this.mSelectItems[4].gameObject.SetActive(true);
			this.mTeamModelItems[4].gameObject.SetActive(true);
		}
		else
		{
			this.mSelectItems[4].gameObject.SetActive(false);
			this.mTeamModelItems[4].gameObject.SetActive(false);
		}
		for (int k = 0; k < 6; k++)
		{
			this.mEquipItems[k] = transform3.Find(string.Format("item{0}", k)).gameObject.AddComponent<GUITeamManageEquipItem>();
			this.mEquipItems[k].InitWithBaseScene(this, k);
		}
		this.mYangChengBtn = transform3.Find("yangCBtn").gameObject;
		UIEventListener expr_4A0 = UIEventListener.Get(this.mYangChengBtn);
		expr_4A0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4A0.onClick, new UIEventListener.VoidDelegate(this.OnYangCBtnClick));
		this.mYangChengMark = this.mYangChengBtn.transform.Find("Effect").gameObject;
		this.mYangChengMark.SetActive(false);
		this.mEnhanceBtn = transform3.Find("enhanceBtn").gameObject;
		UIEventListener expr_50F = UIEventListener.Get(this.mEnhanceBtn);
		expr_50F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_50F.onClick, new UIEventListener.VoidDelegate(this.OnEnhanceBtnClick));
		this.mShiZhuangBtn = transform3.Find("shiZhuangBtn").gameObject;
		UIEventListener expr_552 = UIEventListener.Get(this.mShiZhuangBtn);
		expr_552.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_552.onClick, new UIEventListener.VoidDelegate(this.OnShiZhuangBtnClick));
		this.mShiZhuangBtnEffect = this.mShiZhuangBtn.transform.Find("Effect").gameObject;
		this.mShiZhuangBtnEffect.SetActive(false);
		this.mShiZhuangBtn.SetActive(false);
		this.mChangePetBtn = transform3.Find("changePetBtn").gameObject;
		UIEventListener expr_5CD = UIEventListener.Get(this.mChangePetBtn);
		expr_5CD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_5CD.onClick, new UIEventListener.VoidDelegate(this.OnChangePetBtnClick));
		this.mChangePetBtnEffect = this.mChangePetBtn.transform.Find("Effect").gameObject;
		this.mChangePetBtnEffect.SetActive(false);
		this.mChangePetBtn.SetActive(false);
		this.mEnhanceAllBtn = transform3.Find("enhanceAllBtn").gameObject;
		UIEventListener expr_648 = UIEventListener.Get(this.mEnhanceAllBtn);
		expr_648.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_648.onClick, new UIEventListener.VoidDelegate(this.OnEnhanceAllBtnClick));
		this.mHuanZhuangBtn = transform3.Find("huanZhuangBtn").gameObject;
		UIEventListener expr_68B = UIEventListener.Get(this.mHuanZhuangBtn);
		expr_68B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_68B.onClick, new UIEventListener.VoidDelegate(this.OnHuanZhuangBtnClick));
		Transform transform4 = this.mCommonPetGo.transform.Find("rightInfo");
		this.mRightInfoTable = transform4.Find("rightInfoPanel/contents").GetComponent<UITable>();
		this.mSkillGo = this.mRightInfoTable.transform.Find("b").gameObject;
		this.mSkillLayer = this.mSkillGo.transform.Find("summonSkill").gameObject.AddComponent<GUIPetInfoSkillLayer>();
		this.mSkillLayer.Init(false, false);
		this.mSkillGo.SetActive(false);
		Transform transform5 = this.mRightInfoTable.transform.Find("a");
		this.mHpNum = transform5.Find("hpTxt/num").GetComponent<UILabel>();
		this.mHpNum.text = string.Empty;
		this.mAttackNum = transform5.Find("attackTxt/num").GetComponent<UILabel>();
		this.mAttackNum.text = string.Empty;
		this.mWuFangNum = transform5.Find("wufangTxt/num").GetComponent<UILabel>();
		this.mWuFangNum.text = string.Empty;
		this.mFaFangNum = transform5.Find("fafangTxt/num").GetComponent<UILabel>();
		this.mFaFangNum.text = string.Empty;
		Transform transform6 = this.mRightInfoTable.transform.Find("c");
		UIEventListener expr_80E = UIEventListener.Get(transform6.gameObject);
		expr_80E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_80E.onClick, new UIEventListener.VoidDelegate(this.OnPetInfoCPartClick));
		this.mYuanFenDesc = transform6.Find("yuanfenDesc").GetComponent<UILabel>();
		UIEventListener expr_856 = UIEventListener.Get(this.mYuanFenDesc.gameObject);
		expr_856.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_856.onClick, new UIEventListener.VoidDelegate(this.OnYuanFenClick));
		this.mYuanFenDesc.text = string.Empty;
		this.mLittlePet = transform.Find("littlePet").gameObject;
		this.mPetZhuWeiBtn = this.mLittlePet.transform.Find("zhuWeiBtn").gameObject;
		this.mLopetLayer = transform.Find("Lopet").gameObject.AddComponent<TeamLopetLayer>();
		this.mLopetLayer.gameObject.SetActive(false);
		this.mPetZhuWeiMark = this.mPetZhuWeiBtn.transform.Find("mark").gameObject;
		this.mPetZhuWeiMark.SetActive(false);
		UIEventListener expr_920 = UIEventListener.Get(this.mPetZhuWeiBtn);
		expr_920.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_920.onClick, new UIEventListener.VoidDelegate(this.OnPetZhuWeiBtnClick));
		this.mPetZhuWeiBtn.SetActive(this.IsLocalPlayer && Tools.CanPlay(GameConst.GetInt32(29), true));
		Transform transform7 = this.mLittlePet.transform.Find("pets");
		for (int l = 0; l < 6; l++)
		{
			this.mAssistPets[l] = transform7.Find(string.Format("pet{0}", l)).gameObject.AddComponent<GUITeamManageAssitPetItem>();
			this.mAssistPets[l].InitWithBaseScene(this, l);
		}
		Transform transform8 = this.mLittlePet.transform.Find("yuanFenInfo");
		this.mPetYuanFenItemScrollView = transform8.Find("contentsPanel").GetComponent<UIScrollView>();
		this.mGUIPetYuanFenItemTable = transform8.Find("contentsPanel/contents").gameObject.AddComponent<UITable>();
		this.mGUIPetYuanFenItemTable.columns = 1;
		this.mGUIPetYuanFenItemTable.direction = UITable.Direction.Down;
		this.mGUIPetYuanFenItemTable.sorting = UITable.Sorting.None;
		this.mGUIPetYuanFenItemTable.hideInactive = true;
		this.mGUIPetYuanFenItemTable.keepWithinPanel = true;
		this.mGUIPetYuanFenItemTable.padding = new Vector2(0f, 0f);
		this.mGUISimpleSM = new GUISimpleSM<string, string>("init");
		this.mGUISimpleSM.Configure("init").Permit("showAvatar", "avatar").Permit("showPet0", "pet0").Permit("showPet1", "pet1").Permit("showPet2", "pet2").Permit("showPet3", "pet3").Permit("showLopet", "lopet");
		this.mGUISimpleSM.Configure("avatar").Permit("showPet0", "pet0").Permit("showPet1", "pet1").Permit("showPet2", "pet2").Permit("showPet3", "pet3").Permit("showLopet", "lopet").PermitReentry("showAvatar").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterAvatar();
		});
		this.mGUISimpleSM.Configure("pet0").Permit("showAvatar", "avatar").Permit("showPet1", "pet1").Permit("showPet2", "pet2").Permit("showPet3", "pet3").Permit("showLopet", "lopet").Ignore("showPet0").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterPet0();
		});
		this.mGUISimpleSM.Configure("pet1").Permit("showAvatar", "avatar").Permit("showPet0", "pet0").Permit("showPet2", "pet2").Permit("showPet3", "pet3").Permit("showLopet", "lopet").Ignore("showPet1").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterPet1();
		});
		this.mGUISimpleSM.Configure("pet2").Permit("showAvatar", "avatar").Permit("showPet0", "pet0").Permit("showPet1", "pet1").Permit("showPet3", "pet3").Permit("showLopet", "lopet").Ignore("showPet2").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterPet2();
		});
		this.mGUISimpleSM.Configure("lopet").Permit("showAvatar", "avatar").Permit("showPet0", "pet0").Permit("showPet1", "pet1").Permit("showPet2", "pet2").Permit("showPet3", "pet3").Ignore("showLopet").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterLopet();
		});
		this.mGUISimpleSM.Configure("pet3").Permit("showAvatar", "avatar").Permit("showPet0", "pet0").Permit("showPet1", "pet1").Permit("showPet2", "pet2").Permit("showLopet", "lopet").Ignore("showPet3").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterPet3();
		});
	}

	public void SetUIState()
	{
		if (GameUIManager.mInstance.uiState.CombatPetSlot < 4)
		{
			this.mCommonPetGo.SetActive(true);
			this.mLittlePet.SetActive(false);
			this.mTextureBgGo.SetActive(true);
			this.ShowLopet(false);
		}
		else if (GameUIManager.mInstance.uiState.CombatPetSlot == 4)
		{
			this.mCommonPetGo.SetActive(false);
			this.mLittlePet.SetActive(false);
			this.mTextureBgGo.SetActive(false);
			this.ShowLopet(true);
		}
		else
		{
			this.mCommonPetGo.SetActive(false);
			this.mLittlePet.SetActive(true);
			this.mTextureBgGo.SetActive(false);
			this.ShowLopet(false);
		}
		this.mHuanZhuangBtn.SetActive(this.IsLocalPlayer);
		this.mEnhanceAllBtn.SetActive(this.IsLocalPlayer);
		for (int i = 0; i < 6; i++)
		{
			if (this.mSelectItems[i] != null)
			{
				this.mSelectItems[i].InitWithBaseScene(this, i);
			}
		}
		this.InitModelDatas();
		if (this.IsLocalPlayer)
		{
			this.mTeamManage6Str = Singleton<StringManager>.Instance.GetString("teamManage6");
			base.StartCoroutine(this.ShowActiveRelationDesc());
		}
		this.SetCurSelectItem(GameUIManager.mInstance.uiState.CombatPetSlot);
	}

	protected override void OnPostLoadGUI()
	{
		this.IsLocalPlayer = GameUIManager.mInstance.uiState.IsLocalPlayer;
		this.CreateObjects();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("team");
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		TeamSubSystem expr_4F = Globals.Instance.Player.TeamSystem;
		expr_4F.EquipItemEvent = (TeamSubSystem.ItemUpdateCallback)Delegate.Combine(expr_4F.EquipItemEvent, new TeamSubSystem.ItemUpdateCallback(this.OnEquipItemEvent));
		TeamSubSystem expr_7F = Globals.Instance.Player.TeamSystem;
		expr_7F.EquipPetEvent = (TeamSubSystem.PetUpdateCallback)Delegate.Combine(expr_7F.EquipPetEvent, new TeamSubSystem.PetUpdateCallback(this.OnEquipPetEvent));
		Globals.Instance.CliSession.Register(521, new ClientSession.MsgHandler(this.OnMsgEquipEnhance));
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		this.SetUIState();
		base.StartCoroutine(this.TryOpenMaster());
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_GoldEquipSet, 0f);
	}

	[DebuggerHidden]
	private IEnumerator TryOpenMaster()
	{
        return null;
        //GUITeamManageSceneV2.<TryOpenMaster>c__Iterator97 <TryOpenMaster>c__Iterator = new GUITeamManageSceneV2.<TryOpenMaster>c__Iterator97();
        //<TryOpenMaster>c__Iterator.<>f__this = this;
        //return <TryOpenMaster>c__Iterator;
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.uiState.mOldHpNum = 0;
		GameUIManager.mInstance.uiState.mOldAttackNum = 0;
		GameUIManager.mInstance.uiState.mOldWufangNum = 0;
		GameUIManager.mInstance.uiState.mOldFafangNum = 0;
		for (int i = 0; i < 4; i++)
		{
			GameUIManager.mInstance.uiState.mOldRelationFlags[i] = -1;
			GameUIManager.mInstance.uiState.mOldRelationPetInfoIds[i] = -1;
		}
		if (this.mGUIAttributeTip != null)
		{
			this.mGUIAttributeTip.DestroySelf();
		}
		if (this.mGUITipForRelation != null)
		{
			this.mGUITipForRelation.DestroySelf();
		}
		GameUIManager.mInstance.GetTopGoods().Hide();
		TeamSubSystem expr_CC = Globals.Instance.Player.TeamSystem;
		expr_CC.EquipItemEvent = (TeamSubSystem.ItemUpdateCallback)Delegate.Remove(expr_CC.EquipItemEvent, new TeamSubSystem.ItemUpdateCallback(this.OnEquipItemEvent));
		TeamSubSystem expr_FC = Globals.Instance.Player.TeamSystem;
		expr_FC.EquipPetEvent = (TeamSubSystem.PetUpdateCallback)Delegate.Remove(expr_FC.EquipPetEvent, new TeamSubSystem.PetUpdateCallback(this.OnEquipPetEvent));
		Globals.Instance.CliSession.Unregister(521, new ClientSession.MsgHandler(this.OnMsgEquipEnhance));
	}

	private void ShowAllModelItem()
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.mTeamModelItems[i] != null)
			{
				this.mTeamModelItems[i].SetItemShow(true);
			}
		}
	}

	private void OnModelSWDragStarted()
	{
		this.mIsDragging = true;
		this.ShowAllModelItem();
	}

	private void OnModelSWDragFinished()
	{
		this.mIsDragging = false;
		this.mUpdateTimer = 0.6f;
	}

	private void ShowCurModelItem(int index)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.mTeamModelItems[i] != null)
			{
				this.mTeamModelItems[i].SetItemShow(i == index);
			}
		}
	}

	private void CenterChild(int index, bool isLittlePet)
	{
		if (!isLittlePet)
		{
			this.mModelCenterChild.CenterOn(this.mModelWidgets[index]);
		}
		else
		{
			this.mUIScrollView.SetDragAmount(Mathf.Clamp01(0.2f * (float)index), 0f, false);
			this.mUpdateTimer = 0.6f;
		}
	}

	private void InitModelDatas()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem != null)
		{
			for (int i = 0; i < 4; i++)
			{
				SocketDataEx socket = teamSystem.GetSocket(i, this.IsLocalPlayer);
				this.mTeamModelItems[i].Refresh(new GUITeamManageModelData(socket, i));
			}
			this.mTeamModelItems[4].Refresh(new GUITeamManageModelData(null, 4));
			this.mTeamModelItems[5].Refresh(new GUITeamManageModelData(null, 5));
		}
		if (GameUIManager.mInstance.uiState.CombatPetSlot < 4)
		{
			this.mUIScrollView.SetDragAmount(Mathf.Clamp01(0.25f * (float)GameUIManager.mInstance.uiState.CombatPetSlot), 0f, false);
			this.ShowCurModelItem(GameUIManager.mInstance.uiState.CombatPetSlot);
		}
	}

	private void OnYangCBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.IsLocalPlayer)
		{
			PetDataEx curSelectPetData = this.GetCurSelectPetData();
			if (curSelectPetData != null)
			{
				GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.GetCurSelectPetData();
				GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 0;
				GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
				GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("teamManage2", 0f, 0f);
			}
		}
	}

	private void OnEnhanceBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.IsLocalPlayer)
		{
			GUIEquipMasterInfoPopUp.ShowThis(this.GetCurSelectSocketData(), 0);
		}
	}

	private void OnShiZhuangBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.IsLocalPlayer)
		{
			GameUIManager.mInstance.ChangeSession<GUIShiZhuangSceneV2>(null, false, true);
		}
	}

	private void OnChangePetBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.IsLocalPlayer)
		{
			if (Globals.Instance.Player.PetSystem.GetUnBattlePetNum() == 0)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("hasNoCurPet", 0f, 0f);
				return;
			}
			int curSelectIndex = this.GetCurSelectIndex();
			if (0 < curSelectIndex && curSelectIndex < 4)
			{
				GameCache.Data.HasShowChangePetMark = false;
				GameCache.UpdateNow = true;
				GameUIManager.mInstance.uiState.CombatPetSlot = curSelectIndex;
				GameUIManager.mInstance.ChangeSession<GUIPartnerFightScene>(null, false, false);
			}
		}
	}

	private uint GetEquipEnhanceMoney(ItemInfo info, int enhanceLvl)
	{
		LevelInfo info2 = Globals.Instance.AttDB.LevelDict.GetInfo(enhanceLvl);
		if (info2 != null && info.Quality >= 0 && info.Quality < info2.EnhanceCost.Count)
		{
			return info2.EnhanceCost[info.Quality];
		}
		return 0u;
	}

	private void OnEnhanceAllBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.IsLocalPlayer)
		{
			SocketDataEx curSelectSocketData = this.GetCurSelectSocketData();
			if (curSelectSocketData != null && curSelectSocketData.GetPet() != null)
			{
				if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(18)))
				{
					GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WBTip1", new object[]
					{
						GameConst.GetInt32(18)
					}), 0f, 0f);
					return;
				}
				List<ItemDataEx> list = new List<ItemDataEx>();
				for (int i = 0; i < 4; i++)
				{
					ItemDataEx equip = curSelectSocketData.GetEquip(i);
					if (equip != null)
					{
						list.Add(equip);
					}
				}
				if (list.Count == 0)
				{
					GameUIManager.mInstance.ShowMessageTipByKey("equipImprove70", 0f, 0f);
					return;
				}
				bool flag = true;
				for (int j = 0; j < list.Count; j++)
				{
					ItemDataEx itemDataEx = list[j];
					if (itemDataEx != null && itemDataEx.CanEnhance())
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					GameUIManager.mInstance.ShowMessageTipByKey("equipImprove28", 0f, 0f);
					return;
				}
				int[] array = new int[4];
				for (int k = 0; k < 4; k++)
				{
					ItemDataEx equip2 = curSelectSocketData.GetEquip(k);
					if (equip2 != null)
					{
						array[k] = equip2.GetEquipEnhanceLevel();
					}
					else
					{
						array[k] = -1;
					}
				}
				this.mEnhanceTotalCost = 0u;
				int money = Globals.Instance.Player.Data.Money;
				int maxEquipEnhanceLevel = Globals.Instance.Player.ItemSystem.GetMaxEquipEnhanceLevel(true);
				int num = 0;
				uint equipEnhanceMoney;
				while (true)
				{
					int num2 = maxEquipEnhanceLevel;
					for (int l = 0; l < 4; l++)
					{
						if (num2 > array[l] && array[l] != -1)
						{
							num2 = array[l];
							num = l;
						}
					}
					ItemDataEx equip3 = curSelectSocketData.GetEquip(num);
					if (equip3 != null)
					{
						equipEnhanceMoney = this.GetEquipEnhanceMoney(equip3.Info, num2);
						if ((long)money < (long)((ulong)(this.mEnhanceTotalCost + equipEnhanceMoney)) || num2 >= maxEquipEnhanceLevel)
						{
							break;
						}
						this.mEnhanceTotalCost += equipEnhanceMoney;
						array[num]++;
					}
				}
				if (this.mEnhanceTotalCost == 0u)
				{
					this.mEnhanceTotalCost = equipEnhanceMoney;
				}
				string @string = Singleton<StringManager>.Instance.GetString("equipImprove71", new object[]
				{
					((long)Globals.Instance.Player.Data.Money >= (long)((ulong)this.mEnhanceTotalCost)) ? "[00ff00]" : "[ff0000]",
					this.mEnhanceTotalCost
				});
				GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.OKCancel, null);
				GameMessageBox expr_2E9 = gameMessageBox;
				expr_2E9.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_2E9.OkClick, new MessageBox.MessageDelegate(this.OnSureEnhanceAllClick));
			}
		}
	}

	private void OnSureEnhanceAllClick(object obj)
	{
		if ((long)Globals.Instance.Player.Data.Money < (long)((ulong)this.mEnhanceTotalCost))
		{
			GameMessageBox.ShowMoneyLackMessageBox();
		}
		else
		{
			SocketDataEx curSelectSocketData = this.GetCurSelectSocketData();
			if (curSelectSocketData != null)
			{
				this.oldEquipEnhanceMasterLevel = curSelectSocketData.EquipMasterEnhanceLevel;
			}
			PetDataEx curSelectPetData = this.GetCurSelectPetData();
			curSelectPetData.GetAttribute(ref GameUIManager.mInstance.uiState.mOldHpNum, ref GameUIManager.mInstance.uiState.mOldAttackNum, ref GameUIManager.mInstance.uiState.mOldWufangNum, ref GameUIManager.mInstance.uiState.mOldFafangNum);
			int curSelectIndex = this.GetCurSelectIndex();
			MC2S_EquipEnhance mC2S_EquipEnhance = new MC2S_EquipEnhance();
			mC2S_EquipEnhance.Type = 0;
			mC2S_EquipEnhance.Value = (ulong)((long)curSelectIndex);
			Globals.Instance.CliSession.Send(520, mC2S_EquipEnhance);
		}
	}

	private int GetEquipSlotIndex(ItemDataEx idEx)
	{
		int result = -1;
		if (idEx != null)
		{
			if (idEx.Info.Type == 0)
			{
				switch (idEx.Info.SubType)
				{
				case 0:
					result = 0;
					break;
				case 1:
					result = 1;
					break;
				case 2:
					result = 2;
					break;
				case 3:
					result = 3;
					break;
				}
			}
			else if (idEx.Info.Type == 1)
			{
				int subType = idEx.Info.SubType;
				if (subType != 0)
				{
					if (subType == 1)
					{
						result = 5;
					}
				}
				else
				{
					result = 4;
				}
			}
		}
		return result;
	}

	private void OnHuanZhuangBtnClick(GameObject go)
	{
		if (this.IsLocalPlayer)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
			int curSelectIndex = this.GetCurSelectIndex();
			SocketDataEx curSelectSocketData = this.GetCurSelectSocketData();
			if (curSelectSocketData == null)
			{
				return;
			}
			ItemDataEx[] goodEquips = Tools.GetGoodEquips(curSelectIndex);
			MC2S_AutoEquipItem mC2S_AutoEquipItem = new MC2S_AutoEquipItem();
			mC2S_AutoEquipItem.SocketSlot = curSelectIndex;
			bool flag = false;
			for (int i = 0; i < 6; i++)
			{
				ItemDataEx equip = curSelectSocketData.GetEquip(i);
				ItemDataEx itemDataEx = goodEquips[i];
				if (!flag && itemDataEx != null && itemDataEx.Data.ID != 0uL && (equip == null || (equip != null && itemDataEx.Data.ID != equip.Data.ID)))
				{
					flag = true;
				}
				if (itemDataEx != null)
				{
					if (equip != null)
					{
						if (itemDataEx.Data.ID != equip.Data.ID)
						{
							mC2S_AutoEquipItem.ItemID.Add(itemDataEx.Data.ID);
						}
						else
						{
							mC2S_AutoEquipItem.ItemID.Add(0uL);
						}
					}
					else
					{
						mC2S_AutoEquipItem.ItemID.Add(itemDataEx.Data.ID);
					}
				}
				else
				{
					mC2S_AutoEquipItem.ItemID.Add(0uL);
				}
			}
			if (!flag)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("equipImprove72", 0f, 0f);
			}
			else
			{
				PetDataEx curSelectPetData = this.GetCurSelectPetData();
				curSelectPetData.GetAttribute(ref GameUIManager.mInstance.uiState.mOldHpNum, ref GameUIManager.mInstance.uiState.mOldAttackNum, ref GameUIManager.mInstance.uiState.mOldWufangNum, ref GameUIManager.mInstance.uiState.mOldFafangNum);
				Globals.Instance.CliSession.Send(199, mC2S_AutoEquipItem);
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator DoSelectCurItem(int index)
	{
        return null;
        //GUITeamManageSceneV2.<DoSelectCurItem>c__Iterator98 <DoSelectCurItem>c__Iterator = new GUITeamManageSceneV2.<DoSelectCurItem>c__Iterator98();
        //<DoSelectCurItem>c__Iterator.index = index;
        //<DoSelectCurItem>c__Iterator.<$>index = index;
        //<DoSelectCurItem>c__Iterator.<>f__this = this;
        //return <DoSelectCurItem>c__Iterator;
	}

	public void SetCurSelectItem(int index)
	{
		if (index < 4)
		{
			this.ShowAllModelItem();
		}
		base.StartCoroutine(this.DoSelectCurItem(index));
	}

	public int GetCurSelectIndex()
	{
		int result = 0;
		if (this.mGUISimpleSM != null)
		{
			string state = this.mGUISimpleSM.State;
			switch (state)
			{
			case "pet0":
				result = 1;
				break;
			case "pet1":
				result = 2;
				break;
			case "pet2":
				result = 3;
				break;
			case "lopet":
				result = 4;
				break;
			case "pet3":
				result = 5;
				break;
			}
		}
		return result;
	}

	private void OnModelCenterCB(GameObject go)
	{
		GUITeamManageModelItem component = go.GetComponent<GUITeamManageModelItem>();
		if (component != null)
		{
			GUITeamManageModelData modelData = component.GetModelData();
			if (modelData != null && modelData.mSocketSlotIndex != this.GetCurSelectIndex())
			{
				this.SetCurSelectItem(modelData.mSocketSlotIndex);
			}
		}
	}

	private void SetAvatar()
	{
		this.SetSelectItemsState(0);
		this.mCommonPetGo.SetActive(true);
		this.mLittlePet.SetActive(false);
		this.mTextureBgGo.SetActive(true);
		this.ShowShiZhuangBtn(true);
		this.RefreshPetInfos(false, -1, -1);
	}

	private void OnEnterAvatar()
	{
		this.mModelCenterChild.onCenter = null;
		bool activeInHierarchy = this.mLittlePet.activeInHierarchy;
		this.SetAvatar();
		this.CenterChild(0, activeInHierarchy);
		GUICenterModelItem expr_2C = this.mModelCenterChild;
		expr_2C.onCenter = (GUICenterModelItem.OnCenterCallback)Delegate.Combine(expr_2C.onCenter, new GUICenterModelItem.OnCenterCallback(this.OnModelCenterCB));
	}

	private void SetPet0()
	{
		this.SetSelectItemsState(1);
		this.mCommonPetGo.SetActive(true);
		this.mLittlePet.SetActive(false);
		this.mTextureBgGo.SetActive(true);
		this.ShowShiZhuangBtn(false);
		this.RefreshPetInfos(false, -1, -1);
	}

	private void OnEnterPet0()
	{
		this.mModelCenterChild.onCenter = null;
		bool activeInHierarchy = this.mLittlePet.activeInHierarchy;
		this.SetPet0();
		this.CenterChild(1, activeInHierarchy);
		GUICenterModelItem expr_2C = this.mModelCenterChild;
		expr_2C.onCenter = (GUICenterModelItem.OnCenterCallback)Delegate.Combine(expr_2C.onCenter, new GUICenterModelItem.OnCenterCallback(this.OnModelCenterCB));
	}

	private void SetPet1()
	{
		this.SetSelectItemsState(2);
		this.mCommonPetGo.SetActive(true);
		this.mLittlePet.SetActive(false);
		this.mTextureBgGo.SetActive(true);
		this.ShowShiZhuangBtn(false);
		this.RefreshPetInfos(false, -1, -1);
	}

	private void OnEnterPet1()
	{
		this.mModelCenterChild.onCenter = null;
		bool activeInHierarchy = this.mLittlePet.activeInHierarchy;
		this.SetPet1();
		this.CenterChild(2, activeInHierarchy);
		GUICenterModelItem expr_2C = this.mModelCenterChild;
		expr_2C.onCenter = (GUICenterModelItem.OnCenterCallback)Delegate.Combine(expr_2C.onCenter, new GUICenterModelItem.OnCenterCallback(this.OnModelCenterCB));
	}

	private void SetPet2()
	{
		this.SetSelectItemsState(3);
		this.mCommonPetGo.SetActive(true);
		this.mLittlePet.SetActive(false);
		this.mTextureBgGo.SetActive(true);
		this.ShowShiZhuangBtn(false);
		this.RefreshPetInfos(false, -1, -1);
	}

	private void OnEnterPet2()
	{
		this.mModelCenterChild.onCenter = null;
		bool activeInHierarchy = this.mLittlePet.activeInHierarchy;
		this.SetPet2();
		this.CenterChild(3, activeInHierarchy);
		GUICenterModelItem expr_2C = this.mModelCenterChild;
		expr_2C.onCenter = (GUICenterModelItem.OnCenterCallback)Delegate.Combine(expr_2C.onCenter, new GUICenterModelItem.OnCenterCallback(this.OnModelCenterCB));
	}

	private void SetPet3()
	{
		this.SetSelectItemsState(5);
		for (int i = 0; i < 6; i++)
		{
			this.mEquipItems[i].HideEffects();
		}
		this.mCommonPetGo.SetActive(false);
		this.mLittlePet.SetActive(true);
		this.mTextureBgGo.SetActive(false);
		this.ShowLopet(false);
		this.RefreshLittlePetInfos();
	}

	private void OnEnterPet3()
	{
		this.mModelCenterChild.onCenter = null;
		this.SetPet3();
		GUICenterModelItem expr_18 = this.mModelCenterChild;
		expr_18.onCenter = (GUICenterModelItem.OnCenterCallback)Delegate.Combine(expr_18.onCenter, new GUICenterModelItem.OnCenterCallback(this.OnModelCenterCB));
	}

	private void SetLopet()
	{
		this.SetSelectItemsState(4);
		this.mCommonPetGo.SetActive(false);
		this.mLittlePet.SetActive(false);
		this.mTextureBgGo.SetActive(false);
		this.ShowLopet(true);
		this.RefreshPetInfos(false, -1, -1);
	}

	private void OnEnterLopet()
	{
		this.mModelCenterChild.onCenter = null;
		bool activeInHierarchy = this.mLittlePet.activeInHierarchy;
		this.SetLopet();
		this.CenterChild(4, activeInHierarchy);
		GUICenterModelItem expr_2C = this.mModelCenterChild;
		expr_2C.onCenter = (GUICenterModelItem.OnCenterCallback)Delegate.Combine(expr_2C.onCenter, new GUICenterModelItem.OnCenterCallback(this.OnModelCenterCB));
	}

	private void SetSelectItemsState(int index)
	{
		if (0 <= index && index < 6)
		{
			for (int i = 0; i < 6; i++)
			{
				this.mSelectItems[i].IsSelected = (index == i);
			}
			if (this.mTeamModelItems[index] != null)
			{
				this.mTeamModelItems[index].PlayIdle();
			}
		}
	}

	private void ShowShiZhuangBtn(bool isShow)
	{
		this.ShowLopet(false);
		this.mShiZhuangBtn.SetActive(isShow && this.IsLocalPlayer);
		this.mChangePetBtn.SetActive(!isShow && this.IsLocalPlayer);
		if (this.IsLocalPlayer)
		{
			if (!isShow)
			{
				int curSelectIndex = this.GetCurSelectIndex();
				this.mChangePetBtnEffect.SetActive(Tools.CanBattlePetHasBetterPet2(curSelectIndex) && GameCache.Data.HasShowChangePetMark);
			}
			else
			{
				this.mShiZhuangBtnEffect.SetActive(GameCache.Data.HasNewFashion);
			}
		}
	}

	private void ShowLopet(bool show)
	{
		this.mLopetLayer.Init(this);
		this.mLopetLayer.gameObject.SetActive(show);
		this.mLopetBG.SetActive(show);
		this.mTextureBg.gameObject.SetActive(!show);
		this.mShiZhuangBtn.SetActive(!show && this.IsLocalPlayer);
		this.mEnhanceAllBtn.SetActive(!show && this.IsLocalPlayer);
		this.mYangChengBtn.SetActive(!show && this.IsLocalPlayer);
		this.mEnhanceBtn.SetActive(!show && this.IsLocalPlayer);
		this.mHuanZhuangBtn.SetActive(!show && this.IsLocalPlayer);
		this.mChangePetBtn.SetActive(!show && this.IsLocalPlayer);
		GUITeamManageEquipItem[] array = this.mEquipItems;
		for (int i = 0; i < array.Length; i++)
		{
			GUITeamManageEquipItem gUITeamManageEquipItem = array[i];
			gUITeamManageEquipItem.gameObject.SetActive(!show);
		}
	}

	private void OnYuanFenClick(GameObject go)
	{
		string name = go.name;
		switch (name)
		{
		}
	}

	private void RefreshPetInfos(bool isEnhanceEvent = false, int petSlot = -1, int equipSlot = -1)
	{
		int curSelectIndex = this.GetCurSelectIndex();
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem != null)
		{
			PetDataEx curSelectPetData = this.GetCurSelectPetData();
			this.mYangChengBtn.SetActive(this.IsLocalPlayer && curSelectPetData != null);
			this.mEnhanceBtn.SetActive(this.IsLocalPlayer && curSelectPetData != null);
			SocketDataEx socket = teamSystem.GetSocket(curSelectIndex, this.IsLocalPlayer);
			if (socket != null)
			{
				PetDataEx pet = socket.GetPet();
				if (pet != null)
				{
					this.mGUIPetTitleInfo.gameObject.SetActive(true);
					this.mGUIPetTitleInfo.Refresh(pet, this.IsLocalPlayer, curSelectIndex == 0);
					this.mHpNum.text = socket.MaxHP.ToString();
					this.mAttackNum.text = socket.Attack.ToString();
					this.mWuFangNum.text = socket.PhysicDefense.ToString();
					this.mFaFangNum.text = socket.MagicDefense.ToString();
					if (0 < curSelectIndex && curSelectIndex < 4)
					{
						this.mSkillGo.SetActive(true);
						this.mSkillLayer.ShowSummonSkills(pet, null);
					}
					else
					{
						this.mSkillGo.SetActive(false);
					}
					this.mSb.Remove(0, this.mSb.Length);
					if (socket.IsPlayer())
					{
						for (int i = 0; i < 6; i++)
						{
							if (i + 12 < pet.Info.RelationID.Count)
							{
								RelationInfo info = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[i + 12]);
								if (info != null && socket.IsRelationActive(info))
								{
									this.mSb.Append("[00ff00]").Append(info.Name).Append("[-]\n");
								}
								else
								{
									RelationInfo info2 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[i + 6]);
									if (info2 != null && socket.IsRelationActive(info2))
									{
										this.mSb.Append("[00ff00]").Append(info2.Name).Append("[-]\n");
									}
									else
									{
										RelationInfo info3 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[i]);
										if (info3 != null)
										{
											if (socket.IsRelationActive(info3))
											{
												this.mSb.Append("[00ff00]").Append(info3.Name).Append("[-]\n");
											}
											else
											{
												this.mSb.Append("[b2b2b2]").Append(info3.Name).Append("[-]\n");
											}
										}
									}
								}
							}
							else if (i + 6 < pet.Info.RelationID.Count)
							{
								RelationInfo info4 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[i + 6]);
								if (info4 != null && socket.IsRelationActive(info4))
								{
									this.mSb.Append("[00ff00]").Append(info4.Name).Append("[-]\n");
								}
								else
								{
									RelationInfo info5 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[i]);
									if (info5 != null)
									{
										if (socket.IsRelationActive(info5))
										{
											this.mSb.Append("[00ff00]").Append(info5.Name).Append("[-]\n");
										}
										else
										{
											this.mSb.Append("[b2b2b2]").Append(info5.Name).Append("[-]\n");
										}
									}
								}
							}
							else if (i < pet.Info.RelationID.Count)
							{
								RelationInfo info6 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[i]);
								if (info6 != null)
								{
									if (socket.IsRelationActive(info6))
									{
										this.mSb.Append("[00ff00]").Append(info6.Name).Append("[-]\n");
									}
									else
									{
										this.mSb.Append("[b2b2b2]").Append(info6.Name).Append("[-]\n");
									}
								}
							}
						}
					}
					else
					{
						for (int j = 0; j < 3; j++)
						{
							RelationInfo info7 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[j]);
							if (info7 != null)
							{
								if (socket.IsRelationActive(info7))
								{
									this.mSb.Append("[00ff00]").Append(info7.Name).Append("[-]\n");
								}
								else
								{
									this.mSb.Append("[b2b2b2]").Append(info7.Name).Append("[-]\n");
								}
							}
						}
						if (6 < pet.Info.RelationID.Count)
						{
							RelationInfo info8 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[6]);
							if (info8 != null)
							{
								if (socket.IsRelationActive(info8))
								{
									this.mSb.Append("[00ff00]").Append(info8.Name).Append("[-]\n");
								}
								else if (3 < pet.Info.RelationID.Count)
								{
									RelationInfo info9 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[3]);
									if (info9 != null)
									{
										if (socket.IsRelationActive(info9))
										{
											this.mSb.Append("[00ff00]").Append(info9.Name).Append("[-]\n");
										}
										else
										{
											this.mSb.Append("[b2b2b2]").Append(info9.Name).Append("[-]\n");
										}
									}
								}
							}
							else if (3 < pet.Info.RelationID.Count)
							{
								RelationInfo info10 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[3]);
								if (info10 != null)
								{
									if (socket.IsRelationActive(info10))
									{
										this.mSb.Append("[00ff00]").Append(info10.Name).Append("[-]\n");
									}
									else
									{
										this.mSb.Append("[b2b2b2]").Append(info10.Name).Append("[-]\n");
									}
								}
							}
						}
						else if (3 < pet.Info.RelationID.Count)
						{
							RelationInfo info11 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[3]);
							if (info11 != null)
							{
								if (socket.IsRelationActive(info11))
								{
									this.mSb.Append("[00ff00]").Append(info11.Name).Append("[-]\n");
								}
								else
								{
									this.mSb.Append("[b2b2b2]").Append(info11.Name).Append("[-]\n");
								}
							}
						}
						if (7 < pet.Info.RelationID.Count)
						{
							RelationInfo info12 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[7]);
							if (info12 != null)
							{
								if (socket.IsRelationActive(info12))
								{
									this.mSb.Append("[00ff00]").Append(info12.Name).Append("[-]\n");
								}
								else if (4 < pet.Info.RelationID.Count)
								{
									RelationInfo info13 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[4]);
									if (info13 != null)
									{
										if (socket.IsRelationActive(info13))
										{
											this.mSb.Append("[00ff00]").Append(info13.Name).Append("[-]\n");
										}
										else
										{
											this.mSb.Append("[b2b2b2]").Append(info13.Name).Append("[-]\n");
										}
									}
								}
							}
							else if (4 < pet.Info.RelationID.Count)
							{
								RelationInfo info14 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[4]);
								if (info14 != null)
								{
									if (socket.IsRelationActive(info14))
									{
										this.mSb.Append("[00ff00]").Append(info14.Name).Append("[-]\n");
									}
									else
									{
										this.mSb.Append("[b2b2b2]").Append(info14.Name).Append("[-]\n");
									}
								}
							}
						}
						else if (4 < pet.Info.RelationID.Count)
						{
							RelationInfo info15 = Globals.Instance.AttDB.RelationDict.GetInfo(pet.Info.RelationID[4]);
							if (info15 != null)
							{
								if (socket.IsRelationActive(info15))
								{
									this.mSb.Append("[00ff00]").Append(info15.Name).Append("[-]\n");
								}
								else
								{
									this.mSb.Append("[b2b2b2]").Append(info15.Name).Append("[-]\n");
								}
							}
						}
					}
					this.mYuanFenDesc.text = this.mSb.ToString().TrimEnd(new char[]
					{
						'\n'
					});
					this.mRightInfoTable.repositionNow = true;
				}
				else
				{
					this.mGUIPetTitleInfo.gameObject.SetActive(false);
					this.mHpNum.text = string.Empty;
					this.mAttackNum.text = string.Empty;
					this.mWuFangNum.text = string.Empty;
					this.mFaFangNum.text = string.Empty;
					this.mSkillGo.SetActive(false);
					this.mYuanFenDesc.text = string.Empty;
				}
			}
			else
			{
				this.mGUIPetTitleInfo.gameObject.SetActive(false);
				this.mHpNum.text = string.Empty;
				this.mAttackNum.text = string.Empty;
				this.mWuFangNum.text = string.Empty;
				this.mFaFangNum.text = string.Empty;
				this.mSkillGo.SetActive(false);
				this.mYuanFenDesc.text = string.Empty;
			}
			for (int k = 0; k < 6; k++)
			{
				this.mEquipItems[k].Refresh(isEnhanceEvent, curSelectIndex == petSlot && k == equipSlot);
			}
			for (int l = 0; l < 6; l++)
			{
				this.mEquipItems[l].RefreshRedTag();
			}
			this.mRightInfoTable.repositionNow = true;
			this.RefreshYangChengMark();
		}
	}

	private bool HasYuanFenItems(PetDataEx pdEx)
	{
		bool result = false;
		if (pdEx != null)
		{
			for (int i = 0; i < 3; i++)
			{
				RelationInfo info = Globals.Instance.AttDB.RelationDict.GetInfo(pdEx.Info.RelationID[i]);
				if (info != null)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	[DebuggerHidden]
	private IEnumerator UpdateYuanFenItemSB()
	{
        return null;
        //GUITeamManageSceneV2.<UpdateYuanFenItemSB>c__Iterator99 <UpdateYuanFenItemSB>c__Iterator = new GUITeamManageSceneV2.<UpdateYuanFenItemSB>c__Iterator99();
        //<UpdateYuanFenItemSB>c__Iterator.<>f__this = this;
        //return <UpdateYuanFenItemSB>c__Iterator;
	}

	private void RefreshLittlePetInfos()
	{
		this.RefreshAssistPets();
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem != null)
		{
			for (int i = this.mGUIPetYuanFenItemTable.transform.childCount; i > 0; i--)
			{
				Transform child = this.mGUIPetYuanFenItemTable.transform.GetChild(i - 1);
				UnityEngine.Object.Destroy(child.gameObject);
			}
			for (int j = 1; j <= 3; j++)
			{
				SocketDataEx socket = teamSystem.GetSocket(j, this.IsLocalPlayer);
				if (socket != null)
				{
					PetDataEx pet = socket.GetPet();
					if (pet != null && this.HasYuanFenItems(pet))
					{
						this.AddPetYuanFenItem(socket);
					}
				}
			}
			this.mGUIPetYuanFenItemTable.repositionNow = true;
			this.mRecordItemOriginal = null;
			base.StartCoroutine(this.UpdateYuanFenItemSB());
		}
		this.mPetZhuWeiMark.SetActive(RedFlagTools.CanShowZhuWeiMark());
	}

	private void RefreshAssistPets()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem != null)
		{
			for (int i = 0; i < 6; i++)
			{
				PetDataEx assist = teamSystem.GetAssist(i, this.IsLocalPlayer);
				this.mAssistPets[i].Refresh(assist);
			}
		}
	}

	private void AddPetYuanFenItem(SocketDataEx petData)
	{
		if (petData != null)
		{
			if (this.mRecordItemOriginal == null)
			{
				this.mRecordItemOriginal = Res.LoadGUI("GUI/petYuanFenItem");
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.mRecordItemOriginal);
			gameObject.name = this.mRecordItemOriginal.name;
			gameObject.transform.parent = this.mGUIPetYuanFenItemTable.gameObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			GUIPetYuanFenItem gUIPetYuanFenItem = gameObject.AddComponent<GUIPetYuanFenItem>();
			gUIPetYuanFenItem.InitWithBaseScene(this, petData);
		}
	}

	public PetDataEx GetCurSelectPetData()
	{
		int curSelectIndex = this.GetCurSelectIndex();
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem != null)
		{
			SocketDataEx socket = teamSystem.GetSocket(curSelectIndex, this.IsLocalPlayer);
			if (socket != null)
			{
				return socket.GetPet();
			}
		}
		return null;
	}

	private SocketDataEx GetCurSelectSocketData()
	{
		int curSelectIndex = this.GetCurSelectIndex();
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem != null)
		{
			return teamSystem.GetSocket(curSelectIndex, this.IsLocalPlayer);
		}
		return null;
	}

	private void DoChangeToPetInfoScene(int whichPart)
	{
		PetDataEx curSelectPetData = this.GetCurSelectPetData();
		if (curSelectPetData != null)
		{
			if (this.IsLocalPlayer)
			{
				GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = curSelectPetData;
				GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 0;
				GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
				GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
			}
			else
			{
				GameUIManager.mInstance.ShowPetInfoSceneV2(curSelectPetData, 0, null, 3);
			}
		}
	}

	private void OnPetInfoCPartClick(GameObject go)
	{
		this.DoChangeToPetInfoScene(2);
	}

	private void OnEquipItemEvent(int slot, int equipSlot)
	{
		this.RefreshPetInfos(false, slot, equipSlot);
		this.RefreshSelectItems();
		base.StartCoroutine(this.ShowAttributeChange());
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_GoldEquipSet, 0f);
	}

	private int[] GetCurRelationFlag()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		int[] array = new int[4];
		for (int i = 0; i < 4; i++)
		{
			SocketDataEx socket = teamSystem.GetSocket(i);
			if (socket != null)
			{
				array[i] = socket.RelationFlag;
			}
			else
			{
				array[i] = 0;
			}
		}
		return array;
	}

	private int[] GetCurRelationPetId()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		int[] array = new int[4];
		for (int i = 0; i < 4; i++)
		{
			SocketDataEx socket = teamSystem.GetSocket(i);
			if (socket != null)
			{
				PetDataEx pet = socket.GetPet();
				if (pet != null)
				{
					array[i] = pet.Info.ID;
				}
			}
			else
			{
				array[i] = 0;
			}
		}
		return array;
	}

	private void DoAttributeChange()
	{
		PetDataEx curSelectPetData = this.GetCurSelectPetData();
		if (curSelectPetData != null)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			curSelectPetData.GetAttribute(ref num, ref num2, ref num3, ref num4);
			if (GameUIManager.mInstance.uiState.mOldHpNum != 0 && num != GameUIManager.mInstance.uiState.mOldHpNum)
			{
				if (num > GameUIManager.mInstance.uiState.mOldHpNum)
				{
					this.mSb.Remove(0, this.mSb.Length).Append("[00ff00]").Append(Singleton<StringManager>.Instance.GetString("EAID_1")).Append("+").Append(num - GameUIManager.mInstance.uiState.mOldHpNum);
				}
				else
				{
					this.mSb.Remove(0, this.mSb.Length).Append("[ff0000]").Append(Singleton<StringManager>.Instance.GetString("EAID_1")).Append("-").Append(GameUIManager.mInstance.uiState.mOldHpNum - num);
				}
				this.mSb.Append("[-]");
				this.mTipContents.Add(this.mSb.ToString());
			}
			if (GameUIManager.mInstance.uiState.mOldAttackNum != 0 && num2 != GameUIManager.mInstance.uiState.mOldAttackNum)
			{
				if (num2 > GameUIManager.mInstance.uiState.mOldAttackNum)
				{
					this.mSb.Remove(0, this.mSb.Length).Append("[00ff00]").Append(Singleton<StringManager>.Instance.GetString("EAID_2")).Append("+").Append(num2 - GameUIManager.mInstance.uiState.mOldAttackNum);
				}
				else
				{
					this.mSb.Remove(0, this.mSb.Length).Append("[ff0000]").Append(Singleton<StringManager>.Instance.GetString("EAID_2")).Append("-").Append(GameUIManager.mInstance.uiState.mOldAttackNum - num2);
				}
				this.mSb.Append("[-]");
				this.mTipContents.Add(this.mSb.ToString());
			}
			if (GameUIManager.mInstance.uiState.mOldWufangNum != 0 && num3 != GameUIManager.mInstance.uiState.mOldWufangNum)
			{
				if (num3 > GameUIManager.mInstance.uiState.mOldWufangNum)
				{
					this.mSb.Remove(0, this.mSb.Length).Append("[00ff00]").Append(Singleton<StringManager>.Instance.GetString("EAID_3")).Append("+").Append(num3 - GameUIManager.mInstance.uiState.mOldWufangNum);
				}
				else
				{
					this.mSb.Remove(0, this.mSb.Length).Append("[ff0000]").Append(Singleton<StringManager>.Instance.GetString("EAID_3")).Append("-").Append(GameUIManager.mInstance.uiState.mOldWufangNum - num3);
				}
				this.mSb.Append("[-]");
				this.mTipContents.Add(this.mSb.ToString());
			}
			if (GameUIManager.mInstance.uiState.mOldFafangNum != 0 && num4 != GameUIManager.mInstance.uiState.mOldFafangNum)
			{
				if (num4 > GameUIManager.mInstance.uiState.mOldFafangNum)
				{
					this.mSb.Remove(0, this.mSb.Length).Append("[00ff00]").Append(Singleton<StringManager>.Instance.GetString("EAID_4")).Append("+").Append(num4 - GameUIManager.mInstance.uiState.mOldFafangNum);
				}
				else
				{
					this.mSb.Remove(0, this.mSb.Length).Append("[ff0000]").Append(Singleton<StringManager>.Instance.GetString("EAID_4")).Append("-").Append(GameUIManager.mInstance.uiState.mOldFafangNum - num4);
				}
				this.mSb.Append("[-]");
				this.mTipContents.Add(this.mSb.ToString());
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator ShowActiveRelationDesc()
	{
        return null;
        //GUITeamManageSceneV2.<ShowActiveRelationDesc>c__Iterator9A <ShowActiveRelationDesc>c__Iterator9A = new GUITeamManageSceneV2.<ShowActiveRelationDesc>c__Iterator9A();
        //<ShowActiveRelationDesc>c__Iterator9A.<>f__this = this;
        //return <ShowActiveRelationDesc>c__Iterator9A;
	}

	[DebuggerHidden]
	private IEnumerator ShowAttributeChange()
	{
        return null;
        //GUITeamManageSceneV2.<ShowAttributeChange>c__Iterator9B <ShowAttributeChange>c__Iterator9B = new GUITeamManageSceneV2.<ShowAttributeChange>c__Iterator9B();
        //<ShowAttributeChange>c__Iterator9B.<>f__this = this;
        //return <ShowAttributeChange>c__Iterator9B;
	}

	private void OnMsgEquipEnhance(MemoryStream stream)
	{
		MS2C_EquipEnhance mS2C_EquipEnhance = Serializer.NonGeneric.Deserialize(typeof(MS2C_EquipEnhance), stream) as MS2C_EquipEnhance;
		if (mS2C_EquipEnhance.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_EquipEnhance.Result);
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_018");
		this.RefreshPetInfos(true, -1, -1);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		this.mSb.Remove(0, this.mSb.Length);
		PetDataEx curSelectPetData = this.GetCurSelectPetData();
		if (curSelectPetData != null)
		{
			this.mSb.Append("[c]");
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			curSelectPetData.GetAttribute(ref num, ref num2, ref num3, ref num4);
			if (GameUIManager.mInstance.uiState.mOldHpNum != 0 && num != GameUIManager.mInstance.uiState.mOldHpNum && num > GameUIManager.mInstance.uiState.mOldHpNum)
			{
				flag = true;
				this.mSb.Append("[c8a05a]").Append(Singleton<StringManager>.Instance.GetString("EAID_1")).Append("[00ff00]").Append(" +").Append(num - GameUIManager.mInstance.uiState.mOldHpNum).Append("[-]");
			}
			if (GameUIManager.mInstance.uiState.mOldAttackNum != 0 && num2 != GameUIManager.mInstance.uiState.mOldAttackNum && num2 > GameUIManager.mInstance.uiState.mOldAttackNum)
			{
				flag2 = true;
				if (flag)
				{
					this.mSb.Append(",");
				}
				else
				{
					this.mSb.Append("[c8a05a]");
				}
				this.mSb.Append(Singleton<StringManager>.Instance.GetString("EAID_2")).Append("[00ff00]").Append(" +").Append(num2 - GameUIManager.mInstance.uiState.mOldAttackNum).Append("[-][-]");
			}
			if (GameUIManager.mInstance.uiState.mOldWufangNum != 0 && num3 != GameUIManager.mInstance.uiState.mOldWufangNum && num3 > GameUIManager.mInstance.uiState.mOldWufangNum)
			{
				flag3 = true;
				if (flag || flag2)
				{
					this.mSb.AppendLine();
				}
				this.mSb.Append("[c8a05a]").Append(Singleton<StringManager>.Instance.GetString("EAID_3")).Append("[00ff00]").Append(" +").Append(num3 - GameUIManager.mInstance.uiState.mOldWufangNum).Append("[-]");
			}
			if (GameUIManager.mInstance.uiState.mOldFafangNum != 0 && num4 != GameUIManager.mInstance.uiState.mOldFafangNum && num4 > GameUIManager.mInstance.uiState.mOldFafangNum)
			{
				if (flag3)
				{
					this.mSb.Append(",");
				}
				else
				{
					this.mSb.Append("[c8a05a]");
				}
				this.mSb.Append(Singleton<StringManager>.Instance.GetString("EAID_4")).Append("[00ff00]").Append(" +").Append(num4 - GameUIManager.mInstance.uiState.mOldFafangNum).Append("[-][-]");
			}
			if (mS2C_EquipEnhance.TotalCrit > 0)
			{
				if (this.mSb.Length != 0)
				{
					this.mSb.AppendLine();
				}
				this.mSb.Append("[c8a05a]").Append(Singleton<StringManager>.Instance.GetString("teamManage3", new object[]
				{
					string.Format("[00ff00]{0}[-]", mS2C_EquipEnhance.TotalCrit)
				}));
			}
			if (mS2C_EquipEnhance.TotalMoney > 0)
			{
				if (this.mSb.Length != 0)
				{
					this.mSb.AppendLine();
				}
				this.mSb.Append("[c8a05a]").Append(Singleton<StringManager>.Instance.GetString("teamManage4", new object[]
				{
					string.Format("[00ff00]{0}[-]", mS2C_EquipEnhance.TotalMoney)
				}));
			}
			this.mSb.Append("[/c]");
		}
		SocketDataEx curSelectSocketData = this.GetCurSelectSocketData();
		if (curSelectSocketData != null && curSelectSocketData.EquipMasterEnhanceLevel > this.oldEquipEnhanceMasterLevel)
		{
			GUIUpgradeTipPopUp.ShowThis(Singleton<StringManager>.Instance.GetString("equipImprove78"), this.mSb.ToString(), Singleton<StringManager>.Instance.GetString("equipImprove55", new object[]
			{
				curSelectSocketData.EquipMasterEnhanceLevel
			}), Master.GetMasterDiffValueStr(this.oldEquipEnhanceMasterLevel, curSelectSocketData.EquipMasterEnhanceLevel, Master.EMT.EMT_EquipEnhance), 5f, 2f);
		}
		else
		{
			GUIUpgradeTipPopUp.ShowThis(Singleton<StringManager>.Instance.GetString("equipImprove78"), this.mSb.ToString(), string.Empty, string.Empty, 5f, 2f);
		}
	}

	private void RefreshSelectItems()
	{
		for (int i = 0; i < 6; i++)
		{
			this.mSelectItems[i].Refresh();
		}
	}

	private void OnEquipPetEvent(int slotIndex)
	{
		this.RefreshSelectItems();
		this.RefreshLittlePetInfos();
	}

	private bool IsUnlocked(int index)
	{
		return (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)(GameConst.GetInt32(197) + index * 5));
	}

	public bool HasUnBattlePet()
	{
		bool result = false;
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (!current.IsPlayerBattling() && !current.IsPetBattling() && !current.IsPetAssisting())
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public bool HasAssitPetFull()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		for (int i = 0; i < 6; i++)
		{
			if (this.IsUnlocked(i) && teamSystem.GetAssist(i) == null)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsCanAssitPets()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		bool result = false;
		bool flag = this.HasUnBattlePet();
		if (flag)
		{
			for (int i = 0; i < 6; i++)
			{
				if (this.IsUnlocked(i) && teamSystem.GetAssist(i) == null)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	private void RefreshYangChengMark()
	{
		PetDataEx curSelectPetData = this.GetCurSelectPetData();
		if (curSelectPetData != null)
		{
			this.mYangChengMark.SetActive(Tools.CanPetLvlUp(curSelectPetData) || Tools.CanPetSkillLvlUp(curSelectPetData) || Tools.CanPetPeiYang(curSelectPetData) || Tools.CanShowJueXingMark(curSelectPetData) || Tools.CanShowJiJieMark(curSelectPetData));
		}
		else
		{
			this.mYangChengMark.SetActive(false);
		}
	}

	private void OnPetZhuWeiBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIPetZhuWeiPopUp.ShowMe();
	}

	private void Update()
	{
		this.mUpdateTimer += Time.unscaledDeltaTime;
		if (!this.mIsDragging && this.mUpdateTimer >= 1f)
		{
			this.mUpdateTimer = 0f;
			int curSelectIndex = this.GetCurSelectIndex();
			if (curSelectIndex < 4)
			{
				this.ShowCurModelItem(curSelectIndex);
			}
		}
	}
}
