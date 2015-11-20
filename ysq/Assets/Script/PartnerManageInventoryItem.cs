using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class PartnerManageInventoryItem : UICustomGridItem
{
	private const int StarNums = 5;

	private GUIPartnerManageScene mBaseScene;

	public PetDataEx mPetData;

	private UISprite mItemPartnerBg;

	private UISprite mParBg;

	private UISprite mPar;

	private UISprite mItemPar;

	private UISprite mGoBg;

	private UISprite mSkillBg;

	private GameObject mStarCon;

	private UISprite mFurtherLock;

	private UISprite mAwakeLock;

	private UISprite mPeiYangLock;

	private GameObject[] mStars = new GameObject[5];

	private GameObject mChangeBtn;

	public GameObject mSkillBtn;

	private GameObject mInBtn;

	private GameObject mUpBtn;

	private GameObject mAwakeBtn;

	private GameObject mClothesBtn;

	private GameObject mPeiYangBtn;

	private UILabel mLv;

	private UILabel mGo;

	private UILabel mZiZhi;

	private UILabel mZiZhiNum;

	private UILabel mName;

	private UILabel mDef;

	private UILabel mAwake;

	private UILabel mAwakeNum;

	private UILabel mLvlUp;

	private UILabel mClothesTxt;

	private UILabel mSkillTxt;

	private UILabel mFurtherTxt;

	private UILabel mAwakeTxt;

	private UILabel mPeiYangTxt;

	private string tempStr;

	private int tempInt;

	private static PetDataEx mCurBagPetData;

	private static PartnerManageInventoryItem mCurBagItem;

	public void InitItemData(GUIPartnerManageScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mParBg = base.transform.Find("parBg").GetComponent<UISprite>();
		this.mPar = this.mParBg.transform.Find("par").GetComponent<UISprite>();
		this.mGoBg = this.mParBg.transform.Find("goBg").GetComponent<UISprite>();
		this.mZiZhi = base.transform.Find("zizhi").GetComponent<UILabel>();
		this.mZiZhiNum = base.transform.Find("ziZhiNum").GetComponent<UILabel>();
		this.mChangeBtn = base.transform.Find("changeBtn").gameObject;
		this.mInBtn = base.transform.Find("skillBg/inBtn").gameObject;
		this.mSkillBtn = base.transform.Find("skillBg/skillBtn").gameObject;
		this.mUpBtn = base.transform.Find("skillBg/upBtn").gameObject;
		this.mClothesBtn = base.transform.Find("skillBg/clothesBtn").gameObject;
		this.mAwakeBtn = base.transform.Find("skillBg/awakeBtn").gameObject;
		this.mPeiYangBtn = base.transform.Find("skillBg/peiyangBtn").gameObject;
		this.mName = base.transform.Find("name").GetComponent<UILabel>();
		this.mLv = this.mParBg.transform.Find("Lv").GetComponent<UILabel>();
		this.mGo = this.mParBg.transform.Find("goBg/go").GetComponent<UILabel>();
		this.mDef = base.transform.Find("skillBg/def").GetComponent<UILabel>();
		this.mLvlUp = this.mUpBtn.transform.Find("up").GetComponent<UILabel>();
		this.mClothesTxt = this.mClothesBtn.transform.Find("clothes").GetComponent<UILabel>();
		this.mSkillTxt = this.mSkillBtn.transform.Find("skill").GetComponent<UILabel>();
		this.mFurtherTxt = this.mInBtn.transform.Find("in").GetComponent<UILabel>();
		this.mFurtherLock = this.mInBtn.transform.Find("lock").GetComponent<UISprite>();
		this.mAwakeTxt = this.mAwakeBtn.transform.Find("awake").GetComponent<UILabel>();
		this.mAwakeLock = this.mAwakeBtn.transform.Find("lock").GetComponent<UISprite>();
		this.mPeiYangTxt = this.mPeiYangBtn.transform.Find("peiyang").GetComponent<UILabel>();
		this.mPeiYangLock = this.mPeiYangBtn.transform.Find("lock").GetComponent<UISprite>();
		this.mAwake = base.transform.Find("skillBg/awake").GetComponent<UILabel>();
		this.mAwakeNum = base.transform.Find("skillBg/awakeNum").GetComponent<UILabel>();
		this.mStarCon = base.transform.Find("skillBg/stars").gameObject;
		for (int i = 0; i < 5; i++)
		{
			this.mStars[i] = this.mStarCon.transform.Find(string.Format("star{0}/star", i)).gameObject;
		}
		UIEventListener expr_384 = UIEventListener.Get(this.mParBg.gameObject);
		expr_384.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_384.onClick, new UIEventListener.VoidDelegate(this.OnIconBtnClicked));
		UIEventListener expr_3B5 = UIEventListener.Get(this.mChangeBtn.gameObject);
		expr_3B5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3B5.onClick, new UIEventListener.VoidDelegate(this.OnChangeBtnClicked));
		UIEventListener expr_3E6 = UIEventListener.Get(this.mAwakeBtn.gameObject);
		expr_3E6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3E6.onClick, new UIEventListener.VoidDelegate(this.OnAwakeBtnClicked));
		UIEventListener expr_417 = UIEventListener.Get(this.mPeiYangBtn.gameObject);
		expr_417.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_417.onClick, new UIEventListener.VoidDelegate(this.OnPeiYangBtnClicked));
		UIEventListener expr_448 = UIEventListener.Get(this.mInBtn.gameObject);
		expr_448.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_448.onClick, new UIEventListener.VoidDelegate(this.OnInBtnClicked));
		UIEventListener expr_479 = UIEventListener.Get(this.mUpBtn.gameObject);
		expr_479.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_479.onClick, new UIEventListener.VoidDelegate(this.OnUpBtnClicked));
		UIEventListener expr_4AA = UIEventListener.Get(this.mClothesBtn.gameObject);
		expr_4AA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4AA.onClick, new UIEventListener.VoidDelegate(this.OnClothesBtnClicked));
		UIEventListener expr_4DB = UIEventListener.Get(this.mSkillBtn.gameObject);
		expr_4DB.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4DB.onClick, new UIEventListener.VoidDelegate(this.OnSkillBtnClicked));
	}

	public override void Refresh(object data)
	{
		if (this.mPetData == data)
		{
			return;
		}
		this.mPetData = (PetDataEx)data;
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.mPetData == null)
		{
			return;
		}
		this.mName.text = Singleton<StringManager>.Instance.GetString("PetName", new object[]
		{
			(this.mPetData.GetSocketSlot() != 0) ? Tools.GetPetName(this.mPetData.Info) : Globals.Instance.Player.Data.Name,
			(this.mPetData.Data.Further <= 0u) ? null : Singleton<StringManager>.Instance.GetString("PetAdvance", new object[]
			{
				this.mPetData.Data.Further
			})
		});
		this.mName.color = Tools.GetItemQualityColor(this.mPetData.Info.Quality);
		this.mClothesTxt.text = Singleton<StringManager>.Instance.GetString("PetClothes");
		this.mLvlUp.text = Singleton<StringManager>.Instance.GetString("PetLvlUp");
		this.mSkillTxt.text = Singleton<StringManager>.Instance.GetString("PetSkill");
		this.mFurtherTxt.text = Singleton<StringManager>.Instance.GetString("PetFurther");
		this.mAwakeTxt.text = Singleton<StringManager>.Instance.GetString("PetFurther10");
		this.mPeiYangTxt.text = Singleton<StringManager>.Instance.GetString("PetFurther11");
		if ((ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(12)) || this.mPetData.IsPlayerBattling())
		{
			this.mFurtherLock.gameObject.SetActive(false);
		}
		else
		{
			this.mFurtherLock.gameObject.SetActive(true);
		}
		if ((ulong)this.mPetData.Data.Level >= (ulong)((long)GameConst.GetInt32(24)))
		{
			this.mAwakeLock.gameObject.SetActive(false);
		}
		if ((ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(122)))
		{
			this.mPeiYangLock.gameObject.SetActive(false);
		}
		else
		{
			this.mPeiYangLock.gameObject.SetActive(true);
		}
		if (this.mPetData.IsPlayerBattling())
		{
			SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(0);
			if (socket != null)
			{
				this.mPar.spriteName = socket.GetIcon();
				this.mPar.atlas = this.mBaseScene.mAvatarIcon;
			}
		}
		else
		{
			this.mPar.spriteName = this.mPetData.Info.Icon;
			this.mPar.atlas = this.mBaseScene.mPetIcon;
		}
		this.mDef.text = Singleton<StringManager>.Instance.GetString(string.Format("petType{0}", this.mPetData.Info.Type));
		if (this.mPetData.Data.Level > 0u)
		{
			this.mLv.gameObject.SetActive(true);
			this.mLv.text = Singleton<StringManager>.Instance.GetString("PetLevelNum", new object[]
			{
				this.mPetData.Data.Level
			});
		}
		else
		{
			this.mLv.gameObject.SetActive(false);
		}
		this.tempInt = this.mPetData.Info.SubQuality;
		if (this.tempInt > 0)
		{
			this.mZiZhi.gameObject.SetActive(true);
			this.mZiZhiNum.gameObject.SetActive(true);
			this.mZiZhi.text = Singleton<StringManager>.Instance.GetString("PetSubQualityTxt");
			this.mZiZhiNum.text = Singleton<StringManager>.Instance.GetString("PetSubQualityNum", new object[]
			{
				this.mPetData.Info.SubQuality
			});
		}
		else
		{
			this.mZiZhi.gameObject.SetActive(false);
			this.mZiZhiNum.gameObject.SetActive(false);
		}
		if (this.mPetData.Info.Quality >= 0)
		{
			this.mParBg.spriteName = Tools.GetItemQualityIcon(this.mPetData.Info.Quality);
			NGUITools.SetActive(this.mParBg.gameObject, true);
		}
		else
		{
			NGUITools.SetActive(this.mParBg.gameObject, false);
		}
		if (this.mPetData.IsBattling())
		{
			this.mGoBg.enabled = true;
			this.mGo.enabled = true;
			this.mGo.text = Singleton<StringManager>.Instance.GetString("battle");
			this.mGoBg.color = new Color(255f, 255f, 255f);
		}
		else if (this.mPetData.IsPetAssisting())
		{
			this.mGoBg.enabled = true;
			this.mGo.enabled = true;
			this.mGoBg.color = new Color(255f, 255f, 255f);
			this.mGo.text = Singleton<StringManager>.Instance.GetString("PetFurther8");
		}
		else
		{
			this.mGoBg.enabled = false;
			this.mGo.enabled = false;
		}
		if (this.mInBtn.activeInHierarchy)
		{
			this.mDef.gameObject.SetActive(false);
			this.mAwake.enabled = false;
			this.mAwakeNum.enabled = false;
			this.mStarCon.SetActive(false);
		}
		this.RefreshAwakeShow();
	}

	private void RefreshAwakeShow()
	{
		if (this.mPetData.State == 0)
		{
			if ((ulong)this.mPetData.Data.Level >= (ulong)((long)GameConst.GetInt32(24)))
			{
				this.mAwake.gameObject.SetActive(true);
				this.mAwakeNum.gameObject.SetActive(true);
				this.mStarCon.gameObject.SetActive(true);
				uint num = 0u;
				uint petStarAndLvl = Tools.GetPetStarAndLvl(this.mPetData.Data.Awake, out num);
				this.mAwake.text = Singleton<StringManager>.Instance.GetString("PetAwake1");
				this.mAwakeNum.text = Singleton<StringManager>.Instance.GetString("PetAwake2", new object[]
				{
					petStarAndLvl,
					num
				});
				for (uint num2 = 0u; num2 < 5u; num2 += 1u)
				{
					this.mStars[(int)((UIntPtr)num2)].SetActive(num2 < petStarAndLvl);
				}
			}
			else
			{
				this.mAwake.gameObject.SetActive(false);
				this.mAwakeNum.gameObject.SetActive(false);
				this.mStarCon.gameObject.SetActive(false);
			}
			this.mDef.gameObject.SetActive(true);
			this.mSkillBtn.gameObject.SetActive(false);
			this.mUpBtn.gameObject.SetActive(false);
			this.mClothesBtn.gameObject.SetActive(false);
			this.mInBtn.SetActive(false);
			this.mAwakeBtn.SetActive(false);
			this.mPeiYangBtn.SetActive(false);
			this.mChangeBtn.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
		else
		{
			this.mChangeBtn.transform.localRotation = Quaternion.Euler(0f, 0f, 179.9f);
			if (this.mPetData.IsPlayerBattling())
			{
				this.mSkillBtn.gameObject.SetActive(false);
				this.mUpBtn.gameObject.SetActive(false);
				this.mClothesBtn.gameObject.SetActive(true);
				this.mInBtn.SetActive(true);
				this.mPeiYangBtn.SetActive(true);
				this.mPeiYangBtn.transform.localPosition = new Vector2(this.mInBtn.transform.localPosition.x - 59f, this.mInBtn.transform.localPosition.y);
				if ((ulong)this.mPetData.Data.Level >= (ulong)((long)GameConst.GetInt32(26)))
				{
					this.mAwakeBtn.transform.localPosition = new Vector2(this.mPeiYangBtn.transform.localPosition.x - 59f, this.mInBtn.transform.localPosition.y);
					this.mAwakeBtn.SetActive(true);
				}
				this.mAwake.enabled = false;
				this.mAwakeNum.enabled = false;
				this.mStarCon.SetActive(false);
				this.mDef.gameObject.SetActive(false);
			}
			else
			{
				this.mSkillBtn.gameObject.SetActive(true);
				this.mUpBtn.gameObject.SetActive(true);
				this.mClothesBtn.gameObject.SetActive(false);
				this.mInBtn.SetActive(true);
				this.mPeiYangBtn.SetActive(true);
				if ((ulong)this.mPetData.Data.Level >= (ulong)((long)GameConst.GetInt32(26)))
				{
					this.mAwakeBtn.SetActive(true);
				}
				this.mAwake.enabled = false;
				this.mAwakeNum.enabled = false;
				this.mStarCon.SetActive(false);
				this.mDef.gameObject.SetActive(false);
			}
			if (PartnerManageInventoryItem.mCurBagItem != this)
			{
				PartnerManageInventoryItem.mCurBagItem = this;
				PartnerManageInventoryItem.mCurBagPetData = this.mPetData;
			}
		}
	}

	private void OnIconBtnClicked(GameObject go)
	{
		if (this.mPetData != null)
		{
			GameUIManager.mInstance.uiState.SelectPetID = this.mPetData.Data.ID;
			GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mPetData;
			GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = GameUIManager.mInstance.uiState.PropsBagSceneToTrainIndex;
			GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
			GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
		}
	}

	public void OnSkillBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mPetData != null)
		{
			GameUIManager.mInstance.uiState.SelectPetID = this.mPetData.Data.ID;
			GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mPetData;
			GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 3;
			GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
			GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
		}
	}

	private void OnUpBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mPetData != null)
		{
			GameUIManager.mInstance.uiState.SelectPetID = this.mPetData.Data.ID;
			GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mPetData;
			GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 1;
			GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
			GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
		}
	}

	private void OnClothesBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mPetData != null)
		{
			GameUIManager.mInstance.uiState.SelectPetID = this.mPetData.Data.ID;
			GameUIManager.mInstance.ChangeSession<GUIShiZhuangSceneV2>(null, false, true);
		}
	}

	private void OnInBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if ((ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(12)) || this.mPetData.IsPlayerBattling())
		{
			global::Debug.Log(new object[]
			{
				string.Format("{0},{1}", this.mPetData.GetFurtherNeedLvl(), this.mPetData.GetMaxFurther(false))
			});
			GameUIManager.mInstance.uiState.SelectPetID = this.mPetData.Data.ID;
			GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mPetData;
			GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 2;
			GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
			GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("PetFurther1", new object[]
			{
				GameConst.GetInt32(12)
			}), 0f, 0f);
		}
	}

	private void OnAwakeBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if ((ulong)this.mPetData.Data.Level >= (ulong)((long)GameConst.GetInt32(24)))
		{
			GameUIManager.mInstance.uiState.SelectPetID = this.mPetData.Data.ID;
			GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mPetData;
			GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 5;
			GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
			GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("PetFurther9", new object[]
			{
				GameConst.GetInt32(24)
			}), 0f, 0f);
		}
	}

	private void OnPeiYangBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if ((ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(122)))
		{
			GameUIManager.mInstance.uiState.SelectPetID = this.mPetData.Data.ID;
			GameUIManager.mInstance.uiState.mPetTrainCurPetDataEx = this.mPetData;
			GameUIManager.mInstance.uiState.mPetTrainCurPageIndex = 4;
			GameUIManager.mInstance.uiState.mPetTrainLvlPageIndex = 0;
			GameUIManager.mInstance.ChangeSession<GUIPetTrainSceneV2>(null, false, true);
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("PetFurther12", new object[]
			{
				GameConst.GetInt32(122)
			}), 0f, 0f);
		}
	}

	public void OnChangeBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mChangeBtn.GetComponent<UIButton>().isEnabled = false;
		base.StartCoroutine(this.RefreshChangeBtn());
	}

	[DebuggerHidden]
	private IEnumerator RefreshChangeBtn()
	{
        return null;
        //PartnerManageInventoryItem.<RefreshChangeBtn>c__Iterator87 <RefreshChangeBtn>c__Iterator = new PartnerManageInventoryItem.<RefreshChangeBtn>c__Iterator87();
        //<RefreshChangeBtn>c__Iterator.<>f__this = this;
        //return <RefreshChangeBtn>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator WaitOutShow1()
	{
        return null;
        //PartnerManageInventoryItem.<WaitOutShow1>c__Iterator88 <WaitOutShow1>c__Iterator = new PartnerManageInventoryItem.<WaitOutShow1>c__Iterator88();
        //<WaitOutShow1>c__Iterator.<>f__this = this;
        //return <WaitOutShow1>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator WaitInShow()
	{
        return null;
        //PartnerManageInventoryItem.<WaitInShow>c__Iterator89 <WaitInShow>c__Iterator = new PartnerManageInventoryItem.<WaitInShow>c__Iterator89();
        //<WaitInShow>c__Iterator.<>f__this = this;
        //return <WaitInShow>c__Iterator;
	}
}
