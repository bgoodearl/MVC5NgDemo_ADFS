using System.Collections.Generic;

namespace MVCDemo.ViewModels.Test
{
    public class ClientInfoViewModel
    {
        public ClientInfoViewModel()
        {
            ClientInfo = new List<InfoItem>();
        }

        public List<InfoItem> ClientInfo { get; private set; }
    }
}