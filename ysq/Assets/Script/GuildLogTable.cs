using System;
using System.Collections.Generic;
using UnityEngine;

public class GuildLogTable : UITable
{
	private int SortByLogTime(Transform a, Transform b)
	{
		int timeStamp = a.GetComponent<GuildLogChunk>().GetTimeStamp();
		int timeStamp2 = b.GetComponent<GuildLogChunk>().GetTimeStamp();
		if (timeStamp > timeStamp2)
		{
			return -1;
		}
		if (timeStamp < timeStamp2)
		{
			return 1;
		}
		return 0;
	}

	protected override void Sort(List<Transform> list)
	{
		list.Sort(new Comparison<Transform>(this.SortByLogTime));
	}
}
