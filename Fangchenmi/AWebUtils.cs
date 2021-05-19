using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public static class AWebUtils
{
	public static string OnWebRequestGet(string url, Dictionary<string, string> heads = null)
	{
		try
		{
			ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
			Console.WriteLine("OnWebRequestGet " + url);
			var req = (HttpWebRequest)HttpWebRequest.Create(url);
			if (heads != null)
			{
				foreach (var kv in heads)
				{
					Console.WriteLine($"header : {kv.Key} = {kv.Value}");
					req.Headers.Add(kv.Key, kv.Value);
				}
			}
			using (WebResponse wr = req.GetResponse())
			{
				return new StreamReader(wr.GetResponseStream(), Encoding.UTF8).ReadToEnd();
			}
		}
		catch (System.Net.WebException ex)
		{
			string result = string.Empty;
			//响应流
			var mResponse = ex.Response as HttpWebResponse;
			var responseStream = mResponse.GetResponseStream();
			if (responseStream != null)
			{
				var streamReader = new StreamReader(responseStream, Encoding.GetEncoding(65001));
				//获取返回的信息
				result = streamReader.ReadToEnd();
				streamReader.Close();
				responseStream.Close();
			}
			Console.WriteLine($"{url} 返回错误信息" + result);
			return result;
		}
	}
	public static string OnWebRequestPost(string url, string body, string ContentType, Dictionary<string, string> dic)
	{
		ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
		byte[] bs = Encoding.UTF8.GetBytes(body);
		var req = (HttpWebRequest)HttpWebRequest.Create(url);
		req.Method = "POST";
		req.ContentType = ContentType;
		req.ContentLength = bs.Length;
		req.ProtocolVersion = HttpVersion.Version10;
		foreach (var kv in dic)
		{
			req.Headers.Add(kv.Key, kv.Value);
		}
		try
		{
			using (Stream reqStream = req.GetRequestStream())
			{
				reqStream.Write(bs, 0, bs.Length);
				using (WebResponse wr = req.GetResponse())
				{
					return new StreamReader(wr.GetResponseStream(), Encoding.UTF8).ReadToEnd();
				}
			}
		}
		catch (System.Net.WebException ex)
		{
			if (ex.Response != null)
			{
				string result = string.Empty;
				//响应流
				var mResponse = ex.Response as HttpWebResponse;
				var responseStream = mResponse.GetResponseStream();
				if (responseStream != null)
				{
					var streamReader = new StreamReader(responseStream, Encoding.GetEncoding(65001));
					//获取返回的信息
					result = streamReader.ReadToEnd();
					streamReader.Close();
					responseStream.Close();
				}
				Console.WriteLine($"{url} 返回错误信息" + result);
				return result;
			}
			else
			{
				Console.WriteLine($"{url} 返回错误信息" + ex);
				return ex.Message;
			}
		}
	}
}
