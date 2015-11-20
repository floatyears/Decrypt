using System;
using UnityEngine;

public class GUITeamManageAssitPetItem : MonoBehaviour
{
	private GUITeamManageSceneV2 mBaseScene;

	private PetDataEx mPetDataEx;

	private GameObject mItemGo;

	private UISprite mIcon;

	private UISprite mQualityMask;

	private UILabel mUnlockTip;

	private UILabel mLvl;

	private UILabel mName;

	private GameObject mPlus;

	private int mIndex;

	private GameObject mTagGo;

	private bool mIsShowTag;

	public bool IsShowTag
	{
		get
		{
			return this.mIsShowTag;
		}
		set
		{
			this.mIsShowTag = value;
			if (this.mTagGo != null)
			{
				this.mTagGo.SetActive(value);
			}
		}
	}

	private int UnlockNeedLvl()
	{
		return 25 + this.mIndex * 5;
	}

	private bool IsUnlocked()
	{
		uint num = 0u;
		if (this.mBaseScene.IsLocalPlayer)
		{
			num = Globals.Instance.Player.Data.Level;
		}
		else
		{
			SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(0, false);
			if (socket != null)
			{
				PetDataEx pet = socket.GetPet();
				if (pet != null)
				{
					num = pet.Data.Level;
				}
			}
		}
		return (ulong)num >= (ulong)((long)this.UnlockNeedLvl());
	}

	public void InitWithBaseScene(GUITeamManageSceneV2 baseScene, int index)
	{
		this.mBaseScene = baseScene;
		this.mIndex = index;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mItemGo = base.transform.Find("item").gameObject;
		this.mIcon = this.mItemGo.transform.Find("icon").GetComponent<UISprite>();
		this.mQualityMask = this.mItemGo.transform.Find("qualityMask").GetComponent<UISprite>();
		this.mLvl = this.mItemGo.transform.Find("lvl").GetComponent<UILabel>();
		this.mLvl.text = string.Empty;
		this.mName = this.mItemGo.transform.Find("name").GetComponent<UILabel>();
		this.mName.text = string.Empty;
		this.mUnlockTip = base.transform.Find("unLockTip").GetComponent<UILabel>();
		this.mUnlockTip.text = string.Empty;
		this.mPlus = base.transform.Find("plus").gameObject;
		this.mTagGo = base.transform.Find("mark").gameObject;
		this.mTagGo.SetActive(false);
		UIEventListener expr_133 = UIEventListener.Get(base.gameObject);
		expr_133.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_133.onClick, new UIEventListener.VoidDelegate(this.OnItemClick));
	}

	public void Refresh(PetDataEx data)
	{
		this.mPetDataEx = data;
		this.Refresh();
	}

	private void RefreshRedFlag()
	{
		this.IsShowTag = (this.mBaseScene.IsLocalPlayer && Tools.CanAssistPetShowMark(this.mIndex, this.mPetDataEx));
	}

	private void Refresh()
	{
		if (this.IsUnlocked())
		{
			if (this.mPetDataEx != null)
			{
				this.mItemGo.SetActive(true);
				this.mPlus.SetActive(false);
				this.mUnlockTip.gameObject.SetActive(false);
				this.mIcon.spriteName = this.mPetDataEx.Info.Icon;
				this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mPetDataEx.Info.Quality);
				this.mLvl.text = string.Format("Lv{0}", this.mPetDataEx.Data.Level);
				if (this.mPetDataEx.Data.Further > 0u)
				{
					this.mName.text = string.Format("{0}+{1}", Tools.GetPetName(this.mPetDataEx.Info), this.mPetDataEx.Data.Further);
				}
				else
				{
					this.mName.text = Tools.GetPetName(this.mPetDataEx.Info);
				}
				this.mName.color = Tools.GetItemQualityColor(this.mPetDataEx.Info.Quality);
			}
			else
			{
				this.mItemGo.SetActive(false);
				this.mPlus.SetActive(true && this.mBaseScene.IsLocalPlayer);
				this.mUnlockTip.gameObject.SetActive(false);
			}
		}
		else
		{
			this.mItemGo.SetActive(false);
			this.mPlus.SetActive(false);
			this.mUnlockTip.gameObject.SetActive(true && this.mBaseScene.IsLocalPlayer);
			if (this.mBaseScene.IsLocalPlayer)
			{
				this.mUnlockTip.text = Singleton<StringManager>.Instance.GetString("teamManage0", new object[]
				{
					this.UnlockNeedLvl()
				});
			}
		}
		this.RefreshRedFlag();
	}

	private void OnItemClick(GameObject go)
	{
		if (!this.IsUnlocked())
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (this.mPetDataEx != null)
		{
			GameUIManager.mInstance.ShowPetInfoSceneV2(this.mPetDataEx, 0, null, (!this.mBaseScene.IsLocalPlayer) ? 3 : 0);
		}
		else if (this.mBaseScene.IsLocalPlayer)
		{
			if (Globals.Instance.Player.PetSystem.GetUnBattlePetNum() == 0)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("hasNoCurPet", 0f, 0f);
			}
			else
			{
				GameUIManager.mInstance.uiState.CombatPetSlot = 4 + this.mIndex;
				GameUIManager.mInstance.ChangeSession<GUIPartnerFightScene>(null, false, false);
			}
		}
	}
}
