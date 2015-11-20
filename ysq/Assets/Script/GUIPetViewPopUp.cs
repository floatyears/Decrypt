using Att;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class GUIPetViewPopUp : MonoBehaviour
{
	private static GUIPetViewPopUp mInstance;

	private GameObject mWindow;

	private UILabel mTitle;

	private PetViewUITable[] mContentsTables = new PetViewUITable[5];

	private bool[] tableInit = new bool[5];

	private GUIRollingSceneV2.ERollType mType;

	public static void Show(GUIRollingSceneV2.ERollType type)
	{
		if (GUIPetViewPopUp.mInstance == null)
		{
			GUIPetViewPopUp.CreateInstance();
		}
		GUIPetViewPopUp.mInstance.Init(type);
	}

	private static void CreateInstance()
	{
		if (GUIPetViewPopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIPetViewPopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIPetViewPopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIPetViewPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 4000f);
		GUIPetViewPopUp.mInstance = gameObject2.AddComponent<GUIPetViewPopUp>();
	}

	public static bool TryClose()
	{
		if (GUIPetViewPopUp.mInstance != null)
		{
			GUIPetViewPopUp.mInstance.OnCloseClick(null);
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
		this.mWindow = GameUITools.FindGameObject("Window", base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mWindow);
		this.mTitle = GameUITools.FindUILabel("Title", this.mWindow);
		GameObject gameObject = GameUITools.FindGameObject("Tabs", this.mWindow);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			UIToggle component = gameObject.transform.GetChild(i).GetComponent<UIToggle>();
			EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
			UIEventListener expr_96 = UIEventListener.Get(component.gameObject);
			expr_96.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_96.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		}
		gameObject = GameUITools.FindGameObject("Panels", this.mWindow);
		for (int j = 0; j < gameObject.transform.childCount; j++)
		{
			this.mContentsTables[j] = GameUITools.FindGameObject("Contents", gameObject.transform.GetChild(j).gameObject).AddComponent<PetViewUITable>();
			this.mContentsTables[j].maxPerLine = 6;
			this.mContentsTables[j].arrangement = UICustomGrid.Arrangement.Vertical;
			this.mContentsTables[j].cellHeight = 100f;
			this.mContentsTables[j].cellHeight = 124f;
			this.mContentsTables[j].gapHeight = 8f;
			this.mContentsTables[j].gapWidth = 22f;
			this.mContentsTables[j].bgScrollView = GameUITools.FindGameObject("PanelBG", this.mWindow).GetComponent<UIDragScrollView>();
		}
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
	}

	public void Init(GUIRollingSceneV2.ERollType type)
	{
		this.mType = type;
		GameUITools.PlayOpenWindowAnim(this.mWindow.transform, null, true);
	}

	public void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			this.InitDatas(Convert.ToInt32(UIToggle.current.gameObject.name));
		}
	}

	private void InitDatas(int type)
	{
		this.mContentsTables[type - 1].ResetBGScrollView();
		if (this.tableInit[type - 1])
		{
			return;
		}
		this.tableInit[type - 1] = true;
		this.mContentsTables[type - 1].ClearData();
		GUIRollingSceneV2.ERollType eRollType = this.mType;
		if (eRollType != GUIRollingSceneV2.ERollType.ERollType_Low)
		{
			if (eRollType == GUIRollingSceneV2.ERollType.ERollType_high)
			{
				this.mTitle.text = Singleton<StringManager>.Instance.GetString("rollHighLabel") + Singleton<StringManager>.Instance.GetString("rollView");
				foreach (LuckyRollInfo current in Globals.Instance.AttDB.LuckyRollDict.Values)
				{
					if (current.InfoID[1] != 0 && current.InfoID[1] != 90000 && current.InfoID[1] != 90001 && current.InfoID[1] != Globals.Instance.Player.TeamSystem.GetPet(0).Info.ID)
					{
						PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(current.InfoID[1]);
						if (info == null)
						{
							global::Debug.LogErrorFormat("PetDict.GetInfo error, id = {0}", new object[]
							{
								current.InfoID[1]
							});
						}
						else if (info.ElementType == type && this.mContentsTables[type - 1].GetData((ulong)((long)info.ID)) == null)
						{
							this.mContentsTables[type - 1].AddData(new PetViewItemData(info));
						}
					}
					if (current.Pet[0] != 0 && current.Pet[0] != 90000 && current.Pet[0] != 90001 && current.Pet[0] != Globals.Instance.Player.TeamSystem.GetPet(0).Info.ID)
					{
						PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(current.Pet[0]);
						if (info == null)
						{
							global::Debug.LogErrorFormat("PetDict.GetInfo error, id = {0}", new object[]
							{
								current.Pet[0]
							});
						}
						else if (info.ElementType == type && this.mContentsTables[type - 1].GetData((ulong)((long)info.ID)) == null)
						{
							this.mContentsTables[type - 1].AddData(new PetViewItemData(info));
						}
					}
					if (current.Pet[1] != 0 && current.Pet[1] != 90000 && current.Pet[1] != 90001 && current.Pet[1] != Globals.Instance.Player.TeamSystem.GetPet(0).Info.ID)
					{
						PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(current.Pet[1]);
						if (info == null)
						{
							global::Debug.LogErrorFormat("PetDict.GetInfo error, id = {0}", new object[]
							{
								current.Pet[1]
							});
						}
						else if (info.ElementType == type && this.mContentsTables[type - 1].GetData((ulong)((long)info.ID)) == null)
						{
							this.mContentsTables[type - 1].AddData(new PetViewItemData(info));
						}
					}
				}
			}
		}
		else
		{
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("rollLowLabel") + Singleton<StringManager>.Instance.GetString("rollView");
			foreach (LuckyRollInfo current2 in Globals.Instance.AttDB.LuckyRollDict.Values)
			{
				if (current2.InfoID[0] != 0 && current2.InfoID[0] != 90000 && current2.InfoID[0] != 90001 && current2.InfoID[0] != Globals.Instance.Player.TeamSystem.GetPet(0).Info.ID)
				{
					PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(current2.InfoID[0]);
					if (info == null)
					{
						global::Debug.LogErrorFormat("PetDict.GetInfo error, id = {0}", new object[]
						{
							current2.InfoID[0]
						});
					}
					else if (info.ElementType == type && this.mContentsTables[type - 1].GetData((ulong)((long)info.ID)) == null)
					{
						this.mContentsTables[type - 1].AddData(new PetViewItemData(info));
					}
				}
			}
		}
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.mWindow.transform, new TweenDelegate.TweenCallback(this.CloseImmediate), true);
	}

	private void CloseImmediate()
	{
		UnityEngine.Object.Destroy(GUIPetViewPopUp.mInstance.gameObject);
		GUIPetViewPopUp.mInstance = null;
	}
}
