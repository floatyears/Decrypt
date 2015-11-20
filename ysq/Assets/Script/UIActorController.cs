using System;
using UnityEngine;

public sealed class UIActorController : MonoBehaviour
{
	public delegate void VoidCallBack();

	public UIActorController.VoidCallBack ClickEvent;

	public bool PlayAction = true;

	public bool CanRotate = true;

	public int Gender;

	public string UIAction = string.Empty;

	public bool IsPlayer;

	public bool PlayFaintAction = true;

	public string UIFaintAction = "Skill/U1001";

	private float rotateAccum;

	private static float voiceCD;

	private bool isDancing;

	private bool initFlag;

	private string requestAnim = string.Empty;

	private string requestAnimQueue = string.Empty;

	private Animation animCtrl;

	public Animation AnimCtrl
	{
		get
		{
			return this.animCtrl;
		}
	}

	private void Awake()
	{
		ModelController component = base.GetComponent<ModelController>();
		if (component == null)
		{
			this.animCtrl = base.animation;
		}
		else
		{
			this.animCtrl = component.GetAnimation();
		}
		if (this.animCtrl == null)
		{
			return;
		}
		this.animCtrl["idle"].wrapMode = WrapMode.Once;
		this.animCtrl["std"].wrapMode = WrapMode.Loop;
		if (this.animCtrl["win"] != null)
		{
			this.animCtrl["win"].wrapMode = WrapMode.Once;
		}
		if (this.animCtrl["atk1"] != null)
		{
			this.animCtrl["atk1"].wrapMode = WrapMode.Once;
		}
		if (this.animCtrl["dance"] != null)
		{
			this.animCtrl["dance"].wrapMode = WrapMode.Loop;
		}
		if (this.animCtrl["faint"] != null)
		{
			this.animCtrl["faint"].wrapMode = WrapMode.Loop;
		}
		if (this.animCtrl["idle02"] != null)
		{
			this.animCtrl["idle02"].wrapMode = WrapMode.Once;
		}
		if (this.animCtrl["idle03"] != null)
		{
			this.animCtrl["idle03"].wrapMode = WrapMode.Once;
		}
		if (this.animCtrl["run"] != null)
		{
			this.animCtrl["run"].wrapMode = WrapMode.Loop;
		}
	}

	private void Start()
	{
		if (this.animCtrl == null)
		{
			return;
		}
		this.animCtrl.clip = null;
		this.InitAnimation();
	}

	private void OnEnable()
	{
		this.InitAnimation();
	}

	private void OnDisable()
	{
		this.initFlag = false;
	}

	private void InitAnimation()
	{
		if (this.animCtrl == null || this.initFlag)
		{
			return;
		}
		this.initFlag = true;
		if (!string.IsNullOrEmpty(this.requestAnim))
		{
			this.animCtrl.Play(this.requestAnim);
			this.requestAnim = string.Empty;
			if (!string.IsNullOrEmpty(this.requestAnimQueue))
			{
				this.animCtrl.CrossFadeQueued(this.requestAnimQueue);
				this.requestAnimQueue = string.Empty;
			}
			else
			{
				this.animCtrl.CrossFadeQueued("std");
			}
		}
		else
		{
			this.animCtrl.Play("std");
		}
	}

	private void OnClick()
	{
		if (this.ClickEvent == null)
		{
			this.PlayIdleAnimationAndVoice();
		}
		else
		{
			this.ClickEvent();
		}
	}

	public void PlayIdleAnimationAndVoice()
	{
		int num = this.PlayIdleAnimation();
		if (num >= 0)
		{
			this.PlayVoice(num);
		}
	}

	public int PlayIdleAnimation()
	{
		if (!this.PlayAction)
		{
			return -1;
		}
		string[] array = new string[]
		{
			"idle",
			"win"
		};
		int num;
		if (string.IsNullOrEmpty(this.UIAction))
		{
			num = UtilFunc.RangeRandom(0, 100) % array.Length;
		}
		else
		{
			num = UtilFunc.RangeRandom(0, 100) % (array.Length + 1);
		}
		if (num >= array.Length)
		{
			this.PlayShowAction(this.UIAction);
			return 0;
		}
		this.requestAnim = array[num];
		this.requestAnimQueue = "std";
		return num;
	}

	public void PlayStdAnimation()
	{
		if (!this.PlayAction)
		{
			return;
		}
		this.isDancing = false;
		this.requestAnim = "std";
		this.requestAnimQueue = string.Empty;
	}

	public void PlayHiAnimation()
	{
		if (!this.PlayAction)
		{
			return;
		}
		string[] array = new string[]
		{
			"idle",
			"idle03"
		};
		string[] array2 = new string[]
		{
			"idle",
			"idle"
		};
		int num = UtilFunc.RangeRandom(0, 100) % 2;
		if (this.IsPlayer)
		{
			this.requestAnim = array[num];
		}
		else
		{
			this.requestAnim = array2[num];
		}
		if (this.isDancing)
		{
			if (this.IsPlayer)
			{
				this.requestAnimQueue = "dance";
			}
			else
			{
				this.requestAnimQueue = "win";
			}
		}
		else
		{
			this.requestAnimQueue = "std";
		}
		this.PlayVoice(num);
	}

	public void PlayRunAnimation()
	{
		if (!this.PlayAction)
		{
			return;
		}
		this.requestAnim = "run";
		this.requestAnimQueue = "std";
	}

	public void PlayDanceAnimation(bool once)
	{
		if (!this.PlayAction || this.animCtrl == null)
		{
			return;
		}
		if (once)
		{
			if (this.IsPlayer)
			{
				this.animCtrl["dance"].wrapMode = WrapMode.Once;
			}
			else
			{
				this.animCtrl["win"].wrapMode = WrapMode.Once;
			}
		}
		else if (this.IsPlayer)
		{
			this.animCtrl["dance"].wrapMode = WrapMode.Loop;
		}
		else
		{
			this.animCtrl["win"].wrapMode = WrapMode.Loop;
		}
		this.isDancing = !once;
		if (this.IsPlayer)
		{
			this.requestAnim = "dance";
		}
		else
		{
			this.requestAnim = "win";
		}
		this.requestAnimQueue = "std";
		this.PlayVoice(2);
	}

	public void PlayUIAction()
	{
		if (!string.IsNullOrEmpty(this.UIAction))
		{
			this.PlayShowAction(this.UIAction);
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (!this.CanRotate || !this.PlayAction)
		{
			return;
		}
		base.transform.Rotate(-Vector3.up * delta.x);
		if (this.PlayFaintAction)
		{
			this.rotateAccum += Mathf.Abs(delta.x);
			if (this.rotateAccum >= 720f)
			{
				this.PlayShowAction(this.UIFaintAction);
				this.rotateAccum = 0f;
			}
		}
	}

	private void Update()
	{
		if (this.PlayFaintAction && this.rotateAccum > 0f)
		{
			this.rotateAccum -= Time.deltaTime * 400f;
		}
		if (this.animCtrl == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.requestAnim))
		{
			this.animCtrl.CrossFade(this.requestAnim);
			this.requestAnim = string.Empty;
			if (!string.IsNullOrEmpty(this.requestAnimQueue))
			{
				this.animCtrl.CrossFadeQueued(this.requestAnimQueue);
				this.requestAnimQueue = string.Empty;
			}
		}
	}

	private void PlayShowAction(string actionName)
	{
		if (string.IsNullOrEmpty(actionName))
		{
			return;
		}
		GameObject gameObject = Res.Load<GameObject>(actionName, false);
		if (gameObject != null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, base.transform.position, base.transform.rotation) as GameObject;
			if (gameObject2 != null)
			{
				gameObject2.SendMessage("OnInit", this, SendMessageOptions.DontRequireReceiver);
				base.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
			}
		}
	}

	public void LockAction()
	{
		this.PlayAction = false;
	}

	public void UnlockAction()
	{
		this.PlayAction = true;
	}

	public void PlayVoice(int index)
	{
		if (Globals.Instance.EffectSoundMgr.pause || !EffectSoundManager.IsEffectVoiceOptionOn())
		{
			return;
		}
		if (Time.time >= UIActorController.voiceCD)
		{
			string text = string.Empty;
			if (this.IsPlayer)
			{
				if (this.Gender == 0)
				{
					text = string.Format("Voice/playerb_{0}", index + 1);
				}
				else
				{
					text = string.Format("Voice/playerg_{0}", index + 1);
				}
			}
			else
			{
				text = base.gameObject.name;
				int num = text.IndexOf("(Clone)");
				if (num > 0)
				{
					text = text.Substring(0, num - 1);
					text = "Voice/" + text;
				}
			}
			UIActorController.voiceCD = Time.time + Globals.Instance.EffectSoundMgr.PlayVoice(text, 1f, base.transform.position);
		}
	}
}
