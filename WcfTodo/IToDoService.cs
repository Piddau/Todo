using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DatabaseClasses;
using System.ServiceModel.Web;

namespace WcfTodo
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IToDoService" in both code and config file together.
    [ServiceContract]
    public interface IToDoService
    {
        [OperationContract]
        [WebGet]
        ToDo[] GetTodoLists();

        [OperationContract]
        [WebInvoke]
        void CreateItems(string id, string Descriptions);

        [OperationContract]
        [WebInvoke]
        void EditItem(string id, ToDo information);
    }
}
