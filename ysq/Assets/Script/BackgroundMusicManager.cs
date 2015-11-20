using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
	public enum RUNSOUND : byte
	{
		SEND,
		ROCK,
		GRASS,
		WATER,
		ICE,
		MAX
	}

	private delegate void AduioLoadAsyncCallback(AudioClip clip);

	private BackgroundMusicManager.RUNSOUND CurRUNSOUND;

	private bool gameBGMPause;

	private bool gameBGMVolumeDownTime;

	private AudioSource gameClearAudio;

	private AudioSource gameETCSound;

	private AudioSource gameOverAudio;

	private float gamePauseInitTime;

	private float gamePauseTime;

	private AudioSource gameSound;

	private float gameVolumeDownTimeInitTime;

	private float gameVolumeDownTimeTime;

	private AudioSource lobbySound;

	private AudioSource warmingSound;

	public void ClearAllGameSound()
	{
		this.ClearGameBGM();
		this.ReleaseAudioClip(this.gameClearAudio);
		UnityEngine.Object.DestroyImmediate(this.gameClearAudio);
		this.gameClearAudio = null;
		this.ReleaseAudioClip(this.warmingSound);
		UnityEngine.Object.DestroyImmediate(this.warmingSound);
		this.warmingSound = null;
		this.ReleaseAudioClip(this.gameOverAudio);
		UnityEngine.Object.DestroyImmediate(this.gameOverAudio);
		this.gameOverAudio = null;
	}

	public void ClearGameBGM()
	{
		this.ReleaseAudioClip(this.gameSound);
		UnityEngine.Object.DestroyImmediate(this.gameSound);
		this.gameSound = null;
		this.ReleaseAudioClip(this.gameETCSound);
		UnityEngine.Object.DestroyImmediate(this.gameETCSound);
		this.gameETCSound = null;
	}

	public void ClearLobbySound()
	{
		this.StopLobbySound();
		if (null != this.lobbySound)
		{
			this.lobbySound.clip = null;
			this.lobbySound = null;
		}
	}

	public void ClearAll()
	{
		base.StopAllCoroutines();
		this.ClearAllGameSound();
		this.ClearLobbySound();
	}

	private void LoadAudioClip(string _soundPath, BackgroundMusicManager.AduioLoadAsyncCallback cb)
	{
		if (string.IsNullOrEmpty(_soundPath))
		{
			return;
		}
		base.StartCoroutine(this.LoadResourceAsync(string.Format("Audio/{0}", _soundPath), cb));
	}

	[DebuggerHidden]
	private IEnumerator LoadResourceAsync(string clipName, BackgroundMusicManager.AduioLoadAsyncCallback cb)
	{
        return null;
        //BackgroundMusicManager.<LoadResourceAsync>c__Iterator12 <LoadResourceAsync>c__Iterator = new BackgroundMusicManager.<LoadResourceAsync>c__Iterator12();
        //<LoadResourceAsync>c__Iterator.clipName = clipName;
        //<LoadResourceAsync>c__Iterator.cb = cb;
        //<LoadResourceAsync>c__Iterator.<$>clipName = clipName;
        //<LoadResourceAsync>c__Iterator.<$>cb = cb;
        //return <LoadResourceAsync>c__Iterator;
	}

	private string GetClipNameFromPath(string _soundPath)
	{
		char[] separator = new char[]
		{
			'/'
		};
		string[] array = _soundPath.Split(separator);
		return array[array.Length - 1];
	}

	public bool IsLobbySoundPlaying()
	{
		return this.lobbySound != null && this.lobbySound.isPlaying;
	}

	public bool IsGameBGMPlaying()
	{
		return this.gameSound != null && this.gameSound.isPlaying;
	}

	public bool IsBGMOptionOn()
	{
		return GameSetting.Data.Music;
	}

	public void PauseLobbyMusic(bool pause)
	{
		if (null != this.gameETCSound)
		{
			if (pause)
			{
				this.gameETCSound.Pause();
			}
			else
			{
				this.gameETCSound.Play();
			}
		}
	}

	public void PauseGameBGM(float PauseTime)
	{
		this.gameBGMPause = true;
		this.gamePauseInitTime = Time.time;
		this.gamePauseTime = PauseTime;
		if (null != this.gameETCSound)
		{
			this.gameETCSound.Pause();
		}
		if (null != this.gameSound)
		{
			this.gameSound.Pause();
		}
	}

	public void PlayLobbyMusic()
	{
		this.PlayLobbyMusic("bg/bg_001", true);
	}

	public void PlayLobbyMusic(string musicName, bool loop = true)
	{
		if (this.IsBGMOptionOn() && !string.IsNullOrEmpty(musicName))
		{
			if (null != this.lobbySound && null != this.lobbySound.clip && this.lobbySound.clip.name.CompareTo(this.GetClipNameFromPath(musicName)) == 0)
			{
				if (!this.lobbySound.isPlaying)
				{
					this.lobbySound.Play();
				}
			}
			else
			{
				this.ClearLobbySound();
				this.ClearAllGameSound();
				if (this.lobbySound == null)
				{
					this.lobbySound = (base.gameObject.AddComponent("AudioSource") as AudioSource);
				}
				this.LoadAudioClip(musicName, delegate(AudioClip clip)
				{
					if (this.lobbySound)
					{
						this.lobbySound.clip = clip;
						this.lobbySound.loop = loop;
						this.lobbySound.Play();
					}
				});
			}
		}
	}

	public void PlayGameBGM(string SoundName)
	{
		if (this.IsBGMOptionOn())
		{
			this.ClearLobbySound();
			this.ClearGameBGM();
			if (this.gameSound == null)
			{
				this.gameSound = (base.gameObject.AddComponent("AudioSource") as AudioSource);
			}
			this.LoadAudioClip(string.Format("bg/{0}", SoundName), delegate(AudioClip clip)
			{
				if (this.gameSound)
				{
					this.gameSound.clip = clip;
					this.gameSound.loop = true;
					this.gameSound.Play();
				}
			});
		}
	}

	public void PlayGameBGM_ETC(string SoundName)
	{
		if (this.IsBGMOptionOn() && !string.IsNullOrEmpty(SoundName))
		{
			if (this.gameETCSound == null)
			{
				this.gameETCSound = (base.gameObject.AddComponent("AudioSource") as AudioSource);
			}
			this.LoadAudioClip(string.Format("bg/{0}", SoundName), delegate(AudioClip clip)
			{
				if (this.gameETCSound)
				{
					this.gameETCSound.clip = clip;
					this.gameETCSound.loop = true;
					this.gameETCSound.Play();
				}
			});
		}
	}

	public void PlayGameBGMPlayOnce(string SoundName)
	{
		if (this.IsBGMOptionOn())
		{
			this.PlayGameBGM(SoundName);
			if (this.gameSound)
			{
				this.gameSound.loop = false;
			}
		}
	}

	public void PlayGameClearSound()
	{
		if (this.IsBGMOptionOn())
		{
			if (this.gameClearAudio == null)
			{
				this.gameClearAudio = (base.gameObject.AddComponent("AudioSource") as AudioSource);
			}
			this.LoadAudioClip("ui/ui_004", delegate(AudioClip clip)
			{
				if (this.gameClearAudio)
				{
					this.gameClearAudio.clip = clip;
					this.gameClearAudio.Play();
					this.gameClearAudio.loop = false;
				}
			});
		}
	}

	public void PlayGameOverSound()
	{
		if (this.IsBGMOptionOn())
		{
			if (this.gameOverAudio == null)
			{
				this.gameOverAudio = (base.gameObject.AddComponent("AudioSource") as AudioSource);
			}
			this.LoadAudioClip("ui/ui_005", delegate(AudioClip clip)
			{
				if (this.gameOverAudio)
				{
					this.gameOverAudio.clip = clip;
					this.gameOverAudio.Play();
					this.gameOverAudio.loop = false;
				}
			});
		}
	}

	public void PlayWarmingSound()
	{
		if (this.IsBGMOptionOn())
		{
			if (this.warmingSound == null)
			{
				this.warmingSound = (base.gameObject.AddComponent("AudioSource") as AudioSource);
			}
			if (this.warmingSound.clip == null)
			{
				this.LoadAudioClip("bg/bg_017", delegate(AudioClip clip)
				{
					if (this.warmingSound)
					{
						this.warmingSound.clip = clip;
						this.warmingSound.volume = 10000f;
						this.warmingSound.loop = true;
						this.warmingSound.Play();
					}
				});
			}
			else
			{
				this.warmingSound.volume = 10000f;
				this.warmingSound.loop = true;
				this.warmingSound.Play();
			}
		}
	}

	private void ReleaseAudioClip(AudioSource asource)
	{
		if (null != asource && null != asource.clip)
		{
			asource.clip = null;
		}
	}

	public void StopGameBGM()
	{
		if (!this.IsBGMOptionOn() && this.gameSound != null)
		{
			this.gameSound.Stop();
			this.ReleaseAudioClip(this.gameSound);
			this.gameSound.clip = null;
			UnityEngine.Object.Destroy(this.gameSound);
			this.gameSound = null;
		}
	}

	public void StopGameBGM_Etc()
	{
		if (!this.IsBGMOptionOn() && this.gameETCSound != null)
		{
			this.gameETCSound.Stop();
			this.gameETCSound.clip = null;
			this.ReleaseAudioClip(this.gameSound);
		}
	}

	public void StopGameClearSound()
	{
		if (this.gameClearAudio != null)
		{
			this.gameClearAudio.Stop();
		}
	}

	public void StopGameOverSound()
	{
		if (this.gameOverAudio != null)
		{
			this.gameOverAudio.Stop();
		}
	}

	public void StopLobbySound()
	{
		if (this.lobbySound != null)
		{
			this.lobbySound.Stop();
		}
	}

	public void StopWarmingSound()
	{
		if (this.warmingSound != null)
		{
			this.warmingSound.Stop();
		}
	}

	private void Update()
	{
		if (this.gameBGMPause && Time.time - this.gamePauseInitTime >= this.gamePauseTime)
		{
			this.gameBGMPause = false;
			if (null != this.gameETCSound && this.gameETCSound.loop)
			{
				this.gameETCSound.Play();
			}
			if (null != this.gameSound && this.gameSound.loop)
			{
				this.gameSound.Play();
			}
		}
		if (this.gameBGMVolumeDownTime && Time.time - this.gameVolumeDownTimeInitTime >= this.gameVolumeDownTimeTime)
		{
			if (null != this.gameETCSound)
			{
				this.gameETCSound.volume = this.gameETCSound.volume + 2f * Time.smoothDeltaTime;
				if (this.gameETCSound.volume >= 1f)
				{
					this.gameETCSound.volume = 1f;
					this.gameBGMVolumeDownTime = false;
				}
			}
			if (null != this.gameSound)
			{
				this.gameSound.volume = this.gameSound.volume + 2f * Time.smoothDeltaTime;
				if (this.gameSound.volume >= 1f)
				{
					this.gameSound.volume = 1f;
				}
			}
			if (null != this.lobbySound)
			{
				this.lobbySound.volume = this.lobbySound.volume + 2f * Time.smoothDeltaTime;
				if (this.lobbySound.volume >= 1f)
				{
					this.lobbySound.volume = 1f;
				}
			}
		}
	}

	public void VolumeDown(float DownTime)
	{
		this.gameBGMVolumeDownTime = true;
		this.gameVolumeDownTimeInitTime = Time.time;
		this.gameVolumeDownTimeTime = DownTime;
		if (null != this.gameETCSound)
		{
			this.gameETCSound.volume = Globals.Instance.EffectSoundMgr.volumeDown;
		}
		if (null != this.gameSound)
		{
			this.gameSound.volume = Globals.Instance.EffectSoundMgr.volumeDown;
		}
		if (null != this.lobbySound)
		{
			this.lobbySound.volume = Globals.Instance.EffectSoundMgr.volumeDown;
		}
	}

	public void SetMusicVolume(float volumeNum)
	{
		if (null != this.gameETCSound)
		{
			this.gameETCSound.volume = volumeNum;
		}
		if (null != this.gameSound)
		{
			this.gameSound.volume = volumeNum;
		}
		if (null != this.lobbySound)
		{
			this.lobbySound.volume = volumeNum;
		}
	}

	public void PlayPauseMusic(bool isPause)
	{
		if (isPause)
		{
			if (null != this.gameETCSound)
			{
				this.gameETCSound.Pause();
			}
			if (null != this.gameSound)
			{
				this.gameSound.Pause();
			}
			if (null != this.lobbySound)
			{
				this.lobbySound.Pause();
			}
		}
		else
		{
			if (null != this.gameETCSound)
			{
				this.gameETCSound.Play();
			}
			if (null != this.gameSound)
			{
				this.gameSound.Play();
			}
			if (null != this.lobbySound)
			{
				this.lobbySound.Play();
			}
		}
	}
}
