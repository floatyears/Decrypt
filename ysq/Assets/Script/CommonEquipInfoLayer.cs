using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class CommonEquipInfoLayer : MonoBehaviour
{
	private UILabel mName;

	public CommonIconItem mEquipIconItem;

	private UILabel mLevel;

	private UILabel mLevelValue;

	private UILabel mSubQuality;

	private UILabel mSubQualityValue;

	private bool hasInit;

	private void Init()
	{
		this.hasInit = true;
		this.mName = GameUITools.FindUILabel("Name", base.gameObject);
		this.mEquipIconItem = CommonIconItem.Create(base.gameObject, new Vector3(-47f, 82f, 0f), null, false, 1f, null);
		this.mLevel = GameUITools.FindUILabel("Level", base.gameObject);
		this.mLevelValue = GameUITools.FindUILabel("Value", this.mLevel.gameObject);
		this.mSubQuality = GameUITools.FindUILabel("SubQuality", base.gameObject);
		this.mSubQualityValue = GameUITools.FindUILabel("Value", this.mSubQuality.gameObject);
	}

	public void Refresh(ItemDataEx data, bool isEnhance, bool isLocal = true)
	{
		if (data == null)
		{
			global::Debug.LogError(new object[]
			{
				"ItemDataEx is null"
			});
			return;
		}
		if (!this.hasInit)
		{
			this.Init();
		}
		if (data.Info.Type == 0)
		{
			if (isEnhance)
			{
				this.mLevel.text = Singleton<StringManager>.Instance.GetString("equipImprove11") + Singleton<StringManager>.Instance.GetString("Colon0");
				this.mLevelValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
				{
					data.GetEquipEnhanceLevel(),
					Globals.Instance.Player.ItemSystem.GetMaxEquipEnhanceLevel(isLocal)
				});
			}
			else
			{
				this.mLevel.text = Singleton<StringManager>.Instance.GetString("equipImprove12") + Singleton<StringManager>.Instance.GetString("Colon0");
				this.mLevelValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
				{
					data.GetEquipRefineLevel(),
					Globals.Instance.Player.ItemSystem.GetMaxEquipRefineLevel()
				});
			}
			this.mSubQualityValue.text = data.Info.SubQuality.ToString();
		}
		else if (data.Info.Type == 1)
		{
			if (isEnhance)
			{
				this.mLevel.text = Singleton<StringManager>.Instance.GetString("equipImprove11") + Singleton<StringManager>.Instance.GetString("Colon0");
				this.mLevelValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
				{
					data.GetTrinketEnhanceLevel(),
					Globals.Instance.Player.ItemSystem.GetMaxTrinketEnhanceLevel()
				});
			}
			else
			{
				this.mLevel.text = Singleton<StringManager>.Instance.GetString("equipImprove12") + Singleton<StringManager>.Instance.GetString("Colon0");
				this.mLevelValue.text = Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
				{
					data.GetTrinketRefineLevel(),
					Globals.Instance.Player.ItemSystem.GetMaxTrinketRefineLevel()
				});
			}
			this.mSubQualityValue.text = data.Info.SubQuality.ToString();
		}
		else if (data.Info.Type == 3 && (data.Info.SubType == 1 || data.Info.SubType == 2))
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(data.Info.Value2);
			if (Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(info.ID))
			{
				this.mSubQuality.text = string.Empty;
				this.mSubQualityValue.text = string.Empty;
				this.mLevel.text = string.Empty;
				this.mLevelValue.text = string.Empty;
			}
			else
			{
				this.mSubQualityValue.text = info.SubQuality.ToString();
				this.mLevel.text = string.Empty;
				this.mLevelValue.text = string.Empty;
			}
		}
		else
		{
			if (!Globals.Instance.Player.ItemSystem.IsTrinketEnhanceExp(data.Info.ID))
			{
				global::Debug.LogErrorFormat("CommonEquipInfoLayer's data Type should be Equip or Trinket", new object[0]);
				return;
			}
			this.mSubQuality.text = string.Empty;
			this.mSubQualityValue.text = string.Empty;
			this.mLevel.text = string.Empty;
			this.mLevelValue.text = string.Empty;
		}
		this.mName.text = data.Info.Name;
		this.mName.color = Tools.GetItemQualityColor(data.Info.Quality);
		this.mEquipIconItem.Refresh(data, false, false, false);
	}

	public static CommonEquipInfoLayer CreateCommonEquipInfoLayer(GameObject parent, Vector3 pos)
	{
		GameObject gameObject = Res.LoadGUI("GUI/CommonEquipInfoLayer");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.LoadGUI GUI/CommonEquipInfoLayer error"
			});
			return null;
		}
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject);
		gameObject2.SetActive(true);
		GameUITools.AddChild(parent, gameObject2);
		gameObject2.transform.localPosition = pos;
		return gameObject2.AddComponent<CommonEquipInfoLayer>();
	}

	public void PlayCreateAnim()
	{
		if (!this.hasInit)
		{
			this.Init();
		}
		this.mEquipIconItem.gameObject.SetActive(true);
		Transform[] componentsInChildren = this.mEquipIconItem.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Transform transform = array[i];
			transform.parent = this.mEquipIconItem.transform.parent;
		}
		this.mEquipIconItem.GetComponent<UIWidget>().pivot = UIWidget.Pivot.Center;
		Transform[] array2 = componentsInChildren;
		for (int j = 0; j < array2.Length; j++)
		{
			Transform transform2 = array2[j];
			transform2.parent = this.mEquipIconItem.transform;
		}
		this.mEquipIconItem.transform.localScale = Vector3.zero;
		GUIEquipBagScene session = GameUIManager.mInstance.GetSession<GUIEquipBagScene>();
		if (session == null)
		{
			return;
		}
		if (session.Anim != null && session.Anim.keys.Length <= 0)
		{
			session.Anim = null;
		}
		HOTween.To(this.mEquipIconItem.transform, session.Dura, new TweenParms().Prop("localScale", Vector3.one).Ease(session.Anim).OnComplete(new TweenDelegate.TweenCallback(this.AnimEnd)));
	}

	private void AnimEnd()
	{
		GUIEquipInfoPopUp component = base.transform.parent.gameObject.GetComponent<GUIEquipInfoPopUp>();
		if (component == null)
		{
			return;
		}
		component.PlayUI61();
	}
}
