using Proto;
using ProtoBuf;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class GUIGuildMagicTipPopUp : MonoBehaviour
{
	private int mIndex;

	private UILabel mTipTitle;

	private UILabel mTxt0;

	private UILabel mTxt1;

	private UILabel mTxt2;

	private UILabel mTxt3;

	private UILabel mCostTip;

	private UILabel mCostNum;

	private UISprite mCostSp;

	private int[] mJinDuNums = new int[]
	{
		1,
		3,
		5
	};

	private int[] mHuoYueNums = new int[]
	{
		5,
		5,
		5
	};

	private int[] mExpNums = new int[]
	{
		10,
		50,
		300
	};

	private int[] mGongXianNums = new int[]
	{
		100,
		500,
		3000
	};

	private string mGuild30Str;

	private string mGuild31Str;

	private string mGuild32Str;

	private StringBuilder mSb = new StringBuilder(42);

	public void ShowMe(int index)
	{
		this.InitPopUp(index);
		Globals.Instance.CliSession.Register(931, new ClientSession.MsgHandler(this.OnMsgGuildSign));
		base.gameObject.SetActive(true);
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void InitPopUp(int index)
	{
		this.mIndex = index;
		this.Refresh();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("WindowBg");
		this.mTipTitle = transform.Find("title").GetComponent<UILabel>();
		this.mTxt0 = transform.Find("txt0").GetComponent<UILabel>();
		this.mTxt1 = transform.Find("txt1").GetComponent<UILabel>();
		this.mTxt2 = transform.Find("txt2").GetComponent<UILabel>();
		this.mTxt3 = transform.Find("txt3").GetComponent<UILabel>();
		this.mCostTip = transform.Find("costTip").GetComponent<UILabel>();
		this.mCostSp = transform.Find("costSp").GetComponent<UISprite>();
		this.mCostNum = transform.Find("costNum").GetComponent<UILabel>();
		GameObject gameObject = transform.Find("sureBtn").gameObject;
		UIEventListener expr_D8 = UIEventListener.Get(gameObject);
		expr_D8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D8.onClick, new UIEventListener.VoidDelegate(this.OnSureBtnClick));
		GameObject gameObject2 = transform.Find("closebtn").gameObject;
		UIEventListener expr_110 = UIEventListener.Get(gameObject2);
		expr_110.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_110.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		GameObject gameObject3 = base.transform.Find("background").gameObject;
		UIEventListener expr_14D = UIEventListener.Get(gameObject3);
		expr_14D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_14D.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mGuild30Str = Singleton<StringManager>.Instance.GetString("guild30");
		this.mGuild31Str = Singleton<StringManager>.Instance.GetString("guild31");
		this.mGuild32Str = Singleton<StringManager>.Instance.GetString("guild32");
	}

	private void DoCloseWnd()
	{
		Globals.Instance.CliSession.Unregister(931, new ClientSession.MsgHandler(this.OnMsgGuildSign));
		base.gameObject.SetActive(false);
	}

	private void OnCloseBtnClick(GameObject go)
	{
		this.DoCloseWnd();
	}

	private void OnSureBtnClick(GameObject go)
	{
		if (this.mIndex == 0)
		{
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Money, GameConst.GetInt32(154), 0))
			{
				this.DoCloseWnd();
				GameUIPopupManager.GetInstance().PopState(true, null);
				return;
			}
		}
		else if (this.mIndex == 1)
		{
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(159), 0))
			{
				this.DoCloseWnd();
				GameUIPopupManager.GetInstance().PopState(true, null);
				return;
			}
		}
		else if (this.mIndex == 2 && Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(164), 0))
		{
			this.DoCloseWnd();
			GameUIPopupManager.GetInstance().PopState(true, null);
			return;
		}
		MC2S_GuildSign mC2S_GuildSign = new MC2S_GuildSign();
		mC2S_GuildSign.Type = this.mIndex + 1;
		Globals.Instance.CliSession.Send(930, mC2S_GuildSign);
	}

	private void Refresh()
	{
		ActivityValueData valueMod = Globals.Instance.Player.ActivitySystem.GetValueMod(8);
		bool flag = valueMod != null;
		switch (this.mIndex)
		{
		case 0:
			this.mTipTitle.text = this.mGuild30Str;
			this.mTxt0.text = this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("guild29"), this.mJinDuNums[0]).ToString();
			this.mTxt1.text = this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("guild19"), this.mHuoYueNums[0]).ToString();
			this.mTxt3.text = this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("guild17"), this.mExpNums[0]).ToString();
			this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("guild20"), (!flag) ? this.mGongXianNums[0] : (this.mGongXianNums[0] * valueMod.Value1 / 100));
			if (flag)
			{
				this.mSb.Append("(").Append(Singleton<StringManager>.Instance.GetString("ShopCommon6")).Append(")");
			}
			this.mTxt2.text = this.mSb.ToString();
			this.mCostTip.text = this.mSb.Remove(0, this.mSb.Length).Append(this.mGuild30Str).Append(Singleton<StringManager>.Instance.GetString("guild33")).ToString();
			this.mCostSp.spriteName = "Gold_1";
			this.mCostNum.text = GameConst.GetInt32(154).ToString();
			this.mCostNum.color = ((Globals.Instance.Player.Data.Money < GameConst.GetInt32(154)) ? Color.red : Color.white);
			break;
		case 1:
			this.mTipTitle.text = this.mGuild31Str;
			this.mTxt0.text = this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("guild29"), this.mJinDuNums[1]).ToString();
			this.mTxt1.text = this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("guild19"), this.mHuoYueNums[1]).ToString();
			this.mTxt3.text = this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("guild17"), this.mExpNums[1]).ToString();
			this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("guild20"), (!flag) ? this.mGongXianNums[1] : (this.mGongXianNums[1] * 2));
			if (flag)
			{
				this.mSb.Append("(").Append(Singleton<StringManager>.Instance.GetString("ShopCommon6")).Append(")");
			}
			this.mTxt2.text = this.mSb.ToString();
			this.mCostTip.text = this.mSb.Remove(0, this.mSb.Length).Append(this.mGuild31Str).Append(Singleton<StringManager>.Instance.GetString("guild33")).ToString();
			this.mCostSp.spriteName = "redGem_1";
			this.mCostNum.text = GameConst.GetInt32(159).ToString();
			this.mCostNum.color = ((Globals.Instance.Player.Data.Diamond < GameConst.GetInt32(159)) ? Color.red : Color.white);
			break;
		case 2:
			this.mTipTitle.text = this.mGuild32Str;
			this.mTxt0.text = this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("guild29"), this.mJinDuNums[2]).ToString();
			this.mTxt1.text = this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("guild19"), this.mHuoYueNums[2]).ToString();
			this.mTxt3.text = this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("guild17"), this.mExpNums[2]).ToString();
			this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("guild20"), (!flag) ? this.mGongXianNums[2] : (this.mGongXianNums[2] * 2));
			if (flag)
			{
				this.mSb.Append("(").Append(Singleton<StringManager>.Instance.GetString("ShopCommon6")).Append(")");
			}
			this.mTxt2.text = this.mSb.ToString();
			this.mCostTip.text = this.mSb.Remove(0, this.mSb.Length).Append(this.mGuild32Str).Append(Singleton<StringManager>.Instance.GetString("guild33")).ToString();
			this.mCostSp.spriteName = "redGem_1";
			this.mCostNum.text = GameConst.GetInt32(164).ToString();
			this.mCostNum.color = ((Globals.Instance.Player.Data.Diamond < GameConst.GetInt32(164)) ? Color.red : Color.white);
			break;
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
		this.DoCloseWnd();
	}
}
