using System;
using UnityEngine;

public class GUIPeiYangNumPopUp : GameUIBasePopup
{
	private GUIPetTrainSceneV2 mBaseScene;

	private UISprite mBtn0Sp;

	private UISprite mBtn1Sp;

	private UISprite mBtn2Sp;

	private GameObject mBtn1Bg;

	private GameObject mBtn2Bg;

	private UILabel mBtn0Lb;

	private UILabel mBtn1Lb;

	private UILabel mBtn2Lb;

	public static void ShowMe()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIPeiYangNumPopUp, false, null, null);
	}

	private void Awake()
	{
		this.CreateObjects();
		this.mBaseScene = GameUIManager.mInstance.GetSession<GUIPetTrainSceneV2>();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBg");
		GameObject gameObject = transform.Find("closeBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnCloseClick));
		GameObject gameObject2 = transform.Find("btn0").gameObject;
		this.mBtn0Sp = gameObject2.GetComponent<UISprite>();
		this.mBtn0Sp.spriteName = "teamBagBg";
		UIEventListener expr_7C = UIEventListener.Get(gameObject2);
		expr_7C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_7C.onClick, new UIEventListener.VoidDelegate(this.OnBtn0Click));
		this.mBtn0Lb = gameObject2.transform.Find("txt1").GetComponent<UILabel>();
		this.mBtn0Lb.text = Singleton<StringManager>.Instance.GetString("petPeiYang5");
		GameObject gameObject3 = transform.Find("btn1").gameObject;
		this.mBtn1Sp = gameObject3.GetComponent<UISprite>();
		UIEventListener expr_F5 = UIEventListener.Get(gameObject3);
		expr_F5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_F5.onClick, new UIEventListener.VoidDelegate(this.OnBtn1Click));
		this.mBtn1Lb = gameObject3.transform.Find("txt1").GetComponent<UILabel>();
		this.mBtn1Bg = gameObject3.transform.Find("bg").gameObject;
		if (Globals.Instance.Player.Data.VipLevel < 4u)
		{
			this.mBtn1Sp.spriteName = "recharge_bg";
			this.mBtn1Lb.text = Singleton<StringManager>.Instance.GetString("petPeiYang3", new object[]
			{
				4
			});
			this.mBtn1Lb.color = new Color32(255, 174, 0, 255);
			this.mBtn1Lb.effectColor = new Color32(81, 0, 0, 255);
			this.mBtn1Bg.SetActive(false);
		}
		else
		{
			this.mBtn1Sp.spriteName = "teamBagBg";
			this.mBtn1Lb.text = Singleton<StringManager>.Instance.GetString("petPeiYang5");
			this.mBtn1Lb.color = this.mBtn0Lb.color;
			this.mBtn1Lb.effectColor = this.mBtn0Lb.effectColor;
			this.mBtn1Bg.SetActive(true);
		}
		GameObject gameObject4 = transform.Find("btn2").gameObject;
		this.mBtn2Sp = gameObject4.GetComponent<UISprite>();
		UIEventListener expr_27B = UIEventListener.Get(gameObject4);
		expr_27B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_27B.onClick, new UIEventListener.VoidDelegate(this.OnBtn2Click));
		this.mBtn2Lb = gameObject4.transform.Find("txt1").GetComponent<UILabel>();
		this.mBtn2Bg = gameObject4.transform.Find("bg").gameObject;
		if (Globals.Instance.Player.Data.VipLevel < 5u)
		{
			this.mBtn2Sp.spriteName = "recharge_bg";
			this.mBtn2Lb.text = Singleton<StringManager>.Instance.GetString("petPeiYang3", new object[]
			{
				5
			});
			this.mBtn2Lb.color = new Color32(255, 174, 0, 255);
			this.mBtn2Lb.effectColor = new Color32(81, 0, 0, 255);
			this.mBtn2Bg.SetActive(false);
		}
		else
		{
			this.mBtn2Sp.spriteName = "teamBagBg";
			this.mBtn2Lb.text = Singleton<StringManager>.Instance.GetString("petPeiYang5");
			this.mBtn2Lb.color = this.mBtn0Lb.color;
			this.mBtn2Lb.effectColor = this.mBtn0Lb.effectColor;
			this.mBtn2Bg.SetActive(true);
		}
	}

	private void OnBtn0Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mBaseScene != null)
		{
			this.mBaseScene.SetPeiYangNum(0);
		}
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnBtn1Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.Data.VipLevel < 4u)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("petPeiYang4", 0f, 0f);
			return;
		}
		if (this.mBaseScene != null)
		{
			this.mBaseScene.SetPeiYangNum(1);
		}
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnBtn2Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (Globals.Instance.Player.Data.VipLevel < 5u)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("petPeiYang4", 0f, 0f);
			return;
		}
		if (this.mBaseScene != null)
		{
			this.mBaseScene.SetPeiYangNum(2);
		}
		GameUIPopupManager.GetInstance().PopState(false, null);
	}

	private void OnCloseClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
