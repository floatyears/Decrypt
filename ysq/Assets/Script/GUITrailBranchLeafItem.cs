using Holoville.HOTween;
using System;
using UnityEngine;

public class GUITrailBranchLeafItem : MonoBehaviour
{
	private const int mStateNum = 2;

	private GUITrailTowerSceneV2 mBaseScene;

	private UISprite[] mStates = new UISprite[2];

	private UILabel mState0Num;

	private UILabel mState1Num;

	private int mIndex;

	private int mCurIndex;

	public void InitWithBaseScene(GUITrailTowerSceneV2 baseScene, int index)
	{
		this.mBaseScene = baseScene;
		this.mIndex = index;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mStates[0] = base.transform.Find("s0").GetComponent<UISprite>();
		UIEventListener expr_2F = UIEventListener.Get(this.mStates[0].gameObject);
		expr_2F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2F.onClick, new UIEventListener.VoidDelegate(this.OnBoxLockClicked));
		this.mStates[1] = base.transform.Find("s1").GetComponent<UISprite>();
		this.mState0Num = this.mStates[0].transform.Find("num").GetComponent<UILabel>();
		this.mState1Num = this.mStates[1].transform.Find("num").GetComponent<UILabel>();
		GUITrailTowerSceneV2 expr_B7 = this.mBaseScene;
		expr_B7.OnSaoDangDoneEvent = (GUITrailTowerSceneV2.SaoDangDoneCallback)Delegate.Combine(expr_B7.OnSaoDangDoneEvent, new GUITrailTowerSceneV2.SaoDangDoneCallback(this.OnSaoDangEvent));
	}

	private void OnSaoDangEvent(int lvl)
	{
		if (this.mCurIndex == lvl)
		{
			HOTween.Shake(this.mStates[0].transform, 2f, "localRotation", Quaternion.Euler(0f, 0f, 30f), 0.1f, 0.12f);
		}
	}

	public void Refresh(int startIndex)
	{
		int num = (Globals.Instance.Player.Data.TrialFarmTimeStamp != 0) ? this.mBaseScene.GetCurSaoDangLvl() : Globals.Instance.Player.Data.TrialWave;
		if (Globals.Instance.Player.Data.TrialFarmTimeStamp == 0)
		{
			num = Mathf.Min(GameConst.GetInt32(187), Mathf.Max(1, num + 1));
		}
		else
		{
			num = Mathf.Max(Mathf.Min(Globals.Instance.Player.Data.TrialMaxWave, num + 1), 1);
		}
		this.mCurIndex = startIndex + this.mIndex;
		if (this.mCurIndex == num)
		{
			this.mStates[0].gameObject.SetActive(false);
			this.mStates[1].gameObject.SetActive(true);
			this.mStates[1].spriteName = "boxBattling";
			this.mState1Num.gameObject.SetActive(true);
			this.mState1Num.text = this.mCurIndex.ToString();
		}
		else if (this.mCurIndex > Globals.Instance.Player.Data.TrialMaxWave)
		{
			this.mStates[0].gameObject.SetActive(true);
			this.mStates[1].gameObject.SetActive(false);
			if (this.mCurIndex == 1)
			{
				this.mStates[0].spriteName = "notDone";
				this.mState0Num.gameObject.SetActive(true);
				this.mState0Num.text = this.mCurIndex.ToString();
			}
			else if (this.mCurIndex % 5 == 0)
			{
				this.mStates[0].spriteName = "boxLock";
				this.mState0Num.gameObject.SetActive(false);
			}
			else
			{
				this.mStates[0].spriteName = "gateLock";
				this.mState0Num.gameObject.SetActive(false);
			}
		}
		else if (this.mCurIndex < num)
		{
			this.mStates[0].gameObject.SetActive(true);
			this.mStates[1].gameObject.SetActive(false);
			this.mStates[0].spriteName = "gateDone";
			this.mState0Num.gameObject.SetActive(true);
			this.mState0Num.text = this.mCurIndex.ToString();
		}
		else if (this.mCurIndex > num)
		{
			this.mStates[0].gameObject.SetActive(true);
			this.mStates[1].gameObject.SetActive(false);
			this.mStates[0].spriteName = "notDone";
			this.mState0Num.gameObject.SetActive(true);
			this.mState0Num.text = this.mCurIndex.ToString();
		}
	}

	private void OnBoxLockClicked(GameObject go)
	{
		int num = (Globals.Instance.Player.Data.TrialFarmTimeStamp != 0) ? this.mBaseScene.GetCurSaoDangLvl() : Globals.Instance.Player.Data.TrialWave;
		if (Globals.Instance.Player.Data.TrialFarmTimeStamp == 0)
		{
			num = Mathf.Min(GameConst.GetInt32(187), Mathf.Max(1, num + 1));
		}
		else
		{
			num = Mathf.Max(Mathf.Min(Globals.Instance.Player.Data.TrialMaxWave, num + 1), 1);
		}
		if (this.mCurIndex > 0 && this.mCurIndex > Globals.Instance.Player.Data.TrialMaxWave && this.mCurIndex % 5 == 0)
		{
			GUITrialRewardPopUp.ShowThis(this.mCurIndex, this.mCurIndex, true);
		}
	}
}
