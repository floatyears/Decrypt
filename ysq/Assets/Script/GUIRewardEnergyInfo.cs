using Holoville.HOTween;
using Proto;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GUIRewardEnergyInfo : MonoBehaviour
{
	public GUIReward mBaseScene;

	private GameObject takeDayEnergy;

	private GameObject mEnergyBg;

	private UISprite mNew;

	private UILabel mPlusKeys;

	public void InitWithBaseScene(GUIReward basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.takeDayEnergy = GameUITools.RegisterClickEvent("Take", new UIEventListener.VoidDelegate(this.OnTakeEnergyClick), base.gameObject);
		this.mEnergyBg = GameUITools.FindGameObject("Key/Flare", base.gameObject);
		this.mPlusKeys = GameUITools.FindUILabel("PlusKeys", base.gameObject);
		GameUITools.FindUILabel("Desc", base.gameObject).text = Singleton<StringManager>.Instance.GetString("takeEnergy", new object[]
		{
			GameConst.GetInt32(19)
		});
		GameUITools.SetLabelLocalText("Label", "takeEnergyBtn", this.takeDayEnergy);
	}

	private void RefreshPlusKeys()
	{
		this.mPlusKeys.text = Singleton<StringManager>.Instance.GetString("activityKeysPlusKeys", new object[]
		{
			GameConst.GetInt32(135)
		});
	}

	public void Refresh()
	{
		this.mBaseScene.RefreshRewardKeysFlag();
		this.RefreshPlusKeys();
		this.RefreshDayEnergy();
	}

	private void Update()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		this.RefreshDayEnergy();
	}

	private void RefreshDayEnergy()
	{
		int num = 0;
		if (this.CanTakeDayEnergy(false, out num))
		{
			this.takeDayEnergy.SetActive(true);
			if (!HOTween.IsTweening(this.mEnergyBg.transform))
			{
				HOTween.To(this.mEnergyBg.transform, 3f, new TweenParms().Prop("localRotation", new Vector3(0f, 0f, 360f), true).Loops(-1).Ease(EaseType.Linear));
			}
		}
		else
		{
			this.takeDayEnergy.SetActive(false);
			if (HOTween.IsTweening(this.mEnergyBg.transform))
			{
				HOTween.Kill(this.mEnergyBg.transform);
			}
		}
	}

	private bool CanTakeDayEnergy(bool showTips, out int value)
	{
		value = 0;
		DateTime dateTime = Tools.ServerDateTime(Globals.Instance.Player.GetTimeStamp());
		if (dateTime.Hour >= 12 && dateTime.Hour < 14)
		{
			if ((Globals.Instance.Player.Data.DataFlag & 1) != 0)
			{
				if (showTips)
				{
					GameUIManager.mInstance.ShowMessageTipByKey("PlayerR_8", 0f, 0f);
				}
				return false;
			}
		}
		else if (dateTime.Hour >= 17 && dateTime.Hour < 19)
		{
			if ((Globals.Instance.Player.Data.DataFlag & 2) != 0)
			{
				if (showTips)
				{
					GameUIManager.mInstance.ShowMessageTipByKey("PlayerR_8", 0f, 0f);
				}
				return false;
			}
			value = 1;
		}
		else
		{
			if ((Globals.Instance.Player.Data.VipLevel <= 0u && (ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(19))) || dateTime.Hour < 20 || dateTime.Hour >= 22)
			{
				if (showTips)
				{
					GameUIManager.mInstance.ShowMessageTipByKey("PlayerR_9", 0f, 0f);
				}
				return false;
			}
			if ((Globals.Instance.Player.Data.DataFlag & 2048) != 0)
			{
				if (showTips)
				{
					GameUIManager.mInstance.ShowMessageTipByKey("PlayerR_8", 0f, 0f);
				}
				return false;
			}
			value = 2;
		}
		return true;
	}

	public static bool HasUnTakedKeys()
	{
		DateTime dateTime = Tools.ServerDateTime(Globals.Instance.Player.GetTimeStamp());
		if (dateTime.Hour >= 12 && dateTime.Hour < 14)
		{
			if ((Globals.Instance.Player.Data.DataFlag & 1) == 0)
			{
				return true;
			}
		}
		else if (dateTime.Hour >= 17 && dateTime.Hour < 19)
		{
			if ((Globals.Instance.Player.Data.DataFlag & 2) == 0)
			{
				return true;
			}
		}
		else if ((Globals.Instance.Player.Data.VipLevel > 0u || (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(19))) && dateTime.Hour >= 20 && dateTime.Hour < 22 && (Globals.Instance.Player.Data.DataFlag & 2048) == 0)
		{
			return true;
		}
		return false;
	}

	public void OnMsgGetDayEnergy(MemoryStream stream)
	{
		MS2C_GetDayEnergy mS2C_GetDayEnergy = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetDayEnergy), stream) as MS2C_GetDayEnergy;
		if (mS2C_GetDayEnergy.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_GetDayEnergy.Result);
			return;
		}
		this.RefreshDayEnergy();
		this.mBaseScene.RefreshRewardKeysFlag();
		GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("activityKey", new object[]
		{
			GameConst.GetInt32(135)
		}), 0f, 0f);
		if (mS2C_GetDayEnergy.Event != 0)
		{
			GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIDayEnergyEvent, false, null, null);
			GameUIPopupManager.GetInstance().GetCurrentPopup().InitPopUp(mS2C_GetDayEnergy.Event, mS2C_GetDayEnergy.Value);
		}
	}

	private void OnTakeEnergyClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_009");
		int flag = 0;
		if (!this.CanTakeDayEnergy(true, out flag))
		{
			return;
		}
		MC2S_GetDayEnergy mC2S_GetDayEnergy = new MC2S_GetDayEnergy();
		mC2S_GetDayEnergy.Flag = flag;
		Globals.Instance.CliSession.Send(206, mC2S_GetDayEnergy);
	}
}
