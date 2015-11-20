using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIPetTrainSkillInfo : MonoBehaviour
{
	private const int PassiveSkillNum = 3;

	private const int mItemNum = 1;

	private GUIPetTrainSceneV2 mBaseScene;

	private UITexture mActiveSkill;

	private GameObject mActiveSkillEffect;

	private GameObject[] mPassiveSkills = new GameObject[3];

	private UISprite[] mPassiveSkillIcons = new UISprite[3];

	private UISprite[] mPassiveSkillGreyIcons = new UISprite[3];

	private GameObject[] mPassiveSkillEffects = new GameObject[3];

	private GameObject[] mSkillSelects = new GameObject[4];

	private SkillInfo mActiveSkillInfo;

	private SkillInfo[] mPassiveSkillInfos = new SkillInfo[3];

	private bool[] mPassiveUnlock = new bool[4];

	private int[] mSkillArrayIndex = new int[]
	{
		-1,
		-1,
		-1,
		-1
	};

	private UILabel mName;

	private UILabel mSkillLvl;

	private UILabel mSkillNewLvl;

	private UILabel mSkillDesc;

	private UILabel mCostNum;

	public UIButton mShengjiBtn;

	private GameObject mMaxGo;

	private GameObject mArrowGo;

	private GameObject mStateTip;

	private GameObject mCostGo;

	private GUIPetTrainSkillItem[] mNeedItems = new GUIPetTrainSkillItem[1];

	private StringBuilder mSb = new StringBuilder();

	private GUISimpleSM<string, string> mGUISimpleSM;

	public void InitWithBaseScene(GUIPetTrainSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("summonSkill");
		GameObject gameObject = transform.Find("activeSkill").gameObject;
		this.mActiveSkill = gameObject.transform.Find("skill").GetComponent<UITexture>();
		UIEventListener expr_4D = UIEventListener.Get(this.mActiveSkill.gameObject);
		expr_4D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4D.onClick, new UIEventListener.VoidDelegate(this.OnActiveSkillClick));
		this.mSkillSelects[0] = gameObject.transform.Find("select").gameObject;
		this.mActiveSkillEffect = gameObject.transform.Find("ui58").gameObject;
		Tools.SetParticleRenderQueue2(this.mActiveSkillEffect, 3100);
		NGUITools.SetActive(this.mActiveSkillEffect, false);
		for (int i = 0; i < 3; i++)
		{
			this.mPassiveSkills[i] = transform.Find(string.Format("passiveSkill{0}", i)).gameObject;
			this.mPassiveSkillEffects[i] = this.mPassiveSkills[i].transform.Find("ui58").gameObject;
			Tools.SetParticleRenderQueue2(this.mPassiveSkillEffects[i], 3100);
			NGUITools.SetActive(this.mPassiveSkillEffects[i], false);
			this.mPassiveSkillIcons[i] = this.mPassiveSkills[i].transform.Find("skill").GetComponent<UISprite>();
			UIEventListener expr_166 = UIEventListener.Get(this.mPassiveSkillIcons[i].gameObject);
			expr_166.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_166.onClick, new UIEventListener.VoidDelegate(this.OnPassiveSkillIconClick));
			this.mPassiveSkillGreyIcons[i] = this.mPassiveSkills[i].transform.Find("skillGrey").GetComponent<UISprite>();
			UIEventListener expr_1BD = UIEventListener.Get(this.mPassiveSkillGreyIcons[i].gameObject);
			expr_1BD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1BD.onClick, new UIEventListener.VoidDelegate(this.OnPassiveSkillIconClick));
			this.mSkillSelects[i + 1] = this.mPassiveSkills[i].transform.Find("select").gameObject;
		}
		Transform transform2 = base.transform.Find("infoBg");
		this.mName = transform2.Find("name").GetComponent<UILabel>();
		this.mSkillLvl = transform2.Find("lvl0").GetComponent<UILabel>();
		this.mArrowGo = transform2.Find("arrow").gameObject;
		this.mSkillNewLvl = this.mArrowGo.transform.Find("lvl1").GetComponent<UILabel>();
		this.mMaxGo = transform2.Find("maxTxt").gameObject;
		this.mSkillDesc = transform2.Find("desc").GetComponent<UILabel>();
		this.mStateTip = base.transform.Find("tip").gameObject;
		this.mCostGo = base.transform.Find("txt1").gameObject;
		this.mCostNum = this.mCostGo.transform.Find("costNum").GetComponent<UILabel>();
		for (int j = 0; j < 1; j++)
		{
			this.mNeedItems[j] = transform2.Find(string.Format("item{0}", j)).gameObject.AddComponent<GUIPetTrainSkillItem>();
			this.mNeedItems[j].InitWithBaseScene();
		}
		this.mShengjiBtn = base.transform.Find("shengjiBtn").GetComponent<UIButton>();
		UIEventListener expr_37D = UIEventListener.Get(this.mShengjiBtn.gameObject);
		expr_37D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_37D.onClick, new UIEventListener.VoidDelegate(this.OnShengjiBtnClick));
		this.mGUISimpleSM = new GUISimpleSM<string, string>("init");
		this.mGUISimpleSM.Configure("init").Permit("showSkill0", "skill0").Permit("showSkill1", "skill1").Permit("showSkill2", "skill2").Permit("showSkill3", "skill3");
		this.mGUISimpleSM.Configure("skill0").Permit("showSkill1", "skill1").Permit("showSkill2", "skill2").Permit("showSkill3", "skill3").Ignore("showSkill0").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterSkill0();
		});
		this.mGUISimpleSM.Configure("skill1").Permit("showSkill0", "skill0").Permit("showSkill2", "skill2").Permit("showSkill3", "skill3").Ignore("showSkill1").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterSkill1();
		});
		this.mGUISimpleSM.Configure("skill2").Permit("showSkill0", "skill0").Permit("showSkill1", "skill1").Permit("showSkill3", "skill3").Ignore("showSkill2").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterSkill2();
		});
		this.mGUISimpleSM.Configure("skill3").Permit("showSkill0", "skill0").Permit("showSkill1", "skill1").Permit("showSkill2", "skill2").Ignore("showSkill3").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterSkill3();
		});
	}

	private void SetSkillSelectState()
	{
		for (int i = 0; i < 4; i++)
		{
			this.mSkillSelects[i].SetActive(i == this.GetCurSelectIndex());
		}
	}

	private void SetSkillState()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			int curSkillArrayIndex = this.GetCurSkillArrayIndex();
			int curSelectIndex = this.GetCurSelectIndex();
			SkillInfo skillInfo = (curSelectIndex != 0) ? this.mPassiveSkillInfos[curSelectIndex - 1] : this.mActiveSkillInfo;
			if (skillInfo != null && curSkillArrayIndex != -1)
			{
				SkillInfo info = Globals.Instance.AttDB.SkillDict.GetInfo(skillInfo.ID + 1);
				int num = curPetDataEx.GetSkillLevel(curSkillArrayIndex) + 1;
				this.mName.text = skillInfo.Name;
				this.mSkillLvl.text = this.mSb.Remove(0, this.mSb.Length).Append("LV").Append(num).ToString();
				this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("petTrainTxt7")).Append(skillInfo.Desc);
				if (info != null)
				{
					this.mSb.AppendLine();
					this.mSb.AppendLine();
					this.mSb.Append(Singleton<StringManager>.Instance.GetString("petTrainTxt8")).Append("[00ff00]").Append(info.Desc).Append("[-]");
				}
				this.mSkillDesc.text = this.mSb.ToString();
				if (num < GameConst.GetInt32(232) + 1)
				{
					this.mArrowGo.SetActive(true);
					this.mMaxGo.SetActive(false);
					this.mSkillNewLvl.text = this.mSb.Remove(0, this.mSb.Length).Append("LV").Append(num + 1).ToString();
					this.mNeedItems[0].gameObject.SetActive(true);
					int curItemCount = 0;
					int needItemCount = 0;
					int num2 = 0;
					curPetDataEx.GetSkillCost(curSkillArrayIndex, out curItemCount, out needItemCount, out num2);
					this.mCostGo.SetActive(true);
					this.mStateTip.SetActive(false);
					this.mCostNum.text = num2.ToString();
					int money = Globals.Instance.Player.Data.Money;
					if (money < num2)
					{
						this.mCostNum.color = Color.red;
					}
					else
					{
						this.mCostNum.color = new Color(0.8862745f, 0.768627465f, 0.5921569f);
					}
					ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(101));
					this.mNeedItems[0].Refresh(info2, curItemCount, needItemCount);
					if (this.mPassiveUnlock[curSelectIndex] && money >= num2 && this.mNeedItems[0].IsEnough)
					{
						this.mShengjiBtn.isEnabled = true;
						Tools.SetButtonState(this.mShengjiBtn.gameObject, true);
					}
					else
					{
						this.mShengjiBtn.isEnabled = false;
						Tools.SetButtonState(this.mShengjiBtn.gameObject, false);
					}
				}
				else
				{
					this.mArrowGo.SetActive(false);
					this.mMaxGo.SetActive(true);
					this.mNeedItems[0].gameObject.SetActive(false);
					this.mCostGo.SetActive(false);
					this.mStateTip.SetActive(true);
					this.mShengjiBtn.isEnabled = false;
					Tools.SetButtonState(this.mShengjiBtn.gameObject, false);
				}
			}
		}
	}

	private void OnEnterSkill0()
	{
		this.SetSkillSelectState();
		this.SetSkillState();
	}

	private void OnEnterSkill1()
	{
		this.SetSkillSelectState();
		this.SetSkillState();
	}

	private void OnEnterSkill2()
	{
		this.SetSkillSelectState();
		this.SetSkillState();
	}

	private void OnEnterSkill3()
	{
		this.SetSkillSelectState();
		this.SetSkillState();
	}

	private void OnActiveSkillClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.SelectCurItem(0);
	}

	private void OnPassiveSkillIconClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		string name = go.transform.parent.name;
		if (name.EndsWith("0"))
		{
			this.SelectCurItem(1);
		}
		else if (name.EndsWith("1"))
		{
			this.SelectCurItem(2);
		}
		else if (name.EndsWith("2"))
		{
			this.SelectCurItem(3);
		}
	}

	private int GetCurSkillArrayIndex()
	{
		int curSelectIndex = this.GetCurSelectIndex();
		return this.mSkillArrayIndex[curSelectIndex];
	}

	public void OnShengjiBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			if (curPetDataEx.Data.Further < 3u)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("furtherTooLow", 0f, 0f);
			}
			else
			{
				MC2S_PetSkill mC2S_PetSkill = new MC2S_PetSkill();
				mC2S_PetSkill.PetID = curPetDataEx.Data.ID;
				mC2S_PetSkill.Index = this.GetCurSkillArrayIndex();
				Globals.Instance.CliSession.Send(406, mC2S_PetSkill);
			}
		}
	}

	public void SelectCurItem(int index)
	{
		switch (index)
		{
		case 1:
			this.mGUISimpleSM.Fire("showSkill1");
			break;
		case 2:
			this.mGUISimpleSM.Fire("showSkill2");
			break;
		case 3:
			this.mGUISimpleSM.Fire("showSkill3");
			break;
		default:
			this.mGUISimpleSM.Fire("showSkill0");
			break;
		}
	}

	public int GetCurSelectIndex()
	{
		int result = 0;
		string state = this.mGUISimpleSM.State;
		switch (state)
		{
		case "skill1":
			result = 1;
			break;
		case "skill2":
			result = 2;
			break;
		case "skill3":
			result = 3;
			break;
		}
		return result;
	}

	public void Refresh()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			this.mActiveSkillInfo = curPetDataEx.GetPlayerSkillInfo();
			if (this.mActiveSkillInfo != null)
			{
				Texture mainTexture = Res.Load<Texture>(string.Format("icon/skill/{0}", this.mActiveSkillInfo.Icon), false);
				this.mActiveSkill.mainTexture = mainTexture;
			}
			else
			{
				this.mActiveSkill.mainTexture = null;
			}
			this.mPassiveUnlock[0] = true;
			this.mSkillArrayIndex[0] = 0;
			int i = 0;
			for (int j = 0; j < 3; j++)
			{
				this.mPassiveSkillInfos[i] = curPetDataEx.GetSkillInfo(1 + j);
				if (this.mPassiveSkillInfos[i] != null && this.mPassiveSkillInfos[i].ID != 0)
				{
					this.mPassiveSkills[i].gameObject.SetActive(true);
					this.mSkillArrayIndex[i + 1] = j + 1;
					if (j == 0 || (ulong)curPetDataEx.Data.Further > (ulong)((long)(j + 1)))
					{
						this.mPassiveSkillIcons[i].gameObject.SetActive(true);
						this.mPassiveSkillGreyIcons[i].gameObject.SetActive(false);
						this.mPassiveSkillIcons[i].spriteName = this.mPassiveSkillInfos[i].Icon;
						this.mPassiveUnlock[i + 1] = true;
					}
					else
					{
						this.mPassiveSkillIcons[i].gameObject.SetActive(false);
						this.mPassiveSkillGreyIcons[i].gameObject.SetActive(true);
						this.mPassiveSkillGreyIcons[i].spriteName = this.mPassiveSkillInfos[i].Icon;
						this.mPassiveUnlock[i + 1] = false;
					}
					i++;
				}
			}
			while (i < 3)
			{
				this.mPassiveSkillInfos[i] = null;
				this.mPassiveSkills[i].gameObject.SetActive(false);
				this.mPassiveUnlock[i + 1] = false;
				i++;
			}
			this.SetSkillState();
		}
		this.mBaseScene.ShowPetSkillUpNewMark = Tools.CanPetSkillLvlUp(curPetDataEx);
	}

	public void PlaySkillUpEffect()
	{
		switch (this.GetCurSelectIndex())
		{
		case 1:
			NGUITools.SetActive(this.mPassiveSkillEffects[0], false);
			NGUITools.SetActive(this.mPassiveSkillEffects[0], true);
			break;
		case 2:
			NGUITools.SetActive(this.mPassiveSkillEffects[1], false);
			NGUITools.SetActive(this.mPassiveSkillEffects[1], true);
			break;
		case 3:
			NGUITools.SetActive(this.mPassiveSkillEffects[2], false);
			NGUITools.SetActive(this.mPassiveSkillEffects[2], true);
			break;
		default:
			NGUITools.SetActive(this.mActiveSkillEffect, false);
			NGUITools.SetActive(this.mActiveSkillEffect, true);
			break;
		}
	}

	public void HideSkillUpEffect()
	{
		NGUITools.SetActive(this.mActiveSkillEffect, false);
		NGUITools.SetActive(this.mPassiveSkillEffects[0], false);
		NGUITools.SetActive(this.mPassiveSkillEffects[1], false);
		NGUITools.SetActive(this.mPassiveSkillEffects[2], false);
	}
}
