using System;
using UnityEngine;

public class MagicLoveTouchPos : MonoBehaviour
{
	private GUIMagicLoveScene mBaseScene;

	private int mOffset;

	public void Init(GUIMagicLoveScene basescene, int offset)
	{
		this.mBaseScene = basescene;
		this.mOffset = offset;
	}

	private void OnClick()
	{
		this.mBaseScene.OnPosClick(this.mOffset);
	}

	private void OnDragStart()
	{
		this.mBaseScene.SetPetsLight();
	}

	private void OnDrag(Vector2 delta)
	{
		this.mBaseScene.RotateContent(delta.x);
	}

	private void OnDragEnd()
	{
		this.mBaseScene.CenterOn();
	}
}
