using Att;
using Proto;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GUIPetInfoSceneV2 : MonoBehaviour
{
	public UIAtlasDynamic mPetsIconAtlas;

	public UIAtlasDynamic mItemsIconAtlas;

	private GUIPetTitleInfo mGUIPetTitleInfo;

	private GUIPetInfoSceneRightInfo mGUIPetInfoSceneRightInfo;

	private GameObject mCardModel;

	private GameObject mModelTmp;

	private UIActorController mUIActorController;

	private GameObject mChangePetBtn;

	private GameObject mChangePetBtnMark;

	private GameObject mUnBattleBtn;

	private GameObject mLevelUpBtn;

	private GameObject mSkillBtn;

	private GameObject mFurtherBtn;

	private UISlider mSuiPianJindu;

	private UILabel mSuiPianNumTxt;

	private GameObject mState0Go;

	private GameObject mState1Go;

	private StringBuilder mSb = new StringBuilder(42);

	private GUISimpleSM<string, string> mGUISimpleSM;

	private int mUIState;

	private ItemInfo mItemInfo;

	private PetDataEx mCurPetDataEx;

	private int mRightInfoWhichPart;

	private GameObject mLeftBtn;

	private GameObject mRightBtn;

	private int mCurrentPetDataIndex;

	private List<PetDataEx> mPetDatas = new List<PetDataEx>();

	private ResourceEntity asyncEntiry;

	public int CurUIState
	{
		get
		{
			return this.mUIState;
		}
	}

	public ItemInfo MItemInfo
	{
		get
		{
			return this.mItemInfo;
		}
		set
		{
			this.mItemInfo = value;
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
		}
	}

	public int RightInfoWhichPart
	{
		get
		{
			return this.mRightInfoWhichPart;
		}
		set
		{
			this.mRightInfoWhichPart = value;
			this.mGUIPetInfoSceneRightInfo.SetSelectedPart(this.mRightInfoWhichPart);
		}
	}

	public int WhichPart
	{
		get;
		set;
	}

	public void Show(PetDataEx pdEx, int whichPart, ItemInfo idEx, int uiState)
	{
		this.CurPetDataEx = pdEx;
		this.WhichPart = whichPart;
		this.mUIState = uiState;
		if (uiState == 1 || uiState == 4)
		{
			if (idEx != null)
			{
				this.mItemInfo = idEx;
				this.SetCurState(this.mUIState);
			}
		}
		else
		{
			this.mItemInfo = null;
			this.SetCurState(this.mUIState);
		}
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle");
		this.mCardModel = transform.Find("modelPos").gameObject;
		this.mGUIPetTitleInfo = transform.Find("topInfoPanel/topInfo").gameObject.AddComponent<GUIPetTitleInfo>();
		this.mGUIPetTitleInfo.InitWithBaseScene();
		this.mGUIPetInfoSceneRightInfo = transform.Find("rightInfo").gameObject.AddComponent<GUIPetInfoSceneRightInfo>();
		this.mGUIPetInfoSceneRightInfo.InitWithBaseScene(this);
		this.mState0Go = transform.Find("state0").gameObject;
		Transform transform2 = this.mState0Go.transform;
		this.mChangePetBtn = transform2.Find("changePetBtn").gameObject;
		UIEventListener expr_B7 = UIEventListener.Get(this.mChangePetBtn);
		expr_B7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B7.onClick, new UIEventListener.VoidDelegate(this.OnChangePetBtnClick));
		this.mChangePetBtnMark = this.mChangePetBtn.transform.Find("mark").gameObject;
		this.mChangePetBtnMark.SetActive(false);
		this.mUnBattleBtn = transform2.Find("unBattleBtn").gameObject;
		UIEventListener expr_125 = UIEventListener.Get(this.mUnBattleBtn);
		expr_125.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_125.onClick, new UIEventListener.VoidDelegate(this.OnUnBattleBtnClick));
		GameObject gameObject = transform2.Find("jueXingBtn").gameObject;
		UIEventListener expr_15D = UIEventListener.Get(gameObject);
		expr_15D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_15D.onClick, new UIEventListener.VoidDelegate(this.OnJueXingBtnClick));
		gameObject.SetActive(false);
		this.mSkillBtn = transform2.Find("skillBtn").gameObject;
		UIEventListener expr_1A6 = UIEventListener.Get(this.mSkillBtn);
		expr_1A6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1A6.onClick, new UIEventListener.VoidDelegate(this.OnSkillBtnClick));
		this.mFurtherBtn = transform2.Find("furtherBtn").gameObject;
		UIEventListener expr_1E8 = UIEventListener.Get(this.mFurtherBtn);
		expr_1E8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1E8.onClick, new UIEventListener.VoidDelegate(this.OnFurtherBtnClick));
		this.mLevelUpBtn = transform2.Find("lvlUpBtn").gameObject;
		UIEventListener expr_22A = UIEventListener.Get(this.mLevelUpBtn);
		expr_22A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_22A.onClick, new UIEventListener.VoidDelegate(this.OnLvlUpBtnClick));
		this.mLeftBtn = transform2.Find("leftBtn").gameObject;
		UIEventListener expr_26C = UIEventListener.Get(this.mLeftBtn);
		expr_26C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_26C.onClick, new UIEventListener.VoidDelegate(this.OnLeftBtnClick));
		this.mRightBtn = transform2.Find("rightBtn").gameObject;
		UIEventListener expr_2AE = UIEventListener.Get(this.mRightBtn);
		expr_2AE.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2AE.onClick, new UIEventListener.VoidDelegate(this.OnRightBtnClick));
		this.mState1Go = transform.Find("state1").gameObject;
		Transform transform3 = this.mState1Go.transform;
		this.mSuiPianJindu = transform3.Find("expBar").GetComponent<UISlider>();
		this.mSuiPianNumTxt = this.mSuiPianJindu.transform.Find("num").GetComponent<UILabel>();
		GameObject gameObject2 = transform3.Find("littlePlus").gameObject;
		UIEventListener expr_340 = UIEventListener.Get(gameObject2);
		expr_340.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_340.onClick, new UIEventListener.VoidDelegate(this.OnLittlePlusBtnclick));
		gameObject2.SetActive(false);
		GameObject gameObject3 = transform.Find("closeBtn").gameObject;
		UIEventListener expr_382 = UIEventListener.Get(gameObject3);
		expr_382.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_382.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mGUISimpleSM = new GUISimpleSM<string, string>("init");
		this.mGUISimpleSM.Configure("init").Permit("onState0", "state0").Permit("onState1", "state1").Permit("onState2", "state2").Permit("onState3", "state3").Permit("onState4", "state4");
		this.mGUISimpleSM.Configure("state0").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterState0();
		});
		this.mGUISimpleSM.Configure("state1").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterState1();
		});
		this.mGUISimpleSM.Configure("state2").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterState2();
		});
		this.mGUISimpleSM.Configure("state3").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterState3();
		});
		this.mGUISimpleSM.Configure("state4").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterState4();
		});
	}

	private void Awake()
	{
		this.CreateObjects();
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		TeamSubSystem expr_24 = Globals.Instance.Player.TeamSystem;
		expr_24.EquipPetEvent = (TeamSubSystem.PetUpdateCallback)Delegate.Combine(expr_24.EquipPetEvent, new TeamSubSystem.PetUpdateCallback(this.OnEquipPetEvent));
	}

	private void SetCurState(int index)
	{
		switch (index)
		{
		case 1:
			this.mGUISimpleSM.Fire("onState1");
			break;
		case 2:
			this.mGUISimpleSM.Fire("onState2");
			break;
		case 3:
			this.mGUISimpleSM.Fire("onState3");
			break;
		case 4:
			this.mGUISimpleSM.Fire("onState4");
			break;
		default:
			this.mGUISimpleSM.Fire("onState0");
			break;
		}
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		this.ClearModel();
		TeamSubSystem expr_26 = Globals.Instance.Player.TeamSystem;
		expr_26.EquipPetEvent = (TeamSubSystem.PetUpdateCallback)Delegate.Remove(expr_26.EquipPetEvent, new TeamSubSystem.PetUpdateCallback(this.OnEquipPetEvent));
	}

	private void RefreshState0()
	{
		if (this.mCurPetDataEx.IsPetBattling() || this.mCurPetDataEx.IsPetAssisting())
		{
			this.mChangePetBtn.SetActive(true);
			if (this.mCurPetDataEx.IsPetAssisting())
			{
				int assistSlot = Globals.Instance.Player.TeamSystem.GetAssistSlot(this.mCurPetDataEx.Data.ID);
				if (Tools.CanAssistPetShowMark(assistSlot, this.mCurPetDataEx))
				{
					this.mChangePetBtnMark.SetActive(true);
				}
				else
				{
					this.mChangePetBtnMark.SetActive(false);
				}
			}
			else
			{
				this.mChangePetBtnMark.SetActive(false);
			}
		}
		else
		{
			this.mChangePetBtn.SetActive(false);
		}
		this.mUnBattleBtn.SetActive(this.mCurPetDataEx.IsPetAssisting());
		this.mSkillBtn.SetActive(this.mCurPetDataEx.GetSocketSlot() != 0);
		this.mLevelUpBtn.SetActive(this.mCurPetDataEx.GetSocketSlot() != 0);
		this.mLeftBtn.gameObject.SetActive(this.mPetDatas.Count > 1 && !this.mCurPetDataEx.IsPetAssisting());
		this.mRightBtn.gameObject.SetActive(this.mPetDatas.Count > 1 && !this.mCurPetDataEx.IsPetAssisting());
		this.Refresh();
	}

	private void OnEnterState0()
	{
		this.mState0Go.SetActive(true);
		this.mState1Go.SetActive(false);
		this.mGUIPetInfoSceneRightInfo.SetCurState(2);
		this.InitPetDatas();
		this.SetCurPetIndex();
		this.RefreshState0();
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private void OnEnterState2()
	{
		this.mState0Go.SetActive(false);
		this.mState1Go.SetActive(false);
		this.mGUIPetInfoSceneRightInfo.SetCurState(2);
		this.Refresh();
	}

	private void OnEnterState3()
	{
		this.mState0Go.SetActive(false);
		this.mState1Go.SetActive(false);
		this.mGUIPetInfoSceneRightInfo.SetCurState(2);
		this.Refresh();
	}

	private void OnEnterState1()
	{
		this.mState0Go.SetActive(false);
		this.mState1Go.SetActive(true);
		if (this.mItemInfo != null)
		{
			int itemCount = Globals.Instance.Player.ItemSystem.GetItemCount(this.mItemInfo.ID);
			this.mSuiPianNumTxt.text = this.mSb.Remove(0, this.mSb.Length).Append(itemCount).Append("/").Append(this.mItemInfo.Value1).ToString();
			this.mSuiPianJindu.value = ((this.mItemInfo.Value1 == 0) ? 0f : Mathf.Clamp01((float)itemCount / (float)this.mItemInfo.Value1));
		}
		this.mGUIPetInfoSceneRightInfo.SetCurState(1);
		this.Refresh();
	}

	private void OnEnterState4()
	{
		this.mState0Go.SetActive(false);
		this.mState1Go.SetActive(true);
		if (this.mItemInfo != null)
		{
			int itemCount = Globals.Instance.Player.ItemSystem.GetItemCount(this.mItemInfo.ID);
			this.mSuiPianNumTxt.text = this.mSb.Remove(0, this.mSb.Length).Append(itemCount).Append("/").Append(this.mItemInfo.Value1).ToString();
			this.mSuiPianJindu.value = ((this.mItemInfo.Value1 == 0) ? 0f : Mathf.Clamp01((float)itemCount / (float)this.mItemInfo.Value1));
		}
		this.mGUIPetInfoSceneRightInfo.SetCurState(0);
		this.Refresh();
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIManager.mInstance.DestroyPetInfoSceneV2();
	}

	private void OnLittlePlusBtnclick(GameObject go)
	{
		if (this.mItemInfo != null)
		{
			GUIHowGetPetItemPopUp.ShowThis(this.mItemInfo);
			return;
		}
		if (this.mCurPetDataEx != null)
		{
			GUIHowGetPetItemPopUp.ShowThis(this.mCurPetDataEx.Info);
			return;
		}
	}

	private void OnChangePetBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		if (this.mCurPetDataEx.IsPetBattling() || this.mCurPetDataEx.IsPetAssisting())
		{
			GameUIManager.mInstance.uiState.CombatPetSlot = this.mCurPetDataEx.GetSocketSlot();
			GameUIManager.mInstance.ChangeSession<GUIPartnerFightScene>(null, false, false);
		}
		GameUIManager.mInstance.DestroyPetInfoSceneV2();
	}

	private void OnJueXingBtnClick(GameObject go)
	{
		global::Debug.Log(new object[]
		{
			"OnJueXingBtnClick(GameObject go)"
		});
	}

	private void OnSkillBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.CombatPetSlot = this.mCurPetDataEx.GetSocketSlot();
		GameUIManager.mInstance.DestroyPetInfoSceneV2();
		GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mCurPetDataEx;
		GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 3;
		GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
		GameUIManager.mInstance.uiState.IsShowPetZhuWei = true;
		GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
	}

	public void OnFurtherBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.CombatPetSlot = this.mCurPetDataEx.GetSocketSlot();
		GameUIManager.mInstance.DestroyPetInfoSceneV2();
		GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mCurPetDataEx;
		GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 2;
		GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
		GameUIManager.mInstance.uiState.IsShowPetZhuWei = true;
		GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
	}

	public void OnLvlUpBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.CombatPetSlot = this.mCurPetDataEx.GetSocketSlot();
		GameUIManager.mInstance.DestroyPetInfoSceneV2();
		GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mCurPetDataEx;
		GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 1;
		GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
		GameUIManager.mInstance.uiState.IsShowPetZhuWei = true;
		GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
	}

	private void OnLeftBtnClick(GameObject go)
	{
		if (this.mCurPetDataEx != null)
		{
			if (this.mCurrentPetDataIndex <= 0)
			{
				this.mCurrentPetDataIndex = this.mPetDatas.Count;
			}
			this.mCurrentPetDataIndex--;
			this.CurPetDataEx = this.mPetDatas[this.mCurrentPetDataIndex];
			this.RightInfoWhichPart = 0;
			this.RefreshState0();
		}
	}

	private void OnRightBtnClick(GameObject go)
	{
		if (this.mCurPetDataEx != null)
		{
			this.mCurrentPetDataIndex++;
			if (this.mCurrentPetDataIndex >= this.mPetDatas.Count)
			{
				this.mCurrentPetDataIndex = 0;
			}
			this.CurPetDataEx = this.mPetDatas[this.mCurrentPetDataIndex];
			this.RightInfoWhichPart = 0;
			this.RefreshState0();
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
		if (this.mCurPetDataEx.GetSocketSlot() == 0)
		{
			if (this.mUIState != 3)
			{
				this.asyncEntiry = ActorManager.CreateLocalUIActor(0, 450000, true, true, this.mCardModel, 1.15f, delegate(GameObject go)
				{
					this.asyncEntiry = null;
					this.mModelTmp = go;
					if (this.mModelTmp != null)
					{
						this.mUIActorController = this.mModelTmp.GetComponent<UIActorController>();
						if (this.mUIActorController != null)
						{
							this.mUIActorController.PlayIdleAnimationAndVoice();
						}
						Tools.SetMeshRenderQueue(this.mModelTmp, 5390);
					}
				});
			}
			else
			{
				this.asyncEntiry = ActorManager.CreateRemoteUIActor(0, 450000, true, true, this.mCardModel, 1.15f, delegate(GameObject go)
				{
					this.asyncEntiry = null;
					this.mModelTmp = go;
					if (this.mModelTmp != null)
					{
						this.mUIActorController = this.mModelTmp.GetComponent<UIActorController>();
						if (this.mUIActorController != null)
						{
							this.mUIActorController.PlayIdleAnimationAndVoice();
						}
						Tools.SetMeshRenderQueue(this.mModelTmp, 5390);
					}
				});
			}
		}
		else
		{
			this.asyncEntiry = ActorManager.CreateUIPet(this.mCurPetDataEx.Info.ID, 450, true, true, this.mCardModel, 1.15f, 2, delegate(GameObject go)
			{
				this.asyncEntiry = null;
				this.mModelTmp = go;
				if (this.mModelTmp != null)
				{
					this.mUIActorController = this.mModelTmp.GetComponent<UIActorController>();
					if (this.mUIActorController != null)
					{
						this.mUIActorController.PlayIdleAnimationAndVoice();
					}
					Tools.SetMeshRenderQueue(this.mModelTmp, 5390);
				}
			});
		}
	}

	private void Refresh()
	{
		if (this.mCurPetDataEx != null)
		{
			this.mGUIPetTitleInfo.Refresh(this.mCurPetDataEx, this.mUIState != 3, this.mCurPetDataEx.GetSocketSlot() == 0);
			this.CreateModel();
			this.mGUIPetInfoSceneRightInfo.Refresh(this.mCurPetDataEx);
		}
	}

	private void InitPetDatas()
	{
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (current.IsBattling())
			{
				this.mPetDatas.Add(current);
			}
		}
		this.mPetDatas.Sort(delegate(PetDataEx a, PetDataEx b)
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
	}

	private void SetCurPetIndex()
	{
		for (int i = 0; i < this.mPetDatas.Count; i++)
		{
			if (this.mCurPetDataEx == this.mPetDatas[i])
			{
				this.mCurrentPetDataIndex = i;
				break;
			}
		}
	}

	private void OnUnBattleBtnClick(GameObject go)
	{
		if (this.mCurPetDataEx != null && this.mCurPetDataEx.IsPetAssisting())
		{
			MC2S_SetCombatPet mC2S_SetCombatPet = new MC2S_SetCombatPet();
			mC2S_SetCombatPet.Slot = this.mCurPetDataEx.GetSocketSlot();
			mC2S_SetCombatPet.PetID = 0uL;
			Globals.Instance.CliSession.Send(195, mC2S_SetCombatPet);
		}
	}

	private void OnEquipPetEvent(int slot)
	{
		GameUIManager.mInstance.DestroyPetInfoSceneV2();
	}
}
