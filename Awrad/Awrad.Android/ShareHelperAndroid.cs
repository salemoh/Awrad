using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Awrad.Droid;
using Awrad.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(ShareHelperAndroid))]
namespace Awrad.Droid
{
    class ShareHelperAndroid : IShareHelper
    {
        public async void Share(string imageUrl)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("image/png");

            // Get the URI of the image
            var imageUri = new Uri(imageUrl);
            var imageSource = ImageSource.FromUri(imageUri);
            var handler = new ImageLoaderSourceHandler();
            var context = Xamarin.Forms.Forms.Context;
            var bitmap = await handler.LoadImageAsync(imageSource, context);

            // Get the file name
            var filename = System.IO.Path.GetFileName(imageUrl);
            var path =
                Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads +
                                                                         Java.IO.File.Separator + filename);

            using(var os = new System.IO.FileStream(path.AbsolutePath, System.IO.FileMode.Create))
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, os);
            }


            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(path));

            var intentChooser = Intent.CreateChooser(intent, "Share via");

            context.StartActivity(intentChooser);
        }
    }
}