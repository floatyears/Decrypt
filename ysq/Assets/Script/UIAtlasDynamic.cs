using System;
using System.Collections.Generic;
using UnityEngine;

public class UIAtlasDynamic : UIAtlas
{
	public static List<UIAtlasDynamic> list = new List<UIAtlasDynamic>();

	private List<UIAtlas> atlasArray = new List<UIAtlas>();

	private UIAtlas LoadAtlas(int index)
	{
		GameObject gameObject = Res.Load<GameObject>(string.Format("Atlas/{0}{1}", base.name, index), false);
		if (gameObject == null)
		{
			return null;
		}
		return gameObject.GetComponent<UIAtlas>();
	}

	public static void ClearAllCachedAtlas()
	{
		for (int i = 0; i < UIAtlasDynamic.list.Count; i++)
		{
			UIAtlasDynamic.list[i].atlasArray.Clear();
		}
	}

	public override UIAtlas OnInit(string spriteName)
	{
		if (string.IsNullOrEmpty(spriteName))
		{
			return this;
		}
		if (Application.isPlaying)
		{
			if (!UIAtlasDynamic.list.Contains(this))
			{
				UIAtlasDynamic.list.Add(this);
			}
			for (int i = 0; i < this.atlasArray.Count; i++)
			{
				UIAtlas uIAtlas = this.atlasArray[i];
				if (uIAtlas == null)
				{
					uIAtlas = this.LoadAtlas(i);
					this.atlasArray[i] = uIAtlas;
				}
				UISpriteData sprite = uIAtlas.GetSprite(spriteName);
				if (sprite != null)
				{
					return uIAtlas;
				}
			}
			for (int j = this.atlasArray.Count; j < 2; j++)
			{
				UIAtlas uIAtlas2 = this.LoadAtlas(j);
				if (uIAtlas2 == null)
				{
					break;
				}
				this.atlasArray.Add(uIAtlas2);
				UISpriteData sprite2 = uIAtlas2.GetSprite(spriteName);
				if (sprite2 != null)
				{
					return uIAtlas2;
				}
			}
		}
		return this;
	}
}
