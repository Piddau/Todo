using DatabaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DatabaseLayer;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        // Här måste ni byt ut Data Source till eran SQL Server instans för att det ska fungera.
        private readonly string connString = @"Data Source = Jeton-Dator\SQLEXPRESS; Initial Catalog = DB_ToDoList; User ID = RestFullUser; Password = RestFull123";
        private DAL dal;

        /// <summary>
        /// 
        /// </summary>
        public Service1()
        {
            dal = new DAL(connString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ToDo> GetToDoList()
        {
            return dal.GetToDoList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ToDo> GetToDoListById(int id)
        {
            return dal.GetToDoListById(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ToDo> GetToDoListByName(string name)
        {
            return dal.GetToDoListByName(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toDo"></param>
        public void UpdateToDoList(ToDo toDo)
        {
            dal.UpdateToDoList(toDo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toDo"></param>
        public void AddToDo(ToDo toDo)
        {
            dal.AddToDo(toDo);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DeleteToDo(int id)
        {
            dal.DeleteToDoList(id);
        }
    }
}
