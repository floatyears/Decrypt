using Proto;
using System;
using UnityEngine;

public class ShopCommonGrid : UICustomGrid
{
	public EShopType ShopType;

	private UnityEngine.Object ShopSpecItemPrefab;

	private UnityEngine.Object ShopCommonItemPrefab;

	protected override UICustomGridItem CreateGridItem()
	{
		return this.AddShopGridItem();
	}

	private UICustomGridItem AddShopGridItem()
	{
		GameObject gameObject;
		if (this.ShopType == EShopType.EShop_Common)
		{
			if (this.ShopCommonItemPrefab == null)
			{
				this.ShopCommonItemPrefab = Res.LoadGUI("GUI/ShopCommonItem");
			}
			gameObject = (GameObject)UnityEngine.Object.Instantiate(this.ShopCommonItemPrefab);
			gameObject.transform.parent = base.gameObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			ShopCommonItem shopCommonItem = gameObject.AddComponent<ShopCommonItem>();
			shopCommonItem.Init();
			return shopCommonItem;
		}
		if (this.ShopSpecItemPrefab == null)
		{
			this.ShopSpecItemPrefab = Res.LoadGUI("GUI/ShopRTItem");
		}
		gameObject = (GameObject)UnityEngine.Object.Instantiate(this.ShopSpecItemPrefab);
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		if (this.ShopType == EShopType.EShop_Common2 || this.ShopType == EShopType.EShop_Awaken || this.ShopType == EShopType.EShop_Lopet)
		{
			ShopRTItem shopRTItem = gameObject.AddComponent<ShopRTItem>();
			shopRTItem.Init();
			return shopRTItem;
		}
		ShopMinComItem shopMinComItem = gameObject.AddComponent<ShopMinComItem>();
		shopMinComItem.Init();
		return shopMinComItem;
	}
}
