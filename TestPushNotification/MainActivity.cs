using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Gcm.Client;
using System;

namespace TestPushNotification
{
    public static class NotificationConstants
    {
        public const string SenderId = "711963198034";
        public const string ListenConnectionString = "Endpoint=sb://testnotificationnamespace2.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=PWSvk81ydCwCskdL9e4hun8dZfwzyKt80hthiwkZsJQ=";
        public const string HubName = "TestNotificationHub";
    }

    [Activity(Label = "TestPushNotification", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        void RegisterGCM()
        {
            try
            {
                // Check to ensure everything's set up right
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);

                // Register for push notifications
                System.Diagnostics.Debug.WriteLine("Registering...");
                GcmClient.Register(this, NotificationConstants.SenderId);
            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e.Message, "Error");
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            MainActivity.Instance = this;
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            this.RegisterGCM();

        }

        // Return the current activity instance.
        public static MainActivity Instance { get; private set; }

        private void CreateAndShowDialog(string message, string title)
        {
            MainActivity.Instance.RunOnUiThread(() =>
            {
                AlertDialog.Builder dlg = new AlertDialog.Builder(MainActivity.Instance);
                AlertDialog alert = dlg.Create();
                alert.SetTitle(title);
                alert.SetButton("Ok", delegate
                {
                    alert.Dismiss();
                });
                alert.SetMessage(message);
                alert.Show();
            });
        }
    }
}

