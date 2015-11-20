using System;
using UnityEngine;

public class VoiceRecorderAndroidInterface : IVoiceRecorder
{
	private AndroidJavaObject mXFVoicePlugin;

	private void SafeCall(string method, params object[] args)
	{
		if (this.mXFVoicePlugin != null)
		{
			this.mXFVoicePlugin.Call(method, args);
		}
		else
		{
			global::Debug.LogError(new object[]
			{
				"mXFVoicePlugin is not created. you check is a call 'init' method"
			});
		}
	}

	public void Init(string name)
	{
		this.mXFVoicePlugin = new AndroidJavaObject("com.hsj.neteasevoice.VoiceRecorderPlugin", new object[0]);
		this.SafeCall("Init", new object[]
		{
			name
		});
	}

	public void Term()
	{
		this.SafeCall("Destroy", new object[0]);
	}

	public void StartRecord(string path, int bitDepth, string mrType)
	{
		this.SafeCall("StartRecord", new object[]
		{
			path,
			bitDepth,
			mrType
		});
	}

	public int StopRecord(bool isCancel)
	{
		if (this.mXFVoicePlugin != null)
		{
			return this.mXFVoicePlugin.Call<int>("StopRecord", new object[]
			{
				isCancel
			});
		}
		return 0;
	}

	public void StartPlay(string path, float volume)
	{
		this.SafeCall("StartPlay", new object[]
		{
			path,
			volume
		});
	}

	public void StopPlay()
	{
		this.SafeCall("StopPlay", new object[0]);
	}

	public bool IsPlaying()
	{
		return this.mXFVoicePlugin != null && this.mXFVoicePlugin.Call<bool>("IsPlaying", new object[0]);
	}

	public bool IsRecording()
	{
		return this.mXFVoicePlugin != null && this.mXFVoicePlugin.Call<bool>("IsRecording", new object[0]);
	}

	public void SetVoiceVolume(float num)
	{
		this.SafeCall("SetVoiceVolume", new object[]
		{
			num
		});
	}

	public int GetPowerForRecord()
	{
		if (this.mXFVoicePlugin != null)
		{
			return this.mXFVoicePlugin.Call<int>("GetPowerForRecord", new object[0]);
		}
		return 0;
	}

	public float GetPowerForRecordF()
	{
		if (this.mXFVoicePlugin != null)
		{
			return this.mXFVoicePlugin.Call<float>("GetPowerForRecordF", new object[0]);
		}
		return 0f;
	}
}
