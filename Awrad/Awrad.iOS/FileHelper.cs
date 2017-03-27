using System;
using System.IO;
using Awrad.iOS;
using Awrad.Helpers;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace Awrad.iOS
{
	public class FileHelper : IFileHelper
	{
		public string GetLocalFilePath(string filename)
		{
			string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");
		    string dbPath = Path.Combine(libFolder, filename);


            // Create DB directory if not exist
            if (!Directory.Exists(libFolder))
			{
				Directory.CreateDirectory(libFolder);
			}

            // Copy DB from resources if not exist
		    CopyDbIfNotExist();

			return dbPath;


            void CopyDbIfNotExist()
            {
                //if (!File.Exists(dbPath))
                //{
                    var existingDb = NSBundle.MainBundle.PathForResource("Awrad", "sqlite");
                    File.Copy(existingDb, dbPath);
                //}
            }

        }
    }
}
