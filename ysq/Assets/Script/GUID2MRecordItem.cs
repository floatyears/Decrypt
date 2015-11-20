using System;
using UnityEngine;

public class GUID2MRecordItem : UICustomGridItem
{
	private UILabel mDiamond;

	private UILabel mMoney;

	private UILabel mCriDesc;

	public GUID2MRecordData mRecordData
	{
		get;
		private set;
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mDiamond = base.transform.Find("Diamond/Label").GetComponent<UILabel>();
		this.mMoney = base.transform.Find("Gold/Label").GetComponent<UILabel>();
		this.mCriDesc = base.transform.Find("Txt3").GetComponent<UILabel>();
	}

	public override void Refresh(object data)
	{
		if (this.mRecordData == data)
		{
			return;
		}
		this.mRecordData = (GUID2MRecordData)data;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.mRecordData == null)
		{
			return;
		}
		this.mDiamond.text = this.mRecordData.mData.Diamond.ToString();
		this.mMoney.text = this.mRecordData.mData.Money.ToString();
		if (this.mRecordData.mData.Crit > 1)
		{
			this.mCriDesc.text = Singleton<StringManager>.Instance.GetString("d2mCrit", new object[]
			{
				this.mRecordData.mData.Crit
			});
			int crit = this.mRecordData.mData.Crit;
			switch (crit)
			{
			case 2:
			case 3:
				this.mCriDesc.color = new Color(0.8862745f, 0.2509804f, 1f);
				goto IL_166;
			case 4:
				IL_C9:
				if (crit != 10)
				{
					this.mCriDesc.color = new Color(1f, 0.917647064f, 0f);
					goto IL_166;
				}
				this.mCriDesc.color = new Color(1f, 0.917647064f, 0f);
				goto IL_166;
			case 5:
				this.mCriDesc.color = new Color(1f, 0.168627456f, 0.168627456f);
				goto IL_166;
			}
			goto IL_C9;
			IL_166:
			this.mCriDesc.gameObject.SetActive(true);
		}
		else
		{
			this.mCriDesc.gameObject.SetActive(false);
		}
	}
}
