using Att;
using System;
using System.Text;
using UnityEngine;

public class GUIPetSkills : MonoBehaviour
{
	private const int passiveSkillCount = 3;

	private UITexture mActiveSkill;

	private SkillInfo mActiveSkillInfo;

	private GameObject[] mPassiveSkills = new GameObject[3];

	private UISprite[] mPassiveSkillIcons = new UISprite[3];

	private UISprite[] mPassiveSkillGreyIcons = new UISprite[3];

	private SkillInfo[] mPassiveSkillInfos = new SkillInfo[3];

	private UILabel[] mSkillsLevel = new UILabel[4];

	private GameUIToolTip mSkillToolTip;

	private StringBuilder mStringBuilder = new StringBuilder();

	private bool isLeft = true;

	private void Awake()
	{
		int num = 0;
		while (num < base.transform.childCount && num < 4)
		{
			if (num == 0)
			{
				this.mActiveSkill = base.transform.GetChild(num).Find("Skill").GetComponent<UITexture>();
				UIEventListener expr_3E = UIEventListener.Get(this.mActiveSkill.gameObject);
				expr_3E.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_3E.onPress, new UIEventListener.BoolDelegate(this.OnSkillIconPress));
			}
			else
			{
				this.mPassiveSkills[num - 1] = base.transform.GetChild(num).gameObject;
				this.mPassiveSkillIcons[num - 1] = base.transform.GetChild(num).Find("Skill").GetComponent<UISprite>();
				this.mPassiveSkillGreyIcons[num - 1] = base.transform.GetChild(num).Find("Grey").GetComponent<UISprite>();
				UIEventListener expr_DD = UIEventListener.Get(this.mPassiveSkillIcons[num - 1].gameObject);
				expr_DD.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_DD.onPress, new UIEventListener.BoolDelegate(this.OnPassiveSkillIconPress));
				UIEventListener expr_112 = UIEventListener.Get(this.mPassiveSkillGreyIcons[num - 1].gameObject);
				expr_112.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_112.onPress, new UIEventListener.BoolDelegate(this.OnPassiveSkillIconPress));
			}
			this.mSkillsLevel[num] = base.transform.GetChild(num).Find("Level").GetComponent<UILabel>();
			num++;
		}
	}

	public void Refresh(PetDataEx petData, bool isLeft = true)
	{
		this.isLeft = isLeft;
		if (petData != null)
		{
			this.mActiveSkillInfo = petData.GetPlayerSkillInfo();
			if (this.mActiveSkillInfo != null)
			{
				Texture mainTexture = Res.Load<Texture>(string.Format("icon/skill/{0}", this.mActiveSkillInfo.Icon), false);
				this.mActiveSkill.mainTexture = mainTexture;
				this.mSkillsLevel[0].text = Singleton<StringManager>.Instance.GetString("equipImprove36", new object[]
				{
					this.mActiveSkillInfo.Level
				});
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
						this.mPassiveSkillGreyIcons[i].gameObject.SetActive(false);
						this.mPassiveSkillIcons[i].spriteName = this.mPassiveSkillInfos[i].Icon;
						this.mSkillsLevel[j + 1].text = Singleton<StringManager>.Instance.GetString("equipImprove36", new object[]
						{
							this.mPassiveSkillInfos[j].Level
						});
					}
					else
					{
						this.mPassiveSkillIcons[i].gameObject.SetActive(false);
						this.mPassiveSkillGreyIcons[i].gameObject.SetActive(true);
						this.mPassiveSkillGreyIcons[i].spriteName = this.mPassiveSkillInfos[i].Icon;
						this.mSkillsLevel[j + 1].text = string.Empty;
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
			this.mSkillToolTip.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3((!this.isLeft) ? -100f : 50f, 150f, -7000f));
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
		if (name.EndsWith("1"))
		{
			skillInfo = this.mPassiveSkillInfos[0];
			num = 0;
		}
		else if (name.EndsWith("2"))
		{
			skillInfo = this.mPassiveSkillInfos[1];
			num = 1;
		}
		else if (name.EndsWith("3"))
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
			if (go.name.Equals("Grey"))
			{
				this.mStringBuilder.Append("\n");
				this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("skillIsLocked", new object[]
				{
					num + 2
				}));
			}
			this.mSkillToolTip.Create(Tools.GetCameraRootParent(go.transform), skillInfo.Name, this.mStringBuilder.ToString());
			if (name.EndsWith("3"))
			{
				this.mSkillToolTip.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3((!this.isLeft) ? -100f : 50f, 150f, -7000f));
			}
			else
			{
				this.mSkillToolTip.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3((!this.isLeft) ? -100f : 50f, 150f, -7000f));
			}
			this.mSkillToolTip.EnableToolTip();
		}
		else if (this.mSkillToolTip != null)
		{
			this.mSkillToolTip.HideTipAnim();
		}
	}
}
