using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class GUITrialInGamePopUp : MonoBehaviour
{
	private GUITrialRewardItemTable mGUITrialRewardItemTable;

	private Transform mWinBg;

	private UISprite mWinBgSp;

	public static void ShowThis(int curLvl)
	{
		TrialInfo info = Globals.Instance.AttDB.TrialDict.GetInfo(curLvl);
		if (info != null)
		{
			bool flag = false;
			for (int i = 0; i < info.RewardType.Count; i++)
			{
				int num = info.RewardType[i];
				if (num != 0)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				GameUIManager.mInstance.ShowTrialInGamePopUp(curLvl);
			}
		}
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mWinBg = base.transform.Find("winBG");
		this.mWinBgSp = this.mWinBg.GetComponent<UISprite>();
		this.mGUITrialRewardItemTable = this.mWinBg.Find("contentsPanel/contents").gameObject.AddComponent<GUITrialRewardItemTable>();
		this.mGUITrialRewardItemTable.maxPerLine = 20;
		this.mGUITrialRewardItemTable.arrangement = UICustomGrid.Arrangement.Horizontal;
		this.mGUITrialRewardItemTable.cellWidth = 84f;
		this.mGUITrialRewardItemTable.cellHeight = 84f;
		this.mGUITrialRewardItemTable.gapWidth = 2f;
		this.mGUITrialRewardItemTable.gapHeight = 0f;
		this.mWinBg.localScale = Vector3.zero;
		this.mWinBgSp.color = new Color(this.mWinBgSp.color.r, this.mWinBgSp.color.g, this.mWinBgSp.color.b, 0f);
	}

	public void ShowMe(int curLvl)
	{
		this.Refresh(curLvl);
		Sequence sequence = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate).OnComplete(new TweenDelegate.TweenCallback(this.OnAnimEnd)));
		sequence.Append(HOTween.To(this.mWinBgSp, 0.25f, new TweenParms().Prop("color", new Color(this.mWinBgSp.color.r, this.mWinBgSp.color.g, this.mWinBgSp.color.b, 1f))));
		sequence.Insert(0f, HOTween.To(this.mWinBg, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack)));
		sequence.AppendInterval(3f);
		sequence.Append(HOTween.To(this.mWinBgSp, 0.25f, new TweenParms().Prop("color", new Color(this.mWinBgSp.color.r, this.mWinBgSp.color.g, this.mWinBgSp.color.b, 0f))));
		sequence.Play();
	}

	private void OnAnimEnd()
	{
		GameUIManager.mInstance.DestroyTrialInGamePopUp();
	}

	private void Refresh(int curLvl)
	{
		TrialInfo info = Globals.Instance.AttDB.TrialDict.GetInfo(curLvl);
		if (info != null)
		{
			for (int i = 0; i < info.RewardType.Count; i++)
			{
				int num = info.RewardType[i];
				if (num != 0)
				{
					if (num == 3)
					{
						if (i < info.RewardValue1.Count && i < info.RewardValue2.Count)
						{
							int num2 = info.RewardValue1[i];
							if (num2 != 0)
							{
								ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(num2);
								if (info2 != null)
								{
									this.mGUITrialRewardItemTable.AddData(new GUITrialRewardItemData(ERewardType.EReward_Item, info2, info.RewardValue2[i], 0));
								}
							}
						}
					}
					else if (num == 15)
					{
						if (i < info.RewardValue1.Count)
						{
							this.mGUITrialRewardItemTable.AddData(new GUITrialRewardItemData(ERewardType.EReward_Emblem, null, 0, info.RewardValue1[i]));
						}
					}
					else if (num == 17 && i < info.RewardValue1.Count)
					{
						this.mGUITrialRewardItemTable.AddData(new GUITrialRewardItemData(ERewardType.EReward_LopetSoul, null, 0, info.RewardValue1[i]));
					}
				}
			}
		}
	}
}
