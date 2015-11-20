using Holoville.HOTween;
using Proto;
using System;
using UnityEngine;

public class GuildCraftSetTitle : MonoBehaviour
{
	private UILabel mRedName;

	private UILabel mRedScore;

	private UILabel mRedBgScore;

	private UILabel mBlueName;

	private UILabel mBlueScore;

	private UILabel mBlueBgScore;

	private int mOldRedScore;

	private int mOldBlueScore;

	private UILabel mCDTime;

	private bool mRequestEnd;

	private int mLastCdTime = -1;

	private float mRefreshTimer;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
		this.mRefreshTimer = Time.time;
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("redBg");
		this.mRedName = transform.Find("name").GetComponent<UILabel>();
		this.mRedScore = base.transform.Find("numBg/num0").GetComponent<UILabel>();
		this.mRedBgScore = base.transform.Find("numBg/numbg0").GetComponent<UILabel>();
		this.mRedScore.text = "0";
		Transform transform2 = base.transform.Find("blueBg");
		this.mBlueName = transform2.Find("name").GetComponent<UILabel>();
		this.mBlueScore = base.transform.Find("numBg/num1").GetComponent<UILabel>();
		this.mBlueBgScore = base.transform.Find("numBg/numbg1").GetComponent<UILabel>();
		this.mBlueScore.text = "0";
		GameObject gameObject = base.transform.Find("cdBg").gameObject;
		this.mCDTime = gameObject.transform.Find("cdTime").GetComponent<UILabel>();
	}

	private Color GetScoreColor(int curScore)
	{
		int num = GameConst.GetInt32(220) / 6;
		if (curScore < num)
		{
			return GlobalColorTool.CraftWhite;
		}
		if (curScore < num * 2)
		{
			return GlobalColorTool.CraftGreen;
		}
		if (curScore < num * 3)
		{
			return GlobalColorTool.CraftLan;
		}
		if (curScore < num * 4)
		{
			return GlobalColorTool.CraftChengHuang;
		}
		if (curScore < num * 5)
		{
			return GlobalColorTool.CraftChengHong;
		}
		return GlobalColorTool.CraftRed;
	}

	public void Refresh(bool isInit)
	{
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData != null)
		{
			EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
			this.mRedName.text = mGWEnterData.Red.GuildName;
			if (selfTeamFlag == EGuildWarTeamId.EGWTI_Red)
			{
				this.mRedName.color = Color.green;
			}
			else
			{
				this.mRedName.color = Color.white;
			}
			string text = mGWEnterData.Red.Score.ToString();
			if (!isInit && !this.mRedScore.text.Equals(text))
			{
				Sequence sequence = new Sequence();
				sequence.Append(HOTween.To(this.mRedScore.gameObject.transform, 0.2f, new TweenParms().Prop("localScale", new Vector3(1.8f, 1.8f, 1.8f))));
				sequence.Append(HOTween.To(this.mRedScore.gameObject.transform, 0.2f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence.Play();
			}
			this.mRedScore.text = text;
			this.mRedScore.color = this.GetScoreColor(mGWEnterData.Red.Score);
			this.mRedBgScore.text = string.Format("/{0}", GameConst.GetInt32(220));
			this.mBlueName.text = mGWEnterData.Blue.GuildName;
			if (selfTeamFlag == EGuildWarTeamId.EGWTI_Blue)
			{
				this.mBlueName.color = Color.green;
			}
			else
			{
				this.mBlueName.color = Color.white;
			}
			string text2 = mGWEnterData.Blue.Score.ToString();
			if (!isInit && !this.mBlueScore.text.Equals(text2))
			{
				Sequence sequence2 = new Sequence();
				sequence2.Append(HOTween.To(this.mBlueScore.gameObject.transform, 0.2f, new TweenParms().Prop("localScale", new Vector3(1.8f, 1.8f, 1.8f))));
				sequence2.Append(HOTween.To(this.mBlueScore.gameObject.transform, 0.2f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence2.Play();
			}
			this.mBlueScore.text = text2;
			this.mBlueScore.color = this.GetScoreColor(mGWEnterData.Blue.Score);
			this.mBlueBgScore.text = string.Format("/{0}", GameConst.GetInt32(220));
		}
	}

	private void Update()
	{
		if (Time.time - this.mRefreshTimer >= 0.5f)
		{
			this.mRefreshTimer = Time.time;
			GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
			if (mWarStateInfo == null)
			{
				return;
			}
			GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
			if (mGWEnterData == null)
			{
				return;
			}
			if (mGWEnterData.Winner != EGuildWarTeamId.EGWTI_None)
			{
				return;
			}
			int num = mWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (num < 0)
			{
				num = 0;
			}
			this.mCDTime.text = Tools.FormatTime2(num);
			if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
			{
				if (this.mLastCdTime != num)
				{
					this.mLastCdTime = num;
					if (this.mLastCdTime == 3)
					{
						GameUIManager.mInstance.ShowBattleCDMsg(null);
					}
				}
				if (num <= 0)
				{
					Globals.Instance.Player.GuildSystem.RequestWarUpdate();
				}
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
			{
				if (num <= 0)
				{
					Globals.Instance.Player.GuildSystem.RequestWarUpdate();
				}
			}
			else if ((mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourEnd || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalEnd) && num <= 0 && !this.mRequestEnd)
			{
				this.mRequestEnd = true;
				Globals.Instance.Player.GuildSystem.RequestWarUpdate();
			}
		}
	}
}
