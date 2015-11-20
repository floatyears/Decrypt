using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIPillageScene : GameUISession
{
	private class PillageMessageBoxData
	{
		public ulong targetPlayerID;

		public int itemInfoID;

		public bool Farm;

		public PillageMessageBoxData(ulong playerID, int infoID, bool canFarm)
		{
			this.targetPlayerID = playerID;
			this.itemInfoID = infoID;
			this.Farm = canFarm;
		}
	}

	[NonSerialized]
	public static int LastSelectRecipe;

	private bool RefreshPlayerDataFlag;

	private bool RefreshRecipeFlag;

	private float RefreshWarFreeFlag = 0.1f;

	private UIScrollView mRecipeScrollView;

	private UITable mRecipeTable;

	private UIButton mRecordBtn;

	private GameObject fightRecordNew;

	private UIButton mNoBattleBtn;

	private UILabel mRemainTime;

	private GameObject RecipeContent;

	private UISprite TrinketQulity;

	private UISprite TrinketIcon;

	private UILabel TrinketName;

	[NonSerialized]
	public UIButton btnComposite;

	private GameObject btnCompositeEffect;

	private UIButton btnCompositeLabel;

	private UIButton farmBtn;

	private UIButton farmBtnLabel;

	private GameObject CompositeSfx;

	private float SfxPlayTimestamp;

	[NonSerialized]
	public TrinketItem[] TrinketItems = new TrinketItem[6];

	private Dictionary<int, TrinketRecipeItem> RecipeItems = new Dictionary<int, TrinketRecipeItem>();

	private List<TrinketRecipeItem> RecipeList = new List<TrinketRecipeItem>();

	private TrinketRecipeItem curRecipeSelected;

	[NonSerialized]
	public GUIPillageTargetList PillageTargetList;

	private GUIPillageRecord PillageRecord;

	private GUIPillageFarm PillageFarm;

	private GUIPillageWarFree PillageWarFree;

	private GUIPillageCastingPopUp mCastingPopUp;

	private int lastWarFreeTime = -1;

	private UnityEngine.Object TrinketRecipePrefab;

	private UnityEngine.Object TrinketItemPrefab;

	public static void TryOpen(bool showCast = false)
	{
		if (Globals.Instance.Player.PetSystem.Values.Count == 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("pvpTxt18", 0f, 0f);
			return;
		}
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(8)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("pvpTxt1", new object[]
			{
				GameConst.GetInt32(8)
			}), 0f, 0f);
			return;
		}
		if (showCast && Tools.CanPlay(GameConst.GetInt32(186), true))
		{
			GameUIManager.mInstance.ChangeSession<GUIPillageScene>(delegate(GUIPillageScene obj)
			{
				obj.mCastingPopUp.Open();
			}, false, true);
		}
		else
		{
			GameUIManager.mInstance.ChangeSession<GUIPillageScene>(null, false, true);
		}
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("Pillage");
		Transform transform = base.transform.Find("Left");
		transform.gameObject.SetActive(true);
		this.mRecipeScrollView = transform.FindChild("bagPanel").gameObject.GetComponent<UIScrollView>();
		this.mRecipeTable = this.mRecipeScrollView.transform.FindChild("bagContents").gameObject.GetComponent<UITable>();
		this.mRecipeTable.columns = 1;
		this.mRecipeTable.direction = UITable.Direction.Down;
		this.mRecipeTable.sorting = UITable.Sorting.Alphabetic;
		Transform transform2 = base.transform.Find("Center");
		this.RecipeContent = transform2.FindChild("RecipeContent").gameObject;
		this.TrinketQulity = this.RecipeContent.transform.FindChild("TrinketItem").GetComponent<UISprite>();
		UIEventListener expr_FD = UIEventListener.Get(this.TrinketQulity.gameObject);
		expr_FD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_FD.onClick, new UIEventListener.VoidDelegate(this.OnTrinketItemClicked));
		this.TrinketIcon = this.TrinketQulity.transform.FindChild("ItemIcon").GetComponent<UISprite>();
		this.TrinketName = this.TrinketQulity.transform.FindChild("Name").GetComponent<UILabel>();
		this.btnComposite = transform2.FindChild("BtnComposite").GetComponent<UIButton>();
		UIEventListener expr_184 = UIEventListener.Get(this.btnComposite.gameObject);
		expr_184.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_184.onClick, new UIEventListener.VoidDelegate(this.OnCompositeBtnClick));
		this.btnCompositeLabel = this.btnComposite.GetComponents<UIButton>()[1];
		this.btnCompositeEffect = this.btnComposite.transform.Find("Effect").gameObject;
		this.farmBtn = transform2.Find("FarmBtn").GetComponent<UIButton>();
		UIEventListener expr_1FE = UIEventListener.Get(this.farmBtn.gameObject);
		expr_1FE.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1FE.onClick, new UIEventListener.VoidDelegate(this.OnFarmBtnClick));
		this.farmBtnLabel = this.farmBtn.GetComponents<UIButton>()[1];
		this.CompositeSfx = this.RecipeContent.transform.Find("ui59").gameObject;
		this.CompositeSfx.gameObject.SetActive(false);
		Tools.SetParticleRenderQueue(this.CompositeSfx, 3100, 1f);
		Transform transform3 = base.transform.Find("Right");
		this.mRecordBtn = transform3.FindChild("RecordBtn").GetComponent<UIButton>();
		UIEventListener expr_2AF = UIEventListener.Get(this.mRecordBtn.gameObject);
		expr_2AF.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2AF.onClick, new UIEventListener.VoidDelegate(this.OnRecordBtnClick));
		this.fightRecordNew = this.mRecordBtn.transform.FindChild("new").gameObject;
		this.mNoBattleBtn = transform3.FindChild("NoBattleBtn").GetComponent<UIButton>();
		UIEventListener expr_316 = UIEventListener.Get(this.mNoBattleBtn.gameObject);
		expr_316.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_316.onClick, new UIEventListener.VoidDelegate(this.OnNoBattleBtnClick));
		this.mRemainTime = this.mNoBattleBtn.transform.FindChild("RemainTime").GetComponent<UILabel>();
		this.mRemainTime.text = string.Empty;
		GameUITools.RegisterClickEvent("CastingBtn", new UIEventListener.VoidDelegate(this.OnCastingBtnClick), transform3.gameObject).SetActive(Tools.CanPlay(GameConst.GetInt32(186), true));
		Transform transform4 = base.transform.Find("PillageTargetList");
		this.PillageTargetList = transform4.gameObject.AddComponent<GUIPillageTargetList>();
		this.PillageTargetList.Init();
		this.PillageTargetList.gameObject.SetActive(false);
		Transform transform5 = base.transform.Find("PillageRecord");
		this.PillageRecord = transform5.gameObject.AddComponent<GUIPillageRecord>();
		this.PillageRecord.Init();
		this.PillageRecord.gameObject.SetActive(false);
		Transform transform6 = base.transform.Find("PillageFarm");
		this.PillageFarm = transform6.gameObject.AddComponent<GUIPillageFarm>();
		this.PillageFarm.Init();
		this.PillageFarm.gameObject.SetActive(false);
		Transform transform7 = base.transform.Find("PillageWarFree");
		this.PillageWarFree = transform7.gameObject.AddComponent<GUIPillageWarFree>();
		this.PillageWarFree.Init();
		this.PillageWarFree.gameObject.SetActive(false);
		this.mCastingPopUp = GameUITools.FindGameObject("CastingPopUp", base.gameObject).AddComponent<GUIPillageCastingPopUp>();
		this.mCastingPopUp.Init();
		this.mCastingPopUp.Hide();
		int num = 0;
		foreach (RecipeInfo current in Globals.Instance.AttDB.RecipeDict.Values)
		{
			TrinketRecipeItem trinketRecipeItem = this.AddOneItem(current);
			TrinketRecipeItem expr_501 = trinketRecipeItem;
			expr_501.SelectedEvent = (UIEventListener.VoidDelegate)Delegate.Combine(expr_501.SelectedEvent, new UIEventListener.VoidDelegate(this.OnRecipeItemClicked));
			this.RecipeItems[current.ID] = trinketRecipeItem;
			this.RecipeList.Add(trinketRecipeItem);
			if (trinketRecipeItem.isVisible)
			{
				num++;
			}
		}
		this.RecipeList.Sort(new Comparison<TrinketRecipeItem>(TrinketRecipeItem.SortByName));
		this.mRecipeTable.Reposition();
		TrinketRecipeItem trinketRecipeItem2 = null;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < this.RecipeList.Count; i++)
		{
			if (this.RecipeList[i].gameObject.activeSelf)
			{
				if (this.RecipeList[i].RecipeInfo != null)
				{
					if (GUIPillageScene.LastSelectRecipe == this.RecipeList[i].RecipeInfo.ID)
					{
						trinketRecipeItem2 = this.RecipeList[i];
						num2 = num3;
						break;
					}
					if (trinketRecipeItem2 == null)
					{
						trinketRecipeItem2 = this.RecipeList[i];
						num2 = num3;
						if (GUIPillageScene.LastSelectRecipe == 0)
						{
							break;
						}
					}
					num3++;
				}
			}
		}
		if (trinketRecipeItem2 != null)
		{
			this.OnSelectedRecipe(trinketRecipeItem2);
		}
		if (num2 > 3 && num > 0)
		{
			this.mRecipeScrollView.SetDragAmount(0f, (float)num2 / (float)num, false);
		}
		PvpSubSystem expr_6B5 = Globals.Instance.Player.PvpSystem;
		expr_6B5.QueryPillageTargetEvent = (PvpSubSystem.VoidCallback)Delegate.Combine(expr_6B5.QueryPillageTargetEvent, new PvpSubSystem.VoidCallback(this.OnQueryPillageTargetEvent));
		PvpSubSystem expr_6E5 = Globals.Instance.Player.PvpSystem;
		expr_6E5.QueryPillageRecordEvent = (PvpSubSystem.VoidCallback)Delegate.Combine(expr_6E5.QueryPillageRecordEvent, new PvpSubSystem.VoidCallback(this.OnQueryPillageRecordEvent));
		ItemSubSystem expr_715 = Globals.Instance.Player.ItemSystem;
		expr_715.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Combine(expr_715.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_745 = Globals.Instance.Player.ItemSystem;
		expr_745.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Combine(expr_745.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_775 = Globals.Instance.Player.ItemSystem;
		expr_775.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Combine(expr_775.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdateItemEvent));
		LocalPlayer expr_7A0 = Globals.Instance.Player;
		expr_7A0.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_7A0.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		Globals.Instance.CliSession.Register(823, new ClientSession.MsgHandler(this.OnMsgPvpPillageFarm));
		Globals.Instance.CliSession.Register(507, new ClientSession.MsgHandler(this.OnMsgTrinketCreate));
		Globals.Instance.CliSession.Register(543, new ClientSession.MsgHandler(this.OnMsgTrinketCompound));
		Globals.Instance.CliSession.Register(827, new ClientSession.MsgHandler(this.OnMsgPvpOneKeyPillage));
		this.RefreshPlayerDataFlag = true;
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnPreDestroyGUI()
	{
		base.StopCoroutine(this.PlayCompositeSfx(0uL));
		Globals.Instance.CliSession.Unregister(823, new ClientSession.MsgHandler(this.OnMsgPvpPillageFarm));
		Globals.Instance.CliSession.Unregister(507, new ClientSession.MsgHandler(this.OnMsgTrinketCreate));
		Globals.Instance.CliSession.Unregister(543, new ClientSession.MsgHandler(this.OnMsgTrinketCompound));
		Globals.Instance.CliSession.Unregister(827, new ClientSession.MsgHandler(this.OnMsgPvpOneKeyPillage));
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Hide();
		PvpSubSystem expr_AE = Globals.Instance.Player.PvpSystem;
		expr_AE.QueryPillageTargetEvent = (PvpSubSystem.VoidCallback)Delegate.Remove(expr_AE.QueryPillageTargetEvent, new PvpSubSystem.VoidCallback(this.OnQueryPillageTargetEvent));
		PvpSubSystem expr_DE = Globals.Instance.Player.PvpSystem;
		expr_DE.QueryPillageRecordEvent = (PvpSubSystem.VoidCallback)Delegate.Remove(expr_DE.QueryPillageRecordEvent, new PvpSubSystem.VoidCallback(this.OnQueryPillageRecordEvent));
		ItemSubSystem expr_10E = Globals.Instance.Player.ItemSystem;
		expr_10E.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Remove(expr_10E.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		ItemSubSystem expr_13E = Globals.Instance.Player.ItemSystem;
		expr_13E.RemoveItemEvent = (ItemSubSystem.RemoveItemCallback)Delegate.Remove(expr_13E.RemoveItemEvent, new ItemSubSystem.RemoveItemCallback(this.OnRemoveItemEvent));
		ItemSubSystem expr_16E = Globals.Instance.Player.ItemSystem;
		expr_16E.UpdateItemEvent = (ItemSubSystem.UpdateItemCallback)Delegate.Remove(expr_16E.UpdateItemEvent, new ItemSubSystem.UpdateItemCallback(this.OnUpdateItemEvent));
		LocalPlayer expr_199 = Globals.Instance.Player;
		expr_199.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_199.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
	}

	private void Update()
	{
		if (this.RefreshRecipeFlag && Time.time - this.SfxPlayTimestamp > 0f)
		{
			this._RefreshRecipeList();
		}
		if (this.RefreshPlayerDataFlag)
		{
			this._RefreshPlayerData();
		}
		this.RefreshWarFreeFlag -= Time.fixedDeltaTime;
		if (this.RefreshWarFreeFlag < 0f)
		{
			this._RefreshWarFreeTime();
		}
	}

	private void _RefreshWarFreeTime()
	{
		if (this.mRemainTime == null)
		{
			return;
		}
		if (Globals.Instance.Player == null)
		{
			return;
		}
		this.RefreshWarFreeFlag = 1f;
		int num = (Globals.Instance.Player.Data.WarFreeTime <= 0) ? 0 : (Globals.Instance.Player.Data.WarFreeTime - Globals.Instance.Player.GetTimeStamp());
		if (num == 0 && this.lastWarFreeTime == num)
		{
			return;
		}
		this.lastWarFreeTime = num;
		if (num > 0)
		{
			this.mRemainTime.text = Tools.FormatTime(num);
		}
		else
		{
			this.mRemainTime.text = Singleton<StringManager>.Instance.GetString("Pillage14");
		}
	}

	private void _RefreshPlayerData()
	{
		if (!this.RefreshPlayerDataFlag)
		{
			return;
		}
		this.RefreshPlayerDataFlag = false;
		this.fightRecordNew.SetActive((Globals.Instance.Player.Data.RedFlag & 512) != 0);
		if (this.PillageTargetList != null && this.PillageTargetList.gameObject.activeSelf)
		{
			this.PillageTargetList.RefreshStamina();
		}
	}

	private void OnPlayerUpdateEvent()
	{
		this.RefreshPlayerDataFlag = true;
	}

	private void OnQueryPillageTargetEvent()
	{
		if (this.PillageTargetList != null)
		{
			this.PillageTargetList.Show();
		}
	}

	private void OnQueryPillageRecordEvent()
	{
		if (this.PillageRecord != null)
		{
			this.PillageRecord.Show();
		}
	}

	private TrinketRecipeItem GetFirstVisibleRecipe()
	{
		for (int i = 0; i < this.RecipeList.Count; i++)
		{
			if (this.RecipeList[i].gameObject.activeSelf)
			{
				return this.RecipeList[i];
			}
		}
		return null;
	}

	private void OnAddItemEvent(ItemDataEx data)
	{
		if (data != null && data.Info != null && data.Info.Type == 3 && data.Info.SubType == 2)
		{
			this.RefreshRecipeFlag = true;
		}
		if (this.PillageWarFree.gameObject.activeSelf)
		{
			this.PillageWarFree.RefreshItemCount();
		}
	}

	private void _RefreshRecipeList()
	{
		foreach (KeyValuePair<int, TrinketRecipeItem> current in this.RecipeItems)
		{
			TrinketRecipeItem value = current.Value;
			value.RefreshVisible();
			if (this.curRecipeSelected == value && !value.gameObject.activeSelf)
			{
				this.curRecipeSelected.Selected = false;
				this.curRecipeSelected = null;
			}
		}
		if (this.curRecipeSelected == null)
		{
			this.OnSelectedRecipe(this.GetFirstVisibleRecipe());
			this.mRecipeTable.Reposition();
			this.mRecipeScrollView.SetDragAmount(0f, 0f, false);
		}
		else
		{
			this.RefreshSelectedRecipe(this.curRecipeSelected);
		}
		this.mRecipeTable.repositionNow = true;
		this.RefreshRecipeFlag = false;
	}

	private void OnRemoveItemEvent(ulong id)
	{
		if (this.PillageWarFree.gameObject.activeSelf)
		{
			this.PillageWarFree.RefreshItemCount();
		}
	}

	private void OnUpdateItemEvent(ItemDataEx data)
	{
		if (data != null && data.Info != null && data.Info.Type == 3 && data.Info.SubType == 2)
		{
			this.RefreshRecipeFlag = true;
		}
		if (this.PillageWarFree != null && this.PillageWarFree.gameObject.activeSelf)
		{
			this.PillageWarFree.RefreshItemCount();
		}
	}

	private void OnRecordBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIPillageScene.RequestQueryPillageRecord();
	}

	private void OnNoBattleBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.PillageWarFree)
		{
			this.PillageWarFree.Show();
		}
	}

	private void OnCastingBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mCastingPopUp)
		{
			this.mCastingPopUp.Open();
		}
	}

	public void OnCompositeBtnClick(GameObject go)
	{
		if (this.curRecipeSelected == null || this.curRecipeSelected.RecipeInfo == null || this.curRecipeSelected.RecipeItemInfo == null)
		{
			return;
		}
		if (this.RefreshRecipeFlag)
		{
			return;
		}
		if (!this.curRecipeSelected.isCanComposite)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("Pillage8", 0f, 0f);
			return;
		}
		if (Tools.IsTrinketBagFull())
		{
			return;
		}
		GUIPillageScene.RequestTrinketCreate(this.curRecipeSelected.RecipeItemInfo.ID);
	}

	private void OnFarmBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!Tools.CanPlay(GameConst.GetInt32(211), true))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("pvpTxt1", new object[]
			{
				GameConst.GetInt32(211)
			}), 0f, 0f);
			return;
		}
		if (Globals.Instance.Player.Data.Stamina < GameConst.GetInt32(36))
		{
			GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Stamina);
			return;
		}
		MC2S_PvpOneKeyPillage mC2S_PvpOneKeyPillage = new MC2S_PvpOneKeyPillage();
		mC2S_PvpOneKeyPillage.ItemID = this.curRecipeSelected.RecipeInfo.ID;
		Globals.Instance.CliSession.Send(826, mC2S_PvpOneKeyPillage);
		LocalPlayer player = Globals.Instance.Player;
		GameUIState uiState = GameUIManager.mInstance.uiState;
		uiState.PlayerLevel = player.Data.Level;
		uiState.PlayerEnergy = player.Data.Energy;
		uiState.PlayerExp = player.Data.Exp;
		uiState.PlayerMoney = player.Data.Money;
		uiState.SetOldFurtherData(Globals.Instance.Player.TeamSystem.GetPet(0));
	}

	public TrinketRecipeItem AddOneItem(RecipeInfo info)
	{
		if (this.TrinketRecipePrefab == null)
		{
			this.TrinketRecipePrefab = Res.LoadGUI("GUI/TrinketRecipeItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.TrinketRecipePrefab);
		gameObject.transform.parent = this.mRecipeTable.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		TrinketRecipeItem trinketRecipeItem = gameObject.AddComponent<TrinketRecipeItem>();
		trinketRecipeItem.Init(info);
		return trinketRecipeItem;
	}

	private void OnRecipeItemClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (go == null)
		{
			return;
		}
		if (this.RefreshRecipeFlag)
		{
			return;
		}
		this.OnSelectedRecipe(go.GetComponent<TrinketRecipeItem>());
	}

	private void OnTrinketItemClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (go == null)
		{
			return;
		}
		if (this.curRecipeSelected != null && this.curRecipeSelected.RecipeItemInfo != null)
		{
			GameUIManager.mInstance.ShowItemInfo(this.curRecipeSelected.RecipeItemInfo);
		}
	}

	private void OnSelectedRecipe(TrinketRecipeItem item)
	{
		this.curRecipeSelected = item;
		if (this.curRecipeSelected == null)
		{
			return;
		}
		this.curRecipeSelected.Selected = true;
		for (int i = 0; i < this.RecipeList.Count; i++)
		{
			if (this.RecipeList[i].gameObject.activeSelf)
			{
				if (this.curRecipeSelected != this.RecipeList[i])
				{
					this.RecipeList[i].Selected = false;
				}
			}
		}
		this.RefreshSelectedRecipe(this.curRecipeSelected);
	}

	private void RefreshSelectedRecipe(TrinketRecipeItem item)
	{
		if (item == null || item.RecipeInfo == null || item.RecipeItemInfo == null)
		{
			return;
		}
		int count = item.ItemInfos.Count;
		if (count == 0)
		{
			return;
		}
		GUIPillageScene.LastSelectRecipe = item.RecipeInfo.ID;
		this.TrinketQulity.spriteName = Tools.GetItemQualityIcon(item.RecipeItemInfo.Quality);
		this.TrinketIcon.spriteName = item.RecipeItemInfo.Icon;
		this.TrinketName.text = item.RecipeItemInfo.Name;
		this.TrinketName.color = Tools.GetItemQualityColor(item.RecipeItemInfo.Quality);
		float num = 184f;
		float num2 = 3.14159274f / (float)count * 2f;
		float num3 = (float)((count + 1) % 2) * 3.14159274f / (float)count;
		for (int i = 0; i < count; i++)
		{
			ItemInfo itemInfo = item.ItemInfos[i];
			if (itemInfo != null)
			{
				float f = num2 * (float)i + num3;
				if (this.TrinketItems[i] == null)
				{
					if (this.TrinketItemPrefab == null)
					{
						this.TrinketItemPrefab = Res.LoadGUI("GUI/TrinketItem");
					}
					GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.TrinketItemPrefab);
					gameObject.transform.parent = this.RecipeContent.transform;
					gameObject.transform.localPosition = new Vector3(num * Mathf.Sin(f), num * Mathf.Cos(f), 0f);
					gameObject.transform.localScale = Vector3.one;
					TrinketItem trinketItem = gameObject.AddComponent<TrinketItem>();
					trinketItem.Refresh(itemInfo);
					this.TrinketItems[i] = trinketItem;
				}
				else
				{
					this.TrinketItems[i].Refresh(itemInfo);
					this.TrinketItems[i].transform.localPosition = new Vector3(num * Mathf.Sin(f), num * Mathf.Cos(f), 0f);
				}
			}
		}
		for (int j = count; j < this.TrinketItems.Length; j++)
		{
			if (this.TrinketItems[j] != null)
			{
				this.TrinketItems[j].gameObject.SetActive(false);
			}
		}
		if (Tools.CanPlay(GameConst.GetInt32(210), true))
		{
			Vector3 localPosition = this.btnComposite.transform.localPosition;
			localPosition.x = 100f;
			this.btnComposite.transform.localPosition = localPosition;
			this.farmBtn.gameObject.SetActive(true);
			if (Tools.CanPlay(GameConst.GetInt32(211), true))
			{
				this.farmBtn.normalSprite = "btn_n";
				this.farmBtn.hoverSprite = "btn_n";
				this.farmBtn.pressedSprite = "btn_n";
				this.farmBtnLabel.SetState(UIButtonColor.State.Normal, false);
			}
			else
			{
				this.farmBtn.normalSprite = "btn_d";
				this.farmBtn.hoverSprite = "btn_d";
				this.farmBtn.pressedSprite = "btn_d";
				this.farmBtnLabel.SetState(UIButtonColor.State.Disabled, false);
			}
		}
		else
		{
			this.farmBtn.gameObject.SetActive(false);
		}
		if (item.isCanComposite)
		{
			this.btnComposite.isEnabled = true;
			this.btnCompositeLabel.SetState(UIButtonColor.State.Normal, false);
			this.btnCompositeEffect.gameObject.SetActive(true);
			if (Tools.CanPlay(GameConst.GetInt32(211), true))
			{
				this.farmBtn.isEnabled = false;
				this.farmBtnLabel.SetState(UIButtonColor.State.Disabled, false);
			}
		}
		else
		{
			this.btnComposite.isEnabled = false;
			this.btnCompositeLabel.SetState(UIButtonColor.State.Disabled, false);
			this.btnCompositeEffect.gameObject.SetActive(false);
			if (Tools.CanPlay(GameConst.GetInt32(211), true))
			{
				this.farmBtn.isEnabled = true;
				this.farmBtnLabel.SetState(UIButtonColor.State.Normal, false);
			}
		}
	}

	private void OnMsgTrinketCreate(MemoryStream stream)
	{
		try
		{
			MS2C_TrinketCreate mS2C_TrinketCreate = Serializer.NonGeneric.Deserialize(typeof(MS2C_TrinketCreate), stream) as MS2C_TrinketCreate;
			if (mS2C_TrinketCreate.Result != 0)
			{
				GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_TrinketCreate.Result);
			}
			else
			{
				base.StartCoroutine(this.PlayCompositeSfx(mS2C_TrinketCreate.ItemID));
				this.SfxPlayTimestamp = Time.time + 1.7f;
				this.RefreshRecipeFlag = true;
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogException("MS2C_PvpPillageFarm error", ex);
		}
	}

	[DebuggerHidden]
	private IEnumerator PlayCompositeSfx(ulong itemID)
	{
        return null;
        //GUIPillageScene.<PlayCompositeSfx>c__Iterator86 <PlayCompositeSfx>c__Iterator = new GUIPillageScene.<PlayCompositeSfx>c__Iterator86();
        //<PlayCompositeSfx>c__Iterator.itemID = itemID;
        //<PlayCompositeSfx>c__Iterator.<$>itemID = itemID;
        //<PlayCompositeSfx>c__Iterator.<>f__this = this;
        //return <PlayCompositeSfx>c__Iterator;
	}

	private void OnMsgPvpPillageFarm(MemoryStream stream)
	{
		try
		{
			MS2C_PvpPillageFarm mS2C_PvpPillageFarm = Serializer.NonGeneric.Deserialize(typeof(MS2C_PvpPillageFarm), stream) as MS2C_PvpPillageFarm;
			if (mS2C_PvpPillageFarm.Result == 34)
			{
				Globals.Instance.Player.ShowFrozenFunctionMsg();
			}
			else if (mS2C_PvpPillageFarm.Result != 0)
			{
				GameUIManager.mInstance.ShowMessageTip("EPVPR", mS2C_PvpPillageFarm.Result);
			}
			else if (this.PillageFarm != null)
			{
				if (this.PillageTargetList != null)
				{
					this.PillageTargetList.gameObject.SetActive(false);
				}
				this.PillageFarm.Show(mS2C_PvpPillageFarm.Data);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogException("MS2C_PvpPillageFarm error", ex);
		}
	}

	private void OnMsgPvpOneKeyPillage(MemoryStream stream)
	{
		MS2C_PvpOneKeyPillage mS2C_PvpOneKeyPillage = Serializer.NonGeneric.Deserialize(typeof(MS2C_PvpOneKeyPillage), stream) as MS2C_PvpOneKeyPillage;
		if (mS2C_PvpOneKeyPillage.Result == 34)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_PvpOneKeyPillage.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EPVPR", mS2C_PvpOneKeyPillage.Result);
			return;
		}
		if (this.PillageFarm != null)
		{
			if (this.PillageTargetList != null)
			{
				this.PillageTargetList.gameObject.SetActive(false);
			}
			this.PillageFarm.Show(mS2C_PvpOneKeyPillage.Data);
		}
	}

	private void OnMsgTrinketCompound(MemoryStream stream)
	{
		MS2C_TrinketCompound mS2C_TrinketCompound = Serializer.NonGeneric.Deserialize(typeof(MS2C_TrinketCompound), stream) as MS2C_TrinketCompound;
		if (mS2C_TrinketCompound.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ItemR", mS2C_TrinketCompound.Result);
			return;
		}
		this.mCastingPopUp.PlayCompoundAnim(mS2C_TrinketCompound.TrinketID);
	}

	public static void RequestQueryPillageTarget(ItemInfo itemInfo)
	{
		LocalPlayer player = Globals.Instance.Player;
		if ((ulong)player.Data.Level < (ulong)((long)GameConst.GetInt32(8)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("Pillage10", new object[]
			{
				GameConst.GetInt32(8)
			}), 0f, 0f);
			return;
		}
		if (player.ItemSystem.GetItemByInfoID(itemInfo.ID) != null)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("EPVPR_29", 0f, 0f);
			return;
		}
		MC2S_QueryPillageTarget mC2S_QueryPillageTarget = new MC2S_QueryPillageTarget();
		mC2S_QueryPillageTarget.ItemID = itemInfo.ID;
		Globals.Instance.CliSession.Send(814, mC2S_QueryPillageTarget);
		GameUIManager.mInstance.uiState.PillageItem = itemInfo;
	}

	public static void RequestPvpPillageStart(ulong targetPlayerID, int itemInfoID, bool canFarm)
	{
		LocalPlayer player = Globals.Instance.Player;
		if ((ulong)player.Data.Level < (ulong)((long)GameConst.GetInt32(8)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("Pillage10", new object[]
			{
				GameConst.GetInt32(8)
			}), 0f, 0f);
			return;
		}
		if (player.Data.Stamina < GameConst.GetInt32(36))
		{
			GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Stamina);
			return;
		}
		if (player.ItemSystem.GetItemByInfoID(itemInfoID) != null)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("EPVPR_29", 0f, 0f);
			return;
		}
		if (!canFarm)
		{
			int num = (Globals.Instance.Player.Data.WarFreeTime <= 0) ? 0 : (Globals.Instance.Player.Data.WarFreeTime - Globals.Instance.Player.GetTimeStamp());
			if (num > 0)
			{
				GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("Pillage18"), MessageBox.Type.OKCancel, new GUIPillageScene.PillageMessageBoxData(targetPlayerID, itemInfoID, canFarm));
				if (gameMessageBox != null)
				{
					GameMessageBox expr_121 = gameMessageBox;
					expr_121.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_121.OkClick, new MessageBox.MessageDelegate(GUIPillageScene.OnPillageStartMessageBoxChecked));
				}
				return;
			}
		}
		GUIPillageScene.PillageStartChecked(targetPlayerID, itemInfoID, canFarm);
	}

	private static void OnPillageStartMessageBoxChecked(object obj)
	{
		GUIPillageScene.PillageMessageBoxData pillageMessageBoxData = obj as GUIPillageScene.PillageMessageBoxData;
		if (pillageMessageBoxData != null)
		{
			GUIPillageScene.PillageStartChecked(pillageMessageBoxData.targetPlayerID, pillageMessageBoxData.itemInfoID, pillageMessageBoxData.Farm);
		}
	}

	private static void PillageStartChecked(ulong targetPlayerID, int itemInfoID, bool canFarm)
	{
		Globals.Instance.Player.PvpSystem.SetPillageTargetID(targetPlayerID);
		MC2S_PvpPillageStart mC2S_PvpPillageStart = new MC2S_PvpPillageStart();
		mC2S_PvpPillageStart.TargetID = targetPlayerID;
		mC2S_PvpPillageStart.ItemID = itemInfoID;
		mC2S_PvpPillageStart.Flag = !canFarm;
		Globals.Instance.CliSession.Send(816, mC2S_PvpPillageStart);
		GameUIState uiState = GameUIManager.mInstance.uiState;
		LocalPlayer player = Globals.Instance.Player;
		uiState.PlayerLevel = player.Data.Level;
		uiState.PlayerEnergy = player.Data.Energy;
		uiState.PlayerExp = player.Data.Exp;
		uiState.PlayerMoney = player.Data.Money;
		uiState.SetOldFurtherData(player.TeamSystem.GetPet(0));
	}

	public static void RequestPvpPillageFarm(ulong targetPlayerID, int pillageCount, bool isPass = false)
	{
		if (GameUIManager.mInstance.uiState.PillageItem == null)
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (!isPass && (ulong)player.Data.Level < (ulong)((long)GameConst.GetInt32(9)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("Pillage9", new object[]
			{
				GameConst.GetInt32(9)
			}), 0f, 0f);
			return;
		}
		if (player.Data.Stamina < GameConst.GetInt32(36))
		{
			GUIShortcutBuyItem.Show(GUIShortcutBuyItem.BuyType.Stamina);
			return;
		}
		MC2S_PvpPillageFarm mC2S_PvpPillageFarm = new MC2S_PvpPillageFarm();
		mC2S_PvpPillageFarm.TargetID = targetPlayerID;
		mC2S_PvpPillageFarm.ItemID = GameUIManager.mInstance.uiState.PillageItem.ID;
		mC2S_PvpPillageFarm.Count = pillageCount;
		Globals.Instance.CliSession.Send(822, mC2S_PvpPillageFarm);
		GameUIState uiState = GameUIManager.mInstance.uiState;
		uiState.PlayerLevel = player.Data.Level;
		uiState.PlayerEnergy = player.Data.Energy;
		uiState.PlayerExp = player.Data.Exp;
		uiState.PlayerMoney = player.Data.Money;
		uiState.SetOldFurtherData(Globals.Instance.Player.TeamSystem.GetPet(0));
	}

	public static void RequestQueryPillageRecord()
	{
		MC2S_QueryPillageRecord mC2S_QueryPillageRecord = new MC2S_QueryPillageRecord();
		mC2S_QueryPillageRecord.RecordVersion = Globals.Instance.Player.PvpSystem.PillageRecordVersion;
		Globals.Instance.CliSession.Send(820, mC2S_QueryPillageRecord);
	}

	public static void RequestTrinketCreate(int itemInfoID)
	{
		MC2S_TrinketCreate mC2S_TrinketCreate = new MC2S_TrinketCreate();
		mC2S_TrinketCreate.InfoID = itemInfoID;
		Globals.Instance.CliSession.Send(506, mC2S_TrinketCreate);
	}
}
