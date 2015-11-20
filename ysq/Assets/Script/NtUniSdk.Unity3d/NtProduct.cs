using System;
using System.Collections.Generic;

namespace NtUniSdk.Unity3d
{
	public class NtProduct
	{
		public string id;

		public string name;

		public float price;

		public int eRatio;

		public Dictionary<string, string> sdkPids;

		public string payChannel;

		public NtProduct(string pId, string pName, float pPrice, int _eRatio)
		{
			this.id = pId;
			this.name = pName;
			this.price = pPrice;
			this.eRatio = _eRatio;
		}

		public NtProduct(string pId, string pName, float pPrice, int _eRatio, Dictionary<string, string> _sdkPids)
		{
			this.id = pId;
			this.name = pName;
			this.price = pPrice;
			this.eRatio = _eRatio;
			this.sdkPids = _sdkPids;
		}
	}
}
