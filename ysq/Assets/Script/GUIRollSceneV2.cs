using Att;
using Proto;
using System;
using UnityEngine;

public class GUIRollSceneV2 : GameUISession
{
	public enum EFreeState
	{
		EFreeState_Time_To,
		EFreeState_Free,
		EFreeState_Out
	}

	private UISprite lowRed;

	private UILabel lowFreeTime;

	private UISprite lowCostIcon;

	private UILabel lowCostValue;

	private UILabel lowCostFree;

	private UISprite highRed;

	private UILabel highFreeTime;

	private UISprite highCostIcon;

	private UILabel highCostValue;

	private UILabel highCostFree;

	private UILabel highNextDesc;

	private float timerRefresh;

	private GUIRollSceneV2.EFreeState lowFreeState;

	private GUIRollSceneV2.EFreeState highFreeState;

	private int lowItemCount;

	private int highItemCount;

	private ActivityValueData mActivityValueData;

	protected override void OnPostLoadGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Show("roll");
		this.CreateObjects();
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUITools.CompleteAllHotween();
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	private void CreateObjects()
	{
		this.mActivityValueData = Globals.Instance.Player.ActivitySystem.GetValueMod(5);
		this.lowItemCount = Globals.Instance.Player.ItemSystem.GetLowRollItemCount();
		this.highItemCount = Globals.Instance.Player.ItemSystem.GetHighRollItemCount();
		GameObject parent = GameUITools.RegisterClickEvent("Low", new UIEventListener.VoidDelegate(this.OnLowClick), base.gameObject);
		this.lowRed = GameUITools.FindUISprite("Red", parent);
		GameUITools.RegisterClickEvent("View", new UIEventListener.VoidDelegate(this.OnLowViewClick), parent);
		this.lowFreeTime = GameUITools.FindUILabel("FreeTime", parent);
		parent = GameUITools.FindGameObject("Cost", parent);
		this.lowCostIcon = GameUITools.FindUISprite("Icon", parent);
		this.lowCostFree = GameUITools.FindUILabel("Free", parent);
		this.lowCostValue = GameUITools.FindUILabel("Value", parent);
		parent = GameUITools.RegisterClickEvent("High", new UIEventListener.VoidDelegate(this.OnHighClick), base.gameObject);
		this.highRed = GameUITools.FindUISprite("Red", parent);
		this.highNextDesc = GameUITools.FindUILabel("NextDesc", parent);
		GameUITools.RegisterClickEvent("View", new UIEventListener.VoidDelegate(this.OnHighViewClick), parent);
		this.highFreeTime = GameUITools.FindUILabel("FreeTime", parent);
		parent = GameUITools.FindGameObject("Cost", parent);
		this.highCostIcon = GameUITools.FindUISprite("Icon", parent);
		this.highCostFree = GameUITools.FindUILabel("Free", parent);
		this.highCostValue = GameUITools.FindUILabel("Value", parent);
		LuckyRollInfo info = Globals.Instance.AttDB.LuckyRollDict.GetInfo(1);
		int luckyRoll2Count = Globals.Instance.Player.Data.LuckyRoll2Count;
		int num = info.Count[0];
		int num2 = info.Count[1] + num;
		if (luckyRoll2Count < num2 - 1)
		{
			this.highNextDesc.text = Singleton<StringManager>.Instance.GetString("rollNextDesc", new object[]
			{
				num2 - luckyRoll2Count,
				Singleton<StringManager>.Instance.GetString("rollOrangePet")
			});
		}
		else if (luckyRoll2Count == num2 - 1)
		{
			this.highNextDesc.text = Singleton<StringManager>.Instance.GetString("rollThisTime", new object[]
			{
				Singleton<StringManager>.Instance.GetString("rollOrangePet")
			});
		}
		else
		{
			global::Debug.LogErrorFormat("highRollCount error : {0} ", new object[]
			{
				luckyRoll2Count
			});
			this.highNextDesc.text = luckyRoll2Count.ToString();
		}
		this.lowCostValue.text = GameConst.GetInt32(37).ToString();
		if (Globals.Instance.Player.ItemSystem.GetLowRollItemCount() < GameConst.GetInt32(37))
		{
			this.lowCostValue.color = Color.red;
		}
		if (GUIRollSceneV2.IsLowRollFree())
		{
			this.ChangeFreeState(0, 1);
		}
		else
		{
			this.ChangeFreeState(0, 0);
		}
		if (GUIRollSceneV2.IsHighRollFree())
		{
			this.ChangeFreeState(1, 1);
		}
		else
		{
			this.ChangeFreeState(1, 0);
		}
	}

	private void OnLowViewClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIPetViewPopUp.Show(GUIRollingSceneV2.ERollType.ERollType_Low);
	}

	private void OnHighViewClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIPetViewPopUp.Show(GUIRollingSceneV2.ERollType.ERollType_high);
	}

	private void OnLowClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIRollingSceneV2.Change2This(GUIRollingSceneV2.ERollType.ERollType_Low);
	}

	public void OnHighClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIRollingSceneV2.Change2This(GUIRollingSceneV2.ERollType.ERollType_high);
	}

	private void Update()
	{
		if (base.PostLoadGUIDone && Time.time - this.timerRefresh > 1f)
		{
			this.timerRefresh = Time.time;
			this.Refresh();
		}
	}

	private void Refresh()
	{
		if (Globals.Instance.Player.Data.FreeLuckyRollCount < GameConst.GetInt32(44))
		{
			if (GUIRollSceneV2.IsLowRollFree())
			{
				if (this.lowFreeState != GUIRollSceneV2.EFreeState.EFreeState_Free)
				{
					this.ChangeFreeState(0, 1);
				}
			}
			else if (this.lowFreeState == GUIRollSceneV2.EFreeState.EFreeState_Time_To)
			{
				this.lowFreeTime.text = Singleton<StringManager>.Instance.GetString("rollTimeToFree", new object[]
				{
					this.GetTimeToFree(true)
				});
			}
			else
			{
				this.ChangeFreeState(0, 0);
			}
		}
		else if (this.lowFreeState != GUIRollSceneV2.EFreeState.EFreeState_Out)
		{
			this.ChangeFreeState(0, 0);
		}
		if (GUIRollSceneV2.IsHighRollFree())
		{
			if (this.highFreeState == GUIRollSceneV2.EFreeState.EFreeState_Time_To)
			{
				this.ChangeFreeState(1, 1);
			}
		}
		else if (this.highFreeState == GUIRollSceneV2.EFreeState.EFreeState_Time_To)
		{
			this.highFreeTime.text = Singleton<StringManager>.Instance.GetString("rollTimeToFree", new object[]
			{
				this.GetTimeToFree(false)
			});
		}
		else
		{
			this.ChangeFreeState(1, 0);
		}
	}

	public static bool IsLowRollFree()
	{
		return Globals.Instance.Player.Data.FreeLuckyRollCount < GameConst.GetInt32(44) && Globals.Instance.Player.Data.FreeLuckyRollCD1 < Globals.Instance.Player.GetTimeStamp();
	}

	public static bool IsHighRollFree()
	{
		return Globals.Instance.Player.Data.FreeLuckyRollCD2 < Globals.Instance.Player.GetTimeStamp();
	}

	private string GetTimeToFree(bool isLowRoll)
	{
		if (isLowRoll)
		{
			return UIEnergyTooltip.FormatTime((Globals.Instance.Player.Data.FreeLuckyRollCD1 - Globals.Instance.Player.GetTimeStamp() > GameConst.GetInt32(45)) ? GameConst.GetInt32(45) : (Globals.Instance.Player.Data.FreeLuckyRollCD1 - Globals.Instance.Player.GetTimeStamp()));
		}
		return UIEnergyTooltip.FormatTime((Globals.Instance.Player.Data.FreeLuckyRollCD2 - Globals.Instance.Player.GetTimeStamp() > GameConst.GetInt32(46)) ? GameConst.GetInt32(46) : (Globals.Instance.Player.Data.FreeLuckyRollCD2 - Globals.Instance.Player.GetTimeStamp()));
	}

	private void ChangeFreeState(int type, int state)
	{
		if (type == 0)
		{
			if (state == 0)
			{
				if (Globals.Instance.Player.Data.FreeLuckyRollCount < GameConst.GetInt32(44))
				{
					this.lowFreeState = GUIRollSceneV2.EFreeState.EFreeState_Time_To;
					this.lowFreeTime.text = Singleton<StringManager>.Instance.GetString("rollTimeToFree", new object[]
					{
						this.GetTimeToFree(true)
					});
				}
				else
				{
					this.lowFreeState = GUIRollSceneV2.EFreeState.EFreeState_Out;
					this.lowFreeTime.text = Singleton<StringManager>.Instance.GetString("rollFreeOut");
				}
				this.lowCostFree.gameObject.SetActive(false);
				this.lowCostIcon.gameObject.SetActive(true);
				this.lowCostValue.gameObject.SetActive(true);
				if (this.lowItemCount >= GameConst.GetInt32(38))
				{
					this.lowRed.enabled = true;
				}
				else
				{
					this.lowRed.enabled = false;
				}
			}
			else
			{
				this.lowFreeState = GUIRollSceneV2.EFreeState.EFreeState_Free;
				this.lowFreeTime.text = Singleton<StringManager>.Instance.GetString("rollFreeTimes", new object[]
				{
					GameConst.GetInt32(44) - Globals.Instance.Player.Data.FreeLuckyRollCount,
					GameConst.GetInt32(44)
				});
				this.lowCostFree.gameObject.SetActive(true);
				this.lowCostIcon.gameObject.SetActive(false);
				this.lowCostValue.gameObject.SetActive(false);
				this.lowRed.enabled = true;
			}
		}
		else if (state == 0)
		{
			this.highFreeState = GUIRollSceneV2.EFreeState.EFreeState_Time_To;
			this.highFreeTime.text = Singleton<StringManager>.Instance.GetString("rollTimeToFree", new object[]
			{
				this.GetTimeToFree(false)
			});
			this.highCostFree.gameObject.SetActive(false);
			this.highCostIcon.gameObject.SetActive(true);
			this.highCostValue.gameObject.SetActive(true);
			if (this.highItemCount >= GameConst.GetInt32(39))
			{
				this.highRed.enabled = true;
				this.highCostIcon.spriteName = "highItem";
				this.highCostValue.text = GameConst.GetInt32(39).ToString();
			}
			else
			{
				this.highCostIcon.spriteName = "redGem_1";
				this.highRed.enabled = false;
				if (this.mActivityValueData == null)
				{
					this.highCostValue.text = GameConst.GetInt32(41).ToString();
					this.highCostValue.color = Color.white;
				}
				else
				{
					this.highCostValue.text = Singleton<StringManager>.Instance.GetString("ShopCommon7", new object[]
					{
						GameConst.GetInt32(41) * this.mActivityValueData.Value1 / 100,
						Tools.FormatOffPrice(this.mActivityValueData.Value1)
					});
					this.highCostValue.color = Color.yellow;
				}
			}
		}
		else
		{
			this.highFreeState = GUIRollSceneV2.EFreeState.EFreeState_Free;
			this.highFreeTime.text = string.Empty;
			this.highCostFree.gameObject.SetActive(true);
			this.highCostIcon.gameObject.SetActive(false);
			this.highCostValue.gameObject.SetActive(false);
			this.highRed.enabled = true;
		}
	}

	public static int GetRollOnePrice()
	{
		ActivityValueData valueMod = Globals.Instance.Player.ActivitySystem.GetValueMod(5);
		if (valueMod == null)
		{
			return GameConst.GetInt32(41);
		}
		return GameConst.GetInt32(41) * valueMod.Value1 / 100;
	}
}
