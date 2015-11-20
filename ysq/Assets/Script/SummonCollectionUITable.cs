using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SummonCollectionUITable : UIGrid
{
	public enum FilterType
	{
		Fire,
		Water,
		Wood,
		Light,
		Dark,
		MAX
	}

	private SummonCollectionUITable.FilterType filter = SummonCollectionUITable.FilterType.MAX;

	public SummonCollectionUITable.FilterType Filter
	{
		get
		{
			return this.filter;
		}
		set
		{
			if (this.filter != value)
			{
				this.filter = value;
				for (int i = 0; i < base.transform.childCount; i++)
				{
					CollectionContentBase component = base.transform.GetChild(i).GetComponent<CollectionContentBase>();
					if (!(component == null))
					{
						bool state = true;
						switch (this.filter)
						{
						case SummonCollectionUITable.FilterType.Fire:
							state = (component.mElementType == EElementType.EET_Fire);
							break;
						case SummonCollectionUITable.FilterType.Water:
							state = (component.mElementType == EElementType.EET_Water);
							break;
						case SummonCollectionUITable.FilterType.Wood:
							state = (component.mElementType == EElementType.EET_Wood);
							break;
						case SummonCollectionUITable.FilterType.Light:
							state = (component.mElementType == EElementType.EET_Light);
							break;
						case SummonCollectionUITable.FilterType.Dark:
							state = (component.mElementType == EElementType.EET_Dark);
							break;
						}
						NGUITools.SetActive(component.gameObject, state);
					}
				}
				base.repositionNow = true;
			}
		}
	}

	public int SortSummonCollection(Transform a, Transform b)
	{
		CollectionContentBase component = a.GetComponent<CollectionContentBase>();
		CollectionContentBase component2 = b.GetComponent<CollectionContentBase>();
		if (!(component != null) || !(component2 != null))
		{
			return 0;
		}
		if (component.mPriority > component2.mPriority)
		{
			return 1;
		}
		if (component.mPriority < component2.mPriority)
		{
			return -1;
		}
		return 0;
	}

	protected override void Sort(List<Transform> list)
	{
		list.Sort(new Comparison<Transform>(this.SortSummonCollection));
	}
}
