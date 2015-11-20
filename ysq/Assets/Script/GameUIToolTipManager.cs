using Att;
using System;
using System.Text;
using UnityEngine;

public sealed class GameUIToolTipManager
{
	private static GameUIToolTipManager mInstance;

	private static GameUIToolTip ItemToolTip;

	private static GameUIToolTip ToolTip;

	private static GameUIToolTip SignInTip;

	private StringBuilder mStringBuilder = new StringBuilder();

	public static GameUIToolTipManager GetInstance()
	{
		if (GameUIToolTipManager.mInstance == null)
		{
			GameUIToolTipManager.mInstance = new GameUIToolTipManager();
		}
		return GameUIToolTipManager.mInstance;
	}

	private GameUIToolTip LoadToolTipPerfab(UnityEngine.Object prefab, Transform parent, string goName)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(prefab);
		Transform transform = gameObject.transform;
		transform.parent = parent;
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 3000f);
		transform.localScale = Vector3.one;
		GameUIToolTip gameUIToolTip = gameObject.AddComponent<GameUIToolTip>();
		UIPanel uIPanel = gameObject.AddComponent<UIPanel>();
		uIPanel.depth = 3000;
		uIPanel.renderQueue = UIPanel.RenderQueue.StartAt;
		uIPanel.startingRenderQueue = 6600;
		gameUIToolTip.gameObject.name = goName;
		gameUIToolTip.gameObject.SetActive(false);
		gameUIToolTip.transform.localScale = Vector3.one;
		return gameUIToolTip;
	}

	private GameUIToolTip LoadItemToolTipPerfab(Transform parent, string goName)
	{
		if (GameUIToolTipManager.ItemToolTip == null)
		{
			UnityEngine.Object prefab = Res.LoadGUI("GUI/GameUIToolTip");
			GameUIToolTipManager.ItemToolTip = this.LoadToolTipPerfab(prefab, parent, goName);
		}
		else
		{
			GameUIToolTipManager.ItemToolTip.transform.parent = parent;
			GameUIToolTipManager.ItemToolTip.name = goName;
			GameUIToolTipManager.ItemToolTip.transform.localScale = Vector3.one;
		}
		return GameUIToolTipManager.ItemToolTip;
	}

	private GameUIToolTip LoadToolTipPerfab(Transform parent, string goName)
	{
		if (GameUIToolTipManager.ToolTip == null)
		{
			UnityEngine.Object prefab = Res.LoadGUI("GUI/GameUIPetTooltip");
			GameUIToolTipManager.ToolTip = this.LoadToolTipPerfab(prefab, parent, goName);
		}
		else
		{
			GameUIToolTipManager.ToolTip.transform.parent = parent;
			GameUIToolTipManager.ToolTip.name = goName;
			GameUIToolTipManager.ToolTip.transform.localScale = Vector3.one;
		}
		return GameUIToolTipManager.ToolTip;
	}

	private GameUIToolTip LoadSignInRewardToolTipPerfab(Transform parent, string goName)
	{
		if (GameUIToolTipManager.SignInTip == null)
		{
			UnityEngine.Object prefab = Res.LoadGUI("GUI/GameUISignInItemToolTip");
			GameUIToolTipManager.SignInTip = this.LoadToolTipPerfab(prefab, parent, goName);
		}
		else
		{
			GameUIToolTipManager.SignInTip.transform.parent = parent;
			GameUIToolTipManager.SignInTip.name = goName;
			GameUIToolTipManager.SignInTip.transform.localScale = Vector3.one;
		}
		return GameUIToolTipManager.SignInTip;
	}

	public GameUIToolTip CreateBasicTooltip(Transform parent, string title, string desc)
	{
		Tools.Assert(parent, "Invalid parent");
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
		this.mStringBuilder.Append("Tooltip_").Append(parent.name);
		GameUIToolTip gameUIToolTip = this.LoadItemToolTipPerfab(parent, this.mStringBuilder.ToString());
		gameUIToolTip.Create(parent, title, desc);
		return gameUIToolTip;
	}

	public GameUIToolTip CreateSignInRewardTooltip(Transform parent, string title, string desc)
	{
		Tools.Assert(parent, "Invalid parent");
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
		this.mStringBuilder.Append("Tooltip_").Append(parent.name);
		return this.LoadSignInRewardToolTipPerfab(parent, this.mStringBuilder.ToString());
	}

	public GameUIToolTip CreateSkillTooltip(Transform parent, SkillInfo skillInfo)
	{
		Tools.Assert(parent, "Invalid parent");
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
		this.mStringBuilder.Append("Tooltip_").Append(skillInfo.ID);
		return this.LoadItemToolTipPerfab(parent, this.mStringBuilder.ToString());
	}

	public GameUIToolTip CreatePetTooltip(Transform parent, PetInfo petInfo)
	{
		Tools.Assert(parent, "Invalid parent");
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
		this.mStringBuilder.Append("Tooltip_").Append(petInfo.ID);
		return this.LoadToolTipPerfab(parent, this.mStringBuilder.ToString());
	}

	public GameUIToolTip CreateMonsterTooltip(Transform parent, MonsterInfo petInfo)
	{
		Tools.Assert(parent, "Invalid parent");
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
		this.mStringBuilder.Append("Tooltip_").Append(petInfo.ID);
		return this.LoadToolTipPerfab(parent, this.mStringBuilder.ToString());
	}
}
