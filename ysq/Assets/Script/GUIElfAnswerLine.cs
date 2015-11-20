using System;
using UnityEngine;

public class GUIElfAnswerLine : GUIElfLineBase
{
	private UILabel answerLine;

	private UIWidget widget;

	private BoxCollider BoxCollider;

	private UISprite answerBg;

	private ElfAnswerItem answerData;

	private GameObject line;

	private UILabel evaLabel;

	private GameObject resolveBtn;

	private GameObject unresolveBtn;

	public void InitWithBaseScene(GameUIFairyTalePopUp baseScene, ElfAnswerItem data)
	{
		this.mBaseScene = baseScene;
		this.data = data;
		this.answerData = data;
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		this.answerLine = base.transform.GetComponent<UILabel>();
		this.answerLine.spaceIsNewLine = false;
		this.answerBg = this.answerLine.transform.Find("bg").GetComponent<UISprite>();
		this.widget = this.answerBg.transform.Find("Empty").GetComponent<UIWidget>();
		this.BoxCollider = this.widget.transform.GetComponent<BoxCollider>();
		UIEventListener expr_83 = UIEventListener.Get(this.widget.gameObject);
		expr_83.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_83.onClick, new UIEventListener.VoidDelegate(this.OnAnswerLineClick));
		this.line = this.answerBg.transform.Find("evaPanel").gameObject;
		this.evaLabel = this.line.transform.Find("Label").GetComponent<UILabel>();
		this.unresolveBtn = GameUITools.RegisterClickEvent("unresolvedBtn", new UIEventListener.VoidDelegate(this.OnUnresolveBtnBtnClick), this.line);
		this.resolveBtn = GameUITools.RegisterClickEvent("resolvedBtn", new UIEventListener.VoidDelegate(this.OnResolveBtnBtnClick), this.line);
	}

	private void OnUnresolveBtnBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.answerData.evalResolve = 1;
		this.RefreshEvaluateBtn();
		GameUIFairyTalePopUp.HttpGetElfCommentUrl(this.answerData.strQuest, this.answerData.strOriginal, false);
	}

	private void OnResolveBtnBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.answerData.evalResolve = 2;
		this.RefreshEvaluateBtn();
		GameUIFairyTalePopUp.HttpGetElfCommentUrl(this.answerData.strQuest, this.answerData.strOriginal, true);
	}

	private void Refresh()
	{
		string strAnswer = this.answerData.strAnswer;
		this.answerLine.text = strAnswer;
		this.answerLine.ProcessText();
		if (this.answerData.showEvaluate)
		{
			this.line.SetActive(true);
			this.answerBg.height = this.answerLine.height + 66;
			this.RefreshEvaluateBtn();
		}
		else
		{
			this.line.SetActive(false);
			this.answerBg.height = Mathf.Max(88, this.answerLine.height + 16);
		}
		this.widget.height = this.answerBg.height;
		this.BoxCollider.center = new Vector3(445f, (float)(-(float)this.widget.height / 2), 0f);
		this.BoxCollider.size = new Vector3(890f, (float)(this.widget.height + 20), 0f);
	}

	private void RefreshEvaluateBtn()
	{
		if (this.answerData.evalResolve == 0)
		{
			this.evaLabel.text = Singleton<StringManager>.Instance.GetString("FairyTxt_3");
		}
		else
		{
			this.evaLabel.text = ((this.answerData.evalResolve != 1) ? Singleton<StringManager>.Instance.GetString("FairyTxt_5") : Singleton<StringManager>.Instance.GetString("FairyTxt_4"));
			this.evaLabel.transform.localPosition = new Vector3(0f, -30f, 0f);
			this.resolveBtn.SetActive(false);
			this.unresolveBtn.SetActive(false);
		}
	}

	private void OnAnswerLineClick(GameObject go)
	{
		this.mBaseScene.ProcessUrlClick(this.answerLine);
	}
}
