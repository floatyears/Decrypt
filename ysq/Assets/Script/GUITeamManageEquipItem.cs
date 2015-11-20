using System;
using System.Collections.Generic;
using UnityEngine;

public class GUITeamManageEquipItem : MonoBehaviour
{
	private GUITeamManageSceneV2 mBaseScene;

	private GameObject mItemGo;

	private UISprite mIcon;

	private UISprite mQualityMask;

	private UISprite mQualityTag;

	private GameObject mTagGo;

	private GameObject mPlus;

	private GameObject mLockSp;

	private GameObject mLvlBg;

	private UILabel mEnhanceLvl;

	private UILabel mItemName;

	private bool mIsShowTag;

	private ItemDataEx mItemDataEx;

	private GameObject mEnhanceEffect;

	private GameObject mHuanZhuangEffect;

	private GameObject mJinZhuangFxGo;

	private GameObject mRefineLvlBg;

	private UILabel mRefineLvl;

	private int mIndex;

	private bool mIsLock;

	public bool IsShowTag
	{
		get
		{
			return this.mIsShowTag;
		}
		set
		{
			this.mIsShowTag = value;
			this.mTagGo.SetActive(value);
		}
	}

	public bool IsLock
	{
		get
		{
			return this.mIsLock;
		}
		set
		{
			this.mIsLock = (value && (this.mIndex == 4 || this.mIndex == 5));
			if (this.mLockSp != null)
			{
				this.mLockSp.SetActive(this.mIsLock);
			}
		}
	}

	public void InitWithBaseScene(GUITeamManageSceneV2 baseScene, int index)
	{
		this.mBaseScene = baseScene;
		this.mIndex = index;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mPlus = base.transform.Find("bg/Sprite").gameObject;
		this.mTagGo = base.transform.Find("tag").gameObject;
		if (this.mIndex == 4 || this.mIndex == 5)
		{
			this.mLockSp = base.transform.Find("lock").gameObject;
			this.IsLock = false;
		}
		else
		{
			this.mLockSp = null;
		}
		Transform transform = base.transform.Find("item");
		this.mItemGo = transform.gameObject;
		this.mIcon = transform.Find("icon").GetComponent<UISprite>();
		this.mQualityMask = transform.Find("qualityMask").GetComponent<UISprite>();
		this.mItemGo.SetActive(false);
		this.mRefineLvlBg = transform.Find("BG").gameObject;
		this.mRefineLvl = this.mRefineLvlBg.transform.Find("RefineLevel").GetComponent<UILabel>();
		this.mRefineLvlBg.SetActive(false);
		this.mLvlBg = transform.Find("lvlBg").gameObject;
		this.mQualityTag = this.mLvlBg.GetComponent<UISprite>();
		this.mEnhanceLvl = this.mLvlBg.transform.Find("num").GetComponent<UILabel>();
		this.mEnhanceLvl.text = string.Empty;
		this.mItemName = transform.Find("name").GetComponent<UILabel>();
		this.mLvlBg.SetActive(false);
		this.mEnhanceEffect = base.transform.Find("ui54").gameObject;
		Tools.SetParticleRenderQueue(this.mEnhanceEffect, 4100, 1f);
		NGUITools.SetActive(this.mEnhanceEffect, false);
		this.mHuanZhuangEffect = base.transform.Find("ui55").gameObject;
		Tools.SetParticleRenderQueue(this.mHuanZhuangEffect, 4100, 1f);
		NGUITools.SetActive(this.mHuanZhuangEffect, false);
		UIEventListener expr_20F = UIEventListener.Get(base.gameObject);
		expr_20F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_20F.onClick, new UIEventListener.VoidDelegate(this.OnItemClick));
		this.IsShowTag = false;
	}

	public void HideEffects()
	{
		NGUITools.SetActive(this.mEnhanceEffect, false);
		NGUITools.SetActive(this.mHuanZhuangEffect, false);
	}

	public void PlayEnhanceEffect()
	{
		NGUITools.SetActive(this.mEnhanceEffect, false);
		NGUITools.SetActive(this.mEnhanceEffect, true);
	}

	public void PlayHuanZhuangEffect()
	{
		NGUITools.SetActive(this.mHuanZhuangEffect, false);
		NGUITools.SetActive(this.mHuanZhuangEffect, true);
	}

	public void RefreshRedTag()
	{
		int curSelectIndex = this.mBaseScene.GetCurSelectIndex();
		this.IsShowTag = (this.mBaseScene.IsLocalPlayer && Tools.CanEquipShowMark(curSelectIndex, this.mIndex));
	}

	public void Refresh(bool isEnhanceEvent = false, bool isHuanZhuang = false)
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		bool flag = false;
		uint num = 0u;
		if (this.mBaseScene.IsLocalPlayer)
		{
			num = Globals.Instance.Player.Data.Level;
		}
		else
		{
			SocketDataEx socket = teamSystem.GetSocket(0, false);
			if (socket != null)
			{
				PetDataEx pet = socket.GetPet();
				if (pet != null)
				{
					num = pet.Data.Level;
				}
			}
		}
		if ((ulong)num < (ulong)((long)GameConst.GetInt32(25)) && (this.mIndex == 4 || this.mIndex == 5))
		{
			this.IsLock = true;
		}
		else
		{
			this.IsLock = false;
			int curSelectIndex = this.mBaseScene.GetCurSelectIndex();
			if (teamSystem != null)
			{
				SocketDataEx socket2 = teamSystem.GetSocket(curSelectIndex, this.mBaseScene.IsLocalPlayer);
				if (socket2 != null)
				{
					PetDataEx pet2 = socket2.GetPet();
					if (pet2 != null)
					{
						this.mItemDataEx = socket2.GetEquip(this.mIndex);
						if (this.mItemDataEx != null)
						{
							flag = true;
							this.mItemGo.SetActive(true);
							this.mIcon.spriteName = this.mItemDataEx.Info.Icon;
							this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mItemDataEx.Info.Quality);
							this.mQualityTag.spriteName = Tools.GetItemQualityTagIcon(this.mItemDataEx.Info.Quality);
							if (this.mItemDataEx.Info.Quality >= 4)
							{
								GameObject gameObject = Res.Load<GameObject>("UIFx/ui66", false);
								if (this.mJinZhuangFxGo != null)
								{
									UnityEngine.Object.Destroy(this.mJinZhuangFxGo);
									this.mJinZhuangFxGo = null;
								}
								if (gameObject != null)
								{
									this.mJinZhuangFxGo = NGUITools.AddChild(base.gameObject, gameObject);
									Tools.SetParticleRQWithUIScale(this.mJinZhuangFxGo, 4100);
									this.mJinZhuangFxGo.transform.localPosition = Vector3.zero;
								}
							}
							else if (this.mJinZhuangFxGo != null)
							{
								UnityEngine.Object.Destroy(this.mJinZhuangFxGo);
								this.mJinZhuangFxGo = null;
							}
							int equipEnhanceLevel = this.mItemDataEx.GetEquipEnhanceLevel();
							bool flag2 = false;
							if (equipEnhanceLevel != 0)
							{
								this.mLvlBg.SetActive(true);
								int num2 = (!(this.mEnhanceLvl.text == string.Empty)) ? Convert.ToInt32(this.mEnhanceLvl.text) : equipEnhanceLevel;
								this.mEnhanceLvl.text = equipEnhanceLevel.ToString();
								flag2 = (equipEnhanceLevel > num2);
							}
							else
							{
								this.mLvlBg.SetActive(false);
							}
							this.mItemName.text = this.mItemDataEx.Info.Name;
							this.mItemName.color = Tools.GetItemQualityColor(this.mItemDataEx.Info.Quality);
							this.mPlus.SetActive(false);
							int num3;
							if (this.mIndex < 4)
							{
								num3 = this.mItemDataEx.GetEquipRefineLevel();
							}
							else
							{
								num3 = this.mItemDataEx.GetTrinketRefineLevel();
							}
							if (num3 > 0)
							{
								this.mRefineLvlBg.SetActive(true);
								this.mRefineLvl.text = Singleton<StringManager>.Instance.GetString("petJueXing1", new object[]
								{
									num3
								});
							}
							else
							{
								this.mRefineLvlBg.SetActive(false);
							}
							if (isEnhanceEvent && flag2 && this.mIndex < 4 && this.mBaseScene.IsLocalPlayer)
							{
								this.PlayEnhanceEffect();
							}
							if (isHuanZhuang && this.mBaseScene.IsLocalPlayer)
							{
								this.PlayHuanZhuangEffect();
							}
						}
					}
				}
			}
		}
		if (!flag)
		{
			this.mItemDataEx = null;
			this.mItemGo.SetActive(false);
			this.mPlus.SetActive(this.mBaseScene.IsLocalPlayer && !this.IsLock);
			if (this.mJinZhuangFxGo != null)
			{
				UnityEngine.Object.Destroy(this.mJinZhuangFxGo);
				this.mJinZhuangFxGo = null;
			}
		}
	}

	public void OnItemClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (this.IsLock && this.mBaseScene.IsLocalPlayer)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("teamManage5", new object[]
			{
				GameConst.GetInt32(25)
			}), 0f, 0f);
			return;
		}
		if (this.mItemDataEx != null)
		{
			GUIEquipInfoPopUp.ShowThis(this.mItemDataEx, (!this.mBaseScene.IsLocalPlayer) ? GUIEquipInfoPopUp.EIPT.EIPT_Other : GUIEquipInfoPopUp.EIPT.EIPT_Team, this.mBaseScene.GetCurSelectIndex(), false, this.mBaseScene.IsLocalPlayer);
		}
		else if (this.mBaseScene.IsLocalPlayer)
		{
			int curSelectIndex = this.mBaseScene.GetCurSelectIndex();
			List<ItemDataEx> allEquipTrinketBySlot = Globals.Instance.Player.ItemSystem.GetAllEquipTrinketBySlot(this.mIndex, false);
			if (allEquipTrinketBySlot.Count == 0)
			{
				GUIHowGetPetItemPopUp.ShowLowQualityEquip(this.mIndex);
			}
			else
			{
				TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
				if (teamSystem != null)
				{
					SocketDataEx socket = teamSystem.GetSocket(curSelectIndex);
					if (socket != null && socket.GetPet() != null)
					{
						GameUIManager.mInstance.uiState.CombatPetSlot = curSelectIndex;
						GUISelectEquipBagScene.Change2This(curSelectIndex, this.mIndex);
					}
					else
					{
						GameUIManager.mInstance.ShowMessageTipByKey("teamManage2", 0f, 0f);
					}
				}
			}
		}
	}
}
