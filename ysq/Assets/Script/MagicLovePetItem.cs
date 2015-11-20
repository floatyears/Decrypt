using Att;
using System;
using UnityEngine;

public class MagicLovePetItem : MonoBehaviour
{
	private int mIndex;

	private GameObject mSlot;

	private UITexture mUnknow;

	private GameObject mUI73;

	private GameObject mUI103;

	private Renderer[] mSkinnedRenderers;

	private UIActorController mActor;

	private GameObject mModel;

	private ResourceEntity asyncEntity;

	private byte colorValue;

	public PetInfo petInfo
	{
		get;
		private set;
	}

	public void Init(GUIMagicLoveScene basescene, int index)
	{
		this.mIndex = index;
		this.mSlot = GameUITools.FindGameObject("Slot", base.gameObject);
		Transform transform = base.transform.Find("Unknow");
		if (transform != null)
		{
			this.mUnknow = transform.GetComponent<UITexture>();
			this.mUnknow.gameObject.SetActive(true);
			this.mUnknow.enabled = false;
		}
		this.mUI73 = GameUITools.FindGameObject("ui73", base.gameObject);
		Tools.SetParticleRenderQueue(this.mUI73, 3003, 1f);
		this.mUI73.SetActive(false);
		this.mUI103 = GameUITools.FindGameObject("ui103", base.gameObject);
		Tools.SetParticleRenderQueue(this.mUI103, 3003, 1f);
		this.mUI103.SetActive(false);
	}

	public void Refresh(int petInfoID, bool playWand = false)
	{
		NGUITools.SetActive(this.mUI73, false);
		if (this.petInfo != null && this.petInfo.ID == petInfoID)
		{
			return;
		}
		this.ClearModel();
		if (petInfoID != 0)
		{
			this.petInfo = Globals.Instance.AttDB.PetDict.GetInfo(petInfoID);
			this.asyncEntity = ActorManager.CreateUIPet(petInfoID, 0, true, false, this.mSlot, 1f, 0, delegate(GameObject go)
			{
				this.asyncEntity = null;
				this.mModel = go;
				if (go == null)
				{
					global::Debug.Log(new object[]
					{
						"CreateUIPlayer error"
					});
				}
				else
				{
					NGUITools.SetLayer(go, LayerDefine.MonsterLayer);
					Tools.SetMeshRenderQueue(go, 3002);
					go.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
					this.mActor = go.GetComponent<UIActorController>();
					if (this.mActor == null)
					{
						this.mActor = go.AddComponent<UIActorController>();
					}
					this.mActor.IsPlayer = false;
					this.mSkinnedRenderers = go.GetComponentsInChildren<Renderer>(true);
					if (playWand)
					{
						this.PlayWandEffect();
					}
					this.SetDark(this.colorValue);
					this.ResetRotation();
				}
			});
		}
		if (this.mUnknow != null)
		{
			this.mUnknow.enabled = (petInfoID == 0);
		}
		this.ResetRotation();
	}

	private void PlayWandEffect()
	{
		NGUITools.SetActive(this.mUI73, false);
		NGUITools.SetActive(this.mUI73, true);
	}

	public void ClearModel()
	{
		if (this.asyncEntity != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntity);
			this.asyncEntity = null;
		}
		if (this.mModel != null)
		{
			this.mActor = null;
			UnityEngine.Object.Destroy(this.mModel);
			this.mModel = null;
		}
	}

	public void SetDark(byte value)
	{
		this.colorValue = value;
		if (this.mModel != null && this.mUnknow == null && this.mSkinnedRenderers != null && this.mSkinnedRenderers.Length == 0)
		{
			this.mSkinnedRenderers = this.mModel.GetComponentsInChildren<Renderer>();
		}
		if (this.mSkinnedRenderers != null)
		{
			for (int i = 0; i < this.mSkinnedRenderers.Length; i++)
			{
				this.mSkinnedRenderers[i].material.color = new Color32(value, value, value, 255);
			}
		}
		if (this.mUnknow != null && this.mUnknow.enabled)
		{
			if (value == 100)
			{
				value = 150;
			}
			else
			{
				value = 255;
			}
			this.mUnknow.color = new Color32(value, value, value, 255);
		}
	}

	public void RefreshRewardFX()
	{
		this.mUI103.SetActive(Globals.Instance.Player.MagicLoveSystem.CanTakeReward(this.mIndex));
	}

	private void Update()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.ResetRotation();
	}

	private void ResetRotation()
	{
		base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
	}
}
