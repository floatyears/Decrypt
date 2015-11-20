using Att;
using Proto;
using System;
using UnityEngine;

public class GUIGuildMagicRewardPopUp : MonoBehaviour
{
	private int mIndex;

	private GameObject mSureBtn;

	private GUIMagicRewardItem[] mRewardItems = new GUIMagicRewardItem[4];

	public void ShowMe(int index)
	{
		this.InitPopUp(index);
		base.gameObject.SetActive(true);
	}

	private void InitPopUp(int index)
	{
		this.mIndex = index;
		this.Refresh();
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("WindowBg");
		Transform transform2 = transform.Find("itemBg");
		for (int i = 0; i < 4; i++)
		{
			this.mRewardItems[i] = transform2.Find(string.Format("item{0}", i)).gameObject.AddComponent<GUIMagicRewardItem>();
			this.mRewardItems[i].InitWithBaseScene();
		}
		this.mSureBtn = transform.Find("sureBtn").gameObject;
		UIEventListener expr_85 = UIEventListener.Get(this.mSureBtn);
		expr_85.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_85.onClick, new UIEventListener.VoidDelegate(this.OnSureBtnClick));
		GameObject gameObject = transform.Find("closebtn").gameObject;
		UIEventListener expr_BD = UIEventListener.Get(gameObject);
		expr_BD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_BD.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		GameObject gameObject2 = base.transform.Find("background").gameObject;
		UIEventListener expr_FC = UIEventListener.Get(gameObject2);
		expr_FC.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_FC.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
	}

	private void Refresh()
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(Globals.Instance.Player.GuildSystem.Guild.Level);
			if (info != null)
			{
				int i = 0;
				if (this.mIndex == 0)
				{
					for (int j = 0; j < info.Score1RewardType.Count; j++)
					{
						if (info.Score1RewardType[j] != 0)
						{
							if (i < 4)
							{
								this.mRewardItems[i].Refresh(info.Score1RewardType[j], info.Score1RewardValue1[j], info.Score1RewardValue2[j]);
								i++;
							}
						}
					}
				}
				else if (this.mIndex == 1)
				{
					for (int k = 0; k < info.Score2RewardType.Count; k++)
					{
						if (info.Score2RewardType[k] != 0)
						{
							if (i < 4)
							{
								this.mRewardItems[i].Refresh(info.Score2RewardType[k], info.Score2RewardValue1[k], info.Score2RewardValue2[k]);
								i++;
							}
						}
					}
				}
				else if (this.mIndex == 2)
				{
					for (int l = 0; l < info.Score3RewardType.Count; l++)
					{
						if (info.Score3RewardType[l] != 0)
						{
							if (i < 4)
							{
								this.mRewardItems[i].Refresh(info.Score3RewardType[l], info.Score3RewardValue1[l], info.Score3RewardValue2[l]);
								i++;
							}
						}
					}
				}
				else if (this.mIndex == 3)
				{
					for (int m = 0; m < info.Score4RewardType.Count; m++)
					{
						if (info.Score4RewardType[m] != 0)
						{
							if (i < 4)
							{
								this.mRewardItems[i].Refresh(info.Score4RewardType[m], info.Score4RewardValue1[m], info.Score4RewardValue2[m]);
								i++;
							}
						}
					}
				}
				while (i < 4)
				{
					this.mRewardItems[i].gameObject.SetActive(false);
					i++;
				}
				this.mSureBtn.SetActive(Globals.Instance.Player.GuildSystem.Guild.Score >= info.Score * (this.mIndex + 1));
			}
		}
	}

	private void OnSureBtnClick(GameObject go)
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(Globals.Instance.Player.GuildSystem.Guild.Level);
			if (info != null && Globals.Instance.Player.GuildSystem.Guild.Score >= info.Score * (this.mIndex + 1))
			{
				MC2S_TakeScoreReward mC2S_TakeScoreReward = new MC2S_TakeScoreReward();
				mC2S_TakeScoreReward.Index = this.mIndex;
				Globals.Instance.CliSession.Send(932, mC2S_TakeScoreReward);
			}
		}
	}

	private void DoCloseWnd()
	{
		base.gameObject.SetActive(false);
	}

	private void OnCloseBtnClick(GameObject go)
	{
		this.DoCloseWnd();
	}
}
