using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using SuperWebSocket;
//using SuperWebSocket.SubProtocol;
using System.Runtime.Serialization.Formatters.Binary;

namespace HistoryKing_client
{
    class UserInfo
    {
        public string userIP
        {
            get;
            set;
        }
        public string userID
        {
            get;
            set;
        }
        public int joinRoomNum
        {
            get;
            set;
        }

        public bool roomMaster
        {
            get;
            set;
        }
    }
}
