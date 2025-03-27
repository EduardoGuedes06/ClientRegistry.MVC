

namespace ClientRegistry.Domain
{
    public interface INotificator
    {
        List<Notification.Notification> GetNotifications();
        void Handle(Notification.Notification notificacao);
        bool HaveNotification();
    }
}
