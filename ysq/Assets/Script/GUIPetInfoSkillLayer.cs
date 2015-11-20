using Att;
using System;
using System.Text;
using UnityEngine;

public class GUIPetInfoSkillLayer : MonoBehaviour
{
	private const int PassiveSkillNum = 3;

	private UITexture mActiveSkill;

	private GameObject[] mPassiveSkills = new GameObject[3];

	private UISprite[] mPassiveSkillIcons = new UISprite[3];

	private UISprite[] mPassiveSkillIconGreys = new UISprite[3];

	private SkillInfo mActiveSkillInfo;

	private SkillInfo[] mPassiveSkillInfos = new SkillInfo[3];

	private GameUIToolTip mSkillToolTip;

	private StringBuilder mStringBuilder = new StringBuilder();

	public void Init(bool shouldScroll = false, bool shouldGrey = false)
	{
		GameObject gameObject = base.transform.Find("activeSkill").gameObject;
		this.mActiveSkill = gameObject.transform.Find("skill").GetComponent<UITexture>();
		UIEventListener expr_41 = UIEventListener.Get(this.mActiveSkill.gameObject);
		expr_41.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_41.onPress, new UIEventListener.BoolDelegate(this.OnSkillIconPress));
		for (int i = 0; i < 3; i++)
		{
			this.mPassiveSkills[i] = base.transform.Find(string.Format("passiveSkill{0}", i)).gameObject;
			this.mPassiveSkillIcons[i] = this.mPassiveSkills[i].transform.Find("skill").GetComponent<UISprite>();
			UIEventListener expr_C7 = UIEventListener.Get(this.mPassiveSkillIcons[i].gameObject);
			expr_C7.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_C7.onPress, new UIEventListener.BoolDelegate(this.OnPassiveSkillIconPress));
			this.mPassiveSkillIconGreys[i] = this.mPassiveSkills[i].transform.Find("skillGrey").GetComponent<UISprite>();
			UIEventListener expr_11E = UIEventListener.Get(this.mPassiveSkillIconGreys[i].gameObject);
			expr_11E.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_11E.onPress, new UIEventListener.BoolDelegate(this.OnPassiveSkillIconPress));
		}
		if (shouldScroll)
		{
			this.mActiveSkill.gameObject.AddComponent<UIDragScrollView>();
			for (int j = 0; j < 3; j++)
			{
				this.mPassiveSkillIcons[j].gameObject.AddComponent<UIDragScrollView>();
				this.mPassiveSkillIconGreys[j].gameObject.AddComponent<UIDragScrollView>();
			}
		}
		if (shouldGrey)
		{
			for (int k = 0; k < 3; k++)
			{
				this.mPassiveSkillIconGreys[k].color = Color.black;
			}
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

	private void OnPassiveSkillIconPress(GameObject go, bool isPressed)
	{
		SkillInfo skillInfo = null;
		string name = go.transform.parent.name;
		int num = -1;
		if (name.EndsWith("0"))
		{
			skillInfo = this.mPassiveSkillInfos[0];
			num = 0;
		}
		else if (name.EndsWith("1"))
		{
			skillInfo = this.mPassiveSkillInfos[1];
			num = 1;
		}
		else if (name.EndsWith("2"))
		{
			skillInfo = this.mPassiveSkillInfos[2];
			num = 2;
		}
		if (skillInfo != null && isPressed)
		{
			if (this.mSkillToolTip == null)
			{
				this.mSkillToolTip = GameUIToolTipManager.GetInstance().CreateSkillTooltip(go.transform, skillInfo);
			}
			this.mStringBuilder.Remove(0, this.mStringBuilder.Length).Append("[66FE00]").Append(skillInfo.Desc).Append("[-]");
			if (go.name.Equals("skillGrey"))
			{
				this.mStringBuilder.Append("\n");
				this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("skillIsLocked", new object[]
				{
					num + 2
				}));
			}
			this.mSkillToolTip.Create(Tools.GetCameraRootParent(go.transform), skillInfo.Name, this.mStringBuilder.ToString());
			if (name.EndsWith("2"))
			{
				this.mSkillToolTip.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(-180f, 150f, -7000f));
			}
			else
			{
				this.mSkillToolTip.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(-100f, 150f, -7000f));
			}
			this.mSkillToolTip.EnableToolTip();
		}
		else if (this.mSkillToolTip != null)
		{
			this.mSkillToolTip.HideTipAnim();
		}
	}

	public void ShowSummonSkills(PetDataEx petData, PetInfo petInfo = null)
	{
		if (petData != null)
		{
			this.mActiveSkillInfo = petData.GetPlayerSkillInfo();
			if (this.mActiveSkillInfo != null)
			{
				Texture mainTexture = Res.Load<Texture>(string.Format("icon/skill/{0}", this.mActiveSkillInfo.Icon), false);
				this.mActiveSkill.mainTexture = mainTexture;
			}
			else
			{
				this.mActiveSkill.mainTexture = null;
			}
		}
		else
		{
			this.mActiveSkill.mainTexture = null;
		}
		if (this.mPassiveSkills[0] != null && petData != null)
		{
			int i = 0;
			for (int j = 0; j < 3; j++)
			{
				this.mPassiveSkillInfos[i] = petData.GetSkillInfo(1 + j);
				if (this.mPassiveSkillInfos[i] != null && this.mPassiveSkillInfos[i].ID != 0)
				{
					this.mPassiveSkills[i].gameObject.SetActive(true);
					if (j == 0 || (ulong)petData.Data.Further > (ulong)((long)(j + 1)))
					{
						this.mPassiveSkillIcons[i].gameObject.SetActive(true);
						this.mPassiveSkillIconGreys[i].gameObject.SetActive(false);
						this.mPassiveSkillIcons[i].spriteName = this.mPassiveSkillInfos[i].Icon;
					}
					else
					{
						this.mPassiveSkillIcons[i].gameObject.SetActive(false);
						this.mPassiveSkillIconGreys[i].gameObject.SetActive(true);
						this.mPassiveSkillIconGreys[i].spriteName = this.mPassiveSkillInfos[i].Icon;
					}
					i++;
				}
			}
			while (i < 3)
			{
				this.mPassiveSkillInfos[i] = null;
				this.mPassiveSkills[i].gameObject.SetActive(false);
				i++;
			}
		}
	}
}
