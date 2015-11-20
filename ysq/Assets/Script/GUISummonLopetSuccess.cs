using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUISummonLopetSuccess : MonoBehaviour
{
	private static GUISummonLopetSuccess mInstance;

	private GameObject mCardModel;

	private GameObject mModelTmp;

	private UIActorController mUIActorController;

	private TweenScale mContinueScale;

	private GameObject mContinueBtn;

	private GameObject mFlowerSp;

	private GameObject mUIEffect0;

	private GameObject mUIEffect1;

	private GameObject mUIEffect3;

	private GameObject mUIEffect4;

	private Sequence mSequenceForSuc;

	private GameObject mShareBtn;

	private LopetDataEx mCurPetDataEx;

	private ResourceEntity asyncEntiry;

	private bool isSharing;

	public static void Show(LopetDataEx lopetData)
	{
		if (lopetData == null)
		{
			return;
		}
		if (GUISummonLopetSuccess.mInstance == null)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/GUISummonLopetSuccess");
			if (@object == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUISummonLopetSuccess error"
				});
				return;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = GameUIManager.mInstance.uiCamera.gameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 3000f);
			gameObject.transform.localScale = Vector3.one;
			GUISummonLopetSuccess.mInstance = gameObject.AddComponent<GUISummonLopetSuccess>();
		}
		GUISummonLopetSuccess.mInstance.Init(lopetData);
	}

	public static bool TryClose()
	{
		if (GUISummonLopetSuccess.mInstance != null)
		{
			UnityEngine.Object.Destroy(GUISummonLopetSuccess.mInstance.gameObject);
			GUISummonLopetSuccess.mInstance = null;
			return true;
		}
		return false;
	}

	private void Init(LopetDataEx data)
	{
		if (data == null)
		{
			GUISummonLopetSuccess.TryClose();
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
		this.mContinueBtn = transform.Find("continueBtn").gameObject;
		UIEventListener expr_154 = UIEventListener.Get(this.mContinueBtn);
		expr_154.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_154.onClick, new UIEventListener.VoidDelegate(this.OnContinueBtnClick));
		this.mContinueScale = this.mContinueBtn.GetComponent<TweenScale>();
		this.mContinueBtn.transform.localScale = Vector3.zero;
		this.mSequenceForSuc = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate));
		this.mShareBtn = GameUITools.RegisterClickEvent("ShareBtn", new UIEventListener.VoidDelegate(this.OnShareBtnClick), transform.gameObject);
		this.mShareBtn.SetActive(false);
	}

	private void Refresh()
	{
		this.CreateModel();
		this.PlayUIAnimation();
	}

	private void OnContinueBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.mSequenceForSuc.Kill();
		GUISummonLopetSuccess.TryClose();
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
		if (GUIRewardShareInfo.IsOpen())
		{
			this.mShareBtn.SetActive(this.mCurPetDataEx.Info.Quality == 3);
		}
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
		this.mSequenceForSuc.AppendInterval(0.5f);
		this.mSequenceForSuc.Append(HOTween.To(this.mContinueBtn.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack)));
		this.mSequenceForSuc.AppendCallback(new TweenDelegate.TweenCallback(this.ScaleContinueBtn));
		this.mSequenceForSuc.Play();
	}

	private void OnDestroy()
	{
		if (this.mSequenceForSuc != null)
		{
			this.mSequenceForSuc.Kill();
			this.mSequenceForSuc = null;
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

	private void OnShareBtnClick(GameObject go)
	{
		if (!this.isSharing && !GameUISharePopUp.isSharing)
		{
			base.StartCoroutine(this.ShowSharePopUp());
		}
	}

	[DebuggerHidden]
	private IEnumerator ShowSharePopUp()
	{
        return null;
        //GUISummonLopetSuccess.<ShowSharePopUp>c__Iterator63 <ShowSharePopUp>c__Iterator = new GUISummonLopetSuccess.<ShowSharePopUp>c__Iterator63();
        //<ShowSharePopUp>c__Iterator.<>f__this = this;
        //return <ShowSharePopUp>c__Iterator;
	}
}
