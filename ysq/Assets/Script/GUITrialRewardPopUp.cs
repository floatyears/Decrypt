using Att;
using Proto;
using System;
using UnityEngine;

public class GUITrialRewardPopUp : GameUIBasePopup
{
	private UILabel mStateTip;

	private UILabel mExpNum;

	private UILabel mMoneyNum;

	private GUITrialRewardItemTable mGUITrialRewardItemTable;

	public static void ShowThis(int beginLvl, int curLvl, bool isBenCeng)
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUITrialRewardPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(beginLvl, curLvl, isBenCeng);
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	public override void InitPopUp(int beginLvl, int curLvl, bool isBenCeng)
	{
		this.Refresh(beginLvl, curLvl, isBenCeng);
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBG");
		this.mStateTip = transform.Find("txt0").GetComponent<UILabel>();
		Transform transform2 = transform.Find("bg");
		this.mExpNum = transform2.Find("expBg/num").GetComponent<UILabel>();
		this.mMoneyNum = transform2.Find("moneyBg/num").GetComponent<UILabel>();
		this.mGUITrialRewardItemTable = transform2.Find("contentsPanel/contents").gameObject.AddComponent<GUITrialRewardItemTable>();
		this.mGUITrialRewardItemTable.maxPerLine = 20;
		this.mGUITrialRewardItemTable.arrangement = UICustomGrid.Arrangement.Horizontal;
		this.mGUITrialRewardItemTable.cellWidth = 84f;
		this.mGUITrialRewardItemTable.cellHeight = 84f;
		this.mGUITrialRewardItemTable.gapWidth = 2f;
		this.mGUITrialRewardItemTable.gapHeight = 0f;
		GameObject gameObject = transform.Find("sureBtn").gameObject;
		UIEventListener expr_EA = UIEventListener.Get(gameObject);
		expr_EA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_EA.onClick, new UIEventListener.VoidDelegate(this.OnSureBtnClick));
	}

	private void Refresh(int beginLvl, int curLvl, bool isBenCeng)
	{
		int num = 0;
		int num2 = 0;
		if (isBenCeng)
		{
			this.mStateTip.text = Singleton<StringManager>.Instance.GetString("trailTower10");
			TrialInfo info = Globals.Instance.AttDB.TrialDict.GetInfo(curLvl);
			if (info != null)
			{
				num += info.Value;
				num2 += info.Money;
				for (int i = 0; i < info.RewardType.Count; i++)
				{
					int num3 = info.RewardType[i];
					if (num3 != 0)
					{
						if (num3 == 3)
						{
							if (i < info.RewardValue1.Count && i < info.RewardValue2.Count)
							{
								int num4 = info.RewardValue1[i];
								if (num4 != 0)
								{
									ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(num4);
									if (info2 != null)
									{
										this.mGUITrialRewardItemTable.AddData(new GUITrialRewardItemData(ERewardType.EReward_Item, info2, info.RewardValue2[i], 0));
									}
								}
							}
						}
						else if (num3 == 15)
						{
							if (i < info.RewardValue1.Count)
							{
								this.mGUITrialRewardItemTable.AddData(new GUITrialRewardItemData(ERewardType.EReward_Emblem, null, 0, info.RewardValue1[i]));
							}
						}
						else if (num3 == 17 && i < info.RewardValue1.Count)
						{
							this.mGUITrialRewardItemTable.AddData(new GUITrialRewardItemData(ERewardType.EReward_LopetSoul, null, 0, info.RewardValue1[i]));
						}
					}
				}
			}
		}
		else
		{
			this.mStateTip.text = Singleton<StringManager>.Instance.GetString("trailTower7", new object[]
			{
				beginLvl,
				curLvl
			});
			if (beginLvl < curLvl)
			{
				for (int j = beginLvl; j <= curLvl; j++)
				{
					TrialInfo info = Globals.Instance.AttDB.TrialDict.GetInfo(j);
					if (info != null)
					{
						num += info.Value;
						num2 += info.Money;
						for (int k = 0; k < info.RewardType.Count; k++)
						{
							int num5 = info.RewardType[k];
							if (num5 != 0)
							{
								if (num5 == 3)
								{
									if (k < info.RewardValue1.Count && k < info.RewardValue2.Count)
									{
										int num6 = info.RewardValue1[k];
										if (num6 != 0)
										{
											ItemInfo info3 = Globals.Instance.AttDB.ItemDict.GetInfo(num6);
											if (info3 != null)
											{
												this.mGUITrialRewardItemTable.AddData(new GUITrialRewardItemData(ERewardType.EReward_Item, info3, info.RewardValue2[k], 0));
											}
										}
									}
								}
								else if (num5 == 15)
								{
									if (k < info.RewardValue1.Count)
									{
										this.mGUITrialRewardItemTable.AddData(new GUITrialRewardItemData(ERewardType.EReward_Emblem, null, 0, info.RewardValue1[k]));
									}
								}
								else if (num5 == 17 && k < info.RewardValue1.Count)
								{
									this.mGUITrialRewardItemTable.AddData(new GUITrialRewardItemData(ERewardType.EReward_LopetSoul, null, 0, info.RewardValue1[k]));
								}
							}
						}
					}
				}
			}
		}
		ActivityValueData valueMod = Globals.Instance.Player.ActivitySystem.GetValueMod(4);
		if (valueMod != null)
		{
			num *= valueMod.Value1 / 100;
		}
		this.mExpNum.text = num.ToString();
		this.mMoneyNum.text = num2.ToString();
	}

	private void OnSureBtnClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
