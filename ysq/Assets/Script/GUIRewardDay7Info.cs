using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIRewardDay7Info : MonoBehaviour
{
	private UISprite mInfoBg;

	private UILabel mLoginedLb;

	private GameObject[] mPos = new GameObject[5];

	private GameObject[] mItemGo = new GameObject[5];

	private UIToggle[] mLogins = new UIToggle[7];

	private UIGrid mLoginTable;

	private UIButton mTakeRewardBtn;

	private UILabel mTakeRewardBtnLb;

	private int mRewardIndex;

	private GameObject mSlot;

	private GameObject mModel;

	private UIActorController mUIActorController;

	private bool mIsAniming;

	private GetPetLayer getPetLayer;

	private ResourceEntity asyncEntiry;

	public bool IsAniming
	{
		get
		{
			return this.mIsAniming;
		}
		set
		{
			this.mIsAniming = value;
			if (this.mIsAniming)
			{
				GameUIManager.mInstance.ShowFadeBG(5900, 3000);
			}
			else
			{
				GameUIManager.mInstance.HideFadeBG(false);
			}
		}
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mInfoBg = base.transform.GetComponent<UISprite>();
		this.mSlot = base.transform.Find("slot").gameObject;
		Transform transform = base.transform.Find("leftBg");
		this.mLoginedLb = transform.Find("loginDays").GetComponent<UILabel>();
		uint num = Globals.Instance.Player.Data.OnlineDays;
		if (Globals.Instance.Player.GetTimeStamp() >= Globals.Instance.Player.Data.DayTimeStamp)
		{
			num += 1u;
		}
		GameObject gameObject = transform.Find("tabs").gameObject;
		this.mLoginTable = gameObject.GetComponent<UIGrid>();
		for (int i = 0; i < 7; i++)
		{
			this.mLogins[i] = gameObject.transform.Find(string.Format("tab{0}", i)).GetComponent<UIToggle>();
			EventDelegate.Add(this.mLogins[i].onChange, new EventDelegate.Callback(this.OnLoginCheckChanged));
			if (!Globals.Instance.Player.IsDay7RewardTaken(i + 1) && (long)(i + 1) <= (long)((ulong)num))
			{
				this.mLogins[i].transform.Find("new").gameObject.SetActive(true);
			}
			else
			{
				this.mLogins[i].transform.Find("new").gameObject.SetActive(false);
			}
		}
		Transform transform2 = base.transform.Find("Reward");
		for (int j = 0; j < 5; j++)
		{
			this.mPos[j] = transform2.Find(string.Format("bg/pos{0}", j)).gameObject;
		}
		this.mTakeRewardBtn = transform2.Find("bg/ToGet").GetComponent<UIButton>();
		UIEventListener expr_1EA = UIEventListener.Get(this.mTakeRewardBtn.gameObject);
		expr_1EA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1EA.onClick, new UIEventListener.VoidDelegate(this.OnTakeRewardClick));
		this.mTakeRewardBtnLb = this.mTakeRewardBtn.transform.Find("Label").GetComponent<UILabel>();
	}

	public void Show()
	{
		this.CreateModel();
		this.Refresh();
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public void OnLoginCheckChanged()
	{
		if (UIToggle.current.value)
		{
			uint num = Globals.Instance.Player.Data.OnlineDays;
			if (Globals.Instance.Player.GetTimeStamp() >= Globals.Instance.Player.Data.DayTimeStamp)
			{
				num += 1u;
			}
			for (int i = 0; i < 7; i++)
			{
				if (UIToggle.current == this.mLogins[i])
				{
					if (!Globals.Instance.Player.IsDay7RewardTaken(i + 1) && (long)(i + 1) <= (long)((ulong)num))
					{
						this.mTakeRewardBtn.isEnabled = true;
						this.mTakeRewardBtnLb.color = Color.white;
					}
					else
					{
						this.mTakeRewardBtn.isEnabled = false;
						this.mTakeRewardBtnLb.color = new Color(0.5019608f, 0.5019608f, 0.5019608f);
					}
					MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(i + 1);
					if (info != null)
					{
						for (int j = 0; j < 5; j++)
						{
							for (int k = 0; k < this.mPos[j].transform.childCount; k++)
							{
								Transform child = this.mPos[j].transform.GetChild(k);
								UnityEngine.Object.Destroy(child.gameObject);
								this.mItemGo[j] = null;
							}
						}
						int num2 = 0;
						while (num2 < 5 && num2 < info.Day7RewardType.Count)
						{
							if (info.Day7RewardType[num2] == 0)
							{
								break;
							}
							this.mItemGo[num2] = GameUITools.CreateReward(info.Day7RewardType[num2], info.Day7RewardValue1[num2], info.Day7RewardValue2[num2], this.mPos[num2].transform, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
							if (!(this.mItemGo[num2] == null))
							{
								if (info.Day7RewardType[num2] == 3 || info.Day7RewardType[num2] == 4)
								{
									this.mItemGo[num2].transform.localScale = new Vector3(0.8f, 0.8f, 1f);
								}
								else
								{
									this.mItemGo[num2].transform.localScale = new Vector3(0.85f, 0.85f, 1f);
								}
							}
							num2++;
						}
					}
					break;
				}
			}
		}
	}

	private void OnTakeRewardClick(GameObject go)
	{
		for (int i = 0; i < 7; i++)
		{
			if (this.mLogins[i].gameObject.activeInHierarchy && this.mLogins[i].value)
			{
				MC2S_TakeDay7Reward mC2S_TakeDay7Reward = new MC2S_TakeDay7Reward();
				mC2S_TakeDay7Reward.Index = i + 1;
				Globals.Instance.CliSession.Send(242, mC2S_TakeDay7Reward);
				break;
			}
		}
	}

	public void OnMsgTakeDay7Reward(MS2C_TakeDay7Reward reply)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_009");
		if (this.mUIActorController != null)
		{
			this.mUIActorController.Invoke("OnClick", 0.1f);
		}
		this.mTakeRewardBtn.isEnabled = false;
		this.mTakeRewardBtnLb.color = new Color(0.5019608f, 0.5019608f, 0.5019608f);
		this.mRewardIndex = reply.Index;
		this.DoReward();
		GameAnalytics.OnTakeDay7Reward(reply.Index);
	}

	private void DoReward()
	{
		Sequence sequence = new Sequence(new SequenceParms().UpdateType(UpdateType.TimeScaleIndependentUpdate).OnComplete(new TweenDelegate.TweenCallback(this.OnItemAnimEnd)));
		float p_duration = 0.2f;
		if (this.mItemGo[4] != null)
		{
			sequence.Append(HOTween.To(this.mItemGo[4].transform, p_duration, new TweenParms().Prop("localPosition", new Vector3(160f, 0f, 0f)).Prop("localScale", Vector3.zero)));
		}
		if (this.mItemGo[3] != null)
		{
			sequence.Append(HOTween.To(this.mItemGo[3].transform, p_duration, new TweenParms().Prop("localPosition", new Vector3(250f, 0f, 0f)).Prop("localScale", Vector3.zero)));
		}
		if (this.mItemGo[2] != null)
		{
			sequence.Append(HOTween.To(this.mItemGo[2].transform, p_duration, new TweenParms().Prop("localPosition", new Vector3(330f, 0f, 0f)).Prop("localScale", Vector3.zero)));
		}
		if (this.mItemGo[1] != null)
		{
			sequence.Append(HOTween.To(this.mItemGo[1].transform, p_duration, new TweenParms().Prop("localPosition", new Vector3(420f, 0f, 0f)).Prop("localScale", Vector3.zero)));
		}
		if (this.mItemGo[0] != null)
		{
			sequence.Append(HOTween.To(this.mItemGo[0].transform, p_duration, new TweenParms().Prop("localPosition", new Vector3(510f, 0f, 0f)).Prop("localScale", Vector3.zero)));
		}
		this.IsAniming = true;
		sequence.Play();
	}

	private void OnItemAnimEnd()
	{
		base.StartCoroutine(this.DoFeatureCardAnim());
		this.IsAniming = false;
		if (!GUIReward.ShouldShowDay7Btn())
		{
			GameUITools.FindUISprite("leftBg", base.gameObject).enabled = false;
			GameUITools.FindGameObject("Reward", base.gameObject).gameObject.SetActive(false);
		}
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_LoginReward, 2f);
	}

	[DebuggerHidden]
	private IEnumerator DoFeatureCardAnim()
	{
        return null;
        //GUIRewardDay7Info.<DoFeatureCardAnim>c__Iterator2E <DoFeatureCardAnim>c__Iterator2E = new GUIRewardDay7Info.<DoFeatureCardAnim>c__Iterator2E();
        //<DoFeatureCardAnim>c__Iterator2E.<>f__this = this;
        //return <DoFeatureCardAnim>c__Iterator2E;
	}

	private void ClearModel()
	{
		if (this.asyncEntiry != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntiry);
			this.asyncEntiry = null;
		}
		if (this.mModel != null)
		{
			this.mUIActorController = null;
			UnityEngine.Object.DestroyImmediate(this.mModel);
			this.mModel = null;
		}
	}

	private void OnDisable()
	{
		this.ClearModel();
	}

	private void CreateModel()
	{
		this.ClearModel();
		int id = 0;
		for (int i = 0; i < 7; i++)
		{
			MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(i + 1);
			if (info != null)
			{
				int num = 0;
				while (num < 5 && num < info.Day7RewardType.Count)
				{
					if (info.Day7RewardType[num] == 4)
					{
						id = info.Day7RewardValue1[num];
						break;
					}
					num++;
				}
			}
		}
		PetInfo info2 = Globals.Instance.AttDB.PetDict.GetInfo(id);
		if (info2 != null)
		{
			this.asyncEntiry = ActorManager.CreateUIPet(info2, this.mInfoBg.depth + 1, true, true, this.mSlot, 1f, 0, delegate(GameObject go)
			{
				this.asyncEntiry = null;
				this.mModel = go;
				this.mUIActorController = this.mModel.GetComponent<UIActorController>();
				if (this.mUIActorController == null)
				{
					this.mUIActorController = this.mModel.AddComponent<UIActorController>();
				}
				this.mUIActorController.Invoke("OnClick", 0.5f);
			});
		}
	}

	private void Refresh()
	{
		this.mLoginedLb.text = Singleton<StringManager>.Instance.GetString("7daysTxt1", new object[]
		{
			Globals.Instance.Player.Data.OnlineDays
		});
		for (int i = 0; i < 7; i++)
		{
			this.mLogins[i].gameObject.SetActive(!Globals.Instance.Player.IsDay7RewardTaken(i + 1));
		}
		for (int j = 0; j < 7; j++)
		{
			if (this.mLogins[j].gameObject.activeInHierarchy)
			{
				this.mLogins[j].value = true;
				break;
			}
		}
		this.mLoginTable.repositionNow = true;
	}
}
