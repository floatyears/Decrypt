using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GUIEquipMasterInfoPopUp : GameUIBasePopup
{
	private static int index = -1;

	public SocketDataEx mSocketData;

	private EquipMasterTab mEquipEnhanceTab;

	private EquipMasterTab mEquipRefineTab;

	private EquipMasterTab mTrinketEnhanceTab;

	private EquipMasterTab mTrinketRefineTab;

	private UILabel mProgress;

	private UILabel mTip;

	private UILabel mStartingTitle;

	private UILabel mStartingValue;

	private UILabel mNextTitle;

	private UILabel mNextValue;

	private UILabel mCondition;

	private GameObject mEffect;

	private List<EquipMasterItem> mItems = new List<EquipMasterItem>();

	private StringBuilder sb = new StringBuilder();

	private int masterLevel;

	private int needImproveLevel;

	private List<int> attIDs;

	private List<int> attValues;

	public static void ShowThis(SocketDataEx data, int tabIndex = 0)
	{
		if (data == null)
		{
			global::Debug.LogError(new object[]
			{
				"SocketDataEx is null"
			});
			return;
		}
		if (!data.GetEquipMasterState())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove42", 0f, 0f);
			return;
		}
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIEquipMasterInfoPopUp, false, null, null);
		GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(data, tabIndex);
	}

	public static void TryOpen(SocketDataEx data)
	{
		if (GUIEquipMasterInfoPopUp.index >= 0)
		{
			GUIEquipMasterInfoPopUp.ShowThis(data, GUIEquipMasterInfoPopUp.index);
		}
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		GameUITools.RegisterClickEvent("CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseBtnClick), base.gameObject);
		GameObject gameObject = GameUITools.FindGameObject("Tabs", base.gameObject);
		this.mEquipEnhanceTab = GameUITools.FindGameObject("EquipEnhanceTab", gameObject).AddComponent<EquipMasterTab>();
		this.mEquipEnhanceTab.Init(new EquipMasterTab.CheckChangeCallback(this.OnTabClick));
		this.mEquipRefineTab = GameUITools.FindGameObject("EquipRefineTab", gameObject).AddComponent<EquipMasterTab>();
		this.mEquipRefineTab.Init(new EquipMasterTab.CheckChangeCallback(this.OnTabClick));
		this.mTrinketEnhanceTab = GameUITools.FindGameObject("TrinketEnhanceTab", gameObject).AddComponent<EquipMasterTab>();
		this.mTrinketEnhanceTab.Init(new EquipMasterTab.CheckChangeCallback(this.OnTabClick));
		this.mTrinketRefineTab = GameUITools.FindGameObject("TrinketRefineTab", gameObject).AddComponent<EquipMasterTab>();
		this.mTrinketRefineTab.Init(new EquipMasterTab.CheckChangeCallback(this.OnTabClick));
		gameObject = GameUITools.FindGameObject("Info", base.gameObject);
		this.mProgress = GameUITools.FindUILabel("Progress", gameObject);
		this.mTip = GameUITools.FindUILabel("Tip", gameObject);
		this.mStartingTitle = GameUITools.FindUILabel("StartingTitle", gameObject);
		this.mStartingValue = GameUITools.FindUILabel("StartingValue", gameObject);
		this.mNextTitle = GameUITools.FindUILabel("NextTitle", gameObject);
		this.mNextValue = GameUITools.FindUILabel("NextValue", gameObject);
		this.mCondition = GameUITools.FindUILabel("Condition", gameObject);
		this.mEffect = GameUITools.FindGameObject("Effect", gameObject);
		gameObject = GameUITools.FindGameObject("Items", gameObject);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			this.mItems.Add(gameObject.transform.GetChild(i).gameObject.AddComponent<EquipMasterItem>());
			this.mItems[i].Init(this);
		}
	}

	private void OnDestroy()
	{
	}

	public override void InitPopUp(SocketDataEx data, int tabIndex)
	{
		this.mSocketData = data;
		if (!data.GetEquipMasterState())
		{
			GameUIManager.mInstance.ShowMessageTipByKey("equipImprove42", 0f, 0f);
			this.CloseImmediate();
			return;
		}
		if (!data.GetTrinketMasterState())
		{
			this.mTrinketEnhanceTab.IsEnabled = false;
			this.mTrinketRefineTab.IsEnabled = false;
		}
		switch (tabIndex)
		{
		case 0:
			this.mEquipEnhanceTab.value = true;
			break;
		case 1:
			this.mEquipRefineTab.value = true;
			break;
		case 2:
			if (this.mTrinketEnhanceTab.IsEnabled)
			{
				this.mTrinketEnhanceTab.value = true;
			}
			break;
		case 3:
			if (this.mTrinketRefineTab.IsEnabled)
			{
				this.mTrinketRefineTab.value = true;
			}
			break;
		}
	}

	private void OnTabClick(bool value)
	{
		if (value)
		{
			this.mEffect.gameObject.SetActive(true);
			string name = EquipMasterTab.mCurrent.name;
			switch (name)
			{
			case "EquipEnhanceTab":
				GUIEquipMasterInfoPopUp.index = 0;
				this.mProgress.text = Singleton<StringManager>.Instance.GetString("equipImprove73");
				this.mTip.text = Singleton<StringManager>.Instance.GetString("equipImprove75", new object[]
				{
					Singleton<StringManager>.Instance.GetString("equipLb"),
					Singleton<StringManager>.Instance.GetString("improve")
				});
				this.masterLevel = this.mSocketData.EquipMasterEnhanceLevel;
				this.needImproveLevel = Master.GetEquipEnhanceLevel(this.masterLevel + 1);
				for (int i = 0; i < this.mItems.Count; i++)
				{
					this.mItems[i].Refresh(this.mSocketData.GetEquip(i), true, this.masterLevel);
				}
				if (this.masterLevel == 0)
				{
					this.mCondition.text = Singleton<StringManager>.Instance.GetString("equipImprove59", new object[]
					{
						this.needImproveLevel
					});
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove45", new object[]
					{
						this.needImproveLevel
					}));
					this.sb.Append(Singleton<StringManager>.Instance.GetString("equipImprove49"));
					this.mStartingTitle.text = this.sb.ToString();
					this.mStartingValue.text = string.Empty;
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove55", new object[]
					{
						this.masterLevel + 1
					}));
					this.mNextTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetEquipEnhanceAttInfos(this.masterLevel + 1, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num2 = 0;
						while (num2 < this.attIDs.Count && num2 < this.attValues.Count)
						{
							this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num2], this.attValues[num2]));
							num2++;
						}
					}
					this.mNextValue.text = this.sb.ToString();
				}
				else if (this.masterLevel >= GameConst.GetInt32(236))
				{
					this.mEffect.SetActive(false);
					this.mCondition.text = string.Empty;
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove55", new object[]
					{
						this.masterLevel
					}));
					this.mStartingTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetEquipEnhanceAttInfos(this.masterLevel, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num3 = 0;
						while (num3 < this.attIDs.Count && num3 < this.attValues.Count)
						{
							if (this.attValues[num3] > 0)
							{
								this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num3], this.attValues[num3]));
							}
							num3++;
						}
					}
					this.mStartingValue.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove50"));
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove54"));
					this.mNextTitle.text = this.sb.ToString();
					this.mNextValue.text = string.Empty;
				}
				else
				{
					this.mCondition.text = Singleton<StringManager>.Instance.GetString("equipImprove59", new object[]
					{
						this.needImproveLevel
					});
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove55", new object[]
					{
						this.masterLevel
					}));
					this.mStartingTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetEquipEnhanceAttInfos(this.masterLevel, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num4 = 0;
						while (num4 < this.attIDs.Count && num4 < this.attValues.Count)
						{
							if (this.attValues[num4] > 0)
							{
								this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num4], this.attValues[num4]));
							}
							num4++;
						}
					}
					this.mStartingValue.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove55", new object[]
					{
						this.masterLevel + 1
					}));
					this.mNextTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetEquipEnhanceAttInfos(this.masterLevel + 1, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num5 = 0;
						while (num5 < this.attIDs.Count && num5 < this.attValues.Count)
						{
							this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num5], this.attValues[num5]));
							num5++;
						}
					}
					this.mNextValue.text = this.sb.ToString();
				}
				break;
			case "EquipRefineTab":
				GUIEquipMasterInfoPopUp.index = 1;
				this.mProgress.text = Singleton<StringManager>.Instance.GetString("equipImprove74");
				this.mTip.text = Singleton<StringManager>.Instance.GetString("equipImprove75", new object[]
				{
					Singleton<StringManager>.Instance.GetString("equipLb"),
					Singleton<StringManager>.Instance.GetString("refine")
				});
				this.masterLevel = this.mSocketData.EquipMasterRefineLevel;
				this.needImproveLevel = Master.GetEquipRefineLevel(this.masterLevel + 1);
				for (int j = 0; j < this.mItems.Count; j++)
				{
					this.mItems[j].Refresh(this.mSocketData.GetEquip(j), false, this.masterLevel);
				}
				if (this.masterLevel == 0)
				{
					this.mCondition.text = Singleton<StringManager>.Instance.GetString("equipImprove60", new object[]
					{
						this.needImproveLevel
					});
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove46", new object[]
					{
						this.needImproveLevel
					}));
					this.sb.Append(Singleton<StringManager>.Instance.GetString("equipImprove49"));
					this.mStartingTitle.text = this.sb.ToString();
					this.mStartingValue.text = string.Empty;
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove56", new object[]
					{
						this.masterLevel + 1
					}));
					this.mNextTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetEquipRefineAttInfos(this.masterLevel + 1, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num6 = 0;
						while (num6 < this.attIDs.Count && num6 < this.attValues.Count)
						{
							this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num6], this.attValues[num6]));
							num6++;
						}
					}
					this.mNextValue.text = this.sb.ToString();
				}
				else if (this.masterLevel >= GameConst.GetInt32(237))
				{
					this.mEffect.SetActive(false);
					this.mCondition.text = string.Empty;
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove56", new object[]
					{
						this.masterLevel
					}));
					this.mStartingTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetEquipRefineAttInfos(this.masterLevel, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num7 = 0;
						while (num7 < this.attIDs.Count && num7 < this.attValues.Count)
						{
							if (this.attValues[num7] > 0)
							{
								this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num7], this.attValues[num7]));
							}
							num7++;
						}
					}
					this.mStartingValue.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove51"));
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove54"));
					this.mNextTitle.text = this.sb.ToString();
					this.mNextValue.text = string.Empty;
				}
				else
				{
					this.mCondition.text = Singleton<StringManager>.Instance.GetString("equipImprove60", new object[]
					{
						this.needImproveLevel
					});
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove56", new object[]
					{
						this.masterLevel
					}));
					this.mStartingTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetEquipRefineAttInfos(this.masterLevel, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num8 = 0;
						while (num8 < this.attIDs.Count && num8 < this.attValues.Count)
						{
							if (this.attValues[num8] > 0)
							{
								this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num8], this.attValues[num8]));
							}
							num8++;
						}
					}
					this.mStartingValue.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove56", new object[]
					{
						this.masterLevel + 1
					}));
					this.mNextTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetEquipRefineAttInfos(this.masterLevel + 1, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num9 = 0;
						while (num9 < this.attIDs.Count && num9 < this.attValues.Count)
						{
							this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num9], this.attValues[num9]));
							num9++;
						}
					}
					this.mNextValue.text = this.sb.ToString();
				}
				break;
			case "TrinketEnhanceTab":
				GUIEquipMasterInfoPopUp.index = 2;
				this.mProgress.text = Singleton<StringManager>.Instance.GetString("equipImprove73");
				this.mTip.text = Singleton<StringManager>.Instance.GetString("equipImprove75", new object[]
				{
					Singleton<StringManager>.Instance.GetString("shengQiLb"),
					Singleton<StringManager>.Instance.GetString("improve")
				});
				this.masterLevel = this.mSocketData.TrinketMasterEnhanceLevel;
				this.needImproveLevel = Master.GetTrinketEnhanceLevel(this.masterLevel + 1);
				for (int k = 0; k < this.mItems.Count; k++)
				{
					if (k % 2 == 1)
					{
						this.mItems[k].Refresh(null, true, this.masterLevel);
					}
					else
					{
						this.mItems[k].Refresh(this.mSocketData.GetEquip(k / 2 + 4), true, this.masterLevel);
					}
				}
				if (this.masterLevel == 0)
				{
					this.mCondition.text = Singleton<StringManager>.Instance.GetString("equipImprove61", new object[]
					{
						this.needImproveLevel
					});
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove47", new object[]
					{
						this.needImproveLevel
					}));
					this.sb.Append(Singleton<StringManager>.Instance.GetString("equipImprove49"));
					this.mStartingTitle.text = this.sb.ToString();
					this.mStartingValue.text = string.Empty;
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove57", new object[]
					{
						this.masterLevel + 1
					}));
					this.mNextTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetTrinketEnhanceAttInfos(this.masterLevel + 1, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num10 = 0;
						while (num10 < this.attIDs.Count && num10 < this.attValues.Count)
						{
							this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num10], this.attValues[num10]));
							num10++;
						}
					}
					this.mNextValue.text = this.sb.ToString();
				}
				else if (this.masterLevel >= GameConst.GetInt32(238))
				{
					this.mEffect.SetActive(false);
					this.mCondition.text = string.Empty;
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove57", new object[]
					{
						this.masterLevel
					}));
					this.mStartingTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetTrinketEnhanceAttInfos(this.masterLevel, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num11 = 0;
						while (num11 < this.attIDs.Count && num11 < this.attValues.Count)
						{
							if (this.attValues[num11] > 0)
							{
								this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num11], this.attValues[num11]));
							}
							num11++;
						}
					}
					this.mStartingValue.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove52"));
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove54"));
					this.mNextTitle.text = this.sb.ToString();
					this.mNextValue.text = string.Empty;
				}
				else
				{
					this.mCondition.text = Singleton<StringManager>.Instance.GetString("equipImprove61", new object[]
					{
						this.needImproveLevel
					});
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove57", new object[]
					{
						this.masterLevel
					}));
					this.mStartingTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetTrinketEnhanceAttInfos(this.masterLevel, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num12 = 0;
						while (num12 < this.attIDs.Count && num12 < this.attValues.Count)
						{
							if (this.attValues[num12] > 0)
							{
								this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num12], this.attValues[num12]));
							}
							num12++;
						}
					}
					this.mStartingValue.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove57", new object[]
					{
						this.masterLevel + 1
					}));
					this.mNextTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetTrinketEnhanceAttInfos(this.masterLevel + 1, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num13 = 0;
						while (num13 < this.attIDs.Count && num13 < this.attValues.Count)
						{
							this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num13], this.attValues[num13]));
							num13++;
						}
					}
					this.mNextValue.text = this.sb.ToString();
				}
				break;
			case "TrinketRefineTab":
				GUIEquipMasterInfoPopUp.index = 3;
				this.mProgress.text = Singleton<StringManager>.Instance.GetString("equipImprove74");
				this.mTip.text = Singleton<StringManager>.Instance.GetString("equipImprove75", new object[]
				{
					Singleton<StringManager>.Instance.GetString("shengQiLb"),
					Singleton<StringManager>.Instance.GetString("refine")
				});
				this.masterLevel = this.mSocketData.TrinketMasterRefineLevel;
				this.needImproveLevel = Master.GetTrinketRefineLevel(this.masterLevel + 1);
				for (int l = 0; l < this.mItems.Count; l++)
				{
					if (l % 2 == 1)
					{
						this.mItems[l].Refresh(null, false, this.masterLevel);
					}
					else
					{
						this.mItems[l].Refresh(this.mSocketData.GetEquip(l / 2 + 4), false, this.masterLevel);
					}
				}
				if (this.masterLevel == 0)
				{
					this.mCondition.text = Singleton<StringManager>.Instance.GetString("equipImprove62", new object[]
					{
						this.needImproveLevel
					});
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove48", new object[]
					{
						this.needImproveLevel
					}));
					this.sb.Append(Singleton<StringManager>.Instance.GetString("equipImprove49"));
					this.mStartingTitle.text = this.sb.ToString();
					this.mStartingValue.text = string.Empty;
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove58", new object[]
					{
						this.masterLevel + 1
					}));
					this.mNextTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetTrinketRefineAttInfos(this.masterLevel + 1, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num14 = 0;
						while (num14 < this.attIDs.Count && num14 < this.attValues.Count)
						{
							this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num14], this.attValues[num14]));
							num14++;
						}
					}
					this.mNextValue.text = this.sb.ToString();
				}
				else if (this.masterLevel >= GameConst.GetInt32(239))
				{
					this.mEffect.SetActive(false);
					this.mCondition.text = string.Empty;
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove58", new object[]
					{
						this.masterLevel
					}));
					this.mStartingTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetTrinketRefineAttInfos(this.masterLevel, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num15 = 0;
						while (num15 < this.attIDs.Count && num15 < this.attValues.Count)
						{
							if (this.attValues[num15] > 0)
							{
								this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num15], this.attValues[num15]));
							}
							num15++;
						}
					}
					this.mStartingValue.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove53"));
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove54"));
					this.mNextTitle.text = this.sb.ToString();
					this.mNextValue.text = string.Empty;
				}
				else
				{
					this.mCondition.text = Singleton<StringManager>.Instance.GetString("equipImprove62", new object[]
					{
						this.needImproveLevel
					});
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove58", new object[]
					{
						this.masterLevel
					}));
					this.mStartingTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetTrinketRefineAttInfos(this.masterLevel, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num16 = 0;
						while (num16 < this.attIDs.Count && num16 < this.attValues.Count)
						{
							if (this.attValues[num16] > 0)
							{
								this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num16], this.attValues[num16]));
							}
							num16++;
						}
					}
					this.mStartingValue.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					this.sb.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove58", new object[]
					{
						this.masterLevel + 1
					}));
					this.mNextTitle.text = this.sb.ToString();
					this.sb.Remove(0, this.sb.Length);
					Master.GetTrinketRefineAttInfos(this.masterLevel + 1, out this.attIDs, out this.attValues);
					if (this.attIDs != null && this.attValues != null)
					{
						int num17 = 0;
						while (num17 < this.attIDs.Count && num17 < this.attValues.Count)
						{
							this.sb.AppendLine(Tools.GetETAttStr(this.attIDs[num17], this.attValues[num17]));
							num17++;
						}
					}
					this.mNextValue.text = this.sb.ToString();
				}
				break;
			}
		}
	}

	private void OnCloseBtnClick(GameObject go)
	{
		GUIEquipMasterInfoPopUp.index = -1;
		base.OnButtonBlockerClick();
	}

	public override void OnButtonBlockerClick()
	{
		GUIEquipMasterInfoPopUp.index = -1;
		base.OnButtonBlockerClick();
	}

	public void CloseImmediate()
	{
		GameUIPopupManager.GetInstance().PopState(true, null);
	}
}
