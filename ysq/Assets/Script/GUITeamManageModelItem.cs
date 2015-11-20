using System;
using UnityEngine;

public class GUITeamManageModelItem : MonoBehaviour
{
	private GUITeamManageSceneV2 mBaseScene;

	private GUITeamManageModelData mModelData;

	private GameObject mModelPos;

	private UIActorController mUIActorController;

	private GameObject mModelFxGo;

	private GameObject mBattleBtn;

	private GameObject mBattleBtnBg;

	private ResourceEntity asyncEntiry;

	public GameObject mModelTmp
	{
		get;
		set;
	}

	public GUITeamManageModelData GetModelData()
	{
		return this.mModelData;
	}

	public void InitWithBaseScene(GUITeamManageSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mModelPos = base.transform.Find("modelPos").gameObject;
		this.mBattleBtn = base.transform.Find("battleBtn").gameObject;
		this.mBattleBtnBg = base.transform.Find("Sprite").gameObject;
		UIEventListener expr_5C = UIEventListener.Get(this.mBattleBtn);
		expr_5C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_5C.onClick, new UIEventListener.VoidDelegate(this.OnBattleClick));
		UIEventListener expr_88 = UIEventListener.Get(base.gameObject);
		expr_88.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_88.onClick, new UIEventListener.VoidDelegate(this.OnGameModelClick));
	}

	public void Refresh(GUITeamManageModelData data)
	{
		if (this.mModelData == data)
		{
			return;
		}
		this.mModelData = data;
		this.Refresh();
	}

	public void PlayIdle()
	{
		if (this.mUIActorController != null)
		{
			this.mUIActorController.PlayIdleAnimationAndVoice();
		}
	}

	private void Refresh()
	{
		if (this.mModelData != null)
		{
			if (this.mModelData.mSocketDataEx != null && this.mModelData.mSocketDataEx.GetPet() != null)
			{
				PetDataEx pet = this.mModelData.mSocketDataEx.GetPet();
				this.mModelPos.SetActive(true);
				this.mBattleBtn.SetActive(false);
				this.mBattleBtnBg.SetActive(false);
				this.CreateModel();
				uint num;
				if (this.mModelData.mSocketSlotIndex == 0 && this.mBaseScene.IsLocalPlayer)
				{
					num = (uint)Globals.Instance.Player.Data.FurtherLevel;
				}
				else
				{
					num = pet.Data.Further;
				}
				GameObject gameObject = null;
				if (num == 4u || num == 5u)
				{
					gameObject = Res.Load<GameObject>("UIFx/ui72", false);
				}
				else if (num == 6u || num == 7u)
				{
					gameObject = Res.Load<GameObject>("UIFx/ui71", false);
				}
				else if (num == 8u || num == 9u)
				{
					gameObject = Res.Load<GameObject>("UIFx/ui70", false);
				}
				else if (num == 10u)
				{
					gameObject = Res.Load<GameObject>("UIFx/ui69", false);
				}
				else if (this.mModelFxGo != null)
				{
					UnityEngine.Object.Destroy(this.mModelFxGo);
				}
				if (gameObject != null)
				{
					this.mModelFxGo = NGUITools.AddChild(base.gameObject, gameObject);
					Tools.SetParticleRenderQueue2(this.mModelFxGo, 3900);
					this.mModelFxGo.transform.localPosition = new Vector3(0f, 0f, 1000f);
					this.mModelFxGo.transform.localRotation = Quaternion.Euler(-9.23162f, 0f, 0f);
				}
			}
			else
			{
				this.mModelPos.SetActive(false);
				this.mBattleBtn.SetActive(this.mModelData.mSocketSlotIndex != 4 && this.mModelData.mSocketSlotIndex != 5 && this.mBaseScene.IsLocalPlayer);
				this.mBattleBtnBg.SetActive(this.mModelData.mSocketSlotIndex != 4 && this.mModelData.mSocketSlotIndex != 5 && this.mBaseScene.IsLocalPlayer);
				if (this.mModelFxGo != null)
				{
					UnityEngine.Object.Destroy(this.mModelFxGo);
				}
			}
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
			UnityEngine.Object.DestroyImmediate(this.mModelTmp);
			this.mModelTmp = null;
			this.mUIActorController = null;
		}
	}

	private void CreateModel()
	{
		PetDataEx pet = this.mModelData.mSocketDataEx.GetPet();
		if (this.mModelData != null && this.mModelData.mSocketDataEx != null && pet != null)
		{
			this.ClearModel();
			if (this.mBaseScene.IsLocalPlayer)
			{
				this.asyncEntiry = ActorManager.CreateLocalUIActor(this.mModelData.mSocketSlotIndex, 0, true, false, this.mModelPos, 1.1f, delegate(GameObject go)
				{
					this.asyncEntiry = null;
					this.mModelTmp = go;
					if (this.mModelTmp != null)
					{
						this.mModelTmp.transform.localPosition = new Vector3(0f, 0f, 500f);
						Tools.SetMeshRenderQueue(this.mModelTmp, 3900);
						this.mUIActorController = this.mModelTmp.GetComponent<UIActorController>();
					}
				});
			}
			else
			{
				this.asyncEntiry = ActorManager.CreateRemoteUIActor(this.mModelData.mSocketSlotIndex, 0, true, false, this.mModelPos, 1.1f, delegate(GameObject go)
				{
					this.asyncEntiry = null;
					this.mModelTmp = go;
					if (this.mModelTmp != null)
					{
						this.mModelTmp.transform.localPosition = new Vector3(0f, 0f, 500f);
						Tools.SetMeshRenderQueue(this.mModelTmp, 3900);
						this.mUIActorController = this.mModelTmp.GetComponent<UIActorController>();
					}
				});
			}
		}
	}

	public void OnGameModelClick(GameObject go)
	{
		if (this.mModelData != null && this.mModelData.mSocketDataEx != null && this.mModelData.mSocketDataEx.GetPet() != null)
		{
			if (this.mBaseScene.IsLocalPlayer)
			{
				GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mModelData.mSocketDataEx.GetPet();
				GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 0;
				GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
				GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
			}
			else
			{
				GameUIManager.mInstance.ShowPetInfoSceneV2(this.mModelData.mSocketDataEx.GetPet(), 0, null, 3);
			}
		}
	}

	public void OnBattleClick(GameObject go)
	{
		if (this.mBaseScene.IsLocalPlayer)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
			if (Globals.Instance.Player.PetSystem.GetUnBattlePetNum() == 0)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("hasNoCurPet", 0f, 0f);
				return;
			}
			if (this.mModelData != null)
			{
				GameUIManager.mInstance.uiState.CombatPetSlot = this.mModelData.mSocketSlotIndex;
				GameUIManager.mInstance.ChangeSession<GUIPartnerFightScene>(null, false, false);
			}
		}
	}

	public void SetItemShow(bool isShow)
	{
		if (this.mModelData.mSocketDataEx != null && this.mModelData.mSocketDataEx.GetPet() != null)
		{
			this.mModelPos.SetActive(isShow);
			this.mBattleBtn.SetActive(false);
			this.mBattleBtnBg.SetActive(false);
			if (this.mModelFxGo != null)
			{
				this.mModelFxGo.SetActive(isShow);
			}
		}
		else
		{
			this.mModelPos.SetActive(false);
			if (this.mModelFxGo != null)
			{
				this.mModelFxGo.SetActive(false);
			}
			if (isShow)
			{
				this.mBattleBtn.SetActive(this.mModelData.mSocketSlotIndex != 4 && this.mModelData.mSocketSlotIndex != 5 && this.mBaseScene.IsLocalPlayer);
				this.mBattleBtnBg.SetActive(this.mModelData.mSocketSlotIndex != 4 && this.mModelData.mSocketSlotIndex != 5 && this.mBaseScene.IsLocalPlayer);
			}
			else
			{
				this.mBattleBtn.SetActive(false);
				this.mBattleBtnBg.SetActive(false);
			}
		}
	}
}
