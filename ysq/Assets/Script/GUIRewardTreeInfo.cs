using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIRewardTreeInfo : MonoBehaviour
{
	private GUIRewardCheckBtn mCheckBtn;

	private UILabel mRemainingTime;

	private UILabel mNextDesc;

	private float timerRefresh;

	private int overTime = -1;

	private int retentionTime;

	private GameObject[] mUI78s = new GameObject[5];

	private GameObject[] mUI79s = new GameObject[5];

	private UISprite[] mApples = new UISprite[5];

	private UILabel mRollOneCost;

	private UISprite mRollOneIcon;

	private UISprite mRollOneItemIcon;

	private UILabel mRollTenCost;

	private UILabel mCurrencyValue;

	[NonSerialized]
	public Color32 darkColor = new Color32(255, 255, 255, 255);

	private bool isTen;

	private ActivityRollEquipData mData;

	private List<RewardData> rewardDatas;

	private int time;

	public AnimationCurve RollOneAnim;

	[NonSerialized]
	public float RollOneTime = 2f;

	[NonSerialized]
	public int AveragePoint = 6;

	[NonSerialized]
	public float LightTime = 0.2f;

	private int rollOneIndex;

	private int rollOneResult;

	private bool rollingOne;

	private bool rollingTen;

	private float rollTimer;

	private float animTimer;

	[NonSerialized]
	public float RollTenTime = 1.5f;

	[NonSerialized]
	public float RollTenFrequency = 0.35f;

	private List<int> darkApples;

	private int tempInt;

	private int rollOnePointCount;

	private List<int> appleIndexList = new List<int>();

	private int curAppleIndex;

	private bool isDouble;

	public static bool IsVisible
	{
		get
		{
			return Globals.Instance.Player.ActivitySystem.REData != null && Globals.Instance.Player.ActivitySystem.REData.Base.RewardTimeStamp > Globals.Instance.Player.GetTimeStamp();
		}
	}

	public bool IsOpen
	{
		get
		{
			return Globals.Instance.Player.ActivitySystem.REData != null && Globals.Instance.Player.ActivitySystem.REData.Base.CloseTimeStamp > Globals.Instance.Player.GetTimeStamp();
		}
	}

	public static bool CanTakePartIn()
	{
		return GUIRewardTreeInfo.IsVisible && Globals.Instance.Player.ItemSystem.GetEquipRollItemCount() >= 1;
	}

	public void InitWithBaseScene(GUIReward basescene, GUIRewardCheckBtn btn)
	{
		this.mCheckBtn = btn;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.RollOneAnim.keys[this.RollOneAnim.length - 1] = new Keyframe(1f, 1f);
		if (this.RollOneTime <= 0f)
		{
			this.RollOneTime = 2f;
		}
		if (this.RollTenTime <= 0f)
		{
			this.RollTenTime = 2f;
		}
		if (this.RollTenFrequency < 0f || this.RollTenFrequency > 1f)
		{
			this.RollTenFrequency = 0.5f;
		}
		this.mRemainingTime = GameUITools.FindUILabel("RemainingTime", base.gameObject);
		this.mNextDesc = GameUITools.FindUILabel("NextDesc/Txt", base.gameObject);
		Transform transform = GameUITools.FindGameObject("Apples", base.gameObject).transform;
		int num = 0;
		while (num < 5 && num < transform.childCount)
		{
			this.mUI78s[num] = GameUITools.FindGameObject("ui78", transform.GetChild(num).gameObject);
			this.mUI79s[num] = GameUITools.FindGameObject("ui79", transform.GetChild(num).gameObject);
			Tools.SetParticleRQWithUIScale(this.mUI78s[num], 3100);
			Tools.SetParticleRQWithUIScale(this.mUI79s[num], 3100);
			this.mUI78s[num].gameObject.SetActive(false);
			this.mUI79s[num].gameObject.SetActive(false);
			this.mApples[num] = transform.GetChild(num).gameObject.GetComponent<UISprite>();
			this.mApples[num].color = this.darkColor;
			num++;
		}
		GameUITools.RegisterClickEvent("ViewBtn", new UIEventListener.VoidDelegate(this.OnViewClick), base.gameObject);
		GameUITools.RegisterClickEvent("Rules", new UIEventListener.VoidDelegate(this.OnRulesClick), base.gameObject);
		this.mRollOneCost = GameUITools.FindUILabel("Cost/Value", GameUITools.RegisterClickEvent("RollOne", new UIEventListener.VoidDelegate(this.OnRollOneClick), base.gameObject));
		this.mRollOneIcon = GameUITools.FindUISprite("Icon", this.mRollOneCost.transform.parent.gameObject);
		this.mRollOneItemIcon = GameUITools.FindUISprite("ItemIcon", this.mRollOneCost.transform.parent.gameObject);
		this.mRollTenCost = GameUITools.FindUILabel("Cost/Value", GameUITools.RegisterClickEvent("RollTen", new UIEventListener.VoidDelegate(this.OnRollTenClick), base.gameObject));
		this.mCurrencyValue = GameUITools.FindUILabel("Currency/Value", base.gameObject);
	}

	public void Refresh()
	{
		this.mRemainingTime.enabled = false;
		this.mNextDesc.transform.parent.gameObject.SetActive(false);
		for (int i = 0; i < 5; i++)
		{
			this.mUI78s[i].gameObject.SetActive(false);
			this.mUI79s[i].gameObject.SetActive(false);
		}
		if (GUIRewardTreeInfo.IsVisible)
		{
			this.mData = Globals.Instance.Player.ActivitySystem.REData;
			this.RefreshContent();
		}
	}

	public void RefreshContent()
	{
		this.mCheckBtn.IsShowMark = GUIRewardTreeInfo.CanTakePartIn();
		if (this.mData == null)
		{
			return;
		}
		this.overTime = this.mData.Base.CloseTimeStamp;
		this.retentionTime = this.mData.Base.RewardTimeStamp;
		int equipRollItemCount = Globals.Instance.Player.ItemSystem.GetEquipRollItemCount();
		this.mCurrencyValue.text = equipRollItemCount.ToString();
		if (equipRollItemCount >= 1)
		{
			this.mRollOneIcon.enabled = false;
			this.mRollOneItemIcon.enabled = true;
			this.mRollOneCost.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
			{
				1
			});
		}
		else
		{
			this.mRollOneIcon.enabled = true;
			this.mRollOneItemIcon.enabled = false;
			this.mRollOneCost.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
			{
				this.mData.OneCost
			});
			if (Globals.Instance.Player.Data.Diamond >= this.mData.OneCost)
			{
				this.mRollOneCost.color = Color.white;
			}
			else
			{
				this.mRollOneCost.color = Color.red;
			}
		}
		this.mRollTenCost.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
		{
			this.mData.TenCost
		});
		if (Globals.Instance.Player.Data.Diamond >= this.mData.TenCost)
		{
			this.mRollTenCost.color = Color.white;
		}
		else
		{
			this.mRollTenCost.color = Color.red;
		}
	}

	private void RefreshTime()
	{
		if (this.mData == null || this.mData.Base == null)
		{
			return;
		}
		this.time = this.mData.Base.CloseTimeStamp - Globals.Instance.Player.GetTimeStamp();
		if (this.time >= 0)
		{
			this.mRemainingTime.enabled = true;
			this.mRemainingTime.text = Singleton<StringManager>.Instance.GetString("activityRemainTime", new object[]
			{
				Tools.FormatTimeStr2(this.time, false, false)
			});
			this.time = this.mData.DoubleTimestamp - Globals.Instance.Player.GetTimeStamp();
			if (this.time >= 0)
			{
				this.mNextDesc.transform.parent.gameObject.SetActive(true);
				this.mNextDesc.text = Singleton<StringManager>.Instance.GetString("activityTreeNextTime", new object[]
				{
					Tools.ServerDateTimeFormat1(this.time)
				});
			}
			else
			{
				this.mNextDesc.transform.parent.gameObject.SetActive(false);
			}
		}
		else if (this.retentionTime < Globals.Instance.Player.GetTimeStamp())
		{
			this.mRemainingTime.text = Singleton<StringManager>.Instance.GetString("activityOver");
		}
		else
		{
			this.mRemainingTime.text = Singleton<StringManager>.Instance.GetString("activityOver");
		}
	}

	private void OnViewClick(GameObject go)
	{
		if (this.mData == null)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		List<RewardData> list = new List<RewardData>();
		for (int i = 0; i < this.mData.ItemID.Count; i++)
		{
			list.Add(new RewardData
			{
				RewardType = 3,
				RewardValue1 = this.mData.ItemID[i]
			});
		}
		ItemsBox.Show(list, null, false);
	}

	private void PlayUIFx(bool is79, int index)
	{
		if (index < 0 || index >= 5)
		{
			return;
		}
		if (is79)
		{
			NGUITools.SetActive(this.mUI79s[index], false);
			NGUITools.SetActive(this.mUI79s[index], true);
			this.mApples[index].color = Color.white;
		}
		else
		{
			this.mApples[index].color = Color.white;
			base.StartCoroutine(this.CloseColor(index, this.LightTime));
			NGUITools.SetActive(this.mUI78s[index], false);
			NGUITools.SetActive(this.mUI78s[index], true);
		}
	}

	[DebuggerHidden]
	private IEnumerator CloseColor(int index, float time)
	{
        return null;
        //GUIRewardTreeInfo.<CloseColor>c__Iterator30 <CloseColor>c__Iterator = new GUIRewardTreeInfo.<CloseColor>c__Iterator30();
        //<CloseColor>c__Iterator.time = time;
        //<CloseColor>c__Iterator.index = index;
        //<CloseColor>c__Iterator.<$>time = time;
        //<CloseColor>c__Iterator.<$>index = index;
        //<CloseColor>c__Iterator.<>f__this = this;
        //return <CloseColor>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator StopUI79FX(int index)
	{
        return null;
        //GUIRewardTreeInfo.<StopUI79FX>c__Iterator31 <StopUI79FX>c__Iterator = new GUIRewardTreeInfo.<StopUI79FX>c__Iterator31();
        //<StopUI79FX>c__Iterator.index = index;
        //<StopUI79FX>c__Iterator.<$>index = index;
        //<StopUI79FX>c__Iterator.<>f__this = this;
        //return <StopUI79FX>c__Iterator;
	}

	private void ShowRewards()
	{
		GameUIManager.mInstance.HideFadeBG(false);
		if (this.rewardDatas.Count > 10)
		{
			GUIRewardPanel.Show(this.rewardDatas, null, true, false, new GUIRewardPanel.VoidCallback(this.ShowLuckyReward), false);
			this.rewardDatas.RemoveRange(0, 10);
		}
		else if (this.isDouble)
		{
			GUIRewardPanel.Show(this.rewardDatas, null, true, false, new GUIRewardPanel.VoidCallback(this.TryCommend), false).SetAnimType(GUIRewardPanel.EAnimType.EAT_Double);
		}
		else
		{
			GUIRewardPanel.Show(this.rewardDatas, null, true, false, new GUIRewardPanel.VoidCallback(this.TryCommend), false);
		}
	}

	private void ShowLuckyReward()
	{
		if (this.rewardDatas != null && this.rewardDatas.Count > 0)
		{
			GUIRewardPanel.Show(this.rewardDatas, Singleton<StringManager>.Instance.GetString("luckyGetRewardLb"), true, false, new GUIRewardPanel.VoidCallback(this.TryCommend), false).SetAnimType(GUIRewardPanel.EAnimType.EAT_Give);
		}
	}

	private void TryCommend()
	{
		GameUIManager.mInstance.TryCommend(ECommentType.EComment_GoldEquipSet, 0f);
	}

	private int GetNextLightApple()
	{
		if (this.darkApples == null)
		{
			this.darkApples = new List<int>();
		}
		if (this.darkApples.Count == 0)
		{
			for (int i = 0; i < 5; i++)
			{
				this.darkApples.Add(i);
			}
		}
		int index = UnityEngine.Random.Range(0, this.darkApples.Count);
		this.tempInt = this.darkApples[index];
		this.darkApples.RemoveAt(index);
		return this.tempInt;
	}

	private void PlayRollTenAnim()
	{
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		this.animTimer = 0f;
		this.rollingTen = true;
	}

	private void PlayRollOneAnim(int resultIndex)
	{
		GameUIManager.mInstance.ShowFadeBG(5900, 3000);
		this.animTimer = 0f;
		this.rollOneResult = resultIndex;
		this.rollingOne = true;
		if (this.rollOneIndex > 5)
		{
			this.rollOneIndex -= 5;
		}
		if (resultIndex < this.rollOneIndex)
		{
			resultIndex += 5;
		}
		this.rollOnePointCount = (resultIndex - this.rollOneIndex + 1) % 5;
		int num = this.rollOnePointCount - this.AveragePoint % 5;
		if (Mathf.Abs(num) > 2)
		{
			if (num > 0)
			{
				this.rollOnePointCount += (this.AveragePoint / 5 - 1) * 5;
			}
			else
			{
				this.rollOnePointCount += (this.AveragePoint / 5 + 1) * 5;
			}
		}
		else
		{
			this.rollOnePointCount += this.AveragePoint / 5 * 5;
		}
		this.appleIndexList.Clear();
		int num2 = this.rollOneIndex;
		for (int i = 0; i < this.rollOnePointCount; i++)
		{
			this.appleIndexList.Add(num2++);
		}
		this.curAppleIndex = 0;
	}

	private void Update()
	{
		if (base.gameObject.activeInHierarchy && this.overTime >= 0 && Time.time - this.timerRefresh >= 1f)
		{
			this.timerRefresh = Time.time;
			this.RefreshTime();
		}
		if (base.gameObject.activeInHierarchy && this.RollOneAnim != null)
		{
			if (this.rollingOne && this.appleIndexList != null && this.appleIndexList.Count > 0)
			{
				this.animTimer += Time.deltaTime;
				if (this.curAppleIndex < this.appleIndexList.Count - 1)
				{
					if (this.RollOneAnim.Evaluate(Mathf.Clamp01(this.animTimer / this.RollOneTime)) * (float)this.rollOnePointCount > (float)(this.appleIndexList[this.curAppleIndex] - this.appleIndexList[0]))
					{
						this.PlayUIFx(false, this.appleIndexList[this.curAppleIndex++] % 5);
						this.rollOneIndex++;
						if (this.rollOneIndex >= 5)
						{
							this.rollOneIndex -= 5;
						}
					}
				}
				else if (this.animTimer >= this.RollOneTime)
				{
					this.rollOneIndex = this.rollOneResult;
					this.PlayUIFx(true, this.rollOneResult);
					this.rollingOne = false;
					base.StartCoroutine(this.StopUI79FX(this.rollOneResult));
				}
			}
			else if (this.rollingTen)
			{
				if (this.animTimer < this.RollTenTime)
				{
					this.animTimer += Time.deltaTime;
					if (UnityEngine.Random.Range(0f, 1f) <= this.RollTenFrequency)
					{
						this.PlayUIFx(false, this.GetNextLightApple());
					}
				}
				else
				{
					this.rollingTen = false;
					this.ShowRewards();
				}
			}
		}
	}

	private void OnRollOneClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.ItemSystem.GetEquipRollItemCount() < 1 && (this.mData == null || Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.mData.OneCost, 0)))
		{
			return;
		}
		this.SendRoll(false);
	}

	private void SendRoll(bool ten)
	{
		this.isTen = ten;
		MC2S_RollEquip mC2S_RollEquip = new MC2S_RollEquip();
		mC2S_RollEquip.Flag = ten;
		Globals.Instance.CliSession.Send(764, mC2S_RollEquip);
	}

	private void OnRollTenClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mData == null || Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.mData.TenCost, 0))
		{
			return;
		}
		this.SendRoll(true);
	}

	public void OnMsgRollEquip(MemoryStream stream)
	{
		if (this.mData == null)
		{
			return;
		}
		MS2C_RollEquip mS2C_RollEquip = Serializer.NonGeneric.Deserialize(typeof(MS2C_RollEquip), stream) as MS2C_RollEquip;
		if (mS2C_RollEquip.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_RollEquip.Result);
			return;
		}
		if (mS2C_RollEquip.DoubleTimestamp != 0)
		{
			if (mS2C_RollEquip.DoubleTimestamp == -1)
			{
				this.mData.DoubleTimestamp = 0;
			}
			else
			{
				this.mData.DoubleTimestamp = mS2C_RollEquip.DoubleTimestamp;
			}
		}
		if (mS2C_RollEquip.OneCost != 0)
		{
			this.mData.OneCost = mS2C_RollEquip.OneCost;
		}
		this.rewardDatas = mS2C_RollEquip.Data;
		this.isDouble = mS2C_RollEquip.DoubleReward;
		if (mS2C_RollEquip.DoubleReward)
		{
			this.rewardDatas.AddRange(mS2C_RollEquip.Data);
		}
		if (this.isTen)
		{
			this.PlayRollTenAnim();
		}
		else
		{
			this.PlayRollOneAnim(mS2C_RollEquip.Apple - 1);
		}
		this.RefreshContent();
	}

	private void OnRulesClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mData == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.mData.Base.Desc))
		{
			GameUIRuleInfoPopUp.Show(this.mData.Base.Name, Singleton<StringManager>.Instance.GetString("activityTreeRules"));
		}
		else
		{
			GameUIRuleInfoPopUp.Show(this.mData.Base.Name, this.mData.Base.Desc);
		}
	}
}
