using Proto;
using System;
using UnityEngine;

public class GUIRewardFlashSaleItem : MonoBehaviour
{
	private GUIRewardFlashSaleInfo mBaseScene;

	private int slot;

	private int count;

	private int curCost;

	private string detail;

	private UILabel mTitle;

	private UIButton mBuyBtn;

	private UILabel mOriginalCost;

	private UILabel mMoney;

	private UILabel mTimes;

	private GameUIToolTip mToolTip;

	private void Start()
	{
		this.mTitle = GameUITools.FindUILabel("Title", base.gameObject);
		GameUITools.RegisterPressEvent("Chest", new UIEventListener.BoolDelegate(this.OnChestPress), base.gameObject);
		this.mBuyBtn = GameUITools.RegisterClickEvent("BuyBtn", new UIEventListener.VoidDelegate(this.OnBuyBtnClick), base.gameObject).GetComponent<UIButton>();
		this.mMoney = GameUITools.FindUILabel("Cost", base.gameObject);
		this.mOriginalCost = GameUITools.FindUILabel("OriginalCost", base.gameObject);
		this.mTimes = GameUITools.FindUILabel("Times", base.gameObject);
	}

	public void InitData(GUIRewardFlashSaleInfo basescene, int slot, string name, string detail, int preCost, int curCost, int count)
	{
		this.mBaseScene = basescene;
		this.slot = slot;
		this.curCost = curCost;
		this.detail = detail;
		this.mMoney.text = curCost.ToString();
		this.mOriginalCost.text = preCost.ToString();
		this.mTitle.text = name;
		this.Refresh(count);
	}

	public void Refresh(int count)
	{
		this.count = count;
		if (Globals.Instance.Player.Data.Diamond >= this.curCost)
		{
			this.mMoney.color = Color.white;
		}
		else
		{
			this.mMoney.color = Color.red;
		}
		if (count >= 0)
		{
			this.mTimes.text = Singleton<StringManager>.Instance.GetString("activityFlashSaleTimes", new object[]
			{
				count
			});
		}
		else
		{
			this.mTimes.enabled = false;
		}
		UIButton[] components = this.mBuyBtn.GetComponents<UIButton>();
		if (count == 0)
		{
			this.mBuyBtn.isEnabled = false;
		}
		else
		{
			this.mBuyBtn.isEnabled = true;
		}
		for (int i = 0; i < components.Length; i++)
		{
			components[i].SetState((!this.mBuyBtn.isEnabled) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
		}
	}

	private void OnBuyBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.count == 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("activityFlashSaleItemOver", 0f, 0f);
			return;
		}
		if (!Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, this.curCost, 0))
		{
			this.mBaseScene.SendStartFlashSale2Server(this.slot);
		}
	}

	private void OnChestPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			if (this.mToolTip == null)
			{
				this.mToolTip = GameUIToolTipManager.GetInstance().CreateBasicTooltip(go.transform, string.Empty, string.Empty);
			}
			this.mToolTip.Create(go.transform, this.mTitle.text, this.detail);
			this.mToolTip.EnableToolTip();
			if (this.slot == 2)
			{
				this.mToolTip.transform.localPosition = new Vector3(this.mToolTip.transform.localPosition.x - 65f, this.mToolTip.transform.localPosition.y, this.mToolTip.transform.localPosition.z);
			}
		}
		else if (this.mToolTip != null)
		{
			this.mToolTip.HideTipAnim();
		}
	}
}
