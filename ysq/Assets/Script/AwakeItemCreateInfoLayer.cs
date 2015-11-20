using Att;
using Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class AwakeItemCreateInfoLayer : MonoBehaviour
{
	private AwakeItemDetailLayer mBaseLayer;

	private ItemInfo mItemInfo;

	private ItemInfo mParentItemInfo;

	private UILabel mName;

	private CommonIconItem mIcon;

	private GameObject mUI53;

	private UISprite mTreeBG;

	private UISprite mLeftBranch;

	private UISprite mRightBranch;

	private List<CommonIconItem> mLeafIcons = new List<CommonIconItem>();

	private UILabel mCoseValue;

	private int createCost;

	public void Init(AwakeItemDetailLayer baselayer)
	{
		this.mBaseLayer = baselayer;
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mUI53 = GameUITools.FindGameObject("ui53", base.gameObject);
		Tools.SetParticleRQWithUIScale(this.mUI53, 5500);
		this.mUI53.gameObject.SetActive(false);
		this.mIcon = CommonIconItem.Create(base.gameObject, new Vector3(-43f, 66f, 0f), null, false, 0.9f, null);
		this.mTreeBG = GameUITools.FindUISprite("TreeBG", base.gameObject);
		this.mLeftBranch = GameUITools.FindUISprite("LeftBranch", this.mTreeBG.gameObject);
		this.mRightBranch = GameUITools.FindUISprite("RightBranch", this.mTreeBG.gameObject);
		for (int i = 0; i < 4; i++)
		{
			this.mLeafIcons.Add(CommonIconItem.Create(base.gameObject, new Vector3(0f, 0f, 0f), null, true, 0.5f, null).SetNumStyle(30));
			this.mLeafIcons[i].OnItemIconClickEvent = new CommonIconItem.ItemInfoCallBack(this.OnLeafIconClick);
		}
		this.mCoseValue = GameUITools.FindUILabel("Cost/Value", base.gameObject);
		GameUITools.RegisterClickEvent("Create", new UIEventListener.VoidDelegate(this.OnCreateClick), base.gameObject);
		Globals.Instance.CliSession.Register(537, new ClientSession.MsgHandler(this.OnMsgAwakeItemCreate));
		LocalPlayer expr_192 = Globals.Instance.Player;
		expr_192.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_192.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		Globals.Instance.CliSession.Unregister(537, new ClientSession.MsgHandler(this.OnMsgAwakeItemCreate));
		LocalPlayer expr_3B = Globals.Instance.Player;
		expr_3B.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_3B.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
	}

	private void OnLeafIconClick(ItemInfo info)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseLayer.Refresh(info, this.mItemInfo, false);
	}

	private void OnCreateClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Money, this.createCost, 0))
		{
			return;
		}
		if (!Globals.Instance.Player.ItemSystem.EnoughItem2CreateAwakeItem(this.mItemInfo.ID, 1, false))
		{
			GameUIManager.mInstance.ShowMessageTipByKey("awakeItemCreateError0", 0f, 0f);
			return;
		}
		MC2S_AwakeItemCreate mC2S_AwakeItemCreate = new MC2S_AwakeItemCreate();
		mC2S_AwakeItemCreate.InfoID = this.mItemInfo.ID;
		Globals.Instance.CliSession.Send(536, mC2S_AwakeItemCreate);
	}

	private void OnMsgAwakeItemCreate(MemoryStream stream)
	{
		base.StartCoroutine(this.PlayCreateAnim());
	}

	[DebuggerHidden]
	private IEnumerator PlayCreateAnim()
	{
        return null;
        //AwakeItemCreateInfoLayer.<PlayCreateAnim>c__Iterator41 <PlayCreateAnim>c__Iterator = new AwakeItemCreateInfoLayer.<PlayCreateAnim>c__Iterator41();
        //<PlayCreateAnim>c__Iterator.<>f__this = this;
        //return <PlayCreateAnim>c__Iterator;
	}

	public void Refresh(ItemInfo info, ItemInfo parentInfo)
	{
		if (info == null)
		{
			global::Debug.LogError(new object[]
			{
				"ItemInfo is null"
			});
			return;
		}
		this.mItemInfo = info;
		this.mParentItemInfo = parentInfo;
		NGUITools.SetActive(this.mUI53, false);
		this.Refresh();
	}

	private void Refresh()
	{
		this.mName.text = this.mItemInfo.Name;
		this.mIcon.Refresh(this.mItemInfo, false, false, false);
		int needNum = 1;
		AwakeRecipeInfo info;
		if (this.mParentItemInfo != null)
		{
			info = Globals.Instance.AttDB.AwakeRecipeDict.GetInfo(this.mParentItemInfo.ID);
			if (info == null)
			{
				global::Debug.LogErrorFormat("AwakeRecipeDict get info error , ID : {0} ", new object[]
				{
					this.mItemInfo.ID
				});
			}
			else
			{
				for (int i = 0; i < info.ItemID.Count; i++)
				{
					if (info.ItemID[i] == this.mItemInfo.ID)
					{
						needNum = info.Count[i];
						break;
					}
				}
			}
		}
		this.mIcon.SetNeedNum(Globals.Instance.Player.ItemSystem.GetItemCount(this.mItemInfo.ID), needNum);
		info = Globals.Instance.AttDB.AwakeRecipeDict.GetInfo(this.mItemInfo.ID);
		if (info == null)
		{
			global::Debug.LogErrorFormat("AwakeRecipeDict get info error , ID :{0} ", new object[]
			{
				this.mItemInfo.ID
			});
			return;
		}
		int num = 0;
		int num2 = 0;
		while (num2 < info.ItemID.Count && num2 < this.mLeafIcons.Count)
		{
			if (info.ItemID[num2] != 0 && info.Count[num2] > 0)
			{
				ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(info.ItemID[num2]);
				if (info2 == null)
				{
					global::Debug.LogErrorFormat("ItemDict get info error , ID : {0} ", new object[]
					{
						info.ItemID[num2]
					});
					return;
				}
				this.mLeafIcons[num2].Refresh(info2, false, false, false);
				this.mLeafIcons[num2].SetNeedNum(Globals.Instance.Player.ItemSystem.GetItemCount(info.ItemID[num2]), info.Count[num2]);
				num++;
			}
			num2++;
		}
		foreach (CommonIconItem current in this.mLeafIcons)
		{
			current.gameObject.SetActive(false);
		}
		this.mLeftBranch.enabled = false;
		this.mRightBranch.enabled = false;
		switch (num)
		{
		case 2:
			this.mLeafIcons[0].transform.localPosition = new Vector3(-103f, -61f, 0f);
			this.mLeafIcons[1].transform.localPosition = new Vector3(55f, -61f, 0f);
			for (int j = 0; j < num; j++)
			{
				this.mLeafIcons[j].gameObject.SetActive(true);
			}
			this.mTreeBG.width = 160;
			goto IL_5BF;
		case 3:
			this.mLeafIcons[0].transform.localPosition = new Vector3(-123f, -61f, 0f);
			this.mLeafIcons[1].transform.localPosition = new Vector3(-24f, -61f, 0f);
			this.mLeafIcons[2].transform.localPosition = new Vector3(75f, -61f, 0f);
			for (int k = 0; k < num; k++)
			{
				this.mLeafIcons[k].gameObject.SetActive(true);
			}
			this.mTreeBG.width = 200;
			this.mLeftBranch.enabled = true;
			this.mLeftBranch.transform.localPosition = new Vector3(0.5f, -14f, 0f);
			goto IL_5BF;
		case 4:
			this.mLeafIcons[0].transform.localPosition = new Vector3(-143f, -61f, 0f);
			this.mLeafIcons[1].transform.localPosition = new Vector3(-64f, -61f, 0f);
			this.mLeafIcons[2].transform.localPosition = new Vector3(15f, -61f, 0f);
			this.mLeafIcons[3].transform.localPosition = new Vector3(95f, -61f, 0f);
			for (int l = 0; l < num; l++)
			{
				this.mLeafIcons[l].gameObject.SetActive(true);
			}
			this.mTreeBG.width = 244;
			this.mLeftBranch.enabled = true;
			this.mRightBranch.enabled = true;
			this.mLeftBranch.transform.localPosition = new Vector3(-40f, -14f, 0f);
			this.mRightBranch.transform.localPosition = new Vector3(40f, -14f, 0f);
			goto IL_5BF;
		}
		global::Debug.LogErrorFormat("Create AwakeItem items num error , {0}", new object[]
		{
			num
		});
		IL_5BF:
		this.RefreshCost();
	}

	private void RefreshCost()
	{
		QualityInfo info = Globals.Instance.AttDB.QualityDict.GetInfo(this.mItemInfo.Quality + 1);
		if (info == null)
		{
			global::Debug.LogErrorFormat("QualityInfo get info error , ID : {0}", new object[]
			{
				this.mItemInfo.Quality + 1
			});
			return;
		}
		this.createCost = info.AwakeCreateMoney;
		this.mCoseValue.text = this.createCost.ToString();
		if (Globals.Instance.Player.Data.Money < this.createCost)
		{
			this.mCoseValue.color = Color.red;
		}
		else
		{
			this.mCoseValue.color = Tools.GetDefaultTextColor();
		}
	}

	private void OnPlayerUpdateEvent()
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.RefreshCost();
		}
	}
}
