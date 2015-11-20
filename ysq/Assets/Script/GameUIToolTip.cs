using System;
using UnityEngine;

public class GameUIToolTip : MonoBehaviour
{
	private UISprite mBackBg;

	private UILabel mTitle;

	private UILabel mDesc;

	private UILabel mLevel;

	private UILabel mTimes;

	protected Vector3 mSize = Vector3.zero;

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mBackBg = base.transform.Find("backGround").GetComponent<UISprite>();
		this.mTitle = base.transform.Find("title").GetComponent<UILabel>();
		this.mDesc = base.transform.Find("description").GetComponent<UILabel>();
		Transform transform = base.transform.Find("Level");
		if (transform != null)
		{
			this.mLevel = transform.GetComponent<UILabel>();
		}
		Transform transform2 = base.transform.Find("times");
		if (transform2 != null)
		{
			this.mTimes = transform2.GetComponent<UILabel>();
		}
	}

	public void EnableToolTip()
	{
		base.gameObject.SetActive(true);
	}

	public void HideTipAnim()
	{
		base.gameObject.SetActive(false);
	}

	public void Create(Transform parent, string title, string description)
	{
		Tools.Assert(parent, "Invalid parent");
		Tools.Assert(!string.IsNullOrEmpty(title), "Invalid title");
		Tools.Assert(!string.IsNullOrEmpty(description), "Invalid description");
		base.transform.parent = parent;
		base.transform.localScale = Vector3.one;
		this.mTitle.color = Color.white;
		this.InitTooltip(title, description);
	}

	public void Create(Transform parent, string title, string description, int itemQuality)
	{
		Tools.Assert(parent, "Invalid parent");
		Tools.Assert(!string.IsNullOrEmpty(title), "Invalid title");
		Tools.Assert(!string.IsNullOrEmpty(description), "Invalid description");
		this.mTitle.color = Tools.GetItemQualityColor(itemQuality);
		base.transform.parent = parent;
		base.transform.localScale = Vector3.one;
		this.InitTooltip(title, description);
	}

	public void Create(Transform parent, string title, string description, string lv)
	{
		this.Create(parent, title, description);
		Tools.Assert(!string.IsNullOrEmpty(lv), "Invalid lv");
		this.mLevel.text = lv;
		this.mSize.y = this.mSize.y + this.mLevel.printedSize.y;
		this.mBackBg.height = Mathf.RoundToInt(this.mSize.y);
		base.transform.localPosition = new Vector3(0f, this.mSize.y, 0f);
	}

	public void CreateSignInToolTip(Transform parent, string title, string description, int times, int levels = 0, int itemQuality = 0, Transform cameraTransform = null)
	{
		Tools.Assert(parent, "Invalid parent");
		Tools.Assert(!string.IsNullOrEmpty(title), "Invalid title");
		Tools.Assert(!string.IsNullOrEmpty(description), "Invalid description");
		Tools.Assert(times, "Invalid times");
		if (levels != 0)
		{
			Tools.Assert(levels, "Invalid Levels");
			this.mLevel.gameObject.SetActive(true);
			this.mLevel.text = Singleton<StringManager>.Instance.GetString("signInToolTipsLevels", new object[]
			{
				levels
			});
		}
		else
		{
			this.mLevel.gameObject.SetActive(false);
		}
		this.mTitle.color = Tools.GetItemQualityColor(itemQuality);
		this.InitSignInTooltip(title, description, times);
		base.transform.parent = parent;
		float y = (float)this.mBackBg.height - this.mBackBg.transform.localPosition.y + 66f;
		base.transform.localPosition = new Vector3(0f, y, -2000f);
		base.transform.parent = cameraTransform;
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Min((float)(18 + this.mBackBg.height) - this.mBackBg.transform.localPosition.y, base.transform.localPosition.y), base.transform.localPosition.z);
		base.transform.localScale = Vector3.one;
	}

	private void InitTooltip(string title, string desc)
	{
		this.mTitle.text = title;
		this.mDesc.text = desc;
		Transform transform = this.mDesc.transform;
		Vector3 localScale = transform.localScale;
		if (this.mTitle.width > 276)
		{
			this.mDesc.width = 290 + Mathf.RoundToInt((float)(this.mTitle.width - 276));
		}
		else
		{
			this.mDesc.width = 290;
		}
		this.mSize = this.mDesc.printedSize;
		this.mSize.x = this.mSize.x * localScale.x;
		this.mSize.x = this.mSize.x + 55f;
		this.mSize.y = this.mSize.y * localScale.y;
		this.mSize.y = this.mSize.y + 30f;
		Vector4 border = this.mBackBg.border;
		this.mSize.x = this.mSize.x + (border.x + border.z);
		this.mSize.y = this.mSize.y + (border.y + border.w);
		this.mSize.x = Mathf.Max(332f, this.mSize.x);
		this.mSize.y = Mathf.Max(98f, this.mSize.y);
		if (this.mTitle.width > 276)
		{
			this.mSize.x = (float)(60 + this.mTitle.width);
		}
		this.mBackBg.width = Mathf.RoundToInt(this.mSize.x);
		this.mBackBg.height = Mathf.RoundToInt(this.mSize.y);
		base.transform.localPosition = new Vector3(0f, this.mSize.y, 0f);
	}

	private void InitSignInTooltip(string title, string desc, int times)
	{
		this.mTitle.text = title;
		this.mDesc.text = desc;
		this.mTimes.text = Singleton<StringManager>.Instance.GetString("signInToolTipsTimes", new object[]
		{
			times
		});
		bool activeSelf = this.mLevel.gameObject.activeSelf;
		Transform transform = this.mDesc.transform;
		Vector3 localScale = transform.localScale;
		if (this.mTitle.width > 276)
		{
			this.mDesc.width = 290 + Mathf.RoundToInt((float)(this.mTitle.width - 276));
		}
		else
		{
			this.mDesc.width = 290;
		}
		this.mSize = this.mDesc.printedSize;
		this.mSize.x = this.mSize.x * localScale.x;
		this.mSize.x = this.mSize.x + 55f;
		this.mSize.y = this.mSize.y * localScale.y;
		this.mSize.y = this.mSize.y + 10f;
		Vector4 border = this.mBackBg.border;
		this.mSize.x = this.mSize.x + (border.x + border.z);
		this.mSize.y = this.mSize.y + (border.y + border.w);
		if (this.mTitle.width > 276)
		{
			this.mSize.x = (float)(60 + this.mTitle.width);
		}
		this.mSize.x = Mathf.Max(352f, this.mSize.x);
		this.mSize.y = Mathf.Max(78f, this.mSize.y) + (float)((!activeSelf) ? 28 : 56);
		this.mSize.y = this.mSize.y + this.mTimes.printedSize.y;
		if (activeSelf)
		{
			this.mDesc.transform.localPosition = new Vector3(this.mDesc.transform.localPosition.x, -20f, this.mDesc.transform.localPosition.z);
		}
		else
		{
			this.mDesc.transform.localPosition = new Vector3(this.mDesc.transform.localPosition.x, 8f, this.mDesc.transform.localPosition.z);
		}
		this.mTimes.transform.localPosition = new Vector3(this.mTimes.transform.localPosition.x, 68f - this.mSize.y + this.mTimes.printedSize.y, this.mTimes.transform.localPosition.z);
		this.mBackBg.width = Mathf.RoundToInt(this.mSize.x);
		this.mBackBg.height = Mathf.RoundToInt(this.mSize.y);
	}
}
