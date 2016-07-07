using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;


namespace Coconut
{
	public partial class CoconutMap : ContentPage
	{


		ObservableCollection<Coconut> coconutMasterList = new ObservableCollection<Coconut>();
		public CoconutMap()
		{
			
			InitializeComponent();


		}

		protected async override void OnAppearing()
		{
			coconutsListView.IsRefreshing = true;
			coconutMasterList = await CoconutHelper.asyncfetchCoconuts();


			List<Tuple<Coconut, double>> coconutDistance = new List<Tuple<Coconut, double>>();

			var currentLocation = await getCurrentLocation();

			coconutMap.MoveToRegion(
		MapSpan.FromCenterAndRadius(new Position(currentLocation.Latitude, currentLocation.Longitude), Distance.FromMiles(1)));
			
			foreach (var coconut in coconutMasterList)
			{
				GeoCoordinate pin1 = new GeoCoordinate(coconut.lat, coconut.lng);
				double distanceBetween = pin1.GetDistanceTo(currentLocation);
				coconutDistance.Add(new Tuple<Coconut, double>(coconut, distanceBetween));
			}

			coconutDistance.Sort((x, y) => y.Item2.CompareTo(x.Item2));

			List<Coconut> list = coconutDistance.Select(t => t.Item1).ToList();

			list.Reverse();

			coconutsListView.ItemsSource = list;
			coconutsListView.IsRefreshing = false;
			addPinstoMap();




			base.OnAppearing();
		}


		async Task<GeoCoordinate> getCurrentLocation()
		{
			try
			{
				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 50;

				var position = await locator.GetPositionAsync(timeoutMilliseconds: 1000);

				Debug.WriteLine("Position Status: {0}", position.Timestamp);
				Debug.WriteLine("Position Latitude: {0}", position.Latitude);
				Debug.WriteLine("Position Longitude: {0}", position.Longitude);

				return new GeoCoordinate() { Latitude = position.Latitude, Longitude = position.Longitude};
			}

			catch (Exception ex)
			{
				await DisplayAlert("Whoa", "You didn't allow location services, go to Settings and enable them!!", "Ok!");
				Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
			}

			return null;
		}
		void addPinstoMap()
		{
			foreach(var coconut in coconutMasterList)
			{
				
				var position = new Position(coconut.lat, coconut.lng); // Latitude, Longitude
				var pin = new Pin
				{
					Type = PinType.Place,
					Position = position,
					Label = coconut.name,
					Address = coconut.type
				};
				coconutMap.Pins.Add(pin);
	
			}
		}


		void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var listView = (ListView)sender;
			var coconut = listView.SelectedItem as Coconut;

			coconutMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(coconut.lat, coconut.lng),Distance.FromMiles(.5)));
		}


	}
}

