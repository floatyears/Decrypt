using Att;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public sealed class GameUILevelupPanel : MonoBehaviour
{
	private static GameUILevelupPanel mInstance;

	public GameUIPopupManager.PopClosedCallback mCloseCallback;

	public GameObject effectPrefab;

	private GameObject effectObj;

	private GameObject levelBtnClose;

	private Transform levelupBg;

	private Transform levelupWinBg;

	private static Type[] UnlockPopUpSessions = new Type[]
	{
		typeof(GUIQuestScene),
		typeof(GUIWorldMap),
		typeof(GUIPillageScene),
		typeof(GUIPillageResultScene),
		typeof(GUIAchievementScene),
		typeof(GUIKingRewardResultScene),
		typeof(GUIPVPResultScene),
		typeof(GUIPropsBagScene)
	};

	public int levelupUnlockLevel
	{
		get;
		private set;
	}

	public static bool TryClose()
	{
		if (GameUILevelupPanel.mInstance != null)
		{
			GameUILevelupPanel.mInstance.OnCloseLevelupBgAnimEnd();
			return true;
		}
		return false;
	}

	public void Init(Transform parent, GameUIPopupManager.PopClosedCallback callback)
	{
		this.CreateObjects();
		Transform transform = base.transform;
		transform.parent = parent;
		transform.localPosition = new Vector3(0f, 0f, -1000f);
		transform.localScale = Vector3.one;
		this.mCloseCallback = callback;
		this.ShowLevelupPanel();
		Globals.Instance.EffectSoundMgr.Play("ui/ui_014");
		this.levelBtnClose = this.levelupWinBg.transform.FindChild("closeBtn").gameObject;
		UIEventListener expr_86 = UIEventListener.Get(this.levelBtnClose);
		expr_86.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_86.onClick, new UIEventListener.VoidDelegate(this.OnLevelBtnCloseClick));
		UIEventListener expr_C6 = UIEventListener.Get(this.levelupBg.transform.FindChild("resultWindow").gameObject);
		expr_C6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C6.onClick, new UIEventListener.VoidDelegate(this.OnLevelBtnCloseClick));
	}

	private void UnRegisterBGClick()
	{
		UIEventListener expr_0B = UIEventListener.Get(this.levelBtnClose);
		expr_0B.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(expr_0B.onClick, new UIEventListener.VoidDelegate(this.OnLevelBtnCloseClick));
		UIEventListener expr_4B = UIEventListener.Get(this.levelupBg.transform.FindChild("resultWindow").gameObject);
		expr_4B.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(expr_4B.onClick, new UIEventListener.VoidDelegate(this.OnLevelBtnCloseClick));
	}

	private void RegisterBGClick()
	{
		UIEventListener expr_0B = UIEventListener.Get(this.levelBtnClose);
		expr_0B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_0B.onClick, new UIEventListener.VoidDelegate(this.OnLevelBtnCloseClick));
		UIEventListener expr_4B = UIEventListener.Get(this.levelupBg.transform.FindChild("resultWindow").gameObject);
		expr_4B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_4B.onClick, new UIEventListener.VoidDelegate(this.OnLevelBtnCloseClick));
	}

	private void CreateObjects()
	{
		this.levelupBg = base.transform.Find("LevelupBg");
		this.levelupWinBg = this.levelupBg.Find("winBG");
		this.effectObj = (GameObject)UnityEngine.Object.Instantiate(this.effectPrefab);
		this.effectObj.transform.parent = base.transform;
		Vector3 localPosition = this.effectObj.transform.localPosition;
		localPosition.x = 0f;
		localPosition.z = -200f;
		this.effectObj.transform.localPosition = localPosition;
		this.effectObj.AddComponent<GameRenderQueue>().renderQueue = 4400;
		this.effectObj.SetActive(false);
	}

	private void OnLevelBtnCloseClick(GameObject go)
	{
		this.CloseSelf();
	}

	private void CloseSelf()
	{
		GameUITools.PlayCloseWindowAnim(this.levelupWinBg, new TweenDelegate.TweenCallback(this.OnCloseLevelupBgAnimEnd), true);
	}

	private void OnCloseLevelupBgAnimEnd()
	{
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
		if (this.levelupUnlockLevel > 0)
		{
			bool flag = false;
			if (GameUIManager.mInstance.IsSessionExisted(GameUILevelupPanel.UnlockPopUpSessions))
			{
				flag = GUIUnlockPopUp.Show(this.levelupUnlockLevel, null, this.mCloseCallback);
			}
			if (!flag)
			{
				GameUIManager.mInstance.uiState.UnlockNewGameLevel = this.levelupUnlockLevel;
				if (this.mCloseCallback != null)
				{
					this.mCloseCallback();
				}
				GameUIManager.mInstance.TryCommend(ECommentType.EComment_Level, 0f);
			}
		}
		else
		{
			if (this.mCloseCallback != null)
			{
				this.mCloseCallback();
			}
			GameUIManager.mInstance.TryCommend(ECommentType.EComment_Level, 0f);
		}
		UnityEngine.Object.DestroyImmediate(base.gameObject);
		GameUILevelupPanel.mInstance = null;
	}

	public static GameUILevelupPanel GetInstance()
	{
		if (GameUILevelupPanel.mInstance == null)
		{
			GameObject original = Res.LoadGUI("GUI/GameUILevelupPanel");
			GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
			GameUILevelupPanel.mInstance = gameObject.GetComponent<GameUILevelupPanel>();
		}
		return GameUILevelupPanel.mInstance;
	}

	private void ShowLevelupPanel()
	{
		this.effectObj.SetActive(true);
		GameUIState uiState = GameUIManager.mInstance.uiState;
		ObscuredStats data = Globals.Instance.Player.Data;
		if (Globals.Instance.AttDB.LevelDict.GetInfo((int)uiState.PlayerLevel) == null)
		{
			return;
		}
		this.levelupWinBg.FindChild("lvTxt/lv").GetComponent<UILabel>().text = uiState.PlayerLevel.ToString();
		this.levelupWinBg.FindChild("lvTxt/lvTo").GetComponent<UILabel>().text = data.Level.ToString();
		this.levelupWinBg.FindChild("keyTxt/key").GetComponent<UILabel>().text = uiState.PlayerEnergy.ToString();
		this.levelupWinBg.FindChild("keyTxt/keyTo").GetComponent<UILabel>().text = data.Energy.ToString();
		PetDataEx pet = Globals.Instance.Player.TeamSystem.GetPet(0);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		pet.GetAttribute(ref num, ref num2, ref num3, ref num4);
		this.levelupWinBg.FindChild("hpTxt/hp").GetComponent<UILabel>().text = uiState.MaxHp.ToString();
		this.levelupWinBg.FindChild("hpTxt/hpTo").GetComponent<UILabel>().text = num.ToString();
		this.levelupWinBg.FindChild("attackTxt/attack").GetComponent<UILabel>().text = uiState.Attack.ToString();
		this.levelupWinBg.FindChild("attackTxt/attackTo").GetComponent<UILabel>().text = num2.ToString();
		this.levelupWinBg.FindChild("defPhyTxt/defense").GetComponent<UILabel>().text = uiState.WuFang.ToString();
		this.levelupWinBg.FindChild("defPhyTxt/defenseTo").GetComponent<UILabel>().text = num3.ToString();
		this.levelupWinBg.FindChild("defMgcTxt/defense").GetComponent<UILabel>().text = uiState.FaFang.ToString();
		this.levelupWinBg.FindChild("defMgcTxt/defenseTo").GetComponent<UILabel>().text = num4.ToString();
		SceneInfo nextMapSceneInfo = Tools.GetNextMapSceneInfo(0, uiState.PlayerLevel);
		UILabel component = this.levelupWinBg.FindChild("ChapterTxt").GetComponent<UILabel>();
		GameObject gameObject = this.levelupWinBg.FindChild("line1").gameObject;
		gameObject.SetActive(false);
		if ((ulong)data.Level >= (ulong)((long)nextMapSceneInfo.MinLevel) && (long)nextMapSceneInfo.MinLevel > (long)((ulong)uiState.PlayerLevel))
		{
			gameObject.SetActive(true);
			component.text = Singleton<StringManager>.Instance.GetString("MapIndex", new object[]
			{
				nextMapSceneInfo.MapID % 100
			});
			component.gameObject.SetActive(true);
		}
		else
		{
			component.gameObject.SetActive(false);
		}
		UILabel component2 = this.levelupWinBg.FindChild("FunctionTxt").GetComponent<UILabel>();
		string text = string.Empty;
		int num5 = (int)(uiState.PlayerLevel + 1u);
		while ((long)num5 <= (long)((ulong)data.Level))
		{
			string unlockFunc = Singleton<StringManager>.Instance.GetUnlockFunc(num5, false);
			if (!string.IsNullOrEmpty(unlockFunc))
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				text += unlockFunc;
				this.levelupUnlockLevel = num5;
			}
			num5++;
		}
		if (!string.IsNullOrEmpty(text))
		{
			gameObject.SetActive(true);
			component2.text = text;
			component2.gameObject.SetActive(true);
			if (!component.gameObject.activeSelf)
			{
				Vector3 localPosition = component.transform.localPosition;
				localPosition.y += 11f;
				component2.transform.localPosition = localPosition;
			}
		}
		else
		{
			this.levelupUnlockLevel = 0;
			component2.gameObject.SetActive(false);
		}
		int num6 = 292;
		if (gameObject.activeSelf)
		{
			num6 = 312;
		}
		if (component2.gameObject.activeSelf)
		{
			num6 += 40 + component2.height - 20;
		}
		if (component.gameObject.activeSelf)
		{
			num6 += 40;
		}
		this.levelupWinBg.GetComponent<UISprite>().height = num6;
		this.levelupWinBg.transform.localPosition = new Vector3(0f, (float)(num6 / 2), 0f);
	}
}
