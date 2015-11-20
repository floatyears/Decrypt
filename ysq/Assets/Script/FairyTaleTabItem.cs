using Holoville.HOTween;
using System;
using UnityEngine;

public class FairyTaleTabItem : MonoBehaviour
{
	private const int TAB_ITEM_LIMIT_NUM = 6;

	private GameUIFairyTalePopUp mBaseScene;

	private ElfBtnItem initData;

	private UILabel txtTab;

	private UILabel txtTab2;

	private UITable btnsTable;

	private UIScrollBar scrollBar;

	private GameObject topBtn;

	private GameObject bottomBtn;

	private UITable infoTable;

	private UILabel infoTitle;

	private UILabel infoContent;

	private GameObject mElfCheckBtnPrefab;

	public UIToggle btnTab
	{
		get;
		private set;
	}

	public void InitWithBaseScene(GameUIFairyTalePopUp baseScene, ElfBtnItem data)
	{
		this.mBaseScene = baseScene;
		this.initData = data;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.btnTab = base.transform.Find("tab").GetComponent<UIToggle>();
		EventDelegate.Add(this.btnTab.onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
		this.txtTab = this.btnTab.transform.Find("tabTxt").GetComponent<UILabel>();
		this.txtTab2 = this.btnTab.transform.Find("tabCheck/tabTxt").GetComponent<UILabel>();
		Transform transform = base.transform.Find("tabPanel");
		this.topBtn = GameUITools.RegisterClickEvent("TopBtn", new UIEventListener.VoidDelegate(this.OnTopBtnClick), transform.gameObject);
		this.topBtn.SetActive(false);
		this.bottomBtn = GameUITools.RegisterClickEvent("BottomBtn", new UIEventListener.VoidDelegate(this.OnBottomBtnClick), transform.gameObject);
		this.bottomBtn.SetActive(false);
		this.btnsTable = transform.Find("tabBtnsPanel/tabBtns").GetComponent<UITable>();
		this.scrollBar = transform.Find("scrollBar").GetComponent<UIScrollBar>();
		EventDelegate.Add(this.scrollBar.onChange, new EventDelegate.Callback(this.OnScrollBarValueChange));
		this.txtTab.text = this.initData.strName;
		this.txtTab2.text = this.initData.strName;
		if (this.initData.strName.Length > 2)
		{
			this.txtTab.spacingX = 5;
			this.txtTab2.spacingX = 5;
		}
		this.infoTable = transform.Find("bg/infoPanel/infoContents").GetComponent<UITable>();
		this.infoTitle = this.infoTable.transform.Find("Label0").GetComponent<UILabel>();
		this.infoTitle.spaceIsNewLine = false;
		this.infoContent = this.infoTable.transform.Find("Label1").GetComponent<UILabel>();
		this.infoContent.spaceIsNewLine = false;
		UIEventListener expr_206 = UIEventListener.Get(this.infoContent.gameObject);
		expr_206.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_206.onClick, new UIEventListener.VoidDelegate(this.OnInfoContentClick));
	}

	private void OnTabCheckChanged()
	{
		if (!UIToggle.current.value || UIToggle.current != this.btnTab)
		{
			return;
		}
		this.mBaseScene.current = this;
		if (this.btnsTable.GetChildList().Count <= 0)
		{
			GameUIFairyTalePopUp.HttpGetElfQueryUrl(1509, this.initData.strQuest);
		}
	}

	public void RefreshBtnTab(ElfBtnItem[] btnData)
	{
		for (int i = 0; i < btnData.Length; i++)
		{
			this.InitElfCheckBtn(btnData[i]);
		}
		this.btnsTable.repositionNow = true;
	}

	public void RefreshInfoPanel(ElfInfoItem infoData)
	{
		if (!string.IsNullOrEmpty(infoData.strTitle))
		{
			this.infoTitle.text = infoData.strTitle;
			this.infoTitle.gameObject.SetActive(true);
		}
		else
		{
			this.infoTitle.gameObject.SetActive(false);
		}
		this.infoContent.text = infoData.strContent;
		this.infoTable.repositionNow = true;
	}

	private GUIElfCheckBtn InitElfCheckBtn(ElfBtnItem data)
	{
		if (this.mElfCheckBtnPrefab == null)
		{
			this.mElfCheckBtnPrefab = Res.LoadGUI("GUI/GUIElfCheckBtn");
		}
		if (this.mElfCheckBtnPrefab == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIElfCheckBtn error"
			});
			return null;
		}
		GameObject gameObject = Tools.AddChild(this.btnsTable.gameObject, this.mElfCheckBtnPrefab);
		GUIElfCheckBtn gUIElfCheckBtn = gameObject.AddComponent<GUIElfCheckBtn>();
		gUIElfCheckBtn.InitWithBaseScene(false, data);
		gUIElfCheckBtn.name = string.Format("{0:D2}", this.btnsTable.transform.childCount);
		return gUIElfCheckBtn;
	}

	private void OnTopBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		HOTween.Complete(this.scrollBar);
		if (this.btnsTable.GetChildList().Count > 6)
		{
			HOTween.To(this.scrollBar, this.mBaseScene.DirectionBtnDuration, new TweenParms().Prop("value", this.scrollBar.value - 2f / (float)(this.btnsTable.GetChildList().Count - 6)).Ease(this.mBaseScene.DirectionBtnCurve));
		}
	}

	private void OnBottomBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		HOTween.Complete(this.scrollBar);
		if (this.btnsTable.GetChildList().Count > 6)
		{
			HOTween.To(this.scrollBar, this.mBaseScene.DirectionBtnDuration, new TweenParms().Prop("value", this.scrollBar.value + 2f / (float)(this.btnsTable.GetChildList().Count - 6)).Ease(this.mBaseScene.DirectionBtnCurve));
		}
	}

	private void OnScrollBarValueChange()
	{
		if (this.btnsTable.GetChildList().Count <= 6)
		{
			return;
		}
		if (this.topBtn.activeInHierarchy)
		{
			if ((double)this.scrollBar.value <= 0.01)
			{
				this.topBtn.SetActive(false);
			}
		}
		else if ((double)this.scrollBar.value > 0.01)
		{
			this.topBtn.SetActive(true);
		}
		if (this.bottomBtn.activeInHierarchy)
		{
			if ((double)this.scrollBar.value >= 0.99)
			{
				this.bottomBtn.SetActive(false);
			}
		}
		else if ((double)this.scrollBar.value < 0.99)
		{
			this.bottomBtn.SetActive(true);
		}
	}

	private void OnInfoContentClick(GameObject go)
	{
		this.mBaseScene.ProcessUrlClick(this.infoContent);
	}
}
