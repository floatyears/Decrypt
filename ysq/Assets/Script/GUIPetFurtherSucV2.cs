using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using System;
using System.Text;
using UnityEngine;

public class GUIPetFurtherSucV2 : MonoBehaviour
{
	private GameObject mCardModel;

	private GameObject mModelTmp;

	private UIActorController mUIActorController;

	private UILabel mJinJieNum0;

	private UILabel mJinJieNum1;

	private UILabel mHpNum0;

	private UILabel mHpNum1;

	private UILabel mAttackNum0;

	private UILabel mAttackNum1;

	private UILabel mWufangNum0;

	private UILabel mWufangNum1;

	private UILabel mFafangNum0;

	private UILabel mFafangNum1;

	private UILabel mTianfuDesc;

	private TweenScale mContinueScale;

	private GameObject mContinueBtn;

	private GameObject mFlowerSp;

	private UISprite mRightInfoBg;

	private UISprite mPassSkillIcon;

	private SkillInfo mPassSkillInfo;

	private GameUIToolTip mSkillToolTip;

	private GameObject mPasSkillGo;

	private GameObject mTianfuGo;

	private GameObject mUIEffect0;

	private GameObject mUIEffect1;

	private GameObject mUIEffect3;

	private GameObject mUIEffect4;

	private Sequence mSequenceForSuc;

	private StringBuilder mSb = new StringBuilder();

	private PetDataEx mCurPetDataEx;

	private ResourceEntity asyncEntiry;

	public PetDataEx CurPetDataEx
	{
		get
		{
			return this.mCurPetDataEx;
		}
		set
		{
			this.mCurPetDataEx = value;
			this.Refresh();
		}
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle");
		this.mFlowerSp = transform.Find("flower").gameObject;
		this.mFlowerSp.transform.localScale = Vector3.zero;
		this.mUIEffect0 = this.mFlowerSp.transform.Find("ui57_4").gameObject;
		Tools.SetParticleRenderQueue2(this.mUIEffect0, 5500);
		NGUITools.SetActive(this.mUIEffect0, false);
		this.mUIEffect1 = base.transform.Find("Texture/ui57").gameObject;
		Tools.SetParticleRenderQueue2(this.mUIEffect1, 5490);
		NGUITools.SetActive(this.mUIEffect1, false);
		this.mUIEffect3 = base.transform.Find("Texture/ui57_5").gameObject;
		Tools.SetParticleRenderQueue2(this.mUIEffect3, 5450);
		NGUITools.SetActive(this.mUIEffect3, false);
		this.mUIEffect4 = base.transform.Find("Texture/ui57_6").gameObject;
		Tools.SetParticleRenderQueue2(this.mUIEffect4, 5550);
		NGUITools.SetActive(this.mUIEffect4, false);
		this.mCardModel = transform.Find("modelPos").gameObject;
		Transform transform2 = transform.Find("rightInfo");
		transform2.localScale = new Vector3(0f, 1f, 1f);
		this.mRightInfoBg = transform2.GetComponent<UISprite>();
		this.mJinJieNum0 = transform2.Find("num0").GetComponent<UILabel>();
		this.mJinJieNum1 = transform2.Find("num1").GetComponent<UILabel>();
		Transform transform3 = transform2.Find("hpBg");
		this.mHpNum0 = transform3.Find("num").GetComponent<UILabel>();
		this.mHpNum1 = transform3.Find("num2").GetComponent<UILabel>();
		Transform transform4 = transform2.Find("attackBg");
		this.mAttackNum0 = transform4.Find("num").GetComponent<UILabel>();
		this.mAttackNum1 = transform4.Find("num2").GetComponent<UILabel>();
		Transform transform5 = transform2.Find("wufangBg");
		this.mWufangNum0 = transform5.Find("num").GetComponent<UILabel>();
		this.mWufangNum1 = transform5.Find("num2").GetComponent<UILabel>();
		Transform transform6 = transform2.Find("fafangBg");
		this.mFafangNum0 = transform6.Find("num").GetComponent<UILabel>();
		this.mFafangNum1 = transform6.Find("num2").GetComponent<UILabel>();
		this.mPasSkillGo = transform2.Find("txt1").gameObject;
		this.mPassSkillIcon = this.mPasSkillGo.transform.Find("passiveSkill/skill").GetComponent<UISprite>();
		UIEventListener expr_2BD = UIEventListener.Get(this.mPassSkillIcon.gameObject);
		expr_2BD.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_2BD.onPress, new UIEventListener.BoolDelegate(this.OnPassiveSkillIconPress));
		this.mTianfuGo = transform2.Find("txt2").gameObject;
		this.mTianfuDesc = this.mTianfuGo.transform.Find("tianFuDesc").GetComponent<UILabel>();
		this.mContinueBtn = transform.Find("continueBtn").gameObject;
		UIEventListener expr_335 = UIEventListener.Get(this.mContinueBtn);
		expr_335.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_335.onClick, new UIEventListener.VoidDelegate(this.OnContinueBtnClick));
		this.mContinueScale = this.mContinueBtn.GetComponent<TweenScale>();
		this.mContinueBtn.transform.localScale = Vector3.zero;
		this.mSequenceForSuc = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate));
	}

	private void OnPassiveSkillIconPress(GameObject go, bool isPressed)
	{
		if (this.mPassSkillInfo != null && isPressed)
		{
			if (this.mSkillToolTip == null)
			{
				this.mSkillToolTip = GameUIToolTipManager.GetInstance().CreateSkillTooltip(go.transform, this.mPassSkillInfo);
			}
			this.mSb.Remove(0, this.mSb.Length).Append("[66FE00]").Append(this.mPassSkillInfo.Desc).Append("[-]");
			this.mSkillToolTip.Create(Tools.GetCameraRootParent(go.transform), this.mPassSkillInfo.Name, this.mSb.ToString());
			this.mSkillToolTip.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(-100f, 100f, -7000f));
			this.mSkillToolTip.EnableToolTip();
		}
		else if (this.mSkillToolTip != null)
		{
			this.mSkillToolTip.HideTipAnim();
		}
	}

	private void OnContinueBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.mSequenceForSuc.Kill();
		GameUIManager.mInstance.DestroyPetFurtherSucV2();
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private void Refresh()
	{
		if (this.CurPetDataEx != null)
		{
			this.mJinJieNum0.text = (this.CurPetDataEx.Data.Further - 1u).ToString();
			this.mJinJieNum1.text = this.CurPetDataEx.Data.Further.ToString();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			this.CurPetDataEx.GetAttribute(ref num, ref num2, ref num3, ref num4);
			this.mHpNum1.text = num.ToString();
			this.mAttackNum1.text = num2.ToString();
			this.mWufangNum1.text = num3.ToString();
			this.mFafangNum1.text = num4.ToString();
			this.mHpNum0.text = GameUIManager.mInstance.uiState.MaxHp.ToString();
			this.mAttackNum0.text = GameUIManager.mInstance.uiState.Attack.ToString();
			this.mWufangNum0.text = GameUIManager.mInstance.uiState.WuFang.ToString();
			this.mFafangNum0.text = GameUIManager.mInstance.uiState.FaFang.ToString();
			if (this.CurPetDataEx.GetSocketSlot() != 0 && (this.CurPetDataEx.Data.Further == 3u || this.CurPetDataEx.Data.Further == 4u))
			{
				this.mRightInfoBg.height = 320;
				this.mPasSkillGo.SetActive(true);
				this.mPassSkillInfo = this.CurPetDataEx.GetSkillInfo((int)(this.CurPetDataEx.Data.Further - 1u));
				if (this.mPassSkillInfo != null)
				{
					this.mPassSkillIcon.spriteName = this.mPassSkillInfo.Icon;
				}
			}
			else
			{
				this.mRightInfoBg.height = 270;
				this.mPasSkillGo.SetActive(false);
			}
			int num5 = (int)(this.CurPetDataEx.Data.Further - 1u);
			if (0 <= num5 && num5 < this.CurPetDataEx.Info.TalentID.Count)
			{
				TalentInfo info = Globals.Instance.AttDB.TalentDict.GetInfo(this.CurPetDataEx.Info.TalentID[num5]);
				if (info != null)
				{
					this.mSb.Remove(0, this.mSb.Length).Append("[00ff00]").Append("[").Append(info.Name).Append("] ").Append(info.Desc);
					this.mTianfuDesc.text = this.mSb.ToString();
				}
				else
				{
					this.mTianfuDesc.text = this.mSb.Remove(0, this.mSb.Length).Append("[e2c497]").Append(Singleton<StringManager>.Instance.GetString("curLvlNotTF")).ToString();
				}
			}
			else
			{
				this.mTianfuDesc.text = this.mSb.Remove(0, this.mSb.Length).Append("[e2c497]").Append(Singleton<StringManager>.Instance.GetString("curLvlNotTF")).ToString();
			}
			this.CreateModel();
			this.PlayUIAnimation();
		}
	}

	private void ShowEffect0()
	{
		NGUITools.SetActive(this.mUIEffect0, false);
		NGUITools.SetActive(this.mUIEffect0, true);
		Globals.Instance.EffectSoundMgr.Play("ui/ui_006");
	}

	private void ShowEffect1()
	{
		NGUITools.SetActive(this.mUIEffect1, false);
		NGUITools.SetActive(this.mUIEffect1, true);
	}

	private void ShowEffect3()
	{
		NGUITools.SetActive(this.mUIEffect3, false);
		NGUITools.SetActive(this.mUIEffect4, false);
		NGUITools.SetActive(this.mUIEffect3, true);
		NGUITools.SetActive(this.mUIEffect4, true);
		Globals.Instance.EffectSoundMgr.Play("ui/ui_025a");
	}

	private void ScaleContinueBtn()
	{
		this.mUIActorController.PlayIdleAnimationAndVoice();
		this.mContinueScale.enabled = true;
	}

	private void PlayUIAnimation()
	{
		this.mSequenceForSuc.AppendInterval(0.2f);
		this.mSequenceForSuc.AppendCallback(new TweenDelegate.TweenCallback(this.ShowEffect3));
		this.mSequenceForSuc.AppendInterval(1.8f);
		this.mSequenceForSuc.Append(HOTween.To(this.mFlowerSp.transform, 0.001f, new TweenParms().Prop("localScale", new Vector3(10f, 10f, 10f))));
		this.mSequenceForSuc.Append(HOTween.To(this.mFlowerSp.transform, 0.3f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseInCubic)));
		this.mSequenceForSuc.AppendCallback(new TweenDelegate.TweenCallback(this.ShowEffect0));
		this.mSequenceForSuc.AppendInterval(0.1f);
		this.mSequenceForSuc.AppendCallback(new TweenDelegate.TweenCallback(this.ShowEffect1));
		this.mSequenceForSuc.Append(HOTween.To(this.mCardModel.transform, 0.6f, new TweenParms().Prop("localPosition", new PlugVector3X(-270f))));
		this.mSequenceForSuc.Insert(2.7f, HOTween.To(this.mRightInfoBg.transform, 0.6f, new TweenParms().Prop("localScale", new PlugVector3X(1f))));
		this.mSequenceForSuc.AppendInterval(0.5f);
		this.mSequenceForSuc.Append(HOTween.To(this.mContinueBtn.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack)));
		this.mSequenceForSuc.AppendCallback(new TweenDelegate.TweenCallback(this.ScaleContinueBtn));
		this.mSequenceForSuc.Play();
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
			this.asyncEntiry = ActorManager.CreateLocalUIActor(0, 450, true, true, this.mCardModel, 1.15f, delegate(GameObject go)
			{
				this.asyncEntiry = null;
				this.mModelTmp = go;
				if (this.mModelTmp != null)
				{
					this.mUIActorController = this.mModelTmp.GetComponent<UIActorController>();
					Tools.SetMeshRenderQueue(this.mModelTmp, 5500);
				}
			});
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
					Tools.SetMeshRenderQueue(this.mModelTmp, 5500);
				}
			});
		}
	}
}
