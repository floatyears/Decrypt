using System;
using UnityEngine;

public class GUIRewardCard : MonoBehaviour
{
	private UITexture uiTex;

	private Texture2D tex;

	private int mWidth;

	private int mHeight;

	private int brushSize;

	private int area;

	private int transparentArea;

	private UILabel mNum;

	private Color[] cols;

	private GUIRewardScratchOffPop mBasePop;

	public bool IsInit;

	private float percent = 0.7f;

	public bool IsVisible;

	public void Init(GUIRewardScratchOffPop basepop, int brushSize, float per)
	{
		this.mBasePop = basepop;
		this.uiTex = base.gameObject.GetComponent<UITexture>();
		this.mNum = GameUITools.FindUILabel("Label", base.gameObject);
		this.percent = per;
		if (this.uiTex != null)
		{
			this.tex = (Texture2D)this.uiTex.mainTexture;
			this.mWidth = this.tex.width;
			this.mHeight = this.tex.height;
			this.brushSize = ((basepop.mBaseScene.mBaseScene.ScratchOffBrushSize <= 0) ? 10 : basepop.mBaseScene.mBaseScene.ScratchOffBrushSize);
			this.area = this.mWidth * this.mHeight;
			this.cols = this.tex.GetPixels();
		}
	}

	public void ReInit()
	{
		this.brushSize = ((this.mBasePop.mBaseScene.mBaseScene.ScratchOffBrushSize <= 0) ? 10 : this.mBasePop.mBaseScene.mBaseScene.ScratchOffBrushSize);
		this.percent = this.mBasePop.mBaseScene.mBaseScene.ScratchOffPercent;
		this.IsVisible = false;
		this.mNum.text = string.Empty;
		this.transparentArea = 0;
		this.Reset();
	}

	public void Reset()
	{
		if (this.uiTex != null)
		{
			this.tex.SetPixels(this.cols);
			this.tex.Apply();
		}
	}

	private void Update()
	{
		if (base.gameObject.activeInHierarchy && Input.GetMouseButton(0) && this.uiTex != null && UICamera.hoveredObject == base.gameObject)
		{
			if (string.IsNullOrEmpty(this.mNum.text))
			{
				this.mNum.text = this.mBasePop.mCurNums[this.mBasePop.hasScratchNum];
				this.mBasePop.mBaseScene.mAreaArr[this.mBasePop.hasScratchNum].Refresh(this.mNum.text);
				this.mBasePop.hasScratchNum++;
			}
			this.CheckPoint(Input.mousePosition);
		}
	}

	private bool CheckPoint(Vector3 pScreenPos)
	{
		Vector3 position = UICamera.mainCamera.ScreenToWorldPoint(pScreenPos);
		Vector3 vector = this.uiTex.gameObject.transform.InverseTransformPoint(position);
		if (vector.x > (float)(-(float)this.mWidth / 2) && vector.x < (float)(this.mWidth / 2) && vector.y > (float)(-(float)this.mHeight / 2) && vector.y < (float)(this.mHeight / 2))
		{
			for (int i = (int)vector.x - this.brushSize; i < (int)vector.x + this.brushSize; i++)
			{
				for (int j = (int)vector.y - this.brushSize; j < (int)vector.y + this.brushSize; j++)
				{
					if (Mathf.Pow((float)i - vector.x, 2f) + Mathf.Pow((float)j - vector.y, 2f) <= Mathf.Pow((float)this.brushSize, 2f))
					{
						Color pixel = this.tex.GetPixel(i + this.mWidth / 2, j + this.mHeight / 2);
						if (pixel.a != 0f)
						{
							pixel.a = 0f;
							this.transparentArea++;
							this.tex.SetPixel(i + this.mWidth / 2, j + this.mHeight / 2, pixel);
							if (!this.IsVisible && (float)this.transparentArea / (float)this.area > this.percent)
							{
								this.IsVisible = true;
								this.mBasePop.CheckIsAllVisible();
							}
						}
					}
				}
			}
			this.tex.Apply();
			return true;
		}
		return false;
	}

	public bool CheckIfVisible()
	{
		Color[] pixels = this.tex.GetPixels();
		if (pixels.Length == 0)
		{
			return true;
		}
		int num = 0;
		for (int i = 0; i < pixels.Length; i++)
		{
			if (pixels[i].a == 0f)
			{
				num++;
			}
		}
		return (float)num / (float)this.area > this.percent;
	}
}
