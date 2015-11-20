using Att;
using Holoville.HOTween.Core;
using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GameUIQuestInformation : MonoBehaviour
{
	private static GameUIQuestInformation mInstance;

	private Transform mQuestInfoWin;

	private UILabel title;

	private UILabel desc;

	private UILabel target;

	private GameObject btnGo;

	private GameObject btnReceive;

	private QuestInfo curQuestInfo;

	private GameObject[] RewardItem = new GameObject[4];

	private Transform mParent;

	public void Init(Transform parent, QuestInfo questInfo)
	{
		this.CreateObjects();
		Transform transform = base.transform;
		transform.parent = parent;
		transform.localPosition = new Vector3(0f, 0f, -1000f);
		transform.localScale = Vector3.one;
		this.curQuestInfo = questInfo;
		this.Refresh();
	}

	private void CreateObjects()
	{
		this.mQuestInfoWin = base.transform.Find("winBG");
		UIEventListener expr_30 = UIEventListener.Get(base.transform.Find("BG").gameObject);
		expr_30.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_30.onClick, new UIEventListener.VoidDelegate(this.OnCloseQuestInfoClicked));
		UIEventListener expr_6B = UIEventListener.Get(this.mQuestInfoWin.Find("closeBtn").gameObject);
		expr_6B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6B.onClick, new UIEventListener.VoidDelegate(this.OnCloseQuestInfoClicked));
		UIEventListener expr_A6 = UIEventListener.Get(this.mQuestInfoWin.Find("ReceiveBtn").gameObject);
		expr_A6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A6.onClick, new UIEventListener.VoidDelegate(this.OnQuestInfoReceiveBtnClicked));
		UIEventListener expr_E1 = UIEventListener.Get(this.mQuestInfoWin.Find("GoBtn").gameObject);
		expr_E1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_E1.onClick, new UIEventListener.VoidDelegate(this.OnQuestInfoReceiveBtnClicked));
		Tools.SetParticleRQWithUIScale(this.mQuestInfoWin.transform.FindChild("ReceiveBtn/Sprite/ui67").gameObject, 4400);
		this.title = this.mQuestInfoWin.FindChild("Title").GetComponent<UILabel>();
		this.desc = this.mQuestInfoWin.FindChild("Desc").GetComponent<UILabel>();
		this.target = this.mQuestInfoWin.FindChild("Target").GetComponent<UILabel>();
		this.btnGo = this.mQuestInfoWin.FindChild("GoBtn").gameObject;
		this.btnReceive = this.mQuestInfoWin.FindChild("ReceiveBtn").gameObject;
	}

	public static GameUIQuestInformation GetInstance()
	{
		if (GameUIQuestInformation.mInstance == null)
		{
			GameObject original = Res.LoadGUI("GUI/QuestInformation");
			GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
			GameUIQuestInformation.mInstance = gameObject.AddComponent<GameUIQuestInformation>();
		}
		return GameUIQuestInformation.mInstance;
	}

	public static void SaveOldData()
	{
		ObscuredStats data = Globals.Instance.Player.Data;
		GameUIState uiState = GameUIManager.mInstance.uiState;
		uiState.PlayerLevel = data.Level;
		uiState.PlayerExp = data.Exp;
		uiState.PlayerEnergy = data.Energy;
	}

	public void OnCloseQuestInfoClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.CloseQuestInfo(null);
	}

	private void CloseQuestInfo(Transform parent = null)
	{
		if (parent == null)
		{
			GameUITools.PlayCloseWindowAnim(this.mQuestInfoWin, new TweenDelegate.TweenCallback(this.OnCloseQuestInfoAnimEnd), true);
		}
		else if (this.mQuestInfoWin != null)
		{
			this.mParent = parent;
			GameUITools.PlayCloseWindowAnim(this.mQuestInfoWin, new TweenDelegate.TweenCallback(this.OnCloseQuestInfoAnimEndShowNext), true);
		}
	}

	public void OnCloseQuestInfoAnimEnd()
	{
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}

	public void OnCloseQuestInfoAnimEndShowNext()
	{
		UnityEngine.Object.DestroyImmediate(base.gameObject);
		QuestInfo mainQuest = Globals.Instance.Player.MainQuest;
		if (mainQuest != null && Globals.Instance.Player.GetQuestState(mainQuest.ID) == 1)
		{
			GameUIQuestInformation instance = GameUIQuestInformation.GetInstance();
			instance.Init(this.mParent, mainQuest);
		}
	}

	public void OnQuestInfoReceiveBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIQuestInformation.QuestBtnClicked(this.curQuestInfo);
	}

	private static void SendTakeQuestReward(int questID)
	{
		GameUIQuestInformation.SaveOldData();
		MC2S_TakeQuestReward mC2S_TakeQuestReward = new MC2S_TakeQuestReward();
		mC2S_TakeQuestReward.QuestID = questID;
		Globals.Instance.CliSession.Send(209, mC2S_TakeQuestReward);
	}

	[DebuggerHidden]
	public static IEnumerator PlayCardAnim(int questID, Transform parent)
	{
        return null;
        //GameUIQuestInformation.<PlayCardAnim>c__Iterator94 <PlayCardAnim>c__Iterator = new GameUIQuestInformation.<PlayCardAnim>c__Iterator94();
        //<PlayCardAnim>c__Iterator.parent = parent;
        //<PlayCardAnim>c__Iterator.questID = questID;
        //<PlayCardAnim>c__Iterator.<$>parent = parent;
        //<PlayCardAnim>c__Iterator.<$>questID = questID;
        //return <PlayCardAnim>c__Iterator;
	}

	private void Refresh()
	{
		if (GUIQuestScene.IsTrunk(this.curQuestInfo))
		{
			this.title.text = string.Format("[{0}]{1}", Singleton<StringManager>.Instance.GetString("QuestTrunk"), this.curQuestInfo.Name);
		}
		else
		{
			this.title.text = this.curQuestInfo.Name;
		}
		if (Globals.Instance.Player.GetQuestState(this.curQuestInfo.ID) == 1)
		{
			this.btnGo.SetActive(false);
			this.btnReceive.SetActive(true);
			this.desc.text = this.curQuestInfo.Desc2;
			this.target.text = string.Format("[66ff00]{0}", (this.curQuestInfo.Target3.Length == 0) ? this.curQuestInfo.Target : this.curQuestInfo.Target3);
		}
		else
		{
			this.btnGo.SetActive(true);
			this.btnReceive.SetActive(false);
			this.desc.text = this.curQuestInfo.Desc;
			this.target.text = string.Format("[ffe400]{0}", this.curQuestInfo.Target);
		}
		for (int i = 0; i < this.RewardItem.Length; i++)
		{
			if (this.RewardItem[i] != null)
			{
				UnityEngine.Object.Destroy(this.RewardItem[i]);
				this.RewardItem[i] = null;
			}
		}
		float num = -40f;
		int num2 = 0;
		int num3 = 0;
		while (num2 < this.RewardItem.Length && num3 < this.curQuestInfo.RewardType.Count)
		{
			if (this.curQuestInfo.RewardType[num3] != 0 && this.curQuestInfo.RewardType[num3] != 20)
			{
				if (this.curQuestInfo.RewardType[num3] != 3 && this.curQuestInfo.RewardType[num3] != 4 && this.curQuestInfo.RewardType[num3] != 12)
				{
					this.RewardItem[num2] = GameUITools.CreateMinReward(this.curQuestInfo.RewardType[num3], this.curQuestInfo.RewardValue1[num3], this.curQuestInfo.RewardValue2[num3], this.mQuestInfoWin);
					if (this.RewardItem[num2] != null)
					{
						this.RewardItem[num2].transform.localPosition = new Vector3(-168f, num, 0f);
						num2++;
						num -= 46f;
					}
				}
			}
			num3++;
		}
		float num4 = -130f;
		int num5 = 0;
		while (num2 < this.RewardItem.Length && num5 < this.curQuestInfo.RewardType.Count)
		{
			if (this.curQuestInfo.RewardType[num5] != 0 && this.curQuestInfo.RewardType[num5] != 20)
			{
				if (this.curQuestInfo.RewardType[num5] == 3 || this.curQuestInfo.RewardType[num5] == 4 || this.curQuestInfo.RewardType[num5] == 12)
				{
					this.RewardItem[num2] = GameUITools.CreateReward(this.curQuestInfo.RewardType[num5], this.curQuestInfo.RewardValue1[num5], this.curQuestInfo.RewardValue2[num5], this.mQuestInfoWin, true, true, 0f, 20f, -2000f, 20f, 13f, 7f, 0);
					if (this.RewardItem[num2] != null)
					{
						this.RewardItem[num2].transform.localPosition = new Vector3(num4, num - 42f, 0f);
						num4 += 109f;
						num2++;
					}
				}
			}
			num5++;
		}
		base.gameObject.SetActive(true);
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	public static void QuestBtnClicked(QuestInfo questInfo)
	{
		int questState = Globals.Instance.Player.GetQuestState(questInfo.ID);
		if (questState == 0)
		{
			GameUIQuestInformation.QuestGo(questInfo);
		}
		else if (questState == 1)
		{
			GameUIQuestInformation.SendTakeQuestReward(questInfo.ID);
		}
	}

	private static void QuestGo(QuestInfo questInfo)
	{
		SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(questInfo.ID);
		if (info == null)
		{
			return;
		}
		if (info.Difficulty == 1 && Globals.Instance.Player.GetSceneScore(GameConst.GetInt32(109)) <= 0)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_3"), 0f, 0f);
			return;
		}
		if (info.Difficulty == 9 && Globals.Instance.Player.GetSceneScore(GameConst.GetInt32(61)) <= 0)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_6"), 0f, 0f);
			return;
		}
		GameUIManager.mInstance.uiState.PetSceneInfo = info;
		GameUIManager.mInstance.uiState.QuestSceneInfo = GameUIManager.mInstance.uiState.PetSceneInfo;
		GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, false, false);
	}
}
