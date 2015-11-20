using Att;
using System;
using UnityEngine;

public class SceneNode : MonoBehaviour
{
	public static GUIWorldMap mBaseScene;

	public bool disable;

	private UIButton bkgroud;

	private UISprite banner;

	private GameObject[] mStarScore = new GameObject[3];

	private GameObject adventureReady;

	public SceneInfo sceneInfo
	{
		get;
		private set;
	}

	private void Awake()
	{
		this.bkgroud = base.gameObject.GetComponent<UIButton>();
		this.banner = base.transform.Find("Banner").GetComponent<UISprite>();
		for (int i = 0; i < 3; i++)
		{
			this.mStarScore[i] = base.transform.FindChild(string.Format("star{0}", i)).gameObject;
		}
		UIEventListener expr_71 = UIEventListener.Get(base.gameObject);
		expr_71.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_71.onClick, new UIEventListener.VoidDelegate(this.OnSceneNodeClicked));
	}

	private void OnDestroy()
	{
		UIEventListener expr_0B = UIEventListener.Get(base.gameObject);
		expr_0B.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(expr_0B.onClick, new UIEventListener.VoidDelegate(this.OnSceneNodeClicked));
		UnityEngine.Object.Destroy(this.adventureReady);
	}

	public static string GetSceneIcon(SceneInfo sceneInfo, bool recommend)
	{
		if (sceneInfo == null)
		{
			return string.Empty;
		}
		if (sceneInfo.Difficulty == 0)
		{
			return (!recommend) ? "easy_1" : "easy";
		}
		if (sceneInfo.Difficulty == 1)
		{
			return (!recommend) ? "hard_1" : "hard";
		}
		return "dreamland";
	}

	public void RefreshSceneInfo(SceneInfo info, int sceneScore)
	{
		if (info == null)
		{
			return;
		}
		this.sceneInfo = info;
		bool flag = this.sceneInfo.DayTimes <= 10;
		if (sceneScore == 0)
		{
			this.mStarScore[0].SetActive(false);
			this.mStarScore[1].SetActive(false);
			this.mStarScore[2].SetActive(false);
			this.banner.gameObject.SetActive(false);
			this.bkgroud.normalSprite = ((!flag) ? "Disable_1" : "Disable");
			this.disable = true;
			return;
		}
		this.disable = false;
		this.banner.gameObject.SetActive(flag);
		this.bkgroud.normalSprite = SceneNode.GetSceneIcon(info, flag);
		switch (sceneScore)
		{
		case 1:
			this.mStarScore[0].SetActive(true);
			this.mStarScore[1].SetActive(false);
			this.mStarScore[2].SetActive(false);
			this.mStarScore[0].transform.localPosition = new Vector3(0f, -35f, 0f);
			break;
		case 2:
			this.mStarScore[0].SetActive(true);
			this.mStarScore[1].SetActive(true);
			this.mStarScore[2].SetActive(false);
			this.mStarScore[0].transform.localPosition = new Vector3(-20f, -32f, 0f);
			this.mStarScore[1].transform.localPosition = new Vector3(20f, -32f, 0f);
			break;
		case 3:
			this.mStarScore[0].SetActive(true);
			this.mStarScore[1].SetActive(true);
			this.mStarScore[2].SetActive(true);
			this.mStarScore[0].transform.localPosition = new Vector3(-34f, -26f, 0f);
			this.mStarScore[1].transform.localPosition = new Vector3(0f, -35f, 0f);
			this.mStarScore[2].transform.localPosition = new Vector3(34f, -26f, 0f);
			break;
		}
	}

	public void OnSceneNodeClicked(GameObject go)
	{
		this.OpenAdventureReadyPanel(true);
	}

	public void OpenAdventureReadyPanel(bool showMsgTip)
	{
		if (this.adventureReady != null)
		{
			return;
		}
		if (this.sceneInfo.MapID % 100 > 18)
		{
			if (showMsgTip)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("notopened", 0f, 0f);
			}
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (this.sceneInfo.Difficulty != 0 && this.sceneInfo.PreID2 != 0 && player.GetSceneScore(this.sceneInfo.PreID2) <= 0)
		{
			SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(this.sceneInfo.PreID2);
			MapInfo info2 = Globals.Instance.AttDB.MapDict.GetInfo(info.MapID);
			if (showMsgTip)
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_1", new object[]
				{
					info2.Name
				}), 0f, 0f);
			}
			return;
		}
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)this.sceneInfo.MinLevel))
		{
			if (showMsgTip)
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_2", new object[]
				{
					this.sceneInfo.MinLevel
				}), 0f, 0f);
			}
			return;
		}
		if (this.disable || (this.sceneInfo.PreID != 0 && player.GetSceneScore(this.sceneInfo.PreID) <= 0))
		{
			if (showMsgTip)
			{
				GameUIManager.mInstance.ShowMessageTip("PveR", 2);
			}
			return;
		}
		if (GameUIManager.mInstance.ShowMapUIDialog(this.sceneInfo.ID, new GUIPlotDialog.FinishCallback(this.DialogFinish)))
		{
			return;
		}
		this.DialogFinish();
	}

	private void DialogFinish()
	{
		GameObject prefab = Res.LoadGUI("GUI/GameUIAdventureReady");
		this.adventureReady = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, prefab);
		this.adventureReady.AddComponent<GameUIAdventureReady>().Init(this.sceneInfo);
		this.adventureReady.SetActive(true);
		Vector3 localPosition = this.adventureReady.transform.localPosition;
		localPosition.z += 5000f;
		this.adventureReady.transform.localPosition = localPosition;
	}
}
