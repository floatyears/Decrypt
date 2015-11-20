using System;
using UnityEngine;

public class GUITeamManageSelectItem : MonoBehaviour
{
	private GUITeamManageSceneV2 mBaseScene;

	private GameObject mItemGo;

	private UISprite mIcon;

	private UISprite mQualityMask;

	private GameObject mTagGo;

	private GameObject mSelectBg;

	private GameObject mPlus;

	private GameObject mBgIcon;

	private bool mIsSelected;

	private bool mIsShowTag;

	private int mIndex;

	public bool IsSelected
	{
		get
		{
			return this.mIsSelected;
		}
		set
		{
			this.mIsSelected = value;
			this.mSelectBg.SetActive(value);
		}
	}

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

	public void InitWithBaseScene(GUITeamManageSceneV2 baseScene, int index)
	{
		this.mBaseScene = baseScene;
		this.mIndex = index;
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		if (this.mIndex == 5)
		{
			this.mBgIcon = null;
			this.mItemGo = null;
			this.mIcon = null;
			this.mQualityMask = null;
			this.mPlus = null;
			this.mTagGo = base.transform.Find("tag").gameObject;
			this.mTagGo.SetActive(false);
			this.mSelectBg = base.transform.Find("bg").gameObject;
		}
		else if (this.mIndex == 4)
		{
			this.mBgIcon = base.transform.Find("bgIcon").gameObject;
			this.mPlus = this.mBgIcon.transform.Find("plus").gameObject;
			this.mItemGo = base.transform.Find("item").gameObject;
			this.mIcon = this.mItemGo.transform.Find("icon").GetComponent<UISprite>();
			this.mQualityMask = this.mItemGo.transform.Find("qualityMask").GetComponent<UISprite>();
			this.mTagGo = base.transform.Find("tag").gameObject;
			this.mTagGo.SetActive(false);
			this.mSelectBg = base.transform.Find("bg").gameObject;
			if (!Tools.CanPlay(GameConst.GetInt32(201), this.mBaseScene.IsLocalPlayer) || (!this.mBaseScene.IsLocalPlayer && Globals.Instance.Player.TeamSystem.GetLopet(false) == null))
			{
				base.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			this.mBgIcon = null;
			this.mItemGo = base.transform.Find("item").gameObject;
			this.mIcon = this.mItemGo.transform.Find("icon").GetComponent<UISprite>();
			this.mQualityMask = this.mItemGo.transform.Find("qualityMask").GetComponent<UISprite>();
			this.mTagGo = base.transform.Find("tag").gameObject;
			this.mTagGo.SetActive(false);
			this.mSelectBg = base.transform.Find("bg").gameObject;
			this.mPlus = base.transform.Find("plus").gameObject;
		}
		UIEventListener expr_27F = UIEventListener.Get(base.gameObject);
		expr_27F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_27F.onClick, new UIEventListener.VoidDelegate(this.OnItemClick));
		this.IsSelected = false;
		this.IsShowTag = false;
	}

	public void Refresh()
	{
		if (this.mIndex == 4)
		{
			LopetDataEx curLopet = Globals.Instance.Player.LopetSystem.GetCurLopet(this.mBaseScene.IsLocalPlayer);
			if (curLopet == null)
			{
				this.mBgIcon.SetActive(true);
				this.mItemGo.SetActive(false);
			}
			else
			{
				this.mBgIcon.SetActive(false);
				this.mItemGo.SetActive(true);
				this.mPlus.SetActive(true && this.mBaseScene.IsLocalPlayer);
				this.mIcon.spriteName = curLopet.Info.Icon;
				this.mQualityMask.spriteName = Tools.GetItemQualityIcon(curLopet.Info.Quality);
			}
		}
		else if (this.mIndex != 5)
		{
			TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
			if (teamSystem != null)
			{
				SocketDataEx socket = teamSystem.GetSocket(this.mIndex, this.mBaseScene.IsLocalPlayer);
				if (socket != null)
				{
					PetDataEx pet = socket.GetPet();
					if (pet != null)
					{
						this.mItemGo.SetActive(true);
						this.mPlus.SetActive(false);
						this.mIcon.spriteName = pet.Info.Icon;
						this.mQualityMask.spriteName = Tools.GetItemQualityIcon(pet.Info.Quality);
					}
					else
					{
						this.mItemGo.SetActive(false);
						this.mPlus.SetActive(true && this.mBaseScene.IsLocalPlayer);
					}
				}
			}
		}
		this.RefreshRedFlag();
	}

	public void OnItemClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (this.mIndex == 0 && this.mBaseScene.GetCurSelectIndex() == 0)
		{
			return;
		}
		this.mBaseScene.SetCurSelectItem(this.mIndex);
	}

	public void RefreshRedFlag()
	{
		if (this.mBaseScene.IsLocalPlayer)
		{
			if (Tools.CanBattlePetMark(this.mIndex))
			{
				this.IsShowTag = true;
				return;
			}
			if (this.mIndex == 5 && RedFlagTools.CanShowZhuWeiMark())
			{
				this.IsShowTag = true;
				return;
			}
		}
		this.IsShowTag = false;
	}
}
