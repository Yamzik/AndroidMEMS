using MEMS.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MEMS
{
	public partial class MainPage : ContentPage
	{
		Stopwatch stopWatch = new Stopwatch();
		Timer timer = new Timer(16.67);
		public string accelerometerData;
		public string receivedData = "T(ms);AX(m/s2);AY(m/s2);AZ(m/s2);GX(rad/s);GY(rad/s);GZ(rad/s)\n";
		public double ax;
		public double ay;
		public double az;
		public double gx;
		public double gy;
		public double gz;
		public double et;
		bool go = false;
		public MainPage()
		{
			SetCulture();
			InitializeComponent();
			Gyroscope.Start(SensorSpeed.Fastest);
			Accelerometer.Start(SensorSpeed.Fastest);
			timer.Elapsed += OnTimerElapsed;
			timer.AutoReset = true;
			//Accelerometer.Start(sensorSpeed: SensorSpeed.Fastest);
			Accelerometer.ReadingChanged += (sender, args) =>
			{
				if (go)
				{
					TimeEapsed.Text = $"T: {stopWatch.ElapsedMilliseconds} ms";
					ax = Math.Round(args.Reading.Acceleration.X * 9.81, 2);
					ay = Math.Round(args.Reading.Acceleration.Y * 9.81, 2);
					az = Math.Round(args.Reading.Acceleration.Z * 9.81, 2);
					accXResult.Text = $"X: {ax} m/s2";
					accYResult.Text = $"Y: {ay} m/s2";
					accZResult.Text = $"Z: {az} m/s2";
				}
			};
			Gyroscope.ReadingChanged += (sender, args) =>
			{
				if (go)
				{
					gx = Math.Round(args.Reading.AngularVelocity.X, 2);
					gy = Math.Round(args.Reading.AngularVelocity.Y, 2);
					gz = Math.Round(args.Reading.AngularVelocity.Z, 2);
					gyrXResult.Text = $"X: {gx} rad/s";
					gyrYResult.Text = $"Y: {gy} rad/s";
					gyrZResult.Text = $"Z: {gz} rad/s";
				}
			};
		}
		public void SetCulture()
		{
			NumberFormatInfo nfi = new NumberFormatInfo();
			nfi.NumberDecimalSeparator = ".";
		}
		private void OnTimerElapsed(Object source, ElapsedEventArgs e)
		{
			receivedData += $"{stopWatch.ElapsedMilliseconds};{ax};{ay};{az};{gx};{gy};{gz}\n";
		}
		protected override void OnAppearing()
		{

		}
		private void Button_Clicked(object sender, EventArgs e)
		{
			if (go)
			{
				stopWatch.Stop();
				stopWatch.Reset();
				timer.Stop();
				receivedData = "T(ms);AX(m/s2);AY(m/s2);AZ(m/s2);GX(rad/s);GY(rad/s);GZ(rad/s)";
				go = false;
				TimeEapsed.Text = $"T: 0 ms";
				accXResult.Text = $"X: ";
				accYResult.Text = $"Y: ";
				accZResult.Text = $"Z: ";
				gyrXResult.Text = $"X: ";
				gyrYResult.Text = $"Y: ";
				gyrZResult.Text = $"Z: ";
			}
			else
			{
				go = true;
				stopWatch.Start();
				timer.Start();
			}
		}

		private void Save_Clicked(object sender, EventArgs e)
		{
			DependencyService.Get<IFileService>().CreateFile(receivedData);
		}
		/*void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
{
var data = e.Reading;
accelerometerData = $"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}";
// Process Acceleration X, Y, and Z
}*/
	}
}
