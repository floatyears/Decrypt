using LitJson;
using System;

public class GUIVoiceChatData
{
	public int VoiceTime
	{
		get;
		set;
	}

	public string VoiceMsg
	{
		get;
		set;
	}

	public string VoiceTranslateParam
	{
		get;
		set;
	}

	public JsonData ToJsonData()
	{
		JsonData jsonData = new JsonData();
		jsonData["time"] = this.VoiceTime;
		jsonData["msg"] = this.VoiceMsg;
		jsonData["param"] = this.VoiceTranslateParam;
		return jsonData;
	}

	private JsonData getData(JsonData json, string key)
	{
		JsonData result;
		try
		{
			result = json[key];
		}
		catch (Exception ex)
		{
			Debug.LogError(new object[]
			{
				ex.Message
			});
			result = null;
		}
		return result;
	}

	public void FromJsonStr(string jsonStr)
	{
		JsonData jsonData = JsonMapper.ToObject(jsonStr);
		if (jsonData == null)
		{
			return;
		}
		JsonData data = this.getData(jsonData, "time");
		this.VoiceTime = ((data == null) ? 0 : ((int)data));
		data = this.getData(jsonData, "msg");
		this.VoiceMsg = ((data == null) ? string.Empty : ((string)data));
		data = this.getData(jsonData, "param");
		this.VoiceTranslateParam = ((data == null) ? string.Empty : ((string)data));
	}
}
