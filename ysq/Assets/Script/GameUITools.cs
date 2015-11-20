using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using System.Text;
using UnityEngine;

public static class GameUITools
{
	public static StringBuilder mSb = new StringBuilder(42);

	public static GameObject FindGameObject(string name, GameObject parent)
	{
		if (parent == null)
		{
			return null;
		}
		Transform transform = parent.transform.Find(name);
		if (transform == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("{0} Find GameObject {1} error!", parent.name, name)
			});
			return null;
		}
		return transform.gameObject;
	}

	public static UILabel FindUILabel(string name, GameObject parent)
	{
		GameObject gameObject = GameUITools.FindGameObject(name, parent);
		if (gameObject != null)
		{
			return gameObject.GetComponent<UILabel>();
		}
		return null;
	}

	public static UISprite FindUISprite(string name, GameObject parent)
	{
		GameObject gameObject = GameUITools.FindGameObject(name, parent);
		if (gameObject != null)
		{
			return gameObject.GetComponent<UISprite>();
		}
		return null;
	}

	public static GameObject RegisterClickEvent(string name, UIEventListener.VoidDelegate delgate, GameObject parent)
	{
		GameObject gameObject = GameUITools.FindGameObject(name, parent);
		if (gameObject != null)
		{
			UIEventListener expr_1A = UIEventListener.Get(gameObject);
			expr_1A.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1A.onClick, delgate);
		}
		return gameObject;
	}

	public static GameObject RegisterPressEvent(string name, UIEventListener.BoolDelegate delgate, GameObject parent)
	{
		GameObject gameObject = GameUITools.FindGameObject(name, parent);
		if (gameObject != null)
		{
			UIEventListener expr_1A = UIEventListener.Get(gameObject);
			expr_1A.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_1A.onPress, delgate);
		}
		return gameObject;
	}

	public static GameObject RegisterDragEvent(string name, UIEventListener.VectorDelegate delgate, GameObject parent)
	{
		GameObject gameObject = GameUITools.FindGameObject(name, parent);
		if (gameObject != null)
		{
			UIEventListener expr_1A = UIEventListener.Get(gameObject);
			expr_1A.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(expr_1A.onDrag, delgate);
		}
		return gameObject;
	}

	public static UILabel SetLabelLocalText(GameObject go, string key)
	{
		if (go == null)
		{
			return null;
		}
		UILabel component = go.GetComponent<UILabel>();
		if (component == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("{0} not has label!", go.name)
			});
			return null;
		}
		component.text = Singleton<StringManager>.Instance.GetString(key);
		return component;
	}

	public static UILabel SetLabelLocalText(string name, string key, GameObject parent)
	{
		GameObject go = GameUITools.FindGameObject(name, parent);
		return GameUITools.SetLabelLocalText(go, key);
	}

	public static void PlayOpenWindowAnim(Transform trans, TweenDelegate.TweenCallback cb = null, bool timeScale = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string value = (!(trans.parent != null)) ? trans.name : trans.parent.name;
		string text = stringBuilder.Append(value).Append("Open").ToString();
		stringBuilder.Remove(0, stringBuilder.Length);
		string p_id = stringBuilder.Append(value).Append("Close").ToString();
		if (HOTween.IsTweening(p_id))
		{
			HOTween.Kill(p_id);
		}
		if (!HOTween.IsTweening(text))
		{
			TweenAlpha.Begin(trans.gameObject, 0f, 0f);
			TweenAlpha.Begin(trans.gameObject, 0.3f, 1f);
			if (timeScale)
			{
				HOTween.To(trans, 0f, new TweenParms().Prop("localScale", Vector3.zero));
				HOTween.To(trans, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack).OnComplete(cb)).id = text;
			}
			else
			{
				HOTween.To(trans, 0f, new TweenParms().UpdateType(UpdateType.TimeScaleIndependentUpdate).Prop("localScale", Vector3.zero));
				HOTween.To(trans, 0.25f, new TweenParms().UpdateType(UpdateType.TimeScaleIndependentUpdate).Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack).OnComplete(cb)).id = text;
			}
		}
	}

	public static void PlayCloseWindowAnim(Transform trans, TweenDelegate.TweenCallback cb, bool timeScale = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string value = (!(trans.parent != null)) ? trans.name : trans.parent.name;
		string p_id = stringBuilder.Append(value).Append("Open").ToString();
		stringBuilder.Remove(0, stringBuilder.Length);
		string text = stringBuilder.Append(value).Append("Close").ToString();
		if (HOTween.IsTweening(p_id))
		{
			HOTween.Kill(p_id);
		}
		if (!HOTween.IsTweening(text))
		{
			TweenAlpha.Begin(trans.gameObject, 0f, 1f);
			TweenAlpha.Begin(trans.gameObject, 0.3f, 0f);
			if (timeScale)
			{
				HOTween.To(trans, 0.25f, new TweenParms().Prop("localScale", Vector3.zero).Ease(EaseType.EaseInBack).OnComplete(cb)).id = text;
			}
			else
			{
				HOTween.To(trans, 0.25f, new TweenParms().UpdateType(UpdateType.TimeScaleIndependentUpdate).Prop("localScale", Vector3.zero).Ease(EaseType.EaseInBack).OnComplete(cb)).id = text;
			}
		}
	}

	public static GameObject CreateReward(int rewardType, int rewardValue1, int rewardValue2, Transform parent, bool showValue = true, bool showTips = true, float x = 36f, float y = -7f, float z = -2000f, float r = 20f, float g = 13f, float b = 7f, int quality = 0)
	{
		GameObject gameObject = null;
		switch (rewardType)
		{
		case 1:
		case 2:
		case 8:
		case 9:
		case 10:
		case 11:
		case 13:
		case 14:
		case 15:
		case 17:
		case 18:
			gameObject = Tools.InstantiateGUIPrefab("GUI/RewardMoney");
			if (gameObject != null)
			{
				RewardMoney rewardMoney = gameObject.AddComponent<RewardMoney>();
				if (rewardMoney != null)
				{
					rewardMoney.Init(rewardValue1, showValue, showTips, x, y, z, quality, (ERewardType)rewardType);
				}
			}
			goto IL_294;
		case 3:
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(rewardValue1);
			if (info == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("ItemDict.GetInfo, ID = {0}", rewardValue1)
				});
				return null;
			}
			if (info.Type == 3 && info.SubType == 0)
			{
				gameObject = Tools.InstantiateGUIPrefab("GUI/RewardPetItem");
				if (gameObject != null)
				{
					RewardPetItem rewardPetItem = gameObject.AddComponent<RewardPetItem>();
					if (rewardPetItem != null)
					{
						rewardPetItem.Init(info, rewardValue2, showValue, showTips);
					}
				}
			}
			else if (info.Type == 3 && info.SubType == 3)
			{
				gameObject = Tools.InstantiateGUIPrefab("GUI/RewardLopetItem");
				if (gameObject != null)
				{
					RewardLopetItem rewardLopetItem = gameObject.AddComponent<RewardLopetItem>();
					if (rewardLopetItem != null)
					{
						rewardLopetItem.Init(info, rewardValue2, showValue, showTips);
					}
				}
			}
			else
			{
				gameObject = Tools.InstantiateGUIPrefab("GUI/RewardItem");
				if (gameObject != null)
				{
					RewardItem rewardItem = gameObject.AddComponent<RewardItem>();
					if (rewardItem != null)
					{
						rewardItem.Init(info, rewardValue2, showValue, showTips);
					}
				}
			}
			goto IL_294;
		}
		case 4:
			gameObject = Tools.InstantiateGUIPrefab("GUI/RewardPet");
			if (gameObject != null)
			{
				RewardPet rewardPet = gameObject.AddComponent<RewardPet>();
				if (rewardPet != null)
				{
					rewardPet.Init(rewardValue1, showTips);
				}
			}
			goto IL_294;
		case 12:
			gameObject = Tools.InstantiateGUIPrefab("GUI/RewardFashion");
			if (gameObject != null)
			{
				RewardFashion rewardFashion = gameObject.AddComponent<RewardFashion>();
				if (rewardFashion != null)
				{
					rewardFashion.Init(rewardValue1, showTips);
				}
			}
			goto IL_294;
		case 16:
			gameObject = Tools.InstantiateGUIPrefab("GUI/RewardLopet");
			if (gameObject != null)
			{
				RewardLopet rewardLopet = gameObject.AddComponent<RewardLopet>();
				if (rewardLopet != null)
				{
					rewardLopet.Init(rewardValue1, showTips);
				}
			}
			goto IL_294;
		}
		global::Debug.LogError(new object[]
		{
			string.Format("unknown RewardType = {0}", rewardType)
		});
		IL_294:
		if (gameObject != null)
		{
			gameObject.transform.parent = parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
		}
		return gameObject;
	}

	public static GameObject CreateMinReward(int rewardType, int rewardValue1, int rewardValue2, Transform parent)
	{
		MonoBehaviour monoBehaviour = null;
		switch (rewardType)
		{
		case 0:
			goto IL_40;
		case 1:
		case 2:
			IL_20:
			if (rewardType != 12)
			{
				if (rewardType != 16)
				{
					if (rewardType != 20)
					{
						monoBehaviour = RewardCurrency.CreateReward((ERewardType)rewardType, rewardValue1);
						goto IL_1C2;
					}
					goto IL_40;
				}
				else
				{
					LopetInfo info = Globals.Instance.AttDB.LopetDict.GetInfo(rewardValue1);
					if (info == null)
					{
						global::Debug.LogError(new object[]
						{
							string.Format("LopetDict.GetInfo, ID = {0}", rewardValue1)
						});
						return null;
					}
					monoBehaviour = QuestRewardLopet.CreateReward(info, rewardValue2);
					goto IL_1C2;
				}
			}
			else
			{
				FashionInfo info2 = Globals.Instance.AttDB.FashionDict.GetInfo(rewardValue1);
				if (info2 == null)
				{
					global::Debug.LogError(new object[]
					{
						string.Format("FashionDict.GetInfo, ID = {0}", rewardValue1)
					});
					return null;
				}
				monoBehaviour = QuestRewardFashion.CreateReward(info2, rewardValue2);
				goto IL_1C2;
			}
			break;
		case 3:
		{
			ItemInfo info3 = Globals.Instance.AttDB.ItemDict.GetInfo(rewardValue1);
			if (info3 == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("ItemDict.GetInfo, ID = {0}", rewardValue1)
				});
				return null;
			}
			if (info3.Type == 3 && info3.SubType == 0)
			{
				monoBehaviour = QuestRewardPetItem.CreateReward(info3, rewardValue2);
			}
			else if (info3.Type == 3 && info3.SubType == 3)
			{
				monoBehaviour = QuestRewardLopetItem.CreateReward(info3, rewardValue2);
			}
			else
			{
				monoBehaviour = QuestRewardItem.CreateReward(info3, rewardValue2);
			}
			goto IL_1C2;
		}
		case 4:
		{
			PetInfo info4 = Globals.Instance.AttDB.PetDict.GetInfo(rewardValue1);
			if (info4 == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("PetDict.GetInfo, ID = {0}", rewardValue1)
				});
				return null;
			}
			monoBehaviour = QuestRewardPet.CreateReward(info4, rewardValue2);
			goto IL_1C2;
		}
		}
		goto IL_20;
		IL_40:
		IL_1C2:
		if (monoBehaviour != null)
		{
			monoBehaviour.transform.parent = parent;
			monoBehaviour.transform.localPosition = Vector3.zero;
			monoBehaviour.transform.localRotation = Quaternion.identity;
			monoBehaviour.transform.localScale = Vector3.one;
			return monoBehaviour.gameObject;
		}
		return null;
	}

	public static void PlayUISlilderEffect(UISlider slider, UILabel label, uint num, float endValue, float duration = 0.5f, int totalValue = 0)
	{
		if (HOTween.IsTweening(slider))
		{
			HOTween.Complete(slider);
			HOTween.Kill(slider);
		}
		if (num > 0u)
		{
			HOTween.To(slider, duration, new TweenParms().Prop("value", 1f).SpeedBased(false).OnUpdate(new TweenDelegate.TweenCallbackWParms(GameUITools.OnSliderUpdate), new object[]
			{
				slider,
				label,
				totalValue
			}).OnComplete(new TweenDelegate.TweenCallbackWParms(GameUITools.OnSliderCompl), new object[]
			{
				slider,
				label,
				num,
				endValue,
				duration,
				totalValue
			}));
		}
		else
		{
			HOTween.To(slider, duration, new TweenParms().Prop("value", endValue).SpeedBased(false).OnUpdate(new TweenDelegate.TweenCallbackWParms(GameUITools.OnSliderUpdate), new object[]
			{
				slider,
				label,
				totalValue
			}));
		}
	}

	private static void OnSliderCompl(TweenEvent e)
	{
		if (e.parms != null)
		{
			UISlider uISlider = (UISlider)e.parms[0];
			UILabel uILabel = (UILabel)e.parms[1];
			uint num = (uint)e.parms[2];
			float num2 = (float)e.parms[3];
			float num3 = (float)e.parms[4];
			int num4 = (int)e.parms[5];
			uISlider.value = 0f;
			if (num4 != 0)
			{
				uILabel.text = GameUITools.mSb.Remove(0, GameUITools.mSb.Length).Append(0).Append("/").Append(num4).ToString();
			}
			else
			{
				uILabel.text = GameUITools.mSb.Remove(0, GameUITools.mSb.Length).Append("0%").ToString();
			}
			HOTween.To(uISlider, num3, new TweenParms().Prop("value", 1f).SpeedBased(false).OnUpdate(new TweenDelegate.TweenCallbackWParms(GameUITools.OnSliderUpdate), new object[]
			{
				uISlider,
				uILabel,
				num4
			}).Loops((int)(num - 1u), LoopType.Restart).OnComplete(new TweenDelegate.TweenCallbackWParms(GameUITools.OnSliderCompl2), new object[]
			{
				uISlider,
				uILabel,
				num2,
				num3,
				num4
			}));
		}
	}

	private static void OnSliderCompl2(TweenEvent e)
	{
		if (e.parms != null)
		{
			UISlider uISlider = (UISlider)e.parms[0];
			UILabel uILabel = (UILabel)e.parms[1];
			float num = (float)e.parms[2];
			float p_duration = (float)e.parms[3];
			int num2 = (int)e.parms[4];
			uISlider.value = 0f;
			if (num2 != 0)
			{
				uILabel.text = GameUITools.mSb.Remove(0, GameUITools.mSb.Length).Append(0).Append("/").Append(num2).ToString();
			}
			else
			{
				uILabel.text = GameUITools.mSb.Remove(0, GameUITools.mSb.Length).Append("0%").ToString();
			}
			HOTween.To(uISlider, p_duration, new TweenParms().Prop("value", num).SpeedBased(false).OnUpdate(new TweenDelegate.TweenCallbackWParms(GameUITools.OnSliderUpdate), new object[]
			{
				uISlider,
				uILabel,
				num2
			}));
		}
	}

	private static void OnSliderUpdate(TweenEvent e)
	{
		if (e.parms != null)
		{
			UISlider uISlider = (UISlider)e.parms[0];
			UILabel uILabel = (UILabel)e.parms[1];
			int num = (int)e.parms[2];
			if (num == 0)
			{
				GameUITools.mSb.Remove(0, GameUITools.mSb.Length).Append(Mathf.Round(uISlider.value * 100f)).Append("%");
			}
			else
			{
				GameUITools.mSb.Remove(0, GameUITools.mSb.Length).Append(Mathf.Round(uISlider.value * (float)num)).Append("/").Append(num);
			}
			uILabel.text = GameUITools.mSb.ToString();
		}
	}

	public static void AddChild(GameObject parent, GameObject child)
	{
		if (parent != null && child != null)
		{
			child.transform.parent = parent.transform;
			child.transform.localPosition = Vector3.zero;
			child.transform.localRotation = Quaternion.identity;
			child.transform.localScale = Vector3.one;
			child.layer = parent.layer;
		}
	}

	public static void IncreaseObjectsDepth(GameObject obj, int value)
	{
		UIWidget component = obj.GetComponent<UIWidget>();
		if (component != null)
		{
			component.depth += value;
		}
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			GameUITools.IncreaseObjectsDepth(obj.transform.GetChild(i).gameObject, value);
		}
	}

	public static string FormatPvpRecordTime(int timecount)
	{
		int num = timecount / 86400;
		if (num != 0)
		{
			return Singleton<StringManager>.Instance.GetString("pvpTxt9", new object[]
			{
				num
			});
		}
		int num2 = timecount / 3600;
		if (num2 != 0)
		{
			return Singleton<StringManager>.Instance.GetString("pvpTxt8", new object[]
			{
				num2
			});
		}
		int num3 = timecount / 60;
		if (num3 != 0)
		{
			return Singleton<StringManager>.Instance.GetString("pvpTxt7", new object[]
			{
				num3
			});
		}
		if (timecount != 0)
		{
			return Singleton<StringManager>.Instance.GetString("pvpTxt6", new object[]
			{
				timecount
			});
		}
		return string.Empty;
	}

	public static int GetRemoveGemGoldCost(ItemInfo gemInfo)
	{
		return 1000;
	}

	public static string GetPetTypeStr(int petType)
	{
		string result = string.Empty;
		if (petType < 5 && petType > 0)
		{
			result = Singleton<StringManager>.Instance.GetString(string.Format("petTypeStr{0}", petType));
		}
		return result;
	}

	public static float Distance2D(Vector3 a, Vector3 b)
	{
		return Mathf.Sqrt(GameUITools.DistanceSquared2D(a, b));
	}

	public static float DistanceSquared2D(Vector3 a, Vector3 b)
	{
		return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
	}

	public static void CompleteAllHotween()
	{
		HOTween.Complete();
	}

	public static void UpdateUIBoxCollider(Transform tr, float addHeight = 4f, bool includeWidth = false)
	{
		UISprite component = tr.GetComponent<UISprite>();
		if (component != null)
		{
			component.autoResizeBoxCollider = false;
			BoxCollider component2 = tr.GetComponent<BoxCollider>();
			if (component2 != null)
			{
				UIWidget component3 = tr.GetComponent<UIWidget>();
				if (component3 != null)
				{
					Vector3[] localCorners = component3.localCorners;
					component2.center = Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
					Vector3 vector = localCorners[2] - localCorners[0];
					component2.size = new Vector3(vector.x + ((!includeWidth) ? 0f : addHeight), vector.y + addHeight, 0f);
				}
			}
		}
	}

	public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight, bool mipmap)
	{
		Texture2D texture2D = new Texture2D(targetWidth, targetHeight, source.format, mipmap);
		Color[] pixels = texture2D.GetPixels(0);
		float num = 1f / (float)targetWidth;
		float num2 = 1f / (float)targetHeight;
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i] = source.GetPixelBilinear(num * (float)(i % targetWidth), num2 * Mathf.Floor((float)(i / targetWidth)));
		}
		texture2D.SetPixels(pixels, 0);
		texture2D.Apply();
		return texture2D;
	}
}
