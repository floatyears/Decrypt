using System;

public class BMConfiger
{
	public bool compress = true;

	public bool deterministicBundle;

	public string bundleSuffix = "assetBundle";

	public string buildOutputPath = string.Empty;

	public bool useCache = true;

	public bool useCRC;

	public int downloadThreadsCount = 1;

	public int downloadRetryTime = 2;

	public int bmVersion;
}
