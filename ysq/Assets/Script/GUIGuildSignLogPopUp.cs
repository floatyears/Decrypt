using Holoville.HOTween.Core;
using Proto;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GUIGuildSignLogPopUp : GameUIBasePopup
{
	private static GUIGuildSignLogPopUp mInstance;

	private Transform mWinBG;

	private UILabel mContent;

	private string mGuildLog60Str;

	private string mGuild30Str;

	private string mGuild31Str;

	private string mGuild32Str;

	private StringBuilder mSb = new StringBuilder(42);

	public static void ShowMe()
	{
		if (GUIGuildSignLogPopUp.mInstance == null)
		{
			GUIGuildSignLogPopUp.CreateInstance();
		}
		GUIGuildSignLogPopUp.mInstance.Init();
	}

	private static void CreateInstance()
	{
		if (GUIGuildSignLogPopUp.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIGuildSignLogPopUp");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIGuildSignLogPopUp error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIGuildSignLogPopUp error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 1000f);
		GUIGuildSignLogPopUp.mInstance = gameObject2.AddComponent<GUIGuildSignLogPopUp>();
	}

	public static bool TryClose()
	{
		if (GUIGuildSignLogPopUp.mInstance != null)
		{
			GUIGuildSignLogPopUp.mInstance.OnCloseBtnClick(null);
			return true;
		}
		return false;
	}

	public void Init()
	{
		GameUITools.PlayOpenWindowAnim(this.mWinBG, null, true);
	}

	private void Awake()
	{
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		this.mWinBG = base.transform.Find("winBg");
		GameObject gameObject = this.mWinBG.Find("closeBtn").gameObject;
		UIEventListener expr_32 = UIEventListener.Get(gameObject);
		expr_32.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_32.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		GameObject gameObject2 = base.transform.Find("FadeBG").gameObject;
		UIEventListener expr_6F = UIEventListener.Get(gameObject2);
		expr_6F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6F.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mContent = this.mWinBG.Find("contentsBg/contentsPanel/contents/contentTxt").GetComponent<UILabel>();
		this.mGuildLog60Str = Singleton<StringManager>.Instance.GetString("guildLog60");
		this.mGuild30Str = Singleton<StringManager>.Instance.GetString("guild30");
		this.mGuild31Str = Singleton<StringManager>.Instance.GetString("guild31");
		this.mGuild32Str = Singleton<StringManager>.Instance.GetString("guild32");
	}

	private void Refresh()
	{
		this.mContent.text = string.Empty;
		List<GuildSignRecord> mSignRecords = Globals.Instance.Player.GuildSystem.mSignRecords;
		if (mSignRecords == null)
		{
			return;
		}
		this.mSb.Remove(0, this.mSb.Length);
		for (int i = 0; i < mSignRecords.Count; i++)
		{
			GuildSignRecord guildSignRecord = mSignRecords[i];
			if (guildSignRecord != null)
			{
				string empty = string.Empty;
				if (guildSignRecord.SignType == 3)
				{
					empty = this.mGuild32Str;
				}
				else if (guildSignRecord.SignType == 2)
				{
					empty = this.mGuild31Str;
				}
				else
				{
					empty = this.mGuild30Str;
				}
				this.mSb.Append("[9c8559]").Append(Tools.ServerDateTimeFormat4(guildSignRecord.Timestamp)).Append("[-] ").AppendFormat(this.mGuildLog60Str, guildSignRecord.Name, empty).AppendLine();
			}
		}
		this.mContent.text = this.mSb.ToString();
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.mWinBG.transform, new TweenDelegate.TweenCallback(this.CloseImmediate), true);
	}

	private void CloseImmediate()
	{
		if (GUIGuildSignLogPopUp.mInstance != null)
		{
			UnityEngine.Object.Destroy(GUIGuildSignLogPopUp.mInstance.gameObject);
			GUIGuildSignLogPopUp.mInstance = null;
		}
	}
}
