using DatabaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using WcfService;

namespace TodoConsoleApplication
{
    public class ToDoUtility
    {
        #region Abraham
        /// <summary>
        /// Den här metoden lägger anropar webbservicens AddToDo för att lägga till en ToDo objekt till databasen.
        /// </summary>
        /// <param name="toDo">ToDo objekt</param>
        /// <param name="uri">URI till webbservicen</param>
        public static void AddToDo(ToDo toDo, string uri)
        {
            WebServiceHost host = new WebServiceHost(typeof(Service1), new Uri(uri));
            try
            {
                ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IService1), new WebHttpBinding(), "");
                host.Open();
                using (ChannelFactory<IService1> cf = new ChannelFactory<IService1>(new WebHttpBinding(), uri))
                {
                    cf.Endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
                    IService1 channel = cf.CreateChannel();

                    channel.AddToDo(toDo);

                }
                host.Close();
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("An exception occurred: {0}", cex.Message);
                host.Abort();
            }
        }
        #endregion

        #region Abraham
        /// <summary>
        /// Lägger till flera todos på en gång
        /// </summary>
        /// <param name="todoList"></param>
        /// <param name="uri"></param>
        public static void AddToDos(List<ToDo> todoList, string uri)
        {
            foreach (var todo in todoList)
            {
                AddToDo(todo, uri);
            }
        }
        #endregion

        #region Peter
        /// <summary>
        /// Den här metoden anropar webbservicens UpdateToDoList och uppdaterar ett ToDo objekt i databasen.
        /// </summary>
        /// <param name="toDo">ToDo objekt</param>
        /// <param name="uri">URI till webbservicen</param>
        public static void UpdateToDoList(ToDo toDo, string uri)
        {
            WebServiceHost host = new WebServiceHost(typeof(Service1), new Uri(uri));
            try
            {
                ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IService1), new WebHttpBinding(), "");
                host.Open();
                using (ChannelFactory<IService1> cf = new ChannelFactory<IService1>(new WebHttpBinding(), uri))
                {
                    cf.Endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
                    IService1 channel = cf.CreateChannel();

                    channel.UpdateToDoList(toDo);
                }
                host.Close();
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("An exception occurred: {0}", cex.Message);
                host.Abort();
            }
        }
        #endregion

        #region Jeton
        /// <summary>
        /// Den här metoden anropar webbservicens DeleteToDoList och raderar motsvarande ToDo rad i databasen. 
        /// Men först kontrolleras att id som ska raderas finns
        /// </summary>
        /// <param name="id">id för raden som ska raderas</param>
        /// <param name="uri">URI till webbservicen</param>
        public static void DeleteToDoList(int id, string uri)
        {
            WebServiceHost host = new WebServiceHost(typeof(Service1), new Uri(uri));
            try
            {
                ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IService1), new WebHttpBinding(), "");
                host.Open();
                using (ChannelFactory<IService1> cf = new ChannelFactory<IService1>(new WebHttpBinding(), uri))
                {
                    cf.Endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
                    IService1 channel = cf.CreateChannel();

                    channel.DeleteToDo(id);

                }
                host.Close();
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("An exception occurred: {0}", cex.Message);
                host.Abort();
            }
        }
        #endregion

        #region Sol-Britt
        /// <summary>
        /// Den här metoden anropar webbservicens GetToDoList och hämtar all ToDo objekt i en lista från databasen.
        /// </summary>
        /// <param name="uri">URI till webbservicen</param>
        /// <returns>Lista med ToDos</returns>
        public static List<ToDo> GetToDoList(string uri)
        {
            List<ToDo> toDoList = null;

            WebServiceHost host = new WebServiceHost(typeof(Service1), new Uri(uri));
            try
            {
                ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IService1), new WebHttpBinding(), "");
                host.Open();
                using (ChannelFactory<IService1> cf = new ChannelFactory<IService1>(new WebHttpBinding(), uri))
                {
                    cf.Endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
                    IService1 channel = cf.CreateChannel();

                    toDoList = channel.GetToDoList();

                }
                host.Close();
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("An exception occurred: {0}", cex.Message);
                host.Abort();
            }

            return toDoList;
        }
        #endregion

        #region Jeton & Peter
        /// <summary>
        /// Den här metoden anropar webbservicens GetToDoListById och hämtar ett ToDo objekt med ett specifikt id från databasen.
        /// </summary>
        /// <param name="id">id för raden i databasen som ska hämtas</param>
        /// <param name="uri">URI till webbservicen</param>
        /// <returns>Lista med ToDos</returns>
        public static List<ToDo> GetToDoListById(int id, string uri)
        {
            List<ToDo> toDoList = null;

            WebServiceHost host = new WebServiceHost(typeof(Service1), new Uri(uri));
            try
            {
                ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IService1), new WebHttpBinding(), "");
                host.Open();
                using (ChannelFactory<IService1> cf = new ChannelFactory<IService1>(new WebHttpBinding(), uri))
                {
                    cf.Endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
                    IService1 channel = cf.CreateChannel();

                    toDoList = channel.GetToDoListById(id);

                }
                host.Close();
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("An exception occurred: {0}", cex.Message);
                host.Abort();
            }
            return toDoList;
        }
        #endregion

        #region Används av i princip alla
        /// <summary>
        /// Den här metoden anropar webbservicens GetToDoListByName och hämtar ett ToDo objekt med ett specifikt namn från databasen.
        /// </summary>
        /// <param name="name">id för raden i databasen som ska hämtas</param>
        /// <param name="uri">URI till webbservicen</param>
        /// <returns></returns>
        public static List<ToDo> GetToDoListByName(string name, string uri, string filter)
        {
            List<ToDo> toDoList = null;
            List<ToDo> toDoImportantList = null;

            WebServiceHost host = new WebServiceHost(typeof(Service1), new Uri(uri));
            try
            {
                ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IService1), new WebHttpBinding(), "");
                host.Open();
                using (ChannelFactory<IService1> cf = new ChannelFactory<IService1>(new WebHttpBinding(), uri))
                {
                    cf.Endpoint.EndpointBehaviors.Add(new WebHttpBehavior());
                    IService1 channel = cf.CreateChannel();

                    toDoList = channel.GetToDoListByName(name);
                    toDoImportantList = channel.GetToDoListByName(name + "!");
                }
                host.Close();
            }
            catch (CommunicationException cex)
            {
                Console.WriteLine("An exception occurred: {0}", cex.Message);
                host.Abort();
            }
            switch (filter)
            {
                case "finished":
                    {
                        List<ToDo> toDoListFinished = new List<ToDo>();
                        foreach (var toDo in toDoList)
                        {
                            if (toDo.Finnished)
                                toDoListFinished.Add(toDo);
                        }
                        return toDoListFinished;
                    }
                case "unfinished":
                    {
                        List<ToDo> toDoListUnFinished = new List<ToDo>();
                        foreach (var toDo in toDoList)
                        {
                            if (!toDo.Finnished)
                                toDoListUnFinished.Add(toDo);
                        }
                        return toDoListUnFinished;
                    }
                case "important":
                    {
                        return toDoImportantList;
                    }
                case "deadline":
                    {
                        toDoList.Sort(new ToDo.SortByDeadLineClass());
                        return toDoList;
                    }
                case "none":
                    {
                        return toDoList;
                    }
                default:
                    break;
            }
            return toDoList;
        }
        #endregion
    }
}
