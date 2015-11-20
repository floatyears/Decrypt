using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NtUniSdk.Unity3d
{
	public class NtOrderInfo
	{
		public const string ORDER_ID = "orderId";

		public const string ORDER_SDK_ID = "sdkOrderId";

		public const string PRODUCT_ID = "pid";

		public const string PRODUCT_NAME = "productName";

		public const string PRICE = "productPrice";

		public const string ERATIO = "eRatio";

		public const string SDKPIDS = "sdkPids";

		public const string PRODUCT_PRICE = "productCurrentPrice";

		public const string PRODUCT_COUNT = "productCount";

		public const string ORDER_DESC = "orderDesc";

		public const string ORDER_STATUS = "orderStatus";

		public const string ORDER_ERR_REASON = "orderErrReason";

		public const string ORDER_ETC = "orderEtc";

		public const string SIGNATURE = "signature";

		public const string ORDER_CHANNEL = "orderChannel";

		public const string USER_DATA = "userData";

		public const string TRANSACTION_RECEIPT = "transactionReceipt";

		public const string USER_PRICE_LOCALE_ID = "userPriceLocaleId";

		public const string USER_NAME = "userName";

		private const string SDK_JAVA_CLASS_ORDERINFO = "com.netease.ntunisdk.base.OrderInfo";

		private static Hashtable productList = new Hashtable();

		public string orderId = string.Empty;

		public string sdkOrderId = string.Empty;

		public string productId = string.Empty;

		public float productCurrentPrice = -1f;

		public int productCount = 1;

		public string orderDesc = string.Empty;

		public OrderStatus orderStatus;

		public string orderErrReason = string.Empty;

		public string orderEtc = string.Empty;

		public string signature = string.Empty;

		public string orderChannel = string.Empty;

		public string transactionReceipt = string.Empty;

		public string userData = string.Empty;

		public string userPriceLocaleId = string.Empty;

		public string userName = string.Empty;

		public JsonData ToJsonData()
		{
			JsonData jsonData = new JsonData();
			jsonData["orderId"] = this.orderId;
			jsonData["sdkOrderId"] = this.sdkOrderId;
			jsonData["pid"] = this.productId;
			jsonData["productCurrentPrice"] = (double)this.productCurrentPrice;
			jsonData["productCount"] = this.productCount;
			jsonData["orderDesc"] = this.orderDesc;
			jsonData["orderStatus"] = (int)this.orderStatus;
			jsonData["orderErrReason"] = this.orderErrReason;
			jsonData["orderEtc"] = this.orderEtc;
			jsonData["signature"] = this.signature;
			jsonData["orderChannel"] = this.orderChannel;
			jsonData["transactionReceipt"] = this.transactionReceipt;
			jsonData["userData"] = this.userData;
			jsonData["userPriceLocaleId"] = this.userPriceLocaleId;
			jsonData["userName"] = this.userName;
			return jsonData;
		}

		private static JsonData getData(JsonData json, string key)
		{
			JsonData result;
			try
			{
				result = json[key];
			}
			catch (Exception ex)
			{
				global::Debug.LogError(new object[]
				{
					ex.Message
				});
				result = null;
			}
			return result;
		}

		public static NtOrderInfo FromJsonData(JsonData json)
		{
			NtOrderInfo ntOrderInfo = new NtOrderInfo();
			ntOrderInfo.orderId = (string)NtOrderInfo.getData(json, "orderId");
			ntOrderInfo.sdkOrderId = (string)NtOrderInfo.getData(json, "sdkOrderId");
			ntOrderInfo.productId = (string)NtOrderInfo.getData(json, "pid");
			ntOrderInfo.productCurrentPrice = float.Parse(json["productCurrentPrice"].ToString());
			ntOrderInfo.productCount = int.Parse(json["productCount"].ToString());
			ntOrderInfo.orderDesc = (string)NtOrderInfo.getData(json, "orderDesc");
			ntOrderInfo.orderStatus = (OrderStatus)int.Parse(json["orderStatus"].ToString());
			ntOrderInfo.orderErrReason = (string)NtOrderInfo.getData(json, "orderErrReason");
			ntOrderInfo.orderEtc = (string)NtOrderInfo.getData(json, "orderEtc");
			JsonData data = NtOrderInfo.getData(json, "signature");
			ntOrderInfo.signature = (string)((data == null) ? string.Empty : data);
			data = NtOrderInfo.getData(json, "orderChannel");
			ntOrderInfo.orderChannel = (string)((data == null) ? string.Empty : data);
			data = NtOrderInfo.getData(json, "transactionReceipt");
			ntOrderInfo.transactionReceipt = (string)((data == null) ? string.Empty : data);
			data = NtOrderInfo.getData(json, "userData");
			ntOrderInfo.userData = (string)((data == null) ? string.Empty : data);
			data = NtOrderInfo.getData(json, "userPriceLocaleId");
			ntOrderInfo.userPriceLocaleId = (string)((data == null) ? string.Empty : data);
			data = NtOrderInfo.getData(json, "userName");
			ntOrderInfo.userName = (string)((data == null) ? string.Empty : data);
			return ntOrderInfo;
		}

		public static NtProduct getProduct(string pId)
		{
			if (NtOrderInfo.productList.ContainsKey(pId))
			{
				return (NtProduct)NtOrderInfo.productList[pId];
			}
			return null;
		}

		public static void regProduct(string pId, string pName, float pPrice, int eRatio, Dictionary<string, string> sdkPids)
		{
			NtOrderInfo.productList.Add(pId, new NtProduct(pId, pName, pPrice, eRatio, sdkPids));
			JsonData jsonData = new JsonData();
			jsonData["pid"] = pId;
			jsonData["productName"] = pName;
			jsonData["productPrice"] = (double)pPrice;
			jsonData["eRatio"] = eRatio;
			JsonData jsonData2 = new JsonData();
			foreach (KeyValuePair<string, string> current in sdkPids)
			{
				jsonData2[current.Key] = current.Value;
			}
			jsonData["sdkPids"] = jsonData2;
			NtOrderInfo.callRawSdkApi("com.netease.ntunisdk.base.OrderInfo", "regProduct", new object[]
			{
				JsonMapper.ToJson(jsonData)
			});
		}

		public static void regProduct(string pId, string pName, float pPrice, int eRatio = 1)
		{
			NtOrderInfo.productList.Add(pId, new NtProduct(pId, pName, pPrice, eRatio));
			NtOrderInfo.callRawSdkApi("com.netease.ntunisdk.base.OrderInfo", "regProduct", new object[]
			{
				pId,
				pName,
				pPrice,
				eRatio
			});
		}

		public static bool hasProduct(string pId)
		{
			return NtOrderInfo.callRawSdkApiReturnBool("com.netease.ntunisdk.base.OrderInfo", "hasProduct", new object[]
			{
				pId
			});
		}

		private static void callRawSdkApi(string className, string apiName, params object[] args)
		{
			NtOrderInfo.log("Unity3D callRawSdkApi" + className + apiName + " calling...");
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(className))
			{
				androidJavaClass.CallStatic(apiName, args);
			}
		}

		private static bool callRawSdkApiReturnBool(string className, string apiName, params object[] args)
		{
			NtOrderInfo.log("Unity3D callRawSdkApi" + className + apiName + " calling...");
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(className))
			{
				bool flag = androidJavaClass.CallStatic<bool>(apiName, args);
				result = flag;
			}
			return result;
		}

		private static void log(string msg)
		{
			global::Debug.Log(new object[]
			{
				msg
			});
		}
	}
}
