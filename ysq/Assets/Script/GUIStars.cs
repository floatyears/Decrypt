using System;
using UnityEngine;

public class GUIStars : MonoBehaviour
{
	private UISprite[] mStars;

	public void Init(int num = 5)
	{
		this.mStars = new UISprite[num];
		int i = 0;
		while (i < base.transform.childCount && i < num)
		{
			this.mStars[i] = base.transform.GetChild(i).GetComponent<UISprite>();
			i++;
		}
		while (i < base.transform.childCount)
		{
			base.transform.GetChild(i).gameObject.SetActive(false);
			i++;
		}
	}

	public void Refresh(int stars)
	{
		int i = 0;
		while (i < stars && i < this.mStars.Length)
		{
			if (this.mStars[i] != null)
			{
				this.mStars[i].spriteName = "star";
			}
			i++;
		}
		while (i < this.mStars.Length)
		{
			if (this.mStars[i] != null)
			{
				this.mStars[i].spriteName = "starBg";
			}
			i++;
		}
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public void Show()
	{
		base.gameObject.SetActive(true);
	}
}
