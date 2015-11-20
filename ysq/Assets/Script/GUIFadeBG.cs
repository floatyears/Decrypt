using System;
using UnityEngine;

public class GUIFadeBG : MonoBehaviour
{
	private UIPanel mPanel;

	private void Awake()
	{
		this.mPanel = base.gameObject.GetComponent<UIPanel>();
	}

	public void SetRQ(int value)
	{
		this.mPanel.startingRenderQueue = value;
	}
}
