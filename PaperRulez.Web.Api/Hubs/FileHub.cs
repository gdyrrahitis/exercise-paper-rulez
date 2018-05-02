namespace PaperRulez.Web.Api.Hubs
{
    using Microsoft.AspNet.SignalR;

    public class FileHub: Hub
    {
        public void Send(string client, string documentName)
        {
            Clients.All.sendFileProcessed(client, documentName);
        }
    }
}