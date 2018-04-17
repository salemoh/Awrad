using System;
using System.IO;
using Awrad.iOS;
using Awrad.Helpers;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelperiOS))]
namespace Awrad.iOS
{
	public class FileHelperiOS : IFileHelper
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
		    CopyDbIfNotExist(force: true);

			return dbPath;


            void CopyDbIfNotExist(bool force = false)
            {
                // In development we always deploy the asset DB in case of updates
                if (!File.Exists(dbPath) || force)
                {
                    var existingDb = NSBundle.MainBundle.PathForResource("Awrad", "sqlite");
                    File.Copy(existingDb, dbPath, true);
                }
            }

        }
    }
}
