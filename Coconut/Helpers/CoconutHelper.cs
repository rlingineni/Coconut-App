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
			var jsonData = await "http://www.heyraviteja.com/Coconut-App/coconuts.json".GetJsonAsync<ObservableCollection<Coconut>>();
			return jsonData;

		}

	}
}

