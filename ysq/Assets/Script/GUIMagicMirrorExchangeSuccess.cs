using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using System;
using UnityEngine;

public class GUIMagicMirrorExchangeSuccess : MonoBehaviour
{
	private static GUIMagicMirrorExchangeSuccess mInstance;

	private GameObject mCardModel;

	private GameObject mModelTmp;

	private UIActorController mUIActorController;

	private TweenScale mContinueScale;

	private GameObject mContinueBtn;

	private GameObject mFlowerSp;

	private UISprite mRightInfoBg;

	private UILabel mName;

	private UILabel mLevel;

	private GUIStars mStars;

	private GUIAttributeValue mValues;

	private GUIPetSkills mSkills;

	private GameObject mUIEffect0;

	private GameObject mUIEffect1;

	private GameObject mUIEffect3;

	private GameObject mUIEffect4;

	private Sequence mSequenceForSuc;

	private PetDataEx mCurPetDataEx;

	private ResourceEntity asyncEntiry;

	public static void Show(PetDataEx petData)
	{
		if (petData == null)
		{
			return;
		}
		if (GUIMagicMirrorExchangeSuccess.mInstance == null)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/GUIMagicMirrorExchangeSuccess");
			if (@object == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUIMagicMirrorExchangeSuccess error"
				});
				return;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = GameUIManager.mInstance.uiCamera.gameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 3000f);
			gameObject.transform.localScale = Vector3.one;
			GUIMagicMirrorExchangeSuccess.mInstance = gameObject.AddComponent<GUIMagicMirrorExchangeSuccess>();
		}
		GUIMagicMirrorExchangeSuccess.mInstance.Init(petData);
	}

	public static bool TryClose()
	{
		if (GUIMagicMirrorExchangeSuccess.mInstance != null)
		{
			UnityEngine.Object.Destroy(GUIMagicMirrorExchangeSuccess.mInstance.gameObject);
			GUIMagicMirrorExchangeSuccess.mInstance = null;
			return true;
		}
		return false;
	}

	private void Init(PetDataEx data)
	{
		if (data == null)
		{
			GUIMagicMirrorExchangeSuccess.TryClose();
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
		GameObject parent = GameUITools.FindGameObject("Info", this.mRightInfoBg.gameObject);
		this.mName = GameUITools.FindUILabel("Name", parent);
		this.mLevel = GameUITools.FindUILabel("Level", parent);
		this.mStars = GameUITools.FindGameObject("Stars", parent).AddComponent<GUIStars>();
		this.mStars.Init(5);
		this.mValues = GameUITools.FindGameObject("Values", parent).AddComponent<GUIAttributeValue>();
		this.mSkills = GameUITools.FindGameObject("Skill", parent).AddComponent<GUIPetSkills>();
		this.mContinueBtn = transform.Find("continueBtn").gameObject;
		UIEventListener expr_20C = UIEventListener.Get(this.mContinueBtn);
		expr_20C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_20C.onClick, new UIEventListener.VoidDelegate(this.OnContinueBtnClick));
		this.mContinueScale = this.mContinueBtn.GetComponent<TweenScale>();
		this.mContinueBtn.transform.localScale = Vector3.zero;
		this.mSequenceForSuc = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate));
	}

	private void Refresh()
	{
		if (this.mCurPetDataEx.Data.Further > 0u)
		{
			this.mName.text = Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
			{
				Tools.GetPetName(this.mCurPetDataEx.Info),
				this.mCurPetDataEx.Data.Further
			});
		}
		else
		{
			this.mName.text = Tools.GetPetName(this.mCurPetDataEx.Info);
		}
		this.mName.color = Tools.GetItemQualityColor(this.mCurPetDataEx.Info.Quality);
		this.mLevel.text = Singleton<StringManager>.Instance.GetString("equipImprove16", new object[]
		{
			this.mCurPetDataEx.Data.Level
		});
		uint num;
		this.mStars.Refresh((int)Tools.GetPetStarAndLvl(this.mCurPetDataEx.Data.Awake, out num));
		this.mValues.Refresh(this.mCurPetDataEx, true);
		this.mSkills.Refresh(this.mCurPetDataEx, false);
		this.CreateModel();
		this.PlayUIAnimation();
	}

	private void OnContinueBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.mSequenceForSuc.Kill();
		GUIMagicMirrorExchangeSuccess.TryClose();
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

	private void OnDestroy()
	{
		if (this.mSequenceForSuc != null)
		{
			this.mSequenceForSuc.Kill();
			this.mSequenceForSuc = null;
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
