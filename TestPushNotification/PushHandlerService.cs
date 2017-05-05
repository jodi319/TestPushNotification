using Android.App; 
using Android.Content;
using Android.Util;
using Gcm.Client;
using System;
using System.Collections.Generic;
using System.Text;
using WindowsAzure.Messaging;
using Android.Support.V4.App;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using Android.Media;

namespace TestPushNotification
{
    [Service]
    public class PushHandlerService : GcmServiceBase
    {
        public static string RegistrationID { get; private set; }
        private NotificationHub Hub { get; set; }

        public PushHandlerService() : base(NotificationConstants.SenderId)
        {
        }

        public static string RegistrationId { get; set; }
        protected override void OnRegistered(Context context, string registrationId)
        {
            //Log.Verbose("PushHandlerBroadcastReceiver", "GCM Registered: " + registrationId);
            RegistrationID = registrationId;

            this.Hub = new NotificationHub(NotificationConstants.HubName, NotificationConstants.ListenConnectionString, context);

            try
            {
                this.Hub.UnregisterAll(registrationId);
            }
            catch (Exception ex)
            {

                Log.Error("NotificationTest", ex.Message);
            }

             var tags = new List<string>() { };

            try
            {
                var hubRegistration = this.Hub.Register(registrationId, tags.ToArray());
            }
            catch (Exception ex)
            {

                Log.Error("NotificationTest", ex.Message);
            }



            //var push = TodoItemManager.DefaultManager.CurrentClient.GetPush();

            //MainActivity.CurrentActivity.RunOnUiThread(() => Register(push, null));
        }

        //public async void Register(Microsoft.WindowsAzure.MobileServices.Push push, IEnumerable<string> tags)
        //{
        //    try
        //    {
        //        const string templateBodyGCM = "{\"data\":{\"message\":\"$(messageParam)\"}}";

        //        JObject templates = new JObject();
        //        templates["genericMessage"] = new JObject
        //{
        //    {"body", templateBodyGCM}
        //};

        //        await push.RegisterAsync(RegistrationID, templates);
        //        Log.Info("Push Installation Id", push.InstallationId.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //        Debugger.Break();
        //    }
        //}

        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info("PushHandlerBroadcastReceiver", "GCM Message Received!");

            // To concatenate all the content we have inside "intent"
            var msg = new StringBuilder();

            // We are looping through the "Extra's"
            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                    msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
            }

            ////Store the message
            //var prefs = GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            //var edit = prefs.Edit();
            //edit.PutString("last_msg", msg.ToString());
            //edit.Commit();

            string message = intent.Extras.GetString("message");
            if (string.IsNullOrEmpty(message))
            {
                this.createNotification("Unknown message details", msg.ToString());
            }
            else
            {
                this.createNotification("New message!", message);
            }
            //if (!string.IsNullOrEmpty(msg2))
            //{
            //    this.createNotification("New message!", msg2);;
            //    return;
            //}

            //createNotification("Unknown message details", msg.ToString());
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Error("PushHandlerBroadcastReceiver", "Unregistered RegisterationId : " + registrationId);
        }
         
        protected override void OnError(Context context, string errorId)
        {
            Log.Error("PushHandlerBroadcastReceiver", "GCM Error: " + errorId);
        }
         
        void createNotification(string title, string desc)
        {
            //Create notification
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            //Create an intent to show ui
            var uiIntent = new Intent(this, typeof(MainActivity));

            var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);
            notification.Flags = NotificationFlags.AutoCancel;
            notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));
            notificationManager.Notify(1, notification);
            this.CreateAndShowDialog(desc, title);


            ////Use Notification Builder
            //NotificationCompat.Builder builder = new NotificationCompat.Builder(this);

            ////Create the notification
            ////we use the pending intent, passing our ui intent over which will get called
            ////when the notification is tapped.
            //var notification = builder.SetContentIntent(PendingIntent.GetActivity(this, 0, uiIntent, 0))
            //        .SetSmallIcon(Android.Resource.Drawable.SymActionEmail)
            //        .SetTicker(title)
            //        .SetContentTitle(title)
            //        .SetContentText(desc)

            //        //Set the notification sound
            //        .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))

            //        //Auto cancel will remove the notification once the user touches it
            //        .SetAutoCancel(true).Build();

            ////Show the notification
            //notificationManager.Notify(1, notification);
        }

        private void CreateAndShowDialog(string message, string title)
        {
            MainActivity.Instance.RunOnUiThread(() => {
                AlertDialog.Builder dlg = new AlertDialog.Builder(MainActivity.Instance);
                AlertDialog alert = dlg.Create();
                alert.SetTitle(title);
                alert.SetButton("Ok", delegate {
                    alert.Dismiss();
                });
                alert.SetMessage(message);
                alert.Show();
            });
        }


    }
}