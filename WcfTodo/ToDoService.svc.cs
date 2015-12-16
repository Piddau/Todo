using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DatabaseClasses;
using DAL;

namespace WcfTodo
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ToDoService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ToDoService.svc or ToDoService.svc.cs at the Solution Explorer and start debugging.
    public class ToDoService : IToDoService
    {
        DAL.DAL dal;
        public ToDoService()
        {
            string strConnString = @"Data Source = PETERDATORNY; Initial Catalog = DB_ToDoList; User ID = RestFullUser; Password = RestFull123";
            dal = new DAL.DAL(strConnString);
            
        }

        /// <summary>
        /// Skapa flera punkter genom att skriva dom med comma.
        /// </summary>
        /// <param name="id">Under vilken ToDo Lista skall dessa skapas.</param>
        /// <param name="Descriptions">Strängen för att bedömma </param>
        public void CreateItems(string id, string Descriptions)
        {
            
        }

        /// <summary>
        /// Redigera en punkt.
        /// </summary>
        /// <param name="id">den punkt man önskar redigera</param>
        /// <param name="information">Todon som innehåller den informationen som ska ersättas.</param>
        public void EditItem(string id, ToDo information)
        {
            ToDo dbTodo = dal.GetToDoById(int.Parse(id));

            dbTodo = information;

            dal.UpdateToDo(dbTodo);
        }

        public ToDo[] GetTodoLists()
        {
            return dal.GetToDoList().ToArray();
        }
    }
}
