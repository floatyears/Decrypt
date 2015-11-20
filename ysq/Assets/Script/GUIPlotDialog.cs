using Att;
using NtUniSdk.Unity3d;
using System;
using UnityEngine;

public class GUIPlotDialog : MonoBehaviour
{
	public delegate void FinishCallback();

	public delegate void VoidCallback();

	private UILabel content;

	private TypewriterEffect contentEffect;

	private UILabel name0;

	private Transform slot0;

	private int headType0;

	private int headValue0;

	private GameObject obj0;

	private string curText;

	private UILabel name1;

	private Transform slot1;

	private int headType1;

	private int headValue1;

	private GameObject obj1;

	private GameObject nextBtn;

	private DialogInfo curDialogInfo;

	public static readonly Color UIAmbientColor = new Color(0.7137255f, 0.7137255f, 0.7137255f);

	private Color cacheAmbientLightColor = GUIPlotDialog.UIAmbientColor;

	public GUIPlotDialog.FinishCallback FinishEvent;

	private GUIPlotDialog.VoidCallback ShowNextEvent;

	private ResourceEntity asyncEntiry;

	private void Awake()
	{
		Transform transform = base.transform.FindChild("DialogBg");
		this.content = transform.FindChild("Label").GetComponent<UILabel>();
		this.contentEffect = Tools.GetSafeComponent<TypewriterEffect>(this.content.gameObject);
		EventDelegate.Add(this.contentEffect.onFinished, new EventDelegate.Callback(this.InfoEffectEnd));
		this.contentEffect.enabled = false;
		this.name0 = transform.FindChild("name0").GetComponent<UILabel>();
		this.name1 = transform.FindChild("name1").GetComponent<UILabel>();
		this.slot0 = this.name0.transform.FindChild("slot");
		this.slot1 = this.name1.transform.FindChild("slot");
		this.nextBtn = transform.FindChild("NextBtn").gameObject;
		this.nextBtn.SetActive(false);
		UIEventListener expr_104 = UIEventListener.Get(base.transform.FindChild("FadeBG").gameObject);
		expr_104.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_104.onClick, new UIEventListener.VoidDelegate(this.OnFadeBGClick));
		GamePadMgr.RegClickDelegate(268435455, new GamePadMgr.VoidDelegate(this.ProcessFadeBGClick));
		base.gameObject.SetActive(false);
	}

	public bool ShowDialogInfo(int dialogID, GUIPlotDialog.FinishCallback callBackEvent, GUIPlotDialog.VoidCallback showNextEvent)
	{
		this.curDialogInfo = Globals.Instance.AttDB.DialogDict.GetInfo(dialogID);
		if (this.curDialogInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("DialogDict.GetInfo error, ID = {0}", dialogID)
			});
			this.CloseDialog();
			return false;
		}
		this.FinishEvent = callBackEvent;
		this.ShowNextEvent = showNextEvent;
		base.gameObject.SetActive(true);
		this.ShowDialog();
		return true;
	}

	private void ShowDialog()
	{
		this.cacheAmbientLightColor = RenderSettings.ambientLight;
		RenderSettings.ambientLight = GUIPlotDialog.UIAmbientColor;
		this.curText = this.curDialogInfo.Text;
		if (this.curDialogInfo.Style % 2 == 0)
		{
			this.name0.gameObject.SetActive(true);
			this.name1.gameObject.SetActive(false);
			if (this.obj0 == null || this.headType0 != this.curDialogInfo.HeadType || this.headValue0 != this.curDialogInfo.HeadValue)
			{
				if (this.obj0 != null)
				{
					UnityEngine.Object.DestroyImmediate(this.obj0);
					this.obj0 = null;
				}
				this.CreateModel(this.slot0, delegate(GameObject go)
				{
					this.obj0 = go;
				});
				this.headType0 = this.curDialogInfo.HeadType;
				this.headValue0 = this.curDialogInfo.HeadValue;
			}
			this.RefreshHeadName(this.name0);
		}
		else
		{
			this.name0.gameObject.SetActive(false);
			this.name1.gameObject.SetActive(true);
			if (this.obj1 == null || this.headType1 != this.curDialogInfo.HeadType || this.headValue1 != this.curDialogInfo.HeadValue)
			{
				if (this.obj1 != null)
				{
					UnityEngine.Object.DestroyImmediate(this.obj1);
					this.obj1 = null;
				}
				this.CreateModel(this.slot1, delegate(GameObject go)
				{
					this.obj1 = go;
				});
				this.headType1 = this.curDialogInfo.HeadType;
				this.headValue1 = this.curDialogInfo.HeadValue;
			}
			this.RefreshHeadName(this.name1);
		}
		this.content.text = string.Empty;
		this.contentEffect.enabled = true;
		this.contentEffect.ResetToBeginning(this.curText);
		this.nextBtn.SetActive(false);
	}

	private void OnDestroy()
	{
		GamePadMgr.UnRegClickDelegate(268435455, new GamePadMgr.VoidDelegate(this.ProcessFadeBGClick));
		if (this.asyncEntiry != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntiry);
			this.asyncEntiry = null;
		}
	}

	private void CreateModel(Transform slot, UIEventListener.VoidDelegate callback)
	{
		if (this.asyncEntiry != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntiry);
			this.asyncEntiry = null;
		}
		GameObject obj = null;
		if (this.curDialogInfo.HeadType == 0)
		{
			this.asyncEntiry = ActorManager.CreateLocalUIActor(0, 0, false, false, slot.gameObject, this.curDialogInfo.Scale, delegate(GameObject go)
			{
				this.asyncEntiry = null;
				obj = go;
				if (obj != null)
				{
					obj.transform.localPosition += new Vector3(0f, this.curDialogInfo.OffsetY, 0f);
					obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
					if (obj.animation != null)
					{
						obj.animation.CrossFadeQueued("std");
					}
				}
				if (callback != null)
				{
					callback(go);
				}
			});
		}
		else if (this.curDialogInfo.HeadType == 1)
		{
			this.asyncEntiry = ActorManager.CreateUIPet(this.curDialogInfo.HeadValue, 0, false, false, slot.gameObject, this.curDialogInfo.Scale, 2, delegate(GameObject go)
			{
				this.asyncEntiry = null;
				obj = go;
				if (obj != null)
				{
					obj.transform.localPosition += new Vector3(0f, this.curDialogInfo.OffsetY, 0f);
					obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
					if (obj.animation != null)
					{
						obj.animation.CrossFadeQueued("std");
					}
				}
				if (callback != null)
				{
					callback(go);
				}
			});
		}
		else if (this.curDialogInfo.HeadType == 2)
		{
			this.asyncEntiry = ActorManager.CreateUIMonster(this.curDialogInfo.HeadValue, 0, false, false, slot.gameObject, this.curDialogInfo.Scale, delegate(GameObject go)
			{
				this.asyncEntiry = null;
				obj = go;
				if (obj != null)
				{
					obj.transform.localPosition += new Vector3(0f, this.curDialogInfo.OffsetY, 0f);
					obj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
					if (obj.animation != null)
					{
						obj.animation.CrossFadeQueued("std");
					}
				}
				if (callback != null)
				{
					callback(go);
				}
			});
		}
	}

	private void RefreshHeadName(UILabel name)
	{
		string @string = Singleton<StringManager>.Instance.GetString("Colon");
		int num = this.curText.IndexOf(@string);
		if (num >= 0)
		{
			name.text = this.GetName(this.curText.Substring(0, num));
			this.curText = this.curText.Substring(num + 1);
		}
		else
		{
			global::Debug.LogError(new object[]
			{
				"Can't find Dialog name error"
			});
		}
		this.ReplaceName();
	}

	private void ReplaceName()
	{
		string name = Globals.Instance.Player.Data.Name;
		while (this.FindPlayerName(this.curText))
		{
			this.curText = this.curText.Replace("#YX#", name);
		}
	}

	private string GetName(string name)
	{
		return (!this.FindPlayerName(name)) ? name : Globals.Instance.Player.Data.Name;
	}

	private bool FindPlayerName(string name)
	{
		return name.Contains("#YX#");
	}

	private void UpdateNextInfo()
	{
		if (this.curDialogInfo.NextID == 0)
		{
			this.CloseDialog();
			return;
		}
		this.curDialogInfo = Globals.Instance.AttDB.DialogDict.GetInfo(this.curDialogInfo.NextID);
		if (this.curDialogInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("DialogDict.GetInfo error, ID = {0}", this.curDialogInfo.NextID)
			});
			this.CloseDialog();
			return;
		}
		this.ShowDialog();
		if (this.ShowNextEvent != null)
		{
			this.ShowNextEvent();
		}
	}

	private void CloseDialog()
	{
		RenderSettings.ambientLight = this.cacheAmbientLightColor;
		if (this.FinishEvent != null)
		{
			this.FinishEvent();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnFadeBGClick(GameObject go)
	{
		this.ProcessFadeBGClick();
	}

	private void ProcessFadeBGClick()
	{
		if (this.nextBtn.activeSelf)
		{
			this.UpdateNextInfo();
		}
		else
		{
			this.contentEffect.Finish();
			this.contentEffect.enabled = false;
			this.nextBtn.SetActive(true);
		}
	}

	private void InfoEffectEnd()
	{
		this.nextBtn.SetActive(true);
	}
}
