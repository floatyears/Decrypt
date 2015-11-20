using System;
using UnityEngine;

public sealed class UICollectionScrollView : UIScrollView
{
	public override Bounds bounds
	{
		get
		{
			if (!this.mCalculatedBounds)
			{
				this.mCalculatedBounds = true;
				this.mTrans = base.transform;
				this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mTrans, this.mTrans, true, true);
			}
			return this.mBounds;
		}
	}
}
