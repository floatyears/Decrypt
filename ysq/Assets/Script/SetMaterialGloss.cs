using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/Action/Set Material Gloss")]
public sealed class SetMaterialGloss : ActionBase
{
	public Color color = Color.white;

	public float gloss = 4f;

	public float lerp = 0.05f;

	public float holdDuration = 0.2f;

	protected override void DoAction()
	{
		SetMaterialGloss.ChangeControllerGloss(base.variables.skillCaster, this.color, this.gloss, this.lerp, this.holdDuration);
		base.Finish();
	}

	public static void ChangeControllerGloss(ActorController controller, Color color, float gloss, float lerp, float hold)
	{
		List<CharacterMeshInfo> meshInfos = controller.MeshInfos;
		for (int i = 0; i < meshInfos.Count; i++)
		{
			CharacterMeshInfo characterMeshInfo = meshInfos[i];
			characterMeshInfo.ChangeToColor(color, gloss, lerp, hold);
		}
	}
}
