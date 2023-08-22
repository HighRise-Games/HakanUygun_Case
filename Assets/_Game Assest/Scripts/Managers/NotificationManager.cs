using System;
using UnityEngine;

[DefaultExecutionOrder(int.MinValue)]
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
        else
            CancelNotifications();
    }
    
    private void ScheduleNotifications()
    {
        var now = DateTime.Now;

        foreach (var notification in data.notifications)
        {
            var deliveryTime = now.AddMinutes(notification.deliveryTimeInMinutes);
            var index = data.notifications.FindIndex(x => x == notification);
            
#if RUBY_FRAMEWORK_ADDON
            RubyGames.Framework.RubyFramework.SendNotification(notification.title, notification.description,
                deliveryTime);
            
            LogManager.Log($"Notification {index} scheduled for {deliveryTime}");
#endif
        }
    }

    private void CancelNotifications()
    {
#if RUBY_FRAMEWORK_ADDON
        RubyGames.Framework.RubyFramework.CancelAllNotifications();
        
        LogManager.Log($"All Notifications Cancelled !");
#endif
    }
}