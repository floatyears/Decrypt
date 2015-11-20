using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LuckydeedLayer : MonoBehaviour
{
	private LuckDeedGrid[] luckDeedTable = new LuckDeedGrid[3];

	private UIToggle[] tabPage = new UIToggle[3];

	private GameObject[] newFlag = new GameObject[3];

	private UILabel[] mTimeTxt = new UILabel[3];

	private int curSelectTab;

	public void Init()
	{
		this.CreateObjects();
	}

	public void RefreshLayer()
	{
		ActivityHalloweenData hData = Globals.Instance.Player.ActivitySystem.HData;
		this.mTimeTxt[0].text = Singleton<StringManager>.Instance.GetString("festival12", new object[]
		{
			hData.Ext.FirstRewardTime
		});
		this.mTimeTxt[1].text = Singleton<StringManager>.Instance.GetString("festival12", new object[]
		{
			hData.Ext.SecondRewardTime
		});
		this.mTimeTxt[2].text = Singleton<StringManager>.Instance.GetString("festival12", new object[]
		{
			hData.Ext.ThirdRewardTime
		});
		this.AddData();
		this.luckDeedTable[this.curSelectTab].repositionNow = true;
	}

	private void CreateObjects()
	{
		for (int i = 0; i < this.mTimeTxt.Length; i++)
		{
			this.mTimeTxt[i] = base.transform.Find(string.Format("time{0}/tabTxt0", i)).GetComponent<UILabel>();
		}
		for (int j = 0; j < 3; j++)
		{
			this.luckDeedTable[j] = base.transform.FindChild(string.Format("bagPanel{0}/bagContents", j)).gameObject.AddComponent<LuckDeedGrid>();
			this.luckDeedTable[j].maxPerLine = 1;
			this.luckDeedTable[j].arrangement = UICustomGrid.Arrangement.Vertical;
			this.luckDeedTable[j].cellWidth = 454f;
			this.luckDeedTable[j].cellHeight = 98f;
			this.luckDeedTable[j].gapHeight = 2f;
			this.luckDeedTable[j].gapWidth = 2f;
			this.tabPage[j] = base.transform.Find(string.Format("time{0}", j)).GetComponent<UIToggle>();
			EventDelegate.Add(this.tabPage[j].onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
			UIEventListener expr_132 = UIEventListener.Get(this.tabPage[j].gameObject);
			expr_132.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_132.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
			this.newFlag[j] = this.tabPage[j].transform.Find("new").gameObject;
			this.newFlag[j].SetActive(false);
		}
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		for (int i = 0; i < 2; i++)
		{
			if (this.tabPage[i] == go && this.curSelectTab != i)
			{
				this.curSelectTab = i;
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
				if (this.luckDeedTable[0].mDatas != null)
				{
					this.luckDeedTable[0].repositionNow = true;
				}
			}
			else if (UIToggle.current == this.tabPage[1])
			{
				if (this.luckDeedTable[1].mDatas != null)
				{
					this.luckDeedTable[1].repositionNow = true;
				}
			}
			else if (UIToggle.current == this.tabPage[2] && this.luckDeedTable[2].mDatas != null)
			{
				this.luckDeedTable[2].repositionNow = true;
			}
		}
	}

	public void AddData()
	{
		ActivityHalloweenData hData = Globals.Instance.Player.ActivitySystem.HData;
		List<HalloweenContract> conData = Globals.Instance.Player.ActivitySystem.conData;
		if (hData == null || conData == null)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			this.luckDeedTable[i].ClearData();
			ActivityHalloweenDataEx[] array = new ActivityHalloweenDataEx[3];
			for (int j = 0; j < conData.Count; j++)
			{
				if (i == 0)
				{
					array[conData[j].ID - 1] = new ActivityHalloweenDataEx(hData, conData[j].ID, conData[j].FirstLuckNum, conData[j].FirstLuckPlayer);
				}
				else if (i == 1)
				{
					array[conData[j].ID - 1] = new ActivityHalloweenDataEx(hData, conData[j].ID, conData[j].SecondLuckNum, conData[j].SecondLuckPlayer);
				}
				else if (i == 2)
				{
					array[conData[j].ID - 1] = new ActivityHalloweenDataEx(hData, conData[j].ID, conData[j].ThirdLuckNum, conData[j].ThirdLuckPlayer);
				}
			}
			for (int k = 0; k < 3; k++)
			{
				if (array[k] == null)
				{
					array[k] = new ActivityHalloweenDataEx(hData.Ext.Data[k].ID);
				}
				this.luckDeedTable[i].AddData(array[k]);
			}
			this.luckDeedTable[i].repositionNow = true;
		}
	}
}
