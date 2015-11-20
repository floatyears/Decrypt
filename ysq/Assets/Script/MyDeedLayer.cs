using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MyDeedLayer : MonoBehaviour
{
	private MyDeedGrid[] myDeedTable = new MyDeedGrid[3];

	private UIToggle[] tabPage = new UIToggle[3];

	private GameObject[] newFlag = new GameObject[3];

	private int curSelectTab;

	public void Init()
	{
		this.CreateObjects();
	}

	public void RefreshLayer()
	{
		this.AddData();
	}

	private void CreateObjects()
	{
		for (int i = 0; i < 3; i++)
		{
			this.myDeedTable[i] = base.transform.FindChild(string.Format("bagPanel{0}/bagContents", i)).gameObject.AddComponent<MyDeedGrid>();
			this.myDeedTable[i].maxPerLine = 1;
			this.myDeedTable[i].arrangement = UICustomGrid.Arrangement.Vertical;
			this.myDeedTable[i].cellWidth = 455f;
			this.myDeedTable[i].cellHeight = 55f;
			this.myDeedTable[i].gapHeight = 0f;
			this.myDeedTable[i].gapWidth = 0f;
			this.tabPage[i] = base.transform.Find(string.Format("tab{0}", i)).GetComponent<UIToggle>();
			EventDelegate.Add(this.tabPage[i].onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
			UIEventListener expr_F1 = UIEventListener.Get(this.tabPage[i].gameObject);
			expr_F1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_F1.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
			this.newFlag[i] = this.tabPage[i].transform.Find("new").gameObject;
			this.newFlag[i].SetActive(false);
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

	public void OnTabCheckChanged()
	{
		if (!UIToggle.current.value)
		{
			return;
		}
		if (UIToggle.current.value)
		{
			if (UIToggle.current == this.tabPage[0])
			{
				if (this.myDeedTable[0].mDatas != null)
				{
					this.myDeedTable[0].repositionNow = true;
				}
			}
			else if (UIToggle.current == this.tabPage[1])
			{
				if (this.myDeedTable[1].mDatas != null)
				{
					this.myDeedTable[1].repositionNow = true;
				}
			}
			else if (UIToggle.current == this.tabPage[2] && this.myDeedTable[1].mDatas != null)
			{
				this.myDeedTable[2].repositionNow = true;
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
			this.myDeedTable[i].ClearData();
			for (int j = 0; j < conData.Count; j++)
			{
				if (conData[j].ID == i + 1)
				{
					for (int k = 0; k < conData[j].MyNums.Count; k++)
					{
						this.myDeedTable[i].AddData(new AHDataEx(hData, conData[j].ID, conData[j].MyNums[k], true, false, false));
					}
					for (int l = 0; l < conData[j].LuckyNums.Count; l++)
					{
						this.myDeedTable[i].AddData(new AHDataEx(hData, conData[j].ID, conData[j].LuckyNums[l], false, false, true));
					}
					for (int m = 0; m < conData[j].UnluckyNums.Count; m++)
					{
						this.myDeedTable[i].AddData(new AHDataEx(hData, conData[j].ID, conData[j].UnluckyNums[m], false, true, false));
					}
					break;
				}
			}
			this.myDeedTable[i].repositionNow = true;
		}
	}
}
