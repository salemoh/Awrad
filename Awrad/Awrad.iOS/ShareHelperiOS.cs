using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Awrad.Helpers;
using Awrad.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(ShareHelperiOS))]
namespace Awrad.iOS
{
    class ShareHelperiOS : IShareHelper
    {
        public void Share(string imageUrl)
        {
            throw new NotImplementedException();
        }
    }
}