using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MEMS.Droid;
using MEMS.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(FileService))]
namespace MEMS.Droid
{
	public class FileService : IFileService
	{
		public string GetRootPath()
		{
			return Application.Context.GetExternalFilesDir(null).ToString();
		}
		public void CreateFile(string content)
		{
			var filename = "data.txt";
			var destination = Path.Combine(GetRootPath(), filename);

			File.WriteAllText(destination, content);
		}
	}
}