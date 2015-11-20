using Att;
using System;
using System.Text;
using UnityEngine;

public class GUIShiZhuangSceneV2 : GameUISession
{
	private GUIShiZhuangItemTable mGUIShiZhuangItemTable;

	private GameObject mCardModel;

	private GameObject mModelTmp;

	private UIActorController mUIActorController;

	private UILabel mJinDuNum;

	private UILabel mCurEffect0;

	private UILabel mMaxEffectTxt;

	private UILabel mMaxEffect;

	private UILabel mFashionTimeTip;

	private ShiZhuangItemData mShiZhuangItemData;

	private StringBuilder mSb = new StringBuilder(42);

	private ResourceEntity asyncEntiry;

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle");
		this.mGUIShiZhuangItemTable = transform.Find("winBg/rightInfo/contentPanel/contents").gameObject.AddComponent<GUIShiZhuangItemTable>();
		this.mGUIShiZhuangItemTable.maxPerLine = 3;
		this.mGUIShiZhuangItemTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mGUIShiZhuangItemTable.cellWidth = 180f;
		this.mGUIShiZhuangItemTable.cellHeight = 220f;
		this.mGUIShiZhuangItemTable.InitWithBaseScene(this);
		UILabel component = transform.Find("winBg/rightInfo/tipTxt").GetComponent<UILabel>();
		component.text = Singleton<StringManager>.Instance.GetString("shiZhuangTxt1");
		Transform transform2 = transform.Find("flower");
		Transform transform3 = transform2.Find("bottomPanel/bottomInfo");
		this.mJinDuNum = transform3.Find("jinDuTxt/num").GetComponent<UILabel>();
		Transform transform4 = transform3.Find("curEffectTxt");
		this.mCurEffect0 = transform4.Find("num0").GetComponent<UILabel>();
		this.mMaxEffectTxt = transform3.Find("maxEffectTxt").GetComponent<UILabel>();
		this.mMaxEffect = this.mMaxEffectTxt.transform.Find("num").GetComponent<UILabel>();
		this.mFashionTimeTip = transform3.Find("tipTxt").GetComponent<UILabel>();
		this.mFashionTimeTip.text = string.Empty;
		this.mCardModel = transform2.Find("modelPos").gameObject;
	}

	protected override void OnPostLoadGUI()
	{
		GameCache.Data.HasNewFashion = false;
		GameCache.UpdateNow = true;
		this.CreateObjects();
		this.InitShiZhuangItems();
		this.Refresh();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("PetClothes");
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		TeamSubSystem expr_57 = Globals.Instance.Player.TeamSystem;
		expr_57.EquipFashionEvent = (TeamSubSystem.UpdateFashionCallback)Delegate.Combine(expr_57.EquipFashionEvent, new TeamSubSystem.UpdateFashionCallback(this.OnEquipFashionEvent));
	}

	protected override void OnPreDestroyGUI()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Hide();
		TeamSubSystem expr_20 = Globals.Instance.Player.TeamSystem;
		expr_20.EquipFashionEvent = (TeamSubSystem.UpdateFashionCallback)Delegate.Remove(expr_20.EquipFashionEvent, new TeamSubSystem.UpdateFashionCallback(this.OnEquipFashionEvent));
	}

	private void InitShiZhuangItems()
	{
		this.mGUIShiZhuangItemTable.ClearData();
		foreach (FashionInfo current in Globals.Instance.AttDB.FashionDict.Values)
		{
			if (current != null && Globals.Instance.Player.Data.Gender == current.Gender - 1 && current.Enable != 0)
			{
				this.mGUIShiZhuangItemTable.AddData(new ShiZhuangItemData(current));
			}
		}
	}

	private void OnEquipFashionEvent(int infoId)
	{
		this.mGUIShiZhuangItemTable.SetCurSelectData(infoId);
		this.CreateModel();
	}

	private void ClearModel()
	{
		if (this.asyncEntiry != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntiry);
			this.asyncEntiry = null;
		}
		if (this.mModelTmp != null)
		{
			this.mUIActorController = null;
			UnityEngine.Object.DestroyImmediate(this.mModelTmp);
			this.mModelTmp = null;
		}
	}

	public void CreateModel()
	{
		this.ClearModel();
		ItemSubSystem itemSystem = Globals.Instance.Player.ItemSystem;
		this.mShiZhuangItemData = this.mGUIShiZhuangItemTable.GetCurSelectData();
		if (itemSystem != null && this.mShiZhuangItemData != null && this.mShiZhuangItemData.mFashionInfo != null)
		{
			if (itemSystem.HasFashion(this.mShiZhuangItemData.mFashionInfo.ID) && itemSystem.IsShiXiaoFashion(this.mShiZhuangItemData.mFashionInfo))
			{
				this.mFashionTimeTip.gameObject.SetActive(true);
				if (itemSystem.IsFashionGuoqi(this.mShiZhuangItemData.mFashionInfo.ID))
				{
					this.mFashionTimeTip.text = Singleton<StringManager>.Instance.GetString("shiZhuangTxt3");
				}
				else
				{
					int num = itemSystem.GetFashionTime(this.mShiZhuangItemData.mFashionInfo.ID) - Globals.Instance.Player.GetTimeStamp();
					int num2 = Mathf.Max(num / 86400, 1);
					this.mFashionTimeTip.text = Singleton<StringManager>.Instance.GetString("shiZhuangTxt4", new object[]
					{
						num2
					});
				}
			}
			else
			{
				this.mFashionTimeTip.gameObject.SetActive(false);
			}
			this.asyncEntiry = ActorManager.CreateUIFashion(this.mShiZhuangItemData.mFashionInfo, 51, true, true, this.mCardModel, 1f, delegate(GameObject go)
			{
				this.asyncEntiry = null;
				this.mModelTmp = go;
				if (this.mModelTmp != null)
				{
					this.mUIActorController = this.mModelTmp.GetComponent<UIActorController>();
					this.mUIActorController.PlayIdleAnimationAndVoice();
					this.mModelTmp.transform.localPosition = new Vector3(0f, 0f, -500f);
					Tools.SetMeshRenderQueue(this.mModelTmp, 3900);
				}
			});
		}
	}

	private void RefreshBottomInfo()
	{
		int validFashionCount = Globals.Instance.Player.ItemSystem.GetValidFashionCount();
		int num = Mathf.Min(validFashionCount + 1, this.mGUIShiZhuangItemTable.mDatas.Count);
		this.mSb.Remove(0, this.mSb.Length).Append(validFashionCount).Append("/").Append(this.mGUIShiZhuangItemTable.mDatas.Count);
		int validShiXiaoCount = Globals.Instance.Player.ItemSystem.GetValidShiXiaoCount();
		if (validShiXiaoCount > 0)
		{
			this.mSb.AppendFormat(Singleton<StringManager>.Instance.GetString("shiZhuangTxt5", new object[]
			{
				validShiXiaoCount
			}), new object[0]);
		}
		this.mJinDuNum.text = this.mSb.ToString();
		TinyLevelInfo info = Globals.Instance.AttDB.TinyLevelDict.GetInfo(validFashionCount);
		if (info != null)
		{
			this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("EAID_1")).Append("+").Append(info.FashionMaxHP);
			if (info.FashionAttack > 0)
			{
				this.mSb.Append("，").Append(Singleton<StringManager>.Instance.GetString("EAID_2")).Append("+").Append(info.FashionAttack);
			}
			this.mCurEffect0.text = this.mSb.ToString();
		}
		if (validFashionCount >= this.mGUIShiZhuangItemTable.mDatas.Count)
		{
			this.mMaxEffectTxt.gameObject.SetActive(false);
		}
		else
		{
			this.mMaxEffectTxt.gameObject.SetActive(true);
			info = Globals.Instance.AttDB.TinyLevelDict.GetInfo(num);
			if (info != null)
			{
				this.mMaxEffectTxt.text = this.mSb.Remove(0, this.mSb.Length).AppendFormat(Singleton<StringManager>.Instance.GetString("shiZhuangTxt0"), num).ToString();
				this.mMaxEffect.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("EAID_1")).Append("+").Append(info.FashionMaxHP).Append("，").Append(Singleton<StringManager>.Instance.GetString("EAID_2")).Append("+").Append(info.FashionAttack).ToString();
			}
		}
	}

	private void Refresh()
	{
		this.RefreshBottomInfo();
		this.CreateModel();
	}
}
