using Att;
using Proto;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public class GUIGuardScene : GameUISession
{
	private class Item : MonoBehaviour
	{
		public delegate void ClickCallBack(int index, int level);

		private int index;

		public GUIGuardScene.Item.ClickCallBack OnClickEvent;

		private bool isOpen = true;

		private UILabel mLevel;

		public void Init(int index, GUIGuardScene.Item.ClickCallBack cb)
		{
			UILabel uILabel = GameUITools.FindUILabel("Name", base.gameObject);
			this.mLevel = GameUITools.FindUILabel("Level", base.gameObject);
			MGInfo info = Globals.Instance.AttDB.MGDict.GetInfo(index * 3 + 1);
			if (info == null)
			{
				global::Debug.LogError(new object[]
				{
					"MGDict get info error , ID : {0} ",
					index * 3 + 1
				});
				return;
			}
			if (Tools.CanPlay(info.MinLevel, true))
			{
				this.mLevel.text = Singleton<StringManager>.Instance.GetString("guard4", new object[]
				{
					info.MinLevel
				});
			}
			else
			{
				this.isOpen = false;
				this.mLevel.text = Singleton<StringManager>.Instance.GetString("guard3", new object[]
				{
					info.MinLevel
				});
				uILabel.color = Tools.GetDisabledTextColor(222);
				this.mLevel.color = Tools.GetDisabledTextColor(222);
				uILabel.effectStyle = UILabel.Effect.None;
				this.mLevel.effectStyle = UILabel.Effect.None;
				uILabel.applyGradient = false;
				this.mLevel.applyGradient = false;
				base.gameObject.GetComponent<UITexture>().color = Color.black;
				UIButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<UIButton>();
				UIButton[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					UIButton uIButton = array[i];
					uIButton.enabled = false;
				}
			}
			this.index = index;
			this.OnClickEvent = cb;
		}

		private void OnClick()
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
			if (this.isOpen)
			{
				if (this.OnClickEvent != null)
				{
					this.OnClickEvent(this.index, -1);
				}
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTip(this.mLevel.text, 0f, 0f);
			}
		}
	}

	private static int targetIndex = -1;

	private static int targetLevel = -1;

	private GUIGuardScene.Item[] mItems = new GUIGuardScene.Item[3];

	private GUIGuardReadyPopUp mReadyPopUp;

	public static void Show(bool showLoading = false)
	{
		GameUIManager.mInstance.ChangeSession<GUIGuardScene>(null, showLoading, true);
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.GetTopGoods().Hide();
		Globals.Instance.CliSession.Unregister(653, new ClientSession.MsgHandler(this.OnMsgMGFarm));
	}

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("bg/bg_002", true);
		GameUIManager.mInstance.GetTopGoods().Show("guard0");
		this.CreateObjects();
		Globals.Instance.CliSession.Register(653, new ClientSession.MsgHandler(this.OnMsgMGFarm));
	}

	protected override void OnLoadedFinished()
	{
	}

	private void CreateObjects()
	{
		UIPanel component = base.gameObject.GetComponent<UIPanel>();
		component.leftAnchor.target = GameUIManager.mInstance.uiRoot.transform;
		component.rightAnchor.target = GameUIManager.mInstance.uiRoot.transform;
		component.topAnchor.target = GameUIManager.mInstance.uiRoot.transform;
		component.bottomAnchor.target = GameUIManager.mInstance.uiRoot.transform;
		GameUITools.FindUILabel("Times/Value", base.gameObject).text = (GameConst.GetInt32(125) - Globals.Instance.Player.Data.MGCount).ToString();
		GameUITools.RegisterClickEvent("RulesBtn", new UIEventListener.VoidDelegate(this.OnRulesClick), base.gameObject);
		this.mReadyPopUp = GameUITools.FindGameObject("ReadyPopUp", base.gameObject).AddComponent<GUIGuardReadyPopUp>();
		this.mReadyPopUp.gameObject.SetActive(true);
		this.mReadyPopUp.Init(this);
		this.mReadyPopUp.Hide();
		Transform transform = GameUITools.FindGameObject("Items", base.gameObject).transform;
		int num = 0;
		while (num < 3 && num < transform.childCount)
		{
			this.mItems[num] = transform.GetChild(num).gameObject.AddComponent<GUIGuardScene.Item>();
			this.mItems[num].Init(num, new GUIGuardScene.Item.ClickCallBack(this.OnItemClick));
			num++;
		}
		if (GUIGuardScene.targetIndex >= 0 && GUIGuardScene.targetLevel >= 0)
		{
			this.OnItemClick(GUIGuardScene.targetIndex, GUIGuardScene.targetLevel);
			GUIGuardScene.targetIndex = -1;
			GUIGuardScene.targetLevel = -1;
		}
	}

	private void OnRulesClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GameUIRuleInfoPopUp.ShowThis("guard0", "guard11");
	}

	private void OnItemClick(int index, int level = -1)
	{
		this.mReadyPopUp.Open(index, level);
	}

	public void SaveTarget(int index, int level)
	{
		GUIGuardScene.targetIndex = index;
		GUIGuardScene.targetLevel = level;
	}

	private void OnMsgMGFarm(MemoryStream stream)
	{
		MS2C_MGFarm mS2C_MGFarm = Serializer.NonGeneric.Deserialize(typeof(MS2C_MGFarm), stream) as MS2C_MGFarm;
		if (mS2C_MGFarm.Result == 51)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_MGFarm.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_MGFarm.Result);
			return;
		}
		GUIGuardResultScene.ShowResult(mS2C_MGFarm.Data);
	}
}
