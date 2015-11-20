using Holoville.HOTween;
using Holoville.HOTween.Core;
using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GUIRollRewardsWindow : MonoBehaviour
{
	public enum ERollRewardType
	{
		ERT_Item,
		ERT_Pet,
		ERT_Slice
	}

	private struct RewardItem
	{
		public GUIRollRewardsWindow.ERollRewardType rewardType;

		public int infoID;

		public ulong GUID;

		public uint count;
	}

	private GUISoulReliquaryInfo baseSoulReliquaryLayer;

	private GUIRewardLuckyDrawInfo baseRewardLuckyDrawInfo;

	public GameObject window;

	private GameObject treasureBox;

	private List<GameObject> rewardsList;

	private GameObject rewards;

	private UISprite line;

	public GameObject buttonGroup;

	private GameObject OKBtn;

	private GameObject AgainBtn;

	private UISprite AgainGold;

	public UILabel AgainMoney;

	private UILabel AgainLabel;

	public UISprite mBG;

	public UISprite mTreasure1;

	public UISprite mTreasure2;

	private TweenPosition tp1;

	private TweenPosition tp2;

	private TweenScale ts1;

	private TweenRotation tr2;

	private TweenAlpha ta1;

	private TweenAlpha ta2;

	private Animation anim;

	private GameObject ui45;

	private GameObject ui45_1;

	public UISprite mLeftLit;

	public UISprite mRightLit;

	public TweenPosition LeftTP1;

	public TweenPosition LeftTP2;

	public TweenPosition RightTP1;

	public TweenPosition RightTP2;

	private GetPetLayer getPetLayer;

	private bool pv = true;

	private List<GUIRollRewardsWindow.RewardItem> mRewardItems;

	private Color itemNameColor;

	private bool isShowFlare;

	private int count;

	private int timeFrame;

	private bool continueAnim1;

	private bool continueAnim2;

	private int onFinishTimes;

	public void Init(MS2C_LuckyRoll reply, UnityEngine.Object basescene)
	{
		this.mRewardItems = new List<GUIRollRewardsWindow.RewardItem>();
		foreach (OpenLootData current in reply.Data)
		{
			if (current.InfoID != 0)
			{
				this.mRewardItems.Add(new GUIRollRewardsWindow.RewardItem
				{
					infoID = current.InfoID,
					count = current.Count
				});
			}
		}
		foreach (ulong current2 in reply.PetIDs)
		{
			if (current2 != 0uL)
			{
				this.mRewardItems.Add(new GUIRollRewardsWindow.RewardItem
				{
					rewardType = GUIRollRewardsWindow.ERollRewardType.ERT_Pet,
					GUID = current2,
					count = 1u
				});
			}
		}
		if (basescene is GUISoulReliquaryInfo)
		{
			if (this.baseSoulReliquaryLayer != null)
			{
				this.Refresh();
				return;
			}
			this.baseSoulReliquaryLayer = (GUISoulReliquaryInfo)basescene;
		}
		else if (basescene is GUIRewardLuckyDrawInfo)
		{
			if (this.baseRewardLuckyDrawInfo != null)
			{
				this.Refresh();
				return;
			}
			this.baseRewardLuckyDrawInfo = (GUIRewardLuckyDrawInfo)basescene;
		}
		this.rewardsList = new List<GameObject>();
		this.rewards = GameUITools.FindGameObject("Rewards", base.gameObject);
		this.mBG = GameUITools.FindUISprite("BG", base.gameObject);
		this.window = GameUITools.FindGameObject("Window", base.gameObject);
		this.buttonGroup = GameUITools.FindGameObject("ButtonGroup", this.window);
		this.OKBtn = GameUITools.FindGameObject("OK", this.buttonGroup);
		this.AgainBtn = GameUITools.FindGameObject("Again", this.buttonGroup);
		UIEventListener expr_1E1 = UIEventListener.Get(this.OKBtn);
		expr_1E1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1E1.onClick, new UIEventListener.VoidDelegate(this.OnOKClick));
		UIEventListener expr_20D = UIEventListener.Get(this.AgainBtn);
		expr_20D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_20D.onClick, new UIEventListener.VoidDelegate(this.OnAgainClick));
		if (this.baseRewardLuckyDrawInfo != null)
		{
			this.InitRollWindow();
		}
		else if (this.baseSoulReliquaryLayer != null)
		{
			this.InitSoulReliquaryWindow();
		}
	}

	private void InitRollWindow()
	{
		this.treasureBox = GameUITools.FindGameObject("Treasure-Box", base.gameObject);
		this.mTreasure1 = GameUITools.FindUISprite("Treasure-Box/Treasure1", base.gameObject);
		this.mTreasure2 = GameUITools.FindUISprite("Treasure-Box/Treasure2", base.gameObject);
		this.tp1 = this.mTreasure1.GetComponent<TweenPosition>();
		this.tp2 = this.mTreasure2.GetComponent<TweenPosition>();
		this.ts1 = this.mTreasure1.GetComponent<TweenScale>();
		this.tr2 = this.mTreasure2.GetComponent<TweenRotation>();
		this.ta1 = this.mTreasure1.GetComponent<TweenAlpha>();
		this.ta2 = this.mTreasure2.GetComponent<TweenAlpha>();
		this.AgainGold = GameUITools.FindUISprite("Cost/Gold", this.AgainBtn.gameObject);
		this.AgainMoney = GameUITools.FindUILabel("Cost/Money", this.AgainBtn.gameObject);
		this.AgainLabel = GameUITools.FindUILabel("Label", this.AgainBtn.gameObject);
		this.line = GameUITools.FindUISprite("Line", this.window);
		this.Refresh();
	}

	private void InitSoulReliquaryWindow()
	{
		UILabel uILabel = GameUITools.FindUILabel("Cost/Money", this.AgainBtn.gameObject);
		UILabel uILabel2 = GameUITools.FindUILabel("Label", this.AgainBtn.gameObject);
		GameObject parent = GameUITools.FindGameObject("LidPanel", this.window);
		this.anim = this.window.GetComponent<Animation>();
		if (this.anim != null)
		{
			this.anim.playAutomatically = false;
		}
		this.ui45 = GameUITools.FindGameObject("ui45", this.window);
		this.ui45_1 = GameUITools.FindGameObject("ui45_1", this.window);
		this.mLeftLit = GameUITools.FindUISprite("Left", parent);
		this.mRightLit = GameUITools.FindUISprite("RightPanel/Right", parent);
		this.mLeftLit.gameObject.SetActive(true);
		this.mRightLit.gameObject.SetActive(true);
		uILabel2.text = Singleton<StringManager>.Instance.GetString("activitySoulReliquaryRollOneAgain");
		uILabel.text = GUISoulReliquaryInfo.GetPrice().ToString();
		TweenPosition[] components = this.mLeftLit.GetComponents<TweenPosition>();
		this.LeftTP1 = components[0];
		this.LeftTP2 = components[1];
		components = this.mRightLit.GetComponents<TweenPosition>();
		this.RightTP1 = components[0];
		this.RightTP2 = components[1];
		EventDelegate.Add(this.LeftTP1.onFinished, new EventDelegate.Callback(this.ContinueAnim1));
		EventDelegate.Add(this.RightTP1.onFinished, new EventDelegate.Callback(this.ContinueAnim1));
		EventDelegate.Add(this.LeftTP2.onFinished, new EventDelegate.Callback(this.ContinueAnim2));
		EventDelegate.Add(this.RightTP2.onFinished, new EventDelegate.Callback(this.ContinueAnim2));
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.baseSoulReliquaryLayer != null)
		{
			this.buttonGroup.transform.localScale = Vector3.zero;
			base.transform.localScale = Vector3.one;
			this.mBG.color = new Color32(14, 8, 5, 222);
			TweenAlpha.Begin(base.gameObject, 0f, 1f);
			if (this.ui45 != null)
			{
				this.ui45.SetActive(false);
			}
			if (this.ui45_1 != null)
			{
				this.ui45_1.SetActive(false);
			}
			this.mLeftLit.transform.localPosition = new Vector3(-109f, 0f, 0f);
			this.mRightLit.transform.localPosition = new Vector3(109f, 0f, 0f);
			this.LeftTP1.tweenFactor = 0f;
			this.LeftTP2.tweenFactor = 0f;
			this.RightTP1.tweenFactor = 0f;
			this.RightTP2.tweenFactor = 0f;
			this.LeftTP1.enabled = false;
			this.LeftTP2.enabled = false;
			this.RightTP1.enabled = false;
			this.RightTP2.enabled = false;
			this.continueAnim1 = false;
			this.continueAnim2 = false;
			Globals.Instance.BackgroundMusicMgr.VolumeDown(5f);
			Globals.Instance.EffectSoundMgr.Play("ui/ui_019");
			base.Invoke("PlayOpen", 0.6f);
		}
		else if (this.baseRewardLuckyDrawInfo != null)
		{
			GUIRewardLuckyDrawInfo.ERollType rollType = this.baseRewardLuckyDrawInfo.RollType;
			if (rollType != GUIRewardLuckyDrawInfo.ERollType.ERollType_High_One)
			{
				if (rollType == GUIRewardLuckyDrawInfo.ERollType.ERollType_High_Ten)
				{
					this.window.GetComponent<UISprite>().SetDimensions(950, 512);
					this.line.SetDimensions(950, 2);
					this.OKBtn.transform.localPosition = new Vector3(253f, 0f, 0f);
					this.AgainBtn.transform.localPosition = new Vector3(-256f, 0f, 0f);
					this.AgainGold.spriteName = "redGem_1";
					this.AgainLabel.text = Singleton<StringManager>.Instance.GetString("rollTenAgain");
					this.AgainMoney.text = GameConst.GetInt32(66).ToString();
				}
			}
			else
			{
				this.window.GetComponent<UISprite>().SetDimensions(598, 512);
				this.line.SetDimensions(598, 2);
				this.OKBtn.transform.localPosition = new Vector3(148f, 0f, 0f);
				this.AgainBtn.transform.localPosition = new Vector3(-150f, 0f, 0f);
				this.AgainGold.spriteName = "redGem_1";
				this.AgainLabel.text = Singleton<StringManager>.Instance.GetString("rollOneAgain");
				this.AgainMoney.text = GameConst.GetInt32(65).ToString();
			}
			this.buttonGroup.transform.localScale = Vector3.zero;
			this.treasureBox.SetActive(false);
			base.transform.localScale = Vector3.one;
			this.tp1.tweenFactor = 0f;
			this.tp2.tweenFactor = 0f;
			this.ts1.tweenFactor = 0f;
			this.tr2.tweenFactor = 0f;
			this.ta1.tweenFactor = 0f;
			this.ta2.tweenFactor = 0f;
			this.tp1.enabled = true;
			this.tp2.enabled = true;
			this.ts1.enabled = true;
			this.tr2.enabled = true;
			this.ta1.enabled = true;
			this.ta2.enabled = true;
			TweenAlpha.Begin(base.gameObject, 0f, 1f);
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.PlayTreasureAnim());
		}
	}

	private void PlayOpen()
	{
		base.gameObject.SetActive(true);
		this.window.SetActive(true);
		GameUITools.PlayOpenWindowAnim(this.window.transform, new TweenDelegate.TweenCallback(this.OpenSoulReliquary), true);
	}

	private void OpenSoulReliquary()
	{
		GameUIManager.mInstance.HideFadeBG(false);
		base.Invoke("PlaySoulReliquaryAnim", 0.6f);
	}

	private void PlaySoulReliquaryAnim()
	{
		if (this.anim != null)
		{
			this.anim.Play();
		}
		if (this.ui45 != null)
		{
			this.ui45.SetActive(true);
		}
		if (this.ui45_1 != null)
		{
			this.ui45_1.SetActive(true);
		}
		base.StartCoroutine(this.PlayReliquaryAnim());
	}

	public void OnOKClick(GameObject obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUITools.PlayCloseWindowAnim(base.transform, new TweenDelegate.TweenCallback(this.OnCloseAnimEnd), true);
	}

	private void OnCloseAnimEnd()
	{
		this.Clean();
	}

	private void OnAgainClick(GameObject obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUITools.PlayCloseWindowAnim(base.transform, new TweenDelegate.TweenCallback(this.OnAgainCloseAnimEnd), true);
	}

	private void OnAgainCloseAnimEnd()
	{
		this.Clean();
		if (this.baseSoulReliquaryLayer != null)
		{
			this.baseSoulReliquaryLayer.OnRollClick(null);
		}
		else if (this.baseRewardLuckyDrawInfo != null)
		{
			GUIRewardLuckyDrawInfo.ERollType rollType = this.baseRewardLuckyDrawInfo.RollType;
			if (rollType != GUIRewardLuckyDrawInfo.ERollType.ERollType_High_One)
			{
				if (rollType == GUIRewardLuckyDrawInfo.ERollType.ERollType_High_Ten)
				{
					this.baseRewardLuckyDrawInfo.OnBuyTenClick(null);
				}
			}
			else
			{
				this.baseRewardLuckyDrawInfo.OnBuyOneClick(null);
			}
		}
	}

	private void OnDealAnimEnd()
	{
		HOTween.To(this.buttonGroup.transform, 0.2f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack).OnComplete(new TweenDelegate.TweenCallback(this.NotifyGuideManager)));
	}

	private void NotifyGuideManager()
	{
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private void OnDealOneEndThenShowName(TweenEvent data)
	{
		UILabel uILabel = GameUITools.FindUILabel("Info", this.rewardsList[this.rewardsList.Count - 1]);
		UISprite uISprite = GameUITools.FindUISprite("rollRewardMask", this.rewardsList[this.rewardsList.Count - 1]);
		uILabel.color = this.itemNameColor;
		uILabel.gameObject.SetActive(true);
		uISprite.gameObject.SetActive(false);
		this.pv = false;
		if (data != null && data.parms != null)
		{
			if (this.baseSoulReliquaryLayer != null)
			{
				if ((int)data.parms[0] == this.count - 1)
				{
					this.OnDealAnimEnd();
				}
			}
			else if (this.baseRewardLuckyDrawInfo != null && (int)data.parms[0] == this.count - 1)
			{
				this.OnDealAnimEnd();
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator PlayTreasureAnim()
	{
        return null;
        //GUIRollRewardsWindow.<PlayTreasureAnim>c__Iterator71 <PlayTreasureAnim>c__Iterator = new GUIRollRewardsWindow.<PlayTreasureAnim>c__Iterator71();
        //<PlayTreasureAnim>c__Iterator.<>f__this = this;
        //return <PlayTreasureAnim>c__Iterator;
	}

	private void ContinueAnim1()
	{
		this.onFinishTimes++;
		if (this.onFinishTimes >= 2)
		{
			this.continueAnim1 = true;
			this.onFinishTimes = 0;
		}
	}

	private void ContinueAnim2()
	{
		this.onFinishTimes++;
		if (this.onFinishTimes >= 2)
		{
			this.continueAnim2 = true;
			this.onFinishTimes = 0;
		}
	}

	[DebuggerHidden]
	private IEnumerator PlayReliquaryAnim()
	{
        return null;
        //GUIRollRewardsWindow.<PlayReliquaryAnim>c__Iterator72 <PlayReliquaryAnim>c__Iterator = new GUIRollRewardsWindow.<PlayReliquaryAnim>c__Iterator72();
        //<PlayReliquaryAnim>c__Iterator.<>f__this = this;
        //return <PlayReliquaryAnim>c__Iterator;
	}

	private Transform InitRewardAndGetTransform(GUIRollRewardsWindow.RewardItem currentReward, string name, int index)
	{
		float x = 0f;
		float y = 0f;
		float z = -2000f;
		float r = 20f;
		float g = 13f;
		float b = 7f;
		GameObject gameObject = Tools.InstantiateGUIPrefab("GUI/RewardRoll");
		TweenAlpha component = GameUITools.FindGameObject("rollRewardMask", gameObject).GetComponent<TweenAlpha>();
		GameUITools.AddChild(this.rewards.gameObject, gameObject);
		int rewardType = 0;
		switch (currentReward.rewardType)
		{
		case GUIRollRewardsWindow.ERollRewardType.ERT_Item:
		case GUIRollRewardsWindow.ERollRewardType.ERT_Slice:
			rewardType = 3;
			break;
		case GUIRollRewardsWindow.ERollRewardType.ERT_Pet:
			rewardType = 4;
			break;
		}
		if (this.baseSoulReliquaryLayer != null)
		{
			component.duration = 0.3f;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			r = 41f;
			g = 27f;
			b = 16f;
		}
		else if (this.baseRewardLuckyDrawInfo != null)
		{
			component.duration = 0.25f;
			if (this.baseRewardLuckyDrawInfo.RollType == GUIRewardLuckyDrawInfo.ERollType.ERollType_High_Ten && index == 0)
			{
				x = 50f;
			}
			else if (index == 5)
			{
				x = 50f;
			}
			else if (index == 4 || index == 9)
			{
				x = -100f;
			}
			gameObject.transform.localPosition = new Vector3(0f, 40f, 0f);
			gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
			r = 22f;
			g = 13f;
			b = 8f;
		}
		GameObject parent = GameUITools.CreateReward(rewardType, currentReward.infoID, (int)currentReward.count, gameObject.transform, true, true, x, y, z, r, g, b, 0);
		GameUITools.FindUILabel("Info", gameObject).text = name;
		if (this.isShowFlare)
		{
			UISprite uISprite = GameUITools.FindUISprite("flare", parent);
			uISprite.color = this.itemNameColor;
			uISprite.gameObject.SetActive(true);
		}
		this.rewardsList.Add(gameObject);
		return gameObject.transform;
	}

	private void Clean()
	{
		for (int i = 0; i < this.rewardsList.Count; i++)
		{
			UnityEngine.Object.DestroyImmediate(this.rewardsList[i]);
		}
		this.rewardsList.Clear();
		this.window.SetActive(false);
		base.gameObject.SetActive(false);
	}
}
