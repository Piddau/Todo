using DatabaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebGet]
        List<ToDo> GetToDoList();

        [OperationContract]
        [WebGet]
        List<ToDo> GetToDoListById(int id);

        [OperationContract]
        [WebGet]
        List<ToDo> GetToDoListByName(string name);

        [OperationContract]
        [WebInvoke]
        void UpdateToDoList(ToDo toDo);

        [OperationContract]
        [WebInvoke]
        void AddToDo(ToDo toDo);

        [OperationContract]
        [WebInvoke]
        void DeleteToDo(int id);
    }
}
