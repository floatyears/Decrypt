using LitJson;
using NtUniSdk.Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class GameUIFairyTalePopUp : GameUIBasePopup
{
	private class ElfHotData
	{
		public bool success;

		public List<string> data;
	}

	private const int CHAT_LINE_LIMIT_NUM = 30;

	public AnimationCurve DirectionBtnCurve;

	[NonSerialized]
	public float DirectionBtnDuration = 0.35f;

	[NonSerialized]
	public FairyTaleTabItem current;

	private static List<object> cacheLineData = new List<object>();

	private static Dictionary<string, string> codeDict = new Dictionary<string, string>();

	private int cacheSortNo;

	private string lastQueryQuest;

	private Transform winBG;

	private UIToggle tabQuery;

	private UIInput inputQuery;

	private UIScrollView chatScrollView;

	private UITable chatTable;

	private UIScrollBar chatScrollBar;

	private GameObject mTabItemOriginal;

	private GameObject mElfQuestLinePrefab;

	private GameObject mElfAnswerLinePrefab;

	private Dictionary<string, string> replaceStrMap = new Dictionary<string, string>
	{
		{
			"#Y",
			"[ffff00]"
		},
		{
			"#B",
			"[0000ff]"
		},
		{
			"#K",
			"[000000]"
		},
		{
			"#W",
			"[ffffff]"
		},
		{
			"#R",
			"[ff0000]"
		},
		{
			"#G",
			"[00ff00]"
		},
		{
			"#r",
			"\n"
		},
		{
			"#n",
			"[feeebd]"
		},
		{
			"<b>",
			"[b]"
		},
		{
			"</b>",
			"[/b]"
		}
	};

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.winBG = base.transform.Find("winBG");
		GameObject gameObject = this.winBG.Find("closeBtn").gameObject;
		UIEventListener expr_32 = UIEventListener.Get(gameObject);
		expr_32.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_32.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		this.tabQuery = this.winBG.Find("tabQuery").GetComponent<UIToggle>();
		Transform transform = this.winBG.Find("tabQueryPanel");
		this.inputQuery = transform.Find("chatInputArea/chatInput").GetComponent<UIInput>();
		this.inputQuery.defaultText = Singleton<StringManager>.Instance.GetString("FairyTxt_2");
		this.inputQuery.characterLimit = 32;
		GameUITools.RegisterClickEvent("chatInputArea/submitBtn", new UIEventListener.VoidDelegate(this.OnSubmitBtnClick), transform.gameObject);
		this.chatScrollView = transform.Find("chatPanel").GetComponent<UIScrollView>();
		this.chatTable = transform.Find("chatPanel/chatContents").GetComponent<UITable>();
		this.chatScrollBar = transform.Find("chatScrollBar").GetComponent<UIScrollBar>();
		Globals.Instance.CliSession.Register(1510, new ClientSession.MsgHandler(this.OnMsgElfQuery));
		Globals.Instance.CliSession.Register(1509, new ClientSession.MsgHandler(this.OnMsgElfContent));
		Globals.Instance.CliSession.Register(1511, new ClientSession.MsgHandler(this.OnMsgElfComment));
		Globals.Instance.CliSession.Register(1512, new ClientSession.MsgHandler(this.OnMsgElfHot));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		GameUIFairyTalePopUp.codeDict.Clear();
		GameUIFairyTalePopUp.cacheLineData.Clear();
		Globals.Instance.CliSession.Unregister(1510, new ClientSession.MsgHandler(this.OnMsgElfQuery));
		Globals.Instance.CliSession.Unregister(1509, new ClientSession.MsgHandler(this.OnMsgElfContent));
		Globals.Instance.CliSession.Unregister(1511, new ClientSession.MsgHandler(this.OnMsgElfComment));
		Globals.Instance.CliSession.Unregister(1512, new ClientSession.MsgHandler(this.OnMsgElfHot));
	}

	public void InitFairyTale(ElfBtnItem[] initData)
	{
		TextAsset textAsset = StringManager.LoadTextRes("FairyTaleCodeTable");
		StringManager.ParseString(ref GameUIFairyTalePopUp.codeDict, textAsset.text);
		int num = (initData.Length > 4) ? 4 : initData.Length;
		for (int i = num - 1; i >= 0; i--)
		{
			FairyTaleTabItem fairyTaleTabItem = this.AddFairyTaleTabItem(initData[i]);
			fairyTaleTabItem.btnTab.transform.localPosition = new Vector3((float)(-194 + i * 162), 230f, 0f);
		}
		this.AddElfAnswerLine(new ElfAnswerItem
		{
			strAnswer = this.ParseStrData(Singleton<StringManager>.Instance.GetString("FairyTxt_9")).Insert(0, "[feeebd]")
		});
		for (int j = 0; j < GameUIFairyTalePopUp.cacheLineData.Count; j++)
		{
			object obj = GameUIFairyTalePopUp.cacheLineData[j];
			if (obj is ElfQuestItem)
			{
				ElfQuestItem elfQuestItem = (ElfQuestItem)obj;
				this.lastQueryQuest = elfQuestItem.strQuest;
				this.AddElfQuestLine(elfQuestItem);
			}
			else
			{
				this.AddElfAnswerLine((ElfAnswerItem)obj);
			}
		}
		this.RefreshChatPanel();
	}

	private FairyTaleTabItem AddFairyTaleTabItem(ElfBtnItem initData)
	{
		if (this.mTabItemOriginal == null)
		{
			this.mTabItemOriginal = Res.LoadGUI("GUI/FairyTaleTabItem");
		}
		if (this.mTabItemOriginal == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/FairyTaleTabItem error"
			});
			return null;
		}
		GameObject gameObject = Tools.AddChild(this.winBG.gameObject, this.mTabItemOriginal);
		FairyTaleTabItem fairyTaleTabItem = gameObject.AddComponent<FairyTaleTabItem>();
		fairyTaleTabItem.InitWithBaseScene(this, initData);
		return fairyTaleTabItem;
	}

	private GUIElfQuestLine AddElfQuestLine(ElfQuestItem data)
	{
		if (this.mElfQuestLinePrefab == null)
		{
			this.mElfQuestLinePrefab = Res.LoadGUI("GUI/GUIElfQuestLine");
		}
		if (this.mElfQuestLinePrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIElfQuestLine error"
			});
			return null;
		}
		GameObject gameObject = Tools.AddChild(this.chatTable.gameObject, this.mElfQuestLinePrefab);
		GUIElfQuestLine gUIElfQuestLine = gameObject.AddComponent<GUIElfQuestLine>();
		gUIElfQuestLine.InitWithBaseScene(this, data);
		gUIElfQuestLine.name = string.Format("{0:D3}", ++this.cacheSortNo);
		return gUIElfQuestLine;
	}

	private GUIElfAnswerLine AddElfAnswerLine(ElfAnswerItem data)
	{
		if (this.mElfAnswerLinePrefab == null)
		{
			this.mElfAnswerLinePrefab = Res.LoadGUI("GUI/GUIElfAnswerLine");
		}
		if (this.mElfAnswerLinePrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIElfAnswerLine error"
			});
			return null;
		}
		GameObject gameObject = Tools.AddChild(this.chatTable.gameObject, this.mElfAnswerLinePrefab);
		GUIElfAnswerLine gUIElfAnswerLine = gameObject.AddComponent<GUIElfAnswerLine>();
		gUIElfAnswerLine.InitWithBaseScene(this, data);
		gUIElfAnswerLine.name = string.Format("{0:D3}", ++this.cacheSortNo);
		return gUIElfAnswerLine;
	}

	private void OnCloseClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnSubmitBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		string value = this.inputQuery.value;
		if (string.IsNullOrEmpty(value))
		{
			return;
		}
		this.AddElfQuestLine(new ElfQuestItem
		{
			strQuest = value,
			strShow = value
		});
		this.RefreshChatPanel();
		this.lastQueryQuest = value;
		if (value.Length > 7)
		{
			GameUIFairyTalePopUp.HttpGetElfQueryUrl(1510, value);
		}
		else
		{
			GameUIFairyTalePopUp.HttpGetElfHotUrl(value);
		}
		this.inputQuery.value = string.Empty;
	}

	private void RefreshChatPanel()
	{
		int num = this.chatTable.transform.childCount;
		if (num > 30)
		{
			num -= 30;
			for (int i = 1; i <= num; i++)
			{
				UnityEngine.Object.Destroy(this.chatTable.transform.GetChild(i).gameObject);
			}
		}
		this.chatTable.repositionNow = true;
		base.StartCoroutine(this.RefreshChatScrollBar());
	}

	[DebuggerHidden]
	private IEnumerator RefreshChatScrollBar()
	{
        return null;
        //GameUIFairyTalePopUp.<RefreshChatScrollBar>c__Iterator8E <RefreshChatScrollBar>c__Iterator8E = new GameUIFairyTalePopUp.<RefreshChatScrollBar>c__Iterator8E();
        //<RefreshChatScrollBar>c__Iterator8E.<>f__this = this;
        //return <RefreshChatScrollBar>c__Iterator8E;
	}

	public void ProcessUrlClick(UILabel label)
	{
		string urlAtPosition = label.GetUrlAtPosition(UICamera.lastHit.point);
		if (string.IsNullOrEmpty(urlAtPosition))
		{
			return;
		}
		try
		{
			Uri uri = new Uri(urlAtPosition);
			GameUIManager.mInstance.ShowGUIWebViewPopUp(uri.AbsoluteUri, Singleton<StringManager>.Instance.GetString("keFuZhuanQu"));
		}
		catch
		{
			string wordAtPosition = label.GetWordAtPosition(UICamera.lastHit.point);
			global::Debug.Log(new object[]
			{
				"Clicked on: " + wordAtPosition
			});
			this.AddElfQuestLine(new ElfQuestItem
			{
				strQuest = urlAtPosition,
				strShow = wordAtPosition
			});
			this.RefreshChatPanel();
			this.lastQueryQuest = urlAtPosition;
			GameUIFairyTalePopUp.HttpGetElfQueryUrl(1510, urlAtPosition);
		}
	}

	public static void HttpGetElfQueryUrl(ushort msgID, string quest = null)
	{
		string format = "http://sq.chatbot.nie.163.com/cgi-bin/bot.cgi?ques={0}&user={1}&encode=utf8";
		string arg = GameUIFairyTalePopUp.FormatUserStrInfo();
		string url = null;
		switch (msgID)
		{
		case 1508:
		{
			ObscuredStats data = Globals.Instance.Player.Data;
			url = string.Format(format, GameUIFairyTalePopUp.FormatChannelInfo() + WWW.EscapeURL(Singleton<StringManager>.Instance.GetString("FairyTxt_1", new object[]
			{
				data.Level,
				data.VipLevel
			}), Encoding.UTF8), arg);
			break;
		}
		case 1509:
		case 1510:
			url = string.Format(format, GameUIFairyTalePopUp.FormatChannelInfo() + quest, arg);
			break;
		}
		if (Globals.Instance.CliSession.HttpGet(url, msgID, false, null))
		{
			GameUIManager.mInstance.ShowIndicate();
		}
	}

	public static string FormatUserStrInfo()
	{
		ObscuredStats data = Globals.Instance.Player.Data;
		return string.Format("game_uid={0},level={1},vip={2},phonenumber={3},urs={4},nickname={5},game_server={6}", new object[]
		{
			data.ID,
			data.Level,
			data.VipLevel,
			"None",
			SdkU3d.getPropStr("FULL_UIN"),
			"None",
			GameSetting.Data.ServerID
		});
	}

	public static string FormatChannelInfo()
	{
		string[] array = SdkU3d.getPropStr("FULL_UIN").Split(new char[]
		{
			'@'
		});
		if (array.Length <= 1)
		{
			return string.Empty;
		}
		string key = array[1].ToLower();
		if (!GameUIFairyTalePopUp.codeDict.ContainsKey(key))
		{
			return string.Empty;
		}
		return GameUIFairyTalePopUp.codeDict[key] + " ";
	}

	public static void HttpGetElfCommentUrl(string quest, string answer, bool evaluate)
	{
		string format = "http://chatbot.nie.163.com:8080/cgi-bin/save_evaluate.py?gameid=77&question={0}&answer={1}&evaluate={2}&remarks={3}&encode=utf8";
		string text = GameUIFairyTalePopUp.FormatUserStrInfo();
		string url = string.Format(format, new object[]
		{
			GameUIFairyTalePopUp.FormatChannelInfo() + WWW.EscapeURL(quest, Encoding.UTF8),
			WWW.EscapeURL(answer, Encoding.UTF8),
			(!evaluate) ? 0 : 1,
			text
		});
		if (Globals.Instance.CliSession.HttpGet(url, 1511, false, null))
		{
			GameUIManager.mInstance.ShowIndicate();
		}
	}

	public static void HttpGetElfHotUrl(string prefix)
	{
		string format = "http://tip.chatbot.nie.163.com/cgi-bin/good_evaluate_question_tip.py?game=77&prefix={0}&max_num=30&enc=utf8&renc=utf8";
		string url = string.Format(format, WWW.EscapeURL(prefix, Encoding.UTF8));
		if (Globals.Instance.CliSession.HttpGet(url, 1512, false, null))
		{
			GameUIManager.mInstance.ShowIndicate();
		}
	}

	public static bool ElfIsValid(ushort msgID, BinaryReader reader)
	{
		int num = reader.ReadInt32();
		if (num != 200)
		{
			if (msgID == 1508)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("FairyR_1", 0f, 0f);
			}
			return false;
		}
		GameUIManager.mInstance.HideIndicate();
		return true;
	}

	public static void TryOpenElf(MemoryStream stream)
	{
		BinaryReader reader = new BinaryReader(stream);
		if (!GameUIFairyTalePopUp.ElfIsValid(1508, reader))
		{
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GameUIFairyTalePopUp, false, null, null);
		GameUIFairyTalePopUp gameUIFairyTalePopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GameUIFairyTalePopUp;
		ElfBtnItem[] initData = GameUIFairyTalePopUp.ParseElfBtnData(reader);
		gameUIFairyTalePopUp.InitFairyTale(initData);
	}

	private static string GetQueryAnswer(BinaryReader reader)
	{
		string text = reader.ReadString();
		string a = text.Substring(0, 2);
		if (a != "A:")
		{
			return null;
		}
		return text.Remove(0, 2);
	}

	private string IsValidComment(BinaryReader reader)
	{
		string text = reader.ReadString();
		return (!(text == "1")) ? text : null;
	}

	private List<string> GetHotDataList(BinaryReader reader)
	{
		string json = reader.ReadString();
		try
		{
			GameUIFairyTalePopUp.ElfHotData elfHotData = JsonMapper.ToObject<GameUIFairyTalePopUp.ElfHotData>(json);
			List<string> result;
			if (!elfHotData.success)
			{
				result = null;
				return result;
			}
			result = elfHotData.data;
			return result;
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Parse Login Json Error, {0}", ex.Message)
			});
		}
		return null;
	}

	private static ElfBtnItem[] ParseElfBtnData(BinaryReader reader)
	{
		string queryAnswer = GameUIFairyTalePopUp.GetQueryAnswer(reader);
		if (string.IsNullOrEmpty(queryAnswer))
		{
			return null;
		}
		return GameUIFairyTalePopUp.ParseElfBtnData(queryAnswer);
	}

	private static ElfBtnItem[] ParseElfBtnData(string result)
	{
		ElfBtnItem[] array = null;
		try
		{
			string[] array2 = result.Split(new char[]
			{
				'|'
			});
			char[] trimChars = new char[]
			{
				'\n'
			};
			array = new ElfBtnItem[array2.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i].Trim(trimChars);
				int num = text.IndexOf('@');
				string strName = text;
				string strQuest = text;
				if (num >= 0)
				{
					strName = text.Substring(0, num);
					strQuest = text.Substring(num + 1);
				}
				array[i] = new ElfBtnItem();
				array[i].strName = strName;
				array[i].strQuest = strQuest;
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Parse ElfQuery Error, {0}", ex.Message)
			});
		}
		return array;
	}

	private ElfInfoItem ParseElfContentData(BinaryReader reader)
	{
		string queryAnswer = GameUIFairyTalePopUp.GetQueryAnswer(reader);
		if (string.IsNullOrEmpty(queryAnswer))
		{
			return null;
		}
		return this.ParseElfContentData(queryAnswer);
	}

	private string FormatStrUrl(string url, string strShow, string strColor = "[0066cb]")
	{
		return string.Format(" [url={0}][u]{1}{2}[feeebd][/u][/url] ", url, strColor, strShow);
	}

	private string ParseStrData(string strData)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < strData.Length; i++)
		{
			this.ParseStrData(stringBuilder, strData, ref i);
		}
		return stringBuilder.ToString();
	}

	private string ParseStrParamData(string subQuest, string subParam)
	{
		string[] array = subParam.Split(new char[]
		{
			'&'
		});
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(subQuest);
		stringBuilder.Append(' ');
		ObscuredStats data = Globals.Instance.Player.Data;
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i];
			string text2 = string.Empty;
			string text3 = text;
			switch (text3)
			{
			case "vip":
				text2 = data.VipLevel.ToString();
				break;
			case "uid":
				text2 = data.ID.ToString();
				break;
			case "host":
				text2 = GameSetting.Data.ServerID.ToString();
				break;
			case "battle_id":
				text2 = Globals.Instance.Player.GetCurSceneID().ToString();
				break;
			case "trial":
				text2 = Globals.Instance.Player.Data.TrialMaxWave.ToString();
				break;
			case "lv":
				text2 = data.Level.ToString();
				break;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				if (i > 0)
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(string.Format("{0}={1}", text, text2));
			}
		}
		return stringBuilder.ToString();
	}

	private bool ParseStrUrlData(StringBuilder strBuilder, string strData, string tag, ref int index)
	{
		if (tag == "<ask>")
		{
			int num = strData.IndexOf("</ask>", index + 1);
			if (num < 0)
			{
				num = strData.Length - 1;
			}
			string text = strData.Substring(index + 1, num - index - 1);
			int num2 = text.IndexOf('@');
			string strShow = text;
			string text2 = text;
			bool flag = false;
			if (num2 >= 0 && num2 + 1 < text.Length && text[num2 + 1] != '@')
			{
				strShow = text.Substring(0, num2);
				text2 = text.Substring(num2 + 1);
				flag = true;
			}
			int num3 = text2.IndexOf("&");
			if (num3 >= 0)
			{
				string text3 = text2.Substring(0, num3);
				string subParam = text2.Substring(num3 + 1);
				if (!flag)
				{
					strShow = text3;
				}
				text2 = this.ParseStrParamData(text3, subParam);
			}
			string value = this.FormatStrUrl(text2, strShow, "[0066cb]");
			strBuilder.Append(value);
			index = num + 5;
			return true;
		}
		if (tag == "<url>")
		{
			int num4 = strData.IndexOf("</url>", index + 1);
			if (num4 < 0)
			{
				num4 = strData.Length - 1;
			}
			string text4 = strData.Substring(index + 1, num4 - index - 1);
			int num5 = text4.IndexOf("</show>", 5);
			string strShow2 = text4;
			string url = text4;
			if (text4.StartsWith("<show>") && num5 > 0)
			{
				strShow2 = text4.Substring(6, num5 - 6);
				url = text4.Substring(num5 + 7);
			}
			string value2 = this.FormatStrUrl(url, strShow2, "[0066cb]");
			strBuilder.Append(value2);
			index = num4 + 5;
			return true;
		}
		return false;
	}

	private void ParseStrData(StringBuilder strBuilder, string strData, ref int index)
	{
		char c = strData[index];
		if (c == '#')
		{
			if (index + 1 < strData.Length)
			{
				string text = strData.Substring(index, 2);
				index++;
				if (this.replaceStrMap.ContainsKey(text))
				{
					strBuilder.Append(this.replaceStrMap[text]);
				}
				else
				{
					strBuilder.Append(text);
				}
				return;
			}
		}
		else if (c == '<')
		{
			if (index + 2 < strData.Length)
			{
				int num = strData.IndexOf('>', index + 2);
				if (num >= 0)
				{
					string text2 = strData.Substring(index, num - index + 1);
					index = num;
					if (this.replaceStrMap.ContainsKey(text2))
					{
						strBuilder.Append(this.replaceStrMap[text2]);
					}
					else if (!this.ParseStrUrlData(strBuilder, strData, text2, ref index))
					{
						strBuilder.Append(text2);
					}
					return;
				}
			}
		}
		strBuilder.Append(c);
	}

	private ElfInfoItem ParseElfContentData(string result)
	{
		ElfInfoItem result2 = null;
		try
		{
			result = this.ParseStrData(result);
			string strTitle = null;
			int num = result.IndexOf('\n');
			string text;
			if (num < 0)
			{
				text = result;
			}
			else
			{
				strTitle = result.Substring(0, num);
				text = result.Remove(0, num + 1);
			}
			result2 = new ElfInfoItem
			{
				strTitle = strTitle,
				strContent = text.Insert(0, "[feeebd]")
			};
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Parse ElfContent Error, {0}", ex.Message)
			});
		}
		return result2;
	}

	public void OnMsgElfQuery(MemoryStream stream)
	{
		BinaryReader reader = new BinaryReader(stream);
		if (!GameUIFairyTalePopUp.ElfIsValid(1510, reader))
		{
			return;
		}
		string queryAnswer = GameUIFairyTalePopUp.GetQueryAnswer(reader);
		if (string.IsNullOrEmpty(queryAnswer))
		{
			return;
		}
		if (queryAnswer.IndexOf(Singleton<StringManager>.Instance.GetString("FairyTxt_6")) != 0)
		{
			string text = this.ParseStrData(queryAnswer).Insert(0, "[feeebd]");
			if (text.LastIndexOf("\n") == text.Length - 1)
			{
				text = text.Remove(text.Length - 2);
			}
			this.AddElfAnswerLine(new ElfAnswerItem
			{
				strAnswer = text,
				showEvaluate = true,
				strOriginal = queryAnswer,
				strQuest = this.lastQueryQuest
			});
		}
		else
		{
			string text2 = this.ParseStrData(queryAnswer.Remove(0, Singleton<StringManager>.Instance.GetString("FairyTxt_6").Length)).Insert(0, "[feeebd]");
			if (text2.LastIndexOf("\n") == text2.Length - 1)
			{
				text2 = text2.Remove(text2.Length - 2);
			}
			this.AddElfAnswerLine(new ElfAnswerItem
			{
				strAnswer = text2
			});
		}
		this.RefreshChatPanel();
		this.tabQuery.value = true;
	}

	public void OnMsgElfContent(MemoryStream stream)
	{
		BinaryReader reader = new BinaryReader(stream);
		if (!GameUIFairyTalePopUp.ElfIsValid(1509, reader))
		{
			return;
		}
		string queryAnswer = GameUIFairyTalePopUp.GetQueryAnswer(reader);
		if (string.IsNullOrEmpty(queryAnswer))
		{
			return;
		}
		string[] array = queryAnswer.Split(new char[]
		{
			'|'
		});
		if (array.Length > 1)
		{
			ElfBtnItem[] btnData = GameUIFairyTalePopUp.ParseElfBtnData(queryAnswer);
			this.current.RefreshBtnTab(btnData);
			return;
		}
		ElfInfoItem infoData = this.ParseElfContentData(queryAnswer);
		this.current.RefreshInfoPanel(infoData);
	}

	public void OnMsgElfComment(MemoryStream stream)
	{
		BinaryReader reader = new BinaryReader(stream);
		if (!GameUIFairyTalePopUp.ElfIsValid(1511, reader))
		{
			return;
		}
		string text = this.IsValidComment(reader);
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		global::Debug.LogError(new object[]
		{
			text
		});
	}

	public void OnMsgElfHot(MemoryStream stream)
	{
		BinaryReader reader = new BinaryReader(stream);
		if (!GameUIFairyTalePopUp.ElfIsValid(1512, reader))
		{
			return;
		}
		List<string> hotDataList = this.GetHotDataList(reader);
		if (hotDataList == null)
		{
			return;
		}
		if (hotDataList.Count == 0)
		{
			GameUIFairyTalePopUp.HttpGetElfQueryUrl(1510, this.lastQueryQuest);
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[feeebd]");
		stringBuilder.Append(Singleton<StringManager>.Instance.GetString("FairyTxt_8", new object[]
		{
			this.lastQueryQuest
		}));
		for (int i = 0; i < hotDataList.Count; i++)
		{
			string arg = this.FormatStrUrl(hotDataList[i], hotDataList[i], "[ffff00]");
			stringBuilder.Append(string.Format("\n{0}{1}{2}", i + 1, Singleton<StringManager>.Instance.GetString("Colon"), arg));
		}
		this.AddElfAnswerLine(new ElfAnswerItem
		{
			strAnswer = stringBuilder.ToString()
		});
		this.RefreshChatPanel();
		this.tabQuery.value = true;
	}
}
