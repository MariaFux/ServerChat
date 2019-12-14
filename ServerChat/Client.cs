using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace ServerChat
{
    public class Client
    {
        private string _userName;
        private Socket _handler;
        private Thread _userThread;

        //конструктор принимающий клиента
        public Client(Socket socket)
        {
            _handler = socket;
            _userThread = new Thread(listner);
            _userThread.IsBackground = true;
            _userThread.Start();
        }

        //возвращает имя клиента
        public string UserName
        {
            get
            {
                return _userName;
            }
        }

        //слушатель, если от клиента что-то пришло, то отправляет команду сокету
        private void listner()
        {
            while(true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRec = _handler.Receive(buffer);
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRec);
                    handleComand(data);
                }
                catch
                {
                    Server.EndClient(this);
                    return;
                }
            }
        }

        //закрытие сокета и завершение потока (thread) клиента
        public void End()
        {
            try
            {
                _handler.Close();
                try
                {
                    _userThread.Abort();
                }
                catch { }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка с завершением: {0}.", e.Message);
            }
        }

        //команда сокету
        private void handleComand(string data)
        {
            if (data.Contains("#setname"))
            {
                _userName = data.Split('&')[1];
                UpdateChat();
                return;
            }
            if(data.Contains("#newmsg"))
            {
                string message = data.Split('&')[1];
                ChatController.AddMessage(_userName, message);
                return;
            }
        }

        //обновление всего чата для всех клиентов
        public void UpdateChat()
        {
            Send(ChatController.GetChat());
        }

        //отправляет последнее написанное сообщений
        public void UpdateChat(Boolean b)
        {
            string data = "#updatechat&";
            data += String.Format("{0}~{1}|", ChatController.Chat[ChatController.Chat.Count-1].userName, ChatController.Chat[ChatController.Chat.Count - 1].data);
            Send(data);
        }

        //отправляет сообщение клиенту
        public void Send(string command)
        {
            try
            {
                int byteSent = _handler.Send(Encoding.UTF8.GetBytes(command));
                if (byteSent > 0)
                {
                    Console.WriteLine("Succes!");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Ошибка с отправкой команды: {0}.", e.Message);
                Server.EndClient(this);
            }
        }
    }
}
