using System;
using UnityEngine;

public class GUIEmotionLayer : MonoBehaviour
{
	private UITable mEmotionTable;

	private GUIChatWindowV2 mBaseLayer;

	private GUIEmotionItem mOrignalEmotion;

	private int mEmotionTagNums = 48;

	public void InitWithBaseScene(GUIChatWindowV2 baseLayer)
	{
		this.mBaseLayer = baseLayer;
		this.CreateObjects();
		this.InitEmotions();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("emotionArea");
		this.mEmotionTable = transform.Find("emotionLayer/emotionContents").GetComponent<UITable>();
		this.mOrignalEmotion = this.mEmotionTable.transform.Find("emotion").gameObject.AddComponent<GUIEmotionItem>();
		this.mOrignalEmotion.gameObject.SetActive(false);
		UIEventListener expr_68 = UIEventListener.Get(transform.gameObject);
		expr_68.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_68.onClick, new UIEventListener.VoidDelegate(this.OnBgClick));
	}

	private void InitEmotions()
	{
		for (int i = 1; i <= this.mEmotionTagNums; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.mOrignalEmotion.gameObject) as GameObject;
			if (gameObject != null)
			{
				Transform transform = gameObject.transform;
				transform.parent = this.mEmotionTable.transform;
				transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
				gameObject.layer = this.mEmotionTable.gameObject.layer;
				GUIEmotionItem gUIEmotionItem = gameObject.GetComponent<GUIEmotionItem>();
				if (gUIEmotionItem == null)
				{
					gUIEmotionItem = gameObject.gameObject.AddComponent<GUIEmotionItem>();
				}
				gUIEmotionItem.InitWithBaseLayer(this.mBaseLayer, string.Format("<{0:D2}>", i));
				gUIEmotionItem.gameObject.SetActive(true);
			}
		}
		this.mEmotionTable.repositionNow = true;
	}

	private void OnBgClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		base.gameObject.SetActive(false);
	}

	public void SwitchEmotionLayer()
	{
		base.gameObject.SetActive(!base.gameObject.activeInHierarchy);
	}
}
