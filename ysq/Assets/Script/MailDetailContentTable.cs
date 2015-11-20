using System;
using System.Collections.Generic;
using UnityEngine;

public class MailDetailContentTable : UITable
{
	public int SortMailContentInfo(Transform a, Transform b)
	{
		MailContentElementBase component = a.GetComponent<MailContentElementBase>();
		MailContentElementBase component2 = b.GetComponent<MailContentElementBase>();
		if (!(component != null) || !(component2 != null))
		{
			return 0;
		}
		if (component.ElementPriority < component2.ElementPriority)
		{
			return -1;
		}
		if (component.ElementPriority > component2.ElementPriority)
		{
			return 1;
		}
		return 0;
	}

	protected override void Sort(List<Transform> list)
	{
		list.Sort(new Comparison<Transform>(this.SortMailContentInfo));
	}
}
