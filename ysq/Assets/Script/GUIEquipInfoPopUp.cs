using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIEquipInfoPopUp : GameUIBasePopup
{
	private class EquipSetPoint : MonoBehaviour
	{
		private UILabel mName;

		private UILabel mValue;

		private void Awake()
		{
			this.mName = GameUITools.FindUILabel("Name", base.gameObject);
			this.mValue = GameUITools.FindUILabel("Value", base.gameObject);
		}

		public void Init(string name, string value, bool active)
		{
			this.mName.text = name;
			this.mValue.text = value;
			if (active)
			{
				this.mName.color = Color.green;
				this.mValue.color = Color.green;
			}
			else
			{
				this.mName.color = Color.gray;
				this.mValue.color = Color.gray;
			}
		}
	}

	public enum EIPT
	{
		EIPT_Team,
		EIPT_Bag,
		EIPT_Fragment,
		EIPT_Other,
		EIPT_View
	}

	private static int mCurSelectIndex = -1;

	private static GUIEquipInfoPopUp.EIPT mType;

	private static ItemDataEx mData;

	private static bool IsLocal;

	public CommonEquipInfoLayer mCommonEquipInfoLayer;

	private UILabel mEnhanceContent;

	private UILabel mEnhanceValues;

	private UILabel mRefineContent;

	private UILabel mRefineValues;

	private UILabel mRefineTitle;

	private Transform mRefineContentBG;

	private UILabel mLegendContent;

	private Transform mLegendContentBG;

	private UILabel mSet;

	private CommonIconItem[] mSetItems = new CommonIconItem[4];

	private GameObject[] mEffects = new GameObject[4];

	private GUIEquipInfoPopUp.EquipSetPoint[] mSetPoints = new GUIEquipInfoPopUp.EquipSetPoint[3];

	private GameObject mChangeBtn;

	private GameObject mChangeRed;

	private GameObject mUnloadBtn;

	private GameObject mEnhanceBtn;

	private GameObject mRefineBtn;

	private UISlider mFragmentSlider;

	private UILabel mFragmentValue;

	private UIToggle mSourceTab;

	private UIToggle mInfoTab;

	private FragmentSourceUITable mContentsTable;

	private UILabel mNoSource;

	public GameObject ui61;

	private UILabel mTitle;

	public static void ShowThis(ItemDataEx data, GUIEquipInfoPopUp.EIPT type, int selectIndex = -1, bool showCreateAnim = false, bool isLocal = true)
	{
		if (data == null)
		{
			global::Debug.LogError(new object[]
			{
				"ItemDataEx is null"
			});
			return;
		}
		GUIEquipInfoPopUp.mData = data;
		GUIEquipInfoPopUp.mType = type;
		GUIEquipInfoPopUp.mCurSelectIndex = selectIndex;
		GUIEquipInfoPopUp.IsLocal = isLocal;
		if (showCreateAnim)
		{
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIEquipInfoPopUp, false, new GameUIPopupManager.PopClosedCallback(GUIEquipInfoPopUp.PlayCreate), null);
			GUIEquipInfoPopUp gUIEquipInfoPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GUIEquipInfoPopUp;
			GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp();
			gUIEquipInfoPopUp.mCommonEquipInfoLayer.mEquipIconItem.gameObject.SetActive(false);
		}
		else
		{
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIEquipInfoPopUp, false, null, null);
			GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp();
		}
	}

	private static void PlayCreate()
	{
		GUIEquipInfoPopUp gUIEquipInfoPopUp = GameUIPopupManager.GetInstance().GetCurrentPopup() as GUIEquipInfoPopUp;
		if (gUIEquipInfoPopUp != null)
		{
			gUIEquipInfoPopUp.mCommonEquipInfoLayer.PlayCreateAnim();
		}
	}

	private void Awake()
	{
		this.CreateObjects();
		TeamSubSystem expr_15 = Globals.Instance.Player.TeamSystem;
		expr_15.EquipItemEvent = (TeamSubSystem.ItemUpdateCallback)Delegate.Combine(expr_15.EquipItemEvent, new TeamSubSystem.ItemUpdateCallback(this.OnMsgEquipItemEvent));
	}

	private void OnDestroy()
	{
		if (Globals.Instance != null)
		{
			TeamSubSystem expr_1F = Globals.Instance.Player.TeamSystem;
			expr_1F.EquipItemEvent = (TeamSubSystem.ItemUpdateCallback)Delegate.Remove(expr_1F.EquipItemEvent, new TeamSubSystem.ItemUpdateCallback(this.OnMsgEquipItemEvent));
		}
	}

	private void CreateObjects()
	{
		this.mTitle = GameUITools.FindUILabel("Title", base.gameObject);
		this.ui61 = GameUITools.FindGameObject("ui61", base.gameObject);
		Tools.SetParticleRQWithUIScale(this.ui61, 6000);
		this.ui61.SetActive(false);
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseBtnClick), base.gameObject);
		this.mCommonEquipInfoLayer = CommonEquipInfoLayer.CreateCommonEquipInfoLayer(base.gameObject, new Vector3(-210f, -13f, 0f));
		GameObject gameObject = GameUITools.FindGameObject("InfoPanel/Contents", base.gameObject);
		this.mEnhanceContent = GameUITools.FindUILabel("EnhanceContent", gameObject);
		this.mEnhanceValues = GameUITools.FindUILabel("Values", this.mEnhanceContent.gameObject);
		this.mRefineContent = GameUITools.FindUILabel("RefineContent", gameObject);
		this.mRefineValues = GameUITools.FindUILabel("Values", this.mRefineContent.gameObject);
		this.mRefineTitle = GameUITools.FindUILabel("Title", this.mRefineContent.gameObject);
		this.mRefineContentBG = GameUITools.FindGameObject("BG", this.mRefineContent.gameObject).transform;
		this.mLegendContent = GameUITools.FindUILabel("LegendContent", gameObject);
		this.mLegendContentBG = GameUITools.FindGameObject("BG", this.mLegendContent.gameObject).transform;
		this.mSet = GameUITools.FindUILabel("Set", gameObject);
		Transform transform = GameUITools.FindGameObject("Items", this.mSet.gameObject).transform;
		int num = 0;
		while (num < this.mSetItems.Length && num < transform.childCount)
		{
			this.mSetItems[num] = CommonIconItem.Create(transform.GetChild(num).gameObject, Vector3.zero, null, false, 0.8f, null);
			this.mEffects[num] = GameUITools.FindGameObject("Effect", transform.GetChild(num).gameObject);
			num++;
		}
		gameObject = GameUITools.FindGameObject("Points", this.mSet.gameObject);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			this.mSetPoints[i] = gameObject.transform.GetChild(i).gameObject.AddComponent<GUIEquipInfoPopUp.EquipSetPoint>();
		}
		gameObject = GameUITools.FindGameObject("ButtonGroup", base.gameObject);
		this.mChangeBtn = GameUITools.RegisterClickEvent("Change", new UIEventListener.VoidDelegate(this.OnChangeBtnClick), gameObject);
		this.mChangeRed = GameUITools.FindGameObject("Red", this.mChangeBtn);
		this.mUnloadBtn = GameUITools.RegisterClickEvent("Unload", new UIEventListener.VoidDelegate(this.OnUnlocaBtnClick), gameObject);
		this.mEnhanceBtn = GameUITools.RegisterClickEvent("Enhance", new UIEventListener.VoidDelegate(this.OnEnhanceBtnClick), gameObject);
		this.mRefineBtn = GameUITools.RegisterClickEvent("Refine", new UIEventListener.VoidDelegate(this.OnRefineBtnClick), gameObject);
		this.mFragmentSlider = GameUITools.FindGameObject("Fragment/ExpProgressBar", gameObject).GetComponent<UISlider>();
		this.mFragmentValue = GameUITools.FindUILabel("Value", this.mFragmentSlider.gameObject);
		GameUITools.RegisterClickEvent("Add", new UIEventListener.VoidDelegate(this.OnSourceBtnClick), this.mFragmentSlider.transform.parent.gameObject);
		gameObject = GameUITools.FindGameObject("Tabs", base.gameObject);
		this.mSourceTab = GameUITools.FindGameObject("SourceTab", gameObject).GetComponent<UIToggle>();
		this.mInfoTab = GameUITools.FindGameObject("InfoTab", gameObject).GetComponent<UIToggle>();
		UIEventListener expr_389 = UIEventListener.Get(this.mSourceTab.gameObject);
		expr_389.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_389.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		UIEventListener expr_3BA = UIEventListener.Get(this.mInfoTab.gameObject);
		expr_3BA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3BA.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
		this.mContentsTable = GameUITools.FindGameObject("Source/Panel/Contents", base.gameObject).AddComponent<FragmentSourceUITable>();
		this.mContentsTable.maxPerLine = 1;
		this.mContentsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContentsTable.cellWidth = 370f;
		this.mContentsTable.cellHeight = 76f;
		this.mContentsTable.gapHeight = 6f;
		this.mContentsTable.gapWidth = 0f;
		this.mContentsTable.bgScrollView = GameUITools.FindGameObject("SourceBG", base.gameObject).GetComponent<UIDragScrollView>();
		this.mNoSource = GameUITools.FindUILabel("NoSource", this.mContentsTable.transform.parent.parent.gameObject);
	}

	public override void InitPopUp()
	{
		switch (GUIEquipInfoPopUp.mType)
		{
		case GUIEquipInfoPopUp.EIPT.EIPT_Team:
			this.mFragmentSlider.transform.parent.gameObject.SetActive(false);
			this.mChangeRed.gameObject.SetActive(Tools.CanEquipShowMark(GUIEquipInfoPopUp.mCurSelectIndex, GUIEquipInfoPopUp.mData.GetEquipSlot()));
			break;
		case GUIEquipInfoPopUp.EIPT.EIPT_Bag:
			this.mChangeBtn.gameObject.SetActive(false);
			this.mUnloadBtn.gameObject.SetActive(false);
			this.mFragmentSlider.transform.parent.gameObject.SetActive(false);
			break;
		case GUIEquipInfoPopUp.EIPT.EIPT_Fragment:
			this.mChangeBtn.gameObject.SetActive(false);
			this.mUnloadBtn.gameObject.SetActive(false);
			this.mEnhanceBtn.gameObject.SetActive(false);
			this.mRefineBtn.gameObject.SetActive(false);
			this.mFragmentSlider.value = (float)Globals.Instance.Player.ItemSystem.GetItemCount(GUIEquipInfoPopUp.mData.Info.ID) / (float)GUIEquipInfoPopUp.mData.GetAmount2Create();
			this.mFragmentValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				Globals.Instance.Player.ItemSystem.GetItemCount(GUIEquipInfoPopUp.mData.Info.ID),
				GUIEquipInfoPopUp.mData.GetAmount2Create()
			});
			break;
		case GUIEquipInfoPopUp.EIPT.EIPT_Other:
			this.mChangeBtn.gameObject.SetActive(false);
			this.mUnloadBtn.gameObject.SetActive(false);
			this.mEnhanceBtn.gameObject.SetActive(false);
			this.mRefineBtn.gameObject.SetActive(false);
			this.mFragmentSlider.transform.parent.gameObject.SetActive(false);
			break;
		case GUIEquipInfoPopUp.EIPT.EIPT_View:
			this.mChangeBtn.gameObject.SetActive(false);
			this.mUnloadBtn.gameObject.SetActive(false);
			this.mEnhanceBtn.gameObject.SetActive(false);
			this.mRefineBtn.gameObject.SetActive(false);
			this.mFragmentSlider.transform.parent.gameObject.SetActive(false);
			break;
		}
		if (GUIEquipInfoPopUp.mData.Info.Type == 0)
		{
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("equipImprove77");
			string text;
			string text2;
			Tools.GetEquipEnhanceAttTxt(GUIEquipInfoPopUp.mData, out text, out text2, GUIEquipInfoPopUp.IsLocal);
			this.mEnhanceContent.text = text;
			this.mEnhanceValues.text = text2;
			Tools.GetEquipRefineAttTxt(GUIEquipInfoPopUp.mData, out text, out text2, GUIEquipInfoPopUp.IsLocal);
			this.mRefineContent.text = text;
			this.mRefineValues.text = text2;
			LegendInfo legendInfo = Tools.GetLegendInfo(GUIEquipInfoPopUp.mData.Info);
			if (legendInfo != null)
			{
				this.mLegendContent.text = Tools.GetLegendSkillStr(legendInfo, GUIEquipInfoPopUp.mData.GetEquipRefineLevel());
				this.mLegendContent.gameObject.SetActive(true);
			}
			else
			{
				this.mLegendContent.gameObject.SetActive(false);
			}
			int num = 0;
			int num2 = 0;
			ItemSetInfo itemSetInfo = GUIEquipInfoPopUp.mData.GetItemSetInfo(out num, out num2, GUIEquipInfoPopUp.IsLocal);
			if (itemSetInfo != null)
			{
				if (this.mLegendContent.gameObject.activeInHierarchy)
				{
					this.mSet.topAnchor.target = this.mLegendContentBG;
				}
				else
				{
					this.mSet.topAnchor.target = this.mRefineContentBG;
				}
				this.mSet.topAnchor.relative = 0f;
				this.mSet.topAnchor.absolute = -12;
				int num3 = 0;
				while (num3 < this.mSetItems.Length && this.mSetItems[num3] != null)
				{
					this.mSetItems[num3].Refresh(Globals.Instance.AttDB.ItemDict.GetInfo(itemSetInfo.ItemID[num3]), false, false, false);
					if ((num2 & 1 << num3) != 0)
					{
						this.mSetItems[num3].SetMask = false;
						this.mEffects[num3].gameObject.SetActive(true);
					}
					else
					{
						this.mSetItems[num3].SetMask = true;
					}
					num3++;
				}
				StringBuilder stringBuilder = new StringBuilder();
				int i;
				for (i = 0; i < itemSetInfo.Count.Count; i++)
				{
					stringBuilder.Remove(0, stringBuilder.Length);
					if (itemSetInfo.AttID1[i] > 0 && itemSetInfo.AttID1[i] < 11)
					{
						stringBuilder.Append(Tools.GetETAttStr(itemSetInfo.AttID1[i], itemSetInfo.AttValue1[i]));
					}
					if (itemSetInfo.AttID2[i] > 0 && itemSetInfo.AttID2[i] < 11)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append(Tools.GetETAttStr(itemSetInfo.AttID2[i], itemSetInfo.AttValue2[i]));
					}
					this.mSetPoints[i].Init(Singleton<StringManager>.Instance.GetString("equipImprove13", new object[]
					{
						itemSetInfo.Count[i]
					}), stringBuilder.ToString(), num >= itemSetInfo.Count[i]);
				}
				while (i < this.mSetPoints.Length)
				{
					global::Debug.LogErrorFormat("ItemSetInfo's Count error {0}", new object[]
					{
						itemSetInfo.ID
					});
					i++;
				}
			}
			else
			{
				this.mSet.gameObject.SetActive(false);
			}
			if (GUIEquipInfoPopUp.mType == GUIEquipInfoPopUp.EIPT.EIPT_Other || GUIEquipInfoPopUp.mType == GUIEquipInfoPopUp.EIPT.EIPT_View)
			{
				this.mSourceTab.gameObject.SetActive(true);
				this.mInfoTab.gameObject.SetActive(true);
				GUIHowGetPetItemPopUp.InitSourceItems(GUIEquipInfoPopUp.mData.Info, this.mContentsTable);
				if (this.mContentsTable.mDatas.Count > 0)
				{
					this.mNoSource.gameObject.SetActive(false);
				}
				else
				{
					this.mNoSource.gameObject.SetActive(true);
				}
			}
		}
		else if (GUIEquipInfoPopUp.mData.Info.Type == 1)
		{
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("equipImprove76");
			this.mSet.gameObject.SetActive(false);
			string text;
			string text2;
			Tools.GetEquipEnhanceAttTxt(GUIEquipInfoPopUp.mData, out text, out text2, GUIEquipInfoPopUp.IsLocal);
			this.mEnhanceContent.text = text;
			this.mEnhanceValues.text = text2;
			Tools.GetEquipRefineAttTxt(GUIEquipInfoPopUp.mData, out text, out text2, GUIEquipInfoPopUp.IsLocal);
			this.mRefineContent.text = text;
			this.mRefineValues.text = text2;
			LegendInfo legendInfo2 = Tools.GetLegendInfo(GUIEquipInfoPopUp.mData.Info);
			if (legendInfo2 != null)
			{
				this.mLegendContent.text = Tools.GetLegendSkillStr(legendInfo2, GUIEquipInfoPopUp.mData.GetTrinketRefineLevel());
				this.mLegendContent.gameObject.SetActive(true);
			}
			else
			{
				this.mLegendContent.gameObject.SetActive(false);
			}
			if (GUIEquipInfoPopUp.mType == GUIEquipInfoPopUp.EIPT.EIPT_Other || GUIEquipInfoPopUp.mType == GUIEquipInfoPopUp.EIPT.EIPT_View)
			{
				this.mSourceTab.gameObject.SetActive(true);
				this.mInfoTab.gameObject.SetActive(true);
				GUIHowGetPetItemPopUp.InitSourceItems(GUIEquipInfoPopUp.mData.Info, this.mContentsTable);
				if (this.mContentsTable.mDatas.Count > 0)
				{
					this.mNoSource.gameObject.SetActive(false);
				}
				else
				{
					this.mNoSource.gameObject.SetActive(true);
				}
			}
		}
		else if (Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(GUIEquipInfoPopUp.mData.Info.ID))
		{
			this.mTitle.text = Singleton<StringManager>.Instance.GetString("equipImprove76");
			this.mSet.gameObject.SetActive(false);
			this.mEnhanceBtn.gameObject.SetActive(false);
			this.mRefineBtn.gameObject.SetActive(false);
			this.mRefineContent.gameObject.SetActive(false);
			this.mEnhanceContent.text = Singleton<StringManager>.Instance.GetString("equipImprove68");
			this.mEnhanceValues.text = GUIEquipInfoPopUp.mData.GetTrinketOrItem2EnhanceExp().ToString();
			this.mLegendContent.gameObject.SetActive(false);
			this.mSourceTab.gameObject.SetActive(true);
			this.mInfoTab.gameObject.SetActive(true);
			GUIHowGetPetItemPopUp.InitSourceItems(GUIEquipInfoPopUp.mData.Info, this.mContentsTable);
			if (this.mContentsTable.mDatas.Count > 0)
			{
				this.mNoSource.gameObject.SetActive(false);
			}
			else
			{
				this.mNoSource.gameObject.SetActive(true);
			}
		}
		else if (GUIEquipInfoPopUp.mData.Info.Type == 3)
		{
			this.mSourceTab.value = true;
			if (GUIEquipInfoPopUp.mData.Info.SubType == 1)
			{
				this.mTitle.text = Singleton<StringManager>.Instance.GetString("equipImprove77");
				string text;
				string text2;
				Tools.GetEquipEnhanceAttTxt(GUIEquipInfoPopUp.mData, out text, out text2, GUIEquipInfoPopUp.IsLocal);
				this.mEnhanceContent.text = text;
				this.mEnhanceValues.text = text2;
				this.mRefineTitle.text = Singleton<StringManager>.Instance.GetString("equipImprove63");
				Tools.GetEquipRefineAttTxt(GUIEquipInfoPopUp.mData, out text, out text2, GUIEquipInfoPopUp.IsLocal);
				this.mRefineContent.text = text;
				this.mRefineValues.text = text2;
				ItemDataEx itemDataEx = new ItemDataEx(new ItemData(), Globals.Instance.AttDB.ItemDict.GetInfo(GUIEquipInfoPopUp.mData.Info.Value2));
				LegendInfo legendInfo3 = Tools.GetLegendInfo(itemDataEx.Info);
				if (legendInfo3 != null)
				{
					this.mLegendContent.text = Tools.GetLegendSkillStr(legendInfo3, 0);
					this.mLegendContent.gameObject.SetActive(true);
				}
				else
				{
					this.mLegendContent.gameObject.SetActive(false);
				}
				int num4 = 0;
				int num5 = 0;
				ItemSetInfo itemSetInfo2 = itemDataEx.GetItemSetInfo(out num4, out num5, true);
				if (itemSetInfo2 != null)
				{
					if (this.mLegendContent.gameObject.activeInHierarchy)
					{
						this.mSet.topAnchor.target = this.mLegendContentBG;
					}
					else
					{
						this.mSet.topAnchor.target = this.mRefineContentBG;
					}
					this.mSet.topAnchor.relative = 0f;
					this.mSet.topAnchor.absolute = -12;
					int num6 = 0;
					while (num6 < this.mSetItems.Length && this.mSetItems[num6] != null)
					{
						this.mSetItems[num6].Refresh(Globals.Instance.AttDB.ItemDict.GetInfo(itemSetInfo2.ItemID[num6]), false, false, false);
						this.mSetItems[num6].SetMask = false;
						num6++;
					}
					StringBuilder stringBuilder2 = new StringBuilder();
					int j;
					for (j = 0; j < itemSetInfo2.Count.Count; j++)
					{
						stringBuilder2.Remove(0, stringBuilder2.Length);
						if (itemSetInfo2.AttID1[j] > 0 && itemSetInfo2.AttID1[j] < 11)
						{
							stringBuilder2.Append(Tools.GetETAttStr(itemSetInfo2.AttID1[j], itemSetInfo2.AttValue1[j]));
						}
						if (itemSetInfo2.AttID2[j] > 0 && itemSetInfo2.AttID2[j] < 11)
						{
							stringBuilder2.AppendLine();
							stringBuilder2.Append(Tools.GetETAttStr(itemSetInfo2.AttID2[j], itemSetInfo2.AttValue2[j]));
						}
						this.mSetPoints[j].Init(Singleton<StringManager>.Instance.GetString("equipImprove13", new object[]
						{
							itemSetInfo2.Count[j]
						}), stringBuilder2.ToString(), num4 >= itemSetInfo2.Count[j]);
					}
					while (j < this.mSetPoints.Length)
					{
						global::Debug.LogErrorFormat("ItemSetInfo's Count error {0}", new object[]
						{
							itemSetInfo2.ID
						});
						j++;
					}
				}
				this.mSourceTab.gameObject.SetActive(true);
				this.mInfoTab.gameObject.SetActive(true);
				GUIHowGetPetItemPopUp.InitSourceItems(GUIEquipInfoPopUp.mData.Info, this.mContentsTable);
				if (this.mContentsTable.mDatas.Count > 0)
				{
					this.mNoSource.gameObject.SetActive(false);
				}
				else
				{
					this.mNoSource.gameObject.SetActive(true);
				}
			}
			else if (GUIEquipInfoPopUp.mData.Info.SubType == 2)
			{
				this.mTitle.text = Singleton<StringManager>.Instance.GetString("equipImprove76");
				this.mRefineTitle.text = Singleton<StringManager>.Instance.GetString("equipImprove63");
				this.mSet.gameObject.SetActive(false);
				this.mFragmentSlider.transform.parent.gameObject.SetActive(false);
				ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GUIEquipInfoPopUp.mData.Info.Value2);
				if (info == null)
				{
					global::Debug.LogErrorFormat("ItemDict get info error , ID : {0}", new object[]
					{
						GUIEquipInfoPopUp.mData.Info.Value2
					});
					return;
				}
				if (Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(info.ID))
				{
					this.mEnhanceBtn.gameObject.SetActive(false);
					this.mRefineBtn.gameObject.SetActive(false);
					this.mRefineContent.gameObject.SetActive(false);
					this.mEnhanceContent.text = Singleton<StringManager>.Instance.GetString("equipImprove68");
					this.mEnhanceValues.text = GUIEquipInfoPopUp.mData.GetTrinketOrItem2EnhanceExp().ToString();
					this.mLegendContent.gameObject.SetActive(false);
				}
				else
				{
					string text;
					string text2;
					Tools.GetEquipEnhanceAttTxt(GUIEquipInfoPopUp.mData, out text, out text2, GUIEquipInfoPopUp.IsLocal);
					this.mEnhanceContent.text = text;
					this.mEnhanceValues.text = text2;
					Tools.GetEquipRefineAttTxt(GUIEquipInfoPopUp.mData, out text, out text2, GUIEquipInfoPopUp.IsLocal);
					this.mRefineContent.text = text;
					this.mRefineValues.text = text2;
					LegendInfo legendInfo4 = Tools.GetLegendInfo(GUIEquipInfoPopUp.mData.Info);
					if (legendInfo4 != null)
					{
						this.mLegendContent.text = Tools.GetLegendSkillStr(legendInfo4, 0);
						this.mLegendContent.gameObject.SetActive(true);
					}
					else
					{
						this.mLegendContent.gameObject.SetActive(false);
					}
				}
				this.mSourceTab.gameObject.SetActive(true);
				this.mInfoTab.gameObject.SetActive(true);
				GUIHowGetPetItemPopUp.InitSourceItems(GUIEquipInfoPopUp.mData.Info, this.mContentsTable);
				if (this.mContentsTable.mDatas.Count > 0)
				{
					this.mNoSource.gameObject.SetActive(false);
				}
				else
				{
					this.mNoSource.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			GameUIPopupManager.GetInstance().PopState(false, null);
		}
		this.mCommonEquipInfoLayer.Refresh(GUIEquipInfoPopUp.mData, true, GUIEquipInfoPopUp.IsLocal);
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
	}

	public void OnCloseBtnClick(GameObject go)
	{
		base.OnButtonBlockerClick();
	}

	private void OnSourceBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIHowGetPetItemPopUp.ShowThis(GUIEquipInfoPopUp.mData.Info);
	}

	private void OnChangeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.CloseImmediate();
		GUISelectEquipBagScene.Change2This(GUIEquipInfoPopUp.mData.GetSocketSlot(), GUIEquipInfoPopUp.mData.GetEquipSlot());
	}

	private void OnUnlocaBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(GUIEquipInfoPopUp.mData.GetSocketSlot());
		if (socket != null)
		{
			PetDataEx pet = socket.GetPet();
			if (pet != null)
			{
				pet.GetAttribute(ref GameUIManager.mInstance.uiState.mOldHpNum, ref GameUIManager.mInstance.uiState.mOldAttackNum, ref GameUIManager.mInstance.uiState.mOldWufangNum, ref GameUIManager.mInstance.uiState.mOldFafangNum);
			}
		}
		MC2S_EquipItem mC2S_EquipItem = new MC2S_EquipItem();
		mC2S_EquipItem.SocketSlot = GUIEquipInfoPopUp.mData.GetSocketSlot();
		mC2S_EquipItem.EquipSlot = GUIEquipInfoPopUp.mData.GetEquipSlot();
		mC2S_EquipItem.ItemID = 0uL;
		Globals.Instance.CliSession.Send(197, mC2S_EquipItem);
	}

	private void OnMsgEquipItemEvent(int socketSlot, int equipSlot)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	public void OnEnhanceBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (GUIEquipInfoPopUp.mCurSelectIndex >= 0)
		{
			GameUIManager.mInstance.uiState.CombatPetSlot = GUIEquipInfoPopUp.mCurSelectIndex;
		}
		if (GUIEquipInfoPopUp.mData.Info.Type == 0)
		{
			GUIEquipUpgradeScene.Change2This(GUIEquipInfoPopUp.mData, GUIEquipUpgradeScene.EUpgradeType.EUT_Enhance, GUIEquipInfoPopUp.mCurSelectIndex);
		}
		else if (GUIEquipInfoPopUp.mData.Info.Type == 1)
		{
			GUITrinketUpgradeScene.Change2This(GUIEquipInfoPopUp.mData, GUITrinketUpgradeScene.EUpgradeType.EUT_Enhance, GUIEquipInfoPopUp.mCurSelectIndex);
		}
		this.CloseImmediate();
	}

	private void OnRefineBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (GUIEquipInfoPopUp.mData.Info.Type == 0)
		{
			if (Globals.Instance.Player.ItemSystem.CanEquipRefine())
			{
				if (GUIEquipInfoPopUp.mCurSelectIndex >= 0)
				{
					GameUIManager.mInstance.uiState.CombatPetSlot = GUIEquipInfoPopUp.mCurSelectIndex;
				}
				GUIEquipUpgradeScene.Change2This(GUIEquipInfoPopUp.mData, GUIEquipUpgradeScene.EUpgradeType.EUT_Refine, GUIEquipInfoPopUp.mCurSelectIndex);
				this.CloseImmediate();
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("equipImprove43", new object[]
				{
					GameConst.GetInt32(11)
				}), 0f, 0f);
			}
		}
		else if (GUIEquipInfoPopUp.mData.Info.Type == 1)
		{
			if (Globals.Instance.Player.ItemSystem.CanTrinketRefine())
			{
				if (GUIEquipInfoPopUp.mCurSelectIndex >= 0)
				{
					GameUIManager.mInstance.uiState.CombatPetSlot = GUIEquipInfoPopUp.mCurSelectIndex;
				}
				GUITrinketUpgradeScene.Change2This(GUIEquipInfoPopUp.mData, GUITrinketUpgradeScene.EUpgradeType.EUT_Refine, GUIEquipInfoPopUp.mCurSelectIndex);
				this.CloseImmediate();
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("equipImprove44", new object[]
				{
					GameConst.GetInt32(13)
				}), 0f, 0f);
			}
		}
	}

	private void CloseImmediate()
	{
		GameUIPopupManager.GetInstance().PopState(true, null);
	}

	public void PlayUI61()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_009");
		NGUITools.SetActive(this.ui61, false);
		NGUITools.SetActive(this.ui61, true);
	}
}
