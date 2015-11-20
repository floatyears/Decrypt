using Att;
using Proto;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GuildLogChunk : MonoBehaviour
{
	private UILabel mTime;

	private UILabel mContents;

	private StringBuilder mStringBuilder = new StringBuilder();

	private int mGuildEventStartIndex = -1;

	private int mGuildEventEndIndex = -1;

	public int GetTimeStamp()
	{
		if (this.mGuildEventStartIndex != -1 && this.mGuildEventEndIndex < Globals.Instance.Player.GuildSystem.GuildEventList.Count)
		{
			return Globals.Instance.Player.GuildSystem.GuildEventList[this.mGuildEventStartIndex].TimeStamp;
		}
		return 0;
	}

	public void InitWithBaseScene(int startIndex, int endIndex)
	{
		this.mGuildEventStartIndex = startIndex;
		this.mGuildEventEndIndex = endIndex;
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		this.mTime = base.transform.Find("title/time").GetComponent<UILabel>();
		this.mContents = base.transform.Find("contentTxt").GetComponent<UILabel>();
	}

	private void Refresh()
	{
		List<GuildEvent> guildEventList = Globals.Instance.Player.GuildSystem.GuildEventList;
		if (this.mGuildEventStartIndex != -1 && this.mGuildEventEndIndex < guildEventList.Count)
		{
			this.mTime.text = Tools.ServerDateTimeFormat3(guildEventList[this.mGuildEventStartIndex].TimeStamp);
			this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
			for (int i = this.mGuildEventStartIndex; i <= this.mGuildEventEndIndex; i++)
			{
				this.mStringBuilder.Append("[9c8559]").Append(Tools.ServerDateTimeFormat4(guildEventList[i].TimeStamp)).Append("[-] ");
				switch (guildEventList[i].Type)
				{
				case 1:
					if (guildEventList[i].Value1 == 3)
					{
						this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("guildLog0", new object[]
						{
							guildEventList[i].Value3
						}));
					}
					else if (guildEventList[i].Value1 == 4)
					{
						this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("guildLog1", new object[]
						{
							guildEventList[i].Value3
						}));
					}
					else if (guildEventList[i].Value1 == 2)
					{
						this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("guildLog9", new object[]
						{
							guildEventList[i].Value3
						}));
					}
					if (i != this.mGuildEventEndIndex)
					{
						this.mStringBuilder.AppendLine();
					}
					break;
				case 2:
					this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("guildLog2", new object[]
					{
						guildEventList[i].Value3,
						guildEventList[i].Value4
					}));
					if (i != this.mGuildEventEndIndex)
					{
						this.mStringBuilder.AppendLine();
					}
					break;
				case 3:
					this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("guildLog3", new object[]
					{
						guildEventList[i].Value3
					}));
					if (i != this.mGuildEventEndIndex)
					{
						this.mStringBuilder.AppendLine();
					}
					break;
				case 4:
					this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("guildLog4", new object[]
					{
						guildEventList[i].Value3
					}));
					if (i != this.mGuildEventEndIndex)
					{
						this.mStringBuilder.AppendLine();
					}
					break;
				case 5:
					this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("guildLog5", new object[]
					{
						guildEventList[i].Value1
					}));
					if (i != this.mGuildEventEndIndex)
					{
						this.mStringBuilder.AppendLine();
					}
					break;
				case 6:
					this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("guildLog6", new object[]
					{
						guildEventList[i].Value3
					}));
					if (i != this.mGuildEventEndIndex)
					{
						this.mStringBuilder.AppendLine();
					}
					break;
				case 7:
					this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("guildLog10", new object[]
					{
						guildEventList[i].Value3,
						guildEventList[i].Value4
					}));
					if (i != this.mGuildEventEndIndex)
					{
						this.mStringBuilder.AppendLine();
					}
					break;
				case 10:
					this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("guildLog11", new object[]
					{
						guildEventList[i].Value3
					}));
					if (i != this.mGuildEventEndIndex)
					{
						this.mStringBuilder.AppendLine();
					}
					break;
				case 11:
				{
					GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(guildEventList[i].Value1);
					if (info != null)
					{
						this.mStringBuilder.Append(Singleton<StringManager>.Instance.GetString("guildLog12", new object[]
						{
							guildEventList[i].Value3,
							info.Academy
						}));
						if (i != this.mGuildEventEndIndex)
						{
							this.mStringBuilder.AppendLine();
						}
					}
					break;
				}
				case 12:
					this.mStringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("guildLog13"), guildEventList[i].Value4);
					if (i != this.mGuildEventEndIndex)
					{
						this.mStringBuilder.AppendLine();
					}
					break;
				}
			}
			this.mContents.text = this.mStringBuilder.ToString();
		}
	}
}
