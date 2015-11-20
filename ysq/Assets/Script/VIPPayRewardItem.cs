using System;
using UnityEngine;

public class VIPPayRewardItem : UICustomGridItem
{
	private UILabel Title;

	private UILabel Price;

	private UILabel OffPrice;

	private Transform Reward;

	private GameObject[] RewardItem = new GameObject[4];

	private UILabel step;

	private GameObject GoBtn;

	public VIPRewardData VipData
	{
		get;
		private set;
	}

	public void Init()
	{
		this.Title = base.transform.FindChild("Title").GetComponent<UILabel>();
		this.Price = base.transform.FindChild("Price").GetComponent<UILabel>();
		this.OffPrice = base.transform.FindChild("OffPrice").GetComponent<UILabel>();
		this.Reward = base.transform.FindChild("pricebk/Reward");
		this.step = base.transform.FindChild("step").GetComponent<UILabel>();
		this.GoBtn = base.transform.FindChild("GoBtn").gameObject;
		UIEventListener expr_A8 = UIEventListener.Get(this.GoBtn);
		expr_A8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A8.onClick, new UIEventListener.VoidDelegate(this.OnGoBtnClicked));
	}

	public override void Refresh(object _data)
	{
		if (this.VipData == _data)
		{
			return;
		}
		this.VipData = (VIPRewardData)_data;
		this.Title.text = this.VipData.GetPayRewardTitle();
		this.step.text = Singleton<StringManager>.Instance.GetString("VIPDes14", new object[]
		{
			this.VipData.GetVipLevel()
		});
		this.Price.text = this.VipData.VipInfo.Price.ToString();
		this.OffPrice.text = this.VipData.VipInfo.OffPrice.ToString();
		for (int i = 0; i < this.RewardItem.Length; i++)
		{
			if (this.RewardItem[i] != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem[i]);
				this.RewardItem[i] = null;
			}
		}
		int num = 0;
		for (int j = 0; j < this.VipData.VipInfo.RewardType.Count; j++)
		{
			if (this.VipData.VipInfo.RewardType[j] != 0 && this.VipData.VipInfo.RewardType[j] != 20)
			{
				this.RewardItem[num] = GameUITools.CreateReward(this.VipData.VipInfo.RewardType[j], this.VipData.VipInfo.RewardValue1[j], this.VipData.VipInfo.RewardValue2[j], this.Reward, true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
				if (this.RewardItem[num] != null)
				{
					this.RewardItem[num].gameObject.AddComponent<UIDragScrollView>();
					this.RewardItem[num].transform.localPosition = new Vector3((float)(num * 106), 0f, 0f);
					num++;
				}
				if (num >= this.RewardItem.Length)
				{
					break;
				}
			}
		}
	}

	private void OnGoBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.VipData == null)
		{
			return;
		}
		UIVIPGiftPacks.RequestBuyVipReward(this.VipData.VipInfo);
	}
}
