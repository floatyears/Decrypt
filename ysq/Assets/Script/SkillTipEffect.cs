using Att;
using System;
using UnityEngine;

public class SkillTipEffect : MonoBehaviour
{
	private int mSkillIndex;

	public int mTouchID = -2147483648;

	private RaycastHit mHit;

	public void InitWithBaseScene(int index)
	{
		this.mSkillIndex = index;
	}

	private void OnDrag(Vector2 delta)
	{
		if (!base.enabled || this.mTouchID != UICamera.currentTouchID)
		{
			return;
		}
		this.DragMove(Input.mousePosition);
	}

	public void DragMove(Vector3 pos)
	{
		Ray ray = Camera.main.ScreenPointToRay(pos);
		if (Physics.Raycast(ray, out this.mHit, 100f, 1 << LayerDefine.CollisionLayer))
		{
			base.transform.position = this.mHit.point;
		}
	}

	public void DragEnd()
	{
		this.mTouchID = -2147483648;
		if (!base.enabled)
		{
			return;
		}
		SkillInfo info = Globals.Instance.ActorMgr.PlayerCtrler.ActorCtrler.Skills[this.mSkillIndex].Info;
		if (info != null)
		{
			Globals.Instance.ActorMgr.PlayerCtrler.CastSkill(this.mSkillIndex, this.mHit.point);
		}
	}

	private void OnDragEnd()
	{
		if (this.mTouchID != -2147483648 && !(Globals.Instance.ActorMgr.PlayerCtrler == null))
		{
			GameObject hoveredObject = UICamera.hoveredObject;
			if (hoveredObject != null)
			{
				MainSkillButtonBase mainSkillButtonBase = NGUITools.FindInParents<MainSkillButtonBase>(hoveredObject);
				if (mainSkillButtonBase != null && mainSkillButtonBase.SkillIndex == this.mSkillIndex)
				{
					goto IL_70;
				}
			}
			this.DragEnd();
		}
		IL_70:
		GUICombatMain session = GameUIManager.mInstance.GetSession<GUICombatMain>();
		if (session != null)
		{
			session.ShowHideSkillDragTip(false);
		}
		NGUITools.Destroy(base.gameObject);
	}
}
