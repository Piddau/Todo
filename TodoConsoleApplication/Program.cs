using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WcfTodo;
using DatabaseClasses;

namespace TodoConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServiceHost host = new WebServiceHost(typeof(ToDoService), new Uri("http://localhost:8000/"));

            try
            {
                ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IToDoService), new WebHttpBinding(), "");
                host.Open();

                using (ChannelFactory<IToDoService> cf = new ChannelFactory<IToDoService>(new WebHttpBinding(), "http://localhost:8000"))
                {
                    cf.Endpoint.Behaviors.Add(new WebHttpBehavior());

                    IToDoService channel = cf.CreateChannel();

                    

                    Console.WriteLine("Calling TodoService");
                    ToDo[] todos = channel.GetTodoLists();
                    
                    for(int i = 0; i < todos.Length; i++)
                    {
                        Console.WriteLine("{0}. {1}", i+1 ,todos[i].Name);
                    }
                }
            }
            catch(CommunicationException cex)
            {
                Console.WriteLine("Exception: {0}", cex.Message);
                host.Abort();
            }

            Console.ReadKey();
        }
    }
}
