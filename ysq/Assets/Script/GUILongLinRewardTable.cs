using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUILongLinRewardTable : UICustomGrid
{
	private GUILongLinRewardPopUp mBaseScene;

	public void InitWithBaseScene(GUILongLinRewardPopUp baseScene)
	{
		this.mBaseScene = baseScene;
	}

	protected override UICustomGridItem CreateGridItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/longLinRewardItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GUILongLinRewardItem gUILongLinRewardItem = gameObject.AddComponent<GUILongLinRewardItem>();
		gUILongLinRewardItem.InitWithBaseScene(this.mBaseScene);
		return gUILongLinRewardItem;
	}

	private int SortByRankLvl(BaseData a, BaseData b)
	{
		GUILongLinRewardData gUILongLinRewardData = (GUILongLinRewardData)a;
		GUILongLinRewardData gUILongLinRewardData2 = (GUILongLinRewardData)b;
		if (gUILongLinRewardData != null && gUILongLinRewardData2 != null)
		{
			bool flag = gUILongLinRewardData.IsCanTaken();
			bool flag2 = gUILongLinRewardData2.IsCanTaken();
			if (flag && !flag2)
			{
				return -1;
			}
			if (!flag && flag2)
			{
				return 1;
			}
			bool flag3 = gUILongLinRewardData.IsRewardTakened();
			bool flag4 = gUILongLinRewardData2.IsRewardTakened();
			if (flag3 && !flag4)
			{
				return 1;
			}
			if (!flag3 && flag4)
			{
				return -1;
			}
			if (gUILongLinRewardData.mFDSInfo != null && gUILongLinRewardData2.mFDSInfo != null)
			{
				return gUILongLinRewardData.mFDSInfo.ID - gUILongLinRewardData2.mFDSInfo.ID;
			}
			if (gUILongLinRewardData.mWorldRespawnInfo != null && gUILongLinRewardData2.mWorldRespawnInfo != null)
			{
				return gUILongLinRewardData.mWorldRespawnInfo.ID - gUILongLinRewardData2.mWorldRespawnInfo.ID;
			}
		}
		return 0;
	}

	protected override void Sort(List<BaseData> list)
	{
		list.Sort(new Comparison<BaseData>(this.SortByRankLvl));
	}

	private void OnTweenFinished(TweenEvent callbackData)
	{
		if (callbackData.parms.Length > 0)
		{
			GUILongLinRewardItem gUILongLinRewardItem = (GUILongLinRewardItem)callbackData.parms[0];
			if (gUILongLinRewardItem != null)
			{
				gUILongLinRewardItem.Refresh();
			}
		}
		base.repositionNow = true;
	}

	public void Refresh(int fdId)
	{
		if (fdId == 0)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				GUILongLinRewardItem component = base.transform.GetChild(i).GetComponent<GUILongLinRewardItem>();
				if (component != null)
				{
					component.Refresh();
				}
			}
			base.repositionNow = true;
		}
		else
		{
			for (int j = 0; j < base.transform.childCount; j++)
			{
				GUILongLinRewardItem component2 = base.transform.GetChild(j).GetComponent<GUILongLinRewardItem>();
				if (component2 != null && ((component2.GetRewardData().mFDSInfo != null && component2.GetRewardData().mFDSInfo.ID == fdId) || (component2.GetRewardData().mWorldRespawnInfo != null && component2.GetRewardData().mWorldRespawnInfo.ID == fdId)))
				{
					HOTween.To(component2.transform, 0.2f, new TweenParms().Prop("localPosition", new PlugVector3X(950f)).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.EaseOutSine).OnComplete(new TweenDelegate.TweenCallbackWParms(this.OnTweenFinished), new object[]
					{
						component2
					}));
				}
			}
		}
	}
}
