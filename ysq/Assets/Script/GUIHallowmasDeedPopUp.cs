using Holoville.HOTween.Core;
using Proto;
using System;
using UnityEngine;

public class GUIHallowmasDeedPopUp : MonoBehaviour
{
	private static GUIHallowmasDeedPopUp mInstance;

	private GameObject mWindow;

	private MyDeedLayer mMyDeedLayer;

	private LuckydeedLayer mLuckydeedLayer;

	private UIToggle[] tabPage = new UIToggle[2];

	private GameObject[] newFlag = new GameObject[2];

	public static int curSelectTab;

	public static int CurSelectTab
	{
		get
		{
			return GUIHallowmasDeedPopUp.curSelectTab;
		}
		set
		{
			GUIHallowmasDeedPopUp.curSelectTab = value;
		}
	}

	public static void Show(int index)
	{
		if (GUIHallowmasDeedPopUp.mInstance == null)
		{
			GUIHallowmasDeedPopUp.CurSelectTab = index;
			GUIHallowmasDeedPopUp.CreateInstance();
		}
		GUIHallowmasDeedPopUp.mInstance.Init();
	}

	public static bool TryClose()
	{
		if (GUIHallowmasDeedPopUp.mInstance != null)
		{
			GUIHallowmasDeedPopUp.mInstance.OnCloseClick(null);
			return true;
		}
		return false;
	}

	private static void CreateInstance()
	{
		if (GUIHallowmasDeedPopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIHallowmasDeedPopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIHallowmasDeedPopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIHallowmasDeedPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 4000f);
		GUIHallowmasDeedPopUp.mInstance = gameObject2.AddComponent<GUIHallowmasDeedPopUp>();
		GUIHallowmasDeedPopUp.mInstance.gameObject.SetActive(true);
		MC2S_ActivityHalloweenContract ojb = new MC2S_ActivityHalloweenContract();
		Globals.Instance.CliSession.Send(785, ojb);
	}

	private void Awake()
	{
		this.CreateObjects();
		ActivitySubSystem expr_15 = Globals.Instance.Player.ActivitySystem;
		expr_15.ActivityHalloweenEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_15.ActivityHalloweenEvent, new ActivitySubSystem.VoidCallback(this.OnActivityHalloweenUpdataEvent));
		ActivitySubSystem expr_45 = Globals.Instance.Player.ActivitySystem;
		expr_45.GetHalloweenDataEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_45.GetHalloweenDataEvent, new ActivitySubSystem.VoidCallback(this.OnGetHalloweenDataEvent));
		ActivitySubSystem expr_75 = Globals.Instance.Player.ActivitySystem;
		expr_75.GetConDataEvent = (ActivitySubSystem.VoidCallback)Delegate.Combine(expr_75.GetConDataEvent, new ActivitySubSystem.VoidCallback(this.OnGetConDataEvent));
	}

	private void CreateObjects()
	{
		this.mWindow = GameUITools.FindGameObject("Window", base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mWindow);
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnFadeBGClick), base.gameObject);
		for (int i = 0; i < 2; i++)
		{
			this.tabPage[i] = this.mWindow.transform.Find(string.Format("tab{0}", i)).GetComponent<UIToggle>();
			EventDelegate.Add(this.tabPage[i].onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
			UIEventListener expr_B5 = UIEventListener.Get(this.tabPage[i].gameObject);
			expr_B5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B5.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
			this.newFlag[i] = this.tabPage[i].transform.Find("new").gameObject;
			this.newFlag[i].SetActive(false);
		}
		this.mMyDeedLayer = this.mWindow.transform.Find("myDeedContent").gameObject.AddComponent<MyDeedLayer>();
		this.mMyDeedLayer.Init();
		this.mLuckydeedLayer = this.mWindow.transform.Find("luckyDeedContent").gameObject.AddComponent<LuckydeedLayer>();
		this.mLuckydeedLayer.Init();
		if (GUIHallowmasDeedPopUp.curSelectTab == 0)
		{
			this.tabPage[0].value = true;
		}
		else
		{
			this.tabPage[1].value = true;
		}
	}

	private void OnDestroy()
	{
		ActivitySubSystem expr_0F = Globals.Instance.Player.ActivitySystem;
		expr_0F.ActivityHalloweenEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_0F.ActivityHalloweenEvent, new ActivitySubSystem.VoidCallback(this.OnActivityHalloweenUpdataEvent));
		ActivitySubSystem expr_3F = Globals.Instance.Player.ActivitySystem;
		expr_3F.GetHalloweenDataEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_3F.GetHalloweenDataEvent, new ActivitySubSystem.VoidCallback(this.OnGetHalloweenDataEvent));
		ActivitySubSystem expr_6F = Globals.Instance.Player.ActivitySystem;
		expr_6F.GetConDataEvent = (ActivitySubSystem.VoidCallback)Delegate.Remove(expr_6F.GetConDataEvent, new ActivitySubSystem.VoidCallback(this.OnGetConDataEvent));
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		for (int i = 0; i < 2; i++)
		{
			if (this.tabPage[i] == go)
			{
				GUIHallowmasDeedPopUp.curSelectTab = i;
			}
		}
	}

	private void OnTabCheckChanged()
	{
		if (!UIToggle.current.value)
		{
			return;
		}
		if (UIToggle.current.value)
		{
			if (UIToggle.current == this.tabPage[0])
			{
				this.mMyDeedLayer.RefreshLayer();
			}
			else if (UIToggle.current == this.tabPage[1])
			{
				this.mLuckydeedLayer.RefreshLayer();
			}
		}
	}

	public void Init()
	{
		GameUITools.PlayOpenWindowAnim(this.mWindow.transform, null, true);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.PlayCloseAnim();
	}

	private void CloseAll()
	{
		this.CloseImmediate();
	}

	private void CloseImmediate()
	{
		UnityEngine.Object.Destroy(GUIHallowmasDeedPopUp.mInstance.gameObject);
	}

	private void PlayCloseAnim()
	{
		GameUITools.PlayCloseWindowAnim(this.mWindow.transform, new TweenDelegate.TweenCallback(this.CloseImmediate), true);
	}

	private void OnFadeBGClick(GameObject go)
	{
		this.PlayCloseAnim();
	}

	private void OnActivityHalloweenUpdataEvent()
	{
		if (GUIHallowmasDeedPopUp.CurSelectTab == 0)
		{
			this.mMyDeedLayer.RefreshLayer();
		}
		else if (GUIHallowmasDeedPopUp.CurSelectTab == 1)
		{
			this.mLuckydeedLayer.RefreshLayer();
		}
	}

	private void OnGetHalloweenDataEvent()
	{
		if (GUIHallowmasDeedPopUp.CurSelectTab == 0)
		{
			this.mMyDeedLayer.RefreshLayer();
		}
		else if (GUIHallowmasDeedPopUp.CurSelectTab == 1)
		{
			this.mLuckydeedLayer.RefreshLayer();
		}
	}

	private void OnGetConDataEvent()
	{
		if (GUIHallowmasDeedPopUp.CurSelectTab == 0)
		{
			this.mMyDeedLayer.AddData();
		}
		else if (GUIHallowmasDeedPopUp.CurSelectTab == 1)
		{
			this.mLuckydeedLayer.AddData();
		}
	}
}
