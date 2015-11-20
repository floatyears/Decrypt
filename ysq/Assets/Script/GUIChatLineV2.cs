using Proto;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GUIChatLineV2 : UICustomGridItem
{
	private const int mBgHeight = 88;

	private const int mLineHeight = 22;

	private const string mSpaceStr = "[00000000]......[-]";

	private GUIChatWindowV2 mBaseScene;

	private GameObject mTargetLineGo;

	private GameObject mTargetVipGo;

	private UISprite mTargetVipNum0;

	private UISprite mTargetVipNum1;

	private UISprite mTargetIcon;

	private UISprite mTargetSystem;

	private UISprite mTargetQuality;

	private UISprite mTargetBg;

	private UILabel mTargetName;

	private UILabel mTargetTime;

	private UILabel mTargetMsg;

	private GameObject mTargetVoiceBg;

	private GameObject mTargetVoicePlayedTag;

	private GameObject mTargetVoiceNoPlayTag;

	private UILabel mTargetVoiceTimeNum;

	private UILabel mTargetVoiceDesc;

	private GameObject mTargetVoiceYinLiang;

	private GUIEmotionSprite mTargetEmotionSprite;

	private GameObject mSelfLineGo;

	private GameObject mSelfVipGo;

	private UISprite mSelfVipNum0;

	private UISprite mSelfVipNum1;

	private UISprite mSelfIcon;

	private UISprite mSelfQuality;

	private UISprite mSelfBg;

	private UILabel mSelfName;

	private UILabel mSelfTime;

	private UILabel mSelfMsg;

	private GameObject mSelfVoiceBg;

	private GameObject mSelfVoicePlayedTag;

	private GameObject mSelfVoiceNoPlayTag;

	private UILabel mSelfVoiceTimeNum;

	private UILabel mSelfVoiceDesc;

	private GameObject mSelfVoiceYinLiang;

	private GUIEmotionSprite mSelfEmotionSprite;

	private GUIChatMessageData mCommonChatMessageData;

	private ChatMessage mMsg;

	private List<string> mLineList = new List<string>();

	private List<Vector3> mEList = new List<Vector3>();

	private StringBuilder mSb = new StringBuilder(42);

	public void InitWithBaseScene(GUIChatWindowV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mTargetLineGo = base.transform.Find("targetLine").gameObject;
		Transform transform = this.mTargetLineGo.transform;
		this.mTargetVipGo = transform.Find("VIP").gameObject;
		Transform transform2 = this.mTargetVipGo.transform;
		this.mTargetVipNum0 = transform2.Find("Tens").GetComponent<UISprite>();
		this.mTargetVipNum1 = transform2.Find("Single").GetComponent<UISprite>();
		this.mTargetIcon = transform.Find("CharIcon").GetComponent<UISprite>();
		UIEventListener expr_9B = UIEventListener.Get(this.mTargetIcon.gameObject);
		expr_9B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_9B.onClick, new UIEventListener.VoidDelegate(this.OnTargetIconClick));
		this.mTargetSystem = transform.Find("System").GetComponent<UISprite>();
		this.mTargetSystem.spriteName = "pet_0019a";
		this.mTargetQuality = transform.Find("Frame").GetComponent<UISprite>();
		this.mTargetBg = transform.Find("bg").GetComponent<UISprite>();
		this.mTargetName = this.mTargetBg.transform.Find("name").GetComponent<UILabel>();
		this.mTargetTime = this.mTargetBg.transform.Find("time").GetComponent<UILabel>();
		this.mTargetMsg = this.mTargetBg.transform.Find("desc").GetComponent<UILabel>();
		this.mTargetMsg.spaceIsNewLine = false;
		UIEventListener expr_18A = UIEventListener.Get(this.mTargetMsg.gameObject);
		expr_18A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_18A.onClick, new UIEventListener.VoidDelegate(this.OnTargetMsgClick));
		this.mTargetEmotionSprite = this.mTargetBg.transform.Find("emotion").gameObject.AddComponent<GUIEmotionSprite>();
		this.mTargetEmotionSprite.InitObjects();
		this.mTargetVoiceBg = this.mTargetBg.transform.Find("voiceBg").gameObject;
		this.mTargetVoiceNoPlayTag = this.mTargetVoiceBg.transform.Find("noMark").gameObject;
		UIEventListener expr_226 = UIEventListener.Get(this.mTargetVoiceNoPlayTag);
		expr_226.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_226.onClick, new UIEventListener.VoidDelegate(this.OnTargetVoiceNoPlayTagClick));
		this.mTargetVoicePlayedTag = this.mTargetVoiceBg.transform.Find("mark").gameObject;
		UIEventListener expr_272 = UIEventListener.Get(this.mTargetVoicePlayedTag);
		expr_272.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_272.onClick, new UIEventListener.VoidDelegate(this.OnTargetVoiceNoPlayTagClick));
		this.mTargetVoiceTimeNum = this.mTargetVoiceBg.transform.Find("txt").GetComponent<UILabel>();
		this.mTargetVoiceDesc = this.mTargetVoiceBg.transform.Find("desc").GetComponent<UILabel>();
		this.mTargetVoiceDesc.spaceIsNewLine = false;
		UIEventListener expr_2EF = UIEventListener.Get(this.mTargetVoiceDesc.gameObject);
		expr_2EF.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2EF.onClick, new UIEventListener.VoidDelegate(this.OnTargetVoiceMsgClick));
		this.mTargetVoiceYinLiang = this.mTargetVoiceBg.transform.Find("yinLiang").gameObject;
		UIEventListener expr_33B = UIEventListener.Get(this.mTargetVoiceYinLiang);
		expr_33B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_33B.onClick, new UIEventListener.VoidDelegate(this.OnTargetVoiceYinLiangClick));
		this.mTargetVoiceBg.SetActive(false);
		this.mSelfLineGo = base.transform.Find("selfLine").gameObject;
		Transform transform3 = this.mSelfLineGo.transform;
		this.mSelfVipGo = transform3.Find("VIP").gameObject;
		Transform transform4 = this.mSelfVipGo.transform;
		this.mSelfVipNum0 = transform4.Find("Tens").GetComponent<UISprite>();
		this.mSelfVipNum1 = transform4.Find("Single").GetComponent<UISprite>();
		this.mSelfIcon = transform3.Find("CharIcon").GetComponent<UISprite>();
		this.mSelfQuality = this.mSelfIcon.transform.Find("Frame").GetComponent<UISprite>();
		this.mSelfBg = transform3.Find("bg").GetComponent<UISprite>();
		this.mSelfName = this.mSelfBg.transform.Find("name").GetComponent<UILabel>();
		this.mSelfTime = this.mSelfBg.transform.Find("time").GetComponent<UILabel>();
		this.mSelfMsg = this.mSelfBg.transform.Find("desc").GetComponent<UILabel>();
		this.mSelfMsg.spaceIsNewLine = false;
		UIEventListener expr_4A5 = UIEventListener.Get(this.mSelfMsg.gameObject);
		expr_4A5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4A5.onClick, new UIEventListener.VoidDelegate(this.OnSelfMsgClick));
		this.mSelfEmotionSprite = this.mSelfBg.transform.Find("emotion").gameObject.AddComponent<GUIEmotionSprite>();
		this.mSelfEmotionSprite.InitObjects();
		this.mSelfVoiceBg = this.mSelfBg.transform.Find("voiceBg").gameObject;
		this.mSelfVoiceNoPlayTag = this.mSelfVoiceBg.transform.Find("noMark").gameObject;
		UIEventListener expr_541 = UIEventListener.Get(this.mSelfVoiceNoPlayTag);
		expr_541.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_541.onClick, new UIEventListener.VoidDelegate(this.OnSelfVoiceNoPlayTagClick));
		this.mSelfVoicePlayedTag = this.mSelfVoiceBg.transform.Find("mark").gameObject;
		UIEventListener expr_58D = UIEventListener.Get(this.mSelfVoicePlayedTag);
		expr_58D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_58D.onClick, new UIEventListener.VoidDelegate(this.OnSelfVoiceNoPlayTagClick));
		this.mSelfVoiceTimeNum = this.mSelfVoiceBg.transform.Find("txt").GetComponent<UILabel>();
		this.mSelfVoiceDesc = this.mSelfVoiceBg.transform.Find("desc").GetComponent<UILabel>();
		this.mSelfVoiceDesc.spaceIsNewLine = false;
		UIEventListener expr_60A = UIEventListener.Get(this.mSelfVoiceDesc.gameObject);
		expr_60A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_60A.onClick, new UIEventListener.VoidDelegate(this.OnSelfVoiceMsgClick));
		this.mSelfVoiceYinLiang = this.mSelfVoiceBg.transform.Find("yinLiang").gameObject;
		UIEventListener expr_656 = UIEventListener.Get(this.mSelfVoiceYinLiang);
		expr_656.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_656.onClick, new UIEventListener.VoidDelegate(this.OnSelfVoiceYinLiangClick));
		this.mSelfVoiceBg.SetActive(false);
	}

	public override void Refresh(object data)
	{
		if (this.mCommonChatMessageData == data)
		{
			return;
		}
		this.mCommonChatMessageData = (GUIChatMessageData)data;
		this.mMsg = null;
		if (this.mCommonChatMessageData.mWorldMessage != null)
		{
			if (this.mCommonChatMessageData.mWorldMessage.mWM.Msg != null)
			{
				this.mMsg = this.mCommonChatMessageData.mWorldMessage.mWM.Msg;
			}
		}
		else if (this.mCommonChatMessageData.mChatMessage != null)
		{
			this.mMsg = this.mCommonChatMessageData.mChatMessage;
		}
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.mCommonChatMessageData != null)
		{
			if (this.mMsg != null && this.mMsg.Type == 2u && this.mMsg.Channel == 3)
			{
				this.mSelfLineGo.SetActive(false);
				this.mTargetLineGo.SetActive(true);
				for (int i = this.mTargetBg.transform.childCount - 1; i >= 0; i--)
				{
					Transform child = this.mTargetBg.transform.GetChild(i);
					if (child.name != "line" && child.name != "desc" && child.name != "name" && child.name != "time" && child.name != "voiceBg" && child.gameObject != this.mTargetEmotionSprite.gameObject)
					{
						UnityEngine.Object.Destroy(child.gameObject);
					}
				}
				this.mTargetIcon.gameObject.SetActive(false);
				this.mTargetSystem.gameObject.SetActive(true);
				this.mTargetQuality.spriteName = Tools.GetItemQualityIcon(3);
				this.mTargetVipGo.SetActive(false);
				this.mTargetName.text = Singleton<StringManager>.Instance.GetString("chatSystem");
				this.mTargetName.color = Color.yellow;
				this.mTargetTime.text = Tools.ServerDateTimeFormat1(this.mMsg.TimeStamp);
				this.ParseChatMessage(this.mMsg, false);
			}
			else if (this.mCommonChatMessageData.mIsSelfChat)
			{
				this.mSelfLineGo.SetActive(true);
				this.mTargetLineGo.SetActive(false);
				this.mSelfEmotionSprite.gameObject.SetActive(false);
				for (int j = this.mSelfBg.transform.childCount - 1; j >= 0; j--)
				{
					Transform child2 = this.mSelfBg.transform.GetChild(j);
					if (child2.name != "line" && child2.name != "desc" && child2.name != "name" && child2.name != "time" && child2.name != "voiceBg" && child2.gameObject != this.mSelfEmotionSprite.gameObject)
					{
						UnityEngine.Object.Destroy(child2.gameObject);
					}
				}
				if (this.mMsg != null)
				{
					this.mSelfIcon.spriteName = Tools.GetPlayerIcon(this.mMsg.FashionID);
					this.mSelfQuality.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(this.mMsg.ConstellationLevel));
					if (this.mMsg.VipLevel > 0u)
					{
						this.mSelfVipGo.SetActive(true);
						if (this.mMsg.VipLevel >= 10u)
						{
							this.mSelfVipNum1.gameObject.SetActive(true);
							this.mSelfVipNum1.spriteName = (this.mMsg.VipLevel % 10u).ToString();
							this.mSelfVipNum0.spriteName = (this.mMsg.VipLevel / 10u).ToString();
						}
						else
						{
							this.mSelfVipNum1.gameObject.SetActive(false);
							this.mSelfVipNum0.spriteName = this.mMsg.VipLevel.ToString();
						}
					}
					else
					{
						this.mSelfVipGo.SetActive(false);
					}
					if (this.mBaseScene.GetCurChanel() != 2)
					{
						this.mSelfName.text = this.mMsg.Name;
					}
					else
					{
						this.mSelfName.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("chatTxt7")).Append(this.mMsg.Name).ToString();
					}
					this.mSelfName.color = Tools.GetItemQualityColor(LocalPlayer.GetQuality(this.mMsg.ConstellationLevel));
					this.mSelfTime.text = Tools.ServerDateTimeFormat1(this.mMsg.TimeStamp);
				}
				if (this.mCommonChatMessageData.mWorldMessage != null)
				{
					if (!this.mCommonChatMessageData.mIsVoice)
					{
						this.ParseWorldMessage(this.mCommonChatMessageData.mWorldMessage.mWM, true);
					}
					else
					{
						this.ParseVoiceMessage(this.mCommonChatMessageData.mGUIVoiceChatData, true, this.mCommonChatMessageData.IsVoicePlaying(), this.mCommonChatMessageData.mIsAlreadyPlayed);
					}
				}
				else if (this.mCommonChatMessageData.mChatMessage != null)
				{
					if (!this.mCommonChatMessageData.mIsVoice)
					{
						this.ParseChatMessage(this.mMsg, true);
					}
					else
					{
						this.ParseVoiceMessage(this.mCommonChatMessageData.mGUIVoiceChatData, true, this.mCommonChatMessageData.IsVoicePlaying(), this.mCommonChatMessageData.mIsAlreadyPlayed);
					}
				}
			}
			else
			{
				this.mSelfLineGo.SetActive(false);
				this.mTargetLineGo.SetActive(true);
				this.mTargetEmotionSprite.gameObject.SetActive(false);
				for (int k = this.mTargetBg.transform.childCount - 1; k >= 0; k--)
				{
					Transform child3 = this.mTargetBg.transform.GetChild(k);
					if (child3.name != "line" && child3.name != "desc" && child3.name != "name" && child3.name != "time" && child3.name != "voiceBg" && child3.gameObject != this.mTargetEmotionSprite.gameObject)
					{
						UnityEngine.Object.Destroy(child3.gameObject);
					}
				}
				if (this.mMsg != null)
				{
					if (this.mCommonChatMessageData.mWorldMessage != null)
					{
						if (this.mCommonChatMessageData.mWorldMessage.mIsSystem)
						{
							this.mTargetIcon.gameObject.SetActive(false);
							this.mTargetSystem.gameObject.SetActive(true);
							this.mTargetSystem.spriteName = "pet_0019a";
							this.mTargetVipGo.SetActive(false);
							this.mTargetQuality.spriteName = Tools.GetItemQualityIcon(3).ToString();
							this.mTargetName.text = this.mMsg.Name;
							this.mTargetName.color = Color.yellow;
							this.mTargetTime.text = Tools.ServerDateTimeFormat1(this.mMsg.TimeStamp);
						}
						else
						{
							this.mTargetIcon.gameObject.SetActive(true);
							this.mTargetSystem.gameObject.SetActive(false);
							this.mTargetIcon.spriteName = Tools.GetPlayerIcon(this.mMsg.FashionID);
							this.mTargetQuality.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(this.mMsg.ConstellationLevel));
							if (this.mMsg.VipLevel > 0u)
							{
								this.mTargetVipGo.SetActive(true);
								if (this.mMsg.VipLevel >= 10u)
								{
									this.mTargetVipNum1.gameObject.SetActive(true);
									this.mTargetVipNum1.spriteName = (this.mMsg.VipLevel % 10u).ToString();
									this.mTargetVipNum0.spriteName = (this.mMsg.VipLevel / 10u).ToString();
								}
								else
								{
									this.mTargetVipNum1.gameObject.SetActive(false);
									this.mTargetVipNum0.spriteName = this.mMsg.VipLevel.ToString();
								}
							}
							else
							{
								this.mTargetVipGo.SetActive(false);
							}
							this.mTargetName.text = this.mMsg.Name;
							this.mTargetName.color = Tools.GetItemQualityColor(LocalPlayer.GetQuality(this.mMsg.ConstellationLevel));
							this.mTargetTime.text = Tools.ServerDateTimeFormat1(this.mMsg.TimeStamp);
						}
					}
					else
					{
						this.mTargetIcon.gameObject.SetActive(true);
						this.mTargetSystem.gameObject.SetActive(false);
						this.mTargetIcon.spriteName = Tools.GetPlayerIcon(this.mMsg.FashionID);
						this.mTargetQuality.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(this.mMsg.ConstellationLevel));
						if (this.mMsg.VipLevel > 0u)
						{
							this.mTargetVipGo.SetActive(true);
							if (this.mMsg.VipLevel >= 10u)
							{
								this.mTargetVipNum1.gameObject.SetActive(true);
								this.mTargetVipNum1.spriteName = (this.mMsg.VipLevel % 10u).ToString();
								this.mTargetVipNum0.spriteName = (this.mMsg.VipLevel / 10u).ToString();
							}
							else
							{
								this.mTargetVipNum1.gameObject.SetActive(false);
								this.mTargetVipNum0.spriteName = this.mMsg.VipLevel.ToString();
							}
						}
						else
						{
							this.mTargetVipGo.SetActive(false);
						}
						this.mTargetName.text = this.mMsg.Name;
						this.mTargetName.color = Tools.GetItemQualityColor(LocalPlayer.GetQuality(this.mMsg.ConstellationLevel));
						this.mTargetTime.text = Tools.ServerDateTimeFormat1(this.mMsg.TimeStamp);
					}
				}
				if (this.mCommonChatMessageData.mWorldMessage != null)
				{
					if (!this.mCommonChatMessageData.mIsVoice)
					{
						this.ParseWorldMessage(this.mCommonChatMessageData.mWorldMessage.mWM, false);
					}
					else
					{
						this.ParseVoiceMessage(this.mCommonChatMessageData.mGUIVoiceChatData, false, this.mCommonChatMessageData.IsVoicePlaying(), this.mCommonChatMessageData.mIsAlreadyPlayed);
					}
				}
				else if (this.mCommonChatMessageData.mChatMessage != null)
				{
					if (!this.mCommonChatMessageData.mIsVoice)
					{
						this.ParseChatMessage(this.mCommonChatMessageData.mChatMessage, false);
					}
					else
					{
						this.ParseVoiceMessage(this.mCommonChatMessageData.mGUIVoiceChatData, false, this.mCommonChatMessageData.IsVoicePlaying(), this.mCommonChatMessageData.mIsAlreadyPlayed);
					}
				}
			}
		}
	}

	private void CalVoiceLines(ref string text, bool isSelf)
	{
		NGUIText.finalSize = this.mTargetVoiceDesc.defaultFontSize;
		int num = 450;
		this.mLineList.Clear();
		text = this.mSb.Remove(0, this.mSb.Length).Append("[00000000]......[-]").Append(text).ToString();
		int num2 = 0;
		string item = string.Empty;
		int length = text.Length;
		this.mTargetVoiceDesc.UpdateNGUIText();
		for (int i = 0; i < length; i++)
		{
			if (i - num2 >= 0)
			{
				float num3 = (float)Mathf.RoundToInt(NGUIText.CalculatePrintedSize(text.Substring(num2, i - num2 + 1), num + 30).x);
				if (num3 > (float)num)
				{
					item = text.Substring(num2, i - num2 + 1);
					this.mLineList.Add(item);
					num2 = i + 1;
				}
				if (i == length - 1)
				{
					if (i - num2 >= 0)
					{
						item = text.Substring(num2, i - num2 + 1);
						this.mLineList.Add(item);
					}
				}
			}
		}
	}

	private void ParseVoiceMessage(GUIVoiceChatData voiceChatData, bool isSelf, bool isPlaying, bool isAlreadyPlayed)
	{
		this.mSelfVoiceDesc.text = string.Empty;
		this.mSelfVoiceTimeNum.text = string.Empty;
		this.mTargetVoiceDesc.text = string.Empty;
		this.mTargetVoiceTimeNum.text = string.Empty;
		if (voiceChatData == null)
		{
			return;
		}
		string voiceMsg = voiceChatData.VoiceMsg;
		this.CalVoiceLines(ref voiceMsg, isSelf);
		this.mSb.Remove(0, this.mSb.Length);
		for (int i = 0; i < this.mLineList.Count; i++)
		{
			this.mSb.Append(this.mLineList[i]);
		}
		if (isSelf)
		{
			this.mSelfVoiceBg.SetActive(true);
			this.mSelfMsg.gameObject.SetActive(false);
			this.mSelfVoiceDesc.text = this.mSb.ToString();
			this.mSelfVoiceTimeNum.text = string.Format("{0}\"", voiceChatData.VoiceTime);
			this.mSelfBg.height = ((this.mLineList.Count <= 1 && this.mSelfVoiceDesc.printedSize.y <= 30f) ? 88 : 110);
		}
		else
		{
			this.mTargetVoiceBg.SetActive(true);
			this.mTargetMsg.gameObject.SetActive(false);
			this.mTargetVoiceDesc.text = this.mSb.ToString();
			this.mTargetVoiceTimeNum.text = string.Format("{0}\"", voiceChatData.VoiceTime);
			this.mTargetBg.height = ((this.mLineList.Count <= 1 && this.mTargetVoiceDesc.printedSize.y <= 30f) ? 88 : 110);
		}
		if (isPlaying)
		{
			if (isSelf)
			{
				this.mSelfVoiceNoPlayTag.SetActive(false);
				this.mSelfVoicePlayedTag.SetActive(false);
				this.mSelfVoiceYinLiang.SetActive(true);
			}
			else
			{
				this.mTargetVoiceNoPlayTag.SetActive(false);
				this.mTargetVoicePlayedTag.SetActive(false);
				this.mTargetVoiceYinLiang.SetActive(true);
			}
		}
		else if (isSelf)
		{
			this.mSelfVoiceNoPlayTag.SetActive(!isAlreadyPlayed);
			this.mSelfVoicePlayedTag.SetActive(isAlreadyPlayed);
			this.mSelfVoiceYinLiang.SetActive(false);
		}
		else
		{
			this.mTargetVoiceNoPlayTag.SetActive(!isAlreadyPlayed);
			this.mTargetVoicePlayedTag.SetActive(isAlreadyPlayed);
			this.mTargetVoiceYinLiang.SetActive(false);
		}
	}

	private void ParseWorldMessage(WorldMessage worldMsg, bool isSelf)
	{
		if (worldMsg.Msg != null)
		{
			this.ParseChatMessage(worldMsg.Msg, isSelf);
		}
	}

	public void ParseChatMessage(ChatMessage chatMsg, bool isSelf)
	{
		if (isSelf)
		{
			this.mSelfVoiceBg.SetActive(false);
			this.mSelfMsg.gameObject.SetActive(true);
		}
		else
		{
			this.mTargetVoiceBg.SetActive(false);
			this.mTargetMsg.gameObject.SetActive(true);
		}
		if (chatMsg.Type == 0u)
		{
			string message = chatMsg.Message;
			this.CalExpressionPos(ref message, isSelf);
			this.mSb.Remove(0, this.mSb.Length);
			for (int i = 0; i < this.mLineList.Count; i++)
			{
				this.mSb.Append(this.mLineList[i]);
			}
			for (int j = 0; j < this.mEList.Count; j++)
			{
				this.CreateEmotion(this.mEList[j].x, this.mEList[j].y, string.Format("{0:D2}", (int)this.mEList[j].z), isSelf);
			}
			if (isSelf)
			{
				this.mSelfMsg.text = this.mSb.ToString();
				this.mSelfBg.height = ((this.mLineList.Count <= 1 && this.mSelfMsg.printedSize.y <= 30f) ? 88 : 110);
			}
			else
			{
				this.mTargetMsg.text = this.mSb.ToString();
				this.mTargetBg.height = ((this.mLineList.Count <= 1 && this.mTargetMsg.printedSize.y <= 30f) ? 88 : 110);
			}
		}
		else if (chatMsg.Type == 1u)
		{
			this.mSb.Remove(0, this.mSb.Length);
			string text = chatMsg.Message;
			text = this.mSb.Append(Singleton<StringManager>.Instance.GetString("ChatInviteMsgCostumeParty0")).Append("[u]").Append(Tools.GetColorHex(Color.yellow, Tools.GetUrl(chatMsg.Type.ToString(), Singleton<StringManager>.Instance.GetString("ChatInviteMsgCostumeParty", new object[]
			{
				chatMsg.Name
			})))).Append("[/u]").ToString() + chatMsg.Message;
			this.CalExpressionPos(ref text, isSelf);
			this.mSb.Remove(0, this.mSb.Length);
			for (int k = 0; k < this.mLineList.Count; k++)
			{
				this.mSb.Append(this.mLineList[k]);
			}
			if (isSelf)
			{
				this.mSelfMsg.text = this.mSb.ToString();
				this.mSelfBg.height = ((this.mLineList.Count <= 1 && this.mSelfMsg.printedSize.y <= 30f) ? 88 : 110);
			}
			else
			{
				this.mTargetMsg.text = this.mSb.ToString();
				this.mTargetBg.height = ((this.mLineList.Count <= 1 && this.mTargetMsg.printedSize.y <= 30f) ? 88 : 110);
			}
		}
		else if (chatMsg.Type == 2u && chatMsg.Channel == 3)
		{
			MS2C_InteractionMessage reply;
			bool firstNameIsSelf = GUICostumePartyScene.ConvertChatMsg2InteractionMsg(chatMsg, out reply);
			List<string> interactionStrs = GUICostumePartyScene.GetInteractionStrs(reply, firstNameIsSelf);
			this.mSb.Remove(0, this.mSb.Length);
			this.mSb.Append(interactionStrs[0]);
			this.mSb.Append(Tools.GetColorHex(Color.green, Tools.GetUrl(chatMsg.Type.ToString(), interactionStrs[1])));
			this.mSb.Append(interactionStrs[2]);
			this.mSb.Append(interactionStrs[3]);
			this.mSb.Append(Tools.GetColorHex(Color.green, interactionStrs[4]));
			string text2 = this.mSb.ToString();
			this.CalExpressionPos(ref text2, isSelf);
			if (isSelf)
			{
				this.mSelfMsg.text = text2;
				this.mSelfBg.height = ((this.mLineList.Count <= 1 && this.mSelfMsg.printedSize.y <= 30f) ? 88 : 110);
			}
			else
			{
				this.mTargetMsg.text = text2;
				this.mTargetBg.height = ((this.mLineList.Count <= 1 && this.mTargetMsg.printedSize.y <= 30f) ? 88 : 110);
			}
		}
	}

	private void CreateEmotion(float xPos, float yPos, string emotionStr, bool isSelf)
	{
		if (isSelf)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.mSelfEmotionSprite.gameObject);
			gameObject.name = this.mSelfEmotionSprite.gameObject.name;
			gameObject.transform.parent = this.mSelfEmotionSprite.transform.parent;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = new Vector3(xPos, -48f - yPos * 30f, 0f);
			gameObject.SetActive(true);
			GUIEmotionSprite component = gameObject.GetComponent<GUIEmotionSprite>();
			if (component != null)
			{
				component.InitObjects();
				component.InitEmotion(emotionStr);
			}
		}
		else
		{
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(this.mTargetEmotionSprite.gameObject);
			gameObject2.name = this.mTargetEmotionSprite.gameObject.name;
			gameObject2.transform.parent = this.mTargetEmotionSprite.transform.parent;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.transform.localPosition = new Vector3(xPos, -48f - yPos * 30f, 0f);
			gameObject2.SetActive(true);
			GUIEmotionSprite component2 = gameObject2.GetComponent<GUIEmotionSprite>();
			if (component2 != null)
			{
				component2.InitObjects();
				component2.InitEmotion(emotionStr);
			}
		}
	}

	private void CalExpressionPos(ref string text, bool isSelf)
	{
		if (this.mCommonChatMessageData != null)
		{
			NGUIText.finalSize = this.mTargetMsg.defaultFontSize;
			int num = 500;
			int num2 = (!isSelf) ? 32 : -540;
			this.mLineList.Clear();
			this.mEList.Clear();
			int num3 = 0;
			int num4 = 0;
			string item = string.Empty;
			int length = text.Length;
			this.mTargetMsg.UpdateNGUIText();
			for (int i = 0; i < length; i++)
			{
				if (text[i] == '<' && i + 3 < length && text[i + 3] == '>')
				{
					string s = text.Substring(i + 1, 2);
					int num5 = 0;
					Vector3 zero = Vector3.zero;
					if (int.TryParse(s, out num5) && num5 < 49 && num5 > 0)
					{
						text = text.Remove(i, 4);
						text = text.Insert(i, "[00000000]......[-]");
						length = text.Length;
						int num6 = Mathf.RoundToInt(NGUIText.CalculatePrintedSize(text.Substring(num4, i - num4), num + 30).x);
						if (num6 > num - 30)
						{
							item = text.Substring(num4, i - num4 + 1);
							this.mLineList.Add(item);
							if (num6 <= num - 15 || num6 >= num)
							{
								float num7 = 0f;
								num3++;
								num4 = i;
								zero.x = num7 + (float)num2;
								zero.y = (float)num3;
								zero.z = (float)num5;
							}
							else
							{
								float num7 = (float)num6;
								num4 = i + "[00000000]......[-]".Length;
								zero.x = num7 + (float)num2;
								zero.y = (float)num3;
								zero.z = (float)num5;
								num3++;
							}
						}
						else
						{
							float num7 = (float)num6;
							zero.x = num7 + (float)num2;
							zero.y = (float)num3;
							zero.z = (float)num5;
						}
					}
					if (num5 != 0)
					{
						this.mEList.Add(zero);
					}
				}
				else if (i - num4 >= 0)
				{
					float num8 = (float)Mathf.RoundToInt(NGUIText.CalculatePrintedSize(text.Substring(num4, i - num4 + 1), num + 30).x);
					if (num8 > (float)num)
					{
						item = text.Substring(num4, i - num4 + 1);
						this.mLineList.Add(item);
						num4 = i + 1;
						num3++;
					}
					if (i == length - 1)
					{
						if (i - num4 >= 0)
						{
							item = text.Substring(num4, i - num4 + 1);
							this.mLineList.Add(item);
						}
					}
				}
			}
		}
	}

	private void OnTargetIconClick(GameObject go)
	{
		if (this.mCommonChatMessageData != null && !this.mCommonChatMessageData.mIsSelfChat)
		{
			GUIChatWindowV2 session = GameUIManager.mInstance.GetSession<GUIChatWindowV2>();
			if (session != null)
			{
				session.SetPersonal(this.mMsg);
			}
		}
	}

	private void OnSelfMsgClick(GameObject go)
	{
		string urlAtPosition = this.mSelfMsg.GetUrlAtPosition(UICamera.lastHit.point);
		if (string.IsNullOrEmpty(urlAtPosition))
		{
			return;
		}
		this.ReadUrl(urlAtPosition);
	}

	private void OnTargetMsgClick(GameObject go)
	{
		string urlAtPosition = this.mTargetMsg.GetUrlAtPosition(UICamera.lastHit.point);
		if (string.IsNullOrEmpty(urlAtPosition))
		{
			return;
		}
		this.ReadUrl(urlAtPosition);
	}

	private void ReadUrl(string url)
	{
		EInviteType eInviteType;
		try
		{
			eInviteType = (EInviteType)((int)Enum.Parse(typeof(EInviteType), url));
		}
		catch
		{
			global::Debug.LogErrorFormat("System.Enum.Parse EInviteType error , string : {0}", new object[]
			{
				url
			});
			return;
		}
		EInviteType eInviteType2 = eInviteType;
		if (eInviteType2 != EInviteType.EInviteType_CostumeParty)
		{
			if (eInviteType2 == EInviteType.EInviteType_CostumePartyWhisper)
			{
				GUIChatWindowV2 session = GameUIManager.mInstance.GetSession<GUIChatWindowV2>();
				if (session != null)
				{
					session.SetPersonal(this.mMsg);
				}
			}
		}
		else if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(10)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("costumePartyLevel", new object[]
			{
				GameConst.GetInt32(10)
			}), 0f, 0f);
		}
		else if (Globals.Instance.SenceMgr.sceneInfo != null)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("ChatInviteMsgCostumeParty4", 0f, 0f);
		}
		else if (Globals.Instance.Player.CostumePartySystem.IsInParty())
		{
			if (this.mMsg.Value1 == Globals.Instance.Player.CostumePartySystem.PartyID)
			{
				GUIChatWindowV2 session2 = GameUIManager.mInstance.GetSession<GUIChatWindowV2>();
				if (session2 != null)
				{
					session2.Close();
				}
				GUICostumePartyScene.TryOpen();
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("ChatInviteMsgCostumeParty1", 0f, 0f);
			}
		}
		else
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("ChatInviteMsgCostumeParty5"), MessageBox.Type.OKCancel, null);
			GameMessageBox expr_17D = gameMessageBox;
			expr_17D.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_17D.OkClick, new MessageBox.MessageDelegate(this.OnOKClick));
			GameMessageBox expr_19F = gameMessageBox;
			expr_19F.CancelClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_19F.CancelClick, new MessageBox.MessageDelegate(this.OnCancelClick));
		}
	}

	private void OnOKClick(object obj)
	{
		MC2S_JoinCostumeParty mC2S_JoinCostumeParty = new MC2S_JoinCostumeParty();
		mC2S_JoinCostumeParty.RoomID = this.mMsg.Value1;
		Globals.Instance.CliSession.Send(266, mC2S_JoinCostumeParty);
	}

	private void OnCancelClick(object obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
	}

	private void DoPlayVoice()
	{
		if (this.mCommonChatMessageData != null && this.mCommonChatMessageData.mGUIVoiceChatData != null && this.mCommonChatMessageData.mIsVoice)
		{
			Globals.Instance.VoiceMgr.StartPlay(this.mCommonChatMessageData.mGUIVoiceChatData.VoiceTranslateParam);
			this.Refresh();
		}
	}

	private void DoStopVoice()
	{
		if (this.mCommonChatMessageData != null && this.mCommonChatMessageData.mIsVoice && Globals.Instance.VoiceMgr.IsPlaying())
		{
			Globals.Instance.VoiceMgr.StopPlay();
			this.Refresh();
		}
	}

	private void OnTargetVoiceNoPlayTagClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.DoPlayVoice();
	}

	private void OnTargetVoiceMsgClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mCommonChatMessageData == null)
		{
			return;
		}
		if (this.mCommonChatMessageData.IsVoicePlaying())
		{
			this.DoStopVoice();
		}
		else
		{
			this.DoPlayVoice();
		}
	}

	private void OnSelfVoiceMsgClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mCommonChatMessageData == null)
		{
			return;
		}
		if (this.mCommonChatMessageData.IsVoicePlaying())
		{
			this.DoStopVoice();
		}
		else
		{
			this.DoPlayVoice();
		}
	}

	private void OnTargetVoiceYinLiangClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.DoStopVoice();
	}

	private void OnSelfVoiceNoPlayTagClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.DoPlayVoice();
	}

	private void OnSelfVoiceYinLiangClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.DoStopVoice();
	}
}
