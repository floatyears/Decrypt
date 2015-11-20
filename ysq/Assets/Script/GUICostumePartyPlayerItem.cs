using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUICostumePartyPlayerItem : MonoBehaviour
{
	public enum ECPPS
	{
		ECPPS_Std,
		ECPPS_NormalDance
	}

	private Color mHighColor = new Color(1f, 0.78039217f, 0.384313732f);

	private Color mNormalColor = Color.white;

	private GUICostumePartyScene mBaseScene;

	public GameObject mModel;

	private GameObject ui48;

	private GameObject ui48_1;

	private GameObject ui73;

	public UIActorController mActor;

	public CostumePartyGuest mGuest;

	private int UIID;

	private UILabel mName;

	private UISprite mStage;

	private bool canTouch = true;

	public GUICostumePartyPlayerItem.ECPPS Status;

	private ResourceEntity asyncEntity;

	private bool isCreating;

	private bool isClearing;

	private float timerRefresh;

	public bool CanTouch
	{
		get
		{
			return this.canTouch;
		}
		set
		{
			this.canTouch = value;
			if (!this.canTouch && this.mBaseScene.mSelectedPlayer == this)
			{
				this.mBaseScene.OnBGClick(null);
			}
		}
	}

	public void InitWithBaseScene(GUICostumePartyScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mModel = GameUITools.FindGameObject("Model", base.gameObject);
		this.ui48 = GameUITools.FindGameObject("ui48", this.mModel);
		this.ui48_1 = GameUITools.FindGameObject("ui48_1", this.mModel);
		this.ui73 = GameUITools.FindGameObject("ui73", this.mModel);
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mStage = GameUITools.FindUISprite("Stage", base.gameObject);
		this.mStage.enabled = false;
		this.ui48.SetActive(false);
		this.ui48_1.SetActive(false);
		this.ui73.SetActive(false);
	}

	public void RefreshStatus()
	{
		if (this.mGuest.Timestamp > Globals.Instance.Player.GetTimeStamp())
		{
			this.Status = GUICostumePartyPlayerItem.ECPPS.ECPPS_NormalDance;
		}
		else
		{
			this.Status = GUICostumePartyPlayerItem.ECPPS.ECPPS_Std;
		}
		base.StartCoroutine(this.ChangeStatus());
	}

	[DebuggerHidden]
	private IEnumerator ChangeStatus()
	{
        return null;
        //GUICostumePartyPlayerItem.<ChangeStatus>c__Iterator78 <ChangeStatus>c__Iterator = new GUICostumePartyPlayerItem.<ChangeStatus>c__Iterator78();
        //<ChangeStatus>c__Iterator.<>f__this = this;
        //return <ChangeStatus>c__Iterator;
	}

	private void RefreshTxt()
	{
		if (this.mGuest != null && this.mGuest.PlayerID != 0uL)
		{
			if (Globals.Instance.Player.CostumePartySystem.IsMaster(this.mGuest.PlayerID))
			{
				this.mName.text = Singleton<StringManager>.Instance.GetString("costumePartyMaster", new object[]
				{
					this.mGuest.Name
				});
			}
			else
			{
				this.mName.text = this.mGuest.Name;
			}
			if (this.mGuest.PlayerID == Globals.Instance.Player.Data.ID)
			{
				this.mName.text = Singleton<StringManager>.Instance.GetString("costumePartySelf", new object[]
				{
					this.mName.text
				});
			}
		}
		else
		{
			this.mName.text = string.Empty;
		}
	}

	public void RefreshData(CostumePartyGuest guest)
	{
		this.mGuest = guest;
		this.RefreshTxt();
	}

	public void RefreshPlayer(CostumePartyGuest guest)
	{
		this.Clear(true);
		this.mGuest = guest;
		if (guest == null || guest.PlayerID == 0uL)
		{
			return;
		}
		this.isCreating = true;
		base.StartCoroutine(this.CreatePlayer(false));
	}

	public void PlayWand()
	{
		if (this.mGuest == null || this.mGuest.PlayerID == 0uL)
		{
			return;
		}
		if (this.UIID == this.mGuest.PetID && Globals.Instance.Player.GetTimeStamp() <= this.mGuest.PetTimestamp)
		{
			this.PlayWandEffect();
			return;
		}
		this.Clear(false);
		this.isCreating = true;
		base.StartCoroutine(this.CreatePlayer(true));
	}

	private void PlayWandEffect()
	{
		NGUITools.SetActive(this.ui73, false);
		NGUITools.SetActive(this.ui73, true);
	}

	[DebuggerHidden]
	private IEnumerator CreatePlayer(bool showEffect = false)
	{
        return null;
        //GUICostumePartyPlayerItem.<CreatePlayer>c__Iterator79 <CreatePlayer>c__Iterator = new GUICostumePartyPlayerItem.<CreatePlayer>c__Iterator79();
        //<CreatePlayer>c__Iterator.showEffect = showEffect;
        //<CreatePlayer>c__Iterator.<$>showEffect = showEffect;
        //<CreatePlayer>c__Iterator.<>f__this = this;
        //return <CreatePlayer>c__Iterator;
	}

	public void PleaseLeave()
	{
		if (this.mGuest != null)
		{
			Globals.Instance.Player.CostumePartySystem.LeaveParty(this.mGuest.PlayerID);
		}
		this.mGuest = null;
		this.Clear(true);
		this.RefreshTxt();
		this.CloseLight();
	}

	public void Clear(bool clearData)
	{
		this.isClearing = true;
		base.StartCoroutine(this.ClearModel(clearData));
	}

	[DebuggerHidden]
	private IEnumerator ClearModel(bool clearData)
	{
        return null;
        //GUICostumePartyPlayerItem.<ClearModel>c__Iterator7A <ClearModel>c__Iterator7A = new GUICostumePartyPlayerItem.<ClearModel>c__Iterator7A();
        //<ClearModel>c__Iterator7A.clearData = clearData;
        //<ClearModel>c__Iterator7A.<$>clearData = clearData;
        //<ClearModel>c__Iterator7A.<>f__this = this;
        //return <ClearModel>c__Iterator7A;
	}

	private void ClearData()
	{
		this.mName.text = string.Empty;
		this.mGuest = null;
	}

	public void ClearAsyncEntity()
	{
		if (this.asyncEntity != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntity);
			this.asyncEntity = null;
		}
	}

	private void OnClick()
	{
		if (this.mActor == null)
		{
			return;
		}
		if (!this.CanTouch)
		{
			return;
		}
		if (this != this.mBaseScene.mCurPlayer)
		{
			this.mBaseScene.OnPlayerClick(this);
		}
		this.Hi();
	}

	public void Dance()
	{
		if (this.mActor != null)
		{
			this.mActor.transform.localRotation = Quaternion.Euler(0f, -180f, 0f);
			this.mActor.PlayDanceAnimation(false);
		}
	}

	public void StartCarnival()
	{
		this.RefreshTxt();
		this.Dance();
		this.OpenLight();
	}

	private void OpenLight()
	{
		if (Globals.Instance.Player.CostumePartySystem.IsMaster(this.mGuest.PlayerID))
		{
			this.ui48.SetActive(false);
			this.ui48_1.SetActive(true);
			this.mStage.color = this.mHighColor;
		}
		else
		{
			this.ui48.SetActive(true);
			this.ui48_1.SetActive(false);
			this.mStage.color = this.mNormalColor;
		}
		this.mStage.enabled = true;
	}

	private void CloseLight()
	{
		this.mStage.enabled = false;
		this.ui48.SetActive(false);
		this.ui48_1.SetActive(false);
	}

	public void OverCarnival()
	{
		this.RefreshTxt();
		if (this.mActor != null && this.CanTouch)
		{
			this.mActor.PlayStdAnimation();
		}
		this.CloseLight();
	}

	public void Hi()
	{
		this.mActor.PlayHiAnimation();
	}

	public void DanceOver()
	{
		this.CanTouch = true;
		this.mActor.transform.localRotation = Quaternion.Euler(0f, -180f, 0f);
		this.RefreshStatus();
	}

	private void Update()
	{
		if (this.mGuest == null || this.mGuest.PlayerID == 0uL || !this.mBaseScene.PostLoadGUIDone)
		{
			return;
		}
		if (Time.time - this.timerRefresh <= 1f)
		{
			return;
		}
		this.timerRefresh = Time.time;
		if (Globals.Instance.Player.GetTimeStamp() > this.mGuest.Timestamp)
		{
			GUICostumePartyPlayerItem.ECPPS status = this.Status;
			if (status != GUICostumePartyPlayerItem.ECPPS.ECPPS_Std)
			{
				if (status == GUICostumePartyPlayerItem.ECPPS.ECPPS_NormalDance)
				{
					this.Status = GUICostumePartyPlayerItem.ECPPS.ECPPS_Std;
					this.OverCarnival();
					if (Globals.Instance.Player.Data.ID == this.mGuest.PlayerID)
					{
						this.mBaseScene.RefreshGetReward();
						if (!this.mBaseScene.CanChangeSong())
						{
							this.mBaseScene.PlayBGMusic();
						}
					}
				}
			}
		}
		if (this.UIID != 0 && this.UIID != this.mGuest.FashionID && Globals.Instance.Player.GetTimeStamp() > this.mGuest.PetTimestamp)
		{
			this.PlayWand();
		}
	}
}
