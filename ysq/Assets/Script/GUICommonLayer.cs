using System;
using UnityEngine;

public class GUICommonLayer : MonoBehaviour
{
	private const int ITEM_MAX_COUNT = 5;

	private UITable mCommonTable;

	private GUIChatWindowV2 mBaseLayer;

	public GUICommonItem mGUICommonItem;

	public void InitWithBaseScene(GUIChatWindowV2 baseLayer)
	{
		this.mBaseLayer = baseLayer;
		this.CreateObjects();
		this.InitCommon();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("commonArea");
		this.mCommonTable = transform.Find("commonLayer/commonContents").GetComponent<UITable>();
		this.mGUICommonItem = this.mCommonTable.transform.Find("commonLabelContents").gameObject.AddComponent<GUICommonItem>();
		this.mGUICommonItem.gameObject.SetActive(false);
		UIEventListener expr_68 = UIEventListener.Get(transform.gameObject);
		expr_68.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_68.onClick, new UIEventListener.VoidDelegate(this.OnBgClick));
	}

	private void InitCommon()
	{
		for (int i = 1; i <= 5; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.mGUICommonItem.gameObject) as GameObject;
			if (gameObject != null)
			{
				Transform transform = gameObject.transform;
				transform.parent = this.mCommonTable.transform;
				transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
				gameObject.layer = this.mCommonTable.gameObject.layer;
				GUICommonItem gUICommonItem = gameObject.GetComponent<GUICommonItem>();
				if (gUICommonItem == null)
				{
					gUICommonItem = gameObject.gameObject.AddComponent<GUICommonItem>();
				}
				gUICommonItem.InitWithBaseLayer(this.mBaseLayer, Singleton<StringManager>.Instance.GetString(string.Format("worship_{0}", i - 1)));
				gUICommonItem.gameObject.SetActive(true);
			}
		}
		this.mCommonTable.repositionNow = true;
	}

	private void OnBgClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		base.gameObject.SetActive(false);
	}

	public void SwitchCommonLayer()
	{
		base.gameObject.SetActive(!base.gameObject.activeInHierarchy);
	}
}
