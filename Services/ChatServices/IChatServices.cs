using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Services.ChatServices.ChatServices;

namespace Services.ChatServices
{
    public interface IChatServices
    {
        public IAsyncEnumerable<string> SendMassage(SendMassegeChatModel request);

        public Task<ResultServices> NewChat(string userId);
    }
}