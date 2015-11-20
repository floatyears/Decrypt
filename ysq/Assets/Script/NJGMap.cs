using NJG;
using System;
using UnityEngine;

[AddComponentMenu("NJG MiniMap/NGUI/Map"), ExecuteInEditMode]
public class NJGMap : NJGMapBase
{
	private static NJGMap mInst;

	public UIAtlas atlas;

	public UISpriteData defaultSprite;

	private UICamera uiCam;

	public new static NJGMap instance
	{
		get
		{
			return NJGMap.mInst;
		}
	}

	public override bool isMouseOver
	{
		get
		{
			return UICamera.hoveredObject != null || UICamera.inputHasFocus || base.isMouseOver;
		}
	}

	public UISpriteData GetSprite(int type)
	{
		if (this.atlas == null)
		{
			global::Debug.LogWarning(new object[]
			{
				"You need to assign an atlas",
				this
			});
			return null;
		}
		return (base.Get(type) != null) ? this.atlas.GetSprite(base.Get(type).sprite) : this.defaultSprite;
	}

	public UISpriteData GetSpriteBorder(int type)
	{
		if (this.atlas == null)
		{
			global::Debug.LogWarning(new object[]
			{
				"You need to assign an atlas",
				this
			});
			return null;
		}
		return null;
	}

	public UISpriteData GetArrowSprite(int type)
	{
		if (this.atlas == null)
		{
			global::Debug.LogWarning(new object[]
			{
				"You need to assign an atlas",
				this
			});
			return null;
		}
		return (base.Get(type) != null) ? this.atlas.GetSprite(base.Get(type).arrowSprite) : this.defaultSprite;
	}

	protected override void Awake()
	{
		base.Awake();
		NJGMap.mInst = this;
	}

	private void OnDestroy()
	{
		if (UIMiniMap.instance != null)
		{
			UIMiniMap.instance.material.mainTexture = null;
		}
		if (this.mapTexture != null)
		{
			NJGTools.Destroy(this.mapTexture);
		}
		this.mapTexture = null;
	}
}
