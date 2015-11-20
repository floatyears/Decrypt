using LitJson;
using System;
using System.Collections.Generic;

public class BMUrls
{
	public Dictionary<string, string> downloadUrls;

	public Dictionary<string, string> outputs;

	public BuildPlatform bundleTarget = BuildPlatform.Standalones;

	public bool downloadFromOutput;

	public BMUrls()
	{
		this.downloadUrls = new Dictionary<string, string>
		{
			{
				"WebPlayer",
				string.Empty
			},
			{
				"Standalones",
				string.Empty
			},
			{
				"IOS",
				string.Empty
			},
			{
				"Android",
				string.Empty
			},
			{
				"WP8",
				string.Empty
			}
		};
		this.outputs = new Dictionary<string, string>
		{
			{
				"WebPlayer",
				string.Empty
			},
			{
				"Standalones",
				string.Empty
			},
			{
				"IOS",
				string.Empty
			},
			{
				"Android",
				string.Empty
			},
			{
				"WP8",
				string.Empty
			}
		};
	}

	public string GetInterpretedDownloadUrl(BuildPlatform platform)
	{
		return BMUtility.InterpretPath(this.downloadUrls[platform.ToString()], platform);
	}

	public string GetInterpretedOutputPath(BuildPlatform platform)
	{
		return BMUtility.InterpretPath(this.outputs[platform.ToString()], platform);
	}

	public static string SerializeToString(BMUrls urls)
	{
		return JsonMapper.ToJson(urls);
	}
}
