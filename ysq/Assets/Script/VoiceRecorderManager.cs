using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class VoiceRecorderManager : MonoBehaviour
{
	public delegate void VoiceRecordCallback(string param, string msg);

	public delegate void VoidParamCallback();

	public static string VoiceRecordURL = "http://voice.x.netease.com:8020/ma32/";

	public VoiceRecorderManager.VoiceRecordCallback VoiceRecordEvent;

	public VoiceRecorderManager.VoiceRecordCallback VoiceStartPlayEvent;

	public VoiceRecorderManager.VoidParamCallback VoiceStopPlayEvent;

	private IVoiceRecorder voiceRecorder;

	private string voiceDirectoryName;

	private int fileIndex;

	public string mPlayingVoiceParam = string.Empty;

	public void Awake()
	{
		this.voiceRecorder = new VoiceRecorderAndroidInterface();
		this.voiceRecorder.Init(base.name);
		this.voiceDirectoryName = Application.persistentDataPath + "/VoiceRecord";
	}

	private void Start()
	{
		this.CheckDirectory();
		Globals.Instance.CliSession.Register(1513, new ClientSession.MsgHandler(this.OnMsgUploadVoice));
		Globals.Instance.CliSession.Register(1514, new ClientSession.MsgHandler(this.OnMsgGetVoice));
		Globals.Instance.CliSession.Register(1515, new ClientSession.MsgHandler(this.OnMsgTranslateVoice));
		base.StartCoroutine(this.CheckVoiceExpired());
	}

	public void OnDestroy()
	{
		if (this.voiceRecorder != null)
		{
			this.voiceRecorder.Term();
		}
	}

	[DebuggerHidden]
	private IEnumerator CheckVoiceExpired()
	{
        return null;
        //VoiceRecorderManager.<CheckVoiceExpired>c__Iterator17 <CheckVoiceExpired>c__Iterator = new VoiceRecorderManager.<CheckVoiceExpired>c__Iterator17();
        //<CheckVoiceExpired>c__Iterator.<>f__this = this;
        //return <CheckVoiceExpired>c__Iterator;
	}

	public void DeleteVoiceDirectory()
	{
		if (Directory.Exists(this.voiceDirectoryName))
		{
			try
			{
				Directory.Delete(this.voiceDirectoryName, true);
			}
			catch (Exception ex)
			{
				global::Debug.LogFormat("DeleteVoiceDirectory: {0}", new object[]
				{
					ex.Message
				});
			}
		}
	}

	private bool CheckDirectory()
	{
		if (!Directory.Exists(this.voiceDirectoryName))
		{
			try
			{
				Directory.CreateDirectory(this.voiceDirectoryName);
			}
			catch (Exception ex)
			{
				global::Debug.Log(new object[]
				{
					"StartRecord CreateDirectory: {0}",
					ex.Message
				});
				return false;
			}
			return true;
		}
		return true;
	}

	public void SetVoiceVolume(float num)
	{
		if (this.voiceRecorder == null)
		{
			return;
		}
		this.voiceRecorder.SetVoiceVolume(num);
	}

	public bool StartRecord()
	{
		if (this.voiceRecorder == null || !this.CheckDirectory())
		{
			return false;
		}
		string text = string.Format("{0}/{1}_{2}.amr", this.voiceDirectoryName, Globals.Instance.Player.GetTimeStamp(), ++this.fileIndex);
		if (File.Exists(text))
		{
			File.Delete(text);
		}
		if (this.IsPlaying())
		{
			this.StopPlay();
		}
		this.voiceRecorder.StartRecord(text, 8, "MR475");
		return true;
	}

	public int StopRecord(bool cancel = false)
	{
		if (this.voiceRecorder == null)
		{
			return 0;
		}
		return this.voiceRecorder.StopRecord(cancel);
	}

	public bool IsRecording()
	{
		return this.voiceRecorder != null && this.voiceRecorder.IsRecording();
	}

	public bool IsAlreadyPlayed(string param)
	{
		string path = string.Format("{0}/{1}.amr", this.voiceDirectoryName, param);
		return File.Exists(path);
	}

	public void StartPlay(string param)
	{
		if (this.voiceRecorder == null)
		{
			return;
		}
		string text = string.Format("{0}/{1}.amr", this.voiceDirectoryName, param);
		if (File.Exists(text))
		{
			Globals.Instance.BackgroundMusicMgr.SetMusicVolume(0f);
			this.mPlayingVoiceParam = param;
			this.voiceRecorder.StartPlay(text, 20f);
		}
		else
		{
			string url = string.Format("{0}getfile?{1}", VoiceRecorderManager.VoiceRecordURL, param);
			Globals.Instance.CliSession.HttpGet(url, 1514, true, Encoding.ASCII.GetBytes(param));
		}
	}

	public void StopPlay()
	{
		if (this.voiceRecorder == null)
		{
			return;
		}
		this.voiceRecorder.StopPlay();
		this.mPlayingVoiceParam = string.Empty;
		if (this.VoiceStopPlayEvent != null)
		{
			this.VoiceStopPlayEvent();
		}
	}

	public int GetPowerForRecord()
	{
		if (this.voiceRecorder == null)
		{
			return 0;
		}
		return this.voiceRecorder.GetPowerForRecord();
	}

	public float GetPowerForRecordF()
	{
		if (this.voiceRecorder == null)
		{
			return 0f;
		}
		return this.voiceRecorder.GetPowerForRecordF();
	}

	public bool IsPlaying()
	{
		return this.voiceRecorder != null && this.voiceRecorder.IsPlaying();
	}

	public void onVoiceRecordFinish(string fileName)
	{
		if (fileName == null || fileName == string.Empty)
		{
			return;
		}
		if (!File.Exists(fileName))
		{
			global::Debug.LogErrorFormat("voice record error, file not eixt = {0}", new object[]
			{
				fileName
			});
			GameUIManager.mInstance.ShowMessageTipByKey("chatTxt36", 0f, 0f);
			return;
		}
		byte[] buffer = File.ReadAllBytes(fileName);
		string fileMD5Internal = BMUtility.GetFileMD5Internal(buffer);
		string url = string.Format("{0}upload?md5={1}&host={2}&tousers=_1_&usernum={3}", new object[]
		{
			VoiceRecorderManager.VoiceRecordURL,
			fileMD5Internal,
			GameSetting.Data.ServerID,
			Globals.Instance.Player.Data.AccountID
		});
		Globals.Instance.CliSession.UploadVoice(url, buffer, 1513);
	}

	public void onVoicePlayFinish(string fileName)
	{
		global::Debug.LogFormat("onVoicePlayFinish, file = {0}", new object[]
		{
			fileName
		});
		Globals.Instance.BackgroundMusicMgr.SetMusicVolume(1f);
		this.mPlayingVoiceParam = string.Empty;
		if (this.VoiceStopPlayEvent != null)
		{
			this.VoiceStopPlayEvent();
		}
	}

	public void OnMsgUploadVoice(MemoryStream stream)
	{
		BinaryReader binaryReader = new BinaryReader(stream);
		int num = binaryReader.ReadInt32();
		if (num != 200)
		{
			return;
		}
		string text = binaryReader.ReadString();
		int num2 = text.IndexOf('\n');
		if (num2 != -1)
		{
			string text2 = text.Substring(0, num2);
			if (text2 == "0")
			{
				string text3 = text.Substring(num2 + 1);
				string s = string.Format("key={0}&host={1}&usernum={2}", text3, GameSetting.Data.ServerID, Globals.Instance.Player.Data.AccountID);
				string url = string.Format("{0}get_translation?key={1}", VoiceRecorderManager.VoiceRecordURL, text3);
				Globals.Instance.CliSession.HttpGet(url, 1515, false, Encoding.ASCII.GetBytes(s));
			}
			else
			{
				global::Debug.LogFormat("ret = {0}, msg = {1}", new object[]
				{
					text2,
					text.Substring(num2 + 1)
				});
			}
		}
	}

	public void OnMsgGetVoice(MemoryStream stream)
	{
		BinaryReader binaryReader = new BinaryReader(stream);
		int count = binaryReader.ReadInt32();
		byte[] array = binaryReader.ReadBytes(count);
		string @string = Encoding.ASCII.GetString(array);
		int num = binaryReader.ReadInt32();
		global::Debug.Log(new object[]
		{
			"code = " + num
		});
		if (num != 200)
		{
			return;
		}
		string arg = string.Empty;
		char c = binaryReader.ReadChar();
		while (c != '\0' && c != '\n')
		{
			arg += c;
			c = binaryReader.ReadChar();
		}
		array = binaryReader.ReadBytes((int)(stream.Length - stream.Position));
		string text = string.Format("{0}/{1}.amr", this.voiceDirectoryName, @string);
		if (File.Exists(text))
		{
			File.Delete(text);
		}
		using (FileStream fileStream = new FileStream(text, FileMode.Create))
		{
			fileStream.Write(array, 0, array.Length);
			fileStream.Flush();
			fileStream.Close();
		}
		if (this.voiceRecorder != null)
		{
			Globals.Instance.BackgroundMusicMgr.SetMusicVolume(0f);
			this.mPlayingVoiceParam = @string;
			this.voiceRecorder.StartPlay(text, 20f);
			if (this.VoiceStartPlayEvent != null)
			{
				this.VoiceStartPlayEvent(@string, string.Empty);
			}
		}
	}

	public void OnMsgTranslateVoice(MemoryStream stream)
	{
		BinaryReader binaryReader = new BinaryReader(stream);
		int count = binaryReader.ReadInt32();
		byte[] bytes = binaryReader.ReadBytes(count);
		string @string = Encoding.ASCII.GetString(bytes);
		int num = binaryReader.ReadInt32();
		if (num != 200)
		{
			if (this.VoiceRecordEvent != null)
			{
				this.VoiceRecordEvent(@string, string.Empty);
			}
			return;
		}
		string text = binaryReader.ReadString();
		int num2 = text.IndexOf('\n');
		string msg = string.Empty;
		if (num2 != -1)
		{
			string a = text.Substring(0, num2);
			if (a == "0")
			{
				msg = text.Substring(num2 + 1);
			}
			else
			{
				msg = Singleton<StringManager>.Instance.GetString("VoiceError");
			}
		}
		if (this.VoiceRecordEvent != null)
		{
			this.VoiceRecordEvent(@string, msg);
		}
	}
}
