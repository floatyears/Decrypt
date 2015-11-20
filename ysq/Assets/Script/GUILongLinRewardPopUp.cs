using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUILongLinRewardPopUp : GameUIBasePopup
{
	private UILabel mTitle;

	private UIButton mSureBtn;

	private GUILongLinRewardTable mGUILongLinRewardTable;

	private GameObject[] mTab0s = new GameObject[2];

	private GameObject[] mTab1s = new GameObject[2];

	private GameObject mTabNew0;

	private GameObject mTabNew1;

	private UILabel mTipTxt;

	private UILabel mBottomTip;

	private string mLongLinTxt1;

	private string mLongLinTxt2;

	private string mLongLinTxt3;

	private string mLongLinTxt4;

	private GUISimpleSM<string, string> mGUISimpleSM;

	private List<RewardData> mRwardDatas = new List<RewardData>();

	public static void ShowMe()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUILongLinRewardPopUp, false, null, null);
	}

	private void Awake()
	{
		this.CreateObjects();
		this.SetCurSelectItem(0);
		this.RefreshTabNewMark();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBG");
		this.mTitle = transform.Find("flower/Label").GetComponent<UILabel>();
		this.mBottomTip = transform.Find("txt0").GetComponent<UILabel>();
		this.mBottomTip.overflowMethod = UILabel.Overflow.ShrinkContent;
		this.mBottomTip.width = 440;
		this.mLongLinTxt1 = Singleton<StringManager>.Instance.GetString("longLinTxt1");
		this.mLongLinTxt2 = Singleton<StringManager>.Instance.GetString("longLinTxt2");
		this.mLongLinTxt3 = Singleton<StringManager>.Instance.GetString("longLinTxt3");
		this.mLongLinTxt4 = Singleton<StringManager>.Instance.GetString("longLinTxt4");
		for (int i = 0; i < 2; i++)
		{
			this.mTab0s[i] = transform.Find(string.Format("tab{0}", i)).gameObject;
			UIEventListener expr_E4 = UIEventListener.Get(this.mTab0s[i]);
			expr_E4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_E4.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
			this.mTab0s[i].transform.Find("Label").GetComponent<UILabel>().text = ((i != 0) ? this.mLongLinTxt2 : this.mLongLinTxt1);
			this.mTab1s[i] = transform.Find(string.Format("tabF{0}", i)).gameObject;
			this.mTab1s[i].transform.Find("Label").GetComponent<UILabel>().text = ((i != 0) ? this.mLongLinTxt2 : this.mLongLinTxt1);
		}
		this.mTabNew0 = transform.Find("tabNew0").gameObject;
		this.mTabNew0.SetActive(false);
		this.mTabNew1 = transform.Find("tabNew1").gameObject;
		this.mTabNew1.SetActive(false);
		this.mGUILongLinRewardTable = transform.transform.Find("itemsPanel/itemsContents").gameObject.AddComponent<GUILongLinRewardTable>();
		this.mGUILongLinRewardTable.maxPerLine = 1;
		this.mGUILongLinRewardTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mGUILongLinRewardTable.cellWidth = 610f;
		this.mGUILongLinRewardTable.cellHeight = 91f;
		this.mGUILongLinRewardTable.InitWithBaseScene(this);
		GameObject gameObject = transform.Find("sureBtn").gameObject;
		this.mSureBtn = gameObject.GetComponent<UIButton>();
		UIEventListener expr_26E = UIEventListener.Get(gameObject);
		expr_26E.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_26E.onClick, new UIEventListener.VoidDelegate(this.OnSureBtnClick));
		GameObject gameObject2 = transform.Find("CloseBtn").gameObject;
		UIEventListener expr_2A6 = UIEventListener.Get(gameObject2);
		expr_2A6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2A6.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
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
		Globals.Instance.CliSession.Register(648, new ClientSession.MsgHandler(this.OnMsgTakeFDSReward));
		Globals.Instance.CliSession.Register(651, new ClientSession.MsgHandler(this.OnMsgTakeKWBReward));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		Globals.Instance.CliSession.Unregister(648, new ClientSession.MsgHandler(this.OnMsgTakeFDSReward));
		Globals.Instance.CliSession.Unregister(651, new ClientSession.MsgHandler(this.OnMsgTakeKWBReward));
	}

	public void AddRewardData(int rdType, int rdValue1, int rdValue2)
	{
		this.mRwardDatas.Clear();
		this.mRwardDatas.Add(new RewardData
		{
			RewardType = rdType,
			RewardValue1 = rdValue1,
			RewardValue2 = rdValue2
		});
	}

	private void OnSureBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		int curPageIndex = this.GetCurPageIndex();
		if (curPageIndex == 0)
		{
			this.mRwardDatas.Clear();
			int fDSMaxId = this.GetFDSMaxId();
			for (int i = 1; i <= fDSMaxId; i++)
			{
				FDSInfo info = Globals.Instance.AttDB.FDSDict.GetInfo(i);
				if (info != null)
				{
					if (!worldBossSystem.IsFDSRewardTaken(info.ID) && info.FireDragonScale <= Globals.Instance.Player.Data.FireDragonScale)
					{
						this.mRwardDatas.Add(new RewardData
						{
							RewardType = info.RewardType,
							RewardValue1 = info.RewardValue1,
							RewardValue2 = info.RewardValue2
						});
					}
				}
			}
			MC2S_TakeFDSReward mC2S_TakeFDSReward = new MC2S_TakeFDSReward();
			mC2S_TakeFDSReward.ID = 0;
			Globals.Instance.CliSession.Send(647, mC2S_TakeFDSReward);
		}
		else if (curPageIndex == 1)
		{
			this.mRwardDatas.Clear();
			foreach (WorldRespawnInfo current in Globals.Instance.AttDB.WorldRespawnDict.Values)
			{
				if (current != null)
				{
					if (worldBossSystem.IsWBRewrdCanTaken(current.ID) && !worldBossSystem.IsWBRewardTaken(current.ID))
					{
						bool flag = worldBossSystem.IsWBRewrdDouble(current.ID);
						if (current.RewardType == 1 || current.RewardType == 2)
						{
							this.mRwardDatas.Add(new RewardData
							{
								RewardType = current.RewardType,
								RewardValue1 = (!flag) ? current.RewardValue1 : (current.RewardValue1 * 2),
								RewardValue2 = 0
							});
						}
						else
						{
							this.mRwardDatas.Add(new RewardData
							{
								RewardType = current.RewardType,
								RewardValue1 = current.RewardValue1,
								RewardValue2 = (!flag) ? current.RewardValue2 : (current.RewardValue2 * 2)
							});
						}
					}
				}
			}
			MC2S_TakeKillWorldBossReward mC2S_TakeKillWorldBossReward = new MC2S_TakeKillWorldBossReward();
			mC2S_TakeKillWorldBossReward.Slot = 0;
			Globals.Instance.CliSession.Send(650, mC2S_TakeKillWorldBossReward);
		}
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private int GetFDSMaxId()
	{
		int num = 1;
		int num2 = Globals.Instance.Player.TeamSystem.GetCombatValue() * 100;
		while (true)
		{
			FDSInfo info = Globals.Instance.AttDB.FDSDict.GetInfo(num);
			if (info == null)
			{
				break;
			}
			if (info.FireDragonScale <= num2)
			{
				num++;
			}
			else
			{
				if (num % 6 == 0)
				{
					return num;
				}
				num++;
			}
		}
		num--;
		return num;
	}

	private void InitRewardItems()
	{
		this.mGUILongLinRewardTable.ClearData();
		int curPageIndex = this.GetCurPageIndex();
		if (curPageIndex == 0)
		{
			int fDSMaxId = this.GetFDSMaxId();
			for (int i = 1; i <= fDSMaxId; i++)
			{
				FDSInfo info = Globals.Instance.AttDB.FDSDict.GetInfo(i);
				if (info != null)
				{
					this.mGUILongLinRewardTable.AddData(new GUILongLinRewardData(info, null));
				}
			}
		}
		else if (curPageIndex == 1)
		{
			foreach (WorldRespawnInfo current in Globals.Instance.AttDB.WorldRespawnDict.Values)
			{
				if (current != null)
				{
					this.mGUILongLinRewardTable.AddData(new GUILongLinRewardData(null, current));
				}
			}
		}
	}

	private void OnMsgTakeFDSReward(MemoryStream stream)
	{
		MS2C_TakeFDSReward mS2C_TakeFDSReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeFDSReward), stream) as MS2C_TakeFDSReward;
		if (mS2C_TakeFDSReward.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TakeFDSReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_TakeFDSReward.Result);
			return;
		}
		this.Refresh(mS2C_TakeFDSReward.ID);
		this.RefreshTabNewMark();
		if (this.mRwardDatas.Count > 0)
		{
			GUIRewardPanel.Show(this.mRwardDatas, null, false, false, null, false);
		}
	}

	private void OnMsgTakeKWBReward(MemoryStream stream)
	{
		MS2C_TakeKillWorldBossReward mS2C_TakeKillWorldBossReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeKillWorldBossReward), stream) as MS2C_TakeKillWorldBossReward;
		if (mS2C_TakeKillWorldBossReward.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_TakeKillWorldBossReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_TakeKillWorldBossReward.Result);
			return;
		}
		Globals.Instance.Player.WorldBossSystem.SetWBRewardFlag(mS2C_TakeKillWorldBossReward.HasReward);
		this.Refresh(mS2C_TakeKillWorldBossReward.Slot);
		this.RefreshTabNewMark();
		if (this.mRwardDatas.Count > 0)
		{
			GUIRewardPanel.Show(this.mRwardDatas, null, false, false, null, false);
		}
	}

	private void Refresh(int fdId)
	{
		this.mGUILongLinRewardTable.Refresh(fdId);
		bool flag = false;
		for (int i = 0; i < this.mGUILongLinRewardTable.mDatas.Count; i++)
		{
			GUILongLinRewardData gUILongLinRewardData = (GUILongLinRewardData)this.mGUILongLinRewardTable.mDatas[i];
			if (gUILongLinRewardData != null && gUILongLinRewardData.IsCanTaken())
			{
				flag = true;
				break;
			}
		}
		this.mSureBtn.isEnabled = flag;
		Tools.SetButtonState(this.mSureBtn.gameObject, flag);
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

	private void SetTabStates(int index)
	{
		for (int i = 0; i < 2; i++)
		{
			this.mTab0s[i].SetActive(i != index);
			this.mTab1s[i].SetActive(i == index);
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

	private void OnEnterState0()
	{
		this.mTitle.text = this.mLongLinTxt1;
		this.mBottomTip.text = this.mLongLinTxt3;
		this.SetTabStates(0);
		this.InitRewardItems();
		this.Refresh(0);
	}

	private void OnEnterState1()
	{
		this.mTitle.text = this.mLongLinTxt2;
		this.mBottomTip.text = this.mLongLinTxt4;
		this.SetTabStates(1);
		this.InitRewardItems();
		this.Refresh(0);
	}

	private void RefreshTabNewMark()
	{
		this.mTabNew0.SetActive(false);
		this.mTabNew1.SetActive(false);
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		if (worldBossSystem != null)
		{
			int fDSMaxId = this.GetFDSMaxId();
			for (int i = 1; i <= fDSMaxId; i++)
			{
				FDSInfo info = Globals.Instance.AttDB.FDSDict.GetInfo(i);
				if (info != null)
				{
					if (!worldBossSystem.IsFDSRewardTaken(info.ID) && info.FireDragonScale <= Globals.Instance.Player.Data.FireDragonScale)
					{
						this.mTabNew0.SetActive(true);
						break;
					}
				}
			}
			foreach (WorldRespawnInfo current in Globals.Instance.AttDB.WorldRespawnDict.Values)
			{
				if (current != null)
				{
					if (worldBossSystem.IsWBRewrdCanTaken(current.ID) && !worldBossSystem.IsWBRewardTaken(current.ID))
					{
						this.mTabNew1.SetActive(true);
						break;
					}
				}
			}
		}
	}
}
