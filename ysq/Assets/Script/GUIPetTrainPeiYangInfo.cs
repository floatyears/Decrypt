using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIPetTrainPeiYangInfo : MonoBehaviour
{
	private GUIPetTrainSceneV2 mBaseScene;

	private UISlider mGongJiSlider;

	private UISlider mShengMingSlider;

	private UISlider mWuFangSlider;

	private UISlider mFaFangSlider;

	private UILabel mSliderNum0;

	private UILabel mSliderNum1;

	private UILabel mSliderNum2;

	private UILabel mSliderNum3;

	private UILabel mNum0;

	private UILabel mNum1;

	private UILabel mNum2;

	private UILabel mNum3;

	private UISprite mHaveItemIcon;

	private UISprite mHaveItemQuality;

	private UILabel mHaveNum;

	private GUIPetTrainPeiYangTog mChuanShuoTog;

	private UISprite mChuanItemIcon;

	private UISprite mChuanItemQuality;

	private UILabel mChuanNum;

	private GUIPetTrainPeiYangTog mZongShiTog;

	private UISprite mZongItemIcon;

	private UISprite mZongItemQuality;

	private UILabel mZongNum;

	private UILabel mMoneyNum;

	private GUIPetTrainPeiYangTog mDaShiTog;

	private UISprite mDaItemIcon;

	private UISprite mDaItemQuality;

	private UILabel mDaNum;

	private UILabel mDiamondNum;

	private GameObject mPeiYangBtn;

	private GameObject mTiHuanBtn;

	private GameObject[] mEffect89 = new GameObject[4];

	public GameObject[] mFalgUp = new GameObject[4];

	public TweenScale[] mFalgUpScale = new TweenScale[4];

	private GameObject[] mFlagDown = new GameObject[4];

	public TweenScale[] mFalgDownScale = new TweenScale[4];

	private UILabel mCiShuBtnLb;

	private int mNumType;

	private int mOldAttackPreview;

	private int mOldHpPreview;

	private int mOldWuFangPreview;

	private int mOldFaFangPreview;

	private StringBuilder mSb = new StringBuilder(42);

	public void InitWithBaseScene(GUIPetTrainSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mGongJiSlider = base.transform.Find("txt0/expBar").GetComponent<UISlider>();
		this.mSliderNum0 = this.mGongJiSlider.transform.Find("num").GetComponent<UILabel>();
		this.mNum0 = base.transform.Find("txt0/num").GetComponent<UILabel>();
		this.mEffect89[0] = this.mGongJiSlider.transform.Find("ui89").gameObject;
		Tools.SetParticleRQWithUIScale(this.mEffect89[0], 3500);
		NGUITools.SetActive(this.mEffect89[0], false);
		this.mShengMingSlider = base.transform.Find("txt1/expBar").GetComponent<UISlider>();
		this.mSliderNum1 = this.mShengMingSlider.transform.Find("num").GetComponent<UILabel>();
		this.mNum1 = base.transform.Find("txt1/num").GetComponent<UILabel>();
		this.mEffect89[1] = this.mShengMingSlider.transform.Find("ui89").gameObject;
		Tools.SetParticleRQWithUIScale(this.mEffect89[1], 3500);
		NGUITools.SetActive(this.mEffect89[1], false);
		this.mWuFangSlider = base.transform.Find("txt2/expBar").GetComponent<UISlider>();
		this.mSliderNum2 = this.mWuFangSlider.transform.Find("num").GetComponent<UILabel>();
		this.mNum2 = base.transform.Find("txt2/num").GetComponent<UILabel>();
		this.mEffect89[2] = this.mWuFangSlider.transform.Find("ui89").gameObject;
		Tools.SetParticleRQWithUIScale(this.mEffect89[2], 3500);
		NGUITools.SetActive(this.mEffect89[2], false);
		this.mFaFangSlider = base.transform.Find("txt3/expBar").GetComponent<UISlider>();
		this.mSliderNum3 = this.mFaFangSlider.transform.Find("num").GetComponent<UILabel>();
		this.mNum3 = base.transform.Find("txt3/num").GetComponent<UILabel>();
		this.mEffect89[3] = this.mFaFangSlider.transform.Find("ui89").gameObject;
		Tools.SetParticleRQWithUIScale(this.mEffect89[3], 3500);
		NGUITools.SetActive(this.mEffect89[3], false);
		Transform transform = base.transform.Find("infoBg2");
		Transform transform2 = transform.Find("haveItem");
		this.mHaveItemIcon = transform2.Find("icon").GetComponent<UISprite>();
		UIEventListener expr_2A3 = UIEventListener.Get(this.mHaveItemIcon.gameObject);
		expr_2A3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2A3.onClick, new UIEventListener.VoidDelegate(this.OnItemIconClick));
		this.mHaveItemQuality = transform2.Find("qualityMask").GetComponent<UISprite>();
		this.mHaveNum = transform2.Find("num").GetComponent<UILabel>();
		Transform transform3 = transform.Find("txt1");
		this.mChuanShuoTog = transform3.Find("toggle").gameObject.AddComponent<GUIPetTrainPeiYangTog>();
		this.mChuanShuoTog.InitToggleBtn(true);
		Transform transform4 = transform3.Find("haveItem");
		this.mChuanItemIcon = transform4.Find("icon").GetComponent<UISprite>();
		UIEventListener expr_355 = UIEventListener.Get(this.mChuanItemIcon.gameObject);
		expr_355.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_355.onClick, new UIEventListener.VoidDelegate(this.OnItemIconClick));
		this.mChuanItemQuality = transform4.Find("qualityMask").GetComponent<UISprite>();
		this.mChuanNum = transform4.Find("num").GetComponent<UILabel>();
		Transform transform5 = transform.Find("txt2");
		this.mZongShiTog = transform5.Find("toggle").gameObject.AddComponent<GUIPetTrainPeiYangTog>();
		this.mZongShiTog.InitToggleBtn(false);
		Transform transform6 = transform5.Find("haveItem");
		this.mZongItemIcon = transform6.Find("icon").GetComponent<UISprite>();
		UIEventListener expr_40C = UIEventListener.Get(this.mZongItemIcon.gameObject);
		expr_40C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_40C.onClick, new UIEventListener.VoidDelegate(this.OnItemIconClick));
		this.mZongItemQuality = transform6.Find("qualityMask").GetComponent<UISprite>();
		this.mZongNum = transform6.Find("num").GetComponent<UILabel>();
		this.mMoneyNum = transform5.Find("money/num").GetComponent<UILabel>();
		Transform transform7 = transform.Find("txt3");
		this.mDaShiTog = transform7.Find("toggle").gameObject.AddComponent<GUIPetTrainPeiYangTog>();
		this.mDaShiTog.InitToggleBtn(false);
		Transform transform8 = transform7.Find("haveItem");
		this.mDaItemIcon = transform8.Find("icon").GetComponent<UISprite>();
		UIEventListener expr_4DC = UIEventListener.Get(this.mDaItemIcon.gameObject);
		expr_4DC.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4DC.onClick, new UIEventListener.VoidDelegate(this.OnItemIconClick));
		this.mDaItemQuality = transform8.Find("qualityMask").GetComponent<UISprite>();
		this.mDaNum = transform8.Find("num").GetComponent<UILabel>();
		this.mDiamondNum = transform7.Find("money/num").GetComponent<UILabel>();
		for (int i = 0; i < this.mFalgUp.Length; i++)
		{
			this.mFalgUp[i] = base.transform.Find(string.Format("txt{0}/up", i)).gameObject;
			this.mFalgUpScale[i] = this.mFalgUp[i].GetComponent<TweenScale>();
			this.mFalgUpScale[i].enabled = false;
			this.mFlagDown[i] = base.transform.Find(string.Format("txt{0}/down", i)).gameObject;
			this.mFalgDownScale[i] = this.mFlagDown[i].GetComponent<TweenScale>();
			this.mFalgDownScale[i].enabled = false;
			this.mFalgUp[i].gameObject.SetActive(false);
			this.mFlagDown[i].gameObject.SetActive(false);
		}
		this.mPeiYangBtn = base.transform.Find("peiYangBtn").gameObject;
		UIEventListener expr_64D = UIEventListener.Get(this.mPeiYangBtn);
		expr_64D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_64D.onClick, new UIEventListener.VoidDelegate(this.OnPeiYangBtnClick));
		this.mTiHuanBtn = base.transform.Find("tiHuanBtn").gameObject;
		UIEventListener expr_694 = UIEventListener.Get(this.mTiHuanBtn);
		expr_694.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_694.onClick, new UIEventListener.VoidDelegate(this.OnTiHuanBtnClick));
		GameObject gameObject = base.transform.Find("ciShuBtn").gameObject;
		UIEventListener expr_6D3 = UIEventListener.Get(gameObject);
		expr_6D3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6D3.onClick, new UIEventListener.VoidDelegate(this.OnCiShuBtnClick));
		this.mCiShuBtnLb = gameObject.transform.Find("Label").GetComponent<UILabel>();
		this.mCiShuBtnLb.text = Singleton<StringManager>.Instance.GetString("petPeiYang0", new object[]
		{
			1
		});
		this.mNumType = 0;
		GUIPetTrainPeiYangTog expr_746 = this.mChuanShuoTog;
		expr_746.ToggleChangedEvent = (GUIPetTrainPeiYangTog.ToggleChangedCallback)Delegate.Combine(expr_746.ToggleChangedEvent, new GUIPetTrainPeiYangTog.ToggleChangedCallback(this.OnChuanToggled));
		GUIPetTrainPeiYangTog expr_76D = this.mZongShiTog;
		expr_76D.ToggleChangedEvent = (GUIPetTrainPeiYangTog.ToggleChangedCallback)Delegate.Combine(expr_76D.ToggleChangedEvent, new GUIPetTrainPeiYangTog.ToggleChangedCallback(this.OnZongToggled));
		GUIPetTrainPeiYangTog expr_794 = this.mDaShiTog;
		expr_794.ToggleChangedEvent = (GUIPetTrainPeiYangTog.ToggleChangedCallback)Delegate.Combine(expr_794.ToggleChangedEvent, new GUIPetTrainPeiYangTog.ToggleChangedCallback(this.OnDaToggled));
	}

	public void Refresh()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx == null)
		{
			return;
		}
		CultivateInfo info = Globals.Instance.AttDB.CultivateDict.GetInfo((int)((curPetDataEx.Data.Level + 9u) / 10u));
		if (info != null)
		{
			int num = 0;
			if (curPetDataEx.Info.Quality >= 0 && curPetDataEx.Info.Quality < info.MaxCulAttack.Count)
			{
				num = info.MaxCulAttack[curPetDataEx.Info.Quality];
			}
			this.mSb.Remove(0, this.mSb.Length).Append(curPetDataEx.Data.Attack).Append("/").Append(num);
			if (curPetDataEx.Data.Attack >= num)
			{
				this.mSb.Append(Singleton<StringManager>.Instance.GetString("petPeiYang1"));
			}
			this.mSliderNum0.text = this.mSb.ToString();
			this.mGongJiSlider.value = Mathf.Clamp01((float)curPetDataEx.Data.Attack / (float)num);
			if (curPetDataEx.Data.AttackPreview < 0)
			{
				this.mNum0.text = this.mSb.Remove(0, this.mSb.Length).Append("-").Append(Mathf.Abs(curPetDataEx.Data.AttackPreview)).ToString();
				this.mNum0.color = Color.red;
			}
			else if (curPetDataEx.Data.AttackPreview == 0)
			{
				this.mNum0.text = this.mSb.Remove(0, this.mSb.Length).Append("+0").ToString();
				this.mNum0.color = Color.white;
			}
			else
			{
				this.mNum0.text = this.mSb.Remove(0, this.mSb.Length).Append("+").Append(curPetDataEx.Data.AttackPreview).ToString();
				this.mNum0.color = Color.green;
			}
			int num2 = 0;
			if (curPetDataEx.Info.Quality >= 0 && curPetDataEx.Info.Quality < info.MaxCulMaxHP.Count)
			{
				num2 = info.MaxCulMaxHP[curPetDataEx.Info.Quality];
			}
			this.mSb.Remove(0, this.mSb.Length).Append(curPetDataEx.Data.MaxHP).Append("/").Append(num2);
			if (curPetDataEx.Data.MaxHP >= num2)
			{
				this.mSb.Append(Singleton<StringManager>.Instance.GetString("petPeiYang1"));
			}
			this.mSliderNum1.text = this.mSb.ToString();
			this.mShengMingSlider.value = Mathf.Clamp01((float)curPetDataEx.Data.MaxHP / (float)num2);
			if (curPetDataEx.Data.MaxHPPreview < 0)
			{
				this.mNum1.text = this.mSb.Remove(0, this.mSb.Length).Append("-").Append(Mathf.Abs(curPetDataEx.Data.MaxHPPreview)).ToString();
				this.mNum1.color = Color.red;
			}
			else if (curPetDataEx.Data.MaxHPPreview == 0)
			{
				this.mNum1.text = this.mSb.Remove(0, this.mSb.Length).Append("+0").ToString();
				this.mNum1.color = Color.white;
			}
			else
			{
				this.mNum1.text = this.mSb.Remove(0, this.mSb.Length).Append("+").Append(curPetDataEx.Data.MaxHPPreview).ToString();
				this.mNum1.color = Color.green;
			}
			int num3 = 0;
			if (curPetDataEx.Info.Quality >= 0 && curPetDataEx.Info.Quality < info.MaxCulPhysicDefense.Count)
			{
				num3 = info.MaxCulPhysicDefense[curPetDataEx.Info.Quality];
			}
			this.mSb.Remove(0, this.mSb.Length).Append(curPetDataEx.Data.PhysicDefense).Append("/").Append(num3);
			if (curPetDataEx.Data.PhysicDefense >= num3)
			{
				this.mSb.Append(Singleton<StringManager>.Instance.GetString("petPeiYang1"));
			}
			this.mSliderNum2.text = this.mSb.ToString();
			this.mWuFangSlider.value = Mathf.Clamp01((float)curPetDataEx.Data.PhysicDefense / (float)num3);
			if (curPetDataEx.Data.PhysicDefensePreview < 0)
			{
				this.mNum2.text = this.mSb.Remove(0, this.mSb.Length).Append("-").Append(Mathf.Abs(curPetDataEx.Data.PhysicDefensePreview)).ToString();
				this.mNum2.color = Color.red;
			}
			else if (curPetDataEx.Data.PhysicDefensePreview == 0)
			{
				this.mNum2.text = this.mSb.Remove(0, this.mSb.Length).Append("+0").ToString();
				this.mNum2.color = Color.white;
			}
			else
			{
				this.mNum2.text = this.mSb.Remove(0, this.mSb.Length).Append("+").Append(curPetDataEx.Data.PhysicDefensePreview).ToString();
				this.mNum2.color = Color.green;
			}
			int num4 = 0;
			if (curPetDataEx.Info.Quality >= 0 && curPetDataEx.Info.Quality < info.MaxCulMagicDefense.Count)
			{
				num4 = info.MaxCulMagicDefense[curPetDataEx.Info.Quality];
			}
			this.mSb.Remove(0, this.mSb.Length).Append(curPetDataEx.Data.MagicDefense).Append("/").Append(num4);
			if (curPetDataEx.Data.MagicDefense >= num4)
			{
				this.mSb.Append(Singleton<StringManager>.Instance.GetString("petPeiYang1"));
			}
			this.mSliderNum3.text = this.mSb.ToString();
			this.mFaFangSlider.value = Mathf.Clamp01((float)curPetDataEx.Data.MagicDefense / (float)num4);
			if (curPetDataEx.Data.MagicDefensePreview < 0)
			{
				this.mNum3.text = this.mSb.Remove(0, this.mSb.Length).Append("-").Append(Mathf.Abs(curPetDataEx.Data.MagicDefensePreview)).ToString();
				this.mNum3.color = Color.red;
			}
			else if (curPetDataEx.Data.MagicDefensePreview == 0)
			{
				this.mNum3.text = this.mSb.Remove(0, this.mSb.Length).Append("+0").ToString();
				this.mNum3.color = Color.white;
			}
			else
			{
				this.mNum3.text = this.mSb.Remove(0, this.mSb.Length).Append("+").Append(curPetDataEx.Data.MagicDefensePreview).ToString();
				this.mNum3.color = Color.green;
			}
		}
		ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(178));
		if (info2 != null)
		{
			this.mHaveItemIcon.spriteName = info2.Icon;
			this.mHaveItemQuality.spriteName = Tools.GetItemQualityIcon(info2.Quality);
			this.mHaveNum.text = Globals.Instance.Player.ItemSystem.GetItemCount(info2.ID).ToString();
			this.mChuanItemIcon.spriteName = info2.Icon;
			this.mChuanItemQuality.spriteName = Tools.GetItemQualityIcon(info2.Quality);
			this.mZongItemIcon.spriteName = info2.Icon;
			this.mZongItemQuality.spriteName = Tools.GetItemQualityIcon(info2.Quality);
			this.mDaItemIcon.spriteName = info2.Icon;
			this.mDaItemQuality.spriteName = Tools.GetItemQualityIcon(info2.Quality);
		}
		if (curPetDataEx.Data.AttackPreview >= 0 && curPetDataEx.Data.MaxHPPreview >= 0 && curPetDataEx.Data.PhysicDefensePreview >= 0 && curPetDataEx.Data.MagicDefensePreview >= 0)
		{
			if (curPetDataEx.Data.AttackPreview > 0 || curPetDataEx.Data.MaxHPPreview > 0 || curPetDataEx.Data.PhysicDefensePreview > 0 || curPetDataEx.Data.MagicDefensePreview > 0)
			{
				this.mPeiYangBtn.SetActive(false);
			}
			else
			{
				this.mPeiYangBtn.SetActive(true);
			}
		}
		else
		{
			this.mPeiYangBtn.SetActive(true);
		}
		if (curPetDataEx.Data.AttackPreview <= 0 && curPetDataEx.Data.MaxHPPreview <= 0 && curPetDataEx.Data.PhysicDefensePreview <= 0 && curPetDataEx.Data.MagicDefensePreview <= 0)
		{
			this.mTiHuanBtn.SetActive(false);
		}
		else
		{
			this.mTiHuanBtn.SetActive(true);
		}
		this.RefreshCostInfo();
		this.RefreshPeiYangAni();
		this.mBaseScene.RefreshPeiYangNewMark();
	}

	private void RefreshCostInfo()
	{
		int num = 0;
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(178));
		if (info != null)
		{
			num = Globals.Instance.Player.ItemSystem.GetItemCount(info.ID);
		}
		int num2 = GameConst.GetInt32(179);
		int num3 = GameConst.GetInt32(180);
		int num4 = GameConst.GetInt32(181);
		int num5 = GameConst.GetInt32(182);
		int num6 = GameConst.GetInt32(183);
		if (this.mNumType == 1)
		{
			num2 *= 5;
			num3 *= 5;
			num4 *= 5;
			num5 *= 5;
			num6 *= 5;
		}
		else if (this.mNumType == 2)
		{
			num2 *= 10;
			num3 *= 10;
			num4 *= 10;
			num5 *= 10;
			num6 *= 10;
		}
		this.mChuanNum.text = num2.ToString();
		this.mChuanNum.color = ((num < num2) ? Color.red : new Color(240f, 225f, 176f));
		this.mZongNum.text = num3.ToString();
		this.mZongNum.color = ((num < num3) ? Color.red : new Color(240f, 225f, 176f));
		this.mMoneyNum.text = num5.ToString();
		this.mMoneyNum.color = ((Globals.Instance.Player.Data.Money < num5) ? Color.red : new Color(240f, 225f, 176f));
		this.mDaNum.text = num4.ToString();
		this.mDaNum.color = ((num < num4) ? Color.red : new Color(240f, 225f, 176f));
		this.mDiamondNum.text = num6.ToString();
		this.mDiamondNum.color = ((Globals.Instance.Player.Data.Diamond < num6) ? Color.red : new Color(240f, 225f, 176f));
	}

	private void OnChuanToggled(bool isCheck)
	{
		this.mZongShiTog.IsChecked = false;
		this.mDaShiTog.IsChecked = false;
	}

	private void OnZongToggled(bool isCheck)
	{
		this.mChuanShuoTog.IsChecked = false;
		this.mDaShiTog.IsChecked = false;
	}

	private void OnDaToggled(bool isCheck)
	{
		this.mChuanShuoTog.IsChecked = false;
		this.mZongShiTog.IsChecked = false;
	}

	private int GetCurSelType()
	{
		if (this.mZongShiTog.IsChecked)
		{
			return 1;
		}
		if (this.mDaShiTog.IsChecked)
		{
			return 2;
		}
		return 0;
	}

	private void OnPeiYangBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		int num = 0;
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(178));
		if (info != null)
		{
			num = Globals.Instance.Player.ItemSystem.GetItemCount(info.ID);
		}
		int num2 = GameConst.GetInt32(179);
		int num3 = GameConst.GetInt32(180);
		int num4 = GameConst.GetInt32(181);
		int num5 = GameConst.GetInt32(182);
		int num6 = GameConst.GetInt32(183);
		if (this.mNumType == 1)
		{
			num2 *= 5;
			num3 *= 5;
			num4 *= 5;
			num5 *= 5;
			num6 *= 5;
		}
		else if (this.mNumType == 2)
		{
			num2 *= 10;
			num3 *= 10;
			num4 *= 10;
			num5 *= 10;
			num6 *= 10;
		}
		int curSelType = this.GetCurSelType();
		if (curSelType == 0)
		{
			if (num < num2)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("petPeiYang2", 0f, 0f);
				return;
			}
		}
		else if (curSelType == 1)
		{
			if (num < num3)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("petPeiYang2", 0f, 0f);
				return;
			}
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Money, num5, 0))
			{
				return;
			}
		}
		else if (curSelType == 2)
		{
			if (num < num4)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("petPeiYang2", 0f, 0f);
				return;
			}
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, num6, 0))
			{
				return;
			}
		}
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx == null)
		{
			return;
		}
		MC2S_PetCultivate mC2S_PetCultivate = new MC2S_PetCultivate();
		mC2S_PetCultivate.PetID = ((curPetDataEx.GetSocketSlot() != 0) ? curPetDataEx.Data.ID : 100uL);
		mC2S_PetCultivate.Type = curSelType;
		mC2S_PetCultivate.Count = 1;
		if (this.mNumType == 1)
		{
			mC2S_PetCultivate.Count = 5;
		}
		else if (this.mNumType == 2)
		{
			mC2S_PetCultivate.Count = 10;
		}
		Globals.Instance.CliSession.Send(421, mC2S_PetCultivate);
	}

	private void OnTiHuanBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx == null)
		{
			return;
		}
		this.mOldAttackPreview = curPetDataEx.Data.AttackPreview;
		this.mOldHpPreview = curPetDataEx.Data.MaxHPPreview;
		this.mOldWuFangPreview = curPetDataEx.Data.PhysicDefensePreview;
		this.mOldFaFangPreview = curPetDataEx.Data.MagicDefensePreview;
		MC2S_PetCultivateAck mC2S_PetCultivateAck = new MC2S_PetCultivateAck();
		mC2S_PetCultivateAck.PetID = ((curPetDataEx.GetSocketSlot() != 0) ? curPetDataEx.Data.ID : 100uL);
		Globals.Instance.CliSession.Send(423, mC2S_PetCultivateAck);
	}

	private void OnCiShuBtnClick(GameObject go)
	{
		GUIPeiYangNumPopUp.ShowMe();
	}

	public void SetCiShuNum(int num)
	{
		if (num == 1)
		{
			this.mCiShuBtnLb.text = Singleton<StringManager>.Instance.GetString("petPeiYang0", new object[]
			{
				5
			});
			this.mNumType = 1;
		}
		else if (num == 2)
		{
			this.mCiShuBtnLb.text = Singleton<StringManager>.Instance.GetString("petPeiYang0", new object[]
			{
				10
			});
			this.mNumType = 2;
		}
		else
		{
			this.mCiShuBtnLb.text = Singleton<StringManager>.Instance.GetString("petPeiYang0", new object[]
			{
				1
			});
			this.mNumType = 0;
		}
		this.RefreshCostInfo();
	}

	public void PlayPeiYangEffect()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_028");
		if (this.mOldAttackPreview != 0)
		{
			NGUITools.SetActive(this.mEffect89[0], false);
			NGUITools.SetActive(this.mEffect89[0], true);
		}
		if (this.mOldHpPreview != 0)
		{
			NGUITools.SetActive(this.mEffect89[1], false);
			NGUITools.SetActive(this.mEffect89[1], true);
		}
		if (this.mOldWuFangPreview != 0)
		{
			NGUITools.SetActive(this.mEffect89[2], false);
			NGUITools.SetActive(this.mEffect89[2], true);
		}
		if (this.mOldFaFangPreview != 0)
		{
			NGUITools.SetActive(this.mEffect89[3], false);
			NGUITools.SetActive(this.mEffect89[3], true);
		}
	}

	public void HidePeiYangEffects()
	{
		for (int i = 0; i < this.mEffect89.Length; i++)
		{
			NGUITools.SetActive(this.mEffect89[i], false);
		}
	}

	public void ClosePeiYangAni()
	{
		for (int i = 0; i < 4; i++)
		{
			this.mFalgUp[i].gameObject.SetActive(false);
			this.mFlagDown[i].gameObject.SetActive(false);
		}
	}

	public void RefreshPeiYangAni()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx.Data.AttackPreview > 0)
		{
			this.mFalgUp[0].gameObject.SetActive(true);
			this.mFlagDown[0].gameObject.SetActive(false);
			this.PlayPeiYangAniUp(0);
		}
		else if (curPetDataEx.Data.AttackPreview < 0)
		{
			this.mFalgUp[0].gameObject.SetActive(false);
			this.mFlagDown[0].gameObject.SetActive(true);
			this.PlayPeiYangAniDown(0);
		}
		else
		{
			this.mFlagDown[0].gameObject.SetActive(false);
			this.mFalgUp[0].gameObject.SetActive(false);
		}
		if (curPetDataEx.Data.MaxHPPreview > 0)
		{
			this.mFalgUp[1].gameObject.SetActive(true);
			this.mFlagDown[1].gameObject.SetActive(false);
			this.PlayPeiYangAniUp(1);
		}
		else if (curPetDataEx.Data.MaxHPPreview < 0)
		{
			this.mFalgUp[1].gameObject.SetActive(false);
			this.mFlagDown[1].gameObject.SetActive(true);
			this.PlayPeiYangAniDown(1);
		}
		else
		{
			this.mFlagDown[1].gameObject.SetActive(false);
			this.mFalgUp[1].gameObject.SetActive(false);
		}
		if (curPetDataEx.Data.PhysicDefensePreview > 0)
		{
			this.mFalgUp[2].gameObject.SetActive(true);
			this.mFlagDown[2].gameObject.SetActive(false);
			this.PlayPeiYangAniUp(2);
		}
		else if (curPetDataEx.Data.PhysicDefensePreview < 0)
		{
			this.mFalgUp[2].gameObject.SetActive(false);
			this.mFlagDown[2].gameObject.SetActive(true);
			this.PlayPeiYangAniDown(2);
		}
		else
		{
			this.mFalgUp[2].gameObject.SetActive(false);
			this.mFlagDown[2].gameObject.SetActive(false);
		}
		if (curPetDataEx.Data.MagicDefensePreview > 0)
		{
			this.mFalgUp[3].gameObject.SetActive(true);
			this.mFlagDown[3].gameObject.SetActive(false);
			this.PlayPeiYangAniUp(3);
		}
		else if (curPetDataEx.Data.MagicDefensePreview < 0)
		{
			this.mFalgUp[3].gameObject.SetActive(false);
			this.mFlagDown[3].gameObject.SetActive(true);
			this.PlayPeiYangAniDown(3);
		}
		else
		{
			this.mFalgUp[3].gameObject.SetActive(false);
			this.mFlagDown[3].gameObject.SetActive(false);
		}
	}

	private void PlayPeiYangAniUp(int index)
	{
		if (index < this.mFalgUpScale.Length)
		{
			this.mFalgUpScale[index].enabled = false;
			this.mFalgUpScale[index].enabled = true;
			this.mFalgUp[index].transform.localScale = Vector3.zero;
			this.mFalgUpScale[index].tweenFactor = 0f;
		}
	}

	private void PlayPeiYangAniDown(int index)
	{
		if (index < this.mFalgDownScale.Length)
		{
			this.mFalgDownScale[index].enabled = false;
			this.mFalgDownScale[index].enabled = true;
			this.mFalgDownScale[index].transform.localScale = Vector3.zero;
			this.mFalgDownScale[index].tweenFactor = 0f;
		}
	}

	private void OnItemIconClick(GameObject go)
	{
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(178));
		if (info != null)
		{
			GUIHowGetPetItemPopUp.ShowThis(info);
		}
	}
}
