using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.Annotations;
using AndroidX.Core.App;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Platforms.Android
{
    [Service(Label = nameof(ScreenOffService))]
    [RequiresApi(Api = (int)BuildVersionCodes.R)]
    public class ScreenOffService : Service
    {
        private static readonly string TypeName = typeof(ScreenOffService).FullName;
        public static readonly string ActionStartScreenOffService = TypeName + ".action.START";

        internal const int NOTIFICATION_ID = 12345678;
        private const string NOTIFICATION_CHANNEL_ID = "screen_off_service_channel_01";
        private const string NOTIFICATION_CHANNEL_NAME = "screen_off_service_channel_name";
        private NotificationManager _notificationManager;

        private bool _isStarted;

        private readonly ScreenOffBroadcastReceiver _screenOffBroadcastReceiver;

        public ScreenOffService()
        {
            _screenOffBroadcastReceiver = Microsoft.Maui.Controls.Application.Current.Handler.MauiContext.Services.GetService<ScreenOffBroadcastReceiver>();
        }

        public override void OnCreate()
        {
            base.OnCreate();

            _notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            RegisterScreenOffBroadcastReceiver();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            UnregisterScreenOffBroadcastReceiver();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            CreateNotificationChannel(); // Elsewhere we must've prompted user to allow Notifications

            if (intent.Action == ActionStartScreenOffService)
            {
                try
                {
                    StartForeground();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to start Screen On/Off foreground svc: " + ex);
                }
            }

            return StartCommandResult.Sticky;
        }

        private void RegisterScreenOffBroadcastReceiver()
        {
            var filter = new IntentFilter();
            filter.AddAction(Intent.ActionScreenOff);
            RegisterReceiver(_screenOffBroadcastReceiver, filter);
        }

        private void UnregisterScreenOffBroadcastReceiver()
        {
            try
            {
                if (_screenOffBroadcastReceiver != null)
                {
                    UnregisterReceiver(_screenOffBroadcastReceiver);
                }
            }
            catch (Java.Lang.IllegalArgumentException ex)
            {
                Console.WriteLine($"Error while unregistering {nameof(ScreenOffBroadcastReceiver)}. {ex}");
            }
        }

        private void StartForeground()
        {
            if (!_isStarted)
            {
                Notification notification = BuildInitialNotification();
                StartForeground(NOTIFICATION_ID, notification);

                _isStarted = true;
            }
        }

        private Notification BuildInitialNotification()
        {
            var intentToShowMainActivity = BuildIntentToShowMainActivity();

            var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                .SetContentTitle("Fisherman App")
                .SetContentText("Running in background")
                //.SetSmallIcon(Resource.Drawable.eip_logo_symbol_yellow) // Android top bar icon and Notification drawer item LHS icon
                //.SetLargeIcon(global::Android.Graphics.BitmapFactory.DecodeResource(Resources, Resource.Drawable.eip_logo_yellow)) // Notification drawer item RHS icon
                .SetContentIntent(intentToShowMainActivity)
                .SetOngoing(true)
                .Build();

            return notification;
        }

        private PendingIntent BuildIntentToShowMainActivity()
        {
            var mainActivityIntent = new Intent(this, typeof(MainActivity));
            //mainActivityIntent.SetAction(Constants.ACTION_MAIN_ACTIVITY);
            mainActivityIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
            //mainActivityIntent.PutExtra(Constants.SERVICE_STARTED_KEY, true);

            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, mainActivityIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            return pendingIntent;
        }

        private void CreateNotificationChannel()
        {
            NotificationChannel chan = new(NOTIFICATION_CHANNEL_ID, NOTIFICATION_CHANNEL_NAME, NotificationImportance.Default)
            {
                LightColor = Microsoft.Maui.Graphics.Color.FromRgba(0, 0, 255, 0).ToInt(),
                LockscreenVisibility = NotificationVisibility.Public
            };

            _notificationManager.CreateNotificationChannel(chan);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }

    [BroadcastReceiver(Name = "com.insitesolutions.fishermanapp.ScreenOffBroadcastReceiver", Label = "ScreenOffBroadcastReceiver", Exported = true)]
    [IntentFilter(new[] { Intent.ActionScreenOff }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class ScreenOffBroadcastReceiver : BroadcastReceiver
    {
        private readonly ILogger<ScreenOffBroadcastReceiver> _logger;

        private PowerManager.WakeLock _wakeLock;

        public ScreenOffBroadcastReceiver()
        {
            _logger = Microsoft.Maui.Controls.Application.Current.Handler.MauiContext.Services.GetService<ILogger<ScreenOffBroadcastReceiver>>();
        }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == Intent.ActionScreenOff)
            {
                AcquireWakeLock();
            }
        }

        private void AcquireWakeLock()
        {
            _wakeLock?.Release();

            WakeLockFlags wakeFlags = WakeLockFlags.Partial;

            PowerManager pm = (PowerManager)global::Android.App.Application.Context.GetSystemService(global::Android.Content.Context.PowerService);
            _wakeLock = pm.NewWakeLock(wakeFlags, typeof(ScreenOffBroadcastReceiver).FullName);
            _wakeLock.Acquire();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _wakeLock?.Release();
        }
    }
}
