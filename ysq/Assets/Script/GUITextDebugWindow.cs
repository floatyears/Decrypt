using System;
using UnityEngine;

public class GUITextDebugWindow : MonoBehaviour
{
	private static string mWindowText = string.Empty;

	private float mWindowPosition;

	private bool mIsWindowOpen;

	private Vector2 mScrollViewVector = Vector2.zero;

	private GUIStyle mDebugBoxStyle;

	private float mLeftSide;

	private float mTopSide;

	private float mDebugWidth = 320f;

	private float mDebugHeight = 300f;

	public bool mDebugIsOn = true;

	public bool IsWindowOpen
	{
		get
		{
			return this.mIsWindowOpen;
		}
		set
		{
			this.mIsWindowOpen = value;
			this.mWindowPosition = ((!this.mIsWindowOpen) ? -2000f : this.mLeftSide);
		}
	}

	public static void debug(string newString)
	{
		GUITextDebugWindow.mWindowText = newString + "\n" + GUITextDebugWindow.mWindowText;
	}

	private void Start()
	{
		this.mDebugBoxStyle = new GUIStyle();
		this.mDebugBoxStyle.alignment = TextAnchor.UpperLeft;
		this.mLeftSide = 100f;
		this.mTopSide = 80f;
		this.IsWindowOpen = true;
	}

	private void OnGUI()
	{
		if (this.mDebugIsOn)
		{
			GUI.depth = 6000;
			GUI.BeginGroup(new Rect(this.mWindowPosition, this.mTopSide + 40f, this.mDebugWidth, this.mDebugHeight));
			this.mScrollViewVector = GUI.BeginScrollView(new Rect(0f, 0f, this.mDebugWidth, this.mDebugHeight), this.mScrollViewVector, new Rect(0f, 0f, 0f, 2000f));
			GUI.Box(new Rect(0f, 0f, this.mDebugWidth - 20f, 2000f), GUITextDebugWindow.mWindowText, this.mDebugBoxStyle);
			GUI.EndScrollView();
			GUI.EndGroup();
			if (GUI.Button(new Rect(this.mLeftSide, this.mTopSide, 75f, 40f), "调试"))
			{
				this.IsWindowOpen = !this.IsWindowOpen;
			}
			if (GUI.Button(new Rect(this.mLeftSide + 80f, this.mTopSide, 75f, 40f), "清除"))
			{
				GUITextDebugWindow.mWindowText = string.Empty;
			}
		}
	}
}
