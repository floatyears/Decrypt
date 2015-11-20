using Holoville.HOTween.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUICostumePartyMusicPopUp : MonoBehaviour
{
	private class SongBtn : MonoBehaviour
	{
		public delegate void ChangeCallBack(int index);

		public static GUICostumePartyMusicPopUp.SongBtn mCur;

		public GUICostumePartyMusicPopUp.SongBtn.ChangeCallBack ChangeEvent;

		private TweenRotation mTR;

		private UISprite mPlay;

		private UISprite mPause;

		public int index;

		private bool check;

		public bool IsCheck
		{
			get
			{
				return this.check;
			}
			set
			{
				if (this.check != value)
				{
					this.check = value;
					if (this.check)
					{
						if (GUICostumePartyMusicPopUp.SongBtn.mCur != null && GUICostumePartyMusicPopUp.SongBtn.mCur != this)
						{
							GUICostumePartyMusicPopUp.SongBtn.mCur.IsCheck = false;
						}
						GUICostumePartyMusicPopUp.SongBtn.mCur = this;
					}
					this.mTR.enabled = this.check;
					this.mPlay.enabled = !this.check;
					this.mPause.enabled = this.check;
					if (this.check && this.ChangeEvent != null)
					{
						this.ChangeEvent(this.index);
					}
				}
			}
		}

		public GUICostumePartyMusicPopUp.SongBtn Init(int index, GUICostumePartyMusicPopUp.SongBtn.ChangeCallBack cb)
		{
			this.index = index;
			this.ChangeEvent = cb;
			this.mPlay = GameUITools.FindUISprite("Play", base.gameObject);
			this.mPause = GameUITools.FindUISprite("Pause", base.gameObject);
			this.mTR = GameUITools.RegisterClickEvent("BG", new UIEventListener.VoidDelegate(this.OnBGClick), base.gameObject).GetComponent<TweenRotation>();
			this.mPlay.enabled = true;
			this.mPause.enabled = false;
			this.mTR.enabled = false;
			return this;
		}

		private void OnBGClick(GameObject go)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
			this.IsCheck = true;
		}
	}

	private GUICostumePartyScene mBaseScene;

	private GameObject mBG;

	private List<GUICostumePartyMusicPopUp.SongBtn> mSongBtns = new List<GUICostumePartyMusicPopUp.SongBtn>();

	private GameObject mOpen;

	private GameObject mClose;

	private Transform mBtn;

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBG = GameUITools.FindGameObject("BG", base.gameObject);
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		this.mSongBtns.Add(GameUITools.FindGameObject("0", this.mBG).AddComponent<GUICostumePartyMusicPopUp.SongBtn>().Init(0, new GUICostumePartyMusicPopUp.SongBtn.ChangeCallBack(this.PlayMusic)));
		this.mSongBtns.Add(GameUITools.FindGameObject("1", this.mBG).AddComponent<GUICostumePartyMusicPopUp.SongBtn>().Init(1, new GUICostumePartyMusicPopUp.SongBtn.ChangeCallBack(this.PlayMusic)));
		this.mSongBtns.Add(GameUITools.FindGameObject("2", this.mBG).AddComponent<GUICostumePartyMusicPopUp.SongBtn>().Init(2, new GUICostumePartyMusicPopUp.SongBtn.ChangeCallBack(this.PlayMusic)));
		GameObject parent = GameUITools.FindGameObject("Switch", this.mBG);
		this.mBtn = GameUITools.FindGameObject("btn", parent).transform;
		this.mOpen = GameUITools.RegisterClickEvent("open", new UIEventListener.VoidDelegate(this.OnSwitchClick), parent);
		this.mClose = GameUITools.RegisterClickEvent("close", new UIEventListener.VoidDelegate(this.OnSwitchClick), parent);
		this.Switch(GameSetting.Data.Music);
	}

	public void Init(GUICostumePartyScene basescene)
	{
		this.mBaseScene = basescene;
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.mBG.transform, new TweenDelegate.TweenCallback(this.Hide), true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public void Open()
	{
		base.gameObject.SetActive(true);
		if (GUICostumePartyMusicPopUp.SongBtn.mCur != this.mSongBtns[GameCache.Data.SongID])
		{
			this.mSongBtns[GameCache.Data.SongID].IsCheck = true;
		}
		GameUITools.PlayOpenWindowAnim(this.mBG.transform, null, true);
	}

	private void OnSwitchClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.Switch(!GameSetting.Data.Music);
	}

	private void Switch(bool value)
	{
		GameSetting.Data.Music = value;
		if (value)
		{
			this.mOpen.gameObject.SetActive(true);
			this.mClose.gameObject.SetActive(false);
			this.mBtn.localPosition = new Vector3(-43f, -2f, 0f);
			Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic(GameConst.COSTUMEPARTY_SONG_NAME[GameCache.Data.SongID], true);
		}
		else
		{
			this.mOpen.gameObject.SetActive(false);
			this.mClose.gameObject.SetActive(true);
			this.mBtn.localPosition = new Vector3(-124f, -2f, 0f);
			Globals.Instance.BackgroundMusicMgr.StopLobbySound();
		}
		GameSetting.UpdateNow = true;
	}

	private void PlayMusic(int index)
	{
		if (index >= 0 && index < GameConst.COSTUMEPARTY_SONG_NAME.Length)
		{
			GameCache.Data.SongID = index;
			GameCache.UpdateNow = true;
			if (this.mBaseScene.CanChangeSong())
			{
				Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic(GameConst.COSTUMEPARTY_SONG_NAME[index], true);
			}
		}
	}
}
