using Att;
using System;
using System.Text;
using UnityEngine;

public class GUILopetInfoScene : MonoBehaviour
{
	public enum EType
	{
		EType_Info,
		EType_Fragment
	}

	private static GUILopetInfoScene mInstance;

	private GUILopetTitleInfo mGUIPetTitleInfo;

	private GameObject mCardModel;

	private GameObject mModelTmp;

	private UIActorController mUIActorController;

	private GameObject mState1Go;

	private StringBuilder mSb = new StringBuilder();

	private ResourceEntity asyncEntiry;

	public static void Show(LopetDataEx lopetData, GUILopetInfoScene.EType type = GUILopetInfoScene.EType.EType_Info)
	{
		if (lopetData == null)
		{
			return;
		}
		if (GUILopetInfoScene.mInstance == null)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/GUILopetInfoScene");
			if (@object == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/GUILopetInfoScene error"
				});
				return;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = GameUIManager.mInstance.uiCamera.gameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 3000f);
			gameObject.transform.localScale = Vector3.one;
			GUILopetInfoScene.mInstance = gameObject.AddComponent<GUILopetInfoScene>();
		}
		GUILopetInfoScene.mInstance.Init(lopetData, type);
	}

	public static bool TryClose()
	{
		if (GUILopetInfoScene.mInstance != null)
		{
			UnityEngine.Object.Destroy(GUILopetInfoScene.mInstance.gameObject);
			GUILopetInfoScene.mInstance = null;
			return true;
		}
		return false;
	}

	private void CreateObjects()
	{
	}

	private void Awake()
	{
		this.CreateObjects();
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		this.ClearModel();
	}

	private void Init(LopetDataEx lopetData, GUILopetInfoScene.EType type)
	{
		Transform transform = base.transform.Find("UIMiddle");
		this.mCardModel = transform.Find("modelPos").gameObject;
		this.mGUIPetTitleInfo = transform.Find("topInfoPanel/lopetTopInfo").gameObject.AddComponent<GUILopetTitleInfo>();
		this.mGUIPetTitleInfo.Refresh(lopetData);
		GameObject gameObject = transform.Find("closeBtn").gameObject;
		UIEventListener expr_65 = UIEventListener.Get(gameObject);
		expr_65.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_65.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		UITable component = GameUITools.FindGameObject("rightInfo/rightInfoPanel/content", transform.gameObject).GetComponent<UITable>();
		GameUITools.FindGameObject("a", component.gameObject).AddComponent<GUIAttributeValue>().Refresh(lopetData);
		LopetInfoSkillLayer lopetInfoSkillLayer = GameUITools.FindGameObject("b", component.gameObject).AddComponent<LopetInfoSkillLayer>();
		lopetInfoSkillLayer.Init();
		lopetInfoSkillLayer.Refresh(lopetData);
		UISprite component2 = GameUITools.FindGameObject("e", component.gameObject).GetComponent<UISprite>();
		UILabel uILabel = GameUITools.FindUILabel("desc", component2.gameObject);
		uILabel.text = lopetData.Info.Desc;
		component2.height = 50 + Mathf.RoundToInt(uILabel.printedSize.y);
		component.repositionNow = true;
		SourceItemUITable sourceItemUITable = GameUITools.FindGameObject("rightInfo/rightInfoPanel2/content", transform.gameObject).AddComponent<SourceItemUITable>();
		UILabel uILabel2 = GameUITools.FindUILabel("rightInfo/rightInfoPanel2/txt0", transform.gameObject);
		sourceItemUITable.maxPerLine = 1;
		sourceItemUITable.arrangement = UICustomGrid.Arrangement.Vertical;
		sourceItemUITable.cellWidth = 360f;
		sourceItemUITable.cellHeight = 76f;
		sourceItemUITable.gapHeight = 4f;
		sourceItemUITable.gapWidth = 0f;
		sourceItemUITable.ClearData();
		ItemInfo fragmentInfo = LopetFragment.GetFragmentInfo(lopetData.Info.ID);
		GUIHowGetPetItemPopUp.InitSourceItems(fragmentInfo, sourceItemUITable);
		if (sourceItemUITable.mDatas.Count > 0)
		{
			uILabel2.gameObject.SetActive(false);
		}
		else
		{
			uILabel2.gameObject.SetActive(true);
		}
		UIEventListener expr_210 = UIEventListener.Get(GameUITools.FindGameObject("rightInfo/Tabs/0", transform.gameObject));
		expr_210.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_210.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		UIEventListener expr_246 = UIEventListener.Get(GameUITools.FindGameObject("rightInfo/Tabs/1", transform.gameObject));
		expr_246.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_246.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mState1Go = transform.Find("state1").gameObject;
		if (type != GUILopetInfoScene.EType.EType_Info)
		{
			if (type == GUILopetInfoScene.EType.EType_Fragment)
			{
				this.mState1Go.gameObject.SetActive(true);
			}
		}
		else
		{
			this.mState1Go.gameObject.SetActive(false);
		}
		Transform transform2 = this.mState1Go.transform;
		UISlider component3 = transform2.Find("expBar").GetComponent<UISlider>();
		UILabel component4 = component3.transform.Find("num").GetComponent<UILabel>();
		int itemCount = Globals.Instance.Player.ItemSystem.GetItemCount(fragmentInfo.ID);
		component4.text = this.mSb.Remove(0, this.mSb.Length).Append(itemCount).Append("/").Append(fragmentInfo.Value1).ToString();
		component3.value = ((fragmentInfo.Value1 == 0) ? 0f : Mathf.Clamp01((float)itemCount / (float)fragmentInfo.Value1));
		this.ClearModel();
		this.asyncEntiry = ActorManager.CreateUILopet(lopetData.Info, 450, true, true, this.mCardModel, 1f, delegate(GameObject go)
		{
			this.asyncEntiry = null;
			this.mModelTmp = go;
			if (this.mModelTmp != null)
			{
				this.mUIActorController = this.mModelTmp.GetComponent<UIActorController>();
				if (this.mUIActorController != null)
				{
					this.mUIActorController.PlayIdleAnimationAndVoice();
				}
				Tools.SetMeshRenderQueue(this.mModelTmp, 5390);
			}
		});
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GUILopetInfoScene.TryClose();
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
}
