using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;


namespace Coconut
{
	public class CoconutHelper
	{
		public CoconutHelper()
		{
			
		}

		public async static Task<ObservableCollection<Coconut>> asyncfetchCoconuts()
		{
			var jsonData = await "https://s3-us-west-2.amazonaws.com/wheremycoconut/coconuts.json".GetJsonAsync<ObservableCollection<Coconut>>();
			return jsonData;

		}

	}
}

