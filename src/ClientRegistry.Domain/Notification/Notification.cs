namespace ClientRegistry.Domain.Notification
{
    public class Notification
    {
        public Notification(string mensage)
        {
            Mensage = mensage;
        }

        public string Mensage { get; }
    }
}
