using Holoville.HOTween;
using System;
using UnityEngine;

public class GUIEmotionItem : MonoBehaviour
{
	private UISprite mEmotionSprite;

	private GUIChatWindowV2 mBaseLayer;

	public string EmotionTag
	{
		get;
		private set;
	}

	public void InitWithBaseLayer(GUIChatWindowV2 baseLayer, string tag)
	{
		this.mBaseLayer = baseLayer;
		this.EmotionTag = tag;
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		this.mEmotionSprite = base.transform.GetComponent<UISprite>();
		UIEventListener expr_1C = UIEventListener.Get(base.gameObject);
		expr_1C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C.onClick, new UIEventListener.VoidDelegate(this.OnEmotionClick));
	}

	private void Refresh()
	{
		char[] trimChars = new char[]
		{
			'<',
			'>'
		};
		this.mEmotionSprite.spriteName = this.EmotionTag.Trim(trimChars);
	}

	private void OnEmotionClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		Sequence sequence = new Sequence();
		sequence.Append(HOTween.To(base.transform, 0.1f, new TweenParms().Prop("localScale", new Vector3(0.9f, 0.9f, 0.9f))));
		sequence.Append(HOTween.To(base.transform, 0.1f, new TweenParms().Prop("localScale", Vector3.one)));
		sequence.Play();
		this.mBaseLayer.AppendChatMsg(this.EmotionTag);
	}
}
