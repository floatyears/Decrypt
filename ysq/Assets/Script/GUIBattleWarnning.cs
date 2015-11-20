using System;
using UnityEngine;

public class GUIBattleWarnning : MonoBehaviour
{
	private void Awake()
	{
		GameObject gameObject = base.transform.Find("ui47").gameObject;
		Tools.SetParticleRenderQueue(gameObject, 4000, 1f);
		base.Invoke("CloseBattleCD", 8f);
	}

	private void CloseBattleCD()
	{
		GameUIManager.mInstance.CloseBattleWarnning();
	}
}
