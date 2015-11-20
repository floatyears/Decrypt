using Att;
using Holoville.HOTween.Core;
using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUICostumePartyStartPopUp : MonoBehaviour
{
	private class TimeBtn : MonoBehaviour
	{
		public delegate void ChangeCallBack(int index);

		public static GUICostumePartyStartPopUp.TimeBtn mCur;

		public GUICostumePartyStartPopUp.TimeBtn.ChangeCallBack ChangeEvent;

		private UISprite mSprite;

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
						if (GUICostumePartyStartPopUp.TimeBtn.mCur != null && GUICostumePartyStartPopUp.TimeBtn.mCur != this)
						{
							GUICostumePartyStartPopUp.TimeBtn.mCur.IsCheck = false;
						}
						GUICostumePartyStartPopUp.TimeBtn.mCur = this;
					}
					this.mSprite.spriteName = ((!this.check) ? "btnBg1" : "btnBg2");
					if (this.check && this.ChangeEvent != null)
					{
						this.ChangeEvent(this.index);
					}
				}
			}
		}

		public GUICostumePartyStartPopUp.TimeBtn Init(int index, GUICostumePartyStartPopUp.TimeBtn.ChangeCallBack cb)
		{
			this.mSprite = base.gameObject.GetComponent<UISprite>();
			this.index = index;
			this.ChangeEvent = cb;
			return this;
		}

		private void OnClick()
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
			this.IsCheck = true;
		}
	}

	private GameObject mBG;

	private UITable mContents;

	private List<GUICostumePartyStartPopUp.TimeBtn> mTimeBtns = new List<GUICostumePartyStartPopUp.TimeBtn>();

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBG = GameUITools.FindGameObject("BG", base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mBG);
		this.mContents = GameUITools.FindGameObject("GetRewardItems/Panel/Contents", this.mBG).GetComponent<UITable>();
		GameUITools.RegisterClickEvent("StartBtn", new UIEventListener.VoidDelegate(this.OnStartBtnClick), this.mBG);
		this.mTimeBtns.Add(GameUITools.FindGameObject("0", this.mBG).AddComponent<GUICostumePartyStartPopUp.TimeBtn>().Init(0, new GUICostumePartyStartPopUp.TimeBtn.ChangeCallBack(this.RefreshItem)));
		this.mTimeBtns.Add(GameUITools.FindGameObject("1", this.mBG).AddComponent<GUICostumePartyStartPopUp.TimeBtn>().Init(1, new GUICostumePartyStartPopUp.TimeBtn.ChangeCallBack(this.RefreshItem)));
		this.mTimeBtns.Add(GameUITools.FindGameObject("2", this.mBG).AddComponent<GUICostumePartyStartPopUp.TimeBtn>().Init(2, new GUICostumePartyStartPopUp.TimeBtn.ChangeCallBack(this.RefreshItem)));
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public void Open()
	{
		base.gameObject.SetActive(true);
		if (GUICostumePartyStartPopUp.TimeBtn.mCur != this.mTimeBtns[0])
		{
			this.mTimeBtns[0].IsCheck = true;
		}
		GameUITools.PlayOpenWindowAnim(this.mBG.transform, null, true);
	}

	public void Close()
	{
		GameUITools.PlayCloseWindowAnim(this.mBG.transform, new TweenDelegate.TweenCallback(this.Hide), true);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.Close();
	}

	private void OnStartBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_StartCarnival mC2S_StartCarnival = new MC2S_StartCarnival();
		mC2S_StartCarnival.CarnivalType = GUICostumePartyStartPopUp.TimeBtn.mCur.index + 1;
		Globals.Instance.CliSession.Send(272, mC2S_StartCarnival);
	}

	private void RefreshItem(int index)
	{
		foreach (Transform current in this.mContents.GetChildList())
		{
			UnityEngine.Object.Destroy(current.gameObject);
		}
		this.mContents.GetChildList().Clear();
		CostumePartyInfo info = Globals.Instance.AttDB.CostumePartyDict.GetInfo(index + 1);
		if (info == null)
		{
			global::Debug.LogErrorFormat("CostumePartyDict get info error , ID : {0}", new object[]
			{
				index + 1
			});
			return;
		}
		CostumePartyInfo info2 = Globals.Instance.AttDB.CostumePartyDict.GetInfo((int)(Globals.Instance.Player.Data.Level / 10u));
		if (info2 == null)
		{
			global::Debug.LogErrorFormat("CostumePartyDict get info error , ID : {0}", new object[]
			{
				Globals.Instance.Player.Data.Level / 10u
			});
			return;
		}
		GameObject gameObject = GameUITools.CreateReward(1, info2.Money * info.Time / 4, 0, this.mContents.transform, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
		gameObject.AddComponent<UIDragScrollView>();
		gameObject.transform.localScale = new Vector3(0.55f, 0.55f, 1f);
		int num = 0;
		while (num < info.ItemID.Count && num < info.ItemCount.Count)
		{
			gameObject = GameUITools.CreateReward(3, info.ItemID[num], info.ItemCount[num], this.mContents.transform, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
			gameObject.AddComponent<UIDragScrollView>();
			gameObject.transform.localScale = new Vector3(0.55f, 0.55f, 1f);
			num++;
		}
		this.mContents.repositionNow = true;
	}
}
