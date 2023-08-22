using System;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private NotificationsData data;

    public NotificationManager Initialize()
    {
        return this;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            ScheduleNotifications();
    }

    private void ScheduleNotifications()
    {
        var now = DateTime.Now;

        foreach (var notification in data.notifications)
        {
            var deliveryTime = now.AddMinutes(notification.deliveryTimeInMinutes);
            var index = data.notifications.FindIndex(x => x == notification);
#if RUBY_FRAMEWORK
            RubyGames.Framework.RubyFramework.SendNotification(notification.title
                , notification.description, deliveryTime, "", null, true);
            
            LogManager.Log($"Notification {index} scheduled for {deliveryTime}");
#else
            LogManager.LogWarning($"Notification {index} is not scheduled !", this);
#endif
        }
    }
}