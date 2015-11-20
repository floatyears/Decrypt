using Att;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrinketRecipeItem : MonoBehaviour
{
	private static int[] ALWAYS_SHOW_ID = new int[]
	{
		1031,
		1032,
		1131,
		1132
	};

	public UIEventListener.VoidDelegate SelectedEvent;

	private UISprite ItemIcon;

	private UISprite ItemQuality;

	private UISprite NewFlag;

	private UISprite selectFlag;

	public List<ItemInfo> ItemInfos = new List<ItemInfo>();

	public RecipeInfo RecipeInfo
	{
		get;
		private set;
	}

	public ItemInfo RecipeItemInfo
	{
		get;
		private set;
	}

	public bool Selected
	{
		get
		{
			return !string.IsNullOrEmpty(this.selectFlag.spriteName);
		}
		set
		{
			this.selectFlag.spriteName = ((!value) ? string.Empty : "iconSelect");
			this.selectFlag.alpha = ((!value) ? 0f : 1f);
		}
	}

	public bool isVisible
	{
		get
		{
			return base.gameObject.activeSelf;
		}
		set
		{
			base.gameObject.SetActive(value);
		}
	}

	public bool isCanComposite
	{
		get;
		private set;
	}

	private static bool IS_ALWAYS_SHOW(int id)
	{
		for (int i = 0; i < TrinketRecipeItem.ALWAYS_SHOW_ID.Length; i++)
		{
			if (id == TrinketRecipeItem.ALWAYS_SHOW_ID[i])
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsVisible(RecipeInfo Info)
	{
		if (TrinketRecipeItem.IS_ALWAYS_SHOW(Info.ID))
		{
			return true;
		}
		ItemSubSystem itemSystem = Globals.Instance.Player.ItemSystem;
		for (int i = 0; i < Info.ItemID.Count; i++)
		{
			if (Info.ItemID[i] != 0)
			{
				ItemDataEx itemByInfoID = itemSystem.GetItemByInfoID(Info.ItemID[i]);
				if (itemByInfoID != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static int SortByName(TrinketRecipeItem a, TrinketRecipeItem b)
	{
		return string.Compare(a.gameObject.name, b.gameObject.name);
	}

	public void Init(RecipeInfo Info)
	{
		this.RecipeInfo = Info;
		this.RecipeItemInfo = Globals.Instance.AttDB.ItemDict.GetInfo(this.RecipeInfo.ID);
		if (this.RecipeInfo == null || this.RecipeItemInfo == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.ItemInfos.Clear();
		for (int i = 0; i < this.RecipeInfo.ItemID.Count; i++)
		{
			if (this.RecipeInfo.ItemID[i] != 0)
			{
				ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(this.RecipeInfo.ItemID[i]);
				if (info == null)
				{
					global::Debug.LogErrorFormat("Can not find trinket fragment item info {0}", new object[]
					{
						this.RecipeInfo.ItemID[i]
					});
				}
				else
				{
					this.ItemInfos.Add(info);
				}
			}
		}
		if (this.RecipeItemInfo.Type == 4 && this.RecipeItemInfo.SubType == 9)
		{
			base.gameObject.name = string.Format("A{0}_{1}", 10 - this.RecipeItemInfo.Quality, this.RecipeInfo.ID);
		}
		else
		{
			base.gameObject.name = string.Format("R{0}_{1}", 10 - this.ItemInfos.Count, this.RecipeInfo.ID);
		}
		this.ItemIcon.spriteName = this.RecipeItemInfo.Icon;
		this.ItemQuality.spriteName = Tools.GetItemQualityIcon(this.RecipeItemInfo.Quality);
		this.RefreshVisible();
	}

	public void RefreshVisible()
	{
		if (this.RecipeInfo == null || this.RecipeItemInfo == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.ItemInfos.Count == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		bool flag = TrinketRecipeItem.IS_ALWAYS_SHOW(this.RecipeInfo.ID);
		bool flag2 = true;
		ItemSubSystem itemSystem = Globals.Instance.Player.ItemSystem;
		for (int i = 0; i < this.ItemInfos.Count; i++)
		{
			ItemDataEx itemByInfoID = itemSystem.GetItemByInfoID(this.ItemInfos[i].ID);
			if (itemByInfoID != null)
			{
				flag = true;
			}
			else
			{
				flag2 = false;
			}
			if (flag && !flag2)
			{
				break;
			}
		}
		base.gameObject.SetActive(flag);
		this.NewFlag.gameObject.SetActive(flag2);
		this.isCanComposite = flag2;
	}

	private void Awake()
	{
		this.ItemIcon = base.transform.GetComponent<UISprite>();
		UIEventListener expr_1C = UIEventListener.Get(base.gameObject);
		expr_1C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C.onClick, new UIEventListener.VoidDelegate(this.OnClicked));
		this.ItemQuality = base.transform.Find("QualityMark").GetComponent<UISprite>();
		this.NewFlag = base.transform.Find("new").GetComponent<UISprite>();
		this.selectFlag = base.transform.Find("selected").GetComponent<UISprite>();
		this.Selected = false;
	}

	private void OnClicked(GameObject go)
	{
		if (this.SelectedEvent != null)
		{
			this.SelectedEvent(go);
		}
	}
}
