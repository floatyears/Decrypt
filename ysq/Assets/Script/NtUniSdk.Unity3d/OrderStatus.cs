using System;

namespace NtUniSdk.Unity3d
{
	public enum OrderStatus
	{
		OS_PREPARING,
		OS_SDK_CHECKING,
		OS_SDK_CHECK_OK,
		OS_SDK_CHECK_ERR,
		OS_GS_CHECKING,
		OS_GS_CHECK_OK,
		OS_GS_CHECK_ERR,
		OS_WRONG_PRODUCT_ID
	}
}
