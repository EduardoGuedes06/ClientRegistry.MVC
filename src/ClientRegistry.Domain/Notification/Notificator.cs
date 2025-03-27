namespace ClientRegistry.Domain.Notification
{
    public class Notificator : INotificator
    {
        private List<Notification> _notificacoes;

        public Notificator()
        {
            _notificacoes = new List<Notification>();
        }

        public void Handle(Notification notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        public List<Notification> GetNotifications()
        {
            return _notificacoes;
        }

        public bool HaveNotification()
        {
            return _notificacoes.Any();
        }
    }
}
