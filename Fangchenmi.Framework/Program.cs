using System;
class Program
{
	static void Main(string[] args)
	{
		Fangchenmi.OnDoFangchenmi(new
			{
				ai = "100000000000000001",
				name = "某一一",
				idNum = "110000190101010001",
			}, Fangchenmi.EType.Check, "ctaM7Y");

		//Fangchenmi.OnDoFangchenmi("100000000000000001", Fangchenmi.EType.Query, "ctaM7Y");

		/*Fangchenmi.OnDoFangchenmi(new
		{
			collections = new object[]
			{
				new
				{
					no = 1,
					pi = "1fffbkmd9ebtwi7u7f4oswm9li6twjydqs7qjv",
					si = "111",
					bt = 0,
					ot = ApiDateTime.SecondsFrom19700101ms(),
					ct = 0,
					//di = "123333",
				}
			},
		}, Fangchenmi.EType.Collect, "ctaM7Y");*/

		Console.ReadKey();
	}
}
