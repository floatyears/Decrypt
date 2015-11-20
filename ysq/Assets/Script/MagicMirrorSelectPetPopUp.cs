using Att;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class MagicMirrorSelectPetPopUp : MonoBehaviour
{
	private class SelectPetUIGrid : CommonBagUITable
	{
		private GUIMagicMirrorScene mBaseScene;

		public void Init(GUIMagicMirrorScene basescene)
		{
			this.mBaseScene = basescene;
		}

		protected override int Sort(BaseData a, BaseData b)
		{
			PetDataEx petDataEx = (PetDataEx)a;
			PetDataEx petDataEx2 = (PetDataEx)b;
			if (petDataEx != null && petDataEx2 != null)
			{
				if (this.mBaseScene.setIn)
				{
					bool flag = petDataEx.IsBattling();
					bool flag2 = petDataEx2.IsBattling();
					if (flag && !flag2)
					{
						return 1;
					}
					if (!flag && flag2)
					{
						return -1;
					}
					flag = petDataEx.IsPetAssisting();
					flag2 = petDataEx2.IsPetAssisting();
					if (flag && !flag2)
					{
						return 1;
					}
					if (!flag && flag2)
					{
						return -1;
					}
				}
				else
				{
					bool flag3 = Tools.HasNewPet(petDataEx.Info.ID);
					bool flag4 = Tools.HasNewPet(petDataEx2.Info.ID);
					if (flag3 && !flag4)
					{
						return -1;
					}
					if (!flag3 && flag4)
					{
						return 1; 
					}
				}
				if (petDataEx.Data.Further > petDataEx2.Data.Further)
				{
					return -1;
				}
				if (petDataEx.Data.Further < petDataEx2.Data.Further)
				{
					return 1;
				}
				if (petDataEx.Data.Awake > petDataEx2.Data.Awake)
				{
					return -1;
				}
				if (petDataEx.Data.Awake < petDataEx2.Data.Awake)
				{
					return 1;
				}
				if (petDataEx.Data.Level > petDataEx2.Data.Level)
				{
					return -1;
				}
				if (petDataEx.Data.Level < petDataEx2.Data.Level)
				{
					return 1;
				}
				if (petDataEx.Info.Quality > petDataEx2.Info.Quality)
				{
					return -1;
				}
				if (petDataEx.Info.Quality < petDataEx2.Info.Quality)
				{
					return 1;
				}
				if (petDataEx.Info.SubQuality > petDataEx2.Info.SubQuality)
				{
					return -1;
				}
				if (petDataEx.Info.SubQuality < petDataEx2.Info.SubQuality)
				{
					return 1;
				}
				if (petDataEx.Info.ID < petDataEx2.Info.ID)
				{
					return -1;
				}
				if (petDataEx.Info.ID > petDataEx2.Info.ID)
				{
					return 1;
				}
			}
			return 0;
		}
	}

	private GUIMagicMirrorScene mBaseScene;

	private GameObject mWindow;

	private MagicMirrorSelectPetPopUp.SelectPetUIGrid mContent;

	private UILabel mTips;

	private UIToggle[] mTabs;

	private UISprite[] mTabSprites;

	private PetDataEx mData;

	public void Init(GUIMagicMirrorScene basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		GameUITools.RegisterClickEvent("FadeBG", new UIEventListener.VoidDelegate(this.OnCloseClick), base.gameObject);
		this.mWindow = GameUITools.FindGameObject("Window", base.gameObject);
		GameUITools.RegisterClickEvent("Panel2/CloseBtn", new UIEventListener.VoidDelegate(this.OnCloseClick), this.mWindow);
		this.mContent = GameUITools.FindGameObject("Panel/Content", this.mWindow).AddComponent<MagicMirrorSelectPetPopUp.SelectPetUIGrid>();
		this.mContent.maxPerLine = 2;
		this.mContent.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mContent.cellWidth = 442f;
		this.mContent.cellHeight = 106f;
		this.mContent.gapHeight = 8f;
		this.mContent.gapWidth = 8f;
		this.mContent.InitWithBaseScene(this.mBaseScene, "GUIMagicMirrorSelectPetItem");
		this.mContent.Init(this.mBaseScene);
		this.mTips = GameUITools.FindUILabel("Tips", this.mWindow);
		this.mTabs = new UIToggle[4];
		this.mTabSprites = new UISprite[4];
		Transform transform = GameUITools.FindGameObject("Tabs", this.mWindow).transform;
		int num = 0;
		while (num < transform.childCount && num < this.mTabs.Length)
		{
			this.mTabs[num] = transform.GetChild(num).GetComponent<UIToggle>();
			this.mTabSprites[num] = this.mTabs[num].GetComponent<UISprite>();
			EventDelegate.Add(this.mTabs[num].onChange, new EventDelegate.Callback(this.OnTabCheckChanged));
			UIEventListener expr_18F = UIEventListener.Get(this.mTabs[num].gameObject);
			expr_18F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_18F.onClick, new UIEventListener.VoidDelegate(this.OnTabClick));
			num++;
		}
	}

	public void OnTabCheckChanged()
	{
		if (UIToggle.current.value)
		{
			int type = Convert.ToInt32(UIToggle.current.gameObject.name);
			this.InitData(type);
		}
	}

	private void InitData(int type)
	{
		if (this.mData == null)
		{
			this.mContent.ClearData();
			this.mContent.SetDragAmount(0f, 0f);
			foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
			{
				if (!current.IsPlayerBattling())
				{
					if (type == 1 || type == 3)
					{
						if ((current.Info.Type == 1 || current.Info.Type == 3) && current.Info.Quality >= 3 && current.IsOld())
						{
							this.mContent.AddData(current);
						}
					}
					else if (current.Info.Type == type && current.Info.Quality >= 3 && current.IsOld())
					{
						this.mContent.AddData(current);
					}
				}
			}
		}
		else
		{
			this.mContent.ClearData();
			this.mContent.SetDragAmount(0f, 0f);
			foreach (PetInfo current2 in Globals.Instance.AttDB.PetDict.Values)
			{
				if (current2.ID != 90000 && current2.ID != 90001 && !current2.ShowCollection)
				{
					if (type == 1 || type == 3)
					{
						if ((current2.Type == 1 || current2.Type == 3) && current2.Quality >= 3 && current2.ID != this.mData.Info.ID)
						{
							PetDataEx data = new PetDataEx(this.mData.Data, current2);
							this.mContent.AddData(data);
						}
					}
					else if (current2.Type == this.mData.Info.Type && current2.Quality >= 3 && current2.ID != this.mData.Info.ID)
					{
						PetDataEx data = new PetDataEx(this.mData.Data, current2);
						this.mContent.AddData(data);
					}
				}
			}
		}
	}

	private void OnTabClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (this.mData == null || Convert.ToInt32(go.name) != this.mData.Info.Type)
		{
		}
	}

	public void Open(PetDataEx petData)
	{
		base.gameObject.SetActive(true);
		this.mData = petData;
		if (this.mData == null)
		{
			this.mTips.text = Singleton<StringManager>.Instance.GetString("Mirror0");
			for (int i = 0; i < this.mTabs.Length; i++)
			{
				if (this.mTabs[i] != null)
				{
					this.mTabs[i].enabled = true;
				}
			}
			int num = -1;
			foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
			{
				if (!current.IsPlayerBattling())
				{
					if (current.Info.Quality >= 3 && current.IsOld() && !current.IsPetAssisting() && !current.IsBattling() && (current.Info.Type < num || num < 0))
					{
						num = current.Info.Type;
					}
				}
			}
			if (num < 0)
			{
				num = 1;
			}
			else if (num == 3)
			{
				num = 1;
			}
			if (this.mTabs[num - 1] != null)
			{
				if (this.mTabs[num - 1].value)
				{
					this.InitData(num);
				}
				else
				{
					this.mTabs[num - 1].value = true;
				}
			}
			for (int j = 0; j < this.mTabSprites.Length; j++)
			{
				if (this.mTabSprites[j] != null)
				{
					this.mTabSprites[j].color = Color.white;
				}
			}
		}
		else
		{
			if (this.mData.Info.Type > this.mTabs.Length || this.mData.Info.Type < 0)
			{
				return;
			}
			if (this.mData.Data.Further > 0u)
			{
				this.mTips.text = Singleton<StringManager>.Instance.GetString("Mirror1", new object[]
				{
					Tools.GetItemQualityColorHex(this.mData.Info.Quality, Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
					{
						Tools.GetPetName(this.mData.Info),
						this.mData.Data.Further
					}))
				});
			}
			else
			{
				this.mTips.text = Singleton<StringManager>.Instance.GetString("Mirror1", new object[]
				{
					Tools.GetItemQualityColorHex(this.mData.Info.Quality, Tools.GetPetName(this.mData.Info))
				});
			}
			int num2 = this.mData.Info.Type;
			if (num2 == 3)
			{
				num2 = 1;
			}
			if (this.mTabs[num2 - 1] != null)
			{
				if (this.mTabs[num2 - 1].value)
				{
					this.InitData(num2);
				}
				else
				{
					this.mTabs[num2 - 1].value = true;
				}
			}
			for (int k = 0; k < this.mTabs.Length; k++)
			{
				if (this.mTabs[k] != null)
				{
					this.mTabs[k].enabled = false;
				}
			}
			for (int l = 0; l < this.mTabSprites.Length; l++)
			{
				if (l == num2 - 1)
				{
					if (this.mTabSprites[l] != null)
					{
						this.mTabSprites[l].color = Color.white;
					}
				}
				else if (this.mTabSprites[l] != null)
				{
					this.mTabSprites[l].color = Color.black;
				}
			}
		}
		GameUITools.PlayOpenWindowAnim(this.mWindow.transform, null, true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public void Close()
	{
		GameUITools.PlayCloseWindowAnim(this.mWindow.transform, new TweenDelegate.TweenCallback(this.Hide), true);
	}

	private void OnCloseClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		this.Close();
	}
}
