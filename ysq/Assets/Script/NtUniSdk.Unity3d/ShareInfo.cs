using LitJson;
using System;

namespace NtUniSdk.Unity3d
{
	public class ShareInfo
	{
		public string toUser;

		public string title;

		public string desc;

		public string image;

		public string text;

		public string link;

		public JsonData ToJsonData()
		{
			JsonData jsonData = new JsonData();
			jsonData["toUser"] = this.toUser;
			jsonData["title"] = this.title;
			jsonData["desc"] = this.desc;
			jsonData["image"] = this.image;
			jsonData["text"] = this.text;
			jsonData["link"] = this.link;
			return jsonData;
		}
	}
}
