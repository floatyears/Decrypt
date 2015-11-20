using Att;
using System;
using System.Text;
using UnityEngine;

public class GUIVIPLevelUpPopUp : GameUIBasePopup
{
	private VipLevelInfo vipLevelInfo;

	private UILabel title;

	private UILabel desc;

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.title = base.transform.FindChild("TitleBg_L/Label").GetComponent<UILabel>();
		this.desc = base.transform.FindChild("Label").GetComponent<UILabel>();
		GameObject gameObject = base.transform.Find("CloseBtn").gameObject;
		UIEventListener expr_52 = UIEventListener.Get(gameObject);
		expr_52.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_52.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		Tools.SetParticleRQWithUIScale(GameUITools.FindGameObject("ui23", base.gameObject), 5500);
	}

	public override void InitPopUp(int vipLevel)
	{
		this.Refresh(Globals.Instance.AttDB.VipLevelDict.GetInfo(vipLevel));
	}

	public void Refresh(VipLevelInfo viplevel)
	{
		this.vipLevelInfo = viplevel;
		if (this.vipLevelInfo == null)
		{
			this.desc.text = string.Empty;
			return;
		}
		VipLevelInfo info = Globals.Instance.AttDB.VipLevelDict.GetInfo(viplevel.ID - 1);
		this.title.text = string.Format(Singleton<StringManager>.Instance.GetString("VIPDes13"), new object[0]);
		StringBuilder stringBuilder = new StringBuilder();
		if (this.vipLevelInfo.SceneResetCount > 0 && (info == null || this.vipLevelInfo.SceneResetCount != info.SceneResetCount))
		{
			stringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("VIPDes7"), this.vipLevelInfo.SceneResetCount);
			stringBuilder.AppendLine();
		}
		if (this.vipLevelInfo.ShopCommon2Count > 0 && (info == null || this.vipLevelInfo.ShopCommon2Count != info.ShopCommon2Count))
		{
			stringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("VIPDes8"), this.vipLevelInfo.ShopCommon2Count);
			stringBuilder.AppendLine();
			if (Tools.CanPlay(GameConst.GetInt32(24), true))
			{
				stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPDes3", new object[]
				{
					this.vipLevelInfo.ShopCommon2Count
				}));
			}
			if (Tools.CanPlay(GameConst.GetInt32(201), true))
			{
				stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPDes17", new object[]
				{
					this.vipLevelInfo.ShopLopetCount
				}));
			}
		}
		if (this.vipLevelInfo.BuyCount[1] > 0 && (info == null || this.vipLevelInfo.BuyCount[1] != info.BuyCount[1]))
		{
			stringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("VIPDes5"), this.vipLevelInfo.BuyCount[1]);
			stringBuilder.AppendLine();
		}
		if (this.vipLevelInfo.BuyCount[0] > 0 && (info == null || this.vipLevelInfo.BuyCount[0] != info.BuyCount[0]))
		{
			stringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("VIPDes4"), this.vipLevelInfo.BuyCount[0]);
			stringBuilder.AppendLine();
		}
		if (this.vipLevelInfo.BuyCount[2] > 0 && (info == null || this.vipLevelInfo.BuyCount[2] != info.BuyCount[2]))
		{
			stringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("VIPDes11"), this.vipLevelInfo.BuyCount[2]);
			stringBuilder.AppendLine();
		}
		if (this.vipLevelInfo.D2MCount > 0 && (info == null || this.vipLevelInfo.D2MCount != info.D2MCount))
		{
			stringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("VIPDes6"), this.vipLevelInfo.D2MCount);
			stringBuilder.AppendLine();
		}
		if (this.vipLevelInfo.BuyCount[3] > 0 && (info == null || this.vipLevelInfo.BuyCount[3] != info.BuyCount[3]))
		{
			stringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("VIPDes9"), this.vipLevelInfo.BuyCount[3]);
			stringBuilder.AppendLine();
		}
		if (this.vipLevelInfo.BuyCount[4] > 0 && (info == null || this.vipLevelInfo.BuyCount[4] != info.BuyCount[4]))
		{
			stringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("VIPDes10"), this.vipLevelInfo.BuyCount[4]);
			stringBuilder.AppendLine();
		}
		if (this.vipLevelInfo.ScratchOff > 0 && (info == null || this.vipLevelInfo.ScratchOff != info.ScratchOff))
		{
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPDes12", new object[]
			{
				this.vipLevelInfo.ScratchOff
			}));
		}
		if (this.vipLevelInfo.BuyPillageCount > 0 && (info == null || this.vipLevelInfo.BuyPillageCount != info.BuyPillageCount))
		{
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPDes15", new object[]
			{
				this.vipLevelInfo.BuyPillageCount
			}));
		}
		if (this.vipLevelInfo.BuyRevengeCount > 0 && (info == null || this.vipLevelInfo.BuyRevengeCount != info.BuyRevengeCount))
		{
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPDes16", new object[]
			{
				this.vipLevelInfo.BuyRevengeCount
			}));
		}
		switch (this.vipLevelInfo.ID)
		{
		case 1:
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPLevelDes0"));
			break;
		case 2:
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPLevelDes1"));
			break;
		case 3:
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPLevelDes2"));
			break;
		case 4:
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPLevelDes3"));
			break;
		case 5:
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPLevelDes4"));
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPLevelDes11"));
			break;
		case 6:
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPLevelDes5"));
			break;
		case 7:
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPLevelDes6"));
			break;
		case 9:
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPLevelDes8"));
			break;
		case 10:
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPLevelDes9"));
			break;
		case 12:
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("VIPLevelDes10"));
			break;
		}
		stringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("VIPDes19"), this.vipLevelInfo.ID);
		this.desc.text = stringBuilder.ToString();
	}

	private void OnCloseBtnClick(GameObject go)
	{
		base.OnButtonBlockerClick();
	}
}
