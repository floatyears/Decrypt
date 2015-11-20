using Att;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GetPetLayer : MonoBehaviour
{
	public enum EGPL_ShowNewsType
	{
		Null,
		HighRoll,
		LuckyDraw,
		Create
	}

	public delegate void VoidCallback();

	public AnimationCurve BGAlphaCurve;

	public AnimationCurve FlareAlphaCurve;

	public AnimationCurve TitleScaleCurve;

	public AnimationCurve ColorTxtAlphaCurve;

	[NonSerialized]
	public int UI62RQ = 6700;

	[NonSerialized]
	public int PetRQ = 6600;

	[NonSerialized]
	public float BGAlphaDura = 0.2f;

	[NonSerialized]
	public float Interval1;

	[NonSerialized]
	public float Interval2 = 0.15f;

	[NonSerialized]
	public float Interval3 = 0.1f;

	[NonSerialized]
	public float FlareAlphaDura = 0.5f;

	[NonSerialized]
	public float Interval4 = 0.5f;

	[NonSerialized]
	public Vector3 TitleScaleStartingValue = new Vector3(8f, 8f, 8f);

	[NonSerialized]
	public float TitleScaleDura = 0.3f;

	[NonSerialized]
	public float TitleScaleEffectTime = 0.7f;

	[NonSerialized]
	public float Interval5 = 0.2f;

	[NonSerialized]
	public float ColorTxtAlphaDura = 0.2f;

	[NonSerialized]
	public float Interval6 = 0.6f;

	public GetPetLayer.VoidCallback BGClickCallBack;

	private static GetPetLayer mInstance;

	private UIPanel mPanel;

	private UITexture mBG;

	private UITexture mNormalBG;

	private GameObject mUI62_1;

	private GameObject mUI62_2;

	private UISprite mFlare;

	private GameObject mTitle;

	private GameObject mUI62_3;

	private GameObject mUI62;

	private GameObject mSlot;

	private GameObject mWindow;

	private UILabel mName;

	private UISprite mElement;

	private UISprite mType;

	private UILabel mCon;

	private UILabel mOrange;

	private UILabel mPurple;

	private UILabel mGoOn;

	private GameObject mShareBtn;

	private PetInfo mPetInfo;

	private GetPetLayer.EGPL_ShowNewsType mShowNewsType;

	private bool canClose;

	private GameObject mModel;

	private bool IsInit = true;

	private ResourceEntity asyncEntity;

	private bool isSharing;

	public static GetPetLayer Show(PetDataEx petData, GetPetLayer.VoidCallback cb, GetPetLayer.EGPL_ShowNewsType type = GetPetLayer.EGPL_ShowNewsType.Null)
	{
		if (petData == null)
		{
			global::Debug.LogError(new object[]
			{
				"PetDataEx is null"
			});
			return null;
		}
		if (GetPetLayer.mInstance == null)
		{
			GetPetLayer.CreateInstance();
			GetPetLayer.mInstance.Init(petData.Info, cb, type);
		}
		return GetPetLayer.mInstance;
	}

	public static GetPetLayer Show(PetInfo info, GetPetLayer.VoidCallback cb, GetPetLayer.EGPL_ShowNewsType type = GetPetLayer.EGPL_ShowNewsType.Null)
	{
		if (GetPetLayer.mInstance == null)
		{
			GetPetLayer.CreateInstance();
			GetPetLayer.mInstance.Init(info, cb, type);
		}
		return GetPetLayer.mInstance;
	}

	private static void CreateInstance()
	{
		GameUIManager.mInstance.DestroyPetInfoSceneV2();
		GameObject prefab = Res.LoadGUI("GUI/GetPetLayer");
		GameObject gameObject = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, prefab);
		Vector3 localPosition = gameObject.transform.localPosition;
		localPosition.z = 2000f;
		gameObject.transform.localPosition = localPosition;
		GetPetLayer.mInstance = gameObject.GetComponent<GetPetLayer>();
	}

	public static bool TryDestroy()
	{
		if (GetPetLayer.mInstance != null)
		{
			if (GetPetLayer.mInstance.BGClickCallBack != null)
			{
				GetPetLayer.mInstance.BGClickCallBack();
			}
			UnityEngine.Object.Destroy(GetPetLayer.mInstance.gameObject);
			GetPetLayer.mInstance = null;
			return true;
		}
		return false;
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		if (this.BGAlphaCurve != null && this.BGAlphaCurve.keys.Length <= 0)
		{
			this.BGAlphaCurve = null;
		}
		if (this.FlareAlphaCurve != null && this.FlareAlphaCurve.keys.Length <= 0)
		{
			this.FlareAlphaCurve = null;
		}
		if (this.TitleScaleCurve != null && this.TitleScaleCurve.keys.Length <= 0)
		{
			this.TitleScaleCurve = null;
		}
		if (this.ColorTxtAlphaCurve != null && this.ColorTxtAlphaCurve.keys.Length <= 0)
		{
			this.ColorTxtAlphaCurve = null;
		}
		this.mPanel = base.gameObject.GetComponent<UIPanel>();
		this.mBG = GameUITools.RegisterClickEvent("BG", new UIEventListener.VoidDelegate(this.OnBGClick), base.gameObject).GetComponent<UITexture>();
		this.mNormalBG = GameUITools.RegisterClickEvent("NormalBG", new UIEventListener.VoidDelegate(this.OnBGClick), base.gameObject).GetComponent<UITexture>();
		this.mNormalBG.gameObject.SetActive(false);
		this.mUI62_1 = GameUITools.FindGameObject("ui62_1", this.mNormalBG.gameObject);
		this.mUI62_2 = GameUITools.FindGameObject("ui62_2", this.mNormalBG.gameObject);
		Tools.SetParticleRQWithUIScale(this.mUI62_1, this.mPanel.startingRenderQueue + 2);
		Tools.SetParticleRQWithUIScale(this.mUI62_2, this.mPanel.startingRenderQueue + 1000);
		this.mUI62_1.SetActive(false);
		this.mUI62_2.SetActive(false);
		this.mFlare = GameUITools.FindUISprite("Flare", base.gameObject);
		this.mUI62 = GameUITools.FindGameObject("ui62", base.gameObject);
		this.mUI62.SetActive(false);
		Tools.SetParticleRQWithUIScale(this.mUI62, this.UI62RQ);
		this.mSlot = GameUITools.FindGameObject("Slot", base.gameObject);
		this.mWindow = GameUITools.FindGameObject("Window", base.gameObject);
		this.mCon = GameUITools.FindUILabel("Con", this.mWindow);
		this.mOrange = GameUITools.FindUILabel("Orange", this.mWindow);
		this.mPurple = GameUITools.FindUILabel("Purple", this.mWindow);
		this.mTitle = GameUITools.FindGameObject("Title", this.mWindow);
		this.mName = GameUITools.FindUILabel("Name", this.mTitle);
		this.mElement = GameUITools.FindUISprite("Element", this.mTitle);
		this.mType = GameUITools.FindUISprite("Type", this.mTitle);
		this.mUI62_3 = GameUITools.FindGameObject("ui62_3", this.mTitle);
		this.mUI62_3.SetActive(false);
		Tools.SetParticleRQWithUIScale(this.mUI62_3, this.UI62RQ);
		this.mGoOn = GameUITools.FindUILabel("GoOn", this.mWindow);
		this.mShareBtn = GameUITools.RegisterClickEvent("ShareBtn", new UIEventListener.VoidDelegate(this.OnShareBtnClick), this.mWindow);
		this.mBG.enabled = false;
		this.mUI62.gameObject.SetActive(false);
		this.mSlot.gameObject.SetActive(false);
		this.mFlare.enabled = false;
		this.mTitle.SetActive(false);
		this.mCon.enabled = false;
		this.mOrange.enabled = false;
		this.mPurple.enabled = false;
		this.mGoOn.enabled = false;
		this.mShareBtn.SetActive(false);
	}

	public void Init(PetInfo info, GetPetLayer.VoidCallback cb, GetPetLayer.EGPL_ShowNewsType type = GetPetLayer.EGPL_ShowNewsType.Null)
	{
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				"PetInfo is null"
			});
			return;
		}
		this.mPetInfo = info;
		this.BGClickCallBack = (GetPetLayer.VoidCallback)Delegate.Combine(this.BGClickCallBack, cb);
		this.mShowNewsType = type;
		if (this.mPetInfo.Quality >= 2)
		{
			base.StartCoroutine(this.PlayAnim());
		}
		else
		{
			base.StartCoroutine(this.PlayRollOneAnim());
		}
	}

	private void OnDisable()
	{
		this.ClearModel();
	}

	private void ClearModel()
	{
		if (this.asyncEntity != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntity);
			this.asyncEntity = null;
		}
		if (this.mModel != null)
		{
			UnityEngine.Object.DestroyImmediate(this.mModel);
			this.mModel = null;
		}
	}

	private void ShowGameNew()
	{
		if (this.mPetInfo == null || this.mPetInfo.Quality < 3)
		{
			return;
		}
		string key = string.Empty;
		switch (this.mShowNewsType)
		{
		case GetPetLayer.EGPL_ShowNewsType.Null:
			return;
		case GetPetLayer.EGPL_ShowNewsType.HighRoll:
			key = "chatTxt20";
			break;
		case GetPetLayer.EGPL_ShowNewsType.LuckyDraw:
			key = "chatTxt23";
			break;
		case GetPetLayer.EGPL_ShowNewsType.Create:
			key = "chatTxt22";
			break;
		default:
			return;
		}
		string itemQualityColorHex = Tools.GetItemQualityColorHex(Globals.Instance.Player.GetQuality());
		string itemQualityColorHex2 = Tools.GetItemQualityColorHex(this.mPetInfo.Quality);
		string @string = Singleton<StringManager>.Instance.GetString(key, new object[]
		{
			Globals.Instance.Player.Data.Name,
			itemQualityColorHex2,
			Tools.GetPetName(this.mPetInfo),
			itemQualityColorHex
		});
		GameUIManager.mInstance.ShowGameNew(@string, 2, 110, true);
	}

	[DebuggerHidden]
	private IEnumerator PlayAnim()
	{
        return null;
        //GetPetLayer.<PlayAnim>c__Iterator74 <PlayAnim>c__Iterator = new GetPetLayer.<PlayAnim>c__Iterator74();
        //<PlayAnim>c__Iterator.<>f__this = this;
        //return <PlayAnim>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator PlayRollOneAnim()
	{
        return null;
        //GetPetLayer.<PlayRollOneAnim>c__Iterator75 <PlayRollOneAnim>c__Iterator = new GetPetLayer.<PlayRollOneAnim>c__Iterator75();
        //<PlayRollOneAnim>c__Iterator.<>f__this = this;
        //return <PlayRollOneAnim>c__Iterator;
	}

	private void OnBGClick(GameObject go)
	{
		if (!this.canClose)
		{
			return;
		}
		if (this.BGClickCallBack != null)
		{
			this.BGClickCallBack();
		}
		UnityEngine.Object.DestroyImmediate(base.gameObject);
		GetPetLayer.mInstance = null;
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
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
        //GetPetLayer.<ShowSharePopUp>c__Iterator76 <ShowSharePopUp>c__Iterator = new GetPetLayer.<ShowSharePopUp>c__Iterator76();
        //<ShowSharePopUp>c__Iterator.<>f__this = this;
        //return <ShowSharePopUp>c__Iterator;
	}
}
