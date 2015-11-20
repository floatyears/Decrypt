using System;
using System.Collections;

namespace cn.sharesdk.unity3d
{
	public delegate void ShareResultEvent(ResponseState state, PlatformType type, Hashtable shareInfo, Hashtable error, bool end);
}
