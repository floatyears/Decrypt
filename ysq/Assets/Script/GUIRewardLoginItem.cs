using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIRewardLoginItem : MonoBehaviour
{
	private UILabel mName;

	private UIButton mReceiveBtn;

	private UIButton[] mReceiveBtns;

	private GameObject mReceived;

	private List<Transform> slots = new List<Transform>();

	private GameObject[] rewards = new GameObject[10];

	public void Init()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mReceiveBtn = GameUITools.RegisterClickEvent("ReceiveBtn", new UIEventListener.VoidDelegate(this.OnReceiveBtnClick), base.gameObject).GetComponent<UIButton>();
		this.mReceiveBtns = this.mReceiveBtn.GetComponents<UIButton>();
		this.mReceived = GameUITools.FindGameObject("Received", base.gameObject);
		GameObject gameObject = GameUITools.FindGameObject("Rewards", base.gameObject);
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			this.slots.Add(gameObject.transform.GetChild(i).transform);
		}
	}

	public void Refresh()
	{
		MS2C_HotTimeData rewardActivityHotTimeData = Globals.Instance.Player.ActivitySystem.RewardActivityHotTimeData;
		this.mName.text = rewardActivityHotTimeData.GiftName;
		if ((Globals.Instance.Player.Data.DataFlag & 33554432) != 0)
		{
			this.mReceiveBtn.gameObject.SetActive(false);
			this.mReceived.SetActive(true);
		}
		else
		{
			this.mReceiveBtn.gameObject.SetActive(true);
			this.mReceived.SetActive(false);
			if (!rewardActivityHotTimeData.InTime)
			{
				this.mReceiveBtn.isEnabled = false;
				for (int i = 0; i < this.mReceiveBtns.Length; i++)
				{
					this.mReceiveBtns[i].SetState(UIButtonColor.State.Disabled, true);
				}
			}
			else
			{
				this.mReceiveBtn.isEnabled = true;
				for (int j = 0; j < this.mReceiveBtns.Length; j++)
				{
					this.mReceiveBtns[j].SetState(UIButtonColor.State.Normal, true);
				}
			}
		}
		for (int k = 0; k < this.rewards.Length; k++)
		{
			UnityEngine.Object.Destroy(this.rewards[k]);
			this.rewards[k] = null;
		}
		int num = 0;
		int num2 = 0;
		while (num2 < rewardActivityHotTimeData.Reward.Count && num2 < 10)
		{
			if (rewardActivityHotTimeData.Reward[num2].RewardType != 0)
			{
				this.rewards[num] = GameUITools.CreateReward(rewardActivityHotTimeData.Reward[num2].RewardType, rewardActivityHotTimeData.Reward[num2].RewardValue1, rewardActivityHotTimeData.Reward[num2].RewardValue2, this.slots[num], true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
				if (this.rewards[num] == null)
				{
					num++;
				}
			}
			num2++;
		}
	}

	private void OnReceiveBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		MC2S_TakeHotTimeReward ojb = new MC2S_TakeHotTimeReward();
		Globals.Instance.CliSession.Send(741, ojb);
	}
}
