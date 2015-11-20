using System;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
	public enum Type
	{
		OK,
		OKCancel,
		Custom1Btn,
		Custom2Btn
	}

	public delegate void MessageDelegate(object obj);

	public MessageBox.MessageDelegate OkClick;

	public MessageBox.MessageDelegate CancelClick;

	private string customTextOK;

	private string customTextCancel;

	private float delayOkTime;

	private float delayCancelTime;

	public bool CanCloseByFadeBGClicked = true;

	public bool IsCloseByFadeBGClicked;

	private UIPanel uiPanel;

	private UISprite messageBoxBg;

	private UILabel content;

	public UIButton btnOK;

	private UILabel textOK;

	private UIWidget widgetOK;

	private UIButton btnCancel;

	private UILabel textCancel;

	private UIWidget widgetCancel;

	private GameObject fadeBG;

	public string TextOK
	{
		get
		{
			return this.textOK.text;
		}
		set
		{
			this.textOK.text = value;
			this.customTextOK = value;
		}
	}

	public string TextCancel
	{
		get
		{
			return this.textCancel.text;
		}
		set
		{
			this.textCancel.text = value;
			this.customTextCancel = value;
		}
	}

	public int WidthOK
	{
		get
		{
			return this.widgetOK.width;
		}
		set
		{
			this.widgetOK.width = value;
		}
	}

	public int WidthCancel
	{
		get
		{
			return this.widgetCancel.width;
		}
		set
		{
			this.widgetCancel.width = value;
		}
	}

	public string Content
	{
		get
		{
			return this.content.text;
		}
		set
		{
			this.content.text = value;
		}
	}

	public int StartingRenderQueue
	{
		get
		{
			return this.uiPanel.startingRenderQueue;
		}
		set
		{
			this.uiPanel.startingRenderQueue = value;
		}
	}

	public UIWidget.Pivot ContentPivot
	{
		get
		{
			return this.content.pivot;
		}
		set
		{
			this.content.pivot = value;
		}
	}

	public float DelayOkTime
	{
		get
		{
			return this.delayOkTime;
		}
		set
		{
			this.delayOkTime = value;
		}
	}

	public float DelayCancelTime
	{
		get
		{
			return this.delayCancelTime;
		}
		set
		{
			this.delayCancelTime = value;
		}
	}

	public MessageBox.Type type
	{
		get;
		private set;
	}

	public object userData
	{
		get;
		private set;
	}

	private void Awake()
	{
		base.transform.localPosition = new Vector3(0f, 0f, 1800f);
		this.uiPanel = base.GetComponent<UIPanel>();
		this.messageBoxBg = base.transform.FindChild("WinBG").GetComponent<UISprite>();
		this.content = this.messageBoxBg.transform.FindChild("Content").GetComponent<UILabel>();
		this.btnOK = this.messageBoxBg.transform.FindChild("OK").GetComponent<UIButton>();
		this.textOK = this.btnOK.transform.FindChild("Label").GetComponent<UILabel>();
		this.widgetOK = this.btnOK.transform.GetComponent<UIWidget>();
		this.btnCancel = this.messageBoxBg.transform.FindChild("Cancel").GetComponent<UIButton>();
		this.textCancel = this.btnCancel.transform.FindChild("Label").GetComponent<UILabel>();
		this.widgetCancel = this.btnCancel.transform.GetComponent<UIWidget>();
		this.fadeBG = base.transform.FindChild("FadeBG").gameObject;
		UIEventListener expr_13D = UIEventListener.Get(this.btnOK.gameObject);
		expr_13D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_13D.onClick, new UIEventListener.VoidDelegate(this.OnOKClicked));
		UIEventListener expr_16E = UIEventListener.Get(this.btnCancel.gameObject);
		expr_16E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_16E.onClick, new UIEventListener.VoidDelegate(this.OnCancelClicked));
		UIEventListener expr_19A = UIEventListener.Get(this.fadeBG);
		expr_19A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_19A.onClick, new UIEventListener.VoidDelegate(this.OnFadeBGClicked));
	}

	private void OnDisable()
	{
		this.OkClick = null;
		this.CancelClick = null;
		this.userData = null;
	}

	private void DestroySelf()
	{
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}

	protected void Close()
	{
		this.btnOK.gameObject.collider.enabled = false;
		this.btnCancel.gameObject.collider.enabled = false;
		this.fadeBG.collider.enabled = false;
		GameUITools.PlayCloseWindowAnim(this.messageBoxBg.transform, null, false);
		base.Invoke("DestroySelf", 0.25f);
	}

	private void Update()
	{
		if (this.delayOkTime > 0f)
		{
			this.delayOkTime -= RealTime.deltaTime;
			if (this.delayOkTime <= 0f)
			{
				this.btnOK.isEnabled = true;
				if (!string.IsNullOrEmpty(this.customTextOK))
				{
					this.textOK.text = this.customTextOK;
				}
			}
			else
			{
				this.btnOK.isEnabled = false;
				if (!string.IsNullOrEmpty(this.customTextOK))
				{
					this.textOK.text = string.Format("{0}({1})", this.customTextOK, (int)(this.delayOkTime + 1f));
				}
			}
		}
		if (this.delayCancelTime > 0f)
		{
			this.delayCancelTime -= RealTime.deltaTime;
			if (this.delayCancelTime <= 0f)
			{
				this.btnCancel.isEnabled = true;
				if (!string.IsNullOrEmpty(this.customTextCancel))
				{
					this.textCancel.text = this.customTextCancel;
				}
			}
			else
			{
				this.btnCancel.isEnabled = false;
				if (!string.IsNullOrEmpty(this.customTextCancel))
				{
					this.textCancel.text = string.Format("{0}({1})", this.customTextCancel, (int)(this.delayOkTime + 1f));
				}
			}
		}
	}

	public void OnOKClicked(GameObject go)
	{
		if (Globals.Instance.GameMgr.Status != GameManager.EGameStatus.EGS_None)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		}
		this.Close();
		if (this.OkClick != null)
		{
			this.OkClick(this.userData);
		}
	}

	public void OnFadeBGClicked(GameObject go)
	{
		if (this.CanCloseByFadeBGClicked)
		{
			this.OnCancelClicked(go);
		}
		else if (this.IsCloseByFadeBGClicked)
		{
			this.Close();
		}
	}

	public void OnCancelClicked(GameObject go)
	{
		if (Globals.Instance.GameMgr.Status != GameManager.EGameStatus.EGS_None)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		}
		this.Close();
		if (this.CancelClick != null)
		{
			this.CancelClick(this.userData);
		}
	}

	public MessageBox Show(string text, MessageBox.Type type, object data)
	{
		base.gameObject.SetActive(true);
		GameUITools.PlayOpenWindowAnim(this.messageBoxBg.transform, null, false);
		this.content.text = text;
		this.TextOK = Singleton<StringManager>.Instance.GetString("OK");
		this.TextCancel = Singleton<StringManager>.Instance.GetString("Cancel");
		bool flag = type == MessageBox.Type.OK || type == MessageBox.Type.Custom1Btn;
		Vector3 localPosition = this.btnOK.transform.localPosition;
		localPosition.x = ((!flag) ? 90.5f : 0f);
		this.btnOK.transform.localPosition = localPosition;
		NGUITools.SetActive(this.btnCancel.gameObject, !flag);
		this.OkClick = null;
		this.CancelClick = null;
		this.userData = data;
		this.delayOkTime = 0f;
		this.delayCancelTime = 0f;
		this.widgetOK.width = 83;
		this.widgetCancel.width = 83;
		this.btnOK.gameObject.collider.enabled = true;
		this.btnCancel.gameObject.collider.enabled = true;
		this.fadeBG.collider.enabled = true;
		base.CancelInvoke("DestroySelf");
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		return this;
	}

	public MessageBox Show(string text, MessageBox.Type type, UnityEngine.Object userData, MessageBox.MessageDelegate OkCallback, MessageBox.MessageDelegate CancelCallback)
	{
		MessageBox result = this.Show(text, type, userData);
		this.OkClick = OkCallback;
		this.CancelClick = CancelCallback;
		return result;
	}
}
