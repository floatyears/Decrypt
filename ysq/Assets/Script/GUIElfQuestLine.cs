using System;
using UnityEngine;

public class GUIElfQuestLine : GUIElfLineBase
{
	private UILabel questLine;

	private BoxCollider BoxCollider;

	private ElfQuestItem questData;

	public void InitWithBaseScene(GameUIFairyTalePopUp baseScene, ElfQuestItem data)
	{
		this.mBaseScene = baseScene;
		this.data = data;
		this.questData = data;
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		this.questLine = base.transform.GetComponent<UILabel>();
		this.questLine.spaceIsNewLine = false;
		Transform transform = base.transform.Find("Empty");
		this.BoxCollider = transform.GetComponent<BoxCollider>();
		UISprite uISprite = GameUITools.FindUISprite("CharIcon", base.gameObject);
		uISprite.spriteName = Globals.Instance.Player.GetPlayerIcon();
		UISprite uISprite2 = GameUITools.FindUISprite("CharIcon/Frame", base.gameObject);
		uISprite2.spriteName = Tools.GetItemQualityIcon(Globals.Instance.Player.GetQuality());
	}

	private void Refresh()
	{
		this.questLine.overflowMethod = UILabel.Overflow.ResizeFreely;
		this.questLine.text = this.questData.strShow;
		int num = Mathf.Clamp(Mathf.RoundToInt(this.questLine.printedSize.x), 44, 745);
		if ((num & 1) == 1)
		{
			num++;
		}
		this.questLine.width = num;
		this.questLine.overflowMethod = UILabel.Overflow.ResizeHeight;
		this.BoxCollider.size = new Vector3(890f, (float)(this.questLine.height + 85), 0f);
	}
}
