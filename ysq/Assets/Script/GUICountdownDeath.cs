using System;
using UnityEngine;

public class GUICountdownDeath : MonoBehaviour
{
	private static GUICountdownDeath mInstance;

	private UISprite mTime0;

	private UISprite mTime1;

	private int time;

	private float timerRefresh;

	public static void Show(int time)
	{
		if (time < 1)
		{
			return;
		}
		if (GUICountdownDeath.mInstance == null)
		{
			GUICountdownDeath.CreateInstance();
		}
		GUICountdownDeath.mInstance.Init(time);
	}

	private static void CreateInstance()
	{
		if (GUICountdownDeath.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUICountdownDeath");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUICountdownDeath error"
			});
			return;
		}
		GUICombatMain session = GameUIManager.mInstance.GetSession<GUICombatMain>();
		GameObject gameObject2;
		if (session != null)
		{
			gameObject2 = NGUITools.AddChild(session.gameObject, gameObject);
		}
		else
		{
			gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		}
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUICountdownDeath error"
			});
			return;
		}
		if (session != null)
		{
			gameObject2.transform.localPosition = Vector3.zero;
		}
		else
		{
			gameObject2.transform.localPosition = new Vector3(0f, 0f, 5000f);
		}
		GUICountdownDeath.mInstance = gameObject2.AddComponent<GUICountdownDeath>();
	}

	private void Init(int time)
	{
		UISprite uISprite = GameUITools.FindUISprite("Countdown", base.gameObject);
		this.mTime0 = GameUITools.FindUISprite("Time0", base.gameObject);
		this.mTime1 = GameUITools.FindUISprite("Time1", this.mTime0.gameObject);
		Vector3 localPosition = uISprite.transform.localPosition;
		localPosition.x = (float)(-(float)(uISprite.width + this.mTime0.width) / 2);
		uISprite.transform.localPosition = localPosition;
		this.time = time;
		this.timerRefresh = Time.time;
		this.Refresh();
	}

	private void Refresh()
	{
		if (this.time > 0)
		{
			if (this.time >= 10)
			{
				this.mTime0.spriteName = string.Format("num{0}", this.time / 10);
				this.mTime1.enabled = true;
				this.mTime1.spriteName = string.Format("num{0}", this.time % 10);
			}
			else
			{
				this.mTime0.spriteName = string.Format("num{0}", this.time);
				this.mTime1.enabled = false;
			}
		}
		else
		{
			this.Close();
		}
	}

	private void Update()
	{
		if (this.time > 0 && Time.time - this.timerRefresh >= 1f)
		{
			this.timerRefresh = Time.time;
			this.time--;
			this.Refresh();
		}
	}

	private void Close()
	{
		if (GUICountdownDeath.mInstance != null)
		{
			UnityEngine.Object.Destroy(GUICountdownDeath.mInstance.gameObject);
			GUICountdownDeath.mInstance = null;
		}
	}
}
