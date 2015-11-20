using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("Game/GUI/HUD Text"), ExecuteInEditMode]
public class HUDText : MonoBehaviour
{
	public enum EType
	{
		ET_PLAYER,
		ET_PLAYER_CRITICAL,
		ET_NPC,
		ET_HEAL,
		ET_MAX
	}

	protected class Entry
	{
		public float time;

		public int type;

		public float offset;

		public float max_offset;

		public UILabel label;
	}

	[HideInInspector, SerializeField]
	private UIFont font;

	public UIFont bitmapFont;

	public Font trueTypeFont;

	public int[] fontSize = new int[]
	{
		16,
		16,
		16,
		16
	};

	public FontStyle[] fontStyle = new FontStyle[4];

	public Vector2[] effectDistance = new Vector2[]
	{
		Vector2.one,
		Vector2.one,
		Vector2.one,
		Vector2.one
	};

	public bool[] applyGradient = new bool[4];

	public Color[] gradientTop = new Color[]
	{
		Color.white,
		Color.white,
		Color.white,
		Color.white
	};

	public Color[] gradienBottom = new Color[]
	{
		Color.white,
		Color.white,
		Color.white,
		Color.white
	};

	public UILabel.Effect[] effect = new UILabel.Effect[4];

	public Color[] effectColor = new Color[]
	{
		Color.black,
		Color.black,
		Color.black,
		Color.black
	};

	public AnimationCurve offsetCurveForPlayerCritical = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(3f, 40f)
	});

	public AnimationCurve offsetCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(3f, 40f)
	});

	public AnimationCurve offsetCurveForNPCCritical = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(3f, 40f)
	});

	public AnimationCurve offsetCurveForNPC = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(3f, 40f)
	});

	private AnimationCurve[] offsetCurves = new AnimationCurve[4];

	public AnimationCurve alphaCurveForPlayerCritical = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(1f, 1f),
		new Keyframe(3f, 0f)
	});

	public AnimationCurve alphaCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(1f, 1f),
		new Keyframe(3f, 0f)
	});

	public AnimationCurve alphaCurveForNPCCritical = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(1f, 1f),
		new Keyframe(3f, 0f)
	});

	public AnimationCurve alphaCurveForNPC = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(1f, 1f),
		new Keyframe(3f, 0f)
	});

	private AnimationCurve[] alphaCurves = new AnimationCurve[4];

	public AnimationCurve scaleCurveForPlayerCritical = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 1f)
	});

	public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 1f)
	});

	public AnimationCurve scaleCurveForNPCCritical = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 1f)
	});

	public AnimationCurve scaleCurveForNPC = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 1f)
	});

	private AnimationCurve[] scaleCurves = new AnimationCurve[4];

	private List<HUDText.Entry> mList = new List<HUDText.Entry>();

	private List<HUDText.Entry> mUnused = new List<HUDText.Entry>();

	private StringBuilder mStringBuilder = new StringBuilder();

	private int counter;

	public UnityEngine.Object ambigiousFont
	{
		get
		{
			if (this.trueTypeFont != null)
			{
				return this.trueTypeFont;
			}
			if (this.bitmapFont != null)
			{
				return this.bitmapFont;
			}
			return this.font;
		}
		set
		{
			if (value is Font)
			{
				this.trueTypeFont = (value as Font);
				this.bitmapFont = null;
				this.font = null;
			}
			else if (value is UIFont)
			{
				this.bitmapFont = (value as UIFont);
				this.trueTypeFont = null;
				this.font = null;
			}
		}
	}

	public void Init()
	{
		this.offsetCurves[0] = this.offsetCurve;
		this.offsetCurves[1] = this.offsetCurveForPlayerCritical;
		this.offsetCurves[2] = this.offsetCurveForNPC;
		this.offsetCurves[3] = this.offsetCurveForNPCCritical;
		this.alphaCurves[0] = this.alphaCurve;
		this.alphaCurves[1] = this.alphaCurveForPlayerCritical;
		this.alphaCurves[2] = this.alphaCurveForNPC;
		this.alphaCurves[3] = this.alphaCurveForNPCCritical;
		this.scaleCurves[0] = this.scaleCurve;
		this.scaleCurves[1] = this.scaleCurveForPlayerCritical;
		this.scaleCurves[2] = this.scaleCurveForNPC;
		this.scaleCurves[3] = this.scaleCurveForNPCCritical;
		for (int i = 0; i < 5; i++)
		{
			HUDText.Entry item = this.CreateNew(0);
			this.mUnused.Add(item);
		}
	}

	private void SetEntryText(HUDText.Entry ent)
	{
		ent.label.fontSize = this.fontSize[ent.type];
		ent.label.fontStyle = this.fontStyle[ent.type];
		ent.label.effectDistance = this.effectDistance[ent.type];
		ent.label.applyGradient = this.applyGradient[ent.type];
		ent.label.gradientTop = this.gradientTop[ent.type];
		ent.label.gradientBottom = this.gradienBottom[ent.type];
		ent.label.effectStyle = this.effect[ent.type];
		ent.label.effectColor = this.effectColor[ent.type];
		if (this.trueTypeFont != null)
		{
			ent.label.ambigiousFont = this.trueTypeFont;
		}
		else if (this.bitmapFont != null)
		{
			ent.label.ambigiousFont = this.bitmapFont;
		}
		else
		{
			ent.label.ambigiousFont = this.font;
		}
	}

	private HUDText.Entry CreateNew(int type)
	{
		HUDText.Entry entry = new HUDText.Entry();
		entry.time = Time.realtimeSinceStartup;
		entry.type = type;
		entry.label = NGUITools.AddWidget<UILabel>(base.gameObject, 0);
		this.SetEntryText(entry);
		entry.label.overflowMethod = UILabel.Overflow.ResizeFreely;
		entry.label.cachedTransform.localScale = Vector3.one * 0.001f;
		this.counter++;
		return entry;
	}

	private HUDText.Entry Create(int type)
	{
		if (this.mUnused.Count > 0)
		{
			HUDText.Entry entry = this.mUnused[this.mUnused.Count - 1];
			this.mUnused.RemoveAt(this.mUnused.Count - 1);
			entry.time = Time.realtimeSinceStartup;
			entry.type = type;
			entry.label.depth = 0;
			entry.offset = 0f;
			entry.max_offset = 0f;
			this.SetEntryText(entry);
			entry.label.cachedTransform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
			NGUITools.SetActive(entry.label.gameObject, true);
			this.mList.Add(entry);
			return entry;
		}
		HUDText.Entry entry2 = this.CreateNew(type);
		this.mList.Add(entry2);
		return entry2;
	}

	private void Delete(HUDText.Entry ent)
	{
		this.mList.Remove(ent);
		this.mUnused.Add(ent);
		NGUITools.SetActive(ent.label.gameObject, false);
	}

	public void AddText(string text, int type = 0)
	{
		if (!base.enabled)
		{
			return;
		}
		HUDText.Entry entry = this.Create(type);
		if (text.Equals("miss"))
		{
			entry.label.text = "Twmz";
		}
		else if (text.Equals("immunity"))
		{
			entry.label.text = "Tmy";
		}
	}

	private string BuilderValueStr(string valuePrefix, int value)
	{
		value = Mathf.Abs(value);
		int num = value / 100000;
		int num2 = value % 100000 / 10000;
		int num3 = value % 100000 % 10000 / 1000;
		int num4 = value % 100000 % 10000 % 1000 / 100;
		int num5 = value % 100000 % 10000 % 1000 % 100 / 10;
		int num6 = value % 100000 % 10000 % 1000 % 100 % 10;
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
		if (num != 0)
		{
			this.mStringBuilder.Append(valuePrefix).Append(num);
		}
		if (num != 0 || num2 != 0)
		{
			this.mStringBuilder.Append(valuePrefix).Append(num2);
		}
		if (num != 0 || num2 != 0 || num3 != 0)
		{
			this.mStringBuilder.Append(valuePrefix).Append(num3);
		}
		if (num != 0 || num2 != 0 || num3 != 0 || num4 != 0)
		{
			this.mStringBuilder.Append(valuePrefix).Append(num4);
		}
		if (num != 0 || num2 != 0 || num3 != 0 || num4 != 0 || num5 != 0)
		{
			this.mStringBuilder.Append(valuePrefix).Append(num5);
		}
		if (num != 0 || num2 != 0 || num3 != 0 || num4 != 0 || num5 != 0 || num6 != 0)
		{
			this.mStringBuilder.Append(valuePrefix).Append(num6);
		}
		return this.mStringBuilder.ToString();
	}

	public void AddDamage(int value, int type = 0)
	{
		if (!base.enabled)
		{
			return;
		}
		HUDText.Entry entry = this.Create(type);
		if (type == 2)
		{
			entry.label.text = string.Format("b- {0}", this.BuilderValueStr("b", value));
		}
		else
		{
			entry.label.text = this.BuilderValueStr("d", value);
		}
	}

	public void AddSkillDamage(int value)
	{
		if (!base.enabled)
		{
			return;
		}
		HUDText.Entry entry = this.Create(0);
		entry.label.text = this.BuilderValueStr("e", value);
	}

	public void AddCriticalDamage(int value)
	{
		if (!base.enabled)
		{
			return;
		}
		HUDText.Entry entry = this.Create(1);
		entry.label.text = string.Format("abj {0}", this.BuilderValueStr("a", value));
	}

	public void AddHeal(int value)
	{
		if (!base.enabled)
		{
			return;
		}
		HUDText.Entry entry = this.Create(3);
		entry.label.text = string.Format("c+{0}", this.BuilderValueStr("c", value));
	}

	private void OnDisable()
	{
		for (int i = 0; i < this.mList.Count; i++)
		{
			HUDText.Entry entry = this.mList[i];
			if (entry.label != null)
			{
				this.mUnused.Add(entry);
				NGUITools.SetActive(entry.label.gameObject, false);
			}
		}
		this.mList.Clear();
	}

	private float EvaluateOffSetValue(HUDText.Entry ent, float cutTime)
	{
		return this.offsetCurves[ent.type].Evaluate(cutTime);
	}

	private float EvaluateAlphaValue(HUDText.Entry ent, float cutTime)
	{
		return this.alphaCurves[ent.type].Evaluate(cutTime);
	}

	private float EvaluateScaleValue(HUDText.Entry ent, float cutTime)
	{
		return this.scaleCurves[ent.type].Evaluate(cutTime);
	}

	private float GetOffsetTotalEnd(HUDText.Entry ent)
	{
		return this.offsetCurves[ent.type][this.offsetCurves[ent.type].length - 1].time;
	}

	private float GetAlphaTotalEnd(HUDText.Entry ent)
	{
		return this.alphaCurves[ent.type][this.alphaCurves[ent.type].length - 1].time;
	}

	private float GetScaleTotalEnd(HUDText.Entry ent)
	{
		return this.scaleCurves[ent.type][this.scaleCurves[ent.type].length - 1].time;
	}

	private void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = 0f;
		int i = this.mList.Count;
		while (i > 0)
		{
			HUDText.Entry entry = this.mList[--i];
			float num2 = realtimeSinceStartup - entry.time;
			float offset = entry.offset;
			entry.offset = this.EvaluateOffSetValue(entry, num2);
			entry.max_offset += entry.offset - offset;
			entry.label.alpha = this.EvaluateAlphaValue(entry, num2);
			float num3 = this.EvaluateScaleValue(entry, num2);
			if (num3 < 0.001f)
			{
				num3 = 0.001f;
			}
			if (entry.label != null)
			{
				entry.label.cachedTransform.localScale = new Vector3(num3, num3, 1f);
			}
			float num4 = Mathf.Max(this.GetScaleTotalEnd(entry), Mathf.Max(this.GetOffsetTotalEnd(entry), this.GetAlphaTotalEnd(entry)));
			if (num2 > num4)
			{
				this.Delete(entry);
			}
			else
			{
				if (entry.label != null)
				{
					entry.label.enabled = true;
				}
				if (num > entry.max_offset)
				{
					entry.max_offset = num;
				}
				else
				{
					num = entry.max_offset;
				}
				if (entry.label != null)
				{
					entry.label.cachedTransform.localPosition = new Vector3(0f, num, num);
					num += Mathf.Round(entry.label.cachedTransform.localScale.y * (float)entry.label.fontSize * 0.8f);
				}
			}
		}
	}
}
