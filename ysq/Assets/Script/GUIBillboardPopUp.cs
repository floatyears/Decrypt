using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIBillboardPopUp : GameUIBasePopup
{
	public class BillboardContent
	{
		public int ret
		{
			get;
			set;
		}

		public List<string> urgent
		{
			get;
			set;
		}

		public List<string> notice
		{
			get;
			set;
		}
	}

	public AnimationCurve ContentsCurve;

	[NonSerialized]
	public float ContentsDuration = 0.2f;

	[NonSerialized]
	public UITable mTable;

	private UISprite mWaiting;

	private GameObject mBar;

	private void Start()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mTable = GameUITools.FindGameObject("PanelBG/Panel/Contents", base.gameObject).GetComponent<UITable>();
		this.mWaiting = GameUITools.FindUISprite("Waiting", base.gameObject);
		this.mWaiting.enabled = true;
		this.mBar = GameUITools.FindGameObject("PanelBG/BgPanelScrollBar", base.gameObject);
		this.mBar.gameObject.SetActive(false);
		GameUITools.RegisterClickEvent("OK", new UIEventListener.VoidDelegate(this.OnOKClick), base.gameObject);
		Globals.Instance.CliSession.Register(1507, new ClientSession.MsgHandler(this.OnMsgBillboardList));
		this.RequestBillboardList();
	}

	private void OnDestroy()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		Globals.Instance.CliSession.Unregister(1507, new ClientSession.MsgHandler(this.OnMsgBillboardList));
	}

	private void RequestBillboardList()
	{
		Globals.Instance.CliSession.HttpGet(string.Format("{0}{1}", GameSetting.BillBoardURL, GameSetting.GameVersion), 1507, false, null);
	}

	public void OnMsgBillboardList(MemoryStream stream)
	{
		BinaryReader binaryReader = new BinaryReader(stream);
		int num = binaryReader.ReadInt32();
		if (num != 200)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("LoginR_Billboard", 0f, 0f);
			base.Invoke("RequestBillboardList", 3f);
			return;
		}
		try
		{
			GUIBillboardPopUp.BillboardContent billboardContent = JsonMapper.ToObject<GUIBillboardPopUp.BillboardContent>(binaryReader.ReadString());
			if (billboardContent.ret == 0)
			{
				this.InitPopUp(billboardContent);
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("LoginR_Billboard", 0f, 0f);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Parse BillboardList Json Error, {0}", ex.Message)
			});
			GameUIManager.mInstance.ShowMessageTipByKey("LoginR_Billboard", 0f, 0f);
			this.HideWaiting();
		}
	}

	public override void InitPopUp(GUIBillboardPopUp.BillboardContent contents)
	{
		this.mWaiting.enabled = false;
		for (int i = 0; i < contents.urgent.Count; i += 2)
		{
			if (i + 2 <= contents.urgent.Count)
			{
				GameUITools.AddChild(this.mTable.gameObject, this.InitContents(contents.urgent[i], contents.urgent[i + 1]).gameObject);
			}
		}
		for (int j = 0; j < contents.notice.Count; j += 2)
		{
			if (j + 2 <= contents.notice.Count)
			{
				GameUITools.AddChild(this.mTable.gameObject, this.InitContents(contents.notice[j], contents.notice[j + 1]).gameObject);
			}
		}
		this.mBar.gameObject.SetActive(true);
		this.mTable.repositionNow = true;
		base.Invoke("SetOnlyIfNeeded", 0.01f);
	}

	private void SetOnlyIfNeeded()
	{
		this.mTable.transform.parent.GetComponent<UIScrollView>().showScrollBars = UIScrollView.ShowCondition.OnlyIfNeeded;
	}

	private GUIBillboardItem InitContents(string name, string content)
	{
		GameObject original = Res.LoadGUI("GUI/BillboardItem");
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		GUIBillboardItem gUIBillboardItem = gameObject.AddComponent<GUIBillboardItem>();
		gUIBillboardItem.InitWithBaseScene(this);
		gUIBillboardItem.Refresh(name, content);
		return gUIBillboardItem;
	}

	private void OnOKClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUITools.CompleteAllHotween();
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	public void HideWaiting()
	{
		this.mWaiting.enabled = false;
	}

	public override void OnButtonBlockerClick()
	{
	}
}
