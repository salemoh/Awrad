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

[assembly: Dependency(typeof(ShareHelperAndroid))]
namespace Awrad.Droid
{
    class ShareHelperAndroid : IShareHelper
    {
        public void Share(string imageUrl)
        {
            // Get the URI of the image
            var imageUri = new Java.Net.URL(imageUrl);

            Bitmap bitmap = BitmapFactory.DecodeStream(imageUri.OpenStream());
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("image/*");
            intent.PutExtra(Intent.ExtraStream, imageUrl);
            var intentChooser = Intent.CreateChooser(intent, "Share via");
            var activity = (MainActivity) Forms.Context;
            activity.StartActivityForResult(intentChooser, 100);
        }
    }
}