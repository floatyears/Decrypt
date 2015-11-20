using Holoville.HOTween.Core;
using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsBox : MonoBehaviour
{
	private static ItemsBox mInstance;

	private GameObject mWindow;

	private ItemsBoxUITable mContent;

	public bool ShowNum
	{
		get;
		private set;
	}

	public static void Show(List<RewardData> rewards, string title = null, bool showNum = false)
	{
		if (rewards == null)
		{
			global::Debug.LogError(new object[]
			{
				"rewards is null"
			});
			return;
		}
		if (rewards.Count == 0)
		{
			global::Debug.LogError(new object[]
			{
				"rewards count == 0"
			});
			return;
		}
		GameObject prefab = Res.LoadGUI("GUI/ItemsBox");
		GameObject gameObject = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, prefab);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 4000f);
		if (ItemsBox.mInstance != null)
		{
			ItemsBox.TryClose();
		}
		ItemsBox.mInstance = gameObject.AddComponent<ItemsBox>();
		if (string.IsNullOrEmpty(title))
		{
			title = Singleton<StringManager>.Instance.GetString("showRewardLb");
		}
		else
		{
			title = Singleton<StringManager>.Instance.GetString(title);
		}
		ItemsBox.mInstance.Init(rewards, title, showNum);
	}

	public static bool TryClose()
	{
		if (ItemsBox.mInstance != null)
		{
			ItemsBox.mInstance.Close();
			ItemsBox.mInstance = null;
			return true;
		}
		return false;
	}

	private void Init(List<RewardData> repeatableRewards, string title, bool showNum)
	{
		if (repeatableRewards == null || repeatableRewards.Count == 0)
		{
			return;
		}
		List<RewardData> list = GUIRewardPanel.CombineSameRewardData(repeatableRewards);
		if (list.Count == 0)
		{
			return;
		}
		this.ShowNum = showNum;
		this.mWindow = GameUITools.FindGameObject("Window", base.gameObject);
		this.mContent = GameUITools.FindGameObject("Panel/Content", this.mWindow).AddComponent<ItemsBoxUITable>();
		this.mContent.maxPerLine = 4;
		this.mContent.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContent.cellWidth = 80f;
		this.mContent.cellHeight = 114f;
		this.mContent.gapHeight = 4f;
		this.mContent.gapWidth = 10f;
		this.mContent.bgScrollView = Tools.GetSafeComponent<UIDragScrollView>(GameUITools.FindGameObject("PanelBG", this.mWindow));
		this.mContent.Init(this);
		foreach (RewardData current in list)
		{
			this.mContent.AddData(new ItemsBoxItemData(current));
		}
		GameUITools.FindUILabel("Title", this.mWindow.gameObject).text = title;
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mWindow.gameObject);
		GameUITools.RegisterClickEvent("OK", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mWindow.gameObject);
		GameUITools.PlayOpenWindowAnim(this.mWindow.transform, null, true);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.mWindow.transform, new TweenDelegate.TweenCallback(this.Close), true);
	}

	private void Close()
	{
		UnityEngine.Object.Destroy(ItemsBox.mInstance.gameObject);
	}
}
