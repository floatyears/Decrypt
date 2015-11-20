using System;
using UnityEngine;

public class UIVIPDescItem : MonoBehaviour
{
	private int openLevel;

	private UISprite mCheck;

	private void Awake()
	{
		base.gameObject.SetActive(false);
	}

	public void Init(int openLevel, string info)
	{
		base.gameObject.SetActive(true);
		this.openLevel = openLevel;
		this.mCheck = GameUITools.FindUISprite("Check", base.gameObject);
		UILabel uILabel = GameUITools.FindUILabel("Info", base.gameObject);
		UILabel uILabel2 = GameUITools.FindUILabel("Level", base.gameObject);
		uILabel2.text = Singleton<StringManager>.Instance.GetString("vipReward", new object[]
		{
			openLevel
		});
		uILabel.text = info;
	}

	public void Refresh(int level)
	{
		if (!base.gameObject.activeInHierarchy || this.openLevel == 0)
		{
			return;
		}
		if (this.openLevel > level)
		{
			this.mCheck.spriteName = "select";
		}
		else
		{
			this.mCheck.spriteName = "selected";
		}
	}
}
