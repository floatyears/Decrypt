using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Proto;
using System;
using UnityEngine;

public class GUITrailTowerSucV2 : GameUISession
{
	private GameObject mTitle;

	private GameObject mMaxRecord;

	private GameObject mMaxRecordEffect;

	private GameObject mMidBg;

	private GameObject mSureBtn;

	private UILabel mMaxRecordLb;

	private UILabel mTongGuanWaveLb;

	private UILabel mMoJingLb;

	private UILabel mMoneyLb;

	public int mMaxRecordNum;

	public int mWaveNum;

	public int mMoJingNum;

	public int mMoneyNum;

	private GUITrialRewardItemTable mGUITrialRewardItemTable;

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle");
		this.mTitle = transform.Find("battleOver").gameObject;
		this.mTitle.transform.localScale = Vector3.zero;
		this.mMaxRecord = transform.Find("maxRecord").gameObject;
		this.mMaxRecord.transform.localScale = Vector3.zero;
		this.mMaxRecordEffect = this.mMaxRecord.transform.Find("ui21").gameObject;
		Tools.SetParticleRenderQueue2(this.mMaxRecordEffect, 5500);
		NGUITools.SetActive(this.mMaxRecordEffect, false);
		this.mMidBg = transform.Find("middleInfo").gameObject;
		this.mMidBg.transform.localScale = Vector3.zero;
		Transform transform2 = this.mMidBg.transform;
		this.mMaxRecordLb = transform2.Find("txt0/num").GetComponent<UILabel>();
		this.mMaxRecordLb.text = "0";
		this.mMaxRecordNum = 0;
		this.mTongGuanWaveLb = transform2.Find("txt1/num").GetComponent<UILabel>();
		this.mTongGuanWaveLb.text = "0";
		this.mWaveNum = 0;
		this.mMoJingLb = transform2.Find("txt2/num").GetComponent<UILabel>();
		this.mMoJingLb.text = "0";
		this.mMoJingNum = 0;
		this.mMoneyLb = transform2.Find("txt3/num").GetComponent<UILabel>();
		this.mMoneyLb.text = "0";
		this.mMoneyNum = 0;
		this.mGUITrialRewardItemTable = transform2.Find("contentsPanel/contents").gameObject.AddComponent<GUITrialRewardItemTable>();
		this.mGUITrialRewardItemTable.maxPerLine = 20;
		this.mGUITrialRewardItemTable.arrangement = UICustomGrid.Arrangement.Horizontal;
		this.mGUITrialRewardItemTable.cellWidth = 84f;
		this.mGUITrialRewardItemTable.cellHeight = 84f;
		this.mGUITrialRewardItemTable.gapWidth = 2f;
		this.mGUITrialRewardItemTable.gapHeight = 0f;
		this.mGUITrialRewardItemTable.transform.localScale = Vector3.zero;
		this.mSureBtn = transform.Find("sureBtn").gameObject;
		UIEventListener expr_238 = UIEventListener.Get(this.mSureBtn);
		expr_238.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_238.onClick, new UIEventListener.VoidDelegate(this.OnSureBtnClick));
		this.mSureBtn.transform.localScale = Vector3.zero;
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		Globals.Instance.BackgroundMusicMgr.StopWarmingSound();
		Globals.Instance.BackgroundMusicMgr.ClearGameBGM();
		Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("ui/ui_004b", false);
		this.Refresh();
	}

	protected override void OnPreDestroyGUI()
	{
	}

	private void Refresh()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (GameUIManager.mInstance.uiState.TrailCurLvl < Globals.Instance.Player.Data.TrialWave)
		{
			for (int i = GameUIManager.mInstance.uiState.TrailCurLvl + 1; i <= Globals.Instance.Player.Data.TrialWave; i++)
			{
				TrialInfo info = Globals.Instance.AttDB.TrialDict.GetInfo(i);
				if (info != null)
				{
					num += info.Value;
					num2 += info.Money;
					for (int j = 0; j < info.RewardType.Count; j++)
					{
						int num4 = info.RewardType[j];
						if (num4 != 0)
						{
							if (num4 == 3)
							{
								if (j < info.RewardValue1.Count && j < info.RewardValue2.Count)
								{
									int num5 = info.RewardValue1[j];
									if (num5 != 0)
									{
										ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(num5);
										if (info2 != null)
										{
											this.mGUITrialRewardItemTable.AddData(new GUITrialRewardItemData(ERewardType.EReward_Item, info2, info.RewardValue2[j], 0));
											num3++;
										}
									}
								}
							}
							else if (num4 == 15)
							{
								if (j < info.RewardValue1.Count)
								{
									this.mGUITrialRewardItemTable.AddData(new GUITrialRewardItemData(ERewardType.EReward_Item, null, 0, info.RewardValue1[j]));
								}
							}
							else if (num4 == 17 && j < info.RewardValue1.Count)
							{
								this.mGUITrialRewardItemTable.AddData(new GUITrialRewardItemData(ERewardType.EReward_LopetSoul, null, 0, info.RewardValue1[j]));
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
		Sequence sequence = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate));
		sequence.Append(HOTween.To(this, 1f, new TweenParms().Prop("mMaxRecordNum", Globals.Instance.Player.Data.TrialMaxWave).OnUpdate(new TweenDelegate.TweenCallback(this.OnMaxRecordNumUpdate))));
		sequence.Insert(0f, HOTween.To(this, 1f, new TweenParms().Prop("mWaveNum", Globals.Instance.Player.Data.TrialWave).OnUpdate(new TweenDelegate.TweenCallback(this.OnWaveNumUpdate))));
		sequence.Insert(0f, HOTween.To(this, 1f, new TweenParms().Prop("mMoJingNum", num).OnUpdate(new TweenDelegate.TweenCallback(this.OnMoJingNumUpdate))));
		sequence.Insert(0f, HOTween.To(this, 1f, new TweenParms().Prop("mMoneyNum", num2).OnUpdate(new TweenDelegate.TweenCallback(this.OnMoneyNumUpdate))));
		Sequence sequence2 = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate));
		sequence2.AppendInterval(0.2f);
		sequence2.Append(HOTween.To(this.mTitle.transform, 0.2f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack)));
		sequence2.AppendInterval(0.2f);
		sequence2.Append(HOTween.To(this.mMidBg.transform, 0.2f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack)));
		sequence2.AppendInterval(0.2f);
		sequence2.Append(sequence);
		if (num3 > 0)
		{
			sequence2.AppendInterval(0.1f);
			sequence2.Append(HOTween.To(this.mGUITrialRewardItemTable.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
		}
		if (Globals.Instance.Player.Data.TrialMaxWave > GameUIManager.mInstance.uiState.TrailMaxRecord)
		{
			sequence2.AppendInterval(0.2f);
			sequence2.Append(HOTween.To(this.mMaxRecord.transform, 0.001f, new TweenParms().Prop("localScale", new Vector3(8f, 8f, 8f))));
			sequence2.Append(HOTween.To(this.mMaxRecord.transform, 0.2f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseInCubic)));
			sequence2.AppendCallback(new TweenDelegate.TweenCallback(this.ShowMaxRecordEffect));
		}
		sequence2.AppendInterval(0.2f);
		sequence2.Append(HOTween.To(this.mSureBtn.transform, 0.1f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack)));
		sequence2.Play();
	}

	private void ShowMaxRecordEffect()
	{
		NGUITools.SetActive(this.mMaxRecordEffect, false);
		NGUITools.SetActive(this.mMaxRecordEffect, true);
	}

	private void OnMaxRecordNumUpdate()
	{
		this.mMaxRecordLb.text = this.mMaxRecordNum.ToString();
	}

	private void OnWaveNumUpdate()
	{
		this.mTongGuanWaveLb.text = this.mWaveNum.ToString();
	}

	private void OnMoJingNumUpdate()
	{
		this.mMoJingLb.text = this.mMoJingNum.ToString();
	}

	private void OnMoneyNumUpdate()
	{
		this.mMoneyLb.text = this.mMoneyNum.ToString();
	}

	private void OnSureBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIManager.mInstance.ChangeSession<GUITrailTowerSceneV2>(null, true, true);
	}
}
