using System;
using System.IO;
using Awrad.Droid;
using Awrad.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace Awrad.Droid
{
	public class FileHelper : IFileHelper
	{
		public string GetLocalFilePath(string filename)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			return Path.Combine(path, filename);
		}
	}
}
