using Proto;
using System;
using UnityEngine;

public class MailContentsLayer : MonoBehaviour
{
	private GUIMailScene mBaseScene;

	private MailContentsUITable mMailContentsTable;

	public void InitWithBaseScene(GUIMailScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
		this.InitMailItems();
	}

	private void CreateObjects()
	{
		this.mMailContentsTable = base.transform.FindChild("contentsPanel/mailContents").gameObject.AddComponent<MailContentsUITable>();
		this.mMailContentsTable.maxPerLine = 1;
		this.mMailContentsTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mMailContentsTable.cellWidth = 594f;
		this.mMailContentsTable.cellHeight = 112f;
		this.mMailContentsTable.scrollBar = base.transform.Find("contentsScrollBar").GetComponent<UIScrollBar>();
		this.mMailContentsTable.InitWithBaseScene(this.mBaseScene);
	}

	public void InitMailItems()
	{
		this.mMailContentsTable.ClearData();
		for (int i = 0; i < Globals.Instance.Player.Mails.Count; i++)
		{
			MailData mailData = Globals.Instance.Player.Mails[i];
			if (mailData.AffixType.Count != 0 || Globals.Instance.Player.GetTimeStamp() <= mailData.TimeStamp + 259200)
			{
				this.mMailContentsTable.AddData(new MailItemData(mailData));
			}
		}
		this.Refresh();
	}

	public void ReInitMailItems()
	{
		this.InitMailItems();
	}

	public void OnTakeMailAffixEvent(uint mailID)
	{
		this.ReInitMailItems();
		GUIMainMenuScene session = GameUIManager.mInstance.GetSession<GUIMainMenuScene>();
		if (session != null)
		{
			session.UpdateUnreadMailFlag();
		}
	}

	public void Refresh()
	{
		this.mBaseScene.mNoMailGo.SetActive(Globals.Instance.Player.Mails.Count == 0);
	}
}
