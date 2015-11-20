using Att;
using System;
using System.Text;
using UnityEngine;

public class GUIPetZhuWeiPopUp : GameUIBasePopup
{
	private const int mTabNums = 2;

	private const int mPetNums = 6;

	private GameObject[] mTab0s = new GameObject[2];

	private GameObject[] mTab1s = new GameObject[2];

	private GUIPetZhuWeiItem[] mPetZhuWeiItem = new GUIPetZhuWeiItem[6];

	private GameObject mTipTxt;

	private GameObject mInfoBg;

	private UILabel mTitle0;

	private UILabel mDesc0;

	private UILabel mTitle1;

	private UILabel mDesc1;

	private GUISimpleSM<string, string> mGUISimpleSM;

	private StringBuilder mSb = new StringBuilder(42);

	public static void ShowMe()
	{
		if (Tools.CanPlay(GameConst.GetInt32(29), true))
		{
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIPetZhuWeiPopUp, false, null, null);
		}
	}

	private void Awake()
	{
		this.CreateObjects();
		this.RefreshAssistPets();
		this.SetCurSelectItem(0);
		bool flag = this.IsAssistPetFull();
		this.mTipTxt.SetActive(!flag);
		this.mInfoBg.SetActive(flag);
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBG");
		for (int i = 0; i < 2; i++)
		{
			this.mTab0s[i] = transform.Find(string.Format("tab{0}", i)).gameObject;
			UIEventListener expr_48 = UIEventListener.Get(this.mTab0s[i]);
			expr_48.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_48.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
			this.mTab1s[i] = transform.Find(string.Format("tabF{0}", i)).gameObject;
		}
		bool active = Tools.CanPlay(GameConst.GetInt32(30), true);
		this.mTab0s[1].SetActive(active);
		this.mTab1s[1].SetActive(active);
		GameObject gameObject = transform.Find("CloseBtn").gameObject;
		UIEventListener expr_D8 = UIEventListener.Get(gameObject);
		expr_D8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D8.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		Transform transform2 = transform.Find("bg");
		for (int j = 0; j < 6; j++)
		{
			this.mPetZhuWeiItem[j] = transform2.Find(string.Format("pet{0}", j)).gameObject.AddComponent<GUIPetZhuWeiItem>();
			this.mPetZhuWeiItem[j].InitWithBaseScene(this);
		}
		this.mTipTxt = transform.Find("txt0").gameObject;
		this.mTipTxt.SetActive(false);
		this.mInfoBg = transform.Find("infoGo").gameObject;
		Transform transform3 = this.mInfoBg.transform.Find("info0");
		this.mTitle0 = transform3.Find("title").GetComponent<UILabel>();
		this.mDesc0 = transform3.Find("desc").GetComponent<UILabel>();
		Transform transform4 = this.mInfoBg.transform.Find("info1");
		this.mTitle1 = transform4.Find("title").GetComponent<UILabel>();
		this.mDesc1 = transform4.Find("desc").GetComponent<UILabel>();
		this.mInfoBg.SetActive(false);
		this.mGUISimpleSM = new GUISimpleSM<string, string>("init");
		this.mGUISimpleSM.Configure("init").Permit("onState0", "state0").Permit("onState1", "state1");
		this.mGUISimpleSM.Configure("state0").Permit("onState1", "state1").Ignore("onState0").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterState0();
		});
		this.mGUISimpleSM.Configure("state1").Permit("onState0", "state0").Ignore("onState1").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterState1();
		});
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	public void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (go == this.mTab0s[1])
		{
			this.SetCurSelectItem(1);
		}
		else
		{
			this.SetCurSelectItem(0);
		}
	}

	public void SetCurSelectItem(int index)
	{
		if (index != 1)
		{
			this.mGUISimpleSM.Fire("onState0");
		}
		else
		{
			this.mGUISimpleSM.Fire("onState1");
		}
	}

	public int GetCurPageIndex()
	{
		if (this.mGUISimpleSM.State == "state0")
		{
			return 0;
		}
		if (this.mGUISimpleSM.State == "state1")
		{
			return 1;
		}
		return 0;
	}

	private void SetTabStates(int index)
	{
		this.mTab0s[0].SetActive(0 != index);
		this.mTab1s[0].SetActive(0 == index);
		bool flag = Tools.CanPlay(GameConst.GetInt32(30), true);
		this.mTab0s[1].SetActive(index != 1 && flag);
		this.mTab1s[1].SetActive(index == 1 && flag);
	}

	private void OnEnterState0()
	{
		this.SetTabStates(0);
		this.RefreshAssistPetsLvl();
		this.RefreshZhuWeiInfo();
	}

	private void OnEnterState1()
	{
		this.SetTabStates(1);
		this.RefreshAssistPetsLvl();
		this.RefreshZhuWeiInfo();
	}

	private void RefreshAssistPets()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem != null)
		{
			for (int i = 0; i < 6; i++)
			{
				this.mPetZhuWeiItem[i].Refresh(teamSystem.GetAssist(i, true));
			}
		}
	}

	private void RefreshAssistPetsLvl()
	{
		for (int i = 0; i < 6; i++)
		{
			this.mPetZhuWeiItem[i].RefreshLvlNum();
		}
	}

	private bool IsAssistPetFull()
	{
		bool result = true;
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		for (int i = 0; i < 6; i++)
		{
			if (teamSystem.GetAssist(i) == null)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	private int GetAssistPetMinLvl()
	{
		int num = 2147483647;
		if (this.IsAssistPetFull())
		{
			TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
			for (int i = 0; i < 6; i++)
			{
				PetDataEx assist = teamSystem.GetAssist(i);
				if (assist != null)
				{
					num = Mathf.Min(num, (int)assist.Data.Level);
				}
			}
		}
		return num;
	}

	private int GetAssistPetMinFurtherLvl()
	{
		int num = 2147483647;
		if (this.IsAssistPetFull())
		{
			TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
			for (int i = 0; i < 6; i++)
			{
				PetDataEx assist = teamSystem.GetAssist(i);
				if (assist != null)
				{
					num = Mathf.Min(num, (int)assist.Data.Further);
				}
			}
		}
		return num;
	}

	private void RefreshZhuWeiInfo()
	{
		if (this.IsAssistPetFull())
		{
			int curPageIndex = this.GetCurPageIndex();
			if (curPageIndex == 0)
			{
				int assistPetMinLvl = this.GetAssistPetMinLvl();
				TinyLevelInfo tinyLevelInfo = null;
				foreach (TinyLevelInfo current in Globals.Instance.AttDB.TinyLevelDict.Values)
				{
					if (current != null)
					{
						if (current.AssistMinLevel > 0 && current.AssistMinLevel <= assistPetMinLvl && (tinyLevelInfo == null || current.AssistMinLevel > tinyLevelInfo.AssistMinLevel))
						{
							tinyLevelInfo = current;
						}
					}
				}
				if (tinyLevelInfo != null)
				{
					this.mTitle0.text = Singleton<StringManager>.Instance.GetString("teamManage7", new object[]
					{
						"[ffffff]",
						tinyLevelInfo.AssistMinLevel
					});
					this.mDesc0.text = this.mSb.Remove(0, this.mSb.Length).Append("[e5c383]").Append(Singleton<StringManager>.Instance.GetString("EAF_1")).Append("[ffffff]+").Append(tinyLevelInfo.ALMaxHP).Append("[-]").AppendLine().Append(Singleton<StringManager>.Instance.GetString("EAF_3")).Append("[ffffff]+").Append(tinyLevelInfo.ALAttack).Append("[-]").AppendLine().Append(Singleton<StringManager>.Instance.GetString("EAF_4")).Append("[ffffff]+").Append(tinyLevelInfo.ALDefense).Append("[-]").ToString();
				}
				else
				{
					this.mTitle0.text = Singleton<StringManager>.Instance.GetString("teamManage7", new object[]
					{
						"[ffffff]",
						assistPetMinLvl
					});
					this.mDesc0.text = this.mSb.Remove(0, this.mSb.Length).Append("[e5c383]").Append(Singleton<StringManager>.Instance.GetString("EAF_1")).Append("[ffffff]+").Append(0).Append("[-]").AppendLine().Append(Singleton<StringManager>.Instance.GetString("EAF_3")).Append("[ffffff]+").Append(0).Append("[-]").AppendLine().Append(Singleton<StringManager>.Instance.GetString("EAF_4")).Append("[ffffff]+").Append(0).Append("[-]").ToString();
				}
				bool flag = false;
				foreach (TinyLevelInfo current2 in Globals.Instance.AttDB.TinyLevelDict.Values)
				{
					if (current2 != null)
					{
						if (current2.AssistMinLevel > assistPetMinLvl)
						{
							flag = true;
							if (current2.AssistMinLevel <= GameConst.GetInt32(226))
							{
								this.mTitle1.text = Singleton<StringManager>.Instance.GetString("teamManage7", new object[]
								{
									"[00ff00]",
									current2.AssistMinLevel
								});
								this.mDesc1.text = this.mSb.Remove(0, this.mSb.Length).Append("[e5c383]").Append(Singleton<StringManager>.Instance.GetString("EAF_1")).Append("[00ff00]+").Append(current2.ALMaxHP).Append("[-]").AppendLine().Append(Singleton<StringManager>.Instance.GetString("EAF_3")).Append("[00ff00]+").Append(current2.ALAttack).Append("[-]").AppendLine().Append(Singleton<StringManager>.Instance.GetString("EAF_4")).Append("[00ff00]+").Append(current2.ALDefense).Append("[-]").ToString();
							}
							else
							{
								this.mTitle1.text = this.mSb.Remove(0, this.mSb.Length).Append("[e5c383]").Append(Singleton<StringManager>.Instance.GetString("equipImprove54")).ToString();
								this.mDesc1.text = string.Empty;
							}
							break;
						}
					}
				}
				if (!flag)
				{
					this.mTitle1.text = this.mSb.Remove(0, this.mSb.Length).Append("[e5c383]").Append(Singleton<StringManager>.Instance.GetString("equipImprove54")).ToString();
					this.mDesc1.text = string.Empty;
				}
			}
			else if (curPageIndex == 1)
			{
				int assistPetMinFurtherLvl = this.GetAssistPetMinFurtherLvl();
				TinyLevelInfo tinyLevelInfo2 = null;
				foreach (TinyLevelInfo current3 in Globals.Instance.AttDB.TinyLevelDict.Values)
				{
					if (current3 != null)
					{
						if (current3.AssistMinFurther > 0 && current3.AssistMinFurther <= assistPetMinFurtherLvl && (tinyLevelInfo2 == null || current3.AssistMinFurther > tinyLevelInfo2.AssistMinFurther))
						{
							tinyLevelInfo2 = current3;
						}
					}
				}
				if (tinyLevelInfo2 != null)
				{
					this.mTitle0.text = Singleton<StringManager>.Instance.GetString("teamManage8", new object[]
					{
						"[ffffff]",
						tinyLevelInfo2.AssistMinFurther
					});
					this.mDesc0.text = this.mSb.Remove(0, this.mSb.Length).Append("[e5c383]").Append(Singleton<StringManager>.Instance.GetString("EAF_1")).Append("[ffffff]+").Append(tinyLevelInfo2.AFMaxHP).Append("[-]").AppendLine().Append(Singleton<StringManager>.Instance.GetString("EAF_3")).Append("[ffffff]+").Append(tinyLevelInfo2.AFAttack).Append("[-]").AppendLine().Append(Singleton<StringManager>.Instance.GetString("EAF_4")).Append("[ffffff]+").Append(tinyLevelInfo2.AFDefense).Append("[-]").ToString();
				}
				else
				{
					this.mTitle0.text = Singleton<StringManager>.Instance.GetString("teamManage8", new object[]
					{
						"[ffffff]",
						assistPetMinFurtherLvl
					});
					this.mDesc0.text = this.mSb.Remove(0, this.mSb.Length).Append("[e5c383]").Append(Singleton<StringManager>.Instance.GetString("EAF_1")).Append("[ffffff]+").Append(0).Append("[-]").AppendLine().Append(Singleton<StringManager>.Instance.GetString("EAF_3")).Append("[ffffff]+").Append(0).Append("[-]").AppendLine().Append(Singleton<StringManager>.Instance.GetString("EAF_4")).Append("[ffffff]+").Append(0).Append("[-]").ToString();
				}
				bool flag2 = false;
				foreach (TinyLevelInfo current4 in Globals.Instance.AttDB.TinyLevelDict.Values)
				{
					if (current4 != null)
					{
						if (current4.AssistMinFurther > assistPetMinFurtherLvl)
						{
							flag2 = true;
							if (current4.AssistMinFurther <= GameConst.GetInt32(231))
							{
								this.mTitle1.text = Singleton<StringManager>.Instance.GetString("teamManage8", new object[]
								{
									"[00ff00]",
									current4.AssistMinFurther
								});
								this.mDesc1.text = this.mSb.Remove(0, this.mSb.Length).Append("[e5c383]").Append(Singleton<StringManager>.Instance.GetString("EAF_1")).Append("[00ff00]+").Append(current4.AFMaxHP).Append("[-]").AppendLine().Append(Singleton<StringManager>.Instance.GetString("EAF_3")).Append("[00ff00]+").Append(current4.AFAttack).Append("[-]").AppendLine().Append(Singleton<StringManager>.Instance.GetString("EAF_4")).Append("[00ff00]+").Append(current4.AFDefense).Append("[-]").ToString();
							}
							else
							{
								this.mTitle1.text = this.mSb.Remove(0, this.mSb.Length).Append("[e5c383]").Append(Singleton<StringManager>.Instance.GetString("equipImprove54")).ToString();
								this.mDesc1.text = string.Empty;
							}
							break;
						}
					}
				}
				if (!flag2)
				{
					this.mTitle1.text = this.mSb.Remove(0, this.mSb.Length).Append("[e5c383]").Append(Singleton<StringManager>.Instance.GetString("equipImprove54")).ToString();
					this.mDesc1.text = string.Empty;
				}
			}
		}
	}
}
