using Proto;
using System;
using UnityEngine;

public class GUIRewardScratchOffPop : MonoBehaviour
{
	private string[] nums = new string[]
	{
		"888",
		"666",
		"080",
		"222",
		"100",
		"200",
		"300",
		"400",
		"500",
		"600",
		"700",
		"800",
		"900",
		"333",
		"444",
		"555",
		"666",
		"777",
		"888",
		"999",
		"128",
		"158",
		"188",
		"288",
		"388",
		"010",
		"020",
		"050",
		"030",
		"060"
	};

	private int result;

	public string[] mCurNums = new string[3];

	private GUIRewardCard[] cardArr = new GUIRewardCard[3];

	public GUIRewardScratchOffInfo mBaseScene;

	public int hasScratchNum;

	private bool isEnd;

	private float checkTimer;

	public void Init(GUIRewardScratchOffInfo basescene)
	{
		this.mBaseScene = basescene;
		GameObject gameObject = GameUITools.FindGameObject("Cards", base.gameObject);
		Texture original = null;
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			this.cardArr[i] = gameObject.transform.GetChild(i).gameObject.AddComponent<GUIRewardCard>();
			if (i == 0)
			{
				original = this.cardArr[i].GetComponent<UITexture>().mainTexture;
			}
			else
			{
				this.cardArr[i].GetComponent<UITexture>().mainTexture = (UnityEngine.Object.Instantiate(original) as Texture);
			}
			this.cardArr[i].Init(this, this.mBaseScene.mBaseScene.ScratchOffBrushSize, this.mBaseScene.mBaseScene.ScratchOffPercent);
		}
	}

	public void Refresh(int result)
	{
		this.isEnd = false;
		this.checkTimer = Time.time;
		this.result = result;
		if (result == 0)
		{
			this.mCurNums[0] = this.nums[UnityEngine.Random.Range(0, this.nums.Length)];
			this.mCurNums[1] = this.mCurNums[0];
			this.mCurNums[2] = this.nums[UnityEngine.Random.Range(0, this.nums.Length)];
			while (this.mCurNums[2] == this.mCurNums[0])
			{
				this.mCurNums[2] = this.nums[UnityEngine.Random.Range(0, this.nums.Length)];
			}
		}
		else
		{
			for (int i = 0; i < 3; i++)
			{
				this.mCurNums[i] = string.Format("{0:D3}", result);
			}
		}
		this.hasScratchNum = 0;
		GUIRewardCard[] array = this.cardArr;
		for (int j = 0; j < array.Length; j++)
		{
			GUIRewardCard gUIRewardCard = array[j];
			gUIRewardCard.ReInit();
		}
	}

	public void CheckIsAllVisible()
	{
		if (this.isEnd)
		{
			return;
		}
		GUIRewardCard[] array = this.cardArr;
		for (int i = 0; i < array.Length; i++)
		{
			GUIRewardCard gUIRewardCard = array[i];
			if (!gUIRewardCard.IsVisible)
			{
				return;
			}
		}
		this.isEnd = true;
		base.Invoke("PopUpMessageBox", this.mBaseScene.mBaseScene.ScratchOffPopTime);
	}

	private void PopUpMessageBox()
	{
		GUIRewardCard[] array = this.cardArr;
		for (int i = 0; i < array.Length; i++)
		{
			GUIRewardCard gUIRewardCard = array[i];
			gUIRewardCard.Reset();
		}
		if (this.result == 0)
		{
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("activityScratchOffSorry"), MessageBox.Type.OK, null);
			gameMessageBox.CanCloseByFadeBGClicked = false;
			GameMessageBox expr_4F = gameMessageBox;
			expr_4F.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_4F.OkClick, new MessageBox.MessageDelegate(this.ClosePop));
		}
		else
		{
			GUIRewardPanel.Show(new RewardData
			{
				RewardType = 2,
				RewardValue1 = this.result
			}, null, false, true, null, false);
			this.ClosePop(null);
		}
		this.mBaseScene.ScratchOffEnd();
	}

	private void ClosePop(object obj)
	{
		this.mBaseScene.ClosePop();
	}

	private void OnFadeBGClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		if (this.hasScratchNum == 3)
		{
			this.mBaseScene.ClosePop();
		}
	}

	private void Update()
	{
		if (base.gameObject.activeInHierarchy && !this.isEnd && Time.time - this.checkTimer > 3f)
		{
			this.checkTimer = Time.time;
			if (this.CheckIfAllVisible())
			{
				this.isEnd = true;
				base.Invoke("PopUpMessageBox", this.mBaseScene.mBaseScene.ScratchOffPopTime);
			}
		}
	}

	private bool CheckIfAllVisible()
	{
		GUIRewardCard[] array = this.cardArr;
		for (int i = 0; i < array.Length; i++)
		{
			GUIRewardCard gUIRewardCard = array[i];
			if (!gUIRewardCard.CheckIfVisible())
			{
				return false;
			}
		}
		return true;
	}
}
