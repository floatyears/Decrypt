using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using System;
using UnityEngine;

public class GUILopetAwakeSuccess : MonoBehaviour
{
	private static GUILopetAwakeSuccess mInstance;

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

	private TweenScale mContinueScale;

	private GameObject mContinueBtn;

	private GameObject mFlowerSp;

	private UISprite mRightInfoBg;

	private GameObject mUIEffect0;

	private GameObject mUIEffect1;

	private GameObject mUIEffect3;

	private GameObject mUIEffect4;

	private Sequence mSequenceForSuc;

	private LopetDataEx mCurPetDataEx;

	private ResourceEntity asyncEntiry;

	public static void Show(LopetDataEx petData)
	{
		if (petData == null)
		{
			return;
		}
		if (GUILopetAwakeSuccess.mInstance == null)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/GUILopetAwakeSuccess");
			if (@object == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUILopetAwakeSuccess error"
				});
				return;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = GameUIManager.mInstance.uiCamera.gameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 3000f);
			gameObject.transform.localScale = Vector3.one;
			GUILopetAwakeSuccess.mInstance = gameObject.AddComponent<GUILopetAwakeSuccess>();
		}
		GUILopetAwakeSuccess.mInstance.Init(petData);
	}

	public static bool TryClose()
	{
		if (GUILopetAwakeSuccess.mInstance != null)
		{
			UnityEngine.Object.Destroy(GUILopetAwakeSuccess.mInstance.gameObject);
			GUILopetAwakeSuccess.mInstance = null;
			return true;
		}
		return false;
	}

	private void Init(LopetDataEx data)
	{
		if (data == null)
		{
			GUILopetAwakeSuccess.TryClose();
			return;
		}
		this.CreateObjects();
		this.mCurPetDataEx = data;
		this.Refresh();
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
		this.mContinueBtn = transform.Find("continueBtn").gameObject;
		UIEventListener expr_298 = UIEventListener.Get(this.mContinueBtn);
		expr_298.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_298.onClick, new UIEventListener.VoidDelegate(this.OnContinueBtnClick));
		this.mContinueScale = this.mContinueBtn.GetComponent<TweenScale>();
		this.mContinueBtn.transform.localScale = Vector3.zero;
		this.mSequenceForSuc = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate));
	}

	private void OnContinueBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.mSequenceForSuc.Kill();
		GUILopetAwakeSuccess.TryClose();
	}

	private void OnDestroy()
	{
		if (this.mSequenceForSuc != null)
		{
			this.mSequenceForSuc.Kill();
			this.mSequenceForSuc = null;
		}
	}

	private void Refresh()
	{
		if (this.mCurPetDataEx != null)
		{
			this.mJinJieNum0.text = (this.mCurPetDataEx.Data.Awake - 1u).ToString();
			this.mJinJieNum1.text = this.mCurPetDataEx.Data.Awake.ToString();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			this.mCurPetDataEx.GetAttribute(ref num, ref num2, ref num3, ref num4);
			this.mHpNum1.text = num.ToString();
			this.mAttackNum1.text = num2.ToString();
			this.mWufangNum1.text = num3.ToString();
			this.mFafangNum1.text = num4.ToString();
			this.mHpNum0.text = GameUIManager.mInstance.uiState.MaxHp.ToString();
			this.mAttackNum0.text = GameUIManager.mInstance.uiState.Attack.ToString();
			this.mWufangNum0.text = GameUIManager.mInstance.uiState.WuFang.ToString();
			this.mFafangNum0.text = GameUIManager.mInstance.uiState.FaFang.ToString();
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
		this.asyncEntiry = ActorManager.CreateUILopet(this.mCurPetDataEx.Info, 450, true, true, this.mCardModel, 1f, delegate(GameObject go)
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
