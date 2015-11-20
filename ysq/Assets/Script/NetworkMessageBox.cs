using System;
using UnityEngine;

public sealed class NetworkMessageBox : MonoBehaviour
{
	public static NetworkMessageBox Instance;

	private MessageBox MessageBox;

	public static void ShowMessageBox(int result)
	{
		string @string = Singleton<StringManager>.Instance.GetString("NetError", new object[]
		{
			result
		});
		NetworkMessageBox.ShowMessageBox(@string);
	}

	public void ShowMessageBoxByKey(string key)
	{
		NetworkMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString(key));
	}

	public static void ShowMessageBox(string content)
	{
		if (string.IsNullOrEmpty(content))
		{
			content = "unknown message.";
		}
		if (NetworkMessageBox.Instance == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/MessageBox");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/MessageBox error"
				});
				return;
			}
			GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
			if (gameObject2 == null)
			{
				global::Debug.LogError(new object[]
				{
					"AddChild error"
				});
				return;
			}
			gameObject2.name = "NetworkMessageBox";
			NetworkMessageBox.Instance = gameObject2.AddComponent<NetworkMessageBox>();
		}
		if (NetworkMessageBox.Instance != null)
		{
			NetworkMessageBox.Instance.Show(content);
		}
	}

	private void Awake()
	{
		this.MessageBox = Tools.GetSafeComponent<MessageBox>(base.gameObject);
		this.MessageBox.CanCloseByFadeBGClicked = false;
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = 4000f;
		base.transform.localPosition = localPosition;
		UIPanel component = base.transform.GetComponent<UIPanel>();
		if (component != null)
		{
			component.depth = 6000;
			component.startingRenderQueue = 5900;
		}
	}

	private void OnDisable()
	{
		if (this.MessageBox != null)
		{
			this.MessageBox.OkClick = null;
		}
	}

	private void Show(string content)
	{
		if (Globals.Instance.GameMgr.Status == GameManager.EGameStatus.EGS_Gaming)
		{
			this.MessageBox.Show(content, MessageBox.Type.Custom1Btn, null);
		}
		else
		{
			this.MessageBox.Show(content, MessageBox.Type.Custom2Btn, null);
			this.MessageBox.CancelClick = new MessageBox.MessageDelegate(this.OnCancelCallback);
		}
		this.MessageBox.TextOK = Singleton<StringManager>.Instance.GetString("Reconnect");
		this.MessageBox.OkClick = new MessageBox.MessageDelegate(this.OnRetryCallback);
	}

	public void OnRetryCallback(object userData)
	{
		Globals.Instance.CliSession.Reconnect();
	}

	public void OnCancelCallback(object userData)
	{
		Globals.Instance.CliSession.SendMsgCancel();
	}
}
