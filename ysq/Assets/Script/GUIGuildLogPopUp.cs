using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIGuildLogPopUp : GameUIBasePopup
{
	private GuildLogTable mGuildLogTable;

	private UIScrollBar mUIScrollBar;

	private UnityEngine.Object mChunkItemOriginal;

	private void Awake()
	{
		this.CreateObjects();
		base.StartCoroutine(this.Refresh());
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBg");
		GameObject gameObject = transform.Find("closeBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mGuildLogTable = transform.Find("contentsBg/contentsPanel/contents").gameObject.AddComponent<GuildLogTable>();
		this.mGuildLogTable.columns = 1;
		this.mGuildLogTable.direction = UITable.Direction.Down;
		this.mGuildLogTable.sorting = UITable.Sorting.Custom;
		this.mGuildLogTable.hideInactive = true;
		this.mGuildLogTable.keepWithinPanel = true;
		this.mGuildLogTable.padding = new Vector2(0f, 2f);
		this.mUIScrollBar = transform.transform.Find("contentsBg/contentsScrollBar").GetComponent<UIScrollBar>();
	}

	[DebuggerHidden]
	private IEnumerator Refresh()
	{
        return null;
        //GUIGuildLogPopUp.<Refresh>c__Iterator5D <Refresh>c__Iterator5D = new GUIGuildLogPopUp.<Refresh>c__Iterator5D();
        //<Refresh>c__Iterator5D.<>f__this = this;
        //return <Refresh>c__Iterator5D;
	}

	private void AddLogChunk(int startIndex, int endIndex)
	{
		if (this.mChunkItemOriginal == null)
		{
			this.mChunkItemOriginal = Res.LoadGUI("GUI/guildLogContent");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.mChunkItemOriginal);
		gameObject.name = this.mChunkItemOriginal.name;
		gameObject.transform.parent = this.mGuildLogTable.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		GuildLogChunk guildLogChunk = gameObject.AddComponent<GuildLogChunk>();
		guildLogChunk.InitWithBaseScene(startIndex, endIndex);
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
