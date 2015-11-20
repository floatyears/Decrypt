using System;
using UnityEngine;

[AddComponentMenu("Game/UI Session/GUIGameDebugConsole")]
public class GUIGameDebugConsole : GameUISession
{
	private UIInput mCmdLine;

	protected override void OnPostLoadGUI()
	{
		GameObject gameObject = base.FindGameObject("Background", null);
		base.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.CloseBtnClicked), gameObject);
		gameObject = base.FindGameObject("CmdLine", gameObject);
		base.RegisterClickEvent("EnterBtn", new UIEventListener.VoidDelegate(this.EnterBtnClicked), gameObject);
		if (gameObject != null)
		{
			this.mCmdLine = gameObject.GetComponent<UIInput>();
			EventDelegate.Add(this.mCmdLine.onSubmit, new EventDelegate.Callback(this.CommitCmdLine));
		}
	}

	protected override void OnPreDestroyGUI()
	{
	}

	private void CommitCmdLine()
	{
		if (this.mCmdLine != null && !string.IsNullOrEmpty(this.mCmdLine.value))
		{
			string text = this.mCmdLine.value.Trim();
			this.mCmdLine.value = string.Empty;
			if (text.Length <= 0)
			{
				return;
			}
			char c = text[0];
			if (c == '-')
			{
				if (text.Length >= 2)
				{
					CommandParser.Instance.Parse(text);
				}
				else
				{
					global::Debug.Log(new object[]
					{
						"command error!"
					});
				}
			}
			else
			{
				CommandParser.Instance.Parse("-" + text);
			}
		}
	}

	private void CloseBtnClicked(GameObject go)
	{
		base.Close();
	}

	private void EnterBtnClicked(GameObject go)
	{
		this.CommitCmdLine();
	}
}
