using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HistoryKing_client.Interfaces
{
    class AndroidConnecter
    {
        private int PORT = 1388;
        private Socket soc, client;
        private IPEndPoint ipend;
        private Byte[] sendByte = new Byte[1];
        private Byte[] recvByte = new Byte[1];
        public List<Card> playerCardDeckList; 

        public AndroidConnecter(List<Card> lc)
        {
            this.playerCardDeckList = lc;

            Thread t = new Thread(subThread);
            t.Start();
        }

        public AndroidConnecter()
        {
            Thread t = new Thread(subThread);
            t.Start();
        }

        public void subThread() {
            soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ipend = new IPEndPoint(IPAddress.Any, PORT);
            soc.Bind(ipend);
            soc.Listen(5);//동시 접속자 5명
            Console.WriteLine(ipend.Address);

            //SamGookStage1 samgookstage1 = new SamGookStage1();
            //int cardsize = playerCardDeckList.Count;
            int cardsize = 8;

            Console.WriteLine("접속대기중.............");
            client = soc.Accept();
            Console.WriteLine("접속 하였습니다.");

            Byte[] randomtest = {7,4,3,1,2,6,0,5 };

            if (client.Connected)
            {
                sendByte[0] = (byte)cardsize;
                client.Send(sendByte, sendByte.Length, 0);//카드의 총 크기 전송
                Console.WriteLine("카드의 총 크기 = " + (byte)cardsize);

                if(cardsize != 0)//카드의 크기가0값과 같지 않을때 
                {
                    for (int i = 0; i < cardsize; i++)//리스트에 들어있는 카드이름을 전송한다.
                    {
                        Card card = (Card)playerCardDeckList.ElementAt(i);

                        recvByte[0] = randomtest[i];

                        Console.WriteLine(recvByte[0]);

                        client.Send(recvByte);
                    }

                    while (true)
                    {
                        sendByte[0] = 7;
                        client.Send(sendByte, sendByte.Length, 0);//카드받을 준비가 됐다고 전송 
                        Console.WriteLine("카드 받을 준비가 완료 되어 데이터를 전송 했습니다.");
                        client.Receive(recvByte, recvByte.Length, 0);//카드를 받는다.
                        Console.WriteLine("카드를 받았습니다.");
                    }
                }
            }
        }
    }
}