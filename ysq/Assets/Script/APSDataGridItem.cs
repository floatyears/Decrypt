using System;
using UnityEngine;

public class APSDataGridItem : UICustomGridItem
{
	private APItemDataEx Data;

	protected UISprite bg;

	protected UILabel Title;

	private UILabel Price;

	private UILabel OffPrice;

	protected Transform Reward;

	protected GameObject[] RewardItem = new GameObject[4];

	protected GameObject GoBtn;

	protected GameObject BuyBtn;

	protected GameObject finished;

	protected UILabel step;

	public void Init()
	{
		this.bg = base.transform.GetComponent<UISprite>();
		this.Title = base.transform.FindChild("Title").GetComponent<UILabel>();
		this.Price = base.transform.Find("Price").GetComponent<UILabel>();
		this.OffPrice = base.transform.Find("OffPrice").GetComponent<UILabel>();
		this.Reward = base.transform.Find("pricebk/Reward");
		this.GoBtn = base.transform.FindChild("GoBtn").gameObject;
		this.BuyBtn = base.transform.FindChild("BuyBtn").gameObject;
		this.finished = base.transform.FindChild("finished").gameObject;
		this.step = base.transform.FindChild("step").GetComponent<UILabel>();
		UIEventListener expr_EF = UIEventListener.Get(this.GoBtn);
		expr_EF.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_EF.onClick, new UIEventListener.VoidDelegate(this.OnGoBtnClicked));
		UIEventListener expr_11B = UIEventListener.Get(this.BuyBtn);
		expr_11B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_11B.onClick, new UIEventListener.VoidDelegate(this.OnBuyBtnClicked));
	}

	private void OnGoBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIVip.OpenRecharge();
	}

	private void OnBuyBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.Data == null)
		{
			return;
		}
		this.Data.Buy();
	}

	public override void Refresh(object _data)
	{
		if (_data == this.Data)
		{
			this.RefreshFinnishState();
			return;
		}
		this.Data = (APItemDataEx)_data;
		this.RefreshData();
	}

	private void RefreshData()
	{
		if (this.Data == null || this.Data.APData == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.Title.text = this.Data.GetTitle();
		this.Price.text = this.Data.APData.Price.ToString();
		this.OffPrice.text = this.Data.APData.OffPrice.ToString();
		float num = 106f;
		int num2 = 0;
		for (int i = 0; i < this.RewardItem.Length; i++)
		{
			if (this.RewardItem[i] != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem[i]);
				this.RewardItem[i] = null;
			}
		}
		for (int j = 0; j < this.Data.APData.Data.Count; j++)
		{
			if (this.Data.APData.Data[j].RewardType != 0 && this.Data.APData.Data[j].RewardType != 20)
			{
				this.RewardItem[num2] = GameUITools.CreateReward(this.Data.APData.Data[j].RewardType, this.Data.APData.Data[j].RewardValue1, this.Data.APData.Data[j].RewardValue2, this.Reward, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
				if (this.RewardItem[num2] != null)
				{
					this.RewardItem[num2].gameObject.AddComponent<UIDragScrollView>();
					this.RewardItem[num2].transform.localPosition = new Vector3((float)num2 * num, 0f, 0f);
					num2++;
				}
				if (num2 >= this.RewardItem.Length)
				{
					break;
				}
			}
		}
		this.RefreshFinnishState();
		base.gameObject.SetActive(true);
	}

	private void RefreshFinnishState()
	{
		if (this.Data.IsComplete())
		{
			this.GoBtn.SetActive(false);
			if (this.Data.BuyCount() <= 0)
			{
				this.bg.spriteName = "Price_bg";
				this.BuyBtn.SetActive(false);
				this.finished.SetActive(true);
			}
			else
			{
				this.bg.spriteName = "gold_bg";
				this.BuyBtn.SetActive(true);
				this.finished.SetActive(false);
			}
		}
		else
		{
			this.bg.spriteName = "gold_bg";
			this.GoBtn.SetActive(true);
			this.BuyBtn.SetActive(false);
			this.finished.SetActive(false);
		}
		if (this.Data.APData.Value < 0)
		{
			this.step.gameObject.SetActive(false);
		}
		else
		{
			this.step.gameObject.SetActive(true);
			this.step.text = string.Format("{0} {1}/{2}", Singleton<StringManager>.Instance.GetString("QuestProgress"), Tools.FormatValue(Mathf.Min(this.Data.APData.Value, this.Data.APSD.PayDay)), Tools.FormatValue(this.Data.APData.Value));
		}
	}
}
