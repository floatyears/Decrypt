using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/UICustomGrid")]
public abstract class UICustomGrid : UIWidgetContainer
{
	public enum Arrangement
	{
		Horizontal,
		Vertical,
		VerticalReverse
	}

	public UICustomGrid.Arrangement arrangement = UICustomGrid.Arrangement.Vertical;

	public int maxPerLine;

	public float cellWidth = 100f;

	public float cellHeight = 100f;

	public float gapWidth = 4f;

	public float gapHeight = 4f;

	public float xOffset;

	public float yOffset;

	public UIScrollBar scrollBar;

	public UIDragScrollView bgScrollView;

	protected bool mReposition;

	protected UIPanel mPanel;

	protected UICustomScrollView mScrollView;

	public UIScrollView.DragEffect mDragEffect = UIScrollView.DragEffect.MomentumAndSpring;

	protected bool mInitDone;

	private int mMaxCount;

	private float originPanelX;

	private float originPanelY;

	public UICustomGridItem[] gridItems;

	public List<BaseData> mDatas = new List<BaseData>();

	private int dataCount;

	public ulong focusID;

	private UICenterOnChild mUICenterOnChild;

	private bool mAutoCenterOnChild;

	public bool repositionNow
	{
		get
		{
			return this.mReposition;
		}
		set
		{
			if (value)
			{
				this.mReposition = true;
				base.enabled = true;
			}
		}
	}

	public bool AutoCenterOnChild
	{
		get
		{
			return this.mAutoCenterOnChild;
		}
		set
		{
			this.mAutoCenterOnChild = value;
			if (value)
			{
				this.mUICenterOnChild = base.gameObject.AddComponent<UICenterOnChild>();
				UICenterOnChild expr_24 = this.mUICenterOnChild;
				expr_24.onCenter = (UICenterOnChild.OnCenterCallback)Delegate.Combine(expr_24.onCenter, new UICenterOnChild.OnCenterCallback(this.OnUICenterOnChild));
				UICenterOnChild expr_4C = this.mUICenterOnChild;
				expr_4C.onFinished = (SpringPanel.OnFinished)Delegate.Combine(expr_4C.onFinished, new SpringPanel.OnFinished(this.OnUICenterOnChildFinished));
			}
		}
	}

	public void SetDragAmount(float x, float y)
	{
		if (this.mScrollView)
		{
			this.mScrollView.SetDragAmount(x, y, false);
		}
	}

	public float GetDragAmount()
	{
		if (this.mPanel == null)
		{
			return 0f;
		}
		if (this.mScrollView)
		{
			Bounds bounds = this.mScrollView.bounds;
			Vector2 vector = bounds.min;
			Vector2 vector2 = bounds.max;
			if (this.mScrollView.canMoveVertically)
			{
				if (vector2.y > vector.y)
				{
					Vector4 finalClipRegion = this.mPanel.finalClipRegion;
					int num = Mathf.RoundToInt(finalClipRegion.w);
					if ((num & 1) != 0)
					{
						num--;
					}
					float num2 = (float)num * 0.5f;
					num2 = Mathf.Round(num2);
					if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
					{
						num2 -= this.mPanel.clipSoftness.y;
					}
					float num3 = vector2.y - vector.y;
					float num4 = num2 * 2f;
					float num5 = vector.y;
					float num6 = vector2.y;
					float num7 = finalClipRegion.y - num2;
					float num8 = finalClipRegion.y + num2;
					num5 = num7 - num5;
					num6 -= num8;
					float num9;
					if (num4 < num3)
					{
						num5 = Mathf.Clamp01(num5 / num3);
						num6 = Mathf.Clamp01(num6 / num3);
						num9 = num5 + num6;
						return (num9 <= 0.001f) ? 0f : (num5 / num9);
					}
					num5 = Mathf.Clamp01(-num5 / num3);
					num6 = Mathf.Clamp01(-num6 / num3);
					num9 = num5 + num6;
					return (num9 <= 0.001f) ? 0f : (num5 / num9);
				}
			}
			else if (this.mScrollView.canMoveHorizontally && vector2.x > vector.x)
			{
				Vector4 finalClipRegion2 = this.mPanel.finalClipRegion;
				int num10 = Mathf.RoundToInt(finalClipRegion2.z);
				if ((num10 & 1) != 0)
				{
					num10--;
				}
				float num11 = (float)num10 * 0.5f;
				num11 = Mathf.Round(num11);
				if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num11 -= this.mPanel.clipSoftness.x;
				}
				float num12 = vector2.x - vector.x;
				float num13 = num11 * 2f;
				float num14 = vector.x;
				float num15 = vector2.x;
				float num16 = finalClipRegion2.x - num11;
				float num17 = finalClipRegion2.x + num11;
				num14 = num16 - num14;
				num15 -= num17;
				float num18;
				if (num13 < num12)
				{
					num14 = Mathf.Clamp01(num14 / num12);
					num15 = Mathf.Clamp01(num15 / num12);
					num18 = num14 + num15;
					return (num18 <= 0.001f) ? 0f : (num14 / num18);
				}
				num14 = Mathf.Clamp01(-num14 / num12);
				num15 = Mathf.Clamp01(-num15 / num12);
				num18 = num14 + num15;
				return (num18 <= 0.001f) ? 0f : (num14 / num18);
			}
		}
		return 0f;
	}

	public void CenterOnChild(BaseData data)
	{
	}

	public UICenterOnChild GetUICenterOnChild()
	{
		return this.mUICenterOnChild;
	}

	public BaseData GetData(ulong id)
	{
		for (int i = 0; i < this.mDatas.Count; i++)
		{
			if (this.mDatas[i] != null && this.mDatas[i].GetID() == id)
			{
				return this.mDatas[i];
			}
		}
		return null;
	}

	public void AddData(BaseData data)
	{
		this.mDatas.Add(data);
		this.repositionNow = true;
	}

	public bool RemoveData(ulong id)
	{
		for (int i = 0; i < this.mDatas.Count; i++)
		{
			if (this.mDatas[i] != null && this.mDatas[i].GetID() == id)
			{
				this.mDatas.RemoveAt(i);
				this.repositionNow = true;
				return true;
			}
		}
		return false;
	}

	public void ClearData()
	{
		this.mDatas.Clear();
		this.repositionNow = true;
	}

	public void ClearGridItem()
	{
		if (this.gridItems == null)
		{
			return;
		}
		for (int i = 0; i < this.gridItems.Length; i++)
		{
			if (this.gridItems[i] != null)
			{
				UnityEngine.Object.Destroy(this.gridItems[i].gameObject);
				this.gridItems[i] = null;
			}
		}
	}

	private int GetDataIndex(ulong id)
	{
		for (int i = 0; i < this.mDatas.Count; i++)
		{
			if (this.mDatas[i] != null && this.mDatas[i].GetID() == id)
			{
				return i;
			}
		}
		return 0;
	}

	protected abstract UICustomGridItem CreateGridItem();

	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		this.mPanel.onClipMove = new UIPanel.OnClippingMoved(this.OnClipMoved);
		this.originPanelX = this.mPanel.transform.localPosition.x;
		this.originPanelY = this.mPanel.transform.localPosition.y;
		this.mScrollView = this.mPanel.gameObject.AddComponent<UICustomScrollView>();
		this.mScrollView.disableDragIfFits = true;
		this.mScrollView.dragEffect = this.mDragEffect;
		this.mScrollView.Init((int)this.arrangement);
		if (this.bgScrollView != null)
		{
			this.bgScrollView.scrollView = this.mScrollView;
		}
		if (this.arrangement == UICustomGrid.Arrangement.Horizontal)
		{
			this.mScrollView.movement = UIScrollView.Movement.Horizontal;
			this.mScrollView.horizontalScrollBar = this.scrollBar;
			this.mMaxCount = Mathf.CeilToInt(this.mPanel.width / (this.cellWidth + this.gapWidth)) + 1;
			this.gridItems = new UICustomGridItem[this.mMaxCount];
		}
		else if (this.arrangement == UICustomGrid.Arrangement.Vertical || this.arrangement == UICustomGrid.Arrangement.VerticalReverse)
		{
			this.mScrollView.movement = UIScrollView.Movement.Vertical;
			this.mScrollView.verticalScrollBar = this.scrollBar;
			this.mMaxCount = Mathf.CeilToInt(this.mPanel.height / (this.cellHeight + this.gapHeight)) + 1;
			this.gridItems = new UICustomGridItem[this.mMaxCount * this.maxPerLine];
		}
	}

	protected virtual void Start()
	{
		if (!this.mInitDone)
		{
			this.Init();
		}
		this.Reposition(true);
		base.enabled = false;
	}

	protected virtual void Update()
	{
		if (this.mReposition)
		{
			this.Reposition(true);
		}
		base.enabled = false;
	}

	public void Reposition(bool sort = true)
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(this))
		{
			this.mReposition = true;
			return;
		}
		if (!this.mInitDone)
		{
			this.Init();
		}
		this.mReposition = false;
		if (this.dataCount != this.mDatas.Count)
		{
			this.dataCount = this.mDatas.Count;
			this.UpdateBounds(this.dataCount);
			if (this.focusID != 0uL)
			{
				this.Sort(this.mDatas);
				int num = (this.GetDataIndex(this.focusID) + this.maxPerLine) / this.maxPerLine;
				int num2 = (this.mDatas.Count + this.maxPerLine - 1) / this.maxPerLine;
				if (this.arrangement == UICustomGrid.Arrangement.Horizontal)
				{
					float num3 = (float)(num - 1) * (this.cellWidth + this.gapWidth) / ((float)num2 * (this.cellWidth + this.gapWidth) - this.mScrollView.panel.baseClipRegion.x);
					if (num3 > 0f)
					{
						this.SetDragAmount(num3, 0f);
					}
				}
				else
				{
					float num4 = (float)(num - 1) * (this.cellHeight + this.gapHeight) / ((float)num2 * (this.cellHeight + this.gapHeight) - this.mScrollView.panel.baseClipRegion.w);
					if (num4 > 0f)
					{
						this.SetDragAmount(0f, num4);
					}
				}
				this.focusID = 0uL;
			}
			for (int i = 0; i < this.gridItems.Length; i++)
			{
				if (this.gridItems[i] != null && this.gridItems[i].gameObject.activeInHierarchy)
				{
					this.gridItems[i].gameObject.SetActive(false);
				}
			}
		}
		if (this.mDatas.Count == 0)
		{
			return;
		}
		if (sort)
		{
			this.Sort(this.mDatas);
		}
		if (this.arrangement == UICustomGrid.Arrangement.Horizontal)
		{
			float num5 = Mathf.Abs(this.mScrollView.panel.transform.localPosition.x - this.originPanelX);
			int num6 = Mathf.FloorToInt(num5 / (this.cellWidth + this.gapWidth));
			if (num6 < 0)
			{
				num6 = 0;
			}
			for (int j = 0; j < this.mMaxCount; j++)
			{
				int num7 = num6 + j;
				int num8 = num7;
				if (num8 >= this.dataCount)
				{
					break;
				}
				int num9 = num8 % this.mMaxCount;
				if (num9 >= 0 && num9 < this.gridItems.Length)
				{
					if (this.gridItems[num9] == null)
					{
						this.gridItems[num9] = this.CreateGridItem();
					}
					if (!this.gridItems[num9].gameObject.activeInHierarchy)
					{
						this.gridItems[num9].gameObject.SetActive(true);
					}
					this.gridItems[num9].transform.localPosition = new Vector3(this.xOffset + (float)num7 * (this.cellWidth + this.gapWidth), -this.yOffset, this.gridItems[num9].transform.localPosition.z);
					this.gridItems[num9].Refresh(this.mDatas[num8]);
				}
			}
		}
		else if (this.arrangement == UICustomGrid.Arrangement.Vertical)
		{
			float num10 = this.mScrollView.panel.transform.localPosition.y - this.originPanelY - this.gapHeight;
			int num11 = Mathf.FloorToInt(num10 / (this.cellHeight + this.gapHeight));
			if (num11 < 0)
			{
				num11 = 0;
			}
			for (int k = 0; k < this.mMaxCount; k++)
			{
				int num12 = num11 + k;
				for (int l = 0; l < this.maxPerLine; l++)
				{
					int num13 = num12 * this.maxPerLine + l;
					if (num13 >= this.dataCount)
					{
						break;
					}
					int num14 = num13 % (this.mMaxCount * this.maxPerLine);
					if (num14 >= 0 && num14 < this.gridItems.Length)
					{
						if (this.gridItems[num14] == null)
						{
							this.gridItems[num14] = this.CreateGridItem();
						}
						if (!this.gridItems[num14].gameObject.activeInHierarchy)
						{
							this.gridItems[num14].gameObject.SetActive(true);
						}
						this.gridItems[num14].transform.localPosition = new Vector3(this.xOffset + (float)l * (this.cellWidth + this.gapWidth), -this.yOffset - (float)num12 * (this.cellHeight + this.gapHeight), 0f);
						this.gridItems[num14].Refresh(this.mDatas[num13]);
					}
				}
			}
		}
		else if (this.arrangement == UICustomGrid.Arrangement.VerticalReverse)
		{
			float num15 = Mathf.Abs(this.mScrollView.panel.transform.localPosition.y - this.originPanelY - this.gapHeight);
			int num16 = Mathf.FloorToInt(num15 / (this.cellHeight + this.gapHeight));
			if (num16 < 0)
			{
				num16 = 0;
			}
			for (int m = 0; m < this.mMaxCount; m++)
			{
				int num17 = num16 + m;
				for (int n = 0; n < this.maxPerLine; n++)
				{
					int num18 = num17 * this.maxPerLine + n;
					if (num18 >= this.dataCount)
					{
						break;
					}
					int num19 = num18 % (this.mMaxCount * this.maxPerLine);
					if (num19 >= 0 && num19 < this.gridItems.Length)
					{
						if (this.gridItems[num19] == null)
						{
							this.gridItems[num19] = this.CreateGridItem();
						}
						if (!this.gridItems[num19].gameObject.activeInHierarchy)
						{
							this.gridItems[num19].gameObject.SetActive(true);
						}
						this.gridItems[num19].transform.localPosition = new Vector3(this.xOffset + (float)n * (this.cellWidth + this.gapWidth), -this.yOffset + (float)num17 * (this.cellHeight + this.gapHeight), 0f);
						this.gridItems[num19].Refresh(this.mDatas[num18]);
					}
				}
			}
		}
	}

	public void OnClipMoved(UIPanel panel)
	{
		if (!this.mInitDone)
		{
			return;
		}
		this.Reposition(false);
	}

	private void UpdateBounds(int count)
	{
		Vector3 center = default(Vector3);
		if (this.arrangement == UICustomGrid.Arrangement.Horizontal)
		{
			center.x = base.transform.localPosition.x + (float)count * (this.cellWidth + this.gapWidth) + this.gapWidth;
			center.y = -base.transform.localPosition.y;
			center.z = base.transform.localPosition.z;
			Bounds bounds = new Bounds(center, Vector3.one);
			bounds.Encapsulate(base.transform.localPosition);
			this.mScrollView.bounds = bounds;
		}
		else if (this.arrangement == UICustomGrid.Arrangement.Vertical)
		{
			center.x = -base.transform.localPosition.x;
			center.y = base.transform.localPosition.y - (float)((count + this.maxPerLine - 1) / this.maxPerLine) * (this.cellHeight + this.gapHeight) + this.gapHeight;
			center.z = base.transform.localPosition.z;
			Bounds bounds2 = new Bounds(center, Vector3.one);
			bounds2.Encapsulate(base.transform.localPosition);
			this.mScrollView.bounds = bounds2;
		}
		else if (this.arrangement == UICustomGrid.Arrangement.VerticalReverse)
		{
			center.x = -base.transform.localPosition.x;
			center.y = base.transform.localPosition.y + (float)((count + this.maxPerLine - 1) / this.maxPerLine) * (this.cellHeight + this.gapHeight) + this.gapHeight;
			center.z = base.transform.localPosition.z;
			Bounds bounds3 = new Bounds(center, Vector3.one);
			bounds3.Encapsulate(base.transform.localPosition);
			this.mScrollView.bounds = bounds3;
		}
		this.mScrollView.UpdateScrollbars(true);
		this.mScrollView.RestrictWithinBounds(true);
	}

	public void ConstraintSort()
	{
		this.Sort(this.mDatas);
	}

	protected virtual void Sort(List<BaseData> list)
	{
	}

	protected virtual void OnUICenterOnChild(GameObject centeredOb)
	{
	}

	protected virtual void OnUICenterOnChildFinished()
	{
	}

	public void ResetBGScrollView()
	{
		if (this.bgScrollView != null)
		{
			this.bgScrollView.scrollView = this.mScrollView;
		}
	}
}
