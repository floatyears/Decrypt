using System;
using UnityEngine;

public class UICustomScrollView : UIScrollView
{
	private int mArrangeMent;

	public override Bounds bounds
	{
		get
		{
			return this.mBounds;
		}
		set
		{
			this.mBounds = value;
		}
	}

	public void Init(int arrange)
	{
		this.mArrangeMent = arrange;
	}

	public override void SetDragAmount(float x, float y, bool updateScrollbars)
	{
		if (this.mArrangeMent == 2)
		{
			if (this.mPanel == null)
			{
				this.mPanel = base.GetComponent<UIPanel>();
			}
			base.DisableSpring();
			Bounds bounds = this.bounds;
			if (bounds.min.x == bounds.max.x || bounds.min.y == bounds.max.y)
			{
				return;
			}
			Vector4 finalClipRegion = this.mPanel.finalClipRegion;
			float num = finalClipRegion.w * 0.5f;
			float num2 = bounds.min.y + num;
			float num3 = bounds.max.y - num;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num2 -= this.mPanel.clipSoftness.y;
				num3 += this.mPanel.clipSoftness.y;
			}
			float num4 = Mathf.Lerp(num2, num3, y);
			if (!updateScrollbars)
			{
				Vector3 localPosition = this.mTrans.localPosition;
				if (base.canMoveVertically)
				{
					localPosition.y += finalClipRegion.y - num4;
				}
				this.mTrans.localPosition = localPosition;
			}
			if (base.canMoveVertically)
			{
				finalClipRegion.y = num4;
			}
			Vector4 baseClipRegion = this.mPanel.baseClipRegion;
			this.mPanel.clipOffset = new Vector2(finalClipRegion.x - baseClipRegion.x, finalClipRegion.y - baseClipRegion.y);
			if (updateScrollbars)
			{
				this.UpdateScrollbars(this.mDragID == -10);
			}
		}
		else
		{
			base.SetDragAmount(x, y, updateScrollbars);
		}
	}
}
