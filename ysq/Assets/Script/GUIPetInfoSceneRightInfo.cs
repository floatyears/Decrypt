using Att;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public class GUIPetInfoSceneRightInfo : MonoBehaviour
{
	private const int PetInfoYFItemNums = 18;

	private GUIPetInfoSceneV2 mBaseScene;

	private PetDataEx mPetDataEx;

	private int mWhichPart;

	private UILabel mHpNum;

	private UILabel mAttackNum;

	private UILabel mWufangNum;

	private UILabel mFafangNum;

	private UISprite mRelationBgSp;

	private UISprite mTianFuSp;

	private UILabel mTianFuDesc;

	private UISprite mPetDescSp;

	private UILabel mPetDesc;

	private GameObject mSkillGo;

	private GUIPetInfoSkillLayer mSkillLayer;

	private GUIPetInfoSceneYFItem[] mYFItems = new GUIPetInfoSceneYFItem[18];

	private GameObject mPetYFItemPerfab;

	private UITable mRightInfoTable;

	private UIScrollBar mScrollBar;

	private GameObject mContentForDetailInfo;

	private GameObject mContentForHowGet;

	private GameObject mSuiPianState;

	private GameObject mSuiPianStateBg;

	private UILabel mSuiPianTxt;

	private GameObject[] mTab0s = new GameObject[2];

	private GameObject[] mTab1s = new GameObject[2];

	private GUIPetInfoSceneHowGetTable mRightInfoTableForHowGet;

	private List<RelationInfoData> mRelationInfoDatas = new List<RelationInfoData>();

	private StringBuilder mSb = new StringBuilder();

	private GUISimpleSM<string, string> mGUISimpleSM;

	public void InitWithBaseScene(GUIPetInfoSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mContentForDetailInfo = base.transform.Find("rightInfoPanel").gameObject;
		this.mRightInfoTable = base.transform.Find("rightInfoPanel/contents").gameObject.AddComponent<UITable>();
		this.mRightInfoTable.columns = 1;
		this.mRightInfoTable.direction = UITable.Direction.Down;
		this.mRightInfoTable.sorting = UITable.Sorting.Alphabetic;
		this.mRightInfoTable.hideInactive = true;
		this.mRightInfoTable.keepWithinPanel = true;
		this.mRightInfoTable.padding = new Vector2(0f, 2f);
		this.mScrollBar = base.transform.Find("infoScrollBar").GetComponent<UIScrollBar>();
		this.mContentForHowGet = base.transform.Find("rightInfoPanel2").gameObject;
		this.mSuiPianTxt = this.mContentForHowGet.transform.Find("txt0").GetComponent<UILabel>();
		this.mSuiPianTxt.gameObject.SetActive(false);
		this.mRightInfoTableForHowGet = base.transform.Find("rightInfoPanel2/contents").gameObject.AddComponent<GUIPetInfoSceneHowGetTable>();
		this.mRightInfoTableForHowGet.maxPerLine = 1;
		this.mRightInfoTableForHowGet.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mRightInfoTableForHowGet.cellWidth = 360f;
		this.mRightInfoTableForHowGet.cellHeight = 76f;
		this.mRightInfoTableForHowGet.gapHeight = 4f;
		this.mRightInfoTableForHowGet.gapWidth = 0f;
		Transform transform = this.mRightInfoTable.transform.Find("a");
		this.mHpNum = transform.Find("hpTxt/num").GetComponent<UILabel>();
		this.mAttackNum = transform.Find("attackTxt/num").GetComponent<UILabel>();
		this.mWufangNum = transform.Find("wufangTxt/num").GetComponent<UILabel>();
		this.mFafangNum = transform.Find("fafangTxt/num").GetComponent<UILabel>();
		this.mSkillGo = this.mRightInfoTable.transform.Find("b").gameObject;
		this.mSkillLayer = this.mSkillGo.transform.Find("summonSkill").gameObject.AddComponent<GUIPetInfoSkillLayer>();
		this.mSkillLayer.Init(true, true);
		Transform transform2 = this.mRightInfoTable.transform.Find("c");
		this.mRelationBgSp = transform2.GetComponent<UISprite>();
		this.mTianFuSp = this.mRightInfoTable.transform.Find("d").GetComponent<UISprite>();
		this.mTianFuDesc = this.mTianFuSp.transform.Find("desc").GetComponent<UILabel>();
		this.mPetDescSp = this.mRightInfoTable.transform.Find("e").GetComponent<UISprite>();
		this.mPetDesc = this.mPetDescSp.transform.Find("desc").GetComponent<UILabel>();
		this.mSuiPianState = base.transform.Find("suiPianState").gameObject;
		Transform transform3 = this.mSuiPianState.transform;
		this.mSuiPianStateBg = transform3.Find("tab0SpBg").gameObject;
		for (int i = 0; i < 2; i++)
		{
			this.mTab0s[i] = transform3.Find(string.Format("tab{0}", i)).gameObject;
			UIEventListener expr_346 = UIEventListener.Get(this.mTab0s[i]);
			expr_346.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_346.onClick, new UIEventListener.VoidDelegate(this.OnTab0Click));
			this.mTab1s[i] = transform3.Find(string.Format("tabF{0}", i)).gameObject;
		}
		this.mGUISimpleSM = new GUISimpleSM<string, string>("init");
		this.mGUISimpleSM.Configure("init").Permit("onState0", "state0").Permit("onState1", "state1").Permit("onState2", "state2");
		this.mGUISimpleSM.Configure("state0").Permit("onState1", "state1").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterState0();
		});
		this.mGUISimpleSM.Configure("state1").Permit("onState0", "state0").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterState1();
		});
		this.mGUISimpleSM.Configure("state2").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterState2();
		});
	}

	private void OnTab0Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (go == this.mTab0s[0])
		{
			this.SetCurState(0);
		}
		else if (go == this.mTab0s[1])
		{
			this.SetCurState(1);
		}
	}

	public void SetCurState(int index)
	{
		if (index != 1)
		{
			if (index != 2)
			{
				this.mGUISimpleSM.Fire("onState0");
			}
			else
			{
				this.mGUISimpleSM.Fire("onState2");
			}
		}
		else
		{
			this.mGUISimpleSM.Fire("onState1");
		}
	}

	public int GetCurState()
	{
		int result = 2;
		string state = this.mGUISimpleSM.State;
		switch (state)
		{
		case "state0":
			result = 0;
			break;
		case "state1":
			result = 1;
			break;
		case "state2":
			result = 2;
			break;
		}
		return result;
	}

	private void SetTabState(int index)
	{
		for (int i = 0; i < 2; i++)
		{
			this.mTab0s[i].SetActive(i != index);
			this.mTab1s[i].SetActive(i == index);
		}
	}

	private void OnEnterState0()
	{
		this.SetTabState(0);
		this.mSuiPianState.SetActive(true);
		this.mSuiPianStateBg.SetActive(false);
		this.mContentForDetailInfo.SetActive(true);
		this.mContentForHowGet.SetActive(false);
	}

	private void OnEnterState1()
	{
		this.SetTabState(1);
		this.mSuiPianState.SetActive(true);
		this.mSuiPianStateBg.SetActive(true);
		this.mContentForDetailInfo.SetActive(false);
		this.mContentForHowGet.SetActive(true);
	}

	private void OnEnterState2()
	{
		this.mSuiPianState.SetActive(false);
		this.mContentForDetailInfo.SetActive(true);
		this.mContentForHowGet.SetActive(false);
	}

	public void SetSelectedPart(int part)
	{
		this.mWhichPart = part;
		base.StartCoroutine(this.UpdateScrollBar());
	}

	[DebuggerHidden]
	private IEnumerator UpdateScrollBar()
	{
        return null;
        //GUIPetInfoSceneRightInfo.<UpdateScrollBar>c__Iterator96 <UpdateScrollBar>c__Iterator = new GUIPetInfoSceneRightInfo.<UpdateScrollBar>c__Iterator96();
        //<UpdateScrollBar>c__Iterator.<>f__this = this;
        //return <UpdateScrollBar>c__Iterator;
	}

	private void BuildTianFuStr()
	{
		this.mSb.Remove(0, this.mSb.Length);
		int num = 0;
		if (this.mPetDataEx != null)
		{
			for (int i = 0; i < this.mPetDataEx.Info.TalentID.Count; i++)
			{
				int num2 = 0;
				if (i > 9)
				{
					num2 = (i + 1) * 10;
				}
				if (num2 <= GameConst.GetInt32(226))
				{
					TalentInfo info = Globals.Instance.AttDB.TalentDict.GetInfo(this.mPetDataEx.Info.TalentID[i]);
					if (info != null)
					{
						bool flag = (ulong)this.mPetDataEx.Data.Further > (ulong)((long)i);
						if (flag)
						{
							this.mSb.Append("[00ff00]");
						}
						else
						{
							this.mSb.Append("[b2b2b2]");
						}
						this.mSb.Append("[").Append(info.Name).Append("] ");
						if (!flag)
						{
							this.mSb.Append("(").Append(Singleton<StringManager>.Instance.GetString("teamManage1", new object[]
							{
								i + 1
							})).Append(") ");
						}
						this.mSb.Append(info.Desc).Append("[-]\n");
						num++;
					}
				}
			}
		}
		this.mTianFuDesc.text = this.mSb.ToString().TrimEnd(new char[]
		{
			'\n'
		});
		this.mTianFuSp.height = 50 + Mathf.RoundToInt(this.mTianFuDesc.printedSize.y);
	}

	public void Refresh(PetDataEx pdEx)
	{
		if (this.mPetDataEx != pdEx)
		{
			this.mPetDataEx = pdEx;
			this.RefreshContents0();
			int curState = this.GetCurState();
			if (curState == 0 || curState == 1)
			{
				this.RefreshContents1();
			}
		}
	}

	private int SortRelationInfoData(RelationInfoData rA, RelationInfoData rB)
	{
		if (rA == null || rB == null || rA.mRelationInfo == null || rB.mRelationInfo == null)
		{
			return 0;
		}
		if (rA.mRelationInfo.Type == 0 && rB.mRelationInfo.Type == 1)
		{
			return -1;
		}
		if (rA.mRelationInfo.Type == 1 && rB.mRelationInfo.Type == 0)
		{
			return 1;
		}
		return rA.mRelationInfo.ID - rB.mRelationInfo.ID;
	}

	private void RefreshContents0()
	{
		if (this.mPetDataEx != null)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			this.mPetDataEx.GetAttribute(ref num, ref num2, ref num3, ref num4);
			this.mHpNum.text = num.ToString();
			this.mAttackNum.text = num2.ToString();
			this.mWufangNum.text = num3.ToString();
			this.mFafangNum.text = num4.ToString();
			if (this.mPetDataEx.GetSocketSlot() == 0)
			{
				this.mSkillGo.SetActive(false);
			}
			else
			{
				this.mSkillGo.SetActive(true);
				this.mSkillLayer.ShowSummonSkills(this.mPetDataEx, null);
			}
			this.mRelationInfoDatas.Clear();
			bool flag = this.mPetDataEx.IsBattling();
			if (flag)
			{
				SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(this.mPetDataEx.GetSocketSlot(), this.mBaseScene.CurUIState != 3);
				for (int i = 0; i < this.mPetDataEx.Info.RelationID.Count; i++)
				{
					RelationInfo info = Globals.Instance.AttDB.RelationDict.GetInfo(this.mPetDataEx.Info.RelationID[i]);
					if (info != null)
					{
						this.mRelationInfoDatas.Add(new RelationInfoData(info, socket.IsRelationActive(info)));
					}
				}
			}
			else
			{
				for (int j = 0; j < this.mPetDataEx.Info.RelationID.Count; j++)
				{
					RelationInfo info2 = Globals.Instance.AttDB.RelationDict.GetInfo(this.mPetDataEx.Info.RelationID[j]);
					if (info2 != null)
					{
						this.mRelationInfoDatas.Add(new RelationInfoData(info2, false));
					}
				}
			}
			this.mRelationInfoDatas.Sort(new Comparison<RelationInfoData>(this.SortRelationInfoData));
			int k = 0;
			while (k < this.mRelationInfoDatas.Count && k < 18)
			{
				if (this.mYFItems[k] == null)
				{
					this.mYFItems[k] = this.InitYFItem(k);
				}
				this.mYFItems[k].IsVisible = true;
				this.mYFItems[k].Refresh(this.mRelationInfoDatas[k].mRelationInfo, this.mRelationInfoDatas[k].mIsActive);
				k++;
			}
			while (k < 18)
			{
				if (this.mYFItems[k] != null)
				{
					this.mYFItems[k].IsVisible = false;
				}
				k++;
			}
			this.mRelationBgSp.height = 50 + 130 * this.mRelationInfoDatas.Count;
			this.BuildTianFuStr();
			this.mPetDesc.text = this.mPetDataEx.Info.Desc;
			this.mPetDescSp.height = 50 + Mathf.RoundToInt(this.mPetDesc.printedSize.y);
			this.mRightInfoTable.repositionNow = true;
		}
	}

	private GUIPetInfoSceneYFItem InitYFItem(int index)
	{
		if (this.mPetYFItemPerfab == null)
		{
			this.mPetYFItemPerfab = Res.LoadGUI("GUI/PetYFItem");
		}
		if (this.mPetYFItemPerfab == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.LoadGUI GUI/PetYFItem error"
			});
			return null;
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.mPetYFItemPerfab);
		gameObject.SetActive(true);
		GameUITools.AddChild(this.mRelationBgSp.gameObject, gameObject);
		gameObject.transform.localPosition = new Vector3(191f, (float)(-117 + index * -130), 0f);
		gameObject.transform.localScale = Vector3.one;
		GUIPetInfoSceneYFItem gUIPetInfoSceneYFItem = gameObject.AddComponent<GUIPetInfoSceneYFItem>();
		gUIPetInfoSceneYFItem.InitWithBaseScene(this.mBaseScene);
		return gUIPetInfoSceneYFItem;
	}

	private void RefreshContents1()
	{
		this.mRightInfoTableForHowGet.ClearData();
		ItemInfo mItemInfo = null;
		if (this.mBaseScene.MItemInfo == null)
		{
			mItemInfo = PetFragment.GetFragmentInfo(this.mBaseScene.CurPetDataEx.Info.ID);
		}
		else if (this.mBaseScene.MItemInfo != null)
		{
			mItemInfo = this.mBaseScene.MItemInfo;
		}
		GUIHowGetPetItemPopUp.InitSourceItems(mItemInfo, this.mRightInfoTableForHowGet);
		if (this.mRightInfoTableForHowGet.mDatas.Count > 0)
		{
			this.mSuiPianTxt.gameObject.SetActive(false);
		}
		else
		{
			this.mSuiPianTxt.gameObject.SetActive(true);
		}
	}
}
