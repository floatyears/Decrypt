using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public sealed class StringManager : Singleton<StringManager>
{
	private Dictionary<string, string> stringDict;

	private Dictionary<string, string> updateDict;

	private bool loaded;

	private List<string> namePart1;

	private List<string> namePart2;

	private List<string> namePart3;

	private List<string> namePart4;

	private List<string> loadingTips;

	private List<int> unlockNameList0;

	private Dictionary<int, string> unlockPicNameList;

	private StringBuilder tempSb = new StringBuilder();

	public static TextAsset LoadTextRes(string file)
	{
		string path = string.Format("Globalization/{0}/{1}", GameSetting.Language, file);
		return Res.Load<TextAsset>(path, false);
	}

	public bool LoadStringTable()
	{
		TextAsset textAsset = StringManager.LoadTextRes("StringTable");
		return !(textAsset == null) && StringManager.ParseString(ref this.stringDict, textAsset.text);
	}

	public bool LoadUpdateTable()
	{
		string path = string.Format("Globalization/{0}/{1}", GameSetting.Language, "UpdateTip");
		TextAsset textAsset = Resources.Load(path) as TextAsset;
		return !(textAsset == null) && StringManager.ParseString(ref this.updateDict, textAsset.text);
	}

	public static bool ParseString(ref Dictionary<string, string> strDict, string strData)
	{
		try
		{
			strDict = JsonMapper.ToObject<Dictionary<string, string>>(strData);
		}
		catch (Exception ex)
		{
			global::Debug.LogErrorFormat("Load string table error, {0}", new object[]
			{
				ex.Message
			});
			return false;
		}
		return true;
	}

	private string GetStringInternal(Dictionary<string, string> strDict, string key)
	{
		if (strDict == null)
		{
			return string.Empty;
		}
		string result;
		if (!strDict.TryGetValue(key, out result))
		{
			return string.Empty;
		}
		return result;
	}

	private string GetStringInternal(Dictionary<string, string> strDict, string key, params object[] arglist)
	{
		if (strDict == null)
		{
			global::Debug.LogErrorFormat("Get string table error [{0}] : Dictionary param is null", new object[]
			{
				key
			});
			return string.Empty;
		}
		string text;
		if (!strDict.TryGetValue(key, out text))
		{
			return string.Empty;
		}
		if (text.Length > 0 && arglist.Length > 0)
		{
			text = string.Format(text, arglist);
		}
		return text;
	}

	public string GetString(string key)
	{
		return this.GetStringInternal(this.stringDict, key);
	}

	public string GetString(string key, params object[] arglist)
	{
		return this.GetStringInternal(this.stringDict, key, arglist);
	}

	public string GetUpdateTip(string key)
	{
		return this.GetStringInternal(this.updateDict, key);
	}

	public string GetUpdateTip(string key, params object[] arglist)
	{
		return this.GetStringInternal(this.updateDict, key, arglist);
	}

	public string GetRecommendName(bool male = true)
	{
		if (!this.loaded)
		{
			this.loaded = true;
			this.namePart1 = this.LoadTextList("Name1");
			this.namePart2 = this.LoadTextList("Name2");
			this.namePart3 = this.LoadTextList("Name3");
			this.namePart4 = this.LoadTextList("Name4");
		}
		if (male)
		{
			int count = this.namePart1.Count;
			int count2 = this.namePart2.Count;
			if (count > 0 && count2 > 0)
			{
				int index = UnityEngine.Random.Range(0, count);
				int index2 = UnityEngine.Random.Range(0, count2);
				return string.Format("{0}{1}", this.namePart1[index].Trim(), this.namePart2[index2].Trim());
			}
		}
		else
		{
			int count3 = this.namePart3.Count;
			int count4 = this.namePart4.Count;
			if (count3 > 0 && count4 > 0)
			{
				int index3 = UnityEngine.Random.Range(0, count3);
				int index4 = UnityEngine.Random.Range(0, count4);
				return string.Format("{0}{1}", this.namePart3[index3].Trim(), this.namePart4[index4].Trim());
			}
		}
		return string.Empty;
	}

	public string GetLoadingTips()
	{
		if (this.loadingTips == null)
		{
			this.loadingTips = this.LoadTextList("LoadingTips");
		}
		int count = this.loadingTips.Count;
		if (count > 0)
		{
			int index = UnityEngine.Random.Range(0, count);
			return this.loadingTips[index];
		}
		return string.Empty;
	}

	private List<string> LoadTextList(string fileName)
	{
		TextAsset textAsset = StringManager.LoadTextRes(fileName);
		if (textAsset != null)
		{
			string[] array = textAsset.text.Split(new char[]
			{
				'\n'
			});
			List<string> list = new List<string>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim(new char[]
				{
					'\r'
				});
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(text);
				}
			}
			return list;
		}
		return null;
	}

	public string GetUnlockFunc(int _level, bool isNewGame)
	{
		this.LoadUnlockFunc();
		string text = string.Empty;
		if (this.unlockNameList0.Contains(_level))
		{
			if (_level == GameConst.GetInt32(6))
			{
				text = "pvpLb";
			}
			else if (_level == GameConst.GetInt32(8))
			{
				text = "itemSource18";
			}
			else if (_level == GameConst.GetInt32(7))
			{
				text = "xingZuoLb";
			}
			else if (_level == GameConst.GetInt32(5))
			{
				text = "trailTower0";
			}
			else if (_level == GameConst.GetInt32(2))
			{
				text = "activityKingRewardTitle";
			}
			else if (_level == GameConst.GetInt32(10))
			{
				text = "costumeParty";
			}
			else if (_level == GameConst.GetInt32(1))
			{
				text = "activityWorldBossTitle";
			}
			else if (_level == GameConst.GetInt32(4))
			{
				text = "guildLb";
			}
			else if (_level == GameConst.GetInt32(24))
			{
				text = "awakeRoad0";
			}
			else if (_level == GameConst.GetInt32(122))
			{
				text = "PetFurther11";
			}
			else if (_level == GameConst.GetInt32(32))
			{
				text = "guard0";
			}
			else if (_level == GameConst.GetInt32(246))
			{
				text = "MagicLove";
			}
			else if (_level == GameConst.GetInt32(201))
			{
				text = "LopetLb";
			}
			if (isNewGame)
			{
				return Singleton<StringManager>.Instance.GetString(text);
			}
		}
		if (!isNewGame)
		{
			this.tempSb.Remove(0, this.tempSb.Length);
			if (!string.IsNullOrEmpty(text))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString(text));
			}
			if (_level == GameConst.GetInt32(28))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("summonCollection"));
			}
			if (_level == GameConst.GetInt32(14))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("unlock0"));
			}
			if (_level == GameConst.GetInt32(11))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("unlock1"));
			}
			if (_level == GameConst.GetInt32(9))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("unlock3"));
			}
			if (_level == GameConst.GetInt32(13))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("unlock2"));
			}
			if (_level == GameConst.GetInt32(31))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("friendLb"));
			}
			if (_level == GameConst.GetInt32(197))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("unlock5"));
			}
			if (_level == GameConst.GetInt32(123))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("unlock4"));
			}
			if (_level == GameConst.GetInt32(186))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("unlock6"));
			}
			if (_level == GameConst.GetInt32(29))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("unlock7"));
			}
			if (_level == GameConst.GetInt32(15))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("d2m"));
			}
			if (_level == GameConst.GetInt32(198))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("MirrorLb"));
			}
			if (_level == GameConst.GetInt32(211))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("unlock9"));
			}
			if (_level == GameConst.GetInt32(213))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("unlock10"));
			}
			if (_level == GameConst.GetInt32(214))
			{
				this.tempSb.AppendLine(Singleton<StringManager>.Instance.GetString("unlock12"));
			}
			return this.tempSb.ToString().TrimEnd(new char[0]);
		}
		return string.Empty;
	}

	public string GetUnlockImage(int _level)
	{
		this.LoadUnlockFunc();
		if (this.unlockPicNameList.ContainsKey(_level))
		{
			return this.unlockPicNameList[_level];
		}
		return string.Empty;
	}

	private void LoadUnlockFunc()
	{
		if (this.unlockNameList0 == null)
		{
			this.unlockNameList0 = new List<int>();
			this.unlockPicNameList = new Dictionary<int, string>();
			this.unlockNameList0.Add(GameConst.GetInt32(6));
			this.unlockNameList0.Add(GameConst.GetInt32(8));
			this.unlockNameList0.Add(GameConst.GetInt32(7));
			this.unlockNameList0.Add(GameConst.GetInt32(5));
			this.unlockNameList0.Add(GameConst.GetInt32(2));
			this.unlockNameList0.Add(GameConst.GetInt32(10));
			this.unlockNameList0.Add(GameConst.GetInt32(24));
			this.unlockNameList0.Add(GameConst.GetInt32(1));
			this.unlockNameList0.Add(GameConst.GetInt32(4));
			this.unlockNameList0.Add(GameConst.GetInt32(122));
			this.unlockNameList0.Add(GameConst.GetInt32(32));
			this.unlockNameList0.Add(GameConst.GetInt32(246));
			this.unlockNameList0.Add(GameConst.GetInt32(201));
			this.unlockPicNameList.Add(GameConst.GetInt32(6), "pvp");
			this.unlockPicNameList.Add(GameConst.GetInt32(8), "pillage");
			this.unlockPicNameList.Add(GameConst.GetInt32(7), "constellation");
			this.unlockPicNameList.Add(GameConst.GetInt32(5), "trial");
			this.unlockPicNameList.Add(GameConst.GetInt32(2), "kingReward");
			this.unlockPicNameList.Add(GameConst.GetInt32(10), "costumeParty");
			this.unlockPicNameList.Add(GameConst.GetInt32(24), "awake");
			this.unlockPicNameList.Add(GameConst.GetInt32(1), "worldBoss");
			this.unlockPicNameList.Add(GameConst.GetInt32(4), "u10");
			this.unlockPicNameList.Add(GameConst.GetInt32(122), "cultivate");
			this.unlockPicNameList.Add(GameConst.GetInt32(32), "guard");
			this.unlockPicNameList.Add(GameConst.GetInt32(246), "magicLove");
			this.unlockPicNameList.Add(GameConst.GetInt32(201), "lopet");
		}
	}
}
