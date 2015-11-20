using System;
using UnityEngine;

public class TutorialInitParams
{
	private GameObject maskParent;

	private int depth;

	private string targetName;

	private GameObject targetParent;

	private string handSpriteName;

	private TutorialEntity.ETutorialHandDirection handDirection;

	private Vector3 handPosition = Vector3.zero;

	private bool isInitGuideAnimImmediate = true;

	private bool isRemovePanel;

	private GameObject targetObj;

	private float hideGuideMask4Seconds;

	private bool slightTutorial;

	private Vector3 animationPosition = Vector3.zero;

	private bool hideTargetObj;

	private bool freeTutorial;

	private int panelRenderQueue;

	private float scaleFactor = 1f;

	private string tips;

	private int createObjIntervalFrame;

	private float cloneScale = -1f;

	public float CloneScale
	{
		get
		{
			return this.cloneScale;
		}
		set
		{
			this.cloneScale = value;
		}
	}

	public bool IsInitGuideAnimImmediate
	{
		get
		{
			return this.isInitGuideAnimImmediate;
		}
		set
		{
			this.isInitGuideAnimImmediate = value;
		}
	}

	public string Tips
	{
		get
		{
			return this.tips;
		}
		set
		{
			this.tips = value;
		}
	}

	public float ScaleFactor
	{
		get
		{
			return this.scaleFactor;
		}
		set
		{
			this.scaleFactor = value;
		}
	}

	public int PanelRenderQueue
	{
		get
		{
			return this.panelRenderQueue;
		}
		set
		{
			this.panelRenderQueue = value;
		}
	}

	public bool FreeTutorial
	{
		get
		{
			return this.freeTutorial;
		}
		set
		{
			this.freeTutorial = value;
		}
	}

	public bool HideTargetObj
	{
		get
		{
			return this.hideTargetObj;
		}
		set
		{
			this.hideTargetObj = value;
		}
	}

	public Vector3 AnimationPosition
	{
		get
		{
			return this.animationPosition;
		}
		set
		{
			this.animationPosition = value;
		}
	}

	public TutorialEntity.ETutorialHandDirection HandDirection
	{
		get
		{
			return this.handDirection;
		}
		set
		{
			this.handDirection = value;
		}
	}

	public Vector3 HandPosition
	{
		get
		{
			return this.handPosition;
		}
		set
		{
			this.handPosition = value;
		}
	}

	public bool SlightTutorial
	{
		get
		{
			return this.slightTutorial;
		}
		set
		{
			this.slightTutorial = value;
		}
	}

	public GameObject MaskParent
	{
		get
		{
			return this.maskParent;
		}
		set
		{
			this.maskParent = value;
		}
	}

	public int Depth
	{
		get
		{
			return this.depth;
		}
		set
		{
			this.depth = value;
		}
	}

	public string TargetName
	{
		get
		{
			return this.targetName;
		}
		set
		{
			this.targetName = value;
		}
	}

	public GameObject TargetParent
	{
		get
		{
			return this.targetParent;
		}
		set
		{
			this.targetParent = value;
		}
	}

	public bool IsRemovePanel
	{
		get
		{
			return this.isRemovePanel;
		}
		set
		{
			this.isRemovePanel = value;
		}
	}

	public GameObject TargetObj
	{
		get
		{
			return this.targetObj;
		}
		set
		{
			this.targetObj = value;
		}
	}

	public string HandSpriteName
	{
		get
		{
			return this.handSpriteName;
		}
		set
		{
			this.handSpriteName = value;
		}
	}

	public float HideGuideMask4Seconds
	{
		get
		{
			return this.hideGuideMask4Seconds;
		}
		set
		{
			this.hideGuideMask4Seconds = value;
		}
	}

	public int CreateObjIntervalFrame
	{
		get
		{
			return this.createObjIntervalFrame;
		}
		set
		{
			this.createObjIntervalFrame = value;
		}
	}

	public bool hasNullNecessaryParam()
	{
		if (this.maskParent == null)
		{
			global::Debug.LogError(new object[]
			{
				"Miss TutorialInitParam maskParent"
			});
			return true;
		}
		if (this.handDirection == TutorialEntity.ETutorialHandDirection.ETHD_Null)
		{
			global::Debug.LogError(new object[]
			{
				"Miss TutorialInitParam Hand's direction"
			});
			return true;
		}
		return false;
	}
}
