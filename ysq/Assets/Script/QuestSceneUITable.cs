using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestSceneUITable : UITable
{
	public int SortByQuestData(Transform a, Transform b)
	{
		QuestInfo questInfo = a.GetComponent<QuestSceneItem>().questInfo;
		QuestInfo questInfo2 = b.GetComponent<QuestSceneItem>().questInfo;
		if (questInfo.ID < questInfo2.ID)
		{
			return -1;
		}
		return 1;
	}

	protected override void Sort(List<Transform> list)
	{
		list.Sort(new Comparison<Transform>(this.SortByQuestData));
	}
}
