namespace CallPlan
{
    public class CallInteraction : IInteraction
    {
        public CallInteraction(string phoneNumber)
        {
            Originator = phoneNumber;
        }
        public string Originator { get; private set; }
    }
}
