using Proto;
using System;
using UnityEngine;

public class GuildCreateTabLayer : MonoBehaviour
{
	private UIInput mGuildName;

	private UIInput mGuildAnnounce;

	private UILabel mCostNum;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
		this.mCostNum.color = (((long)Globals.Instance.Player.Data.Diamond >= (long)((ulong)GameConst.GetInt32(147))) ? Color.white : Color.red);
		this.mCostNum.text = GameConst.GetInt32(147).ToString();
	}

	private void CreateObjects()
	{
		this.mGuildName = base.transform.Find("nameInput").GetComponent<UIInput>();
		UIInput expr_21 = this.mGuildName;
		expr_21.onValidate = (UIInput.OnValidate)Delegate.Combine(expr_21.onValidate, new UIInput.OnValidate(this.OnInputGuildName));
		this.mGuildAnnounce = base.transform.Find("announceInput").GetComponent<UIInput>();
		this.mCostNum = base.transform.Find("costNum").GetComponent<UILabel>();
		GameObject gameObject = base.transform.Find("createBtn").gameObject;
		UIEventListener expr_94 = UIEventListener.Get(gameObject);
		expr_94.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_94.onClick, new UIEventListener.VoidDelegate(this.OnCreateBtnClick));
	}

	public char OnInputGuildName(string text, int pos, char ch)
	{
		if (ch == ' ')
		{
			return '\0';
		}
		for (char c = '0'; c <= '9'; c += '\u0001')
		{
			if (c == ch)
			{
				return '\0';
			}
		}
		return ch;
	}

	private void OnCreateBtnClick(GameObject go)
	{
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(3)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("guild8", new object[]
			{
				GameConst.GetInt32(3)
			}), 0f, 0f);
			return;
		}
		int joinGuildCD = Globals.Instance.Player.GuildSystem.GetJoinGuildCD();
		if (joinGuildCD != 0)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("guild35", new object[]
			{
				Tools.FormatTimeStr3(joinGuildCD, false, true)
			}), 0f, 0f);
			return;
		}
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, GameConst.GetInt32(147), 0))
		{
			return;
		}
		if (string.IsNullOrEmpty(this.mGuildName.value))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guild1", 0f, 0f);
			return;
		}
		if (Tools.GetLength(this.mGuildName.value) > 12)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("EGR_11", 0f, 0f);
			return;
		}
		if (Tools.GetLength(this.mGuildAnnounce.value) > 25)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("EGR_4", 0f, 0f);
			return;
		}
		MC2S_CreateGuild mC2S_CreateGuild = new MC2S_CreateGuild();
		mC2S_CreateGuild.Name = this.mGuildName.value;
		mC2S_CreateGuild.Manifesto = this.mGuildAnnounce.value;
		Globals.Instance.CliSession.Send(903, mC2S_CreateGuild);
	}
}
