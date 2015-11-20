using System;
using UnityEngine;

[AddComponentMenu("Game/Action/WarningAction")]
public class WarningAction : ActionBase
{
	protected override void DoAction()
	{
		GameUIManager.mInstance.ShowBattleWarnning();
		base.Finish();
	}
}
