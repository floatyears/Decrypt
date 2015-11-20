using LitJson;
using System;

namespace NtUniSdk.Unity3d
{
	public class QueryRankInfo
	{
		public const string QUERY_RANK_TYPE_SCORE = "QUERY_RANK_TYPE_SCORE";

		public const string QUERY_RANK_TYPE_GRADE = "QUERY_RANK_TYPE_GRADE";

		public const string QUERY_RANK_SCOPE_ALL = "QUERY_RANK_SCOPE_ALL";

		public const string QUERY_RANK_SCOPE_FRIEND = "QUERY_RANK_SCOPE_FRIEND";

		private const string QUERY_RANK_TYPE = "queryRankType";

		private const string QUERY_RANK_Scope = "queryRankScope";

		public string queryRankType;

		public string queryRankScope;

		public JsonData ToJsonData()
		{
			JsonData jsonData = new JsonData();
			jsonData["queryRankType"] = this.queryRankType;
			jsonData["QUERY_RANK_TYPE_SCORE"] = this.queryRankScope;
			return jsonData;
		}
	}
}
