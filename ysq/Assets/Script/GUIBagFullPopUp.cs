using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class GUIBagFullPopUp : MonoBehaviour
{
	public enum EBagType
	{
		EBT_Pet,
		EBT_Equip,
		EBT_Trinket,
		EBT_Lopet
	}

	private static GUIBagFullPopUp mInstance;

	private GUIBagFullPopUp.EBagType mCurType;

	private GameObject mWindow;

	private UILabel mContentTxt;

	private UILabel mSaleTxt;

	private UILabel mBreakTxt;

	public static void Show(GUIBagFullPopUp.EBagType type)
	{
		if (GUIBagFullPopUp.mInstance == null)
		{
			GUIBagFullPopUp.CreateInstance();
		}
		GUIBagFullPopUp.mInstance.Init(type);
	}

	private static void CreateInstance()
	{
		if (GUIBagFullPopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIBagFullPopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIBagFullPopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIBagFullPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 3000f);
		GUIBagFullPopUp.mInstance = gameObject2.AddComponent<GUIBagFullPopUp>();
	}

	public static bool TryClose()
	{
		if (GUIBagFullPopUp.mInstance != null)
		{
			GUIBagFullPopUp.mInstance.OnCloseClick(null);
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
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		this.mWindow = GameUITools.FindGameObject("Window", base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mWindow);
		this.mContentTxt = GameUITools.FindUILabel("Content/Txt", this.mWindow);
		this.mSaleTxt = GameUITools.FindUILabel("Txt", GameUITools.RegisterClickEvent("Sale", new UIEventListener.VoidDelegate(this.OnSaleBtnClick), this.mWindow));
		this.mBreakTxt = GameUITools.FindUILabel("Txt", GameUITools.RegisterClickEvent("Break", new UIEventListener.VoidDelegate(this.OnBreakBtnClick), this.mWindow));
	}

	public void Init(GUIBagFullPopUp.EBagType type)
	{
		this.mCurType = type;
		switch (this.mCurType)
		{
		case GUIBagFullPopUp.EBagType.EBT_Pet:
		{
			string @string = Singleton<StringManager>.Instance.GetString("summonLb");
			this.mContentTxt.text = Singleton<StringManager>.Instance.GetString("bag0", new object[]
			{
				@string
			});
			this.mSaleTxt.text = Singleton<StringManager>.Instance.GetString("bag3", new object[]
			{
				@string
			});
			this.mBreakTxt.text = Singleton<StringManager>.Instance.GetString("bag2", new object[]
			{
				@string
			});
			break;
		}
		case GUIBagFullPopUp.EBagType.EBT_Equip:
		{
			string @string = Singleton<StringManager>.Instance.GetString("equipLb");
			this.mContentTxt.text = Singleton<StringManager>.Instance.GetString("bag0", new object[]
			{
				@string
			});
			this.mSaleTxt.text = Singleton<StringManager>.Instance.GetString("bag1", new object[]
			{
				@string
			});
			this.mBreakTxt.text = Singleton<StringManager>.Instance.GetString("bag2", new object[]
			{
				@string
			});
			break;
		}
		case GUIBagFullPopUp.EBagType.EBT_Trinket:
		{
			string @string = Singleton<StringManager>.Instance.GetString("shengQiLb");
			this.mContentTxt.text = Singleton<StringManager>.Instance.GetString("bag0", new object[]
			{
				@string
			});
			this.mSaleTxt.text = Singleton<StringManager>.Instance.GetString("bag4", new object[]
			{
				@string
			});
			this.mBreakTxt.text = Singleton<StringManager>.Instance.GetString("bag5", new object[]
			{
				@string
			});
			break;
		}
		case GUIBagFullPopUp.EBagType.EBT_Lopet:
		{
			string @string = Singleton<StringManager>.Instance.GetString("LopetLb");
			this.mContentTxt.text = Singleton<StringManager>.Instance.GetString("bag0", new object[]
			{
				@string
			});
			this.mSaleTxt.text = Singleton<StringManager>.Instance.GetString("bag6", new object[]
			{
				@string
			});
			this.mBreakTxt.text = Singleton<StringManager>.Instance.GetString("bag2", new object[]
			{
				@string
			});
			break;
		}
		}
		base.gameObject.SetActive(true);
		GameUITools.PlayOpenWindowAnim(this.mWindow.transform, null, true);
	}

	private void OnSaleBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		switch (this.mCurType)
		{
		case GUIBagFullPopUp.EBagType.EBT_Pet:
		{
			GUIPartnerManageScene session = GameUIManager.mInstance.GetSession<GUIPartnerManageScene>();
			if (session != null)
			{
				session.mTab0.value = true;
			}
			else
			{
				GameUIManager.mInstance.ChangeSession<GUIPartnerManageScene>(null, false, true);
			}
			break;
		}
		case GUIBagFullPopUp.EBagType.EBT_Equip:
		{
			GUIEquipBagScene session2 = GameUIManager.mInstance.GetSession<GUIEquipBagScene>();
			if (session2 != null)
			{
				session2.mEquipTab.value = true;
			}
			else
			{
				GameUIManager.mInstance.ChangeSession<GUIEquipBagScene>(null, false, true);
			}
			break;
		}
		case GUIBagFullPopUp.EBagType.EBT_Trinket:
			GameUIManager.mInstance.ChangeSession<GUITrinketBagScene>(null, false, true);
			break;
		case GUIBagFullPopUp.EBagType.EBT_Lopet:
		{
			GUILopetBagScene session3 = GameUIManager.mInstance.GetSession<GUILopetBagScene>();
			if (session3 != null)
			{
				session3.mPetTab.value = true;
			}
			else
			{
				GUILopetBagScene.TryOpen();
			}
			break;
		}
		}
		this.CloseImmediate();
	}

	private void OnBreakBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		switch (this.mCurType)
		{
		case GUIBagFullPopUp.EBagType.EBT_Pet:
		{
			GUIRecycleScene session = GameUIManager.mInstance.GetSession<GUIRecycleScene>();
			if (session != null)
			{
				session.mPetBreakTab.value = true;
			}
			else
			{
				GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_PetBreak);
			}
			break;
		}
		case GUIBagFullPopUp.EBagType.EBT_Equip:
			GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_EquipBreak);
			break;
		case GUIBagFullPopUp.EBagType.EBT_Trinket:
			GameUIManager.mInstance.ChangeSession<GUITrinketBagScene>(null, false, true);
			break;
		case GUIBagFullPopUp.EBagType.EBT_Lopet:
		{
			GUIRecycleScene session2 = GameUIManager.mInstance.GetSession<GUIRecycleScene>();
			if (session2 != null)
			{
				session2.mLopetBreakTab.value = true;
			}
			else
			{
				GUIRecycleScene.Change2This(GUIRecycleScene.ERecycleT.ERecycleT_LopetBreak);
			}
			break;
		}
		}
		this.CloseImmediate();
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.mWindow.transform, new TweenDelegate.TweenCallback(this.CloseImmediate), true);
	}

	private void CloseImmediate()
	{
		UnityEngine.Object.Destroy(GUIBagFullPopUp.mInstance.gameObject);
	}
}
