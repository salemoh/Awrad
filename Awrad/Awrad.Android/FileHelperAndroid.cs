﻿using System;
using System.IO;
using Android.Content.Res;
using Awrad.Droid;
using Awrad.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelperAndroid))]
namespace Awrad.Droid
{
    public class FileHelperAndroid : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            path = Path.Combine(path, filename);

            // If no DB or DB too small then copy it from assets
            CopyDbIfNotExist(force: true);

            // Return the path to the DB
            return path;

            void CopyDbIfNotExist(bool force = false)
            {
                // Check if file exists or too small
                var copyDB = !File.Exists(path) || (new FileInfo(path).Length < 100);

                if (copyDB || force)
                {
                    // Get an instance of asset manager
                    var assets = Android.App.Application.Context.Assets;

                    using (var br = new BinaryReader(assets.Open(filename)))
                    {
                        using (var bw = new BinaryWriter(new FileStream(path, FileMode.Create)))
                        {
                            byte[] buffer = new byte[2048];
                            int length = 0;
                            while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                bw.Write(buffer, 0, length);
                            }
                        }
                    }
                }

            }
        }
    }
}
