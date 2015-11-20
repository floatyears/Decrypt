using LitJson;
using System;
using System.Collections.Generic;

namespace NtUniSdk.Unity3d
{
	internal class AccountInfo
	{
		public const string UNKNOWN = "unknown";

		public const string ACOUNT_ID = "accountId";

		public const string ID_TYPE = "idType";

		public const string NICKNAME = "nickname";

		public const string ICON = "icon";

		public const string IN_GAME = "inGame";

		public const string RANK_SCORE = "rankScore";

		public const string RANK = "rank";

		public const string REMARK = "remark";

		public string accountId;

		public string idType;

		public string nickname;

		public string icon;

		public bool inGame;

		public double rankScore;

		public long rank;

		public string remark;

		public static List<AccountInfo> ListFromJsonData(JsonData json)
		{
			List<AccountInfo> list = new List<AccountInfo>();
			for (int i = 0; i < json.Count; i++)
			{
				AccountInfo item = AccountInfo.fromJsonData(json[i]);
				list.Add(item);
			}
			return list; 
		}

		public static AccountInfo fromJsonData(JsonData json)
		{
			return new AccountInfo
			{
				accountId = (string)json["accountId"],
				idType = (string)json["idType"],
				nickname = (string)json["nickname"],
				icon = (string)json["icon"],
				inGame = bool.Parse(json["inGame"].ToString()),
				rankScore = double.Parse(json["rankScore"].ToString()),
				rank = long.Parse(json["rank"].ToString()),
				remark = (string)json["remark"]
			};
		}

		public JsonData toJsonData()
		{
			JsonData jsonData = new JsonData();
			jsonData["accountId"] = this.accountId;
			jsonData["idType"] = this.idType;
			jsonData["nickname"] = this.nickname;
			jsonData["icon"] = this.icon;
			jsonData["inGame"] = this.inGame;
			jsonData["rankScore"] = this.rankScore;
			jsonData["rank"] = this.rank;
			return jsonData;
		}
	}
}
