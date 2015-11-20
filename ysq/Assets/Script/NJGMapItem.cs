using NJG;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NJG MiniMap/Map Item")]
public class NJGMapItem : MonoBehaviour
{
	public static List<NJGMapItem> list = new List<NJGMapItem>();

	public bool isRevealed;

	public bool revealFOW;

	public bool drawDirection;

	public string content = string.Empty;

	public int type;

	public int revealDistance;

	public UIMapArrowBase arrow;

	public bool showDeath;

	public Action<bool> onSelect;

	public bool showIcon;

	public bool isActive = true;

	private Color mColor = Color.clear;

	private bool mInteraction;

	private bool mRotate;

	private bool mArrowRotate;

	private bool mUpdatePos;

	private bool mAnimOnVisible;

	private bool mAnimOnAction;

	private bool mLoop;

	private bool mArrow;

	private float mFadeOut = -1f;

	private Vector2 mIconScale;

	private Vector2 mBorderScale;

	private int mIconSize = 2147483647;

	private int mSize = 2147483647;

	private int mBSize = 2147483647;

	private int mBorderSize = 2147483647;

	private int mDepth = 2147483647;

	private int mArrowDepth = 2147483647;

	private int mArrowOffset = 2147483647;

	private bool mInteractionSet;

	private bool mColorSet;

	private bool mRotateSet;

	private bool mArrowRotateSet;

	private bool mUpdatePosSet;

	private bool mAnimOnVisibleSet;

	private bool mAnimOnActionSet;

	private bool mLoopSet;

	private bool mArrowSet;

	private bool mForceSelect;

	private bool mSelected;

	private Transform mTrans;

	private NJGFOW.Revealer mRevealer;

	public Color color
	{
		get
		{
			if (!this.mColorSet)
			{
				this.mColor = NJGMapBase.instance.GetColor(this.type);
			}
			this.mColorSet = true;
			return this.mColor;
		}
	}

	public bool rotate
	{
		get
		{
			if (!this.mRotateSet)
			{
				this.mRotateSet = true;
				this.mRotate = NJGMapBase.instance.GetRotate(this.type);
			}
			return this.mRotate;
		}
	}

	public bool interaction
	{
		get
		{
			if (!this.mInteractionSet)
			{
				this.mInteractionSet = true;
				this.mInteraction = NJGMapBase.instance.GetInteraction(this.type);
			}
			return this.mInteraction;
		}
	}

	public bool arrowRotate
	{
		get
		{
			if (!this.mArrowRotateSet)
			{
				this.mArrowRotateSet = true;
				this.mArrowRotate = NJGMapBase.instance.GetArrowRotate(this.type);
			}
			return this.mArrowRotate;
		}
	}

	public bool updatePosition
	{
		get
		{
			if (!this.mUpdatePosSet)
			{
				this.mUpdatePosSet = true;
				this.mUpdatePos = NJGMapBase.instance.GetUpdatePosition(this.type);
			}
			return this.mUpdatePos;
		}
	}

	public bool animateOnVisible
	{
		get
		{
			if (!this.mAnimOnVisibleSet)
			{
				this.mAnimOnVisibleSet = true;
				this.mAnimOnVisible = NJGMapBase.instance.GetAnimateOnVisible(this.type);
			}
			return this.mAnimOnVisible;
		}
	}

	public bool showOnAction
	{
		get
		{
			if (!this.mAnimOnActionSet)
			{
				this.mAnimOnActionSet = true;
				this.mAnimOnAction = NJGMapBase.instance.GetAnimateOnAction(this.type);
			}
			return this.mAnimOnAction;
		}
	}

	public bool loopAnimation
	{
		get
		{
			if (!this.mLoopSet)
			{
				this.mLoopSet = true;
				this.mLoop = NJGMapBase.instance.GetLoopAnimation(this.type);
			}
			return this.mLoop;
		}
	}

	public bool haveArrow
	{
		get
		{
			if (!this.mArrowSet)
			{
				this.mArrowSet = true;
				this.mArrow = NJGMapBase.instance.GetHaveArrow(this.type);
			}
			return this.mArrow;
		}
	}

	public float fadeOutAfterDelay
	{
		get
		{
			if (this.mFadeOut == -1f)
			{
				this.mFadeOut = NJGMapBase.instance.GetFadeOutAfter(this.type);
			}
			return this.mFadeOut;
		}
	}

	public int size
	{
		get
		{
			if (NJGMapBase.instance.GetCustom(this.type))
			{
				this.mSize = NJGMapBase.instance.GetSize(this.type);
			}
			else
			{
				this.mSize = NJGMapBase.instance.iconSize;
			}
			return this.mSize;
		}
	}

	public int borderSize
	{
		get
		{
			if (NJGMapBase.instance.GetCustomBorder(this.type))
			{
				this.mBSize = NJGMapBase.instance.GetBorderSize(this.type);
			}
			else
			{
				this.mBSize = NJGMapBase.instance.borderSize;
			}
			return this.mBSize;
		}
	}

	public virtual Vector3 iconScale
	{
		get
		{
			if (this.mIconSize != this.size)
			{
				this.mIconSize = this.size;
				this.mIconScale.x = (this.mIconScale.y = (float)this.size);
			}
			return this.mIconScale;
		}
	}

	public virtual Vector3 borderScale
	{
		get
		{
			if (this.mBorderSize != this.borderSize)
			{
				this.mBorderSize = this.borderSize;
				this.mBorderScale.x = (this.mBorderScale.y = (float)this.borderSize);
			}
			return this.mBorderScale;
		}
	}

	public int depth
	{
		get
		{
			if (this.mDepth == 2147483647)
			{
				this.mDepth = NJGMapBase.instance.GetDepth(this.type);
			}
			return this.mDepth;
		}
	}

	public int arrowDepth
	{
		get
		{
			if (this.mArrowDepth == 2147483647)
			{
				this.mArrowDepth = NJGMapBase.instance.GetArrowDepth(this.type);
			}
			return this.mArrowDepth;
		}
	}

	public int arrowOffset
	{
		get
		{
			if (this.mArrowOffset == 2147483647)
			{
				this.mArrowOffset = NJGMapBase.instance.GetArrowOffset(this.type);
			}
			return this.mArrowOffset;
		}
	}

	public bool isSelected
	{
		get
		{
			return this.mSelected;
		}
		set
		{
			this.mSelected = value;
			if (this.onSelect != null)
			{
				this.onSelect(this.mSelected);
			}
		}
	}

	public bool forceSelection
	{
		get
		{
			return this.mForceSelect;
		}
		set
		{
			this.mForceSelect = value;
		}
	}

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	private void Start()
	{
		if (NJGMapBase.instance != null && this.revealFOW && NJGMapBase.instance.fow.enabled)
		{
			this.mRevealer = NJGFOW.CreateRevealer();
		}
	}

	private void OnEnable()
	{
		if (!NJGMapItem.list.Contains(this))
		{
			NJGMapItem.list.Add(this);
		}
	}

	private void OnDestroy()
	{
		if (Application.isPlaying)
		{
			if (NJGMapBase.instance != null && NJGMapBase.instance.fow.enabled)
			{
				NJGFOW.DeleteRevealer(this.mRevealer);
			}
			this.mRevealer = null;
			if (this.arrow != null)
			{
				UIMiniMapBase.inst.DeleteArrow(this.arrow);
			}
			this.arrow = null;
		}
		NJGMapItem.list.Remove(this);
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
		if (this.revealFOW)
		{
			if (NJGMapBase.instance == null || UIMiniMapBase.inst == null)
			{
				return;
			}
			if (this.mRevealer == null)
			{
				this.mRevealer = NJGFOW.CreateRevealer();
			}
			if (this.isActive)
			{
				this.mRevealer.pos = UIMiniMapBase.inst.WorldToMap(this.cachedTransform.position, false);
				this.mRevealer.revealDistance = ((this.revealDistance <= 0) ? NJGMapBase.instance.fow.revealDistance : this.revealDistance);
				this.mRevealer.isActive = true;
			}
			else
			{
				this.mRevealer.isActive = false;
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (this.revealFOW)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(base.transform.position, (float)this.revealDistance);
		}
	}

	public void Select()
	{
		this.mSelected = true;
	}

	public void Select(bool forceSelect)
	{
		this.mSelected = true;
		this.mForceSelect = forceSelect;
	}

	public void UnSelect()
	{
		this.mSelected = false;
	}

	public void Show()
	{
		this.showIcon = true;
	}
}
