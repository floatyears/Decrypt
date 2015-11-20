using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GUIGuildMagicPopUp : GameUIBasePopup
{
	private const int mJinDuNum = 4;

	private UISlider mJinDuBar;

	private UISprite[] mBoxes = new UISprite[4];

	private UILabel[] mBoxesNum = new UILabel[4];

	private GameObject[] mBoxEffects = new GameObject[4];

	private UILabel mCurJinDuNum;

	private UILabel mPeopleNum;

	private GUIGuildMagicItem[] mGuildMagicItems = new GUIGuildMagicItem[3];

	private GUIGuildMagicRewardPopUp mGUIGuildMagicRewardPopUp;

	private GUIGuildMagicTipPopUp mGUIGuildMagicTipPopUp;

	private StringBuilder mSb = new StringBuilder(42);

	private void Awake()
	{
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("WindowBg");
		this.mJinDuBar = transform.Find("Slider").GetComponent<UISlider>();
		Transform transform2 = this.mJinDuBar.transform;
		for (int i = 0; i < 4; i++)
		{
			this.mBoxes[i] = transform2.Find(string.Format("c{0}", i)).GetComponent<UISprite>();
			UIEventListener expr_6F = UIEventListener.Get(this.mBoxes[i].gameObject);
			expr_6F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6F.onClick, new UIEventListener.VoidDelegate(this.OnBoxClicked));
			this.mBoxesNum[i] = this.mBoxes[i].transform.Find("num").GetComponent<UILabel>();
			this.mBoxEffects[i] = this.mBoxes[i].transform.Find("ui65").gameObject;
			Tools.SetParticleRenderQueue2(this.mBoxEffects[i], 5500);
			NGUITools.SetActive(this.mBoxEffects[i], false);
		}
		this.mCurJinDuNum = transform2.Find("txt0/num").GetComponent<UILabel>();
		this.mPeopleNum = transform2.Find("txt1/num").GetComponent<UILabel>();
		for (int j = 0; j < 3; j++)
		{
			this.mGuildMagicItems[j] = transform.Find(j.ToString()).gameObject.AddComponent<GUIGuildMagicItem>();
			this.mGuildMagicItems[j].InitWithBaseScene(this, j);
		}
		GameObject gameObject = transform.Find("closebtn").gameObject;
		UIEventListener expr_188 = UIEventListener.Get(gameObject);
		expr_188.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_188.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		GameObject gameObject2 = transform.Find("logGuild").gameObject;
		UIEventListener expr_1C2 = UIEventListener.Get(gameObject2);
		expr_1C2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C2.onClick, new UIEventListener.VoidDelegate(this.OnLogBtnClick));
		this.mGUIGuildMagicRewardPopUp = base.transform.Find("RewardPopUp").gameObject.AddComponent<GUIGuildMagicRewardPopUp>();
		this.mGUIGuildMagicRewardPopUp.InitWithBaseScene();
		this.mGUIGuildMagicRewardPopUp.gameObject.SetActive(false);
		this.mGUIGuildMagicTipPopUp = base.transform.Find("TipPopUp").gameObject.AddComponent<GUIGuildMagicTipPopUp>();
		this.mGUIGuildMagicTipPopUp.InitWithBaseScene();
		this.mGUIGuildMagicTipPopUp.gameObject.SetActive(false);
		Globals.Instance.CliSession.Register(933, new ClientSession.MsgHandler(this.OnMsgTakeScoreReward));
		Globals.Instance.CliSession.Register(931, new ClientSession.MsgHandler(this.OnMsgGuildSign));
		GuildSubSystem expr_2AA = Globals.Instance.Player.GuildSystem;
		expr_2AA.SignRecordsEvent = (GuildSubSystem.VoidCallback)Delegate.Combine(expr_2AA.SignRecordsEvent, new GuildSubSystem.VoidCallback(this.OnSignRecords));
	}

	private bool IsBoxRewardTaken(int index)
	{
		return (Globals.Instance.Player.Data.GuildScoreRewardFlag & 1 << index) != 0;
	}

	private void Refresh()
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(Globals.Instance.Player.GuildSystem.Guild.Level);
			if (info != null)
			{
				int num = info.Score * 5;
				this.mJinDuBar.value = ((num == 0) ? 0f : ((float)Globals.Instance.Player.GuildSystem.Guild.Score / (float)num));
				this.mCurJinDuNum.text = Globals.Instance.Player.GuildSystem.Guild.Score.ToString();
				this.mPeopleNum.text = this.mSb.Remove(0, this.mSb.Length).Append(Globals.Instance.Player.GuildSystem.Guild.SignNum).Append("/").Append(Globals.Instance.Player.GuildSystem.Members.Count).ToString();
				for (int i = 0; i < 4; i++)
				{
					this.mBoxesNum[i].text = (info.Score * (i + 1)).ToString();
					this.mBoxes[i].spriteName = ((!this.IsBoxRewardTaken(i)) ? "chest" : "chest_open");
					this.mBoxEffects[i].SetActive(info.Score * (i + 1) <= Globals.Instance.Player.GuildSystem.Guild.Score && !this.IsBoxRewardTaken(i));
				}
			}
			for (int j = 0; j < 3; j++)
			{
				this.mGuildMagicItems[j].Refresh();
			}
		}
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		Globals.Instance.CliSession.Unregister(933, new ClientSession.MsgHandler(this.OnMsgTakeScoreReward));
		Globals.Instance.CliSession.Unregister(931, new ClientSession.MsgHandler(this.OnMsgGuildSign));
		GuildSubSystem expr_60 = Globals.Instance.Player.GuildSystem;
		expr_60.SignRecordsEvent = (GuildSubSystem.VoidCallback)Delegate.Remove(expr_60.SignRecordsEvent, new GuildSubSystem.VoidCallback(this.OnSignRecords));
	}

	private void OnBoxClicked(GameObject go)
	{
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(Globals.Instance.Player.GuildSystem.Guild.Level);
		if (info != null)
		{
			string name = go.name;
			if (name.Contains("0") && !this.IsBoxRewardTaken(0))
			{
				if (Globals.Instance.Player.GuildSystem.Guild.Score >= info.Score * 1)
				{
					MC2S_TakeScoreReward mC2S_TakeScoreReward = new MC2S_TakeScoreReward();
					mC2S_TakeScoreReward.Index = 0;
					Globals.Instance.CliSession.Send(932, mC2S_TakeScoreReward);
				}
				else
				{
					this.mGUIGuildMagicRewardPopUp.ShowMe(0);
				}
			}
			else if (name.Contains("1") && !this.IsBoxRewardTaken(1))
			{
				if (Globals.Instance.Player.GuildSystem.Guild.Score >= info.Score * 2)
				{
					MC2S_TakeScoreReward mC2S_TakeScoreReward2 = new MC2S_TakeScoreReward();
					mC2S_TakeScoreReward2.Index = 1;
					Globals.Instance.CliSession.Send(932, mC2S_TakeScoreReward2);
				}
				else
				{
					this.mGUIGuildMagicRewardPopUp.ShowMe(1);
				}
			}
			else if (name.Contains("2") && !this.IsBoxRewardTaken(2))
			{
				if (Globals.Instance.Player.GuildSystem.Guild.Score >= info.Score * 3)
				{
					MC2S_TakeScoreReward mC2S_TakeScoreReward3 = new MC2S_TakeScoreReward();
					mC2S_TakeScoreReward3.Index = 2;
					Globals.Instance.CliSession.Send(932, mC2S_TakeScoreReward3);
				}
				else
				{
					this.mGUIGuildMagicRewardPopUp.ShowMe(2);
				}
			}
			else if (name.Contains("3") && !this.IsBoxRewardTaken(3))
			{
				if (Globals.Instance.Player.GuildSystem.Guild.Score >= info.Score * 4)
				{
					MC2S_TakeScoreReward mC2S_TakeScoreReward4 = new MC2S_TakeScoreReward();
					mC2S_TakeScoreReward4.Index = 3;
					Globals.Instance.CliSession.Send(932, mC2S_TakeScoreReward4);
				}
				else
				{
					this.mGUIGuildMagicRewardPopUp.ShowMe(3);
				}
			}
		}
	}

	private void OnCloseBtnClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnMsgTakeScoreReward(MemoryStream stream)
	{
		MS2C_TakeScoreReward mS2C_TakeScoreReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeScoreReward), stream) as MS2C_TakeScoreReward;
		if (mS2C_TakeScoreReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_TakeScoreReward.Result);
			return;
		}
		this.Refresh();
		List<RewardData> list = new List<RewardData>();
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(Globals.Instance.Player.GuildSystem.Guild.Level);
		if (info != null)
		{
			if (mS2C_TakeScoreReward.Index == 0)
			{
				for (int i = 0; i < info.Score1RewardType.Count; i++)
				{
					if (info.Score1RewardType[i] != 0)
					{
						list.Add(new RewardData
						{
							RewardType = info.Score1RewardType[i],
							RewardValue1 = info.Score1RewardValue1[i],
							RewardValue2 = info.Score1RewardValue2[i]
						});
					}
				}
			}
			else if (mS2C_TakeScoreReward.Index == 1)
			{
				for (int j = 0; j < info.Score2RewardType.Count; j++)
				{
					if (info.Score2RewardType[j] != 0)
					{
						list.Add(new RewardData
						{
							RewardType = info.Score2RewardType[j],
							RewardValue1 = info.Score2RewardValue1[j],
							RewardValue2 = info.Score2RewardValue2[j]
						});
					}
				}
			}
			else if (mS2C_TakeScoreReward.Index == 2)
			{
				for (int k = 0; k < info.Score3RewardType.Count; k++)
				{
					if (info.Score3RewardType[k] != 0)
					{
						list.Add(new RewardData
						{
							RewardType = info.Score3RewardType[k],
							RewardValue1 = info.Score3RewardValue1[k],
							RewardValue2 = info.Score3RewardValue2[k]
						});
					}
				}
			}
			else if (mS2C_TakeScoreReward.Index == 3)
			{
				for (int l = 0; l < info.Score4RewardType.Count; l++)
				{
					if (info.Score4RewardType[l] != 0)
					{
						list.Add(new RewardData
						{
							RewardType = info.Score4RewardType[l],
							RewardValue1 = info.Score4RewardValue1[l],
							RewardValue2 = info.Score4RewardValue2[l]
						});
					}
				}
			}
			GUIRewardPanel.Show(list, null, false, true, null, false);
		}
	}

	private void OnMsgGuildSign(MemoryStream stream)
	{
		MS2C_GuildSign mS2C_GuildSign = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildSign), stream) as MS2C_GuildSign;
		if (mS2C_GuildSign.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GuildSign.Result);
			return;
		}
		this.Refresh();
	}

	public void ShowTipPopUp(int index)
	{
		this.mGUIGuildMagicTipPopUp.ShowMe(index);
	}

	private void OnLogBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_QueryGuildSignRecord mC2S_QueryGuildSignRecord = new MC2S_QueryGuildSignRecord();
		mC2S_QueryGuildSignRecord.Version = Globals.Instance.Player.GuildSystem.mSignRecordVersion;
		Globals.Instance.CliSession.Send(956, mC2S_QueryGuildSignRecord);
	}

	private void OnSignRecords()
	{
		GUIGuildSignLogPopUp.ShowMe();
	}
}
