using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class EquipRefineLayer : MonoBehaviour
{
	public GUIEquipUpgradeScene mBaseScene;

	private CommonEquipInfoLayer mCommonEquipInfoLayer;

	private UILabel mLevel;

	private UISlider mExpProgressBar;

	private UISprite mBarFG;

	private UILabel mBarValue;

	private UILabel mLevelStartingValue;

	private UILabel mLevelNextValue;

	private GameObject mLevelEffect;

	private UILabel mPoint0;

	private UILabel mPoint0StartingValue;

	private UILabel mPoint0AddedValue;

	private UILabel mPoint1;

	private UILabel mPoint1StartingValue;

	private UILabel mPoint1AddedValue;

	private UILabel mLegendPoint;

	private GameObject ui52;

	private GameObject ui53;

	private List<EquipRefineExpItem> mItems = new List<EquipRefineExpItem>();

	private int oldRefineLevel;

	private int oldMasterLevel;

	private int mCurExp;

	private uint mMaxExp;

	private int mCurLevel;

	private int exp;

	private ItemDataEx mCurExpItem;

	private int count;

	public void InitWithBaseScene(GUIEquipUpgradeScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mCommonEquipInfoLayer = GameUITools.FindGameObject("CommonEquipInfoLayer", base.gameObject).AddComponent<CommonEquipInfoLayer>();
		this.ui52 = GameUITools.FindGameObject("ui52", base.gameObject);
		this.ui53 = GameUITools.FindGameObject("ui53", base.gameObject);
		Tools.SetParticleRQWithUIScale(this.ui52, 4500);
		Tools.SetParticleRQWithUIScale(this.ui53, 4500);
		this.ui52.gameObject.SetActive(false);
		this.ui53.gameObject.SetActive(false);
		GameObject gameObject = GameUITools.FindGameObject("RefineInfo/Info/RefineExp", base.gameObject);
		this.mLevel = GameUITools.FindUILabel("Level", gameObject);
		this.mExpProgressBar = GameUITools.FindGameObject("ExpProgressBar", gameObject).GetComponent<UISlider>();
		this.mBarFG = GameUITools.FindUISprite("FG", this.mExpProgressBar.gameObject);
		this.mBarValue = GameUITools.FindUILabel("Value", this.mExpProgressBar.gameObject);
		gameObject = GameUITools.FindGameObject("RefineLevel", gameObject.transform.parent.gameObject);
		this.mLevelStartingValue = GameUITools.FindUILabel("StartingValue", gameObject);
		this.mLevelNextValue = GameUITools.FindUILabel("NextValue", gameObject);
		this.mLevelEffect = GameUITools.FindGameObject("Effect", gameObject);
		this.mPoint0 = GameUITools.FindUILabel("Point0", gameObject.transform.parent.gameObject);
		this.mPoint0StartingValue = GameUITools.FindUILabel("StartingValue", this.mPoint0.gameObject);
		this.mPoint0AddedValue = GameUITools.FindUILabel("AddedValue", this.mPoint0.gameObject);
		this.mPoint1 = GameUITools.FindUILabel("Point1", gameObject.transform.parent.gameObject);
		this.mPoint1StartingValue = GameUITools.FindUILabel("StartingValue", this.mPoint1.gameObject);
		this.mPoint1AddedValue = GameUITools.FindUILabel("AddedValue", this.mPoint1.gameObject);
		this.mLegendPoint = GameUITools.FindUILabel("LegendPoint", gameObject.transform.parent.gameObject);
		gameObject = GameUITools.FindGameObject("Items", gameObject.transform.parent.parent.gameObject);
		List<ItemInfo> list = new List<ItemInfo>();
		foreach (ItemInfo current in Globals.Instance.AttDB.ItemDict.Values)
		{
			if (current.Type == 4 && current.SubType == 2)
			{
				list.Add(current);
			}
		}
		int num = 0;
		while (num < gameObject.transform.childCount && num < list.Count)
		{
			this.mItems.Add(gameObject.transform.GetChild(num).gameObject.AddComponent<EquipRefineExpItem>());
			this.mItems[num].InitWithBaseScene(this, list[num]);
			num++;
		}
	}

	public void Refresh(bool isInit = true)
	{
		if (HOTween.IsTweening(this.mExpProgressBar))
		{
			HOTween.Kill(this.mExpProgressBar);
		}
		NGUITools.SetActive(this.ui52, false);
		NGUITools.SetActive(this.ui53, false);
		this.mCommonEquipInfoLayer.Refresh(this.mBaseScene.mEquipData, false, true);
		this.mLevel.text = this.mBaseScene.mEquipData.GetEquipRefineLevel().ToString();
		this.mLevelStartingValue.text = this.mBaseScene.mEquipData.GetEquipRefineLevel().ToString();
		this.mPoint0.text = Tools.GetEquipAEStr((ESubTypeEquip)this.mBaseScene.mEquipData.Info.SubType);
		this.mPoint0StartingValue.text = this.mBaseScene.mEquipData.GetEquipRefineAttValue0().ToString();
		this.mPoint1.text = Tools.GetEquipARStr((ESubTypeEquip)this.mBaseScene.mEquipData.Info.SubType);
		this.mPoint1StartingValue.text = Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
		{
			this.mBaseScene.mEquipData.GetEquipRefineAttValue1().ToString("0.0")
		});
		LegendInfo legendInfo = Tools.GetLegendInfo(this.mBaseScene.mEquipData.Info);
		if (legendInfo != null)
		{
			this.mLegendPoint.enabled = true;
			this.mLegendPoint.text = Tools.GetNextLegendSkillStr(legendInfo, this.mBaseScene.mEquipData.GetEquipRefineLevel());
		}
		else
		{
			this.mLegendPoint.enabled = false;
		}
		if (this.mBaseScene.mEquipData.IsRefineMax())
		{
			this.mExpProgressBar.GetComponent<UISprite>().enabled = false;
			this.mBarFG.enabled = false;
			this.mBarValue.text = Singleton<StringManager>.Instance.GetString("equipImprove26");
			this.mBarValue.color = Color.red;
			this.mLevelNextValue.enabled = false;
			this.mLevelEffect.gameObject.SetActive(false);
			this.mPoint0AddedValue.enabled = false;
			this.mPoint1AddedValue.enabled = false;
			foreach (EquipRefineExpItem current in this.mItems)
			{
				current.gameObject.SetActive(false);
			}
		}
		else
		{
			if (isInit)
			{
				this.mMaxExp = this.mBaseScene.mEquipData.GetEquipRefineMaxExp();
				this.mExpProgressBar.value = ((this.mMaxExp != 0u) ? ((float)this.mBaseScene.mEquipData.GetEquipRefineExp() / this.mMaxExp) : 1f);
				this.mCurExp = this.mBaseScene.mEquipData.GetEquipRefineExp();
				this.mCurLevel = this.mBaseScene.mEquipData.GetEquipRefineLevel();
				this.mBarValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
				{
					this.mBaseScene.mEquipData.GetEquipRefineExp(),
					this.mMaxExp
				});
			}
			this.mLevelNextValue.text = (this.mBaseScene.mEquipData.GetEquipRefineLevel() + 1).ToString();
			this.mPoint0AddedValue.text = Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
			{
				this.mBaseScene.mEquipData.GetEquipRefineAttDelta0()
			});
			this.mPoint1AddedValue.text = Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
			{
				Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
				{
					this.mBaseScene.mEquipData.GetEquipRefineAttDelta1().ToString("0.0")
				})
			});
			foreach (EquipRefineExpItem current2 in this.mItems)
			{
				current2.Refresh();
			}
		}
		this.mCurExpItem = null;
		this.exp = 0;
		this.count = 0;
	}

	public void SendRefine2Server(ulong itemID, int count)
	{
		MC2S_EquipRefine mC2S_EquipRefine = new MC2S_EquipRefine();
		mC2S_EquipRefine.ItemID = itemID;
		mC2S_EquipRefine.Count = count;
		mC2S_EquipRefine.EquipID = this.mBaseScene.mEquipData.GetID();
		Globals.Instance.CliSession.Send(522, mC2S_EquipRefine);
		this.oldRefineLevel = this.mBaseScene.mEquipData.GetEquipRefineLevel();
		if (this.mBaseScene.mSocketData != null)
		{
			this.oldMasterLevel = this.mBaseScene.mSocketData.EquipMasterRefineLevel;
		}
	}

	public void OnMsgEquipRefine(MemoryStream stream)
	{
		MS2C_EquipRefine mS2C_EquipRefine = Serializer.NonGeneric.Deserialize(typeof(MS2C_EquipRefine), stream) as MS2C_EquipRefine;
		if (mS2C_EquipRefine.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_EquipRefine.Result);
			return;
		}
		this.Refresh(true);
		this.mCurExpItem = null;
		this.count = 0;
		this.exp = 0;
		base.StartCoroutine(this.EnableAdd());
		if (this.oldRefineLevel < this.mBaseScene.mEquipData.GetEquipRefineLevel())
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
			{
				Tools.GetEquipAEStr((ESubTypeEquip)this.mBaseScene.mEquipData.Info.SubType),
				this.mBaseScene.mEquipData.GetEquipRefineAttDelta0()
			}));
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
			{
				Tools.GetEquipARStr((ESubTypeEquip)this.mBaseScene.mEquipData.Info.SubType),
				Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
				{
					this.mBaseScene.mEquipData.GetEquipRefineAttDelta1().ToString("0.0")
				})
			}));
			if (this.mBaseScene.mSocketData != null && this.mBaseScene.mSocketData.EquipMasterRefineLevel > this.oldMasterLevel)
			{
				GUIUpgradeTipPopUp.ShowThis(Singleton<StringManager>.Instance.GetString("equipImprove65", new object[]
				{
					this.mBaseScene.mEquipData.GetEquipRefineLevel()
				}), stringBuilder.ToString().TrimEnd(new char[0]), Singleton<StringManager>.Instance.GetString("equipImprove56", new object[]
				{
					this.mBaseScene.mSocketData.EquipMasterRefineLevel
				}), Master.GetMasterDiffValueStr(this.oldMasterLevel, this.mBaseScene.mSocketData.EquipMasterRefineLevel, Master.EMT.EMT_EquipRefine), 5f, 1f);
			}
			else
			{
				GUIUpgradeTipPopUp.ShowThis(Singleton<StringManager>.Instance.GetString("equipImprove65", new object[]
				{
					this.mBaseScene.mEquipData.GetEquipRefineLevel()
				}), stringBuilder.ToString().TrimEnd(new char[0]), string.Empty, string.Empty, 5f, 1f);
			}
			NGUITools.SetActive(this.ui53, false);
			NGUITools.SetActive(this.ui53, true);
			Globals.Instance.EffectSoundMgr.Play("ui/ui_020b");
		}
		else
		{
			NGUITools.SetActive(this.ui52, false);
			NGUITools.SetActive(this.ui52, true);
			Globals.Instance.EffectSoundMgr.Play("ui/ui_020a");
		}
	}

	[DebuggerHidden]
	private IEnumerator EnableAdd()
	{
        return null;
        //EquipRefineLayer.<EnableAdd>c__Iterator45 <EnableAdd>c__Iterator = new EquipRefineLayer.<EnableAdd>c__Iterator45();
        //<EnableAdd>c__Iterator.<>f__this = this;
        //return <EnableAdd>c__Iterator;
	}

	private void EnableAddExp(bool enable)
	{
		foreach (EquipRefineExpItem current in this.mItems)
		{
			current.EnableCollider(enable);
		}
	}

	public void PlayUISliderEffect(int addExp)
	{
		if (HOTween.IsTweening(this.mExpProgressBar))
		{
			HOTween.Complete(this.mExpProgressBar);
			HOTween.Kill(this.mExpProgressBar);
		}
		this.mCurExp += addExp;
		if ((long)this.mCurExp >= (long)((ulong)this.mMaxExp))
		{
			HOTween.To(this.mExpProgressBar, 0.15f, new TweenParms().Prop("value", 1f).SpeedBased(false).OnUpdate(new TweenDelegate.TweenCallback(this.OnSliderUpdate)).OnComplete(new TweenDelegate.TweenCallback(this.OnSliderComplete)));
			this.mCurExp -= (int)this.mMaxExp;
			this.mCurLevel++;
		}
		else
		{
			HOTween.To(this.mExpProgressBar, 0.15f, new TweenParms().Prop("value", (float)this.mCurExp / ((this.mMaxExp == 0u) ? 1u : this.mMaxExp)).SpeedBased(false).OnUpdate(new TweenDelegate.TweenCallback(this.OnSliderUpdate)).OnComplete(new TweenDelegate.TweenCallback(this.OnSliderEnd)));
		}
	}

	private void OnSliderUpdate()
	{
		this.mBarValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
		{
			Mathf.Floor(this.mExpProgressBar.value * this.mMaxExp),
			this.mMaxExp
		});
	}

	private void OnSliderComplete()
	{
		LevelInfo info = Globals.Instance.AttDB.LevelDict.GetInfo(this.mCurLevel + 1);
		if (info != null && this.mBaseScene.mEquipData.Info.Quality >= 0 && this.mBaseScene.mEquipData.Info.Quality < info.RefineExp.Count)
		{
			this.mMaxExp = info.RefineExp[this.mBaseScene.mEquipData.Info.Quality];
			this.mExpProgressBar.value = 0f;
			if ((long)this.mCurExp >= (long)((ulong)this.mMaxExp))
			{
				HOTween.To(this.mExpProgressBar, 0.15f, new TweenParms().Prop("value", 1f).SpeedBased(false).OnUpdate(new TweenDelegate.TweenCallback(this.OnSliderUpdate)).OnComplete(new TweenDelegate.TweenCallback(this.OnSliderComplete)));
				this.mCurExp -= (int)this.mMaxExp;
				this.mCurLevel++;
			}
			else
			{
				HOTween.To(this.mExpProgressBar, 0.15f, new TweenParms().Prop("value", (float)this.mCurExp / ((this.mMaxExp == 0u) ? 1u : this.mMaxExp)).SpeedBased(false).OnUpdate(new TweenDelegate.TweenCallback(this.OnSliderUpdate)).OnComplete(new TweenDelegate.TweenCallback(this.OnSliderEnd)));
			}
			return;
		}
		global::Debug.LogError(new object[]
		{
			"Get Equip Refine Max Exp Error , Info ID : {0} , Level : {1} ",
			this.mBaseScene.mEquipData.Info.ID,
			this.mCurLevel + 1
		});
	}

	private void OnSliderEnd()
	{
		this.mBarValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
		{
			this.mCurExp,
			this.mMaxExp
		});
	}

	private void Refine()
	{
		if (this.mCurExpItem != null && this.count > 0)
		{
			this.SendRefine2Server(this.mCurExpItem.GetID(), this.count);
			this.EnableAddExp(false);
		}
	}

	public void AddExp(ItemDataEx data, bool refine = false)
	{
		if (data == null && refine)
		{
			this.Refine();
			return;
		}
		this.mCurExpItem = data;
		this.count++;
		this.exp += data.Info.Value1;
		this.PlayUISliderEffect(data.Info.Value1);
		if (refine || this.exp >= this.mBaseScene.mEquipData.GetEquipRefineExp2Upgrade() || data.GetCount() <= this.count)
		{
			this.Refine();
		}
	}
}
