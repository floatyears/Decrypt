using Proto;
using System;
using UnityEngine;

public class GUIGuildCraftJieGuo : MonoBehaviour
{
	private static GUIGuildCraftJieGuo mInstance;

	private UITexture mResultSp;

	private GameObject mEffect90_1;

	private GameObject mEffect90;

	public static bool IsActive()
	{
		return GUIGuildCraftJieGuo.mInstance != null;
	}

	public static void ShowMe()
	{
		if (GUIGuildCraftJieGuo.mInstance == null)
		{
			GUIGuildCraftJieGuo.CreateInstance();
		}
		GUIGuildCraftJieGuo.mInstance.Refresh();
	}

	public static void CloseMe()
	{
		if (GUIGuildCraftJieGuo.mInstance != null)
		{
			UnityEngine.Object.Destroy(GUIGuildCraftJieGuo.mInstance.gameObject);
			GUIGuildCraftJieGuo.mInstance = null;
		}
	}

	private static void CreateInstance()
	{
		if (GUIGuildCraftJieGuo.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUIGuildCraftJieGuo");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUIGuildCraftJieGuo error"
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
		GUIGuildCraftJieGuo.mInstance = gameObject2.AddComponent<GUIGuildCraftJieGuo>();
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mResultSp = base.transform.Find("Panel/result").GetComponent<UITexture>();
		this.mEffect90_1 = base.transform.Find("ui90_1").gameObject;
		Tools.SetParticleRenderQueue2(this.mEffect90_1, 5610);
		NGUITools.SetActive(this.mEffect90_1, false);
		this.mEffect90 = base.transform.Find("Panel/ui90").gameObject;
		Tools.SetParticleRenderQueue2(this.mEffect90, 5710);
		NGUITools.SetActive(this.mEffect90, false);
	}

	public void Refresh()
	{
		NGUITools.SetActive(this.mEffect90_1, false);
		NGUITools.SetActive(this.mEffect90, false);
		EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
		if (selfTeamFlag != EGuildWarTeamId.EGWTI_None)
		{
			GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
			if (mGWEnterData == null)
			{
				return;
			}
			if (mGWEnterData.Winner == selfTeamFlag)
			{
				Texture mainTexture = Res.Load<Texture>("Craft/craftShengLi", false);
				this.mResultSp.mainTexture = mainTexture;
				NGUITools.SetActive(this.mEffect90_1, true);
				NGUITools.SetActive(this.mEffect90, true);
				Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("ui/ui_004", false);
			}
			else
			{
				Texture mainTexture2 = Res.Load<Texture>("Craft/craftShiBai", false);
				this.mResultSp.mainTexture = mainTexture2;
				Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("ui/ui_005", false);
			}
		}
	}
}
