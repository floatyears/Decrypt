using System;
using UnityEngine;

public class GUIRewardArea : MonoBehaviour
{
	private UISprite mCover;

	private UILabel mNum;

	private void Awake()
	{
		this.mCover = base.gameObject.GetComponent<UISprite>();
		if (this.mCover == null)
		{
			global::Debug.LogError(new object[]
			{
				" activity ScratchOff Area find uisprite error"
			});
		}
		this.mNum = GameUITools.FindUILabel("Label", base.gameObject);
	}

	public void Refresh(string num = "")
	{
		this.mNum.text = num;
	}

	public void Init()
	{
		this.mCover.enabled = true;
		this.mNum.text = string.Empty;
	}

	public void ClearCover()
	{
		this.mCover.enabled = false;
	}
}
