using Att;
using Holoville.HOTween;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIRollingSceneV2 : GameUISession
{
	public enum ERollType
	{
		ERollType_Low,
		ERollType_high
	}

	public enum ERollTimes
	{
		ERollTimes_One,
		ERollTimes_Ten
	}

	public AnimationCurve RollOneAnimCurve;

	[NonSerialized]
	public float RollOneAnimDuration = 0.2f;

	[NonSerialized]
	public float PlayGetPetLayerTime = 0.05f;

	[NonSerialized]
	public int UI62_1RQ = 3002;

	private UITexture mBG;

	private UITexture mStage;

	private GameObject mCloseBtn;

	private UILabel mTitleName;

	private UILabel mTitleDesc0;

	private UILabel mTitleDesc1;

	private UISprite mTitleElement;

	private UISprite mTitleType;

	private GameObject mSlot;

	private GameObject mUI62_1;

	private GameObject mUI62_2;

	private GameObject mViewBtn;

	private UISprite mCurrencyIcon;

	private UILabel mCurrencyValue;

	private UISprite mCurrencyAdd;

	private UILabel mNextDesc;

	private UISprite mOneCostIcon;

	private UILabel mOneCostValue;

	private UILabel mOneCostFree;

	private UISprite mTenCostIcon;

	private UILabel mTenCostValue;

	private UILabel mAgainTxt;

	private UISprite mAgainIcon;

	private UILabel mAgainValue;

	private UILabel mAgainFree;

	private GameObject mRollOne;

	private GameObject mRollTen;

	private UIButton mRollTenBtn;

	private GameObject mRollAgain;

	private GameObject mOK;

	private UISprite mTenBG;

	private int leftPos = -242;

	private int centerPos;

	private int btnLowPos = -191;

	private int btnHighPos = -166;

	private Color32 redCol = new Color32(225, 32, 32, 255);

	private PetDataEx mCurPetData;

	private ActivityValueData mActivityValueData;

	private int oneCost;

	private int tenCost;

	private ResourceEntity asyncEntity;

	private GameObject petModel;

	private UIActorController actorController;

	private Vector3 tempScale;

	private int petCount;

	private bool pv;

	private GetPetLayer getPetLayer;

	private GameObject mRewards;

	private List<GameObject> rewardsList = new List<GameObject>();

	private Color itemNameColor;

	private bool isShowFlare;

	private GUIRollingSceneV2.ERollType rollType;

	private GUIRollingSceneV2.ERollTimes rollTimes;

	private string lowItemIconName = "lowItem";

	private string highItemIconName = "highItem";

	private string highDiamondIconName = "redGem_1";

	private int lowItemCount;

	private int highItemCount;

	public static void Change2This(GUIRollingSceneV2.ERollType type)
	{
		GameUIManager.mInstance.ChangeSession<GUIRollingSceneV2>(delegate(GUIRollingSceneV2 sen)
		{
			sen.InitState(type);
		}, false, false);
	}

	public static bool TryClose()
	{
		if (GameUIManager.mInstance.CurUISession is GUIRollingSceneV2)
		{
			GUIRollingSceneV2 gUIRollingSceneV = (GUIRollingSceneV2)GameUIManager.mInstance.CurUISession;
			if (gUIRollingSceneV.mOK.activeInHierarchy || gUIRollingSceneV.mCloseBtn.activeInHierarchy)
			{
				gUIRollingSceneV.OnCloseClick(null);
				return true;
			}
		}
		return false;
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		Globals.Instance.CliSession.Register(225, new ClientSession.MsgHandler(this.OnMsgRoll));
		LocalPlayer expr_30 = Globals.Instance.Player;
		expr_30.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_30.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
	}

	protected override void OnPreDestroyGUI()
	{
		if (this.asyncEntity != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntity);
			this.asyncEntity = null;
		}
		Globals.Instance.CliSession.Unregister(225, new ClientSession.MsgHandler(this.OnMsgRoll));
		LocalPlayer expr_47 = Globals.Instance.Player;
		expr_47.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_47.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
	}

	private void CreateObjects()
	{
		this.mActivityValueData = Globals.Instance.Player.ActivitySystem.GetValueMod(5);
		if (this.mActivityValueData == null)
		{
			this.oneCost = GameConst.GetInt32(41);
			this.tenCost = GameConst.GetInt32(42);
		}
		else
		{
			this.oneCost = GameConst.GetInt32(41) * this.mActivityValueData.Value1 / 100;
			this.tenCost = GameConst.GetInt32(42) * this.mActivityValueData.Value1 / 100;
		}
		this.mBG = GameUITools.FindGameObject("BG", base.gameObject).GetComponent<UITexture>();
		this.mStage = GameUITools.FindGameObject("Stage", base.gameObject).GetComponent<UITexture>();
		GameObject gameObject = GameUITools.FindGameObject("Window/Title", base.gameObject);
		this.mTitleName = GameUITools.FindUILabel("Name", gameObject);
		this.mTitleDesc0 = GameUITools.FindUILabel("Desc0", gameObject);
		this.mTitleDesc1 = GameUITools.FindUILabel("Desc1", gameObject);
		this.mTitleElement = GameUITools.FindUISprite("Element", gameObject);
		this.mTitleType = GameUITools.FindUISprite("Type", gameObject);
		gameObject = gameObject.transform.parent.gameObject;
		this.mSlot = GameUITools.FindGameObject("Slot", gameObject);
		this.mUI62_1 = GameUITools.FindGameObject("ui62_1", gameObject);
		Tools.SetParticleRQWithUIScale(this.mUI62_1, this.UI62_1RQ);
		this.mUI62_1.SetActive(false);
		this.mUI62_2 = GameUITools.FindGameObject("ui62_2", gameObject);
		Tools.SetParticleRQWithUIScale(this.mUI62_2, 3002);
		this.mUI62_2.SetActive(false);
		this.mCloseBtn = GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), gameObject);
		this.mViewBtn = GameUITools.RegisterClickEvent("ViewBtn", new UIEventListener.VoidDelegate(this.OnViewBtnClick), gameObject);
		gameObject = GameUITools.FindGameObject("Currency", gameObject);
		this.mCurrencyIcon = GameUITools.FindUISprite("Icon", gameObject);
		this.mCurrencyValue = GameUITools.FindUILabel("Value", gameObject);
		this.mCurrencyAdd = GameUITools.RegisterClickEvent("Add", new UIEventListener.VoidDelegate(this.OnCurrencyAddClic), gameObject).GetComponent<UISprite>();
		gameObject = gameObject.transform.parent.gameObject;
		this.mNextDesc = GameUITools.FindUILabel("NextDesc/Txt", gameObject);
		this.mRollOne = GameUITools.RegisterClickEvent("RollOne", new UIEventListener.VoidDelegate(this.OnRollOneClick), gameObject);
		this.mRollTen = GameUITools.RegisterClickEvent("RollTen", new UIEventListener.VoidDelegate(this.OnRollTenClick), gameObject);
		this.mRollTenBtn = this.mRollTen.GetComponent<UIButton>();
		this.mRollAgain = GameUITools.RegisterClickEvent("RollAgain", new UIEventListener.VoidDelegate(this.OnRollAgainClick), gameObject);
		this.mRollAgain = GameUITools.FindGameObject("RollAgain", gameObject);
		this.mOK = GameUITools.RegisterClickEvent("OK", new UIEventListener.VoidDelegate(this.OnOKClick), gameObject);
		this.mRewards = GameUITools.FindGameObject("Rewards", gameObject);
		gameObject = GameUITools.FindGameObject("Cost", this.mRollOne);
		this.mOneCostIcon = GameUITools.FindUISprite("Icon", gameObject);
		this.mOneCostValue = GameUITools.FindUILabel("Value", gameObject);
		this.mOneCostFree = GameUITools.FindUILabel("Free", gameObject);
		gameObject = GameUITools.FindGameObject("Cost", this.mRollTen);
		this.mTenCostIcon = GameUITools.FindUISprite("Icon", gameObject);
		this.mTenCostValue = GameUITools.FindUILabel("Value", gameObject);
		this.mAgainTxt = GameUITools.FindUILabel("Txt", this.mRollAgain);
		gameObject = GameUITools.FindGameObject("Cost", this.mRollAgain);
		this.mAgainIcon = GameUITools.FindUISprite("Icon", gameObject);
		this.mAgainValue = GameUITools.FindUILabel("Value", gameObject);
		this.mAgainFree = GameUITools.FindUILabel("Free", gameObject);
		this.mTenBG = GameUITools.FindUISprite("TenBG", this.mRollAgain.transform.parent.gameObject);
		this.mTitleName.transform.parent.gameObject.SetActive(false);
		this.mNextDesc.transform.parent.gameObject.SetActive(false);
		this.mCurrencyValue.transform.parent.gameObject.SetActive(false);
		this.mRollOne.SetActive(false);
		this.mRollAgain.SetActive(false);
		this.mRollTen.SetActive(false);
		this.mOK.SetActive(false);
	}

	private void OnMsgRoll(MemoryStream stream)
	{
		MS2C_LuckyRoll mS2C_LuckyRoll = Serializer.NonGeneric.Deserialize(typeof(MS2C_LuckyRoll), stream) as MS2C_LuckyRoll;
		if (mS2C_LuckyRoll.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_LuckyRoll.Result);
			return;
		}
		if (mS2C_LuckyRoll.Type == 0 || mS2C_LuckyRoll.Type == 1)
		{
			this.Clean();
			this.StartRoll(mS2C_LuckyRoll.PetIDs);
			this.RefreshCurrency();
			this.RefreshNextDesc();
			GameAnalytics.LuckyRollEvent(mS2C_LuckyRoll);
		}
	}

	private void ClearModel()
	{
		if (this.mCurPetData != null)
		{
			this.mCurPetData = null;
		}
		if (this.asyncEntity != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntity);
			this.asyncEntity = null;
		}
		if (this.petModel != null)
		{
			this.actorController = null;
			UnityEngine.Object.Destroy(this.petModel);
			this.petModel = null;
		}
	}

	private void StartRoll(List<ulong> ids)
	{
		this.mCloseBtn.gameObject.SetActive(false);
		this.mViewBtn.gameObject.SetActive(false);
		this.mCurrencyIcon.transform.parent.gameObject.SetActive(false);
		this.mTitleName.transform.parent.gameObject.SetActive(false);
		this.mNextDesc.transform.parent.gameObject.SetActive(false);
		this.mRollOne.gameObject.SetActive(false);
		this.mRollTen.gameObject.SetActive(false);
		this.mRollAgain.gameObject.SetActive(false);
		this.mOK.gameObject.SetActive(false);
		this.mTitleDesc0.enabled = false;
		this.mTitleDesc1.enabled = false;
		this.ClearModel();
		GUIRollingSceneV2.ERollTimes eRollTimes = this.rollTimes;
		if (eRollTimes != GUIRollingSceneV2.ERollTimes.ERollTimes_One)
		{
			if (eRollTimes == GUIRollingSceneV2.ERollTimes.ERollTimes_Ten)
			{
				if (ids.Count < 1)
				{
					return;
				}
				this.mTenBG.gameObject.SetActive(true);
				this.mBG.color = Color.black;
				this.mStage.color = Color.black;
				List<PetDataEx> list = new List<PetDataEx>();
				foreach (ulong current in ids)
				{
					PetDataEx pet = Globals.Instance.Player.PetSystem.GetPet(current);
					if (pet != null)
					{
						list.Add(pet);
					}
				}
				base.StartCoroutine(this.PlayAnim(list));
			}
		}
		else
		{
			if (ids.Count < 1)
			{
				return;
			}
			this.mCurPetData = Globals.Instance.Player.PetSystem.GetPet(ids[0]);
			if (this.mCurPetData == null)
			{
				return;
			}
			this.mTenBG.gameObject.SetActive(false);
			this.mBG.color = Color.white;
			this.mStage.color = Color.white;
			base.StartCoroutine(this.PlayRollOneAnim());
		}
	}

	[DebuggerHidden]
	private IEnumerator PlayRollOneAnim()
	{
        return null;
        //GUIRollingSceneV2.<PlayRollOneAnim>c__Iterator6F <PlayRollOneAnim>c__Iterator6F = new GUIRollingSceneV2.<PlayRollOneAnim>c__Iterator6F();
        //<PlayRollOneAnim>c__Iterator6F.<>f__this = this;
        //return <PlayRollOneAnim>c__Iterator6F;
	}

	[DebuggerHidden]
	private IEnumerator PlayAnim(List<PetDataEx> pets)
	{
        return null;
        //GUIRollingSceneV2.<PlayAnim>c__Iterator70 <PlayAnim>c__Iterator = new GUIRollingSceneV2.<PlayAnim>c__Iterator70();
        //<PlayAnim>c__Iterator.pets = pets;
        //<PlayAnim>c__Iterator.<$>pets = pets;
        //<PlayAnim>c__Iterator.<>f__this = this;
        //return <PlayAnim>c__Iterator;
	}

	private void OnDealOneEndThenShowName(TweenEvent data)
	{
		UILabel uILabel = GameUITools.FindUILabel("Info", this.rewardsList[this.rewardsList.Count - 1]);
		UISprite uISprite = GameUITools.FindUISprite("rollRewardMask", this.rewardsList[this.rewardsList.Count - 1]);
		uILabel.color = this.itemNameColor;
		uILabel.gameObject.SetActive(true);
		uISprite.gameObject.SetActive(false);
		this.pv = false;
		if (data != null && data.parms != null && (int)data.parms[0] == this.petCount - 1)
		{
			this.RollEnd();
		}
	}

	private Transform InitRewardAndGetTransform(PetDataEx currentReward, string name, int index)
	{
		float x = 0f;
		float y = 0f;
		float z = -2000f;
		float r = 20f;
		float g = 13f;
		float b = 7f;
		GameObject gameObject = Tools.InstantiateGUIPrefab("GUI/RewardRoll");
		TweenAlpha component = GameUITools.FindGameObject("rollRewardMask", gameObject).GetComponent<TweenAlpha>();
		GameUITools.AddChild(this.mRewards.gameObject, gameObject);
		component.duration = 0.25f;
		if (index == 0)
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
		gameObject.transform.localPosition = new Vector3(0f, -134f, 0f);
		gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		GameObject parent = GameUITools.CreateReward(4, currentReward.Info.ID, 1, gameObject.transform, true, true, x, y, z, r, g, b, 0);
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

	public void RollOneFeatureCardClick()
	{
		this.petModel.gameObject.SetActive(true);
		this.RollEnd();
	}

	private void Clean()
	{
		for (int i = 0; i < this.rewardsList.Count; i++)
		{
			UnityEngine.Object.DestroyImmediate(this.rewardsList[i]);
		}
		this.rewardsList.Clear();
	}

	private void OnModelClick()
	{
		GameUIManager.mInstance.ShowPetInfoSceneV2(this.mCurPetData, 0, null, 0);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIManager.mInstance.ChangeSession<GUIRollSceneV2>(null, false, true);
	}

	public void OnOKClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIRollSceneV2>(null, false, true);
	}

	public void OnRollOneClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.rollTimes = GUIRollingSceneV2.ERollTimes.ERollTimes_One;
		GUIRollingSceneV2.ERollType eRollType = this.rollType;
		if (eRollType != GUIRollingSceneV2.ERollType.ERollType_Low)
		{
			if (eRollType == GUIRollingSceneV2.ERollType.ERollType_high)
			{
				if (this.highItemCount > 0 || GUIRollSceneV2.IsHighRollFree() || !Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.oneCost, 0))
				{
					this.SendRollRequestToServer();
				}
			}
		}
		else if (this.lowItemCount > 0 || GUIRollSceneV2.IsLowRollFree())
		{
			this.SendRollRequestToServer();
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTipByKey("rollLowItemNotEnough", 0f, 0f);
		}
	}

	private void OnRollTenClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.rollTimes = GUIRollingSceneV2.ERollTimes.ERollTimes_Ten;
		GUIRollingSceneV2.ERollType eRollType = this.rollType;
		if (eRollType != GUIRollingSceneV2.ERollType.ERollType_Low)
		{
			if (eRollType == GUIRollingSceneV2.ERollType.ERollType_high)
			{
				if (this.highItemCount >= GameConst.GetInt32(40) || !Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.tenCost, 0))
				{
					this.SendRollRequestToServer();
				}
			}
		}
		else if (this.lowItemCount >= GameConst.GetInt32(38))
		{
			this.SendRollRequestToServer();
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTipByKey("rollLowItemNotEnough", 0f, 0f);
		}
	}

	private void OnRollAgainClick(GameObject go)
	{
		GUIRollingSceneV2.ERollTimes eRollTimes = this.rollTimes;
		if (eRollTimes != GUIRollingSceneV2.ERollTimes.ERollTimes_One)
		{
			if (eRollTimes == GUIRollingSceneV2.ERollTimes.ERollTimes_Ten)
			{
				this.OnRollTenClick(null);
			}
		}
		else
		{
			this.OnRollOneClick(null);
		}
	}

	private void OnViewBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIRollingSceneV2.ERollType eRollType = this.rollType;
		if (eRollType != GUIRollingSceneV2.ERollType.ERollType_Low)
		{
			if (eRollType == GUIRollingSceneV2.ERollType.ERollType_high)
			{
				GUIPetViewPopUp.Show(GUIRollingSceneV2.ERollType.ERollType_high);
			}
		}
		else
		{
			GUIPetViewPopUp.Show(GUIRollingSceneV2.ERollType.ERollType_Low);
		}
	}

	private void OnCurrencyAddClic(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIVip.OpenRecharge();
	}

	private void SendRollRequestToServer()
	{
		if (Tools.IsPetBagFull())
		{
			return;
		}
		MC2S_LuckyRoll mC2S_LuckyRoll = new MC2S_LuckyRoll();
		GUIRollingSceneV2.ERollType eRollType = this.rollType;
		if (eRollType != GUIRollingSceneV2.ERollType.ERollType_Low)
		{
			if (eRollType == GUIRollingSceneV2.ERollType.ERollType_high)
			{
				mC2S_LuckyRoll.Type = 1;
				GUIRollingSceneV2.ERollTimes eRollTimes = this.rollTimes;
				if (eRollTimes != GUIRollingSceneV2.ERollTimes.ERollTimes_One)
				{
					if (eRollTimes == GUIRollingSceneV2.ERollTimes.ERollTimes_Ten)
					{
						mC2S_LuckyRoll.Flag = true;
						mC2S_LuckyRoll.Free = false;
					}
				}
				else
				{
					mC2S_LuckyRoll.Flag = false;
					mC2S_LuckyRoll.Free = GUIRollSceneV2.IsHighRollFree();
				}
			}
		}
		else
		{
			mC2S_LuckyRoll.Type = 0;
			GUIRollingSceneV2.ERollTimes eRollTimes = this.rollTimes;
			if (eRollTimes != GUIRollingSceneV2.ERollTimes.ERollTimes_One)
			{
				if (eRollTimes == GUIRollingSceneV2.ERollTimes.ERollTimes_Ten)
				{
					mC2S_LuckyRoll.Free = false;
					mC2S_LuckyRoll.Flag = true;
				}
			}
			else
			{
				mC2S_LuckyRoll.Free = GUIRollSceneV2.IsLowRollFree();
				mC2S_LuckyRoll.Flag = false;
			}
		}
		Globals.Instance.CliSession.Send(224, mC2S_LuckyRoll);
	}

	private void OnPlayerUpdateEvent()
	{
		this.RefreshDiamondCostTxtColor();
		this.RefreshCurrency();
	}

	public void InitState(GUIRollingSceneV2.ERollType type)
	{
		this.rollType = type;
		this.mTitleName.transform.parent.gameObject.SetActive(true);
		this.mNextDesc.transform.parent.gameObject.SetActive(true);
		this.mCurrencyValue.transform.parent.gameObject.SetActive(true);
		this.mTenBG.gameObject.SetActive(false);
		this.mTitleElement.enabled = false;
		this.mTitleType.enabled = false;
		this.RefreshCurrency();
		GUIRollingSceneV2.ERollType eRollType = this.rollType;
		if (eRollType != GUIRollingSceneV2.ERollType.ERollType_Low)
		{
			if (eRollType == GUIRollingSceneV2.ERollType.ERollType_high)
			{
				this.mTitleName.text = Singleton<StringManager>.Instance.GetString("rollHighLabel");
				this.mTitleDesc0.enabled = false;
				this.mTitleDesc1.enabled = true;
				this.mNextDesc.transform.parent.gameObject.SetActive(true);
				this.RefreshNextDesc();
				this.highItemCount = Globals.Instance.Player.ItemSystem.GetHighRollItemCount();
				if (GUIRollSceneV2.IsHighRollFree())
				{
					this.mRollOne.SetActive(true);
					this.mRollTen.SetActive(false);
					this.mRollAgain.SetActive(false);
					this.mOK.SetActive(false);
					this.mRollOne.transform.localPosition = new Vector3((float)this.centerPos, this.mRollOne.transform.localPosition.y, 0f);
					this.mOneCostFree.enabled = true;
					this.mOneCostIcon.enabled = false;
					this.mOneCostValue.enabled = false;
				}
				else if (this.highItemCount >= GameConst.GetInt32(40))
				{
					this.mOneCostIcon.spriteName = this.highItemIconName;
					this.mTenCostIcon.spriteName = this.highItemIconName;
					this.mAgainIcon.spriteName = this.highItemIconName;
					this.mRollOne.SetActive(true);
					this.mRollTen.SetActive(true);
					this.mRollAgain.SetActive(false);
					this.mOK.SetActive(false);
					this.mRollOne.transform.localPosition = new Vector3((float)this.leftPos, this.mRollOne.transform.localPosition.y, 0f);
					this.mOneCostFree.enabled = false;
					this.mOneCostIcon.enabled = true;
					this.mOneCostValue.enabled = true;
					this.mOneCostIcon.spriteName = this.highItemIconName;
					this.mOneCostValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
					{
						GameConst.GetInt32(39)
					});
					this.mTenCostIcon.spriteName = this.highItemIconName;
					this.mTenCostValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
					{
						GameConst.GetInt32(40)
					});
				}
				else if (this.highItemCount == 0)
				{
					this.mOneCostIcon.spriteName = this.highDiamondIconName;
					this.mTenCostIcon.spriteName = this.highDiamondIconName;
					this.mAgainIcon.spriteName = this.highDiamondIconName;
					this.mRollOne.SetActive(true);
					this.mRollTen.SetActive(true);
					this.mRollAgain.SetActive(false);
					this.mOK.SetActive(false);
					this.mRollOne.transform.localPosition = new Vector3((float)this.leftPos, this.mRollOne.transform.localPosition.y, 0f);
					this.mOneCostFree.enabled = false;
					this.mOneCostIcon.enabled = true;
					this.mOneCostValue.enabled = true;
					this.mOneCostIcon.spriteName = this.highDiamondIconName;
					this.mTenCostIcon.spriteName = this.highDiamondIconName;
					this.mOneCostValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
					{
						this.oneCost
					});
					this.mTenCostValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
					{
						this.tenCost
					});
					if (Globals.Instance.Player.Data.Diamond < this.oneCost)
					{
						this.mOneCostValue.color = this.redCol;
					}
					else if (this.mActivityValueData == null)
					{
						this.mOneCostValue.color = Color.white;
					}
					else
					{
						this.mOneCostValue.color = Color.yellow;
					}
					if (Globals.Instance.Player.Data.Diamond < this.tenCost)
					{
						this.mTenCostValue.color = this.redCol;
					}
					else if (this.mActivityValueData == null)
					{
						this.mTenCostValue.color = Color.white;
					}
					else
					{
						this.mTenCostValue.color = Color.yellow;
					}
				}
				else
				{
					this.mOneCostIcon.spriteName = this.highItemIconName;
					this.mRollOne.SetActive(true);
					this.mRollTen.SetActive(false);
					this.mRollAgain.SetActive(false);
					this.mOK.SetActive(false);
					this.mRollOne.transform.localPosition = new Vector3((float)this.centerPos, this.mRollOne.transform.localPosition.y, 0f);
					this.mOneCostFree.enabled = false;
					this.mOneCostIcon.enabled = true;
					this.mOneCostValue.enabled = true;
					this.mOneCostIcon.spriteName = this.highItemIconName;
					this.mOneCostValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
					{
						GameConst.GetInt32(39)
					});
				}
			}
		}
		else
		{
			this.mOneCostIcon.spriteName = this.lowItemIconName;
			this.mTenCostIcon.spriteName = this.lowItemIconName;
			this.mAgainIcon.spriteName = this.lowItemIconName;
			this.mTitleName.text = Singleton<StringManager>.Instance.GetString("rollLowLabel");
			this.mTitleDesc0.enabled = true;
			this.mTitleDesc1.enabled = false;
			this.mNextDesc.transform.parent.gameObject.SetActive(false);
			this.mOneCostValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
			{
				GameConst.GetInt32(37)
			});
			this.mTenCostValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
			{
				GameConst.GetInt32(38)
			});
			this.lowItemCount = Globals.Instance.Player.ItemSystem.GetLowRollItemCount();
			if (GUIRollSceneV2.IsLowRollFree())
			{
				this.mRollOne.SetActive(true);
				this.mRollTen.SetActive(false);
				this.mRollAgain.SetActive(false);
				this.mOK.SetActive(false);
				this.mRollOne.transform.localPosition = new Vector3((float)this.centerPos, this.mRollOne.transform.localPosition.y, 0f);
				this.mOneCostFree.enabled = true;
				this.mOneCostIcon.enabled = false;
				this.mOneCostValue.enabled = false;
			}
			else if (this.lowItemCount >= GameConst.GetInt32(38))
			{
				this.mRollOne.SetActive(true);
				this.mRollTen.SetActive(true);
				this.mRollAgain.SetActive(false);
				this.mOK.SetActive(false);
				this.mRollOne.transform.localPosition = new Vector3((float)this.leftPos, this.mRollOne.transform.localPosition.y, 0f);
				this.mOneCostFree.enabled = false;
				this.mOneCostIcon.enabled = true;
				this.mOneCostValue.enabled = true;
			}
			else if (this.lowItemCount == 0)
			{
				this.mRollOne.SetActive(true);
				this.mRollTen.SetActive(true);
				this.mRollAgain.SetActive(false);
				this.mOK.SetActive(false);
				this.mRollOne.transform.localPosition = new Vector3((float)this.leftPos, this.mRollOne.transform.localPosition.y, 0f);
				this.mOneCostFree.enabled = false;
				this.mOneCostIcon.enabled = true;
				this.mOneCostValue.enabled = true;
				this.mOneCostValue.color = this.redCol;
				this.mTenCostValue.color = this.redCol;
				this.mRollTenBtn.isEnabled = false;
				UIButton[] components = this.mRollTen.GetComponents<UIButton>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].SetState(UIButtonColor.State.Disabled, true);
				}
			}
			else
			{
				this.mRollOne.SetActive(true);
				this.mRollTen.SetActive(false);
				this.mRollAgain.SetActive(false);
				this.mOK.SetActive(false);
				this.mRollOne.transform.localPosition = new Vector3((float)this.centerPos, this.mRollOne.transform.localPosition.y, 0f);
				this.mOneCostFree.enabled = false;
				this.mOneCostIcon.enabled = true;
				this.mOneCostValue.enabled = true;
			}
		}
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private void RefreshNextDesc()
	{
		if (this.rollType == GUIRollingSceneV2.ERollType.ERollType_high)
		{
			LuckyRollInfo info = Globals.Instance.AttDB.LuckyRollDict.GetInfo(1);
			int luckyRoll2Count = Globals.Instance.Player.Data.LuckyRoll2Count;
			int num = info.Count[0];
			int num2 = info.Count[1] + num;
			if (luckyRoll2Count < num2 - 1)
			{
				this.mNextDesc.text = Singleton<StringManager>.Instance.GetString("rollNextDesc", new object[]
				{
					num2 - luckyRoll2Count,
					Singleton<StringManager>.Instance.GetString("rollOrangePet")
				});
			}
			else if (luckyRoll2Count == num2 - 1)
			{
				this.mNextDesc.text = Singleton<StringManager>.Instance.GetString("rollThisTime", new object[]
				{
					Singleton<StringManager>.Instance.GetString("rollOrangePet")
				});
			}
			else if (luckyRoll2Count < num - 1)
			{
				this.mNextDesc.text = Singleton<StringManager>.Instance.GetString("rollNextDesc", new object[]
				{
					num - luckyRoll2Count,
					Singleton<StringManager>.Instance.GetString("rollPurplePet")
				});
			}
			else if (luckyRoll2Count == num - 1)
			{
				this.mNextDesc.text = Singleton<StringManager>.Instance.GetString("rollThisTime", new object[]
				{
					Singleton<StringManager>.Instance.GetString("rollPurplePet")
				});
			}
		}
	}

	private void RefreshCurrency()
	{
		this.lowItemCount = Globals.Instance.Player.ItemSystem.GetLowRollItemCount();
		this.highItemCount = Globals.Instance.Player.ItemSystem.GetHighRollItemCount();
		GUIRollingSceneV2.ERollType eRollType = this.rollType;
		if (eRollType != GUIRollingSceneV2.ERollType.ERollType_Low)
		{
			if (eRollType == GUIRollingSceneV2.ERollType.ERollType_high)
			{
				if (this.highItemCount > 0)
				{
					this.mCurrencyAdd.enabled = false;
					this.mCurrencyIcon.spriteName = this.highItemIconName;
					this.mCurrencyValue.text = Tools.FormatCurrency(this.highItemCount);
				}
				else
				{
					this.mCurrencyAdd.enabled = true;
					this.mCurrencyIcon.spriteName = this.highDiamondIconName;
					this.mCurrencyValue.text = Tools.FormatCurrency(Globals.Instance.Player.Data.Diamond);
				}
			}
		}
		else
		{
			this.mCurrencyAdd.enabled = false;
			this.mCurrencyIcon.spriteName = this.lowItemIconName;
			this.mCurrencyValue.text = Tools.FormatCurrency(this.lowItemCount);
		}
	}

	private void RefreshDiamondCostTxtColor()
	{
		if (this.rollType == GUIRollingSceneV2.ERollType.ERollType_high)
		{
			if (this.highItemCount > 0)
			{
				return;
			}
			if (Globals.Instance.Player.Data.Diamond < GameConst.GetInt32(41))
			{
				this.mOneCostValue.color = this.redCol;
			}
			else
			{
				this.mOneCostValue.color = Color.white;
			}
			if (Globals.Instance.Player.Data.Diamond < GameConst.GetInt32(42))
			{
				this.mTenCostValue.color = this.redCol;
			}
			else
			{
				this.mTenCostValue.color = Color.white;
			}
			GUIRollingSceneV2.ERollTimes eRollTimes = this.rollTimes;
			if (eRollTimes != GUIRollingSceneV2.ERollTimes.ERollTimes_One)
			{
				if (eRollTimes == GUIRollingSceneV2.ERollTimes.ERollTimes_Ten)
				{
					if (Globals.Instance.Player.Data.Diamond < GameConst.GetInt32(42))
					{
						this.mAgainValue.color = this.redCol;
					}
					else
					{
						this.mAgainValue.color = Color.white;
					}
				}
			}
			else if (Globals.Instance.Player.Data.Diamond < GameConst.GetInt32(41))
			{
				this.mAgainValue.color = this.redCol;
			}
			else
			{
				this.mAgainValue.color = Color.white;
			}
		}
	}

	public void RollEnd()
	{
		this.mRollAgain.gameObject.SetActive(true);
		this.mOK.gameObject.SetActive(true);
		GUIRollingSceneV2.ERollType eRollType = this.rollType;
		if (eRollType != GUIRollingSceneV2.ERollType.ERollType_Low)
		{
			if (eRollType == GUIRollingSceneV2.ERollType.ERollType_high)
			{
				GUIRollingSceneV2.ERollTimes eRollTimes = this.rollTimes;
				if (eRollTimes != GUIRollingSceneV2.ERollTimes.ERollTimes_One)
				{
					if (eRollTimes == GUIRollingSceneV2.ERollTimes.ERollTimes_Ten)
					{
						this.mRollAgain.transform.localPosition = new Vector3(this.mRollAgain.transform.localPosition.x, (float)this.btnHighPos, this.mRollAgain.transform.localPosition.z);
						this.mOK.transform.localPosition = new Vector3(this.mOK.transform.localPosition.x, (float)this.btnHighPos, this.mOK.transform.localPosition.z);
						if (this.highItemCount >= GameConst.GetInt32(40))
						{
							this.mAgainTxt.text = Singleton<StringManager>.Instance.GetString("rollAgain", new object[]
							{
								10
							});
							this.mAgainIcon.spriteName = this.highItemIconName;
							this.mAgainFree.enabled = false;
							this.mAgainIcon.enabled = true;
							this.mAgainValue.enabled = true;
							this.mAgainValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
							{
								GameConst.GetInt32(40)
							});
							this.mAgainValue.color = Color.white;
						}
						else if (GUIRollSceneV2.IsHighRollFree())
						{
							this.mAgainTxt.text = Singleton<StringManager>.Instance.GetString("rollAgain", new object[]
							{
								1
							});
							this.mAgainFree.enabled = true;
							this.mAgainIcon.enabled = false;
							this.mAgainValue.enabled = false;
							this.mAgainValue.color = Color.white;
							this.rollTimes = GUIRollingSceneV2.ERollTimes.ERollTimes_One;
						}
						else if (this.highItemCount > 0)
						{
							this.mAgainTxt.text = Singleton<StringManager>.Instance.GetString("rollAgain", new object[]
							{
								1
							});
							this.mAgainFree.enabled = false;
							this.mAgainIcon.enabled = true;
							this.mAgainValue.enabled = true;
							this.mAgainValue.color = Color.white;
							this.mAgainIcon.spriteName = this.highItemIconName;
							this.mAgainValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
							{
								GameConst.GetInt32(39)
							});
							this.rollTimes = GUIRollingSceneV2.ERollTimes.ERollTimes_One;
						}
						else
						{
							this.mAgainTxt.text = Singleton<StringManager>.Instance.GetString("rollAgain", new object[]
							{
								10
							});
							this.mAgainFree.enabled = false;
							this.mAgainIcon.enabled = true;
							this.mAgainValue.enabled = true;
							this.mAgainIcon.spriteName = this.highDiamondIconName;
							this.mAgainValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
							{
								this.tenCost
							});
							if (Globals.Instance.Player.Data.Diamond < this.tenCost)
							{
								this.mAgainValue.color = this.redCol;
							}
							else if (this.mActivityValueData == null)
							{
								this.mAgainValue.color = Color.white;
							}
							else
							{
								this.mAgainValue.color = Color.yellow;
							}
						}
						GameUIManager.mInstance.TryCommend(ECommentType.EComment_Summon10, 0f);
					}
				}
				else
				{
					this.mNextDesc.transform.parent.gameObject.SetActive(true);
					this.mAgainTxt.text = Singleton<StringManager>.Instance.GetString("rollAgain", new object[]
					{
						1
					});
					this.mTitleName.transform.parent.gameObject.SetActive(true);
					this.mTitleName.text = Tools.GetPetName(this.mCurPetData.Info);
					this.mTitleName.applyGradient = false;
					this.mTitleName.color = Tools.GetItemQualityColor(this.mCurPetData.Info.Quality);
					this.mTitleElement.spriteName = Tools.GetPropertyIconWithBorder((EElementType)this.mCurPetData.Info.ElementType);
					this.mTitleType.spriteName = Tools.GetPetTypeIcon(this.mCurPetData.Info.Type);
					this.mTitleElement.enabled = true;
					this.mTitleType.enabled = true;
					this.mRollAgain.transform.localPosition = new Vector3(this.mRollAgain.transform.localPosition.x, (float)this.btnLowPos, this.mRollAgain.transform.localPosition.z);
					this.mOK.transform.localPosition = new Vector3(this.mOK.transform.localPosition.x, (float)this.btnLowPos, this.mOK.transform.localPosition.z);
					if (GUIRollSceneV2.IsHighRollFree())
					{
						this.mAgainFree.enabled = true;
						this.mAgainIcon.enabled = false;
						this.mAgainValue.enabled = false;
						this.mAgainValue.color = Color.white;
					}
					else if (this.highItemCount > 0)
					{
						this.mAgainIcon.spriteName = this.highItemIconName;
						this.mAgainFree.enabled = false;
						this.mAgainIcon.enabled = true;
						this.mAgainValue.enabled = true;
						this.mAgainValue.color = Color.white;
						this.mAgainValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
						{
							GameConst.GetInt32(39)
						});
					}
					else
					{
						this.mAgainIcon.spriteName = this.highDiamondIconName;
						this.mAgainFree.enabled = false;
						this.mAgainIcon.enabled = true;
						this.mAgainValue.enabled = true;
						this.mAgainValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
						{
							this.oneCost
						});
						if (Globals.Instance.Player.Data.Diamond < this.oneCost)
						{
							this.mAgainValue.color = this.redCol;
						}
						else if (this.mActivityValueData == null)
						{
							this.mAgainValue.color = Color.white;
						}
						else
						{
							this.mAgainValue.color = Color.yellow;
						}
					}
				}
			}
		}
		else
		{
			GUIRollingSceneV2.ERollTimes eRollTimes = this.rollTimes;
			if (eRollTimes != GUIRollingSceneV2.ERollTimes.ERollTimes_One)
			{
				if (eRollTimes == GUIRollingSceneV2.ERollTimes.ERollTimes_Ten)
				{
					this.mRollAgain.transform.localPosition = new Vector3(this.mRollAgain.transform.localPosition.x, (float)this.btnHighPos, this.mRollAgain.transform.localPosition.z);
					this.mOK.transform.localPosition = new Vector3(this.mOK.transform.localPosition.x, (float)this.btnHighPos, this.mOK.transform.localPosition.z);
					if (this.lowItemCount >= GameConst.GetInt32(40))
					{
						this.mAgainTxt.text = Singleton<StringManager>.Instance.GetString("rollAgain", new object[]
						{
							10
						});
						this.mAgainValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
						{
							GameConst.GetInt32(38)
						});
						this.mRollAgain.transform.localPosition = new Vector3(this.mRollAgain.transform.localPosition.x, (float)this.btnHighPos, this.mRollAgain.transform.localPosition.z);
						this.mOK.transform.localPosition = new Vector3(this.mOK.transform.localPosition.x, (float)this.btnHighPos, this.mOK.transform.localPosition.z);
					}
					else if (GUIRollSceneV2.IsLowRollFree())
					{
						this.mAgainTxt.text = Singleton<StringManager>.Instance.GetString("rollAgain", new object[]
						{
							1
						});
						this.mAgainValue.enabled = false;
						this.mAgainFree.enabled = true;
						this.mAgainIcon.enabled = false;
						this.rollTimes = GUIRollingSceneV2.ERollTimes.ERollTimes_One;
					}
					else
					{
						this.mAgainTxt.text = Singleton<StringManager>.Instance.GetString("rollAgain", new object[]
						{
							1
						});
						this.mAgainValue.enabled = true;
						this.mAgainFree.enabled = false;
						this.mAgainIcon.enabled = true;
						this.mAgainValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
						{
							GameConst.GetInt32(37)
						});
						this.rollTimes = GUIRollingSceneV2.ERollTimes.ERollTimes_One;
						if (this.lowItemCount < GameConst.GetInt32(37))
						{
							this.mAgainValue.color = this.redCol;
						}
						else
						{
							this.mAgainValue.color = Color.white;
						}
					}
				}
			}
			else
			{
				this.mAgainTxt.text = Singleton<StringManager>.Instance.GetString("rollAgain", new object[]
				{
					1
				});
				this.mTitleName.transform.parent.gameObject.SetActive(true);
				this.mTitleName.text = Tools.GetPetName(this.mCurPetData.Info);
				this.mTitleName.applyGradient = false;
				this.mTitleName.color = Tools.GetItemQualityColor(this.mCurPetData.Info.Quality);
				this.mTitleElement.spriteName = Tools.GetPropertyIconWithBorder((EElementType)this.mCurPetData.Info.ElementType);
				this.mTitleType.spriteName = Tools.GetPetTypeIcon(this.mCurPetData.Info.Type);
				this.mTitleElement.enabled = true;
				this.mTitleType.enabled = true;
				this.mRollAgain.transform.localPosition = new Vector3(this.mRollAgain.transform.localPosition.x, (float)this.btnLowPos, this.mRollAgain.transform.localPosition.z);
				this.mOK.transform.localPosition = new Vector3(this.mOK.transform.localPosition.x, (float)this.btnLowPos, this.mOK.transform.localPosition.z);
				if (GUIRollSceneV2.IsLowRollFree())
				{
					this.mAgainFree.enabled = true;
					this.mAgainIcon.enabled = false;
					this.mAgainValue.enabled = false;
				}
				else
				{
					this.mAgainFree.enabled = false;
					this.mAgainIcon.enabled = true;
					this.mAgainValue.enabled = true;
					this.mAgainValue.text = Singleton<StringManager>.Instance.GetString("rollCost", new object[]
					{
						GameConst.GetInt32(37)
					});
					if (this.lowItemCount < GameConst.GetInt32(37))
					{
						this.mAgainValue.color = this.redCol;
					}
					else
					{
						this.mAgainValue.color = Color.white;
					}
				}
			}
		}
	}
}
