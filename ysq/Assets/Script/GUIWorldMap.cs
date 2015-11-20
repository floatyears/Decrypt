using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Game/UI Session/GUIWorldMap")]
public class GUIWorldMap : GameUISession
{
	public enum WorldMapType
	{
		Easy,
		Hard,
		Dreamland = 9,
		END = 3
	}

	private enum eUIPopupState
	{
		EPS_AdReady,
		EPS_Dialog,
		EPS_NewGame,
		EPS_QuestInfomation,
		EPS_Max
	}

	private const int MAP_COUNT = 18;

	private const int IMAGE_BLOCK_SIZE = 512;

	private const int LEVEL_ITEM_COUNT = 10;

	private UIRoot root;

	private GameObject mLeftBtn;

	private GameObject mRightBtn;

	private GameObject[] mBtnToggles = new GameObject[3];

	private GameObject mBox;

	private GameObject mBoxBtn;

	private GameObject mBoxEffect;

	private UILabel mStartLb;

	private GameObject mMapReward;

	private static int IMAGE_BLOCK_ROW = 4;

	private static int IMAGE_BLOCK_COL = 13;

	private Vector2[] WORLD_MAP_POS = new Vector2[]
	{
		new Vector2(0f, 37f),
		new Vector2(30f, 485f),
		new Vector2(0f, 990f),
		new Vector2(220f, 1365f),
		new Vector2(805f, 847f),
		new Vector2(900f, 340f),
		new Vector2(1356f, -25f),
		new Vector2(1422f, 650f),
		new Vector2(1331f, 1240f),
		new Vector2(2617f, 1307f),
		new Vector2(3405f, 1356f),
		new Vector2(2712f, 643f),
		new Vector2(3420f, 546f),
		new Vector2(2600f, 97f),
		new Vector2(3260f, 97f),
		new Vector2(3979f, 81f),
		new Vector2(5402f, 1339f),
		new Vector2(5246f, 956f)
	};

	private Vector2 WORLD_MAP_OFFSET = new Vector2(-312f, 64f);

	private Vector2 BASE_RESOLUTION = new Vector2(1136f, 640f);

	private List<UITexture> mUITexList = new List<UITexture>();

	private Texture[] mTexArray = new Texture[GUIWorldMap.IMAGE_BLOCK_ROW * GUIWorldMap.IMAGE_BLOCK_COL];

	private int curItemCount;

	private SceneNode[] mSceneNodes = new SceneNode[10];

	private Vector2[,,] LEVEL_ITEM_POS = new Vector2[3, 18, 10];

	private GameObject nextLevelEffect;

	private Vector2 curMapPos;

	public static int difficulty;

	private static int curEasyMapIndex;

	private static int curHardMapIndex;

	private static int curDreamMapIndex;

	public int lastMapIndex;

	private bool update;

	private int screenWidth;

	private int screenHeight;

	protected uint mOldLevel;

	private TweenPosition chapterTweenPos;

	private UILabel chapterName;

	private GameObject playerActor;

	private GameObject[] bossActors = new GameObject[10];

	private ResourceEntity playerAsyncEntitys;

	private ResourceEntity[] bossAsyncEntitys = new ResourceEntity[10];

	public GameObject instruction;

	private GameObject newQuestTag;

	private GameObject mNewPet;

	private UILabel nightmareCount;

	private GameObject nightmareFx;

	private int curPopupState;

	public int totalScore
	{
		get;
		private set;
	}

	public int curMapIndex
	{
		get
		{
			if (GUIWorldMap.difficulty == 0)
			{
				return GUIWorldMap.curEasyMapIndex;
			}
			if (GUIWorldMap.difficulty == 1)
			{
				return GUIWorldMap.curHardMapIndex;
			}
			return GUIWorldMap.curDreamMapIndex;
		}
		set
		{
			if (GUIWorldMap.difficulty == 0)
			{
				GUIWorldMap.curEasyMapIndex = Mathf.Min(value, 17);
				return;
			}
			if (GUIWorldMap.difficulty == 1)
			{
				GUIWorldMap.curHardMapIndex = Mathf.Min(value, 17);
				return;
			}
			GUIWorldMap.curDreamMapIndex = Mathf.Min(value, 17);
		}
	}

	private void UpdatePos(float x, float y)
	{
		float num = x + (this.BASE_RESOLUTION.x - (float)this.screenWidth) / 2f;
		float f = num + (float)this.screenWidth;
		float num2 = y + (this.BASE_RESOLUTION.y - (float)this.screenHeight) / 2f;
		float f2 = num2 + (float)this.screenHeight;
		int num3 = Mathf.RoundToInt(num) / 512;
		int num4 = Mathf.RoundToInt(f) / 512;
		if (num4 >= GUIWorldMap.IMAGE_BLOCK_COL)
		{
			num4 = GUIWorldMap.IMAGE_BLOCK_COL - 1;
		}
		int num5 = Mathf.RoundToInt(num2) / 512;
		int num6 = Mathf.RoundToInt(f2) / 512;
		if (num6 >= GUIWorldMap.IMAGE_BLOCK_ROW)
		{
			num6 = GUIWorldMap.IMAGE_BLOCK_ROW - 1;
		}
		int num7 = 0;
		int i = num3;
		int num8 = num3 * GUIWorldMap.IMAGE_BLOCK_ROW;
		while (i <= num4)
		{
			int j = num5;
			int num9 = num8 + num5;
			while (j <= num6)
			{
				UITexture uITexture = this.GetUITexture(num7, num9);
				uITexture.gameObject.transform.localPosition = new Vector3((float)(i * 512) - x + this.WORLD_MAP_OFFSET.x, y - (float)(j * 512) + this.WORLD_MAP_OFFSET.y, 0f);
				uITexture.gameObject.SetActive(true);
				num7++;
				num9++;
				j++;
			}
			num8 += GUIWorldMap.IMAGE_BLOCK_ROW;
			i++;
		}
		for (int k = num7; k < this.mUITexList.Count; k++)
		{
			this.mUITexList[k].gameObject.SetActive(false);
		}
		this.UpdateSceneNodePos(x, y);
	}

	private void UpdateSceneNodePos(float x, float y)
	{
		int num = GUIWorldMap.difficulty;
		if (GUIWorldMap.difficulty == 9)
		{
			num = 2;
		}
		for (int i = 0; i < this.curItemCount; i++)
		{
			this.mSceneNodes[i].gameObject.transform.localPosition = new Vector3(this.LEVEL_ITEM_POS[num, this.curMapIndex, i].x - x, y + this.LEVEL_ITEM_POS[num, this.curMapIndex, i].y, 0f);
		}
	}

	private void Update()
	{
		if (this.update)
		{
			Vector2 vector = this.WORLD_MAP_POS[this.curMapIndex];
			Vector2 a = NGUIMath.SpringLerp(this.curMapPos, vector, 10f, Time.deltaTime);
			if ((a - vector).sqrMagnitude < 0.01f)
			{
				a = vector;
				this.lastMapIndex = this.curMapIndex;
				this.update = false;
			}
			this.curMapPos = a;
			this.UpdatePos((float)((int)this.curMapPos.x), (float)((int)this.curMapPos.y));
		}
	}

	private UITexture GetUITexture(int index, int blockIndex)
	{
		if (this.mUITexList.Count > index)
		{
			UITexture uITexture = this.mUITexList[index];
			Texture texture = this.GetTexture(blockIndex);
			if (uITexture.mainTexture != texture)
			{
				uITexture.mainTexture = texture;
			}
			return uITexture;
		}
		UITexture uITexture2 = NGUITools.AddWidget<UITexture>(base.gameObject);
		uITexture2.name = "Block" + index;
		uITexture2.depth = 0;
		uITexture2.mainTexture = this.GetTexture(blockIndex);
		uITexture2.width = 512;
		uITexture2.height = 512;
		this.mUITexList.Add(uITexture2);
		return uITexture2;
	}

	private Texture GetTexture(int blockIndex)
	{
		if (this.mTexArray.Length <= blockIndex)
		{
			return null;
		}
		if (this.mTexArray[blockIndex] != null)
		{
			return this.mTexArray[blockIndex];
		}
		this.mTexArray[blockIndex] = GUIWorldMap.GetTextureRes(blockIndex);
		return this.mTexArray[blockIndex];
	}

	private static Texture GetTextureRes(int blockIndex)
	{
		return Resources.Load<Texture>(string.Format("WorldMap/worldmap_a_{0:D2}", blockIndex + 1));
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("bg/bg_002", true);
		this.root = GameUIManager.mInstance.uiRoot;
		if (this.root == null)
		{
			return;
		}
		this.CalculateScreenResolution();
		Transform transform = base.transform.Find("topPanel");
		this.instruction = base.transform.FindChild("Instruction").gameObject;
		this.instruction.SetActive(false);
		GameObject gameObject = base.transform.FindChild("Level").gameObject;
		if (gameObject.transform.childCount != 10)
		{
			return;
		}
		SceneNode.mBaseScene = this;
		for (int i = 0; i < 10; i++)
		{
			GameObject gameObject2 = gameObject.transform.Find(i.ToString()).gameObject;
			this.mSceneNodes[i] = gameObject2.AddComponent<SceneNode>();
		}
		this.nextLevelEffect = base.transform.FindChild("ui31").gameObject;
		this.nextLevelEffect.SetActive(false);
		TextAsset textAsset = Resources.Load<TextAsset>("WorldMap/LEVEL_ITEM_POS");
		string[] array = textAsset.text.Split(new char[]
		{
			'\n'
		});
		char[] separator = new char[]
		{
			','
		};
		int j = 0;
		int num = 0;
		while (j < 3)
		{
			for (int k = 0; k < 18; k++)
			{
				int num2 = 0;
				string text;
				while ((text = array[num++].Trim(new char[]
				{
					'\r'
				})) != string.Empty)
				{
					string[] array2 = text.Split(separator);
					this.LEVEL_ITEM_POS[j, k, num2++] = new Vector2(Convert.ToSingle(array2[0]), Convert.ToSingle(array2[1]));
				}
			}
			j++;
		}
		GameObject gameObject3 = GameUITools.RegisterClickEvent("TaskBtn", new UIEventListener.VoidDelegate(this.OnQuestClick), transform.gameObject);
		GameUITools.SetLabelLocalText("Label", "questLb", gameObject3);
		this.newQuestTag = gameObject3.transform.Find("new").gameObject;
		this.newQuestTag.gameObject.SetActive(Globals.Instance.Player.HasQuestReward());
		GameObject parent = base.RegisterClickEvent("PetBtn", new UIEventListener.VoidDelegate(this.OnPetBtnClick), transform.gameObject);
		base.SetLabelLocalText("Label", "petLb", parent);
		this.mNewPet = base.FindGameObject("new", parent);
		this.RefreshTeamBtnNewFlag();
		Transform transform2 = base.transform.Find("chapter-bg/chapter-name");
		this.chapterName = transform2.GetComponent<UILabel>();
		this.chapterTweenPos = transform2.gameObject.GetComponent<TweenPosition>();
		this.chapterTweenPos.method = UITweener.Method.BounceEaseOut;
		this.chapterTweenPos.enabled = false;
		this.chapterTweenPos.duration = 0.2f;
		this.mLeftBtn = transform.FindChild("button-left").gameObject;
		UIEventListener expr_308 = UIEventListener.Get(this.mLeftBtn);
		expr_308.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_308.onClick, new UIEventListener.VoidDelegate(this.OnLeftClick));
		this.mRightBtn = transform.FindChild("button-right").gameObject;
		UIEventListener expr_34A = UIEventListener.Get(this.mRightBtn);
		expr_34A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_34A.onClick, new UIEventListener.VoidDelegate(this.OnRightClick));
		GameObject gameObject4 = transform.FindChild("easy").gameObject;
		gameObject4.name = 0.ToString();
		UIEventListener expr_398 = UIEventListener.Get(gameObject4);
		expr_398.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_398.onClick, new UIEventListener.VoidDelegate(this.OnDifficultyCheck));
		this.mBtnToggles[0] = gameObject4;
		gameObject4 = transform.FindChild("hard").gameObject;
		gameObject4.name = 1.ToString();
		UIEventListener expr_3ED = UIEventListener.Get(gameObject4);
		expr_3ED.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3ED.onClick, new UIEventListener.VoidDelegate(this.OnDifficultyCheck));
		this.mBtnToggles[1] = gameObject4;
		gameObject4 = transform.FindChild("dreamland").gameObject;
		gameObject4.name = 9.ToString();
		UIEventListener expr_443 = UIEventListener.Get(gameObject4);
		expr_443.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_443.onClick, new UIEventListener.VoidDelegate(this.OnDifficultyCheck));
		this.mBtnToggles[2] = gameObject4;
		this.nightmareCount = gameObject4.transform.Find("Checkmark/nightmareCount").GetComponent<UILabel>();
		this.nightmareFx = base.transform.Find("ui80").gameObject;
		this.nightmareFx.SetActive(false);
		this.RefreshNightmareCount();
		gameObject4 = this.mBtnToggles[1].transform.Find("lock").gameObject;
		gameObject4.SetActive(Globals.Instance.Player.GetSceneScore(GameConst.GetInt32(109)) <= 0);
		gameObject4 = this.mBtnToggles[2].transform.Find("lock").gameObject;
		gameObject4.SetActive(Globals.Instance.Player.GetSceneScore(GameConst.GetInt32(61)) <= 0);
		this.mBox = transform.FindChild("box").gameObject;
		this.mBoxBtn = this.mBox.transform.FindChild("box-button").gameObject;
		UIEventListener expr_57F = UIEventListener.Get(this.mBox.gameObject);
		expr_57F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_57F.onClick, new UIEventListener.VoidDelegate(this.OnBoxClick));
		this.mBoxEffect = this.mBox.transform.FindChild("ui30").gameObject;
		this.mStartLb = this.mBox.transform.FindChild("star/Label").gameObject.GetComponent<UILabel>();
		this.InitMapIndex();
		if (GUIWorldMap.difficulty == 0)
		{
			this.mBtnToggles[0].GetComponent<UIToggle>().value = true;
		}
		else
		{
			if (GUIWorldMap.difficulty == 1)
			{
				if (Globals.Instance.Player.GetSceneScore(GameConst.GetInt32(109)) <= 0)
				{
					GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_3"), 0f, 0f);
					GUIWorldMap.difficulty = 0;
					this.mBtnToggles[GUIWorldMap.difficulty].GetComponent<UIToggle>().value = true;
				}
				else
				{
					this.mBtnToggles[1].GetComponent<UIToggle>().value = true;
				}
			}
			if (GUIWorldMap.difficulty == 9)
			{
				if (Globals.Instance.Player.GetSceneScore(GameConst.GetInt32(61)) <= 0)
				{
					GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_6"), 0f, 0f);
					GUIWorldMap.difficulty = 0;
					this.mBtnToggles[GUIWorldMap.difficulty].GetComponent<UIToggle>().value = true;
				}
				else
				{
					this.mBtnToggles[2].GetComponent<UIToggle>().value = true;
					this.nightmareFx.SetActive(true);
				}
			}
		}
		this.lastMapIndex = this.curMapIndex;
		this.curMapPos = this.WORLD_MAP_POS[this.curMapIndex];
		this.RefreshSceneNode();
		this.UpdatePos(this.curMapPos.x, this.curMapPos.y);
		this.curPopupState = 0;
		this.OpenMapPopup();
		float num3 = (float)Screen.width / (float)Screen.height;
		if (num3 > 1.78f || num3 < 1.33f)
		{
			Transform transform3 = base.transform.Find("bg");
			if (transform3 != null)
			{
				transform3.gameObject.SetActive(true);
			}
		}
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("pveLb3");
		topGoods.BackClickListener = new UIEventListener.VoidDelegate(this.OnBackClick);
		UIPanel component = topGoods.gameObject.GetComponent<UIPanel>();
		component.renderQueue = UIPanel.RenderQueue.StartAt;
		component.startingRenderQueue = 4100;
		component.transform.localPosition = new Vector3(component.transform.localPosition.x, component.transform.localPosition.y, component.transform.localPosition.z - 200f);
		LocalPlayer expr_86F = Globals.Instance.Player;
		expr_86F.QuestTakeRewardEvent = (LocalPlayer.TakeRewardCallback)Delegate.Combine(expr_86F.QuestTakeRewardEvent, new LocalPlayer.TakeRewardCallback(this.OnQuestTakeRewardEvent));
		ItemSubSystem expr_89F = Globals.Instance.Player.ItemSystem;
		expr_89F.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Combine(expr_89F.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		LocalPlayer expr_8CA = Globals.Instance.Player;
		expr_8CA.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Combine(expr_8CA.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
	}

	private void OnBackClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		Type type = GameUIManager.mInstance.GobackSession();
		if (type == typeof(GUITeamManageSceneV2))
		{
			GameUIManager.mInstance.ClearGobackSession();
		}
	}

	protected override void OnPreDestroyGUI()
	{
		LocalPlayer expr_0A = Globals.Instance.Player;
		expr_0A.QuestTakeRewardEvent = (LocalPlayer.TakeRewardCallback)Delegate.Remove(expr_0A.QuestTakeRewardEvent, new LocalPlayer.TakeRewardCallback(this.OnQuestTakeRewardEvent));
		ItemSubSystem expr_3A = Globals.Instance.Player.ItemSystem;
		expr_3A.AddItemEvent = (ItemSubSystem.AddItemCallback)Delegate.Remove(expr_3A.AddItemEvent, new ItemSubSystem.AddItemCallback(this.OnAddItemEvent));
		LocalPlayer expr_65 = Globals.Instance.Player;
		expr_65.PlayerUpdateEvent = (LocalPlayer.VoidCallback)Delegate.Remove(expr_65.PlayerUpdateEvent, new LocalPlayer.VoidCallback(this.OnPlayerUpdateEvent));
		if (this.playerAsyncEntitys != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.playerAsyncEntitys);
			this.playerAsyncEntitys = null;
		}
		for (int i = 0; i < this.bossAsyncEntitys.Length; i++)
		{
			if (this.bossAsyncEntitys[i] != null)
			{
				ActorManager.CancelCreateUIActorAsync(this.bossAsyncEntitys[i]);
				this.bossAsyncEntitys[i] = null;
			}
		}
		for (int j = 0; j < this.mTexArray.Length; j++)
		{
			Texture texture = this.mTexArray[j];
			if (texture != null)
			{
				Resources.UnloadAsset(texture);
			}
		}
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		UIPanel component = topGoods.gameObject.GetComponent<UIPanel>();
		component.renderQueue = UIPanel.RenderQueue.Automatic;
		component.transform.localPosition = new Vector3(component.transform.localPosition.x, component.transform.localPosition.y, component.transform.localPosition.z + 200f);
		topGoods.Hide();
		UnityEngine.Object.DestroyImmediate(this.mMapReward);
		GameUIManager.mInstance.uiState.ResultSceneInfo2 = null;
		GameUIManager.mInstance.uiState.PetSceneInfo = null;
		GameUIManager.mInstance.uiState.QuestSceneInfo = null;
		GameUIManager.mInstance.uiState.ResetWMSceneInfo = false;
		GameUIManager.mInstance.uiState.LastQuestInfo = null;
	}

	public void PopupFinishEvent()
	{
		this.OpenMapPopup();
	}

	private void OpenMapPopup()
	{
		switch (this.curPopupState++)
		{
		case 0:
		{
			SceneInfo resultSceneInfo = GameUIManager.mInstance.uiState.ResultSceneInfo;
			if (resultSceneInfo != null)
			{
				this.mSceneNodes[resultSceneInfo.ID % 100 - 1].OpenAdventureReadyPanel(false);
				GameUIManager.mInstance.uiState.ResultSceneInfo = null;
			}
			this.OpenMapPopup();
			break;
		}
		case 1:
			if (!GameUIManager.mInstance.ShowMapUIDialog(0, new GUIPlotDialog.FinishCallback(this.PopupFinishEvent)))
			{
				this.OpenMapPopup();
			}
			break;
		case 2:
			if (GameUIManager.mInstance.uiState.UnlockNewGameLevel == 0 || !GUIUnlockPopUp.Show(GameUIManager.mInstance.uiState.UnlockNewGameLevel, null, new GameUIPopupManager.PopClosedCallback(this.PopupFinishEvent)))
			{
				this.OpenMapPopup();
			}
			break;
		case 3:
		{
			QuestInfo lastQuestInfo = GameUIManager.mInstance.uiState.LastQuestInfo;
			QuestInfo mainQuest = Globals.Instance.Player.MainQuest;
			if (mainQuest != null && lastQuestInfo != mainQuest && Globals.Instance.Player.GetQuestState(mainQuest.ID) == 1)
			{
				if (GameUIManager.mInstance.GetSession<GUIWorldMap>() != null)
				{
					GameUIQuestInformation.GetInstance().Init(base.transform, mainQuest);
				}
			}
			else
			{
				Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
			}
			break;
		}
		}
	}

	public void OnQuestClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.ChangeSession<GUIQuestScene>(null, false, true);
	}

	public void OnPetBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIManager.mInstance.uiState.IsLocalPlayer = true;
		GameUIManager.mInstance.uiState.CombatPetSlot = 0;
		GameUIManager.mInstance.ChangeSession<GUITeamManageSceneV2>(null, false, true);
	}

	public void RefreshTeamBtnNewFlag()
	{
		this.mNewPet.SetActive(false);
		for (int i = 0; i <= 5; i++)
		{
			if (Tools.CanBattlePetMark(i))
			{
				this.mNewPet.SetActive(true);
				break;
			}
		}
	}

	public void OnLeftClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!base.PostLoadGUIDone)
		{
			return;
		}
		if (this.curMapIndex - 1 >= 0)
		{
			this.lastMapIndex = this.curMapIndex--;
			this.ChangeMapPos();
		}
	}

	public void OnRightClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!base.PostLoadGUIDone)
		{
			return;
		}
		if (this.curMapIndex + 1 < 18)
		{
			int num = (GUIWorldMap.difficulty + 1) * 1000;
			MapInfo info = Globals.Instance.AttDB.MapDict.GetInfo(this.curMapIndex + 1 + num);
			if (info == null)
			{
				return;
			}
			int num2 = info.ID / 1000;
			int num3 = info.ID % 1000;
			int id = num2 * 100000 + num3 * 1000 + 1;
			SceneInfo info2 = Globals.Instance.AttDB.SceneDict.GetInfo(id);
			if (info2 == null)
			{
				return;
			}
			if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)info2.MinLevel))
			{
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_4", new object[]
				{
					info2.MinLevel
				}), 0f, 0f);
				return;
			}
			if (this.curMapIndex > 0)
			{
				MapInfo info3 = Globals.Instance.AttDB.MapDict.GetInfo(this.curMapIndex + num);
				if (info3 != null && !Tools.IsMapAllPassed(info3))
				{
					GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_5", new object[]
					{
						info3.Name
					}), 0f, 0f);
					return;
				}
			}
			this.lastMapIndex = this.curMapIndex++;
			this.ChangeMapPos();
		}
	}

	private void RefreshBtn()
	{
		this.mRightBtn.SetActive(this.curMapIndex != 17);
		this.mLeftBtn.SetActive(this.curMapIndex != 0);
	}

	public void OnDifficultyCheck(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		int num = Convert.ToInt32(go.name);
		if (GUIWorldMap.difficulty == num)
		{
			return;
		}
		if (num == 1 && Globals.Instance.Player.GetSceneScore(GameConst.GetInt32(109)) <= 0)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_3"), 0f, 0f);
			this.mBtnToggles[GUIWorldMap.difficulty].GetComponent<UIToggle>().value = true;
			return;
		}
		if (num == 9 && Globals.Instance.Player.GetSceneScore(GameConst.GetInt32(61)) <= 0)
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_6"), 0f, 0f);
			this.mBtnToggles[GUIWorldMap.difficulty].GetComponent<UIToggle>().value = true;
			return;
		}
		if (num == 9)
		{
			this.nightmareFx.SetActive(true);
		}
		else
		{
			this.nightmareFx.SetActive(false);
		}
		GUIWorldMap.difficulty = num;
		this.curMapPos = this.WORLD_MAP_POS[this.curMapIndex];
		this.RefreshSceneNode();
		this.UpdatePos(this.curMapPos.x, this.curMapPos.y);
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	public void OnBoxClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameObject prefab = Res.LoadGUI("GUI/GameUIMapReward");
		this.mMapReward = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, prefab);
		Vector3 localPosition = this.mMapReward.transform.localPosition;
		localPosition.z += 4800f;
		this.mMapReward.transform.localPosition = localPosition;
		this.mMapReward.AddComponent<GameUIMapReward>().Init(this, this.curMapIndex + 1 + (GUIWorldMap.difficulty + 1) * 1000);
		this.mMapReward.SetActive(true);
	}

	public void RefreshBoxEffect(MapInfo curInfo)
	{
		this.mBox.SetActive(true);
		UISprite component = this.mBoxBtn.GetComponent<UISprite>();
		component.spriteName = "chest_small";
		UITweener[] components = this.mBoxBtn.GetComponents<UITweener>();
		int mapRewardMask = Globals.Instance.Player.GetMapRewardMask(curInfo.ID);
		int i;
		for (i = 0; i < 3; i++)
		{
			if (this.totalScore < curInfo.NeedStar[i])
			{
				break;
			}
			if ((mapRewardMask & 1 << i) == 0)
			{
				this.mBoxEffect.SetActive(true);
				for (int j = 0; j < components.Length; j++)
				{
					components[j].enabled = true;
				}
				return;
			}
		}
		if (i == 3)
		{
			component.spriteName = "chest_open";
		}
		this.mBoxEffect.SetActive(false);
		this.mBoxBtn.transform.localRotation = Quaternion.identity;
		for (int k = 0; k < components.Length; k++)
		{
			components[k].enabled = false;
		}
	}

	private void InitMapIndex()
	{
		if (this.InitMapIndex(GameUIManager.mInstance.uiState.ResultSceneInfo))
		{
			return;
		}
		if (this.InitMapIndex(GameUIManager.mInstance.uiState.ResultSceneInfo2))
		{
			return;
		}
		if (this.InitMapIndex(GameUIManager.mInstance.uiState.PetSceneInfo))
		{
			return;
		}
		if (this.InitMapIndex(GameUIManager.mInstance.uiState.QuestSceneInfo))
		{
			return;
		}
		if (GameUIManager.mInstance.uiState.ResetWMSceneInfo && this.InitMapIndex(Tools.GetNextSceneInfo(GUIWorldMap.difficulty)))
		{
			return;
		}
	}

	private bool InitMapIndex(SceneInfo sceneInfo)
	{
		if (sceneInfo != null)
		{
			if (sceneInfo.Difficulty == 9)
			{
				GUIWorldMap.difficulty = 0;
				SceneInfo nextSceneInfo = Tools.GetNextSceneInfo(GUIWorldMap.difficulty);
				this.curMapIndex = nextSceneInfo.MapID - (GUIWorldMap.difficulty + 1) * 1000 - 1;
				GUIWorldMap.difficulty = 1;
				nextSceneInfo = Tools.GetNextSceneInfo(GUIWorldMap.difficulty);
				this.curMapIndex = nextSceneInfo.MapID - (GUIWorldMap.difficulty + 1) * 1000 - 1;
			}
			else
			{
				GUIWorldMap.difficulty = (sceneInfo.Difficulty ^ 1);
				SceneInfo nextSceneInfo2 = Tools.GetNextSceneInfo(GUIWorldMap.difficulty);
				this.curMapIndex = nextSceneInfo2.MapID - (GUIWorldMap.difficulty + 1) * 1000 - 1;
				GUIWorldMap.difficulty = 9;
				nextSceneInfo2 = Tools.GetNextSceneInfo(GUIWorldMap.difficulty);
				this.curMapIndex = nextSceneInfo2.MapID - (GUIWorldMap.difficulty + 1) * 1000 - 1;
			}
			GUIWorldMap.difficulty = sceneInfo.Difficulty;
			this.curMapIndex = sceneInfo.MapID - (GUIWorldMap.difficulty + 1) * 1000 - 1;
			return true;
		}
		return false;
	}

	public void ChangeMapPos()
	{
		this.chapterTweenPos.enabled = true;
		this.update = true;
		this.chapterName.leftAnchor.absolute = -242;
		this.chapterName.rightAnchor.absolute = 35;
		this.chapterName.UpdateAnchors();
		this.chapterTweenPos.from.x = this.chapterTweenPos.gameObject.transform.localPosition.x + 261f;
		this.chapterTweenPos.from.y = this.chapterTweenPos.gameObject.transform.localPosition.y;
		this.chapterTweenPos.to.x = this.chapterTweenPos.gameObject.transform.localPosition.x;
		this.chapterTweenPos.to.y = this.chapterTweenPos.gameObject.transform.localPosition.y;
		this.chapterTweenPos.ResetToBeginning();
		this.curMapPos = this.WORLD_MAP_POS[this.lastMapIndex];
		this.RefreshSceneNode();
		this.UpdatePos(this.curMapPos.x, this.curMapPos.y);
	}

	private void CalculateScreenResolution()
	{
		this.screenWidth = Screen.width;
		this.screenHeight = Screen.height;
		float pixelSizeAdjustment = this.root.GetPixelSizeAdjustment(this.screenHeight);
		this.screenWidth = Mathf.CeilToInt((float)this.screenWidth * pixelSizeAdjustment);
		this.screenHeight = Mathf.CeilToInt((float)(this.screenHeight - 58) * pixelSizeAdjustment);
	}

	private void RefreshSceneNode()
	{
		int num = (GUIWorldMap.difficulty + 1) * 1000;
		MapInfo info = Globals.Instance.AttDB.MapDict.GetInfo(this.curMapIndex + 1 + num);
		if (info == null)
		{
			return;
		}
		this.chapterName.text = string.Format("{0}{1,-20}{2} {3}", new object[]
		{
			this.lastMapIndex + 1,
			string.Empty,
			this.curMapIndex + 1,
			info.Name
		});
		Color32 c;
		Color32 c2;
		if (GUIWorldMap.difficulty == 0)
		{
			c = new Color32(0, 116, 198, 255);
			c2 = new Color32(4, 19, 55, 255);
		}
		else if (GUIWorldMap.difficulty == 1)
		{
			c = new Color32(198, 23, 0, 255);
			c2 = new Color32(55, 27, 4, 255);
		}
		else
		{
			c = new Color32(181, 34, 232, 255);
			c2 = new Color32(48, 3, 77, 255);
		}
		this.chapterName.gradientTop = c;
		this.chapterName.gradientBottom = c2;
		this.RefreshBtn();
		this.totalScore = 0;
		int curPassScene = -1;
		this.curItemCount = 0;
		int lastPassScene = 0;
		int num2 = 0;
		bool flag = false;
		int num3 = this.curMapIndex + 1 + num;
		foreach (SceneInfo current in Globals.Instance.AttDB.SceneDict.Values)
		{
			if (current.MapID == num3 - 1)
			{
				num2++;
				if (Globals.Instance.Player.GetSceneScore(current.ID) > 0)
				{
					lastPassScene = num2;
				}
			}
			if (current.MapID != num3)
			{
				if (flag)
				{
					break;
				}
			}
			else
			{
				flag = true;
				int sceneScore = Globals.Instance.Player.GetSceneScore(current.ID);
				this.totalScore += sceneScore;
				this.mSceneNodes[this.curItemCount].RefreshSceneInfo(current, sceneScore);
				this.mSceneNodes[this.curItemCount].gameObject.SetActive(true);
				if (sceneScore > 0)
				{
					curPassScene = this.curItemCount;
				}
				this.curItemCount++;
				if (this.curItemCount >= 10)
				{
					break;
				}
			}
		}
		for (int i = this.curItemCount; i < 10; i++)
		{
			this.mSceneNodes[i].gameObject.SetActive(false);
		}
		this.mStartLb.text = string.Format("{0}/{1}", this.totalScore, this.curItemCount * 3);
		this.UpdateSceneNodePos(this.curMapPos.x, this.curMapPos.y);
		this.RefreshBoxEffect(info);
		if (!flag)
		{
			return;
		}
		this.CreateSceneNodeActor(curPassScene, lastPassScene, num2);
		SceneInfo petSceneInfo = GameUIManager.mInstance.uiState.PetSceneInfo;
		if (petSceneInfo == null)
		{
			return;
		}
		SceneNode sceneNode = this.mSceneNodes[petSceneInfo.ID % 100 - 1];
		if (sceneNode.sceneInfo != petSceneInfo)
		{
			this.instruction.SetActive(false);
		}
		else
		{
			this.instruction.transform.parent = sceneNode.transform;
			this.instruction.transform.localPosition = new Vector3(-7f, 45f, 0f);
			this.instruction.SetActive(true);
		}
	}

	private void CreateSceneNodeActor(int curPassScene, int lastPassScene, int lastItemCount)
	{
		if (this.curMapIndex < 18 && ((curPassScene == -1 && (this.curMapIndex == 0 || lastPassScene == lastItemCount)) || (curPassScene != -1 && curPassScene < this.curItemCount - 1)))
		{
			this.mSceneNodes[curPassScene + 1].disable = false;
			if (this.playerActor == null)
			{
				if (this.playerAsyncEntitys != null)
				{
					ActorManager.CancelCreateUIActorAsync(this.playerAsyncEntitys);
					this.playerAsyncEntitys = null;
				}
				this.playerAsyncEntitys = this.CreateWorldMapPlayer(base.transform, delegate(GameObject go)
				{
					this.playerAsyncEntitys = null;
					this.playerActor = go;
					if (this.playerActor != null)
					{
						this.playerActor.animation.clip = this.playerActor.animation.GetClip("run");
						this.playerActor.animation.wrapMode = WrapMode.Loop;
						this.playerActor.transform.parent = this.mSceneNodes[curPassScene + 1].transform;
						this.playerActor.transform.localPosition = new Vector3(0f, 0f, -100f);
						this.playerActor.transform.localRotation = Quaternion.Euler(0f, (float)((this.mSceneNodes[curPassScene + 1].sceneInfo.UIActorRotation != 0) ? 120 : -120), 0f);
						this.playerActor.SetActive(true);
					}
				});
			}
			else
			{
				this.playerActor.transform.parent = this.mSceneNodes[curPassScene + 1].transform;
				this.playerActor.transform.localPosition = new Vector3(0f, 0f, -100f);
				this.playerActor.transform.localRotation = Quaternion.Euler(0f, (float)((this.mSceneNodes[curPassScene + 1].sceneInfo.UIActorRotation != 0) ? 120 : -120), 0f);
				this.playerActor.SetActive(true);
			}
			this.nextLevelEffect.transform.parent = this.mSceneNodes[curPassScene + 1].transform;
			this.nextLevelEffect.transform.localPosition = Vector3.zero;
			this.nextLevelEffect.SetActive(true);
			for (int i = 0; i < this.curItemCount; i++)
			{
				GameObject gameObject = this.bossActors[i];
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
					this.bossActors[i] = null;
				}
				if (this.bossAsyncEntitys[i] != null)
				{
					ActorManager.CancelCreateUIActorAsync(this.bossAsyncEntitys[i]);
					this.bossAsyncEntitys[i] = null;
				}
				SceneInfo sceneInfo = this.mSceneNodes[i].sceneInfo;
				if (sceneInfo.UIBoss && curPassScene + 1 < i)
				{
					this.bossAsyncEntitys[i] = this.CreateWorldMapMaster(i, sceneInfo, this.mSceneNodes[this.curItemCount - 1].transform);
				}
			}
		}
		else
		{
			if (this.playerActor != null)
			{
				this.playerActor.SetActive(false);
			}
			for (int j = 0; j < this.bossActors.Length; j++)
			{
				if (this.bossActors[j] != null)
				{
					this.bossActors[j].SetActive(false);
				}
			}
			this.nextLevelEffect.SetActive(false);
		}
	}

	public ResourceEntity CreateWorldMapMaster(int index, SceneInfo sceneInfo, Transform parent)
	{
		for (int i = 2; i >= 0; i--)
		{
			MonsterInfo info = Globals.Instance.AttDB.MonsterDict.GetInfo(sceneInfo.Enemy[i]);
			if (info != null)
			{
				return GUIWorldMap.CreateWorldMapActorAsnc(info.ResLoc, string.Empty, parent, info.ScaleInUI, 180f, delegate(GameObject go)
				{
					this.bossAsyncEntitys[index] = null;
					if (go != null)
					{
						if (go.animation != null)
						{
							go.animation.clip = go.animation.GetClip("std");
							go.animation.wrapMode = WrapMode.Loop;
						}
						go.transform.localPosition = new Vector3(0f, 0f, -100f);
						go.SetActive(true);
						this.bossActors[index] = go;
					}
				});
			}
		}
		return null;
	}

	public ResourceEntity CreateWorldMapPlayer(Transform parent, Action<GameObject> cb)
	{
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(0);
		if (socket != null && socket.IsPlayer())
		{
			return GUIWorldMap.CreateWorldMapActorAsnc(socket.GetResLoc(), socket.GetWeaponResLoc(), parent, 1f, 120f, cb);
		}
		if (cb != null)
		{
			cb(null);
		}
		return null;
	}

	public static ResourceEntity CreateWorldMapActorAsnc(string prefabPath, string weaponPrefabPath, Transform parent, float scale = 1f, float rotation = 180f, Action<GameObject> callback = null)
	{
		return Globals.Instance.ResourceCache.LoadResourceAsync(prefabPath, typeof(GameObject), delegate(UnityEngine.Object resActorPrefab)
		{
			GameObject obj = null;
			if (resActorPrefab == null)
			{
				global::Debug.LogErrorFormat("CreateUIActorAsync error, name = {0}", new object[]
				{
					prefabPath
				});
			}
			else
			{
				UnityEngine.Object weaponPrefab = (!string.IsNullOrEmpty(weaponPrefabPath)) ? Globals.Instance.ResourceCache.LoadResource<GameObject>(weaponPrefabPath, 0f) : null;
				obj = GUIWorldMap.CreateWorldMapActor(resActorPrefab, weaponPrefab, parent, scale, rotation, callback);
			}
			if (callback != null)
			{
				callback(obj);
			}
		}, 0f);
	}

	public static GameObject CreateWorldMapActor(UnityEngine.Object actorPrefab, UnityEngine.Object weaponPrefab, Transform parent, float scale = 1f, float rotation = 180f, Action<GameObject> cb = null)
	{
		if (actorPrefab == null)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(actorPrefab) as GameObject;
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Instantiate error, name = {0}", actorPrefab.name)
			});
		}
		else
		{
			NGUITools.SetLayer(gameObject, LayerDefine.uiLayer);
			ParticleScaler particleScaler = gameObject.AddComponent<ParticleScaler>();
			particleScaler.scaleByUIRoot = false;
			particleScaler.scaleByParent = true;
			particleScaler.renderQueue = 4000;
			if (parent != null)
			{
				gameObject.transform.parent = parent;
				gameObject.transform.localPosition = Vector3.zero;
				scale *= 80f;
				gameObject.transform.localScale = new Vector3(scale, scale, scale);
				gameObject.transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
				gameObject.SetActive(false);
			}
			if (weaponPrefab != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(weaponPrefab) as GameObject;
				if (gameObject2 == null)
				{
					global::Debug.LogError(new object[]
					{
						string.Format("Instantiate error, path = {0}", weaponPrefab.name)
					});
				}
				else
				{
					NGUITools.SetLayer(gameObject2, LayerDefine.uiLayer);
					GameObject gameObject3 = ObjectUtil.FindChildObject(gameObject, "W_Rhand");
					if (gameObject3 == null)
					{
						global::Debug.LogError(new object[]
						{
							"Can not find socket : W_Rhand"
						});
					}
					else
					{
						gameObject2.transform.parent = gameObject3.transform;
						gameObject2.transform.localPosition = Vector3.zero;
						gameObject2.transform.localRotation = Quaternion.identity;
						gameObject2.transform.localScale = Vector3.one;
					}
				}
			}
		}
		if (cb != null)
		{
			cb(gameObject);
		}
		return gameObject;
	}

	private void OnQuestTakeRewardEvent(int id)
	{
		if (Globals.Instance.Player.HasQuestReward())
		{
			this.newQuestTag.gameObject.SetActive(true);
		}
		else
		{
			this.newQuestTag.gameObject.SetActive(false);
		}
		base.StartCoroutine(GameUIQuestInformation.PlayCardAnim(id, base.transform));
	}

	private void OnAddItemEvent(ItemDataEx idEx)
	{
		if (idEx != null && (idEx.Info.Type == 0 || idEx.Info.Type == 1))
		{
			this.RefreshTeamBtnNewFlag();
		}
	}

	private void RefreshNightmareCount()
	{
		int num = GameConst.GetInt32(124) - Globals.Instance.Player.Data.NightmareCount;
		this.nightmareCount.text = Singleton<StringManager>.Instance.GetString("WorldMap_7", new object[]
		{
			(num <= 0) ? "[ff0000]" : "[00ff00]",
			(num <= 0) ? 0 : num
		});
	}

	private void OnPlayerUpdateEvent()
	{
		this.RefreshNightmareCount();
	}
}
