using Att;
using System;
using System.Text;
using UnityEngine;

public class LopetInfoSkillLayer : MonoBehaviour
{
	private GameObject mActive;

	private UITexture mActiveSkill;

	private SkillInfo mActiveSkillInfo;

	private GameUIToolTip mSkillToolTip;

	private StringBuilder mStringBuilder = new StringBuilder();

	public void Init()
	{
		GameObject parent = GameUITools.FindGameObject("skills", base.gameObject);
		this.mActive = GameUITools.FindGameObject("active", parent);
		this.mActiveSkill = GameUITools.FindGameObject("skill", this.mActive).GetComponent<UITexture>();
		UIEventListener expr_4D = UIEventListener.Get(this.mActiveSkill.gameObject);
		expr_4D.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_4D.onPress, new UIEventListener.BoolDelegate(this.OnSkillIconPress));
	}

	public void Refresh(LopetDataEx lopetData)
	{
		if (lopetData == null)
		{
			this.mActive.SetActive(false);
			return;
		}
		this.mActive.SetActive(true);
		this.mActiveSkillInfo = lopetData.GetPlayerSkillInfo();
		if (this.mActiveSkillInfo != null)
		{
			this.mActiveSkill.mainTexture = Res.Load<Texture>(string.Format("icon/skill/{0}", this.mActiveSkillInfo.Icon), false);
		}
		else
		{
			this.mActiveSkill.mainTexture = null;
		}
	}

	private void OnSkillIconPress(GameObject go, bool isPressed)
	{
		if (this.mActiveSkillInfo != null && isPressed)
		{
			if (this.mSkillToolTip == null)
			{
				this.mSkillToolTip = GameUIToolTipManager.GetInstance().CreateSkillTooltip(go.transform, this.mActiveSkillInfo);
			}
			this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append("[66FE00]").Append(this.mActiveSkillInfo.Desc).Append("[-]");
			this.mSkillToolTip.Create(Tools.GetCameraRootParent(go.transform), this.mActiveSkillInfo.Name, this.mStringBuilder.ToString());
			this.mSkillToolTip.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(-100f, 150f, -7000f));
			this.mSkillToolTip.EnableToolTip();
		}
		else if (this.mSkillToolTip != null)
		{
			this.mSkillToolTip.HideTipAnim();
		}
	}
}
