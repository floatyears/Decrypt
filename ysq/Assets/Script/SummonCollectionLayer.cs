using Att;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public sealed class SummonCollectionLayer : MonoBehaviour
{
	private GUISummonCollectionScene mBaseScene;

	private UICollectionBetterTable mCollectionTable2;

	private UIScrollBar mScrollBar;

	public UICollectionScrollView mSW;

	private UILabel mCollectionTipTxt;

	public List<PetInfo> mFirePetInfos = new List<PetInfo>();

	public List<PetInfo> mWoodPetInfos = new List<PetInfo>();

	public List<PetInfo> mWaterPetInfos = new List<PetInfo>();

	public List<PetInfo> mLightPetInfos = new List<PetInfo>();

	public List<PetInfo> mDarkPetInfos = new List<PetInfo>();

	private UnityEngine.Object mTitleOriginal;

	private UnityEngine.Object mContentOriginal;

	private UIToggle mAllCheck;

	private int mBasePriority;

	public GameObject mbagHideGo;

	public List<Transform> mContents = new List<Transform>();

	private int SortSummonCollection(PetInfo a, PetInfo b)
	{
		if (a == null || b == null)
		{
			return 0;
		}
		if (a.Quality > b.Quality)
		{
			return -1;
		}
		if (a.Quality < b.Quality)
		{
			return 1;
		}
		if (a.ID > b.ID)
		{
			return -1;
		}
		if (a.ID < b.ID)
		{
			return 1;
		}
		return 0;
	}

	public void SortInfos()
	{
		this.mFirePetInfos.Sort(new Comparison<PetInfo>(this.SortSummonCollection));
		this.mWoodPetInfos.Sort(new Comparison<PetInfo>(this.SortSummonCollection));
		this.mWaterPetInfos.Sort(new Comparison<PetInfo>(this.SortSummonCollection));
		this.mLightPetInfos.Sort(new Comparison<PetInfo>(this.SortSummonCollection));
		this.mDarkPetInfos.Sort(new Comparison<PetInfo>(this.SortSummonCollection));
	}

	private int GetPetsTotalNum()
	{
		return this.mFirePetInfos.Count + this.mWoodPetInfos.Count + this.mWaterPetInfos.Count + this.mLightPetInfos.Count + this.mDarkPetInfos.Count;
	}

	private int GetHasPetsTotalNum()
	{
		int num = 0;
		for (int i = 1; i < 6; i++)
		{
			num += this.GetHasPetsNum((EElementType)i);
		}
		return num;
	}

	private int GetTypePetsTotalNum(EElementType tp)
	{
		int result = 0;
		switch (tp)
		{
		case EElementType.EET_Fire:
			result = this.mFirePetInfos.Count;
			break;
		case EElementType.EET_Wood:
			result = this.mWoodPetInfos.Count;
			break;
		case EElementType.EET_Water:
			result = this.mWaterPetInfos.Count;
			break;
		case EElementType.EET_Light:
			result = this.mLightPetInfos.Count;
			break;
		case EElementType.EET_Dark:
			result = this.mDarkPetInfos.Count;
			break;
		}
		return result;
	}

	private int GetHasPetsNum(EElementType tp)
	{
		int num = 0;
		switch (tp)
		{
		case EElementType.EET_Fire:
			for (int i = 0; i < this.mFirePetInfos.Count; i++)
			{
				if (Globals.Instance.Player.PetSystem.GetPetByInfoID(this.mFirePetInfos[i].ID) != null)
				{
					num++;
				}
			}
			break;
		case EElementType.EET_Wood:
			for (int j = 0; j < this.mWoodPetInfos.Count; j++)
			{
				if (Globals.Instance.Player.PetSystem.GetPetByInfoID(this.mWoodPetInfos[j].ID) != null)
				{
					num++;
				}
			}
			break;
		case EElementType.EET_Water:
			for (int k = 0; k < this.mWaterPetInfos.Count; k++)
			{
				if (Globals.Instance.Player.PetSystem.GetPetByInfoID(this.mWaterPetInfos[k].ID) != null)
				{
					num++;
				}
			}
			break;
		case EElementType.EET_Light:
			for (int l = 0; l < this.mLightPetInfos.Count; l++)
			{
				if (Globals.Instance.Player.PetSystem.GetPetByInfoID(this.mLightPetInfos[l].ID) != null)
				{
					num++;
				}
			}
			break;
		case EElementType.EET_Dark:
			for (int m = 0; m < this.mDarkPetInfos.Count; m++)
			{
				if (Globals.Instance.Player.PetSystem.GetPetByInfoID(this.mDarkPetInfos[m].ID) != null)
				{
					num++;
				}
			}
			break;
		}
		return num;
	}

	public void InitWithBaseScene(GUISummonCollectionScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mbagHideGo = base.transform.Find("summonBag/bagHide").gameObject;
		this.mScrollBar = base.transform.Find("summonBag/bgPanelScrollBar").GetComponent<UIScrollBar>();
		this.mSW = base.transform.Find("summonBag/bagPanel").gameObject.AddComponent<UICollectionScrollView>();
		this.mSW.contentPivot = UIWidget.Pivot.TopLeft;
		this.mSW.movement = UIScrollView.Movement.Vertical;
		this.mSW.dragEffect = UIScrollView.DragEffect.MomentumAndSpring;
		this.mSW.scrollWheelFactor = 1f;
		this.mSW.momentumAmount = 35f;
		this.mSW.restrictWithinPanel = true;
		this.mSW.disableDragIfFits = true;
		this.mSW.smoothDragStart = true;
		this.mSW.iOSDragEmulation = true;
		this.mSW.verticalScrollBar = this.mScrollBar;
		this.mSW.showScrollBars = UIScrollView.ShowCondition.OnlyIfNeeded;
		this.mCollectionTable2 = base.transform.Find("summonBag/bagPanel/bagContents").gameObject.AddComponent<UICollectionBetterTable>();
		this.mCollectionTable2.InitWithBaseLayer(this);
		this.mAllCheck = base.transform.Find("allBtn").GetComponent<UIToggle>();
		EventDelegate.Add(this.mAllCheck.onChange, new EventDelegate.Callback(this.OnFilterChanged));
		UIToggle component = base.transform.Find("fireBtn").GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnFilterChanged));
		UIToggle component2 = base.transform.Find("waterBtn").GetComponent<UIToggle>();
		EventDelegate.Add(component2.onChange, new EventDelegate.Callback(this.OnFilterChanged));
		UIToggle component3 = base.transform.Find("woodBtn").GetComponent<UIToggle>();
		EventDelegate.Add(component3.onChange, new EventDelegate.Callback(this.OnFilterChanged));
		UIToggle component4 = base.transform.Find("lightBtn").GetComponent<UIToggle>();
		EventDelegate.Add(component4.onChange, new EventDelegate.Callback(this.OnFilterChanged));
		UIToggle component5 = base.transform.Find("darkBtn").GetComponent<UIToggle>();
		EventDelegate.Add(component5.onChange, new EventDelegate.Callback(this.OnFilterChanged));
		this.mCollectionTipTxt = base.transform.Find("tipTxt").GetComponent<UILabel>();
	}

	private void AddContents(int tp)
	{
		switch (tp)
		{
		case 1:
		{
			int num = -1;
			PetInfo[] array = null;
			for (int i = 0; i < this.mFirePetInfos.Count; i++)
			{
				if (i / 8 != num)
				{
					num = i / 8;
					array = new PetInfo[8];
					array[0] = this.mFirePetInfos[i];
				}
				else
				{
					array[i % 8] = this.mFirePetInfos[i];
				}
				if (i % 8 == 7 || i == this.mFirePetInfos.Count - 1)
				{
					this.AddCollectionContent((EElementType)tp, array);
				}
			}
			break;
		}
		case 2:
		{
			int num2 = -1;
			PetInfo[] array2 = null;
			for (int j = 0; j < this.mWoodPetInfos.Count; j++)
			{
				if (j / 8 != num2)
				{
					num2 = j / 8;
					array2 = new PetInfo[8];
					array2[0] = this.mWoodPetInfos[j];
				}
				else
				{
					array2[j % 8] = this.mWoodPetInfos[j];
				}
				if (j % 8 == 7 || j == this.mWoodPetInfos.Count - 1)
				{
					this.AddCollectionContent((EElementType)tp, array2);
				}
			}
			break;
		}
		case 3:
		{
			int num3 = -1;
			PetInfo[] array3 = null;
			for (int k = 0; k < this.mWaterPetInfos.Count; k++)
			{
				if (k / 8 != num3)
				{
					num3 = k / 8;
					array3 = new PetInfo[8];
					array3[0] = this.mWaterPetInfos[k];
				}
				else
				{
					array3[k % 8] = this.mWaterPetInfos[k];
				}
				if (k % 8 == 7 || k == this.mWaterPetInfos.Count - 1)
				{
					this.AddCollectionContent((EElementType)tp, array3);
				}
			}
			break;
		}
		case 4:
		{
			int num4 = -1;
			PetInfo[] array4 = null;
			for (int l = 0; l < this.mLightPetInfos.Count; l++)
			{
				if (l / 8 != num4)
				{
					num4 = l / 8;
					array4 = new PetInfo[8];
					array4[0] = this.mLightPetInfos[l];
				}
				else
				{
					array4[l % 8] = this.mLightPetInfos[l];
				}
				if (l % 8 == 7 || l == this.mLightPetInfos.Count - 1)
				{
					this.AddCollectionContent((EElementType)tp, array4);
				}
			}
			break;
		}
		case 5:
		{
			int num5 = -1;
			PetInfo[] array5 = null;
			for (int m = 0; m < this.mDarkPetInfos.Count; m++)
			{
				if (m / 8 != num5)
				{
					num5 = m / 8;
					array5 = new PetInfo[8];
					array5[0] = this.mDarkPetInfos[m];
				}
				else
				{
					array5[m % 8] = this.mDarkPetInfos[m];
				}
				if (m % 8 == 7 || m == this.mDarkPetInfos.Count - 1)
				{
					this.AddCollectionContent((EElementType)tp, array5);
				}
			}
			break;
		}
		}
	}

	private void UpdateScrollBar()
	{
		this.mSW.UpdateScrollbars();
		this.mScrollBar.value = 0.001f;
	}

	public void InitCollectionItems()
	{
		if (this.GetTypePetsTotalNum(EElementType.EET_Fire) != 0)
		{
			this.AddCollectionTitle(EElementType.EET_Fire);
			this.AddContents(1);
		}
		if (this.GetTypePetsTotalNum(EElementType.EET_Water) != 0)
		{
			this.AddCollectionTitle(EElementType.EET_Water);
			this.AddContents(3);
		}
		if (this.GetTypePetsTotalNum(EElementType.EET_Wood) != 0)
		{
			this.AddCollectionTitle(EElementType.EET_Wood);
			this.AddContents(2);
		}
		if (this.GetTypePetsTotalNum(EElementType.EET_Light) != 0)
		{
			this.AddCollectionTitle(EElementType.EET_Light);
			this.AddContents(4);
		}
		if (this.GetTypePetsTotalNum(EElementType.EET_Dark) != 0)
		{
			this.AddCollectionTitle(EElementType.EET_Dark);
			this.AddContents(5);
		}
		this.mTitleOriginal = null;
		this.mContentOriginal = null;
		StringBuilder stringBuilder = new StringBuilder();
		this.mCollectionTipTxt.text = stringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("summonCollectionTxt2"), this.GetHasPetsTotalNum(), this.GetPetsTotalNum()).ToString();
		this.mCollectionTable2.SortSelf();
		this.mAllCheck.value = true;
		this.UpdateScrollBar();
	}

	private void AddCollectionTitle(EElementType tp)
	{
		if (this.mTitleOriginal == null)
		{
			this.mTitleOriginal = Res.LoadGUI("GUI/collectionTitle");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.mTitleOriginal);
		gameObject.name = this.mTitleOriginal.name;
		gameObject.transform.parent = this.mCollectionTable2.gameObject.transform;
		this.mContents.Add(gameObject.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		CollectionTitle collectionTitle = gameObject.AddComponent<CollectionTitle>();
		collectionTitle.mPriority = this.mBasePriority++;
		int hasPetsNum = this.GetHasPetsNum(tp);
		int typePetsTotalNum = this.GetTypePetsTotalNum(tp);
		collectionTitle.InitWithBaseScene(this.mBaseScene, tp, hasPetsNum, typePetsTotalNum);
	}

	private void AddCollectionContent(EElementType tp, PetInfo[] petInfos)
	{
		if (this.mContentOriginal == null)
		{
			this.mContentOriginal = Res.LoadGUI("GUI/collectionContent");
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.mContentOriginal);
		gameObject.name = this.mContentOriginal.name;
		gameObject.transform.parent = this.mCollectionTable2.gameObject.transform;
		this.mContents.Add(gameObject.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		CollectionContent collectionContent = gameObject.AddComponent<CollectionContent>();
		collectionContent.mPriority = this.mBasePriority++;
		collectionContent.InitWithBaseScene(this.mBaseScene, tp, petInfos);
	}

	private void OnFilterChanged()
	{
		if (UIToggle.current.value)
		{
			string name = UIToggle.current.gameObject.name;
			switch (name)
			{
			case "allBtn":
				this.mCollectionTable2.Filter = UICollectionBetterTable.FilterType.MAX;
				break;
			case "fireBtn":
				this.mCollectionTable2.Filter = UICollectionBetterTable.FilterType.Fire;
				break;
			case "waterBtn":
				this.mCollectionTable2.Filter = UICollectionBetterTable.FilterType.Water;
				break;
			case "woodBtn":
				this.mCollectionTable2.Filter = UICollectionBetterTable.FilterType.Wood;
				break;
			case "lightBtn":
				this.mCollectionTable2.Filter = UICollectionBetterTable.FilterType.Light;
				break;
			case "darkBtn":
				this.mCollectionTable2.Filter = UICollectionBetterTable.FilterType.Dark;
				break;
			}
			this.mSW.UpdateScrollbars();
			this.UpdateScrollBar();
		}
	}

	public void Refresh(PetDataEx petData)
	{
		if (petData != null)
		{
			this.mCollectionTable2.Refresh(petData);
		}
	}
}
