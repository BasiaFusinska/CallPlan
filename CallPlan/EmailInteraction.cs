namespace CallPlan
{
    public class EmailInteraction : IInteraction
    {
        public EmailInteraction(string email)
        {
            Originator = email;
        }
        public string Originator { get; private set; }
    }
}
