using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIRewardDartInfo : MonoBehaviour
{
	private class RandomPosRota
	{
		public Vector3 pos;

		public Quaternion rota;

		public void Init(Vector3 pos, Quaternion rota)
		{
			this.pos = pos;
			this.rota = rota;
		}
	}

	private GUIReward mBaseScene;

	private GUIRewardCheckBtn mCheckBtn;

	private UILabel mGetCount;

	private GameObject mDial;

	private UILabel[] mNums = new UILabel[6];

	private UILabel mRemainingTime;

	private UILabel mCurCost;

	private UILabel mMaxGet;

	private UIButton mPlayBtn;

	private UILabel mTimes;

	private GameObject mDart;

	private GameObject mui42;

	private float timerRefresh;

	private int overTime = -1;

	private int retentionTime;

	private int cost;

	private int times;

	private MS2C_StartDart startReply;

	private bool isCreating;

	private ResourceEntity asyncEntiry;

	private UIActorController mActor;

	private GameObject mFire;

	private GameObject mExplode;

	private bool rotateOver;

	private List<GUIRewardDartInfo.RandomPosRota> randomPosRota = new List<GUIRewardDartInfo.RandomPosRota>();

	private int time;

	public static bool IsVisible
	{
		get
		{
			return (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(22)) && GUIRewardDartInfo.Status;
		}
	}

	public static bool Status
	{
		get
		{
			return (Globals.Instance.Player.Data.DataFlag & 128) != 0;
		}
	}

	public bool IsOpen
	{
		get
		{
			return this.overTime - Globals.Instance.Player.GetTimeStamp() > 0;
		}
	}

	public void InitWithBaseScene(GUIReward baseScene, GUIRewardCheckBtn btn)
	{
		this.mBaseScene = baseScene;
		this.mCheckBtn = btn;
		this.CreateObjects();
	}

	protected void CreateObjects()
	{
		this.mGetCount = GameUITools.FindUILabel("GetCount", base.gameObject);
		this.mDial = GameUITools.FindGameObject("Dial", base.gameObject);
		GameObject gameObject = GameUITools.FindGameObject("Nums", this.mDial);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			this.mNums[i] = GameUITools.FindUILabel(i.ToString(), gameObject);
			this.mNums[i].text = string.Empty;
		}
		gameObject = GameUITools.FindGameObject("Infos", base.gameObject);
		this.mRemainingTime = GameUITools.FindUILabel("RemainingTime/Label", gameObject);
		this.mCurCost = GameUITools.FindUILabel("CurCost", gameObject);
		this.mMaxGet = GameUITools.FindUILabel("MaxGet", gameObject);
		this.mPlayBtn = GameUITools.FindGameObject("PlayBtn", gameObject).GetComponent<UIButton>();
		this.mTimes = GameUITools.FindUILabel("Times", gameObject);
		this.mDart = GameUITools.FindGameObject("Dart", base.gameObject);
		this.mui42 = GameUITools.FindGameObject("ui42", base.gameObject);
		this.mui42.SetActive(false);
		Tools.SetParticleRQWithUIScale(this.mui42, 4400);
		this.mGetCount.text = string.Empty;
		this.mRemainingTime.text = string.Empty;
		this.mCurCost.text = string.Empty;
		this.mMaxGet.text = string.Empty;
		this.mTimes.text = string.Empty;
		UIEventListener expr_193 = UIEventListener.Get(this.mPlayBtn.gameObject);
		expr_193.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_193.onClick, new UIEventListener.VoidDelegate(this.OnPlayBtnClick));
		GUIRewardDartInfo.RandomPosRota randomPosRota = new GUIRewardDartInfo.RandomPosRota();
		randomPosRota.Init(new Vector3(-8f, -67f, 0f), Quaternion.Euler(0f, 0f, 30f));
		this.randomPosRota.Add(randomPosRota);
		randomPosRota = new GUIRewardDartInfo.RandomPosRota();
		randomPosRota.Init(new Vector3(-38f, 10f, 0f), Quaternion.Euler(0f, 0f, 30f));
		this.randomPosRota.Add(randomPosRota);
		randomPosRota = new GUIRewardDartInfo.RandomPosRota();
		randomPosRota.Init(new Vector3(35f, -46f, 0f), Quaternion.Euler(0f, 0f, 126f));
		this.randomPosRota.Add(randomPosRota);
		randomPosRota = new GUIRewardDartInfo.RandomPosRota();
		randomPosRota.Init(new Vector3(-15f, -29f, 0f), Quaternion.Euler(0f, 0f, 82f));
		this.randomPosRota.Add(randomPosRota);
		GameObject original = Res.Load<GameObject>("Skill/pet/pet_0034_3c", false);
		this.mExplode = (GameObject)UnityEngine.Object.Instantiate(original);
		this.mExplode.SetActive(false);
		Tools.SetParticleRQWithUIScale(this.mExplode, 4400);
		GameUITools.AddChild(this.mDart, this.mExplode);
		NGUITools.SetChildLayer(this.mExplode.transform, this.mExplode.layer);
	}

	public void OnMsgGetDartData(MemoryStream stream)
	{
		MS2C_GetDartData mS2C_GetDartData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetDartData), stream) as MS2C_GetDartData;
		if (!GUIRewardDartInfo.IsVisible)
		{
			return;
		}
		if (mS2C_GetDartData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_GetDartData.Result);
			return;
		}
		if (mS2C_GetDartData.Count != 0)
		{
			if (mS2C_GetDartData.Data == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("Dart Data is null", new object[0])
				});
				return;
			}
			if (mS2C_GetDartData.Data.Count != 6)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("Dart Data Error, Count : {0}", mS2C_GetDartData.Data.Count)
				});
				return;
			}
		}
		this.mGetCount.text = Singleton<StringManager>.Instance.GetString("activityDartGetCount", new object[]
		{
			mS2C_GetDartData.Diamond
		});
		this.mTimes.text = Singleton<StringManager>.Instance.GetString("activityDartTimes", new object[]
		{
			mS2C_GetDartData.Count
		});
		this.mCurCost.text = Singleton<StringManager>.Instance.GetString("activityDartCurCost", new object[]
		{
			mS2C_GetDartData.Cost
		});
		this.times = mS2C_GetDartData.Count;
		this.cost = mS2C_GetDartData.Cost;
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			this.mNums[i].text = mS2C_GetDartData.Data[i].ToString();
			if (mS2C_GetDartData.Data[i] > num)
			{
				num = mS2C_GetDartData.Data[i];
			}
		}
		this.mMaxGet.text = Singleton<StringManager>.Instance.GetString("activityDartMaxGet", new object[]
		{
			num
		});
		this.overTime = mS2C_GetDartData.OverTime;
		this.retentionTime = mS2C_GetDartData.RetentionTime;
		if (this.IsOpen)
		{
			this.RefreshBtn(mS2C_GetDartData.Count > 0);
		}
		else
		{
			this.RefreshBtn(false);
		}
	}

	public void OnMsgStartDart(MemoryStream stream)
	{
		this.startReply = (Serializer.NonGeneric.Deserialize(typeof(MS2C_StartDart), stream) as MS2C_StartDart);
		if (this.startReply.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", this.startReply.Result);
			return;
		}
		GameUIManager.mInstance.GetTopGoods().UpdateUIGoods(TopGoods.EGoodsUIType.EGT_UIDiamond, Globals.Instance.Player.Data.Diamond - Convert.ToInt32(this.mNums[this.startReply.Slot].text));
		this.times = this.startReply.Count;
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		base.StartCoroutine(this.Shot());
	}

	public static bool CanTakePartIn()
	{
		return GUIRewardDartInfo.IsVisible && (Globals.Instance.Player.Data.RedFlag & 4) != 0;
	}

	public void Refresh()
	{
		this.mCheckBtn.IsShowMark = GUIRewardDartInfo.CanTakePartIn();
		NGUITools.SetActive(this.mui42, false);
		NGUITools.SetActive(this.mExplode, false);
		NGUITools.SetActive(this.mFire, false);
		if (GUIRewardDartInfo.IsVisible)
		{
			if (this.mActor == null && !this.isCreating)
			{
				this.CreateModel();
			}
			MC2S_GetDartData ojb = new MC2S_GetDartData();
			Globals.Instance.CliSession.Send(700, ojb);
		}
	}

	private void OnDestroy()
	{
		this.ClearModel();
	}

	private void CreateModel()
	{
		this.ClearModel();
		this.isCreating = true;
		this.asyncEntiry = ActorManager.CreateUIMonster(this.mBaseScene.ModelMonsterInfoID, 0, true, false, base.gameObject, 1f, delegate(GameObject go)
		{
			this.asyncEntiry = null;
			if (go == null)
			{
				global::Debug.Log(new object[]
				{
					"CreateUIMonster error"
				});
			}
			else
			{
				this.mActor = go.GetComponent<UIActorController>();
				this.mActor.transform.localPosition = this.mBaseScene.ModelStandPosition;
				this.mActor.transform.localScale = this.mBaseScene.ModelScale;
				this.isCreating = false;
				go.GetComponent<ParticleScaler>().renderQueue = 0;
				GameObject original = Res.Load<GameObject>("Skill/pet/pet_0034_3a", false);
				this.mFire = (GameObject)UnityEngine.Object.Instantiate(original);
				this.mFire.SetActive(false);
				Tools.SetParticleRQWithUIScale(this.mFire, 4400);
				GameUITools.AddChild(go, this.mFire);
				NGUITools.SetChildLayer(this.mFire.transform, this.mFire.layer);
			}
		});
	}

	private void ClearModel()
	{
		if (this.asyncEntiry != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntiry);
			this.asyncEntiry = null;
		}
		if (this.mActor != null)
		{
			UnityEngine.Object.DestroyImmediate(this.mActor.gameObject);
			this.mActor = null;
		}
	}

	private void OnPlayBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!this.IsOpen)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityOverTip", 0f, 0f);
			this.Refresh();
			return;
		}
		if (this.times <= 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityDartOver", 0f, 0f);
			this.RefreshBtn(false);
			return;
		}
		if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.cost, 0))
		{
			GameUIManager.mInstance.GetTopGoods().DisableUpdate(TopGoods.EGoodsUIType.EGT_UIDiamond);
			MC2S_StartDart ojb = new MC2S_StartDart();
			Globals.Instance.CliSession.Send(702, ojb);
		}
	}

	private void OnDisable()
	{
		GameUIManager.mInstance.GetTopGoods().EnableUpdate(TopGoods.EGoodsUIType.EGT_UIDiamond);
	}

	private void CanShot()
	{
		this.rotateOver = true;
	}

	[DebuggerHidden]
	private IEnumerator Shot()
	{
        return null;
        //GUIRewardDartInfo.<Shot>c__Iterator6D <Shot>c__Iterator6D = new GUIRewardDartInfo.<Shot>c__Iterator6D();
        //<Shot>c__Iterator6D.<>f__this = this;
        //return <Shot>c__Iterator6D;
	}

	private void OnAnimEnd()
	{
		GameUIManager.mInstance.HideFadeBG(false);
		GameUIManager.mInstance.GetTopGoods().EnableUpdate(TopGoods.EGoodsUIType.EGT_UIDiamond);
		GameUIManager.mInstance.GetTopGoods().OnPlayerUpdateEvent();
		GUIRewardPanel.Show(new RewardData
		{
			RewardType = 2,
			RewardValue1 = Convert.ToInt32(this.mNums[this.startReply.Slot].text)
		}, null, false, true, null, false);
		this.RefreshTxt(null);
		NGUITools.SetActive(this.mui42, false);
		NGUITools.SetActive(this.mui42, true);
		this.mCheckBtn.IsShowMark = GUIRewardDartInfo.CanTakePartIn();
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_Shot, 0f);
	}

	private void RefreshTxt(object obj)
	{
		if (this.startReply.Count != 0)
		{
			if (this.startReply.Data == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("Dart Data is null", new object[0])
				});
				return;
			}
			if (this.startReply.Data.Count != 6)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("Dart Data Error, Count : {0}", this.startReply.Data.Count)
				});
				return;
			}
		}
		this.mGetCount.text = Singleton<StringManager>.Instance.GetString("activityDartGetCount", new object[]
		{
			this.startReply.Diamond
		});
		this.mTimes.text = Singleton<StringManager>.Instance.GetString("activityDartTimes", new object[]
		{
			this.startReply.Count
		});
		if (this.startReply.Count != 0)
		{
			this.mCurCost.text = Singleton<StringManager>.Instance.GetString("activityDartCurCost", new object[]
			{
				this.startReply.Cost
			});
			this.cost = this.startReply.Cost;
			int num = 0;
			for (int i = 0; i < 6; i++)
			{
				this.mNums[i].text = this.startReply.Data[i].ToString();
				if (this.startReply.Data[i] > num)
				{
					num = this.startReply.Data[i];
				}
			}
			this.mMaxGet.text = Singleton<StringManager>.Instance.GetString("activityDartMaxGet", new object[]
			{
				num
			});
		}
		this.mDart.gameObject.SetActive(false);
		if (this.IsOpen)
		{
			this.RefreshBtn(this.startReply.Count > 0);
		}
		else
		{
			this.RefreshBtn(false);
		}
	}

	private void RefreshBtn(bool enabled)
	{
		this.mPlayBtn.isEnabled = enabled;
		UIButton[] components = this.mPlayBtn.GetComponents<UIButton>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].SetState((!enabled) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
		}
	}

	private void Update()
	{
		if (base.gameObject.activeInHierarchy && this.mRemainingTime != null && this.overTime >= 0 && Time.time - this.timerRefresh > 1f)
		{
			this.timerRefresh = Time.time;
			this.RefreshRemainingTime();
		}
	}

	private void RefreshRemainingTime()
	{
		this.mRemainingTime.text = this.GetRemainingTime();
	}

	private string GetRemainingTime()
	{
		this.time = this.overTime - Globals.Instance.Player.GetTimeStamp();
		if (this.time > 0)
		{
			return UIEnergyTooltip.FormatTime(this.time);
		}
		if (this.retentionTime < Globals.Instance.Player.GetTimeStamp())
		{
			return Singleton<StringManager>.Instance.GetString("activityOver");
		}
		return Singleton<StringManager>.Instance.GetString("activityOver");
	}
}
