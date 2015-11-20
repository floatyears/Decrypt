using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class CryptHelper
{
	private static byte[] _key1 = new byte[]
	{
		209,
		189,
		68,
		63,
		232,
		239,
		208,
		62,
		110,
		79,
		252,
		62,
		31,
		130,
		114,
		63
	};

	private static string Key
	{
		get
		{
			return "magicgame@)!$@PM";
		}
	}

	public static string AESEncrypt(string plainText)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		byte[] inArray = CryptHelper.AESEncrypt(bytes);
		return Convert.ToBase64String(inArray);
	}

	public static byte[] AESEncrypt(byte[] inputByteArray)
	{
		SymmetricAlgorithm symmetricAlgorithm = Rijndael.Create();
		symmetricAlgorithm.Key = Encoding.UTF8.GetBytes(CryptHelper.Key);
		symmetricAlgorithm.IV = CryptHelper._key1;
		byte[] result = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, symmetricAlgorithm.CreateEncryptor(), CryptoStreamMode.Write))
			{
				cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
				cryptoStream.FlushFinalBlock();
				result = memoryStream.ToArray();
			}
		}
		return result;
	}

	public static string AESDecrypt(string showText)
	{
		byte[] cipherText = Convert.FromBase64String(showText);
		byte[] bytes = CryptHelper.AESDecrypt(cipherText);
		return Encoding.UTF8.GetString(bytes);
	}

	public static byte[] AESDecrypt(byte[] cipherText)
	{
		SymmetricAlgorithm symmetricAlgorithm = Rijndael.Create();
		symmetricAlgorithm.Key = Encoding.UTF8.GetBytes(CryptHelper.Key);
		symmetricAlgorithm.IV = CryptHelper._key1;
		byte[] result = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Write))
			{
				cryptoStream.Write(cipherText, 0, cipherText.Length);
				cryptoStream.FlushFinalBlock();
				result = memoryStream.ToArray();
			}
		}
		return result;
	}
}
