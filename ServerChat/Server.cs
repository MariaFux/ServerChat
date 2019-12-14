using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ServerChat
{
    public static class Server
    {
        //список клиентов
        public  static List<Client> Clients = new List<Client>();

        //добавление нового клиента
        public static void NewClient(Socket handel)
        {
            try
            {
                Client newClient = new Client(handel); 
                Clients.Add(newClient);
                Console.WriteLine("Новый пользователь соединен: {0}.", handel.RemoteEndPoint);

            }
            catch(Exception e)
            {
                Console.WriteLine("Ошибка с добавлением нового клиента: {0}.", e.Message);
            }
        }

        //завершение работы клиента
        public static void EndClient(Client client)
        {
            try
            {
                client.End();
                Clients.Remove(client);
                Console.WriteLine("Пользователь {0} был отсоединен.", client.UserName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка с отсоединением: {0}.", e.Message);
            }
        }

        //обновление всего чата для всех клиентов
        public static void UpdateAllChats()
        {
            try
            {
                int countClients = Clients.Count;
                for (int i = 0; i < countClients; i++)
                {
                    Clients[i].UpdateChat();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка с обновлением всего чата: {0}.", e.Message);
            }
        }
        //добавление последнего сообщения
        public static void UpdateAllChats(Boolean b)
        {
            try
            {
                int countClients = Clients.Count;
                for (int i = 0; i < countClients; i++)
                {
                    Clients[i].UpdateChat(b);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка с обновлением всего чата: {0}.", e.Message);
            }
        }
        
    }
}