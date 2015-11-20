using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIPillageTargetList : MonoBehaviour
{
	private Transform mWinBG;

	public UITable mTargetTable;

	private UILabel CurStamina;

	private UILabel ConstStamina;

	private UIButton BtnRefresh;

	private UILabel BtnRefreshText;

	private float BtnRefreshTextTimer;

	private UnityEngine.Object PillageTargetItemPrefab;

	public void Show()
	{
		this.RefreshStamina();
		this.Show(Globals.Instance.Player.PvpSystem.PillageTargets);
	}

	public void RefreshStamina()
	{
		LocalPlayer player = Globals.Instance.Player;
		this.CurStamina.text = player.Data.Stamina.ToString();
		this.ConstStamina.text = GameConst.GetInt32(36).ToString();
		if (player.Data.Stamina < GameConst.GetInt32(36))
		{
			this.ConstStamina.color = Color.red;
		}
		else
		{
			this.ConstStamina.color = Color.white;
		}
	}

	public void Init()
	{
		base.transform.localPosition = new Vector3(0f, 0f, -550f);
		this.mWinBG = base.transform.Find("winBG");
		this.mTargetTable = this.mWinBG.FindChild("bagPanel/bagContents").gameObject.GetComponent<UITable>();
		this.mTargetTable.columns = 1;
		this.mTargetTable.direction = UITable.Direction.Down;
		GameObject gameObject = this.mWinBG.FindChild("closeBtn").gameObject;
		UIEventListener expr_89 = UIEventListener.Get(gameObject);
		expr_89.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_89.onClick, new UIEventListener.VoidDelegate(this.OnCloseTargetList));
		UIEventListener expr_B5 = UIEventListener.Get(base.gameObject);
		expr_B5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B5.onClick, new UIEventListener.VoidDelegate(this.OnCloseTargetList));
		this.CurStamina = this.mWinBG.FindChild("CurStamina/num").GetComponent<UILabel>();
		this.ConstStamina = this.mWinBG.FindChild("Stamina/num").GetComponent<UILabel>();
		this.BtnRefresh = this.mWinBG.FindChild("BtnRefresh").GetComponent<UIButton>();
		UIEventListener expr_137 = UIEventListener.Get(this.BtnRefresh.gameObject);
		expr_137.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_137.onClick, new UIEventListener.VoidDelegate(this.OnBtnRefreshClicked));
		this.BtnRefreshText = this.BtnRefresh.transform.Find("Label").GetComponent<UILabel>();
		this.BtnRefreshText.text = Singleton<StringManager>.Instance.GetString("Pillage17");
		this.BtnRefresh.isEnabled = true;
	}

	private void FixedUpdate()
	{
		this.RefreshBtnRefreshText();
	}

	private void RefreshBtnRefreshText()
	{
		if (!this.BtnRefresh.isEnabled)
		{
			this.BtnRefreshTextTimer -= Time.fixedDeltaTime;
			if (this.BtnRefreshTextTimer < 0f)
			{
				this.BtnRefreshTextTimer = 3.40282347E+38f;
				this.BtnRefreshText.text = Singleton<StringManager>.Instance.GetString("Pillage17");
				this.BtnRefresh.isEnabled = true;
			}
			else
			{
				this.BtnRefreshText.text = string.Format("{0}({1})", Singleton<StringManager>.Instance.GetString("Pillage17"), (int)(this.BtnRefreshTextTimer + 1f));
			}
		}
	}

	private void OnCloseTargetList(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.ClearTargets();
		base.gameObject.SetActive(false);
	}

	private void OnBtnRefreshClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (GameUIManager.mInstance.uiState.PillageItem == null)
		{
			return;
		}
		GUIPillageScene.RequestQueryPillageTarget(GameUIManager.mInstance.uiState.PillageItem);
		this.BtnRefresh.isEnabled = false;
		this.BtnRefreshTextTimer = 5f;
	}

	private PillageTargetItem AddOneTragetItem(RankData _data)
	{
		if (this.PillageTargetItemPrefab == null)
		{
			this.PillageTargetItemPrefab = Res.LoadGUI("GUI/PillageTargetItem");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.PillageTargetItemPrefab);
		gameObject.transform.parent = this.mTargetTable.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		PillageTargetItem pillageTargetItem = gameObject.AddComponent<PillageTargetItem>();
		pillageTargetItem.ShowRankData(_data);
		gameObject.AddComponent<UIDragScrollView>();
		return pillageTargetItem;
	}

	public static int RankDataSortFunc(RankData a, RankData b)
	{
		bool flag = Tools.IsRebot(a.Data.GUID);
		bool flag2 = Tools.IsRebot(b.Data.GUID);
		if (flag && !flag2)
		{
			return 1;
		}
		if (!flag && flag2)
		{
			return -1;
		}
		if (flag && flag2)
		{
			return -a.Value.CompareTo(b.Value);
		}
		return a.Data.Level.CompareTo(b.Data.Level);
	}

	private void Show(List<RankData> Targets)
	{
		this.ClearTargets();
		if (GameUIManager.mInstance.uiState.PillageItem == null)
		{
			return;
		}
		Targets.Sort(new Comparison<RankData>(GUIPillageTargetList.RankDataSortFunc));
		LocalPlayer player = Globals.Instance.Player;
		for (int i = 0; i < Targets.Count; i++)
		{
			PillageTargetItem pillageTargetItem = this.AddOneTragetItem(Targets[i]);
			if (!Tools.IsRebot(Targets[i].Data.GUID) || (i < 2 && GameUIManager.mInstance.uiState.PillageItem.Quality >= 2) || (i < 2 && (long)Targets[i].Data.Level > (long)((ulong)player.Data.Level)))
			{
				pillageTargetItem.Farm = false;
			}
			else
			{
				pillageTargetItem.Farm = true;
			}
		}
		this.mTargetTable.repositionNow = true;
		base.gameObject.SetActive(true);
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	private void ClearTargets()
	{
		Transform transform = this.mTargetTable.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			UnityEngine.Object.Destroy(child.gameObject);
		}
	}
}
