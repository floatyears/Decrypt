using Att;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public sealed class UIVIPDesc : MonoBehaviour
{
	private VipLevelInfo vipLevelInfo;

	private UILabel title;

	private UILabel desc0;

	private UILabel desc1;

	private Transform desc2;

	private UISprite mBG;

	private List<UIVIPDescItem> desc2Items = new List<UIVIPDescItem>();

	private StringBuilder tempSb = new StringBuilder();

	private void Awake()
	{
		this.title = base.transform.FindChild("Title").GetComponent<UILabel>();
		this.desc0 = base.transform.FindChild("Desc0").GetComponent<UILabel>();
		this.desc1 = base.transform.FindChild("Desc1").GetComponent<UILabel>();
		this.mBG = base.transform.FindChild("BG").GetComponent<UISprite>();
	}

	public void Refresh(VipLevelInfo viplevel)
	{
		if (this.desc2 == null)
		{
			this.desc2 = ((GameUIVip)GameUIManager.mInstance.CurUISession).mDesc2;
			this.InitDesc2();
		}
		this.vipLevelInfo = viplevel;
		this.desc0.text = string.Empty;
		this.desc1.text = string.Empty;
		if (this.vipLevelInfo == null)
		{
			return;
		}
		VipLevelInfo info = Globals.Instance.AttDB.VipLevelDict.GetInfo(viplevel.ID - 1);
		this.title.text = Singleton<StringManager>.Instance.GetString("VIPDes1", new object[]
		{
			this.vipLevelInfo.TotalPay,
			viplevel.ID
		});
		if (this.vipLevelInfo.SceneResetCount > 0 && (info == null || this.vipLevelInfo.SceneResetCount != info.SceneResetCount))
		{
			this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes7", new object[]
			{
				this.vipLevelInfo.SceneResetCount
			}), false);
		}
		if (this.vipLevelInfo.ShopCommon2Count > 0 && (info == null || this.vipLevelInfo.ShopCommon2Count != info.ShopCommon2Count))
		{
			this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes8", new object[]
			{
				this.vipLevelInfo.ShopCommon2Count
			}), false);
			if (Tools.CanPlay(GameConst.GetInt32(24), true))
			{
				this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes3", new object[]
				{
					this.vipLevelInfo.ShopCommon2Count
				}), false);
			}
			if (Tools.CanPlay(GameConst.GetInt32(201), true))
			{
				this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes17", new object[]
				{
					this.vipLevelInfo.ShopLopetCount
				}), false);
			}
		}
		if (this.vipLevelInfo.BuyCount[1] > 0 && (info == null || this.vipLevelInfo.BuyCount[1] != info.BuyCount[1]))
		{
			this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes5", new object[]
			{
				this.vipLevelInfo.BuyCount[1]
			}), false);
		}
		if (this.vipLevelInfo.BuyCount[0] > 0 && (info == null || this.vipLevelInfo.BuyCount[0] != info.BuyCount[0]))
		{
			this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes4", new object[]
			{
				this.vipLevelInfo.BuyCount[0]
			}), false);
		}
		if (this.vipLevelInfo.BuyCount[2] > 0 && (info == null || this.vipLevelInfo.BuyCount[2] != info.BuyCount[2]))
		{
			this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes11", new object[]
			{
				this.vipLevelInfo.BuyCount[2]
			}), false);
		}
		if (this.vipLevelInfo.D2MCount > 0 && (info == null || this.vipLevelInfo.D2MCount != info.D2MCount))
		{
			this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes6", new object[]
			{
				this.vipLevelInfo.D2MCount
			}), false);
		}
		if (this.vipLevelInfo.BuyCount[3] > 0 && (info == null || this.vipLevelInfo.BuyCount[3] != info.BuyCount[3]))
		{
			this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes9", new object[]
			{
				this.vipLevelInfo.BuyCount[3]
			}), false);
		}
		if (this.vipLevelInfo.BuyCount[4] > 0 && (info == null || this.vipLevelInfo.BuyCount[4] != info.BuyCount[4]))
		{
			this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes10", new object[]
			{
				this.vipLevelInfo.BuyCount[4]
			}), false);
		}
		if (this.vipLevelInfo.ScratchOff > 0)
		{
			this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes12", new object[]
			{
				this.vipLevelInfo.ScratchOff - Globals.Instance.AttDB.VipLevelDict.GetInfo(16).ScratchOff
			}), false);
		}
		if (this.vipLevelInfo.BuyPillageCount > 0)
		{
			this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes15", new object[]
			{
				viplevel.BuyPillageCount
			}), false);
		}
		if (viplevel.BuyRevengeCount > 0)
		{
			this.AddDesc(Singleton<StringManager>.Instance.GetString("VIPDes16", new object[]
			{
				viplevel.BuyRevengeCount
			}), false);
		}
		this.AddDescEnd();
		this.RefreshDesc2();
		if (this.desc0.height < this.desc1.height)
		{
			this.mBG.bottomAnchor.target = this.desc1.transform;
			this.mBG.topAnchor.target = this.desc1.transform;
			this.mBG.bottomAnchor.absolute = 26;
			this.mBG.topAnchor.absolute = 10;
		}
	}

	private void AddDesc(string str, bool end = false)
	{
		this.tempSb.AppendLine(str);
		int num = this.tempSb.ToString().Trim().Split(new char[]
		{
			'\n'
		}).Length;
		int num2 = 6;
		if (Tools.CanPlay(GameConst.GetInt32(201), true))
		{
			num2 = 7;
		}
		if (num == num2 && string.IsNullOrEmpty(this.desc0.text))
		{
			this.desc0.text = this.tempSb.ToString();
			this.tempSb.Remove(0, this.tempSb.Length);
		}
	}

	private void AddDescEnd()
	{
		if (string.IsNullOrEmpty(this.desc0.text))
		{
			this.desc0.text = this.tempSb.ToString();
		}
		else
		{
			this.desc1.text = this.tempSb.ToString();
		}
		this.tempSb.Remove(0, this.tempSb.Length);
	}

	private void InitDesc2()
	{
		this.InitDesc2Item(11);
		this.desc2Items[0].Init(1, Singleton<StringManager>.Instance.GetString("VIPLevelDes0"));
		this.desc2Items[1].Init(2, Singleton<StringManager>.Instance.GetString("VIPLevelDes1"));
		this.desc2Items[2].Init(3, Singleton<StringManager>.Instance.GetString("VIPLevelDes2"));
		this.desc2Items[3].Init(4, Singleton<StringManager>.Instance.GetString("VIPLevelDes3"));
		this.desc2Items[4].Init(5, Singleton<StringManager>.Instance.GetString("VIPLevelDes4"));
		this.desc2Items[5].Init(5, Singleton<StringManager>.Instance.GetString("VIPLevelDes11"));
		this.desc2Items[6].Init(6, Singleton<StringManager>.Instance.GetString("VIPLevelDes5"));
		this.desc2Items[7].Init(7, Singleton<StringManager>.Instance.GetString("VIPLevelDes6"));
		this.desc2Items[8].Init(9, Singleton<StringManager>.Instance.GetString("VIPLevelDes8"));
		this.desc2Items[9].Init(10, Singleton<StringManager>.Instance.GetString("VIPLevelDes9"));
		this.desc2Items[10].Init(12, Singleton<StringManager>.Instance.GetString("VIPLevelDes10"));
	}

	private void InitDesc2Item(int count)
	{
		this.desc2Items.Clear();
		GameObject gameObject = Res.LoadGUI("GUI/UIVIPDescItem");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.LoadGUI GUI/UIVIPDescItem error"
			});
		}
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
			gameObject2.SetActive(true);
			GameUITools.AddChild(this.desc2.gameObject, gameObject2);
			gameObject2.transform.localPosition = new Vector3((float)(-290 + i % 3 * 290), (float)(55 - i / 3 * 37), 0f);
			gameObject2.transform.localScale = Vector3.one;
			this.desc2Items.Add(gameObject2.AddComponent<UIVIPDescItem>());
		}
	}

	private void RefreshDesc2()
	{
		foreach (UIVIPDescItem current in this.desc2Items)
		{
			current.Refresh((int)Globals.Instance.Player.Data.VipLevel);
		}
	}
}
