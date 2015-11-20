using Att;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("Game/UI Session/GUIQuestScene")]
public class GUIQuestScene : GameUISession
{
	private QuestSceneUITable mQuestTable;

	private QuestSceneItem trunkQuest;

	private QuestSceneItem branchQuest;

	protected override void OnPostLoadGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Show("questLb");
		Transform transform = base.transform.FindChild("WindowBg");
		this.mQuestTable = transform.FindChild("QuestPanel/QuestContents").gameObject.AddComponent<QuestSceneUITable>();
		this.mQuestTable.columns = 1;
		this.mQuestTable.direction = UITable.Direction.Down;
		this.mQuestTable.sorting = UITable.Sorting.Custom;
		this.mQuestTable.padding = new Vector2(0f, 2f);
		LocalPlayer player = Globals.Instance.Player;
		if (player.MainQuest != null && (ulong)player.Data.Level >= (ulong)((long)player.MainQuest.ShowLevel))
		{
			this.trunkQuest = this.AddOneItem(player.MainQuest);
		}
		if (player.BranchQuest != null && (ulong)player.Data.Level >= (ulong)((long)player.BranchQuest.ShowLevel))
		{
			this.branchQuest = this.AddOneItem(player.BranchQuest);
		}
		LocalPlayer expr_107 = Globals.Instance.Player;
		expr_107.QuestTakeRewardEvent = (LocalPlayer.TakeRewardCallback)Delegate.Combine(expr_107.QuestTakeRewardEvent, new LocalPlayer.TakeRewardCallback(this.OnTakeQuestRewardEvent));
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Hide();
		LocalPlayer expr_19 = Globals.Instance.Player;
		expr_19.QuestTakeRewardEvent = (LocalPlayer.TakeRewardCallback)Delegate.Remove(expr_19.QuestTakeRewardEvent, new LocalPlayer.TakeRewardCallback(this.OnTakeQuestRewardEvent));
	}

	private QuestSceneItem AddOneItem(QuestInfo qInfo)
	{
		GameObject original = Res.LoadGUI("GUI/QuestSceneItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(original);
		gameObject.transform.parent = this.mQuestTable.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		QuestSceneItem questSceneItem = gameObject.AddComponent<QuestSceneItem>();
		QuestSceneItem expr_5A = questSceneItem;
		expr_5A.QuestItemClicked = (QuestSceneItem.QuestItemEvent)Delegate.Combine(expr_5A.QuestItemClicked, new QuestSceneItem.QuestItemEvent(this.OnQuestItemClicked));
		questSceneItem.InitQuestItem(qInfo);
		questSceneItem.RefreshQuestItem();
		return questSceneItem;
	}

	private void OnQuestItemClicked(QuestInfo questInfo)
	{
		GameUIQuestInformation.GetInstance().Init(base.transform, questInfo);
	}

	public void OnTakeQuestRewardEvent(int questID)
	{
		base.StartCoroutine(this.RefreshQuestItems());
		base.StartCoroutine(GameUIQuestInformation.PlayCardAnim(questID, base.transform));
	}

	[DebuggerHidden]
	private IEnumerator RefreshQuestItems()
	{
        return null;
        //GUIQuestScene.<RefreshQuestItems>c__Iterator93 <RefreshQuestItems>c__Iterator = new GUIQuestScene.<RefreshQuestItems>c__Iterator93();
        //<RefreshQuestItems>c__Iterator.<>f__this = this;
        //return <RefreshQuestItems>c__Iterator;
	}

	public static bool IsTrunk(QuestInfo qInfo)
	{
		if (qInfo == null)
		{
			return false;
		}
		SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(qInfo.ID);
		return info != null && 0 == info.Difficulty;
	}
}
