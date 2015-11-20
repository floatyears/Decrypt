using System;
using UnityEngine;

public class EffectSoundManager : MonoBehaviour
{
	public float masterVolume = 1f;

	public float tutorialVolume = 1.5f;

	public bool gameBGMVolumeDownTime;

	private float gameVolumeDownTimeInitTime;

	private float gameVolumeDownTimeTime;

	private float pauseInitTime;

	private int pauseRunningTime;

	public float volumeDown = 0.5f;

	private ResourceLoader ResourceCache = new ResourceLoader();

	public bool pause
	{
		get;
		private set;
	}

	public static bool IsEffectSoundOptionOn()
	{
		return GameSetting.Data.Sound;
	}

	public static bool IsEffectVoiceOptionOn()
	{
		return GameSetting.Data.Voice;
	}

	public void CacheSoundResourceSync(string _soundPath, float cacheTime)
	{
		if (string.IsNullOrEmpty(_soundPath))
		{
			return;
		}
		this.ResourceCache.LoadResourceAsync(string.Format("Audio/{0}", _soundPath), typeof(AudioClip), null, cacheTime);
	}

	private AudioClip LoadAudioClip(string _soundPath)
	{
		if (string.IsNullOrEmpty(_soundPath))
		{
			return null;
		}
		return this.ResourceCache.LoadResource<AudioClip>(string.Format("Audio/{0}", _soundPath), 2f);
	}

	public void ClearCache()
	{
		this.ResourceCache.Clear();
	}

	public void Pause(int PauseTime)
	{
		this.pauseInitTime = Time.time;
		this.pauseRunningTime = PauseTime;
		this.pause = true;
	}

	public float Play(string Sound)
	{
		return this.Play(Sound, 1f, Vector3.zero);
	}

	public float Play(string Sound, float Volume, Vector3 Pos)
	{
		if (!this.pause && EffectSoundManager.IsEffectSoundOptionOn() && !Globals.Instance.VoiceMgr.IsPlaying() && !Globals.Instance.VoiceMgr.IsRecording())
		{
			AudioClip audioClip = this.LoadAudioClip(Sound);
			if (audioClip != null)
			{
				this.Play(audioClip, Volume * this.masterVolume, Pos);
				return audioClip.length;
			}
		}
		return 0f;
	}

	public void Play(AudioClip clip, float Volume, Vector3 Pos)
	{
		if (clip != null && !this.pause && EffectSoundManager.IsEffectSoundOptionOn() && !Globals.Instance.VoiceMgr.IsPlaying() && !Globals.Instance.VoiceMgr.IsRecording())
		{
			float volumeScale = Volume * this.masterVolume;
			base.audio.PlayOneShot(clip, volumeScale);
		}
	}

	public float PlayTutorial(string Sound)
	{
		return this.PlayTutorial(Sound, 1f, Vector3.zero);
	}

	public void StopTutorial()
	{
		base.audio.Stop();
	}

	public float PlayTutorial(string Sound, float Volume, Vector3 Pos)
	{
		if (!this.pause && EffectSoundManager.IsEffectSoundOptionOn())
		{
			AudioClip audioClip = this.LoadAudioClip(string.Format("tutorial/{0}", Sound));
			if (audioClip != null)
			{
				float volumeScale = Volume * this.tutorialVolume;
				base.audio.Stop();
				base.audio.PlayOneShot(audioClip, volumeScale);
				return audioClip.length;
			}
		}
		return 0f;
	}

	public float PlayVoice(string Sound, float Volume, Vector3 Pos)
	{
		if (!string.IsNullOrEmpty(Sound) && !this.pause && EffectSoundManager.IsEffectVoiceOptionOn() && !Globals.Instance.VoiceMgr.IsPlaying() && !Globals.Instance.VoiceMgr.IsRecording())
		{
			AudioClip audioClip = this.LoadAudioClip(Sound);
			if (audioClip != null)
			{
				this.Play(audioClip, Volume * this.masterVolume, Pos);
				return audioClip.length;
			}
		}
		return 0f;
	}

	private float PlayVoice(AudioClip clip, float Volume, Vector3 Pos)
	{
		if (clip != null && !this.pause && EffectSoundManager.IsEffectVoiceOptionOn() && !Globals.Instance.VoiceMgr.IsPlaying() && !Globals.Instance.VoiceMgr.IsRecording())
		{
			float num = Volume * this.masterVolume;
			base.audio.PlayOneShot(clip, num);
			AudioSource.PlayClipAtPoint(clip, Pos, num);
			return clip.length;
		}
		return 0f;
	}

	private void Update()
	{
		if (this.pause && Time.time - this.pauseInitTime >= (float)this.pauseRunningTime)
		{
			this.pause = false;
		}
		if (this.gameBGMVolumeDownTime && Time.time - this.gameVolumeDownTimeInitTime >= this.gameVolumeDownTimeTime)
		{
			this.gameBGMVolumeDownTime = false;
			this.masterVolume = 1f;
		}
		this.ResourceCache.Update();
	}

	public void VolumeDown(float DownTime)
	{
		this.gameBGMVolumeDownTime = true;
		this.gameVolumeDownTimeInitTime = Time.time;
		this.gameVolumeDownTimeTime = DownTime;
		this.masterVolume = this.volumeDown;
	}
}
