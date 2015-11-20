using System;
using UnityEngine;

public class GUIBattleCountDown : MonoBehaviour
{
	public delegate void CDEndCallback();

	public GUIBattleCountDown.CDEndCallback CDEndEvent;

	private void Awake()
	{
		GameObject gameObject = base.transform.Find("ui37").gameObject;
		Tools.SetParticleRenderQueue(gameObject, 4000, 1f);
		base.Invoke("OnCDEndEvent", 4f);
		base.Invoke("CloseBattleCD", 4.5f);
	}

	private void OnCDEndEvent()
	{
		if (this.CDEndEvent != null)
		{
			this.CDEndEvent();
		}
	}

	private void CloseBattleCD()
	{
		GameUIManager.mInstance.CloseBattleCDMsg();
	}
}
