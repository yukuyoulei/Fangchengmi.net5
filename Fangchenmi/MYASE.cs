using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class MyCryptoClass
{
	/**
     * 转换16进制字符串
     **/
	private static byte[] hexStringToByte(String str)
	{
		return Enumerable.Range(0, str.Length)
				 .Where(x => x % 2 == 0)
				 .Select(x => Convert.ToByte(str.Substring(x, 2), 16))
				 .ToArray();
	}

	public static byte[] getBytes(this string content)
	{
		return Encoding.UTF8.GetBytes(content);
	}
	/***
     * @version 1.0 aes-128-gcm 加密
     * @params content 为加密信息 secretKey 为32位的16进制key
     * @return 返回base64编码
     **/
	public static String encryptByAES128Gcm(String key, String plainData)
	{
		Random rand = new Random();
		byte[] bb = new byte[12];
		rand.NextBytes(bb);

		var gcmBlockCipher = new GcmBlockCipher(new AesEngine());
		var parameters = new AeadParameters(
			new KeyParameter(hexStringToByte(key)),
			128, //128 = 16 * 8 => (tag size * 8)
			bb,
			null);
		gcmBlockCipher.Init(true, parameters);

		var data = Encoding.UTF8.GetBytes(plainData);
		var cipherData = new byte[gcmBlockCipher.GetOutputSize(data.Length)];

		var length = gcmBlockCipher.ProcessBytes(data, 0, data.Length, cipherData, 0);
		gcmBlockCipher.DoFinal(cipherData, length);
		byte[] message = new byte[bb.Length + cipherData.Length];
		Array.Copy(bb, message, bb.Length);
		Array.Copy(cipherData, 0, message, bb.Length, cipherData.Length);
		return Convert.ToBase64String(message);
	}

}