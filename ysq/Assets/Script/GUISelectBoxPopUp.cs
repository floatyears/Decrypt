using Holoville.HOTween.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectBoxPopUp : MonoBehaviour
{
	private class SelectItem : MonoBehaviour
	{
		private GUISelectBoxPopUp mBasePop;

		public int index;

		public void Init(GUISelectBoxPopUp basepop, int petInfoID, int index)
		{
			this.mBasePop = basepop;
			GameUITools.CreateReward(4, petInfoID, 1, base.transform, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
			this.index = index;
			UIToggle component = GameUITools.FindGameObject("BG", base.gameObject).GetComponent<UIToggle>();
			UIEventListener expr_5F = UIEventListener.Get(component.gameObject);
			expr_5F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_5F.onClick, new UIEventListener.VoidDelegate(this.OnBGClick));
			EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnChange));
		}

		private void OnChange()
		{
			if (UIToggle.current.value)
			{
				this.mBasePop.index = this.index;
			}
		}

		private void OnBGClick(GameObject go)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		}

		public void SetPos(int x)
		{
			Vector3 localPosition = base.transform.localPosition;
			localPosition.x = (float)x;
			base.transform.localPosition = localPosition;
		}
	}

	private static GUISelectBoxPopUp mInstance;

	public GUIMultiUsePopUp.UseItemCallBack UseItemEvent;

	private GameObject mWindow;

	private ItemDataEx mData;

	private List<GUISelectBoxPopUp.SelectItem> mItems = new List<GUISelectBoxPopUp.SelectItem>();

	public int index = -1;

	public static void Show(ItemDataEx data, GUIMultiUsePopUp.UseItemCallBack cb)
	{
		if (data == null || data.GetCount() <= 0)
		{
			return;
		}
		if (data.Info.Type != 2 || data.Info.SubType != 7)
		{
			global::Debug.LogErrorFormat("Data info type or subtype error , type : {0} , subtype : {1} ", new object[]
			{
				data.Info.Type,
				data.Info.SubType
			});
			return;
		}
		if (GUISelectBoxPopUp.mInstance == null)
		{
			GUISelectBoxPopUp.CreateInstance();
		}
		GUISelectBoxPopUp.mInstance.Init(data, cb);
	}

	private static void CreateInstance()
	{
		if (GUISelectBoxPopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUISelectBoxPopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUISelectBoxPopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUISelectBoxPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 3000f);
		GUISelectBoxPopUp.mInstance = gameObject2.AddComponent<GUISelectBoxPopUp>();
	}

	public static void TryClose()
	{
		if (GUISelectBoxPopUp.mInstance != null)
		{
			GUISelectBoxPopUp.mInstance.Close();
		}
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mWindow = GameUITools.FindGameObject("Window", base.gameObject);
		GameUITools.RegisterClickEvent("OK", new UIEventListener.VoidDelegate(this.OnOKClick), this.mWindow);
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
	}

	private void OnOKClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.index == -1)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("selectBoxerr0", 0f, 0f);
			return;
		}
		if (this.UseItemEvent != null)
		{
			this.UseItemEvent(this.mData, this.index);
		}
	}

	public void Init(ItemDataEx data, GUIMultiUsePopUp.UseItemCallBack cb)
	{
		if (data == null || data.GetCount() < 0)
		{
			return;
		}
		this.mData = data;
		this.UseItemEvent = cb;
		List<int> list = new List<int>();
		if (data.Info.Value1 != 0)
		{
			list.Add(data.Info.Value1);
		}
		if (data.Info.Value2 != 0)
		{
			list.Add(data.Info.Value2);
		}
		if (data.Info.Value3 != 0)
		{
			list.Add(data.Info.Value3);
		}
		if (data.Info.Value4 != 0)
		{
			list.Add(data.Info.Value4);
		}
		if (list.Count <= 1 || list.Count > 4)
		{
			global::Debug.LogErrorFormat("selectbox itemIDs count error , count : {0} , InfoID : {1} ", new object[]
			{
				list.Count,
				data.Info.ID
			});
			this.Close();
			return;
		}
		Transform transform = GameUITools.FindGameObject("Items", this.mWindow).transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			this.mItems.Add(transform.GetChild(i).gameObject.AddComponent<GUISelectBoxPopUp.SelectItem>());
		}
		if (this.mItems.Count <= 1 || this.mItems.Count > 4)
		{
			global::Debug.LogErrorFormat("selectbox items count error , count : {0}", new object[]
			{
				this.mItems.Count
			});
			this.Close();
			return;
		}
		switch (list.Count)
		{
		case 2:
			this.mItems[0].SetPos(-58);
			this.mItems[1].SetPos(58);
			break;
		case 3:
			this.mItems[0].SetPos(-130);
			this.mItems[1].SetPos(0);
			this.mItems[2].SetPos(130);
			break;
		case 4:
			this.mItems[0].SetPos(-168);
			this.mItems[1].SetPos(-58);
			this.mItems[2].SetPos(58);
			this.mItems[3].SetPos(168);
			break;
		}
		int j = 0;
		while (j < list.Count && j < this.mItems.Count && j < 4)
		{
			this.mItems[j].Init(this, list[j], j);
			j++;
		}
		while (j < this.mItems.Count)
		{
			this.mItems[j].gameObject.SetActive(false);
			j++;
		}
		GameUITools.PlayOpenWindowAnim(this.mWindow.transform, null, true);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.Close();
	}

	private void Close()
	{
		GameUITools.PlayCloseWindowAnim(this.mWindow.transform, new TweenDelegate.TweenCallback(this.CloseImmediate), true);
	}

	private void CloseImmediate()
	{
		UnityEngine.Object.Destroy(GUISelectBoxPopUp.mInstance.gameObject);
		GUISelectBoxPopUp.mInstance = null;
	}
}
