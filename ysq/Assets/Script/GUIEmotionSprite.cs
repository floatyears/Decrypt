using System;
using UnityEngine;

public class GUIEmotionSprite : MonoBehaviour
{
	private UISprite mEmotion;

	public void InitObjects()
	{
		this.CreateObjects();
	}

	public int GetSpriteWidth()
	{
		return this.mEmotion.width;
	}

	private void CreateObjects()
	{
		this.mEmotion = base.transform.GetComponent<UISprite>();
	}

	public void InitEmotion(string emotionTag)
	{
		char[] trimChars = new char[]
		{
			'<',
			'>'
		};
		this.mEmotion.spriteName = emotionTag.Trim(trimChars);
	}
}
