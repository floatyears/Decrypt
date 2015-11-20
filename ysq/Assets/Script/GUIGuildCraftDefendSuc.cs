using System;
using UnityEngine;

public class GUIGuildCraftDefendSuc : MonoBehaviour
{
	private static GUIGuildCraftDefendSuc mInstance;

	private GameObject mBgSp;

	private GameObject mUI97;

	public static void ShowMe()
	{
		if (GUIGuildCraftDefendSuc.mInstance == null)
		{
			GUIGuildCraftDefendSuc.CreateInstance();
		}
		GUIGuildCraftDefendSuc.mInstance.Init();
	}

	public static void CloseMe()
	{
		if (GUIGuildCraftDefendSuc.mInstance != null)
		{
			UnityEngine.Object.Destroy(GUIGuildCraftDefendSuc.mInstance.gameObject);
			GUIGuildCraftDefendSuc.mInstance = null;
		}
	}

	private static void CreateInstance()
	{
		if (GUIGuildCraftDefendSuc.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIGuildCraftDefendSuc");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIGuildCraftDefendSuc error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUIGuildCraftJieGuo error"
			});
			return;
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 1000f);
		GUIGuildCraftDefendSuc.mInstance = gameObject2.AddComponent<GUIGuildCraftDefendSuc>();
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBgSp = base.transform.Find("result").gameObject;
		UILabel component = this.mBgSp.transform.Find("txt").GetComponent<UILabel>();
		component.text = Singleton<StringManager>.Instance.GetString("guildCraft71");
		this.mUI97 = base.transform.Find("ui97").gameObject;
		Tools.SetParticleRenderQueue2(this.mUI97, 3900);
		NGUITools.SetActive(this.mUI97, false);
		this.mBgSp.transform.localScale = Vector3.zero;
	}

	public void Init()
	{
		GameUITools.PlayOpenWindowAnim(this.mBgSp.transform, delegate
		{
			NGUITools.SetActive(this.mUI97, true);
			base.Invoke("DestroyMe", 2.5f);
		}, true);
	}

	public void DestroyMe()
	{
		GUIGuildCraftDefendSuc.CloseMe();
	}
}
