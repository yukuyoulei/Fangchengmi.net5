using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class InitValueDefs
{
	public const string FangChenMi_appid = "358479a046884505a46643744129d2e6";
	public const string FangChenMi_secretkey = "0dfc53753f56f131eef4901ea0b2ff16";
	public const string FangChenMi_bizid = "1101999999";

	public static long SecondsFrom19700101ms()
	{
		var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
		return (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
	}
}
public static class Fangchenmi
{
	public enum EType
	{
		Check,
		Query,
		Collect,
	}
	private static string[] normalUrls = { "https://api.wlc.nppa.gov.cn/idcard/authentication/check"
			, "http://api2.wlc.nppa.gov.cn/idcard/authentication/query"
			, "http://api2.wlc.nppa.gov.cn/behavior/collection/loginout" };
	private static string[] testUrls = { "https://wlc.nppa.gov.cn/test/authentication/check/"
			, "https://wlc.nppa.gov.cn/test/authentication/query/"
			, "https://wlc.nppa.gov.cn/test/collection/loginout/"};

	public static string OnDoFangchenmi(object args, EType eType, string testcode = "")
	{
		var url = normalUrls[(int)eType];
		if (!string.IsNullOrEmpty(testcode))
			url = $"{testUrls[(int)eType]}{testcode}";
		var data = "";
		var strjson = "";
		if (eType == EType.Query)
		{
			strjson = "ai=" + args;
		}
		else
		{
			strjson = Newtonsoft.Json.JsonConvert.SerializeObject(args);
			Console.WriteLine($"<<< {strjson}");
			data = MyCryptoClass.encryptByAES128Gcm(InitValueDefs.FangChenMi_secretkey, strjson);
			data = Newtonsoft.Json.JsonConvert.SerializeObject(new { data = data });
			Console.WriteLine($"<<< {data}");
		}
		var timestamps = InitValueDefs.SecondsFrom19700101ms().ToString();
		var dic = new Dictionary<string, string>();
		dic["timestamps"] = timestamps;
		dic["appId"] = InitValueDefs.FangChenMi_appid;
		dic["bizId"] = InitValueDefs.FangChenMi_bizid;
		var res = "";
		if (eType == EType.Query)
		{
			dic["ai"] = args.ToString();
			res = AWebUtils.OnWebRequestGet($"{url}?{strjson}", new Dictionary<string, string>()
				{
					{ "appId", InitValueDefs.FangChenMi_appid },
					{ "bizId", InitValueDefs.FangChenMi_bizid},
					{ "timestamps", timestamps },
					{ "sign", GetSign(data, dic) },
				});
		}
		else
			res = AWebUtils.OnWebRequestPost(url, data, "application/json; charset=utf-8", new Dictionary<string, string>()
			{
				{ "appId", InitValueDefs.FangChenMi_appid },
				{ "bizId", InitValueDefs.FangChenMi_bizid},
				{ "timestamps", timestamps },
				{ "sign", GetSign(data, dic) },
			});
		Console.WriteLine($"Fangchenmi res {res}");
		return res;
	}

	private static string GetSign(string data, Dictionary<string, string> dic)
	{
		var keys = dic.Keys.ToList();
		keys.Sort();
		var data1 = "";
		foreach (var k in keys)
		{
			data1 += k + dic[k];
		}
		data1 += data;
		data1 = InitValueDefs.FangChenMi_secretkey + data1;
		Console.WriteLine($"data1 key-values {data1}");
		return EasyEncryption.SHA.ComputeSHA256Hash(data1);
	}

}
