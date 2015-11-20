using Holoville.HOTween;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TopGoods : MonoBehaviour
{
	public enum EGoodsUIType
	{
		EGT_UIEnergy,
		EGT_UIStamina,
		EGT_UIMoney,
		EGT_UIDiamond,
		EGT_UIHonor,
		EGT_UIMedal,
		EGT_UIReputation,
		EGT_UIKingRewardMedal,
		EGT_UIMagicSoul,
		ERT_UIMax
	}

	public const int BIT_MOVE = 4;

	public UIEventListener.VoidDelegate BackClickListener;

	private bool eventRegister;

	private UISprite[] goodsItem = new UISprite[9];

	private UILabel[] goodsTxt = new UILabel[9];

	private int[] goodsOldNum = new int[9];

	private int maxEnergy;

	private int maxStamina;

	private UISprite UIChat;

	private GameObject mChatNewMark;

	private float timerRefresh;

	private GameObject mBackBtn;

	private UILabel mBackLabel;

	private List<TopGoods.EGoodsUIType> StopUpdateTypes = new List<TopGoods.EGoodsUIType>();

	public string BackLabelText
	{
		get
		{
			return this.mBackLabel.text;
		}
		set
		{
			this.mBackLabel.text = value;
		}
	}

	public bool SetVisible
	{
		set
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(value);
			}
		}
	}

	private void Awake()
	{
		Transform transform = base.transform.Find("UIMiddle");
		for (TopGoods.EGoodsUIType eGoodsUIType = TopGoods.EGoodsUIType.EGT_UIEnergy; eGoodsUIType < TopGoods.EGoodsUIType.ERT_UIMax; eGoodsUIType++)
		{
			this.goodsItem[(int)eGoodsUIType] = transform.Find(eGoodsUIType.ToString()).GetComponent<UISprite>();
			this.goodsTxt[(int)eGoodsUIType] = this.goodsItem[(int)eGoodsUIType].transform.Find("Label").GetComponent<UILabel>();
		}
		Transform transform2 = this.goodsTxt[0].transform.parent;
		UIEventListener expr_84 = UIEventListener.Get(transform2.gameObject);
		expr_84.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_84.onPress, new UIEventListener.BoolDelegate(this.PressEnergy));
		transform2 = transform2.Find("plusBtn");
		UIEventListener expr_BC = UIEventListener.Get(transform2.gameObject);
		expr_BC.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_BC.onClick, new UIEventListener.VoidDelegate(this.ClickEnergyPlus));
		transform2 = this.goodsTxt[2].transform.parent;
		Tools.SetParticleRenderQueue(transform2.gameObject, 3500, 1f);
		UIEventListener expr_110 = UIEventListener.Get(transform2.gameObject);
		expr_110.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_110.onClick, new UIEventListener.VoidDelegate(this.ClickGold));
		transform2 = this.goodsTxt[3].transform.parent;
		Tools.SetParticleRenderQueue(transform2.gameObject, 3500, 1f);
		UIEventListener expr_164 = UIEventListener.Get(transform2.gameObject);
		expr_164.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_164.onClick, new UIEventListener.VoidDelegate(this.ClickGem));
		transform2 = this.goodsTxt[1].transform.parent;
		UIEventListener expr_1A3 = UIEventListener.Get(transform2.gameObject);
		expr_1A3.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_1A3.onPress, new UIEventListener.BoolDelegate(this.PressStamina));
		transform2 = transform2.Find("plusBtn");
		UIEventListener expr_1DB = UIEventListener.Get(transform2.gameObject);
		expr_1DB.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1DB.onClick, new UIEventListener.VoidDelegate(this.OnClickStamina));
		transform2 = transform.Find("UIChat");
		this.UIChat = transform2.GetComponent<UISprite>();
		this.mChatNewMark = this.UIChat.transform.Find("new").gameObject;
		this.mChatNewMark.SetActive(false);
		UIEventListener expr_24B = UIEventListener.Get(transform2.gameObject);
		expr_24B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_24B.onClick, new UIEventListener.VoidDelegate(this.ClickChat));
		this.mBackBtn = GameUITools.FindGameObject("UIMiddle/UIBack", base.gameObject);
		this.mBackLabel = GameUITools.FindUILabel("Label", this.mBackBtn);
		UIEventListener expr_2A3 = UIEventListener.Get(this.mBackBtn);
		expr_2A3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2A3.onClick, new UIEventListener.VoidDelegate(this.OnBackClicked));
	}

	private long GetSlotMask(TopGoods.EGoodsUIType type, long slot)
	{
        return slot << (int)((int)type * (int)TopGoods.EGoodsUIType.EGT_UIHonor);
	}

	public void SetGoodsSlot(params TopGoods.EGoodsUIType[] arglist)
	{
		long num = 0L;
		for (int i = 0; i < arglist.Length; i++)
		{
			num |= this.GetSlotMask(arglist[i], (long)(i + 1));
		}
		for (TopGoods.EGoodsUIType eGoodsUIType = TopGoods.EGoodsUIType.EGT_UIEnergy; eGoodsUIType < TopGoods.EGoodsUIType.ERT_UIMax; eGoodsUIType++)
		{
			int num2 = (int)eGoodsUIType;
			long num3 = num & 15L;
			if (num3 != 0L)
			{
				this.goodsTxt[num2].transform.parent.gameObject.SetActive(true);
			}
			else
			{
				this.goodsTxt[num2].transform.parent.gameObject.SetActive(false);
			}
			num >>= 4;
		}
		float num4 = this.UIChat.transform.localPosition.x - 6f;
		for (int j = 0; j < arglist.Length; j++)
		{
			int num5 = (int)arglist[j];
			UISprite uISprite = this.goodsItem[num5];
			if (uISprite != null)
			{
				uISprite.transform.localPosition = new Vector3(num4 - (float)(uISprite.width / 2), -38f, 0f);
				num4 -= (float)uISprite.width;
				num4 -= 6f;
			}
		}
	}

	public void DefaultGoodsSlot()
	{
		TopGoods.EGoodsUIType[] expr_07 = new TopGoods.EGoodsUIType[4];
		expr_07[0] = TopGoods.EGoodsUIType.EGT_UIDiamond;
		expr_07[1] = TopGoods.EGoodsUIType.EGT_UIMoney;
		expr_07[2] = TopGoods.EGoodsUIType.EGT_UIStamina;
		this.SetGoodsSlot(expr_07);
	}

	public void Hide()
	{
		NGUITools.SetActive(base.gameObject, false);
		this.BackClickListener = null;
	}

	public void Show(string gobackBtnTextKey)
	{
		if (base.gameObject.activeInHierarchy)
		{
			return;
		}
		NGUITools.SetActive(base.gameObject, true);
		if (string.IsNullOrEmpty(gobackBtnTextKey))
		{
			this.mBackLabel.text = Singleton<StringManager>.Instance.GetString("Return");
		}
		else
		{
			this.mBackLabel.text = Singleton<StringManager>.Instance.GetString(gobackBtnTextKey);
		}
		this.OnPlayerUpdateEvent();
		if (!this.eventRegister)
		{
			this.eventRegister = true;
			LocalPlayer expr_7F = Globals.Instance.Player;
			expr_7F.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_7F.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		}
	}

	private void ClickGem(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIVip.OpenRecharge();
	}

	private void OnClickStamina(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Stamina);
	}

	private void ClickGold(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.Data.Level < 12u)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("d2mNeedLvl", 0f, 0f);
			return;
		}
		GameUIManager.mInstance.CreateSession<GUIAlchemy>(null);
	}

	private void PressEnergy(GameObject go, bool state)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameMessageBox.ShowEnergyTips(go, state, false);
	}

	private void PressStamina(GameObject go, bool state)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameMessageBox.ShowEnergyTips(go, state, true);
	}

	private void ClickEnergyPlus(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Energy);
	}

	private void ClickChat(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIChatWindowV2.TryShowMe();
	}

	public void OnPlayerUpdateEvent()
	{
		LocalPlayer player = Globals.Instance.Player;
		this.UpdateUIGoods(TopGoods.EGoodsUIType.EGT_UIEnergy, player.Data.Energy);
		this.UpdateUIGoods(TopGoods.EGoodsUIType.EGT_UIStamina, player.Data.Stamina);
		this.UpdateUIGoods(TopGoods.EGoodsUIType.EGT_UIMoney, player.Data.Money);
		this.UpdateUIGoods(TopGoods.EGoodsUIType.EGT_UIDiamond, player.Data.Diamond);
		this.UpdateUIGoods(TopGoods.EGoodsUIType.EGT_UIHonor, player.Data.Honor);
		this.UpdateUIGoods(TopGoods.EGoodsUIType.EGT_UIReputation, player.Data.Reputation);
		this.UpdateUIGoods(TopGoods.EGoodsUIType.EGT_UIKingRewardMedal, player.Data.KingMedal);
		this.UpdateUIGoods(TopGoods.EGoodsUIType.EGT_UIMagicSoul, player.Data.MagicSoul);
	}

	public void DisableUpdate(TopGoods.EGoodsUIType type)
	{
		this.StopUpdateTypes.Add(type);
	}

	public void EnableUpdate(TopGoods.EGoodsUIType type)
	{
		this.StopUpdateTypes.Remove(type);
	}

	public void UpdateUIEnergy(int value)
	{
		int num = Globals.Instance.Player.GetMaxEnergy();
		if (!this.eventRegister || this.goodsOldNum[0] != value || this.maxEnergy != num)
		{
			UILabel uILabel = this.goodsTxt[0];
			uILabel.text = string.Format("{0}{1}[-]/{2}", (value <= num) ? "[ffffff]" : "[00ff00]", Tools.FormatValue(value), num);
			if (this.goodsOldNum[0] != 0)
			{
				Sequence sequence = new Sequence();
				sequence.Append(HOTween.To(uILabel.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
				sequence.Append(HOTween.To(uILabel.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence.Play();
			}
			this.goodsOldNum[0] = value;
			this.maxEnergy = num;
		}
	}

	public void UpdateUIStamina(int value)
	{
		int num = Globals.Instance.Player.GetMaxStamina();
		if (!this.eventRegister || this.goodsOldNum[1] != value || this.maxStamina != num)
		{
			UILabel uILabel = this.goodsTxt[1];
			uILabel.text = string.Format("{0}{1}[-]/{2}", (value <= num) ? "[ffffff]" : "[00ff00]", Tools.FormatValue(value), num);
			if (this.goodsOldNum[1] != 0)
			{
				Sequence sequence = new Sequence();
				sequence.Append(HOTween.To(uILabel.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
				sequence.Append(HOTween.To(uILabel.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence.Play();
			}
			this.goodsOldNum[1] = value;
			this.maxStamina = num;
		}
	}

	public void UpdateUIGoods(TopGoods.EGoodsUIType type, int num)
	{
		if (this.StopUpdateTypes.Contains(type))
		{
			return;
		}
		if (type == TopGoods.EGoodsUIType.EGT_UIEnergy)
		{
			this.UpdateUIEnergy(num);
			return;
		}
		if (type == TopGoods.EGoodsUIType.EGT_UIStamina)
		{
			this.UpdateUIStamina(num);
			return;
		}
		if (!this.eventRegister || this.goodsOldNum[(int)type] != num)
		{
			UILabel uILabel = this.goodsTxt[(int)type];
			uILabel.text = Tools.FormatCurrency(num);
			if (this.goodsOldNum[(int)type] != 0)
			{
				Sequence sequence = new Sequence();
				sequence.Append(HOTween.To(uILabel.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", new Vector3(1.5f, 1.5f, 1.5f))));
				sequence.Append(HOTween.To(uILabel.gameObject.transform, 0.15f, new TweenParms().Prop("localScale", Vector3.one)));
				sequence.Play();
			}
			this.goodsOldNum[(int)type] = num;
		}
	}

	private void RefreshChatBtn()
	{
		this.mChatNewMark.SetActive(Globals.Instance.Player.ShowChatBtnAnim);
	}

	private void Update()
	{
		if (Time.time - this.timerRefresh > 0.5f && Globals.Instance && Globals.Instance.Player != null)
		{
			this.timerRefresh = Time.time;
			this.RefreshChatBtn();
		}
	}

	public void OnBackClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		if (this.BackClickListener != null)
		{
			UIEventListener.VoidDelegate backClickListener = this.BackClickListener;
			this.BackClickListener = null;
			backClickListener(go);
			return;
		}
		GameUIManager.mInstance.GobackSession();
	}
}
