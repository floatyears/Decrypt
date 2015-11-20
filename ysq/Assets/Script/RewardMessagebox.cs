using Holoville.HOTween.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class RewardMessagebox : MonoBehaviour
{
	public class RewardData
	{
		public int rewardType;

		public int rewardValue1;

		public int rewardValue2;

		public string customDesc;
	}

	public UIEventListener.VoidDelegate OnOKClickEvent;

	public static RewardMessagebox mInstance;

	private UISprite mQuestMBWin;

	private UILabel rewardLb;

	public void ShowRewardMessageBox(string title, string subTitle, List<RewardMessagebox.RewardData> Data, bool playSound = true)
	{
		int count = Data.Count;
		if (count == 0)
		{
			return;
		}
		this.mQuestMBWin = base.transform.Find("winBG").GetComponent<UISprite>();
		UIEventListener expr_43 = UIEventListener.Get(base.transform.Find("BG").gameObject);
		expr_43.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_43.onClick, new UIEventListener.VoidDelegate(this.OnCloseQuestMBClicked));
		UIEventListener expr_83 = UIEventListener.Get(this.mQuestMBWin.transform.Find("GoBtn").gameObject);
		expr_83.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_83.onClick, new UIEventListener.VoidDelegate(this.OnQuestMBOKClicked));
		UILabel component = this.mQuestMBWin.transform.FindChild("Title").GetComponent<UILabel>();
		UILabel component2 = this.mQuestMBWin.transform.FindChild("subTitle").GetComponent<UILabel>();
		this.rewardLb = this.mQuestMBWin.transform.FindChild("Sprite/Reward").GetComponent<UILabel>();
		component.text = title;
		if (string.IsNullOrEmpty(subTitle))
		{
			component2.gameObject.SetActive(false);
		}
		else
		{
			component2.gameObject.SetActive(true);
			component2.text = subTitle;
		}
		int[] array = new int[]
		{
			325,
			325,
			340,
			380
		};
		if (count <= 4)
		{
			this.RefreshRewardBox(Data);
			this.mQuestMBWin.height = array[count - 1];
		}
		else
		{
			this.RefreshMutilRewardBox(Data);
			this.mQuestMBWin.height = array[3];
		}
		base.gameObject.SetActive(true);
		if (playSound)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_009");
		}
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		GameUITools.PlayOpenWindowAnim(this.mQuestMBWin.transform, null, true);
	}

	private void RefreshRewardBox(List<RewardMessagebox.RewardData> Data)
	{
		int count = Data.Count;
		if (count == 0)
		{
			return;
		}
		float[] array = new float[]
		{
			22f,
			50f,
			75f,
			80f
		};
		float[] array2 = new float[]
		{
			0f,
			-60f,
			-45f,
			-40f
		};
		float num = array[count - 1];
		float num2 = array2[count - 1];
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			RewardMessagebox.RewardData data = Data[i];
			GameObject x = this.RefreshRewardItem(this.mQuestMBWin.transform, data, -70f, num);
			flag |= this.RefreshCustomDesc(this.mQuestMBWin.transform, data, (float)((!(x != null)) ? -72 : -172), num);
			num += num2;
		}
		this.rewardLb.gameObject.SetActive(!flag);
	}

	private void RefreshMutilRewardBox(List<RewardMessagebox.RewardData> Data)
	{
		if (Data.Count == 0)
		{
			return;
		}
		Transform transform = this.mQuestMBWin.transform.FindChild("Sprite/contents");
		transform.gameObject.SetActive(true);
		UIPanel component = transform.Find("contentsPanel").GetComponent<UIPanel>();
		UIScrollView component2 = component.transform.GetComponent<UIScrollView>();
		UITable component3 = component.transform.Find("itemsContents").GetComponent<UITable>();
		int count = Data.Count;
		float num = -20f;
		for (int i = 0; i < count; i++)
		{
			RewardMessagebox.RewardData data = Data[i];
			this.RefreshRewardItem(component3.transform, data, 0f, 0f);
			num += -40f;
		}
		component3.Reposition();
		component2.InvalidateBounds();
		component3.gameObject.AddComponent<UIDragScrollView>();
		BoxCollider boxCollider = component3.gameObject.AddComponent<BoxCollider>();
		Vector4 finalClipRegion = component.finalClipRegion;
		float y = component2.bounds.size.y;
		boxCollider.center = new Vector3(110f, -y / 2f, 0f);
		boxCollider.size = new Vector3(finalClipRegion.z, y, 0f);
	}

	private GameObject RefreshRewardItem(Transform parent, RewardMessagebox.RewardData data, float x, float y)
	{
		if (data.rewardType <= 0 || data.rewardType >= 20)
		{
			return null;
		}
		GameObject gameObject = GameUITools.CreateMinReward(data.rewardType, data.rewardValue1, data.rewardValue2, parent);
		if (gameObject != null)
		{
			if (data.rewardType == 3 || data.rewardType == 4)
			{
				gameObject.transform.localPosition = new Vector3(x, y, 0f);
			}
			else
			{
				gameObject.transform.localPosition = new Vector3(x + 2f, y, 0f);
			}
		}
		else
		{
			global::Debug.LogError(new object[]
			{
				"Create reward error."
			});
		}
		return gameObject;
	}

	private bool RefreshCustomDesc(Transform parent, RewardMessagebox.RewardData data, float x, float y)
	{
		if (!string.IsNullOrEmpty(data.customDesc))
		{
			GameObject gameObject = NGUITools.AddChild(parent.gameObject, this.rewardLb.gameObject);
			gameObject.transform.localPosition = new Vector3(x, y - 2f, 0f);
			UILabel component = gameObject.GetComponent<UILabel>();
			component.text = data.customDesc;
			return true;
		}
		return false;
	}

	public static RewardMessagebox GetInstance()
	{
		if (RewardMessagebox.mInstance == null)
		{
			GameObject prefab = Res.LoadGUI("GUI/QuestRewardMessagebox");
			GameObject gameObject = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, prefab);
			gameObject.transform.localPosition = new Vector3(0f, 0f, 1000f);
			RewardMessagebox.mInstance = gameObject.AddComponent<RewardMessagebox>();
		}
		return RewardMessagebox.mInstance;
	}

	private void OnCloseQuestMBClicked(GameObject go)
	{
		this.CloseQuestMB();
	}

	private void CloseQuestMB()
	{
		GameUITools.PlayCloseWindowAnim(this.mQuestMBWin.transform, new TweenDelegate.TweenCallback(this.OnCloseQuestMBAnimEnd), true);
	}

	private void OnCloseQuestMBAnimEnd()
	{
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}

	public void OnQuestMBOKClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.CloseQuestMB();
		if (this.OnOKClickEvent != null)
		{
			this.OnOKClickEvent(go);
			this.OnOKClickEvent = null;
		}
	}
}
