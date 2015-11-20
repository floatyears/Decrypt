using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GUIRewardPanel : MonoBehaviour
{
	public enum EAnimType
	{
		EAT_Null,
		EAT_Double,
		EAT_Give
	}

	public delegate void VoidCallback();

	private static GameObject prefab = null;

	private static List<GUIRewardPanel> panelList = new List<GUIRewardPanel>();

	public GUIRewardPanel.VoidCallback OnClickEvent;

	[NonSerialized]
	public float time0 = 0.42f;

	[NonSerialized]
	public float time1 = 1.3f;

	[NonSerialized]
	public float time2 = 0.42f;

	private Sequence anim;

	private UIPanel mPanel;

	private GameObject mGoOn;

	private GameObject mFadeBG;

	private UILabel mTitle;

	private GameObject mUI87;

	private UISprite mTitleIcon;

	private Animation mTitleIconAnim;

	private float oneRowY = 43f;

	private float animOneRowY = 25f;

	private GUIRewardPanel.EAnimType mAnimType;

	public static GUIRewardPanel Show(List<OpenLootData> rewards, List<int> petInfoIDs, string title = null, bool needClick = false, bool combine = true, GUIRewardPanel.VoidCallback cb = null, bool showPetAnim = false)
	{
		if (rewards == null && petInfoIDs == null)
		{
			global::Debug.LogError(new object[]
			{
				"rewards and petInfoIDs is null"
			});
			return null;
		}
		if (rewards.Count == 0 && petInfoIDs.Count == 0)
		{
			global::Debug.LogError(new object[]
			{
				"rewards count == 0 && petInfoIDs count == 0"
			});
			return null;
		}
		List<RewardData> list = new List<RewardData>();
		foreach (OpenLootData current in rewards)
		{
			list.Add(new RewardData
			{
				RewardType = 3,
				RewardValue1 = current.InfoID,
				RewardValue2 = (int)current.Count
			});
		}
		foreach (int current2 in petInfoIDs)
		{
			if (Globals.Instance.AttDB.PetDict.GetInfo(current2) == null)
			{
				global::Debug.LogErrorFormat("PetDict.GetInfo error, {0}", new object[]
				{
					current2
				});
			}
			else
			{
				list.Add(new RewardData
				{
					RewardType = 4,
					RewardValue1 = current2
				});
			}
		}
		return GUIRewardPanel.Show(list, title, needClick, combine, cb, showPetAnim);
	}

	public static GUIRewardPanel Show(RewardData reward, string title = null, bool needClick = false, bool combine = true, GUIRewardPanel.VoidCallback cb = null, bool showPetAnim = false)
	{
		if (reward == null)
		{
			global::Debug.LogError(new object[]
			{
				"reward is null"
			});
			return null;
		}
		return GUIRewardPanel.Show(new List<RewardData>
		{
			reward
		}, title, needClick, combine, cb, showPetAnim);
	}

	public static GUIRewardPanel Show(List<RewardData> rewards, string title = null, bool needClick = false, bool combine = true, GUIRewardPanel.VoidCallback cb = null, bool showPetAnim = false)
	{
		if (rewards == null)
		{
			global::Debug.LogError(new object[]
			{
				"rewards is null"
			});
			return null;
		}
		if (rewards.Count == 0)
		{
			global::Debug.LogError(new object[]
			{
				"rewards count == 0"
			});
			return null;
		}
		if (GUIRewardPanel.prefab == null)
		{
			GUIRewardPanel.prefab = Res.LoadGUI("GUI/GUIRewardPanel");
		}
		GameObject gameObject = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, GUIRewardPanel.prefab);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 2800f);
		GUIRewardPanel gUIRewardPanel = gameObject.AddComponent<GUIRewardPanel>();
		if (string.IsNullOrEmpty(title))
		{
			title = Singleton<StringManager>.Instance.GetString("getRewardLb");
		}
		gUIRewardPanel.InitPanel(rewards, title, needClick, combine, cb, showPetAnim);
		return gUIRewardPanel;
	}

	public static void CloseAll()
	{
		while (GUIRewardPanel.panelList.Count > 0)
		{
			GUIRewardPanel.panelList[0].ImmediateClose();
		}
	}

	public static List<RewardData> CombineSameRewardData(List<RewardData> repeatableRewards)
	{
		List<RewardData> list = new List<RewardData>();
		bool flag = false;
		foreach (RewardData current in repeatableRewards)
		{
			switch (current.RewardType)
			{
			case 1:
			case 2:
			case 5:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 13:
			case 14:
			case 15:
			case 17:
			case 18:
				flag = false;
				foreach (RewardData current2 in list)
				{
					if (current2.RewardType == current.RewardType)
					{
						current2.RewardValue1 += current.RewardValue1;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(current);
				}
				break;
			case 3:
			case 4:
			case 12:
			case 16:
				flag = false;
				foreach (RewardData current3 in list)
				{
					if (current3.RewardType == current.RewardType && current3.RewardValue1 == current.RewardValue1)
					{
						current3.RewardValue2 += current.RewardValue2;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(current);
				}
				break;
			}
		}
		return list;
	}

	private void InitPanel(List<RewardData> repeatableRewards, string title, bool needClick, bool combine, GUIRewardPanel.VoidCallback cb, bool showPetAnim)
	{
		if (repeatableRewards == null || repeatableRewards.Count == 0)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_009");
		this.OnClickEvent = cb;
		List<RewardData> list;
		if (combine)
		{
			list = GUIRewardPanel.CombineSameRewardData(repeatableRewards);
		}
		else
		{
			list = repeatableRewards;
		}
		if (list.Count == 0)
		{
			this.ImmediateClose();
			return;
		}
		this.mPanel = base.GetComponent<UIPanel>();
		UISprite uISprite = GameUITools.FindUISprite("BG", base.gameObject);
		this.mTitle = GameUITools.FindUILabel("Title", base.gameObject);
		this.mGoOn = GameUITools.FindGameObject("GoOn", base.gameObject);
		this.mGoOn.gameObject.SetActive(false);
		this.mFadeBG = GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnFadeBGClick), base.gameObject);
		this.mFadeBG.gameObject.SetActive(false);
		this.mUI87 = GameUITools.FindGameObject("ui87", base.gameObject);
		Tools.SetParticleRQWithUIScale(this.mUI87, 5500);
		this.mUI87.gameObject.SetActive(false);
		this.mTitleIcon = GameUITools.FindUISprite("TitleIcon", base.gameObject);
		this.mTitleIconAnim = this.mTitleIcon.GetComponent<Animation>();
		this.mTitleIcon.gameObject.SetActive(false);
		this.mTitle.text = title;
		uISprite.height = 206;
		if (list.Count == 1)
		{
			uISprite.width = 580;
			CommonIconItem.Create(base.gameObject, new Vector3(-45f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[0], true, true, false);
		}
		else if (list.Count == 2)
		{
			uISprite.width = 580;
			CommonIconItem.Create(base.gameObject, new Vector3(-114f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[0], true, true, false);
			CommonIconItem.Create(base.gameObject, new Vector3(25f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[1], true, true, false);
		}
		else if (list.Count == 3)
		{
			uISprite.width = 660;
			CommonIconItem.Create(base.gameObject, new Vector3(-183f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[0], true, true, false);
			CommonIconItem.Create(base.gameObject, new Vector3(-45f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[1], true, true, false);
			CommonIconItem.Create(base.gameObject, new Vector3(91f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[2], true, true, false);
		}
		else if (list.Count == 4)
		{
			uISprite.width = 860;
			CommonIconItem.Create(base.gameObject, new Vector3(-252f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[0], true, true, false);
			CommonIconItem.Create(base.gameObject, new Vector3(-114f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[1], true, true, false);
			CommonIconItem.Create(base.gameObject, new Vector3(25f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[2], true, true, false);
			CommonIconItem.Create(base.gameObject, new Vector3(163f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[3], true, true, false);
		}
		else if (list.Count == 5)
		{
			uISprite.width = 860;
			CommonIconItem.Create(base.gameObject, new Vector3(-321f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[0], true, true, false);
			CommonIconItem.Create(base.gameObject, new Vector3(-183f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[1], true, true, false);
			CommonIconItem.Create(base.gameObject, new Vector3(-45f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[2], true, true, false);
			CommonIconItem.Create(base.gameObject, new Vector3(91f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[3], true, true, false);
			CommonIconItem.Create(base.gameObject, new Vector3(229f, this.oneRowY, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[4], true, true, false);
		}
		else
		{
			uISprite.width = 860;
			uISprite.height = 316;
			int num = 0;
			while (num < list.Count && num < 5)
			{
				CommonIconItem.Create(base.gameObject, new Vector3((float)(-321 + num * 138), 110f, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[num], true, true, false);
				num++;
			}
			int num2 = 5;
			while (num2 < list.Count && num2 < 10)
			{
				CommonIconItem.Create(base.gameObject, new Vector3((float)(-321 + (num2 - 5) * 138), -25f, 0f), null, false, 0.9f, null).SetNameStyle(6, UILabel.Overflow.ResizeHeight).Refresh(list[num2], true, true, false);
				num2++;
			}
		}
		base.transform.localPosition = new Vector3(0f, -113f, base.transform.localPosition.z);
		this.mPanel.alpha = 0f;
		if (GUIRewardPanel.panelList != null && GUIRewardPanel.panelList.Count > 0)
		{
			this.mPanel.startingRenderQueue = GUIRewardPanel.panelList[GUIRewardPanel.panelList.Count - 1].mPanel.startingRenderQueue + 10;
		}
		if (needClick)
		{
			this.anim = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnCompleteEvent1)));
			this.anim.Append(HOTween.To(base.transform, this.time0, new TweenParms().Prop("localPosition", new Vector3(0f, 0f, base.transform.localPosition.z))));
			this.anim.Insert(0f, HOTween.To(this.mPanel, this.time0, new TweenParms().Prop("alpha", 1)));
			GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		}
		else
		{
			this.anim = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnCompleteEvent)));
			this.anim.Append(HOTween.To(base.transform, this.time0, new TweenParms().Prop("localPosition", new Vector3(0f, 0f, base.transform.localPosition.z))));
			this.anim.Insert(0f, HOTween.To(this.mPanel, this.time0, new TweenParms().Prop("alpha", 1)));
			this.anim.AppendInterval(this.time1);
			this.anim.Append(HOTween.To(base.transform, this.time2, new TweenParms().Prop("localPosition", new Vector3(0f, 150f, base.transform.localPosition.z))));
			this.anim.Insert(this.time0 + this.time1, HOTween.To(this.mPanel, this.time0, new TweenParms().Prop("alpha", 0)));
		}
		if (showPetAnim)
		{
			base.StartCoroutine(this.ShowPets(list));
		}
		else
		{
			this.anim.Play();
			Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		}
		this.mFadeBG.gameObject.SetActive(needClick);
		GUIRewardPanel.panelList.Add(this);
	}

	private int SortByQuality(KeyValuePair<ItemInfo, RewardData> a, KeyValuePair<ItemInfo, RewardData> b)
	{
		if (a.Key.Quality > b.Key.Quality)
		{
			return -1;
		}
		if (a.Key.Quality < b.Key.Quality)
		{
			return 1;
		}
		return 0;
	}

	[DebuggerHidden]
	private IEnumerator ShowPets(List<RewardData> rewards)
	{
        return null;
        //GUIRewardPanel.<ShowPets>c__Iterator4A <ShowPets>c__Iterator4A = new GUIRewardPanel.<ShowPets>c__Iterator4A();
        //<ShowPets>c__Iterator4A.rewards = rewards;
        //<ShowPets>c__Iterator4A.<$>rewards = rewards;
        //<ShowPets>c__Iterator4A.<>f__this = this;
        //return <ShowPets>c__Iterator4A;
	}

	private void OnCompleteEvent()
	{
		GUIRewardPanel.panelList.Remove(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnCompleteEvent1()
	{
		GameUIManager.mInstance.HideFadeBG(false);
		this.mGoOn.gameObject.SetActive(true);
		GUIRewardPanel.EAnimType eAnimType = this.mAnimType;
		if (eAnimType != GUIRewardPanel.EAnimType.EAT_Double)
		{
			if (eAnimType == GUIRewardPanel.EAnimType.EAT_Give)
			{
				this.mTitleIcon.gameObject.SetActive(true);
				this.mTitleIcon.spriteName = "lucky";
				this.mTitleIconAnim.Play();
				this.mUI87.gameObject.SetActive(false);
				this.mUI87.gameObject.SetActive(true);
			}
		}
		else
		{
			this.mTitleIcon.gameObject.SetActive(true);
			this.mTitleIcon.spriteName = "double";
			this.mTitleIconAnim.Play();
			this.mUI87.gameObject.SetActive(false);
			this.mUI87.gameObject.SetActive(true);
		}
	}

	private void OnCompleteEvent2()
	{
		GameUIManager.mInstance.HideFadeBG(false);
		GUIRewardPanel.panelList.Remove(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void ImmediateClose()
	{
		if (this.anim != null)
		{
			HOTween.Kill(this.anim);
		}
		this.OnCompleteEvent();
	}

	private void OnFadeBGClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		this.anim = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnCompleteEvent2)));
		this.anim.Append(HOTween.To(base.transform, this.time2, new TweenParms().Prop("localPosition", new Vector3(0f, 150f, base.transform.localPosition.z))));
		this.anim.Insert(0f, HOTween.To(this.mPanel, this.time0, new TweenParms().Prop("alpha", 0)));
		this.anim.Play();
		if (this.OnClickEvent != null)
		{
			this.OnClickEvent();
		}
	}

	public void SetAnimType(GUIRewardPanel.EAnimType type)
	{
		this.mAnimType = type;
		if (type == GUIRewardPanel.EAnimType.EAT_Double || type == GUIRewardPanel.EAnimType.EAT_Give)
		{
			this.mTitle.gameObject.SetActive(false);
			CommonIconItem[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonIconItem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				CommonIconItem commonIconItem = componentsInChildren[i];
				Vector3 localPosition = commonIconItem.transform.localPosition;
				localPosition.y = this.animOneRowY;
				commonIconItem.transform.localPosition = localPosition;
			}
		}
	}
}
