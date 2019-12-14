using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChat
{
    public static class ChatController
    {
        //максимальное количество сообщений
        private const int _maxMessage = 100;
        //список сообщений
        public static List<message> Chat = new List<message>();

        //структура класса сообщений (сообщения, новый тип данных)
        public struct message
        {
            public string userName;
            public string data;
            public message(string name, string msg)
            {
                userName = name;
                data = msg;
            }
        }

        //добавление сообщений в список
        public static void AddMessage(string userName, string msg)
        {
            try
            {
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(msg))
                {
                    return;
                }
                int countMessages = Chat.Count;
                if (countMessages > _maxMessage)
                {
                    ClearChat();
                }
                message newMessage = new message(userName, msg);
                Chat.Add(newMessage);
                Console.WriteLine("Новое сообщение от {0}.", userName);
                Server.UpdateAllChats(true);
            }
            catch(Exception e)
            {
                Console.WriteLine("Ошибка с  добавлением сообщения: {0}.", e.Message);
            }
        }

        //очиститка чата
        public static void ClearChat()
        {
            Chat.Clear();
        }

        //получение всех сообщений чат
        public static string GetChat()
        {
            try
            {
                string data = "#updatechat&";
                int countMessages = Chat.Count;
                if (countMessages <= 0)
                {
                    return string.Empty;
                }
                for (int i = 0; i < countMessages; i++)
                {
                    data += String.Format("{0}~{1}|", Chat[i].userName, Chat[i].data);
                }
                return data;
            }
            catch(Exception e)
            {
                Console.WriteLine("Ошибка с получением чата: {0}.", e.Message);
                return string.Empty;
            }
        }
    }
}
