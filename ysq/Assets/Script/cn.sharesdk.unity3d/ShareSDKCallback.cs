using System;
using UnityEngine;

namespace cn.sharesdk.unity3d
{
	public class ShareSDKCallback : MonoBehaviour
	{
		private void _callback(string data)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					AndroidUtils.getInstance().onActionCallback(data);
				}
			}
		}
	}
}
