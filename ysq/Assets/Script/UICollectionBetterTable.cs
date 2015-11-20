using Att;
using System;
using UnityEngine;

public sealed class UICollectionBetterTable : MonoBehaviour
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

	private int itemSize = 105;

	private int itemSize2 = 95;

	public bool cullContent = true;

	private Transform mTrans;

	private UIPanel mPanel;

	private UIScrollView mScroll;

	private BetterList<Transform> mChildren = new BetterList<Transform>();

	private SummonCollectionLayer mBaseLayer;

	private UICollectionBetterTable.FilterType filter = UICollectionBetterTable.FilterType.MAX;

	public UICollectionBetterTable.FilterType Filter
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
				for (int i = 0; i < this.mBaseLayer.mContents.Count; i++)
				{
					CollectionContentBase component = this.mBaseLayer.mContents[i].GetComponent<CollectionContentBase>();
					if (!(component == null))
					{
						bool flag = true;
						switch (this.filter)
						{
						case UICollectionBetterTable.FilterType.Fire:
							flag = (component.mElementType == EElementType.EET_Fire);
							break;
						case UICollectionBetterTable.FilterType.Water:
							flag = (component.mElementType == EElementType.EET_Water);
							break;
						case UICollectionBetterTable.FilterType.Wood:
							flag = (component.mElementType == EElementType.EET_Wood);
							break;
						case UICollectionBetterTable.FilterType.Light:
							flag = (component.mElementType == EElementType.EET_Light);
							break;
						case UICollectionBetterTable.FilterType.Dark:
							flag = (component.mElementType == EElementType.EET_Dark);
							break;
						}
						if (flag)
						{
							component.transform.parent = base.transform;
							component.transform.position = Vector3.zero;
							component.transform.localScale = Vector3.one;
							NGUITools.SetActive(component.gameObject, flag);
						}
						else
						{
							component.transform.parent = this.mBaseLayer.mbagHideGo.transform;
						}
					}
				}
			}
			this.WrapContent();
			this.SortSelf();
		}
	}

	public void InitWithBaseLayer(SummonCollectionLayer baseLayer)
	{
		this.mBaseLayer = baseLayer;
		this.DoInit();
	}

	private void DoInit()
	{
		this.SortSelf();
		this.WrapContent();
		if (this.mScroll != null)
		{
			this.mScroll.GetComponent<UIPanel>().onClipMove = new UIPanel.OnClippingMoved(this.OnMove);
			this.mScroll.restrictWithinPanel = true;
		}
	}

	private void OnMove(UIPanel panel)
	{
		this.WrapContent();
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

	public void SortSelf()
	{
		if (!this.CacheScrollView())
		{
			return;
		}
		this.mChildren.Clear();
		for (int i = 0; i < this.mTrans.childCount; i++)
		{
			if (this.mTrans.GetChild(i).gameObject.activeInHierarchy)
			{
				this.mChildren.Add(this.mTrans.GetChild(i));
			}
		}
		this.mChildren.Sort(new BetterList<Transform>.CompareFunc(this.SortSummonCollection));
		this.ResetChildPositions();
	}

	private void ResetChildPositions()
	{
		int num = 0;
		for (int i = 0; i < this.mChildren.size; i++)
		{
			Transform transform = this.mChildren[i];
			transform.localPosition = new Vector3(0f, (float)(-(float)num), 0f);
			CollectionContentBase component = transform.GetComponent<CollectionContentBase>();
			if (component != null)
			{
				if (component.mIsTitle)
				{
					num += this.itemSize2;
				}
				else
				{
					num += this.itemSize;
				}
			}
		}
	}

	public void WrapContent()
	{
		if (this.mPanel == null)
		{
			return;
		}
		Vector3[] worldCorners = this.mPanel.worldCorners;
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector = worldCorners[i];
			vector = this.mTrans.InverseTransformPoint(vector);
			worldCorners[i] = vector;
		}
		Vector3 vector2 = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
		float num = worldCorners[0].y - (float)this.itemSize;
		float num2 = worldCorners[2].y + (float)this.itemSize;
		for (int j = 0; j < this.mChildren.size; j++)
		{
			Transform transform = this.mChildren[j];
			float num3 = transform.localPosition.y - vector2.y;
			if (this.cullContent)
			{
				num3 += this.mPanel.clipOffset.y - this.mTrans.localPosition.y;
				if (!UICamera.IsPressed(transform.gameObject))
				{
					NGUITools.SetActive(transform.gameObject, num3 > num && num3 < num2, false);
				}
			}
		}
	}

	private bool CacheScrollView()
	{
		this.mTrans = base.transform;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		this.mScroll = this.mPanel.GetComponent<UIScrollView>();
		return !(this.mScroll == null);
	}

	public void Refresh(PetDataEx petData)
	{
		if (petData != null)
		{
			for (int i = 0; i < this.mChildren.size; i++)
			{
				CollectionContent collectionContent = this.mChildren[i].GetComponent<CollectionContentBase>() as CollectionContent;
				if (collectionContent != null)
				{
					collectionContent.Refresh(petData);
				}
			}
		}
	}
}
