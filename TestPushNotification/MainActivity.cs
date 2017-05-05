using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Gcm.Client;

namespace TestPushNotification
{
    [Activity(Label = "TestPushNotification", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
       // void Register =

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}

