using NJG;
using System;
using UnityEngine;

[AddComponentMenu("NJG MiniMap/NGUI/Minimap"), ExecuteInEditMode, RequireComponent(typeof(UIAnchor))]
public class UIMiniMap : UIMiniMapBase
{
	private static UIMiniMap mInst;

	public UISprite overlay;

	private UIAnchor mAnchor;

	public static Action<Vector3> onMapDoubleClick;

	private Vector3 mClickOffset;

	public static UIMiniMap instance
	{
		get
		{
			return UIMiniMap.mInst;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		UIMiniMap.mInst = this;
	}

	protected override void Start()
	{
		if (this.map == null)
		{
			return;
		}
		this.mAnchor = base.GetComponent<UIAnchor>();
		base.Start();
		this.UpdateAlignment();
	}

	protected override void OnStart()
	{
		base.OnStart();
	}

	protected override void UpdateZoomKeys()
	{
	}

	protected override void UpdateKeys()
	{
		if (!UICamera.inputHasFocus && Input.GetKeyDown(this.lockKey))
		{
			this.rotateWithPlayer = !this.rotateWithPlayer;
		}
	}

	private void OnHover(bool isOver)
	{
		this.isMouseOver = isOver;
	}

	protected override UIMapIconBase GetEntry(NJGMapItem item)
	{
		int i = 0;
		int count = this.mList.Count;
		while (i < count)
		{
			UIMapIcon uIMapIcon = (UIMapIcon)this.mList[i];
			if (uIMapIcon.item.Equals(item))
			{
				return uIMapIcon;
			}
			i++;
		}
		if (this.mUnused.Count > 0)
		{
			UIMapIcon uIMapIcon2 = (UIMapIcon)this.mUnused[this.mUnused.Count - 1];
			uIMapIcon2.item = item;
			uIMapIcon2.sprite.spriteName = NJGMap.instance.GetSprite(item.type).name;
			uIMapIcon2.sprite.depth = 1 + item.depth;
			uIMapIcon2.sprite.color = item.color;
			uIMapIcon2.sprite.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
			if (uIMapIcon2.sprite.localSize != (Vector2)item.iconScale)
			{
				if (uIMapIcon2.collider != null)
				{
					uIMapIcon2.collider.size = item.iconScale;
				}
				uIMapIcon2.sprite.width = (int)item.iconScale.x;
				uIMapIcon2.sprite.height = (int)item.iconScale.y;
			}
			UISpriteData spriteBorder = NJGMap.instance.GetSpriteBorder(item.type);
			if (spriteBorder != null && uIMapIcon2.border != null)
			{
				uIMapIcon2.border.spriteName = spriteBorder.name;
				uIMapIcon2.border.depth = 1 + item.depth;
				uIMapIcon2.border.color = item.color;
                if (uIMapIcon2.border.localSize != (Vector2)item.borderScale)
				{
					uIMapIcon2.border.width = (int)item.borderScale.x;
					uIMapIcon2.border.height = (int)item.borderScale.y;
				}
			}
			this.mUnused.RemoveAt(this.mUnused.Count - 1);
			NGUITools.SetActive(uIMapIcon2.gameObject, true);
			this.mList.Add(uIMapIcon2);
			return uIMapIcon2;
		}
		GameObject gameObject = NGUITools.AddChild(base.iconRoot.gameObject);
		gameObject.name = "Icon" + this.mCount;
		UISprite uISprite = NGUITools.AddWidget<UISprite>(gameObject);
		uISprite.name = "Icon";
		uISprite.depth = 1 + item.depth;
		uISprite.atlas = NJGMap.instance.atlas;
		uISprite.spriteName = NJGMap.instance.GetSprite(item.type).name;
		uISprite.color = item.color;
		uISprite.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
		uISprite.width = (int)item.iconScale.x;
		uISprite.height = (int)item.iconScale.y;
		UIMapIcon uIMapIcon3 = gameObject.AddComponent<UIMapIcon>();
		uIMapIcon3.item = item;
		uIMapIcon3.sprite = uISprite;
		if (item.interaction)
		{
			if (uIMapIcon3.collider == null)
			{
				NGUITools.AddWidgetCollider(gameObject);
				uIMapIcon3.collider = gameObject.GetComponent<BoxCollider>();
				uIMapIcon3.collider.size = item.iconScale;
			}
			UISpriteData spriteBorder = NJGMap.instance.GetSpriteBorder(item.type);
			if (spriteBorder != null)
			{
				UISprite uISprite2 = NGUITools.AddWidget<UISprite>(gameObject);
				uISprite2.name = "Selection";
				uISprite2.depth = item.depth + 2;
				uISprite2.atlas = NJGMap.instance.atlas;
				uISprite2.spriteName = spriteBorder.name;
				uISprite2.color = item.color;
				uISprite2.width = (int)item.borderScale.x;
				uISprite2.height = (int)item.borderScale.y;
				uIMapIcon3.border = uISprite2;
			}
		}
		if (uIMapIcon3 == null)
		{
			global::Debug.LogError(new object[]
			{
				"Expected to find a Game Map Icon on the prefab to work with",
				this
			});
			UnityEngine.Object.Destroy(gameObject);
		}
		else
		{
			this.mCount++;
			uIMapIcon3.item = item;
			this.mList.Add(uIMapIcon3);
		}
		return uIMapIcon3;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.shaderType == NJGMapBase.ShaderType.TextureMask)
		{
			if (this.mMask != this.maskTexture)
			{
				this.mMask = this.maskTexture;
				if (this.material != null)
				{
					this.material.SetTexture("_Mask", this.mMask);
				}
			}
		}
		else if (this.material != null)
		{
			this.material.SetColor("_MaskColor", NJGMapBase.instance.cameraBackgroundColor);
		}
	}

	public override void UpdateAlignment()
	{
		base.UpdateAlignment();
		if (this.mAnchor == null)
		{
			this.mAnchor = base.GetComponent<UIAnchor>();
		}
		if (this.mAnchor != null)
		{
			this.mAnchor.side = (UIAnchor.Side)this.pivot;
		}
		if (base.iconRoot != null)
		{
			base.iconRoot.localPosition = new Vector3(this.rendererTransform.localPosition.x, this.rendererTransform.localPosition.y, 1f);
		}
		if (this.overlay != null && NJGMap.instance.atlas != null)
		{
			UISpriteData sprite = NJGMap.instance.atlas.GetSprite(this.overlay.spriteName);
			if (sprite != null)
			{
				Vector4 border = this.overlay.border;
				this.overlay.width = (int)this.mapScale.x + (int)(border.x + border.z + (float)this.overlayBorderOffset);
				this.overlay.height = (int)this.mapScale.y + (int)(border.y + border.w + (float)this.overlayBorderOffset);
			}
		}
	}

	public void OnDoubleClick()
	{
        this.mClickOffset = UICamera.currentTouch.pos - (Vector2)base.cachedTransform.position;
		this.mClickOffset.x = Mathf.Abs(this.mClickOffset.x);
		this.mClickOffset.y = -Mathf.Abs(this.mClickOffset.y);
		if (UIMiniMap.onMapDoubleClick != null)
		{
			UIMiniMap.onMapDoubleClick(this.mClickPos);
		}
	}

	public override UIMapArrowBase GetArrow(UnityEngine.Object o)
	{
		NJGMapItem nJGMapItem = (NJGMapItem)o;
		int i = 0;
		int count = this.mListArrow.Count;
		while (i < count)
		{
			if (this.mListArrow[i].item == nJGMapItem)
			{
				return (UIMapArrow)this.mListArrow[i];
			}
			i++;
		}
		if (this.mUnusedArrow.Count > 0)
		{
			UIMapArrow uIMapArrow = (UIMapArrow)this.mUnusedArrow[this.mUnusedArrow.Count - 1];
			uIMapArrow.item = nJGMapItem;
			uIMapArrow.child = uIMapArrow.sprite.cachedTransform;
			uIMapArrow.sprite.spriteName = NJGMap.instance.GetArrowSprite(nJGMapItem.type).name;
			uIMapArrow.sprite.depth = 1 + nJGMapItem.arrowDepth;
			uIMapArrow.sprite.color = nJGMapItem.color;
			uIMapArrow.sprite.width = (int)this.arrowScale.x;
			uIMapArrow.sprite.height = (int)this.arrowScale.y;
			uIMapArrow.sprite.cachedTransform.localPosition = new Vector3(0f, this.mapScale.y / 2f - (float)nJGMapItem.arrowOffset, 0f);
			this.mUnusedArrow.RemoveAt(this.mUnusedArrow.Count - 1);
			NGUITools.SetActive(uIMapArrow.gameObject, true);
			this.mListArrow.Add(uIMapArrow);
			return uIMapArrow;
		}
		GameObject gameObject = NGUITools.AddChild(this.rendererTransform.parent.gameObject);
		gameObject.name = "Arrow" + this.mArrowCount;
		gameObject.transform.parent = UIMiniMap.instance.arrowRoot.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		UISprite uISprite = NGUITools.AddWidget<UISprite>(gameObject);
		uISprite.depth = 1 + nJGMapItem.arrowDepth;
		uISprite.atlas = NJGMap.instance.atlas;
		uISprite.spriteName = NJGMap.instance.GetArrowSprite(nJGMapItem.type).name;
		uISprite.color = nJGMapItem.color;
		uISprite.width = (int)this.arrowScale.x;
		uISprite.height = (int)this.arrowScale.y;
		uISprite.cachedTransform.localPosition = new Vector3(0f, this.rendererTransform.localScale.y / 2f - (float)nJGMapItem.arrowOffset, 0f);
		UIMapArrow uIMapArrow2 = gameObject.AddComponent<UIMapArrow>();
		uIMapArrow2.child = uISprite.cachedTransform;
		uIMapArrow2.child.localEulerAngles = new Vector3(0f, 180f, 0f);
		uIMapArrow2.item = nJGMapItem;
		uIMapArrow2.sprite = uISprite;
		if (uIMapArrow2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"Expected to find a UIMapArrow on the prefab to work with"
			});
			UnityEngine.Object.Destroy(gameObject);
		}
		else
		{
			this.mArrowCount++;
			uIMapArrow2.item = nJGMapItem;
			this.mListArrow.Add(uIMapArrow2);
		}
		return uIMapArrow2;
	}
}
