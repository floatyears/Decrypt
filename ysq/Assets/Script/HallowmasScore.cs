using System;
using UnityEngine;

public class HallowmasScore : MonoBehaviour
{
	private int DailyScoreIndex = -1;

	private GameObject mCloseBtn;

	private Transform[] item = new Transform[4];

	private GameObject[] itemReward = new GameObject[4];

	private UILabel tip;

	private UIButton BtnChecked;

	private GameObject finished;

	public void Init()
	{
		Transform transform = base.transform.Find("BG");
		this.mCloseBtn = transform.Find("closeBtn").gameObject;
		UIEventListener expr_32 = UIEventListener.Get(this.mCloseBtn);
		expr_32.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_32.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClicked));
		Transform transform2 = transform.transform.Find("itemBg");
		for (int i = 0; i < 4; i++)
		{
			this.item[i] = transform2.Find(string.Format("Item{0}", i));
		}
		this.tip = transform.Find("tip").GetComponent<UILabel>();
		this.BtnChecked = transform.Find("BtnChecked").GetComponent<UIButton>();
		UIEventListener expr_D0 = UIEventListener.Get(this.BtnChecked.gameObject);
		expr_D0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D0.onClick, new UIEventListener.VoidDelegate(this.OnTakeAchievementReward));
		this.finished = transform.Find("get").gameObject;
	}

	public void Show(int scoreIndex)
	{
		this.DailyScoreIndex = scoreIndex;
		if (scoreIndex < 0 || scoreIndex >= Globals.Instance.Player.ActivitySystem.HData.Ext.ScoreReward.Count)
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		for (int i = 0; i < this.itemReward.Length; i++)
		{
			if (this.itemReward[i] != null)
			{
				UnityEngine.Object.Destroy(this.itemReward[i]);
				this.itemReward[i] = null;
			}
		}
		int num = 0;
		if (player.ActivitySystem.HData.Ext.ScoreReward != null)
		{
			for (int j = 0; j < player.ActivitySystem.HData.Ext.ScoreReward[scoreIndex].Rewards.Count; j++)
			{
				this.itemReward[num] = GameUITools.CreateReward(player.ActivitySystem.HData.Ext.ScoreReward[scoreIndex].Rewards[j].RewardType, player.ActivitySystem.HData.Ext.ScoreReward[scoreIndex].Rewards[j].RewardValue1, player.ActivitySystem.HData.Ext.ScoreReward[scoreIndex].Rewards[j].RewardValue2, this.item[num], true, true, 0f, 20f, -2000f, 20f, 13f, 7f, 0);
				if (this.itemReward[num] != null)
				{
					num++;
				}
			}
		}
		this.tip.text = Singleton<StringManager>.Instance.GetString("QuestTxt6", new object[]
		{
			Globals.Instance.Player.ActivitySystem.HData.Ext.ScoreReward[scoreIndex].Score
		});
		this.IsTakeBtnTrue(scoreIndex);
		base.gameObject.SetActive(true);
	}

	public void IsTakeBtnTrue(int scoreIndex)
	{
		if (Globals.Instance.Player.ActivitySystem.PlayerScore < Globals.Instance.Player.ActivitySystem.HData.Ext.ScoreReward[scoreIndex].Score)
		{
			this.BtnChecked.gameObject.SetActive(true);
			this.BtnChecked.isEnabled = false;
			this.finished.gameObject.SetActive(false);
		}
		else if (GUIHallowmasCupScene.IsGet(scoreIndex))
		{
			this.BtnChecked.gameObject.SetActive(false);
			this.finished.gameObject.SetActive(true);
		}
		else
		{
			this.BtnChecked.gameObject.SetActive(true);
			this.BtnChecked.isEnabled = true;
			this.finished.gameObject.SetActive(false);
		}
	}

	private void OnTakeAchievementReward(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GUIHallowmasCupScene.RequestTalkScoreReward(this.DailyScoreIndex);
		base.gameObject.SetActive(false);
	}

	private void OnCloseBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		base.gameObject.SetActive(false);
	}
}
