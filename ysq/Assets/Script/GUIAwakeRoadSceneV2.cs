using Att;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIAwakeRoadSceneV2 : GameUISession
{
	private const int SceneNodeNum = 5;

	private GameObject mLeftBtn;

	private GameObject mRightBtn;

	private GameObject mPageLeftBtn;

	private GameObject mPageRightBtn;

	private Transform mLvlBgGo;

	public GUIAwakeRoadSceneNode[] mSceneNodes = new GUIAwakeRoadSceneNode[5];

	private GUIAwakeQiaoYuNode mGUIAwakeQiaoYuNode;

	private UIScrollBar mTabBtnScrollBar;

	private GUIAwakePageItemTable mGUIAwakePageItemTable;

	private GameObject mBox;

	private GameObject mBoxBtn;

	private GameObject mBoxEffect;

	private UILabel mStartLb;

	private GameObject instruction;

	private GameObject mMapReward;

	private int mCurPageIndex;

	public int totalScore
	{
		get;
		private set;
	}

	public int CurPageIndex
	{
		get
		{
			return this.mCurPageIndex;
		}
		set
		{
			this.mCurPageIndex = value;
			this.mGUIAwakePageItemTable.SetPageIndex(this.mCurPageIndex);
			this.mLeftBtn.SetActive(this.mCurPageIndex != 1);
			this.mRightBtn.SetActive(this.mCurPageIndex != 32);
			this.RefreshSceneNode();
		}
	}

	public static bool CanShowRedTag()
	{
		if (Tools.CanPlay(GameConst.GetInt32(24), true))
		{
			for (int i = 1; i <= 32; i++)
			{
				int num = i + 3000;
				MapInfo info = Globals.Instance.AttDB.MapDict.GetInfo(num);
				if (info != null)
				{
					int num2 = 0;
					for (int j = 1; j <= 5; j++)
					{
						int sceneID = 600000 + i * 1000 + j;
						if (Globals.Instance.Player.GetSceneScore(sceneID) == 0)
						{
							break;
						}
						num2 += Globals.Instance.Player.GetSceneScore(sceneID);
					}
					int mapRewardMask = Globals.Instance.Player.GetMapRewardMask(num);
					for (int k = 0; k < 3; k++)
					{
						if (num2 >= info.NeedStar[k] && (mapRewardMask & 1 << k) == 0)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public static void TryOpen(SceneInfo sceneInfo)
	{
		if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(24)))
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("mysteryOpenTip", new object[]
			{
				GameConst.GetInt32(24)
			}), 0f, 0f);
			return;
		}
		GameUIManager.mInstance.uiState.AdventureSceneInfo = sceneInfo;
		GameUIManager.mInstance.ChangeSession<GUIAwakeRoadSceneV2>(null, false, true);
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("UIMiddle");
		Transform transform2 = transform.Find("winBg");
		Transform transform3 = transform2.Find("bottomPanel");
		transform2.GetComponent<UITexture>().mainTexture = Res.Load<Texture>("MainBg/awakeBigBg", false);
		GameObject go = GameUITools.FindGameObject("ui76", transform2.gameObject);
		Tools.SetParticleRQWithUIScale(go, 3000);
		this.mBox = transform2.Find("box").gameObject;
		this.mBoxBtn = this.mBox.transform.Find("box-button").gameObject;
		UIEventListener expr_A1 = UIEventListener.Get(this.mBox.gameObject);
		expr_A1.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A1.onClick, new UIEventListener.VoidDelegate(this.OnBoxClick));
		this.mBoxEffect = this.mBox.transform.FindChild("ui30").gameObject;
		Tools.SetParticleRenderQueue2(this.mBoxEffect, 3006);
		NGUITools.SetActive(this.mBoxEffect, false);
		this.mStartLb = this.mBox.transform.FindChild("star/Label").gameObject.GetComponent<UILabel>();
		this.mLeftBtn = transform3.Find("leftBtn").gameObject;
		UIEventListener expr_144 = UIEventListener.Get(this.mLeftBtn);
		expr_144.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_144.onClick, new UIEventListener.VoidDelegate(this.OnLeftBtnClick));
		this.mLeftBtn.SetActive(false);
		this.mRightBtn = transform3.Find("rightBtn").gameObject;
		UIEventListener expr_192 = UIEventListener.Get(this.mRightBtn);
		expr_192.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_192.onClick, new UIEventListener.VoidDelegate(this.OnRightBtnClick));
		this.mRightBtn.SetActive(false);
		this.mPageLeftBtn = transform3.Find("leftBtnPage").gameObject;
		UIEventListener expr_1E0 = UIEventListener.Get(this.mPageLeftBtn);
		expr_1E0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1E0.onClick, new UIEventListener.VoidDelegate(this.OnPageLeftBtnClick));
		this.mPageRightBtn = transform3.Find("rightBtnPage").gameObject;
		UIEventListener expr_222 = UIEventListener.Get(this.mPageRightBtn);
		expr_222.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_222.onClick, new UIEventListener.VoidDelegate(this.OnPageRightBtnClick));
		this.mTabBtnScrollBar = transform2.transform.Find("scrollBar").GetComponent<UIScrollBar>();
		EventDelegate.Add(this.mTabBtnScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBarValueChange));
		this.mGUIAwakePageItemTable = transform2.Find("pagePanel/contents").gameObject.AddComponent<GUIAwakePageItemTable>();
		this.mGUIAwakePageItemTable.maxPerLine = 32;
		this.mGUIAwakePageItemTable.arrangement = UICustomGrid.Arrangement.Horizontal;
		this.mGUIAwakePageItemTable.cellWidth = 135f;
		this.mGUIAwakePageItemTable.cellHeight = 60f;
		this.mGUIAwakePageItemTable.InitWithBaseScene(this);
		this.mGUIAwakePageItemTable.scrollBar = this.mTabBtnScrollBar;
		this.mLvlBgGo = transform2.Find("infoBg");
		for (int i = 0; i < 5; i++)
		{
			this.mSceneNodes[i] = this.mLvlBgGo.Find(i.ToString()).gameObject.AddComponent<GUIAwakeRoadSceneNode>();
			this.mSceneNodes[i].InitWithBaseScene();
			this.mSceneNodes[i].gameObject.SetActive(false);
		}
		this.instruction = this.mLvlBgGo.Find("instruction").gameObject;
		this.mGUIAwakeQiaoYuNode = this.mLvlBgGo.Find("qiaoYu").gameObject.AddComponent<GUIAwakeQiaoYuNode>();
		this.mGUIAwakeQiaoYuNode.InitWithBaseScene();
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("bg/bg_002", true);
		this.CreateObjects();
		this.InitPageItems();
		base.StartCoroutine(this.InitPageIndex());
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("awakeRoad0");
		topGoods.transform.localPosition = new Vector3(topGoods.transform.localPosition.x, topGoods.transform.localPosition.y, topGoods.transform.localPosition.z - 1000f);
		UIPanel component = topGoods.GetComponent<UIPanel>();
		if (component != null)
		{
			component.renderQueue = UIPanel.RenderQueue.StartAt;
			component.startingRenderQueue = 3200;
		}
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic();
		Globals.Instance.CliSession.Register(610, new ClientSession.MsgHandler(this.OnMsgTakeMapReward));
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.uiState.AdventureSceneInfo = null;
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.transform.localPosition = new Vector3(topGoods.transform.localPosition.x, topGoods.transform.localPosition.y, topGoods.transform.localPosition.z + 1000f);
		UIPanel component = topGoods.GetComponent<UIPanel>();
		if (component != null)
		{
			component.renderQueue = UIPanel.RenderQueue.Automatic;
		}
		topGoods.Hide();
		Globals.Instance.CliSession.Unregister(610, new ClientSession.MsgHandler(this.OnMsgTakeMapReward));
	}

	private void InitPageItems()
	{
		this.mGUIAwakePageItemTable.ClearData();
		for (int i = 1; i <= 32; i++)
		{
			this.mGUIAwakePageItemTable.AddData(new GUIAwakePageItemData(i));
		}
	}

	private void OpenMapPopup()
	{
		SceneInfo resultSceneInfo = GameUIManager.mInstance.uiState.ResultSceneInfo;
		if (resultSceneInfo != null && resultSceneInfo.ID / 100000 == 6)
		{
			int num = resultSceneInfo.ID % 100 - 1;
			if (0 <= num && num < 5)
			{
				this.mSceneNodes[resultSceneInfo.ID % 100 - 1].OpenAdventureReadyPanel(false);
			}
			else if (num == 5)
			{
				this.mGUIAwakeQiaoYuNode.OpenAdventureReadyPanel(false);
			}
			GameUIManager.mInstance.uiState.ResultSceneInfo = null;
		}
	}

	[DebuggerHidden]
	private IEnumerator InitPageIndex()
	{
        return null;
        //GUIAwakeRoadSceneV2.<InitPageIndex>c__Iterator8A <InitPageIndex>c__Iterator8A = new GUIAwakeRoadSceneV2.<InitPageIndex>c__Iterator8A();
        //<InitPageIndex>c__Iterator8A.<>f__this = this;
        //return <InitPageIndex>c__Iterator8A;
	}

	private void DoInitPageIndex(SceneInfo sceneInfo)
	{
		int curPageIndex = 1;
		if (sceneInfo != null)
		{
			curPageIndex = sceneInfo.MapID - 3000;
		}
		this.CurPageIndex = curPageIndex;
		this.CheckBottomPageIndex();
	}

	private void OnLeftBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!base.PostLoadGUIDone)
		{
			return;
		}
		if (this.CurPageIndex - 1 > 0)
		{
			HOTween.To(this.mLvlBgGo, 0.25f, new TweenParms().Prop("localPosition", new PlugVector3X(1050f)).OnComplete(()=>
			{
				this.mLvlBgGo.localPosition = new Vector3(-1050f, -44f, 0f);
				this.CurPageIndex--;
                HOTween.To(this.mLvlBgGo, 0.25f, new TweenParms().Prop("localPosition", new PlugVector3X(0f)).OnComplete(() =>
				{
					this.CheckBottomPageIndex();
				}));
			}));
		}
	}

	private bool IsMapAllPassed(MapInfo mapInfo)
	{
		if (mapInfo == null)
		{
			return false;
		}
		int num = mapInfo.ID % 1000;
		for (int i = 0; i < 5; i++)
		{
			int num2 = 600000 + num * 1000 + i + 1;
			if (Globals.Instance.AttDB.SceneDict.GetInfo(num2) != null)
			{
				if (Globals.Instance.Player.GetSceneScore(num2) == 0)
				{
					return false;
				}
			}
		}
		return true;
	}

	private void OnRightBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!base.PostLoadGUIDone)
		{
			return;
		}
		if (this.CurPageIndex + 1 <= 32)
		{
			int num = 3000;
			MapInfo info = Globals.Instance.AttDB.MapDict.GetInfo(this.CurPageIndex + 1 + num);
			if (info != null)
			{
				int num2 = info.ID % 1000;
				int id = 600000 + num2 * 1000 + 1;
				SceneInfo info2 = Globals.Instance.AttDB.SceneDict.GetInfo(id);
				if (info2 == null)
				{
					return;
				}
				if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)info2.MinLevel))
				{
					GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("awakeRoad2", new object[]
					{
						info2.MinLevel
					}), 0f, 0f);
					return;
				}
				if (this.CurPageIndex > 0)
				{
					MapInfo info3 = Globals.Instance.AttDB.MapDict.GetInfo(this.CurPageIndex + num);
					if (info3 != null && !this.IsMapAllPassed(info3))
					{
						GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_5", new object[]
						{
							info3.Name
						}), 0f, 0f);
						return;
					}
				}
                HOTween.To(this.mLvlBgGo, 0.25f, new TweenParms().Prop("localPosition", new PlugVector3X(-1050f)).OnComplete(() =>
				{
					this.mLvlBgGo.localPosition = new Vector3(1050f, -44f, 0f);
					this.CurPageIndex++;
                    HOTween.To(this.mLvlBgGo, 0.25f, new TweenParms().Prop("localPosition", new PlugVector3X(0f)).OnComplete(() =>
					{
						this.CheckBottomPageIndex();
					}));
				}));
			}
		}
	}

	private void CheckBottomPageIndex()
	{
		if (this.CurPageIndex < 7)
		{
			this.mGUIAwakePageItemTable.SetDragAmount(0f, 0f);
		}
		else if (this.CurPageIndex < 13)
		{
			this.mGUIAwakePageItemTable.SetDragAmount(0.24f, 0f);
		}
		else if (this.CurPageIndex < 19)
		{
			this.mGUIAwakePageItemTable.SetDragAmount(0.48f, 0f);
		}
		else if (this.CurPageIndex < 25)
		{
			this.mGUIAwakePageItemTable.SetDragAmount(0.72f, 0f);
		}
		else
		{
			this.mGUIAwakePageItemTable.SetDragAmount(1f, 0f);
		}
	}

	public void TrySetCurPageIndex(int index)
	{
		if (0 < index && index <= 32)
		{
			int num = 3000;
			MapInfo info = Globals.Instance.AttDB.MapDict.GetInfo(index + num);
			if (info != null)
			{
				int num2 = info.ID % 1000;
				int id = 600000 + num2 * 1000 + 1;
				SceneInfo info2 = Globals.Instance.AttDB.SceneDict.GetInfo(id);
				if (info2 == null)
				{
					return;
				}
				if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)info2.MinLevel))
				{
					GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("awakeRoad3", new object[]
					{
						info2.MinLevel
					}), 0f, 0f);
					return;
				}
				if (index - 1 > 0)
				{
					MapInfo info3 = Globals.Instance.AttDB.MapDict.GetInfo(index - 1 + num);
					if (info3 != null && !this.IsMapAllPassed(info3))
					{
						GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WorldMap_5", new object[]
						{
							info3.Name
						}), 0f, 0f);
						return;
					}
				}
				if (index < this.CurPageIndex)
				{
                    HOTween.To(this.mLvlBgGo, 0.25f, new TweenParms().Prop("localPosition", new PlugVector3X(1050f)).OnComplete(() =>
					{
						this.mLvlBgGo.localPosition = new Vector3(-1050f, -44f, 0f);
						this.CurPageIndex = index;
						HOTween.To(this.mLvlBgGo, 0.25f, new TweenParms().Prop("localPosition", new PlugVector3X(0f)));
					}));
				}
				else if (index > this.CurPageIndex)
				{
                    HOTween.To(this.mLvlBgGo, 0.25f, new TweenParms().Prop("localPosition", new PlugVector3X(-1050f)).OnComplete(() =>
					{
						this.mLvlBgGo.localPosition = new Vector3(1050f, -44f, 0f);
						this.CurPageIndex = index;
						HOTween.To(this.mLvlBgGo, 0.25f, new TweenParms().Prop("localPosition", new PlugVector3X(0f)));
					}));
				}
			}
		}
	}

	private void RefreshSceneNode()
	{
		int num = 3000;
		int id = this.CurPageIndex + num;
		MapInfo info = Globals.Instance.AttDB.MapDict.GetInfo(id);
		if (info == null)
		{
			return;
		}
		this.totalScore = 0;
		SceneInfo info2;
		for (int i = 0; i < 5; i++)
		{
			int num2 = 600000 + this.CurPageIndex * 1000 + i + 1;
			info2 = Globals.Instance.AttDB.SceneDict.GetInfo(num2);
			if (info2 != null)
			{
				int sceneScore = Globals.Instance.Player.GetSceneScore(num2);
				this.totalScore += sceneScore;
				this.mSceneNodes[i].Refresh(info2, sceneScore);
				this.mSceneNodes[i].gameObject.SetActive(true);
			}
		}
		int id2 = 600000 + this.CurPageIndex * 1000 + 6;
		info2 = Globals.Instance.AttDB.SceneDict.GetInfo(id2);
		if (info2 != null)
		{
			this.mGUIAwakeQiaoYuNode.Refresh(info2);
		}
		this.mStartLb.text = string.Format("{0}/{1}", this.totalScore, 15);
		this.RefreshBoxEffect(info);
		SceneInfo adventureSceneInfo = GameUIManager.mInstance.uiState.AdventureSceneInfo;
		if (adventureSceneInfo != null)
		{
			GUIAwakeRoadSceneNode gUIAwakeRoadSceneNode = this.mSceneNodes[adventureSceneInfo.ID % 100 - 1];
			if (gUIAwakeRoadSceneNode.mSceneInfo != adventureSceneInfo)
			{
				this.instruction.SetActive(false);
			}
			else
			{
				this.instruction.transform.parent = gUIAwakeRoadSceneNode.mModelPos.transform;
				this.instruction.transform.localPosition = new Vector3(-7f, 45f, -400f);
				this.instruction.SetActive(true);
			}
		}
	}

	public void OnBoxClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameObject prefab = Res.LoadGUI("GUI/GameUIMapReward");
		this.mMapReward = NGUITools.AddChild(GameUIManager.mInstance.uiCamera.gameObject, prefab);
		Vector3 localPosition = this.mMapReward.transform.localPosition;
		localPosition.z = 3000f;
		this.mMapReward.transform.localPosition = localPosition;
		this.mMapReward.AddComponent<GameUIMapReward>().Init(this, this.CurPageIndex + 3000);
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

	private void OnPageLeftBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		HOTween.To(this.mTabBtnScrollBar, 0.2f, new TweenParms().Prop("value", this.mTabBtnScrollBar.value - 0.24f));
	}

	private void OnPageRightBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		HOTween.To(this.mTabBtnScrollBar, 0.2f, new TweenParms().Prop("value", this.mTabBtnScrollBar.value + 0.24f));
	}

	private void OnScrollBarValueChange()
	{
		if (this.mPageLeftBtn.activeInHierarchy)
		{
			if (this.mTabBtnScrollBar.value <= 0.01f)
			{
				this.mPageLeftBtn.SetActive(false);
			}
		}
		else if (this.mTabBtnScrollBar.value > 0.01f)
		{
			this.mPageLeftBtn.SetActive(true);
		}
		if (this.mPageRightBtn.activeInHierarchy)
		{
			if (this.mTabBtnScrollBar.value >= 0.99f)
			{
				this.mPageRightBtn.SetActive(false);
			}
		}
		else if (this.mTabBtnScrollBar.value < 0.99f)
		{
			this.mPageRightBtn.SetActive(true);
		}
	}

	private void OnMsgTakeMapReward(MemoryStream stream)
	{
		MS2C_TakeMapReward mS2C_TakeMapReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeMapReward), stream) as MS2C_TakeMapReward;
		if (mS2C_TakeMapReward.Result == 0 && this.mGUIAwakePageItemTable != null)
		{
			this.mGUIAwakePageItemTable.RefreshTabState();
		}
	}
}
