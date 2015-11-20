using Att;
using System;
using UnityEngine;

public class TrinketItem : MonoBehaviour
{
	private UISprite ItemIcon;

	private UISprite ItemQuality;

	private UILabel ItemNum;

	private GameObject Gray;

	private GameObject Sfx;

	private ItemInfo ItemInfo;

	private void Awake()
	{
		this.ItemQuality = base.transform.GetComponent<UISprite>();
		this.ItemIcon = base.transform.Find("ItemIcon").GetComponent<UISprite>();
		this.ItemNum = base.transform.Find("Num").GetComponent<UILabel>();
		this.Gray = base.transform.Find("Gray").gameObject;
		this.Sfx = base.transform.Find("ui59_1").gameObject;
		this.Sfx.SetActive(false);
		Tools.SetParticleRenderQueue(this.Sfx, 3100, 1f);
		UIEventListener expr_A9 = UIEventListener.Get(base.gameObject);
		expr_A9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A9.onClick, new UIEventListener.VoidDelegate(this.OnTrinketItemClick));
	}

	public void Refresh(ItemInfo item)
	{
		this.Sfx.gameObject.SetActive(false);
		this.ItemInfo = item;
		if (this.ItemInfo == null)
		{
			base.gameObject.SetActive(false);
		}
		this.ItemIcon.spriteName = this.ItemInfo.Icon;
		this.ItemQuality.spriteName = Tools.GetItemQualityIcon(this.ItemInfo.Quality);
		ItemSubSystem itemSystem = Globals.Instance.Player.ItemSystem;
		int itemCount = itemSystem.GetItemCount(this.ItemInfo.ID);
		this.ItemNum.text = itemCount.ToString();
		this.Gray.SetActive(itemCount == 0);
		base.gameObject.SetActive(true);
	}

	public void PlaySfx()
	{
		this.Sfx.gameObject.SetActive(true);
	}

	public void StopSfx()
	{
		this.Sfx.gameObject.SetActive(false);
	}

	public void OnTrinketItemClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_003");
		if (go == null)
		{
			return;
		}
		if (this.ItemInfo == null)
		{
			return;
		}
		GUIPillageScene.RequestQueryPillageTarget(this.ItemInfo);
	}
}
