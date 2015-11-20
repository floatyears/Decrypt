using System;
using UnityEngine;

public sealed class HackerMessageBox : MonoBehaviour
{
	public static HackerMessageBox Instance;

	private MessageBox MessageBox;

	private bool PauseGame;

	private static int HackerCount;

	public static void ShowMessageBox(string content)
	{
		HackerMessageBox.HackerCount++;
		if (string.IsNullOrEmpty(content))
		{
			content = "Software environment error!";
		}
		if (HackerMessageBox.Instance == null)
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
			Vector3 localPosition = gameObject2.transform.localPosition;
			localPosition.z = 4000f;
			gameObject2.transform.localPosition = localPosition;
			gameObject2.name = "HackerMessageBox";
			HackerMessageBox.Instance = gameObject2.AddComponent<HackerMessageBox>();
		}
		if (HackerMessageBox.Instance != null)
		{
			HackerMessageBox.Instance.Show(content);
		}
	}

	public static void HideMessageBox()
	{
		if (HackerMessageBox.Instance != null)
		{
			HackerMessageBox.Instance.Hide();
		}
	}

	private void Awake()
	{
		this.MessageBox = Tools.GetSafeComponent<MessageBox>(base.gameObject);
		this.MessageBox.StartingRenderQueue += 3000;
		this.MessageBox.TextOK = Singleton<StringManager>.Instance.GetString("OK");
		if (string.IsNullOrEmpty(this.MessageBox.TextOK))
		{
			this.MessageBox.TextOK = "OK";
		}
		this.MessageBox.CanCloseByFadeBGClicked = false;
	}

	private void OnDisable()
	{
		if (this.MessageBox != null)
		{
			this.MessageBox.OkClick = null;
		}
	}

	private void Hide()
	{
		if (this.PauseGame)
		{
			Globals.Instance.GameMgr.Play();
			this.PauseGame = false;
		}
		if (this.MessageBox != null && this.MessageBox.OkClick != null)
		{
			this.MessageBox.OnOKClicked(null);
		}
	}

	private void Show(string content)
	{
		this.MessageBox.Show(content, MessageBox.Type.OK, null);
		this.MessageBox.OkClick = new MessageBox.MessageDelegate(this.OnCheckCallback);
		if (HackerMessageBox.HackerCount >= 1)
		{
			this.MessageBox.DelayOkTime = (float)((HackerMessageBox.HackerCount - 1) * 5);
			this.MessageBox.WidthOK = 120;
		}
		Globals.Instance.SpeedHackDetector.StopMonitoring();
		if (!Globals.Instance.GameMgr.IsPause())
		{
			Globals.Instance.GameMgr.Pause();
			this.PauseGame = true;
		}
	}

	public void OnCheckCallback(object userData)
	{
		if (this.PauseGame)
		{
			Globals.Instance.GameMgr.Play();
			this.PauseGame = false;
		}
		this.MessageBox.OkClick = null;
		Globals.Instance.SpeedHackDetector.StartMonitoring();
	}
}
