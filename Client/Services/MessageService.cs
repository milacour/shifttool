namespace ShiftTool.Client.Services
{
    public class MessageService
    {
        public event Action<string, bool> OnMessage;

        public void SendMessage(string message, bool isError)
        {
            OnMessage?.Invoke(message, isError);
        }
    }
}

