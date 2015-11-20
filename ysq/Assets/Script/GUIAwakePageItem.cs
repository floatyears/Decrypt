using Att;
using System;
using UnityEngine;

public class GUIAwakePageItem : UICustomGridItem
{
	private GUIAwakeRoadSceneV2 mBaseScene;

	private GameObject mTabGo;

	private GameObject mTabFGo;

	private UILabel mTabGoLb;

	private UILabel mTabFGoLb;

	private GameObject mNewMark;

	private GUIAwakePageItemData mGUIAwakePageItemData;

	private bool mIsShowNewMark;

	public GUIAwakePageItemData PageItemData
	{
		get
		{
			return this.mGUIAwakePageItemData;
		}
	}

	public bool IsShowNewMark
	{
		get
		{
			return this.mIsShowNewMark;
		}
		set
		{
			this.mIsShowNewMark = value;
			this.mNewMark.SetActive(this.mIsShowNewMark);
		}
	}

	public void InitWithBaseScene(GUIAwakeRoadSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	public void UpdateUIBoxCollider(Transform tr, float addWidth)
	{
		UISprite component = tr.GetComponent<UISprite>();
		if (component != null)
		{
			component.autoResizeBoxCollider = false;
			BoxCollider component2 = tr.GetComponent<BoxCollider>();
			if (component2 != null)
			{
				UIWidget component3 = tr.GetComponent<UIWidget>();
				if (component3 != null)
				{
					Vector3[] localCorners = component3.localCorners;
					component2.center = Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
					Vector3 vector = localCorners[2] - localCorners[0];
					component2.size = new Vector3(vector.x + addWidth, vector.y, 0f);
				}
			}
		}
	}

	private void CreateObjects()
	{
		this.mTabGo = base.transform.Find("tab").gameObject;
		UIEventListener expr_26 = UIEventListener.Get(this.mTabGo);
		expr_26.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_26.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mTabGoLb = this.mTabGo.transform.Find("Label").GetComponent<UILabel>();
		this.mTabFGo = base.transform.Find("tabF").gameObject;
		this.mTabFGoLb = this.mTabFGo.transform.Find("Label").GetComponent<UILabel>();
		this.mTabGoLb.spacingX = 0;
		this.mTabFGoLb.spacingX = 0;
		this.mNewMark = base.transform.Find("newMark").gameObject;
		this.mNewMark.SetActive(false);
		this.UpdateUIBoxCollider(this.mTabGo.transform, 30f);
		this.UpdateUIBoxCollider(this.mTabFGo.transform, 30f);
	}

	public override void Refresh(object data)
	{
		if (this.mGUIAwakePageItemData != data)
		{
			this.mGUIAwakePageItemData = (GUIAwakePageItemData)data;
			this.Refresh();
		}
	}

	private int GetTotalScore()
	{
		int num = 0;
		for (int i = 1; i <= 32; i++)
		{
			for (int j = 1; j <= 5; j++)
			{
				int num2 = 600000 + i * 1000 + j;
				if (num2 % 100000 / 1000 == this.mGUIAwakePageItemData.mPageIndex)
				{
					if (Globals.Instance.Player.GetSceneScore(num2) == 0)
					{
						break;
					}
					num += Globals.Instance.Player.GetSceneScore(num2);
				}
			}
		}
		return num;
	}

	private bool IsCanTakeReward()
	{
		if (this.mGUIAwakePageItemData != null)
		{
			int num = this.mGUIAwakePageItemData.mPageIndex + 3000;
			MapInfo info = Globals.Instance.AttDB.MapDict.GetInfo(num);
			if (info == null)
			{
				return false;
			}
			int mapRewardMask = Globals.Instance.Player.GetMapRewardMask(num);
			for (int i = 0; i < 3; i++)
			{
				if (this.GetTotalScore() >= info.NeedStar[i] && (mapRewardMask & 1 << i) == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool IsCanQiYu()
	{
		if (this.mGUIAwakePageItemData != null)
		{
			int num = 600000 + this.mGUIAwakePageItemData.mPageIndex * 1000 + 6;
			int sceneID = num - 1;
			return Globals.Instance.Player.GetSceneScore(sceneID) > 0 && Globals.Instance.Player.GetSceneTimes(num) == 0;
		}
		return false;
	}

	public void Refresh()
	{
		if (this.mGUIAwakePageItemData == null)
		{
			return;
		}
		this.mTabGoLb.text = Singleton<StringManager>.Instance.GetString("awakeRoad1", new object[]
		{
			this.mGUIAwakePageItemData.mPageIndex
		});
		this.mTabFGoLb.text = this.mTabGoLb.text;
		this.mTabGo.SetActive(!this.mGUIAwakePageItemData.mIsChecked);
		this.mTabFGo.SetActive(this.mGUIAwakePageItemData.mIsChecked);
		this.IsShowNewMark = (this.IsCanTakeReward() || this.IsCanQiYu());
	}

	private void OnTabClick(GameObject go)
	{
		if (this.mGUIAwakePageItemData != null)
		{
			this.mBaseScene.TrySetCurPageIndex(this.mGUIAwakePageItemData.mPageIndex);
		}
	}
}
