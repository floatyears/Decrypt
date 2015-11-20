using Att;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GUIPetTrainBaseInfo : MonoBehaviour
{
	private const int PetInfoYFItemNums = 18;

	private GUIPetTrainSceneV2 mBaseScene;

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

	private GUIPetTrainSceneYFItem[] mYFItems = new GUIPetTrainSceneYFItem[18];

	private GameObject mPetYFItemPerfab;

	private UITable mRightInfoTable;

	private List<RelationInfoData> mRelationInfoDatas = new List<RelationInfoData>();

	private StringBuilder mSb = new StringBuilder(42);

	public void InitWithBaseScene(GUIPetTrainSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mRightInfoTable = base.transform.Find("contents").gameObject.AddComponent<UITable>();
		this.mRightInfoTable.columns = 1;
		this.mRightInfoTable.direction = UITable.Direction.Down;
		this.mRightInfoTable.sorting = UITable.Sorting.Alphabetic;
		this.mRightInfoTable.hideInactive = true;
		this.mRightInfoTable.keepWithinPanel = true;
		this.mRightInfoTable.padding = new Vector2(0f, 2f);
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

	private void BuildTianFuStr()
	{
		this.mSb.Remove(0, this.mSb.Length);
		int num = 0;
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			for (int i = 0; i < curPetDataEx.Info.TalentID.Count; i++)
			{
				int num2 = 0;
				if (i > 9)
				{
					num2 = (i + 1) * 10;
				}
				if (num2 <= GameConst.GetInt32(226))
				{
					TalentInfo info = Globals.Instance.AttDB.TalentDict.GetInfo(curPetDataEx.Info.TalentID[i]);
					if (info != null)
					{
						bool flag = (ulong)curPetDataEx.Data.Further > (ulong)((long)i);
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

	public void Refresh()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			curPetDataEx.GetAttribute(ref num, ref num2, ref num3, ref num4);
			this.mHpNum.text = num.ToString();
			this.mAttackNum.text = num2.ToString();
			this.mWufangNum.text = num3.ToString();
			this.mFafangNum.text = num4.ToString();
			if (curPetDataEx.GetSocketSlot() == 0)
			{
				this.mSkillGo.SetActive(false);
			}
			else
			{
				this.mSkillGo.SetActive(true);
				this.mSkillLayer.ShowSummonSkills(curPetDataEx, null);
			}
			this.mRelationInfoDatas.Clear();
			bool flag = curPetDataEx.IsBattling();
			if (flag)
			{
				SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(curPetDataEx.GetSocketSlot(), true);
				for (int i = 0; i < curPetDataEx.Info.RelationID.Count; i++)
				{
					RelationInfo info = Globals.Instance.AttDB.RelationDict.GetInfo(curPetDataEx.Info.RelationID[i]);
					if (info != null)
					{
						this.mRelationInfoDatas.Add(new RelationInfoData(info, socket.IsRelationActive(info)));
					}
				}
			}
			else
			{
				for (int j = 0; j < curPetDataEx.Info.RelationID.Count; j++)
				{
					RelationInfo info2 = Globals.Instance.AttDB.RelationDict.GetInfo(curPetDataEx.Info.RelationID[j]);
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
			this.mPetDesc.text = curPetDataEx.Info.Desc;
			this.mPetDescSp.height = 50 + Mathf.RoundToInt(this.mPetDesc.printedSize.y);
			this.mRightInfoTable.repositionNow = true;
		}
	}

	private GUIPetTrainSceneYFItem InitYFItem(int index)
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
		gameObject.transform.localPosition = new Vector3(189f, (float)(-117 + index * -130), 0f);
		gameObject.transform.localScale = Vector3.one;
		GUIPetTrainSceneYFItem gUIPetTrainSceneYFItem = gameObject.AddComponent<GUIPetTrainSceneYFItem>();
		gUIPetTrainSceneYFItem.InitWithBaseScene(this.mBaseScene);
		return gUIPetTrainSceneYFItem;
	}
}
