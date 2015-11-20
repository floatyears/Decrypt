using System;
using UnityEngine;

[AddComponentMenu("Game/Action/PlayAnimationAction")]
public sealed class PlayAnimationAction : ActionBase
{
	public PlayAnimation anim = new PlayAnimation();

	protected override void DoAction()
	{
		if (this.anim.WrapMode == WrapMode.Loop)
		{
			LockingAction component = base.GetComponent<LockingAction>();
			if (component != null)
			{
				component.SetStopLoopAnim(true);
			}
		}
		PlayAnimationAction.PlayAnimation(base.variables, this.anim);
		base.Finish();
	}

	public static void PlayAnimation(SkillVariables variables, PlayAnimation anim)
	{
		AnimationController animationCtrler = variables.skillCaster.AnimationCtrler;
		if (animationCtrler != null)
		{
			animationCtrler.PlayAnimation(anim);
		}
		else
		{
			GameObject gameObject = variables.skillCaster.gameObject;
			if (gameObject && gameObject.animation)
			{
				if (anim.BlendTime < 0.001f)
				{
					gameObject.animation.Play(anim.AnimName, anim.PlayMode);
				}
				else
				{
					gameObject.animation.CrossFade(anim.AnimName, anim.BlendTime, anim.PlayMode);
				}
				gameObject.animation.wrapMode = anim.WrapMode;
			}
		}
	}
}
