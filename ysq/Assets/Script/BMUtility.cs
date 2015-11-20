using ICSharpCode.SharpZipLib.GZip;
using LitJson;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class BMUtility
{
	public static void Swap<T>(ref T a, ref T b)
	{
		T t = a;
		a = b;
		b = t;
	}

	public static string InterpretPath(string origPath, BuildPlatform platform)
	{
		MatchCollection matchCollection = Regex.Matches(origPath, "\\$\\((\\w+)\\)");
		foreach (Match match in matchCollection)
		{
			string value = match.Groups[1].Value;
			origPath = origPath.Replace("$(" + value + ")", BMUtility.EnvVarToString(value, platform));
		}
		return origPath;
	}

	private static string EnvVarToString(string varString, BuildPlatform platform)
	{
		switch (varString)
		{
		case "DataPath":
			return Application.dataPath;
		case "PersistentDataPath":
			return Application.persistentDataPath;
		case "StreamingAssetsPath":
			return Application.streamingAssetsPath;
		case "Platform":
			return platform.ToString();
		}
		global::Debug.LogError(new object[]
		{
			"Cannot solve enviroment var " + varString
		});
		return string.Empty;
	}

	public static T LoadJsonFile<T>(string path)
	{
		if (!File.Exists(path))
		{
			return default(T);
		}
		TextReader textReader = null;
		try
		{
			textReader = new StreamReader(path);
		}
		catch (IOException)
		{
			return default(T);
		}
		T result = JsonMapper.ToObject<T>(textReader.ReadToEnd());
		textReader.Close();
		return result;
	}

	public static void SaveJsonFile<T>(T data, string path)
	{
		TextWriter textWriter = null;
		try
		{
			textWriter = new StreamWriter(path);
		}
		catch (IOException)
		{
			global::Debug.LogError(new object[]
			{
				"Cannot write to " + path
			});
			return;
		}
		string value = JsonFormatter.PrettyPrint(JsonMapper.ToJson(data));
		textWriter.Write(value);
		textWriter.Flush();
		textWriter.Close();
	}

	public static string GetFileMD5Internal(string fileName)
	{
		byte[] buffer = File.ReadAllBytes(fileName);
		return BMUtility.GetFileMD5Internal(buffer);
	}

	public static string GetFileMD5Internal(byte[] buffer)
	{
		MD5 mD = new MD5CryptoServiceProvider();
		byte[] array = mD.ComputeHash(buffer);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	public static byte[] AESDecryptAndBZip2DeCompression(string path)
	{
		byte[] buffer = File.ReadAllBytes(path);
		return BMUtility.AESDecryptAndBZip2DeCompression(buffer);
	}

	public static byte[] AESDecryptAndBZip2DeCompression(byte[] buffer)
	{
		byte[] bytes = CryptHelper.AESDecrypt(buffer);
		buffer = BMUtility.BZip2DeCompression(bytes);
		return buffer;
	}

	public static byte[] BZip2DeCompression(byte[] bytes)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream(bytes))
		{
			using (GZipInputStream gZipInputStream = new GZipInputStream(memoryStream))
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					byte[] array = new byte[4096];
					int count;
					while ((count = gZipInputStream.Read(array, 0, array.Length)) != 0)
					{
						memoryStream2.Write(array, 0, count);
					}
					result = memoryStream2.ToArray();
				}
			}
		}
		return result;
	}

	public static void BZip2Compression(string path, byte[] bytes)
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (GZipOutputStream gZipOutputStream = new GZipOutputStream(memoryStream))
			{
				gZipOutputStream.Write(bytes, 0, bytes.Length);
				gZipOutputStream.Finish();
				bytes = memoryStream.ToArray();
				gZipOutputStream.Close();
				memoryStream.Close();
			}
		}
		bytes[4] = 0;
		bytes[5] = 0;
		bytes[6] = 0;
		bytes[7] = 0;
		File.WriteAllBytes(path, bytes);
	}
}
