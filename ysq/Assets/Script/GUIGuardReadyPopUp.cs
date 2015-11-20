using Att;
using Holoville.HOTween.Core;
using Proto;
using System;
using UnityEngine;

public class GUIGuardReadyPopUp : MonoBehaviour
{
	private class LevelItem : MonoBehaviour
	{
		public static GUIGuardReadyPopUp.LevelItem mCur;

		public EventDelegate.Callback OnCheckEvent;

		private UISprite mSprite;

		private UILabel mLabel;

		private Color32 defaultC = new Color32(248, 172, 66, 255);

		private Color32[] checkColors = new Color32[]
		{
			new Color32(191, 219, 29, 255),
			new Color32(255, 176, 43, 255),
			new Color32(255, 83, 36, 255)
		};

		private bool state;

		private int index;

		public bool State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (value != this.state)
				{
					this.state = value;
					if (this.state)
					{
						this.mSprite.spriteName = string.Format("{0}C", this.Index);
						this.mLabel.color = Color.white;
						this.mLabel.applyGradient = true;
						this.mLabel.gradientBottom = this.checkColors[this.Index];
					}
					else
					{
						this.InitStyle();
					}
					this.mSprite.MakePixelPerfect();
					if (this.state)
					{
						if (GUIGuardReadyPopUp.LevelItem.mCur != null && GUIGuardReadyPopUp.LevelItem.mCur != this)
						{
							GUIGuardReadyPopUp.LevelItem.mCur.State = false;
						}
						GUIGuardReadyPopUp.LevelItem.mCur = this;
						if (this.OnCheckEvent != null)
						{
							this.OnCheckEvent();
						}
					}
				}
			}
		}

		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		private void InitStyle()
		{
			this.mSprite.spriteName = this.Index.ToString();
			this.mLabel.color = this.defaultC;
			this.mLabel.applyGradient = false;
		}

		public void Init(int index, EventDelegate.Callback cb)
		{
			this.Index = index;
			this.OnCheckEvent = cb;
			this.mSprite = base.GetComponent<UISprite>();
			this.mLabel = GameUITools.FindUILabel("Label", base.gameObject);
			this.InitStyle();
		}

		private void OnClick()
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
			this.State = true;
		}
	}

	private class PendingPetFragment : MonoBehaviour
	{
		private UILabel mNum;

		private GameUIToolTip toolTip;

		public void Init()
		{
			this.mNum = GameUITools.FindUILabel("Num", base.gameObject);
			UIEventListener expr_21 = UIEventListener.Get(base.gameObject);
			expr_21.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_21.onPress, new UIEventListener.BoolDelegate(this.OnRewardPress));
		}

		public void Refresh(int min, int max)
		{
			this.mNum.text = ((min != max) ? Singleton<StringManager>.Instance.GetString("activityHotTimeV2Time", new object[]
			{
				min,
				max
			}) : min.ToString());
		}

		private void OnRewardPress(GameObject go, bool isPressed)
		{
			if (isPressed)
			{
				if (this.toolTip == null)
				{
					this.toolTip = GameUIToolTipManager.GetInstance().CreateBasicTooltip(go.transform, string.Empty, string.Empty);
				}
				string @string = Singleton<StringManager>.Instance.GetString("guard2");
				string string2 = Singleton<StringManager>.Instance.GetString("guard1");
				this.toolTip.Create(Tools.GetCameraRootParent(go.transform), @string, string2, 3);
				this.toolTip.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(36f, this.toolTip.transform.localPosition.y - 7f, -7000f));
				this.toolTip.EnableToolTip();
			}
			else if (this.toolTip != null)
			{
				this.toolTip.HideTipAnim();
			}
		}
	}

	private GUIGuardScene mBaseScene;

	private UIPanel mPanel;

	private GameObject mBG;

	private UILabel mTitle;

	private UILabel mTeamCombat;

	private UILabel mRecommendCombat;

	private GameObject mRewards;

	private GUIGuardReadyPopUp.PendingPetFragment mPending;

	private GameObject mFarmBtn;

	private UILabel mFarmCost;

	private GUIGuardReadyPopUp.LevelItem[] mLevelItems = new GUIGuardReadyPopUp.LevelItem[3];

	private int index;

	private MGInfo mMGInfo;

	public void Init(GUIGuardScene basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mPanel = base.gameObject.GetComponent<UIPanel>();
		this.mBG = GameUITools.FindGameObject("BG", base.gameObject);
		this.mTitle = GameUITools.FindUILabel("Title", this.mBG);
		this.mTeamCombat = GameUITools.FindUILabel("TeamCombat", this.mBG);
		this.mRecommendCombat = GameUITools.FindUILabel("RecommendCombat", this.mBG);
		GameUITools.FindUILabel("Times/Value", this.mBG).text = (GameConst.GetInt32(125) - Globals.Instance.Player.Data.MGCount).ToString();
		this.mRewards = GameUITools.FindGameObject("Rewards", this.mBG);
		this.mPending = GameUITools.FindGameObject("Pending", this.mBG).AddComponent<GUIGuardReadyPopUp.PendingPetFragment>();
		this.mPending.Init();
		GameObject gameObject = GameUITools.FindGameObject("Levels", this.mBG);
		int num = 0;
		while (num < 3 && num < gameObject.transform.childCount)
		{
			this.mLevelItems[num] = gameObject.transform.GetChild(num).gameObject.AddComponent<GUIGuardReadyPopUp.LevelItem>();
			this.mLevelItems[num].Init(num, new EventDelegate.Callback(this.RefreshInfo));
			num++;
		}
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mBG);
		GameUITools.RegisterClickEvent("TeamBtn", new UIEventListener.VoidDelegate(this.OnTeamClick), this.mBG);
		GameUITools.RegisterClickEvent("StartBtn", new UIEventListener.VoidDelegate(this.OnStartBtnClick), this.mBG);
		this.mFarmBtn = GameUITools.RegisterClickEvent("FarmBtn", new UIEventListener.VoidDelegate(this.OnFarmBtnClick), this.mBG);
	}

	public void Open(int index, int level)
	{
		if (index >= 3 || index < 0)
		{
			global::Debug.LogError(new object[]
			{
				"index error , index : {0}",
				index
			});
			return;
		}
		this.mPanel.alpha = 1f;
		this.index = index;
		this.mTitle.text = Singleton<StringManager>.Instance.GetString(string.Format("guardTitle{0}", index));
		if (level < 0)
		{
			this.mLevelItems[GameCache.GetGuardLevel(index)].State = true;
		}
		else
		{
			this.mLevelItems[level].State = true;
		}
		this.RefreshInfo();
		GameUITools.PlayOpenWindowAnim(this.mBG.transform, null, true);
	}

	private void RefreshInfo()
	{
		for (int i = 0; i < this.mRewards.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.mRewards.transform.GetChild(i).gameObject);
		}
		int num = this.index * 3 + GUIGuardReadyPopUp.LevelItem.mCur.Index + 1;
		this.mMGInfo = Globals.Instance.AttDB.MGDict.GetInfo(num);
		if (this.mMGInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				"MGDict get info error , ID : {0} ",
				num
			});
			return;
		}
		int combatValue = Globals.Instance.Player.TeamSystem.GetCombatValue();
		this.mTeamCombat.text = combatValue.ToString();
		this.mRecommendCombat.text = this.mMGInfo.CombatValue.ToString();
		float num2 = (this.mMGInfo.CombatValue <= 0) ? 1f : ((float)combatValue / (float)this.mMGInfo.CombatValue);
		float[] expr_10C = new float[4];
		expr_10C[0] = 1f;
		expr_10C[1] = 0.8f;
		expr_10C[2] = 0.6f;
		float[] array = expr_10C;
		Color[] array2 = new Color[]
		{
			new Color32(102, 254, 0, 255),
			new Color32(254, 217, 14, 255),
			new Color32(252, 141, 0, 255),
			new Color32(254, 1, 3, 255)
		};
		for (int j = 0; j < array.Length; j++)
		{
			if (num2 >= array[j])
			{
				this.mTeamCombat.color = array2[j];
				break;
			}
		}
		for (int k = 0; k < this.mRewards.transform.childCount; k++)
		{
			UnityEngine.Object.Destroy(this.mRewards.transform.GetChild(k).gameObject);
		}
		if (this.mMGInfo == null)
		{
			return;
		}
		this.mPending.Refresh(this.mMGInfo.MinFragmentCount, this.mMGInfo.MaxFragmentCount);
		for (int l = 0; l < this.mMGInfo.RewardType.Count; l++)
		{
			if (this.mMGInfo.RewardType[l] > 0 && this.mMGInfo.RewardType[l] < 20)
			{
				Transform transform = GameUITools.CreateReward(this.mMGInfo.RewardType[l], this.mMGInfo.RewardValue1[l], this.mMGInfo.RewardValue2[l], this.mRewards.transform, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0).transform;
				transform.localScale = new Vector3(0.65f, 0.65f, 1f);
				transform.localPosition = new Vector3((float)(-66 + 70 * l), 8f, 0f);
			}
		}
		if (Tools.CanPlay(GameConst.GetInt32(214), true) && (1 << num & Globals.Instance.Player.Data.MGFlag) != 0)
		{
			this.mFarmCost = GameUITools.FindUILabel("Cost", this.mFarmBtn);
			this.mFarmCost.text = GameConst.GetInt32(215).ToString();
			this.mFarmBtn.SetActive(true);
		}
		else
		{
			this.mFarmBtn.SetActive(false);
		}
	}

	public void Hide()
	{
		this.mPanel.alpha = 0f;
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUITools.PlayCloseWindowAnim(this.mBG.transform, new TweenDelegate.TweenCallback(this.Hide), true);
	}

	private void OnTeamClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseScene.SaveTarget(this.index, GUIGuardReadyPopUp.LevelItem.mCur.Index);
		GameUIManager.mInstance.uiState.IsLocalPlayer = true;
		GameUIManager.mInstance.uiState.CombatPetSlot = 0;
		GameUIManager.mInstance.ChangeSession<GUITeamManageSceneV2>(null, false, true);
	}

	private void OnStartBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.Data.MGCount >= GameConst.GetInt32(125))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guard7", 0f, 0f);
			return;
		}
		GameCache.SetGuardLevel(this.index, GUIGuardReadyPopUp.LevelItem.mCur.Index);
		GameUIManager.mInstance.uiState.AdventureSceneInfo = Globals.Instance.AttDB.SceneDict.GetInfo(GameConst.GetInt32(120));
		if (GameUIManager.mInstance.uiState.AdventureSceneInfo == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("SceneInfo is null Error, MemoryGearSceneID : {0}", GameConst.GetInt32(120))
			});
			return;
		}
		GameUIManager.mInstance.uiState.PveSceneID = GameConst.GetInt32(120);
		GameUIManager.mInstance.uiState.PveSceneValue = this.mMGInfo.ID;
		MC2S_PveStart mC2S_PveStart = new MC2S_PveStart();
		mC2S_PveStart.SceneID = GameUIManager.mInstance.uiState.PveSceneID;
		mC2S_PveStart.Value = GameUIManager.mInstance.uiState.PveSceneValue;
		Globals.Instance.CliSession.Send(600, mC2S_PveStart);
	}

	private void OnFarmBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.Data.MGCount >= GameConst.GetInt32(125))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guard7", 0f, 0f);
			return;
		}
		GameCache.SetGuardLevel(this.index, GUIGuardReadyPopUp.LevelItem.mCur.Index);
		MC2S_MGFarm mC2S_MGFarm = new MC2S_MGFarm();
		mC2S_MGFarm.MGID = this.mMGInfo.ID;
		Globals.Instance.CliSession.Send(652, mC2S_MGFarm);
	}
}
