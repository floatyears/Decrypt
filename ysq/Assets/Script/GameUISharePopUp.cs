using Att;
using cn.sharesdk.unity3d;
using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GameUISharePopUp : GameUIBasePopup
{
	private GameObject rewardMoney;

	private UILabel content;

	private string contentTxt;

	private ShareAchievementDataEx shareData;

	private string imagePath = string.Empty;

	private static string screenshotName = "Screenshot";

	private bool flagShare;

	private bool focus;

	private float flagShareInitTime;

	public static bool isSharing
	{
		get;
		private set;
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBG");
		GameObject gameObject = transform.Find("titleBg/CloseBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		Transform transform2 = transform.Find("Sprite");
		int[] array = new int[]
		{
			22,
			23,
			1,
			39
		};
		for (int i = 0; i < 4; i++)
		{
			GameObject gameObject2 = transform2.Find(string.Format("Img{0}", i)).gameObject;
			gameObject2.name = array[i].ToString();
			UIEventListener expr_A8 = UIEventListener.Get(gameObject2);
			expr_A8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A8.onClick, new UIEventListener.VoidDelegate(this.OnShareBtnClick));
		}
		Transform transform3 = transform.Find("panel");
		this.content = transform3.Find("content").gameObject.GetComponent<UILabel>();
		this.rewardMoney = transform3.Find("RewardMoney").gameObject;
	}

	private void SendShareLog(int platform)
	{
        if (this.shareData != null)
        {
            MC2S_Share ojb = new MC2S_Share();
            switch (platform)
            {
                case 0x16:
                    ojb.ShareChannel = 1;
                    break;

                case 0x17:
                    ojb.ShareChannel = 3;
                    break;

                case 0x26:
                    ojb.ShareChannel = 2;
                    break;

                case 0x27:
                    ojb.ShareChannel = 4;
                    break;

                case 1:
                    ojb.ShareChannel = 5;
                    break;

                default:
                    ojb.ShareChannel = 0;
                    break;
            }
            switch (this.shareData.Info.ConditionType)
            {
                case 0x2b:
                    ojb.SharePoint = 5;
                    break;

                case 0x2e:
                    ojb.SharePoint = 7;
                    break;

                case 0x3b:
                    ojb.SharePoint = 8;
                    break;

                case 60:
                    ojb.SharePoint = 10;
                    break;

                case 0x3d:
                    ojb.SharePoint = 9;
                    break;

                case 0x3e:
                    ojb.SharePoint = 12;
                    break;

                case 0x12:
                    if (this.shareData.Info.Value == 30)
                    {
                        ojb.SharePoint = 1;
                    }
                    else if (this.shareData.Info.Value == 50)
                    {
                        ojb.SharePoint = 2;
                    }
                    else if (this.shareData.Info.Value == 80)
                    {
                        ojb.SharePoint = 11;
                    }
                    break;

                case 0x1c:
                    ojb.SharePoint = 6;
                    break;

                case 0x27:
                    if (this.shareData.Info.Value == 1)
                    {
                        ojb.SharePoint = 3;
                    }
                    else if (this.shareData.Info.Value == 0x3e8)
                    {
                        ojb.SharePoint = 4;
                    }
                    break;
            }
            Globals.Instance.CliSession.Send(250, ojb);
        }
	}

	private void OnCloseClick(GameObject go)
	{
		this.SendShareLog(0);
		if (!GameUISharePopUp.isSharing)
		{
			this.OnCloseSharePopUp();
		}
	}

	private void OnCloseSharePopUp()
	{
		GameUISharePopUp.isSharing = false;
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnShareBtnClick(GameObject go)
	{
		if (GameUISharePopUp.isSharing)
		{
			return;
		}
		int num = Convert.ToInt32(go.name);
		this.SendShareLog(num);
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!string.IsNullOrEmpty(this.imagePath) && !File.Exists(this.imagePath))
		{
			return;
		}
		ContentType type = ContentType.Text;
		PlatformType platformType = (PlatformType)num;
		if (platformType != PlatformType.WeChatSession)
		{
			if (platformType != PlatformType.WeChatTimeline)
			{
				if (platformType == PlatformType.SinaWeibo)
				{
					type = (string.IsNullOrEmpty(this.imagePath) ? ContentType.Text : ContentType.Image);
					goto IL_C2;
				}
				if (platformType != PlatformType.YiXinTimeline)
				{
					goto IL_C2;
				}
			}
			type = (string.IsNullOrEmpty(this.imagePath) ? ContentType.News : ContentType.Image);
		}
		else
		{
			type = ContentType.News;
		}
		IL_C2:
		string @string = Singleton<StringManager>.Instance.GetString("shareTxt6", new object[]
		{
			GameSetting.Data.ServerName,
			GameSetting.ServerID
		});
		ShareResultEvent evt = new ShareResultEvent(this.ShareResultHandler);
		ShareSDK.Share((PlatformType)num, @string, this.contentTxt, this.imagePath, type, evt);
		GameUISharePopUp.isSharing = true;
		this.flagShare = true;
		this.flagShareInitTime = Time.time;
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (focusStatus)
		{
			this.focus = true;
		}
	}

	private void LateUpdate()
	{
		if (this.focus)
		{
			this.focus = false;
			this.SendAndroidShareAchievement();
			return;
		}
		if (this.flagShare && (double)(Time.time - this.flagShareInitTime) >= 3.0)
		{
			this.SendAndroidShareAchievement();
		}
	}

	private void SendAndroidShareAchievement()
	{
		if (this.flagShare)
		{
			this.SendShareAchievement();
			this.flagShare = false;
		}
	}

	private void SendShareAchievement()
	{
		if (this.shareData != null && !this.shareData.Data.Shared)
		{
			ShareAchievementData data = this.shareData.Data;
			MC2S_ShareAchievement mC2S_ShareAchievement = new MC2S_ShareAchievement();
			mC2S_ShareAchievement.ID = data.ID;
			Globals.Instance.CliSession.Send(731, mC2S_ShareAchievement);
		}
		GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("shareTxt1"), 0f, 0f);
		this.OnCloseSharePopUp();
	}

	private void ShareResultHandler(ResponseState state, PlatformType type, Hashtable shareInfo, Hashtable error, bool end)
	{
		if (state == ResponseState.Fail)
		{
			int num = 0;
			if (type != PlatformType.WeChatSession && type != PlatformType.WeChatTimeline)
			{
				if (type == PlatformType.YiXinTimeline)
				{
					num = 4;
				}
			}
			else
			{
				num = 3;
			}
			if (num != 0)
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString(string.Format("shareTxt{0}", num)), 0f, 0f);
			}
			this.flagShare = false;
			this.OnCloseSharePopUp();
			return;
		}
		if (state == ResponseState.Success)
		{
			global::Debug.Log(new object[]
			{
				"share result :" + shareInfo
			});
			this.SendAndroidShareAchievement();
			return;
		}
		this.OnCloseSharePopUp();
	}

	public void Refresh(ShareAchievementDataEx data, bool screenshot = false)
	{
		this.shareData = data;
		ShareAchievementInfo info = this.shareData.Info;
		this.imagePath = string.Empty;
		EAchievementConditionType conditionType = (EAchievementConditionType)info.ConditionType;
		List<EAchievementConditionType> list = new List<EAchievementConditionType>
		{
			EAchievementConditionType.EACT_PlayerLevel,
			EAchievementConditionType.EACT_FriendCount,
			EAchievementConditionType.EACT_GainGoldEquip,
			EAchievementConditionType.EACT_GainGoldTrinket,
			EAchievementConditionType.EACT_GuildPvp
		};
		if (!list.Contains(conditionType))
		{
			string text = conditionType.ToString();
			EAchievementConditionType eAchievementConditionType = conditionType;
            switch (eAchievementConditionType)
            {
                case EAchievementConditionType.EACT_SceneChapter:
                default:
                    if (screenshot)
                    {
                        string perstScreenshotImgFilePath = GameUISharePopUp.GetPerstScreenshotImgFilePath(GameUISharePopUp.screenshotName);
                        Texture2D texture2D = this.LoadImageFromBytes(File.ReadAllBytes(perstScreenshotImgFilePath));
                        if (texture2D == null)
                        {
                            global::Debug.LogErrorFormat("Can not load Screenshot Image {0}.", new object[]
					{
						perstScreenshotImgFilePath
					});
                            return;
                        }
                        base.StartCoroutine(this.GenerateShareImage(text, GameUISharePopUp.screenshotName, texture2D));
                    }
                    else
                    {
                        base.StartCoroutine(this.GenerateShareImage(text, text, null));
                    }
                    break;
            }
		}
		this.Refresh(info.Desc, info.ShardMsg, (this.shareData.IsComplete() && this.shareData.Data.Shared) ? 0 : info.RewardDiamond);
	}

	public void Refresh(string desc, string shardMsg, int rewardValue)
	{
		this.contentTxt = shardMsg;
		this.content.text = desc;
		if (rewardValue > 0)
		{
			GameUITools.CreateMinReward(2, rewardValue, 1, this.rewardMoney.transform);
		}
		else
		{
			this.rewardMoney.SetActive(false);
		}
	}

	private static string GetResShareImgFilePath(string fileName)
	{
		return string.Format("Share/{0}", fileName);
	}

	private static string GetPerstShareImgFilePath(string fileName)
	{
		return string.Format("{0}/Share/{1}.jpg", Application.persistentDataPath, fileName);
	}

	private static string GetPerstScreenshotImgFilePath(string fileName)
	{
		return string.Format("{0}/{1}", Application.persistentDataPath, fileName);
	}

	[DebuggerHidden]
	public static IEnumerator GenerateShareScreenshot()
	{
        return null;
        //return new GameUISharePopUp.<GenerateShareScreenshot>c__Iterator90();
	}

	[DebuggerHidden]
	public IEnumerator GenerateShareImage(string typeName, string saveName, Texture2D shareBgImg = null)
	{
        return null;
        //GameUISharePopUp.<GenerateShareImage>c__Iterator91 <GenerateShareImage>c__Iterator = new GameUISharePopUp.<GenerateShareImage>c__Iterator91();
        //<GenerateShareImage>c__Iterator.typeName = typeName;
        //<GenerateShareImage>c__Iterator.shareBgImg = shareBgImg;
        //<GenerateShareImage>c__Iterator.saveName = saveName;
        //<GenerateShareImage>c__Iterator.<$>typeName = typeName;
        //<GenerateShareImage>c__Iterator.<$>shareBgImg = shareBgImg;
        //<GenerateShareImage>c__Iterator.<$>saveName = saveName;
        //<GenerateShareImage>c__Iterator.<>f__this = this;
        //return <GenerateShareImage>c__Iterator;
	}

	public Texture2D LoadImageFromResPath(string fileName)
	{
		TextAsset textAsset = Res.Load<TextAsset>(fileName, false);
		return this.LoadImageFromBytes(textAsset.bytes);
	}

	private Texture2D LoadImageFromBytes(byte[] bytes)
	{
		Texture2D texture2D = new Texture2D(512, 512, TextureFormat.ARGB32, false);
		texture2D.LoadImage(bytes);
		return texture2D;
	}
}
