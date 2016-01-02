using DatabaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoConsoleApplication
{
    /// <summary>
    /// Det här programmet kommunicerar med en SQL databas för att hantera To-Do listor.
    /// </summary>
    class Program
    {
        public static string uri = "http://localhost:2222/"; // Byt ut till eran lokala server
        static void Main(string[] args)
        {
            int input;

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("To-Do Applikation");
            Console.WriteLine("--------------------------------------");

            do
            {
                PrintMenu();
                ReadInput(out input);
            } while (input != 0);

        }

        /// <summary>
        /// Skriver ut menyn för de operationer som kan utföras av programmet.
        /// </summary>
        private static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------------------");

            #region Sol-Britt
            Console.WriteLine("Hämta alla To-Dos för givet namn: 1"); // Krav 1.
            Console.WriteLine();
            #endregion

            #region Abraham
            Console.WriteLine("Skapa ny To-Do (separera med ',' för flera samtidigt): 2"); // Krav 2, 3 & 7 
            Console.WriteLine();
            #endregion

            #region Peter
            Console.WriteLine("Uppdatera en To-Do: 3"); // Krav 8
            Console.WriteLine();
            #endregion

            #region Jeton
            Console.WriteLine("Radera en To-Do: 4"); // Krav 4
            Console.WriteLine();
            #endregion

            #region Sol-Britt
            Console.WriteLine("Hämta alla avklarade för givet namn: 5"); // Krav 5 & 6
            Console.WriteLine();
            Console.WriteLine("Hämta alla kvarvarande för givet namn: 6"); // Krav 5 & 6
            Console.WriteLine();
            #endregion

            #region Peter
            Console.WriteLine("Sortera To-Dos efter deadline: 7"); // Krav 9
            Console.WriteLine();
            #endregion

            #region Jeton
            Console.WriteLine("Sortera To-Dos efter viktiga !: 8"); // Krav 10
            Console.WriteLine();
            #endregion
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Exit: 0");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine();
        }

        /// <summary>
        /// Läser in användarens val och kontrollerar att det är rätt format.
        /// </summary>
        /// <param name="input"></param>
        private static void ReadInput(out int input)
        {
            if (int.TryParse(Console.ReadLine(), out input) && input <= 8 && input >= 0)
            {
                FindQuery(input);
            }
            else
            {
                Console.WriteLine("Du har angivit fel format på siffran eller utanför intervallet 0-8.");
                input = -1;
            }
        }

        /// <summary>
        /// Beroende på användarens val anropas motsvarande förfrågan till databasen.
        /// </summary>
        /// <param name="input">menu val</param>
        private static void FindQuery(int input)
        {
            List<ToDo> todoList = null;
            string name;
            ToDo todo = null;

            switch (input)
            {
                case 0:
                    Environment.Exit(0); // Vid 0 avslutas programmet
                    break;
                case 1:
                    #region Sol-Britt - Visa punkter efter namn
                    name = PrintAndReadName();
                    if (name == "0") return;
                    todoList = ToDoUtility.GetToDoListByName(name, uri, "none");
                    PrintList(todoList);
                    break;
                    #endregion
                case 2:
                    #region Abraham - Skapa nya punkter
                    List<ToDo> todos = PrintAndReadToDos();
                    if (todos == null) return;
                    ToDoUtility.AddToDos(todos, uri);
                    break;
                    #endregion
                case 3:
                    #region Peter - Uppdatera punkter
                    todo = PrintAndReadUpdateToDo();
                    if (todo == null) return;
                    ToDoUtility.UpdateToDoList(todo, uri);
                    break;
                    #endregion
                case 4:
                    #region Jeton - Radera punkter
                    int idToDelete = int.Parse(PrintAndReadId()); // läser in id att radera.
                    if (idToDelete == 0) return; // Om id var 0 vill användaren gå tillbaka.
                    if (ControlIfIdExists(idToDelete, uri)) // Kontrollerar om id finns i databasen innan man raderar.
                    {
                        ToDoUtility.DeleteToDoList(idToDelete, uri); // raderar posten med id:t
                    }
                    break;
                    #endregion
                case 5:
                    #region Sol-Britt - Visa avklarade punkter
                    name = PrintAndReadName();
                    if (name == "0") return;
                    todoList = ToDoUtility.GetToDoListByName(name, uri, "finished");
                    PrintList(todoList);
                    break;
                    #endregion
                case 6:
                    #region Sol-Britt - Visa kvarvarande punkter
                    name = PrintAndReadName();
                    if (name == "0") return;
                    todoList = ToDoUtility.GetToDoListByName(name, uri, "unfinished");
                    PrintList(todoList);
                    break;
                    #endregion
                case 7:
                    #region Peter - Punkter efter deadline
                    name = PrintAndReadName();
                    if (name == "0") return;
                    todoList = ToDoUtility.GetToDoListByName(name, uri, "deadline");
                    PrintList(todoList);
                    break;
                    #endregion
                case 8:
                    #region Jeton - Viktiga punkter
                    name = PrintAndReadName();
                    if (name == "0") return;
                    todoList = ToDoUtility.GetToDoListByName(name, uri, "important");
                    PrintList(todoList);
                    break;
                    #endregion
                default:
                    break;
            }
        }

        #region Jeton och Peter, används för att kontrollera ID.
        /// <summary>
        /// Den här metoden hämtar all ToDos och kontrollerar att Id finns bland dessa.
        /// </summary>
        /// <param name="idToDelete">id att radera</param>
        /// <param name="uri"></param>
        private static bool ControlIfIdExists(int idToCheck, string uri)
        {
            List<ToDo> allTodoList = ToDoUtility.GetToDoList(uri);
            bool idExists = false;

            foreach (var todo in allTodoList)
            {
                if (todo.Id == idToCheck)
                    idExists = true;
            }

            if (!idExists)
            {
                Console.WriteLine("Id för todo:n du angivit finns inte i databasen!");
            }
            return idExists;
        }
        #endregion


        #region Peter - Används för att uppdatera en Todo
        /// <summary>
        /// Den här metoden läser in id för todo som ska uppdateras, och sedan skriver ut vilka egenskaper som ska uppdateras
        /// där användaren väljer och anger nya värden.
        /// </summary>
        /// <returns></returns>
        private static ToDo PrintAndReadUpdateToDo()
        {
            int id = int.Parse(PrintAndReadId());
            if (id == 0)
            {
                return null;
            }

            if (ControlIfIdExists(id, uri))
            {
                ToDo todoToUpdate = ToDoUtility.GetToDoListById(id, uri)[0];
                PrintAndReadPropertyToUpdate(todoToUpdate);
                return todoToUpdate;
            }
            return null;
        }


        /// <summary>
        /// Den här metoden skriver ut lista med egenskaperna för en todo,
        /// användaren väljer sedan en egenskap och anger det nya värdet.
        /// </summary>
        private static void PrintAndReadPropertyToUpdate(ToDo todo)
        {
            int choice;
            do
            {
                PrintPropertyList();
                if (!int.TryParse(Console.ReadLine(), out choice) || choice < 0 || choice > 5)
                {
                    Console.WriteLine("Felaktig inmating ska vara mellan 0 - 5!");
                }

                switch (choice)
                {
                    case 0:
                        return;
                        break;
                    case 1:
                        todo.Name = PrintAndReadName();
                        break;
                    case 2:
                        todo.Description = PrintAndReadDescription();
                        break;
                    case 3:
                        todo.Finnished = bool.Parse(PrintAndReadFinished());
                        break;
                    case 4:
                        todo.DeadLine = DateTime.Parse(PrintAndReadDeadline());
                        break;
                    case 5:
                        todo.EstimationTime = int.Parse(PrintAndReadEstimationTime());
                        break;
                    default:
                        break;
                }
            } while (choice != 0);
        }



        /// <summary>
        /// Egenskaperna för en todo som skrivs ut när en Todo ska uppdateras.
        /// </summary>
        private static void PrintPropertyList()
        {
            Console.WriteLine();
            Console.WriteLine("Välj egenskap du vill uppdatera:");
            Console.WriteLine();
            Console.WriteLine("Namn: 1");
            Console.WriteLine();
            Console.WriteLine("Beskrivning: 2");
            Console.WriteLine();
            Console.WriteLine("Är klar: 3");
            Console.WriteLine();
            Console.WriteLine("Deadline: 4");
            Console.WriteLine();
            Console.WriteLine("Estimerat tid: 5");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Klar: 0");
            Console.WriteLine();
        }
        #endregion


        #region Alla/Jeton - används för att skriva ut en lista samt estimat.
        /// <summary>
        /// Den här metoden skriver ut listan till konsolen och den estimerade tiden det tar för alla todos samt
        /// sluttiden om arbetet skulle börja direkt.
        /// </summary>
        /// <param name="todoList"></param>
        private static void PrintList(List<ToDo> todoList)
        {
            if (todoList != null && todoList.Count > 0)
            {
                foreach (var todo in todoList)
                {
                    Console.WriteLine();
                    Console.WriteLine(todo.ToString());
                }
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Totalt estimerat tid: " + GetEstimationTime(todoList) + " min");
                Console.WriteLine("Vid omgående start blir klar: " + DateTime.Now.AddMinutes(GetEstimationTime(todoList)));
                Console.WriteLine("--------------------------------------");
            }
        }
        #endregion

        #region Jeton - Beräknar estimat
        /// <summary>
        /// Den här metoden räknar ut estimeringstiden genom att lägga ihop estimeringstidern för todos i listan.
        /// </summary>
        /// <param name="todoList">lista med todos</param>
        /// <returns>total estimeringtid</returns>
        private static int GetEstimationTime(List<ToDo> todoList)
        {
            int totalEstimationTime = 0;
            foreach (var todo in todoList)
            {
                totalEstimationTime += todo.EstimationTime;
            }
            return totalEstimationTime;
        }
        #endregion


        #region Alla som läser in ett namn - Peter uppdatera todo
        /// <summary>
        /// Den här metoden läser in namn från användaren.
        /// </summary>
        /// <returns>namn</returns>
        private static string PrintAndReadName()
        {
            Console.WriteLine();
            Console.WriteLine("Ange namn för To-Do (0 för gå tillbaka):");
            string names;
            do
            {
                names = Console.ReadLine();

                if (names == "0") return "0";

                if (string.IsNullOrEmpty(names) || names.Length < 6)
                {
                    Console.WriteLine("Namnet måste vara minst 6 tecken!");
                }

            } while (string.IsNullOrEmpty(names) || names.Length < 6);

            return names;
        }
        #endregion

        #region Alla som läser in id - Peter uppdatera todo
        /// <summary>
        /// Den här metoden läser in ett id från användaren.
        /// </summary>
        /// <returns>id</returns>
        private static string PrintAndReadId()
        {
            Console.WriteLine();
            Console.WriteLine("Ange id för To-Do (0 för gå tillbaka):");
            int id;
            do
            {
                if (!int.TryParse(Console.ReadLine(), out id) || id < 0)
                {
                    Console.WriteLine("Felaktigt format eller id <= 0!");
                }
                if (id == 0) return "0";
            } while (id < 0);

            return id.ToString();
        }
        #endregion

        #region Alla som läser in beskrivning - Peter uppdatera todo
        /// <summary>
        /// Den här metoden läser in beskrivning från användaren.
        /// </summary>
        /// <returns>beskrivning</returns>
        private static string PrintAndReadDescription()
        {
            Console.WriteLine();
            Console.WriteLine("Ange beskrivning för To-Do (0 för gå tillbaka): ");
            string description;
            do
            {
                description = Console.ReadLine();
                if (string.IsNullOrEmpty(description))
                {
                    Console.WriteLine("Beskrivningen får inte vara tom!");
                }
            } while (string.IsNullOrEmpty(description));

            return description;
        }
        #endregion

        #region Alla som läser in deadline - Peter uppdatera todo
        /// <summary>
        /// Den här metoden läser in slutdatum och tid från användaren.
        /// </summary>
        /// <returns>deadline</returns>
        private static string PrintAndReadDeadline()
        {
            Console.WriteLine();
            Console.WriteLine("Ange slut datum och tid på formatet (2014-06-06 18:39:00): ");
            DateTime deadline;
            do
            {
                if (!DateTime.TryParse(Console.ReadLine(), out deadline) || deadline < DateTime.Now)
                {
                    Console.WriteLine("Felaktigt format eller tidpunkt bakåt i tiden!");
                }
            } while (deadline < DateTime.Now);

            return deadline.ToString();
        }
        #endregion

        #region Alla som läser in estimeringstid
        /// <summary>
        /// Den här metoden läser in slutdatum och tid från användaren.
        /// </summary>
        /// <returns>estimation time</returns>
        private static string PrintAndReadEstimationTime()
        {
            Console.WriteLine();
            Console.WriteLine("Ange estimerad tid det tar för To-Do i minuter (0 för att gå tillbaka): ");
            int estimationTime;
            do
            {
                if (!int.TryParse(Console.ReadLine(), out estimationTime) || estimationTime < 0)
                {
                    return estimationTime.ToString();
                }
            } while (estimationTime < 0);

            return estimationTime.ToString();
        }
        #endregion

        #region Alla som läser in om todo klar - Peter uppdatera todo
        /// <summary>
        /// Den här metoden läser in om en todo är klar från användaren.
        /// </summary>
        /// <returns>är klar</returns>
        private static string PrintAndReadFinished()
        {
            Console.WriteLine();
            Console.WriteLine("Är To-Do klar (Y/N): ");
            char finished;
            do
            {
                if (char.TryParse(Console.ReadLine(), out finished))
                {
                    if (finished == 'y' || finished == 'Y')
                    {
                        return true.ToString();
                    }
                    else if (finished == 'n' || finished == 'N')
                    {
                        return false.ToString();
                    }
                    else if (finished == '0') return "0";
                    else
                    {
                        Console.WriteLine("Felaktig inmatning!");
                    }
                }
            } while (finished != 'Y' || finished != 'y' || finished != 'N' || finished != 'n');

            return finished.ToString();
        }
        #endregion

        #region Abraham skapa todos - creationdate
        /// <summary>
        /// Skapelsetiden för en todo, vilket läses från datorn och inte användaren.
        /// </summary>
        /// <returns>Exakt tiden just nu.</returns>
        private static string PrintAndReadCreatedDate()
        {
            return DateTime.Now.ToString();
        }
        #endregion

        #region Abraham - Skapa todos
        /// <summary>
        /// Den här metoden läser in information om en ToDo från användaren.
        /// </summary>
        /// <returns>ToDo</returns>
        private static List<ToDo> PrintAndReadToDos()
        {
            List<ToDo> todoList = new List<ToDo>();

            todoList = PrintAndReadNames(todoList);

            if (todoList == null) return null;

            todoList = PrintAndReadDescriptions(todoList);

            if (todoList == null) return null;

            todoList = PrintAndReadFinished(todoList);

            if (todoList == null) return null;

            todoList = PrintAndReadCreatedDates(todoList);

            todoList = PrintAndReadDeadline(todoList);

            if (todoList == null) return null;

            todoList = PrintAndReadEstimationTimes(todoList);

            if (todoList == null) return null;

            return todoList;
        }
        #endregion

        #region Jeton - Estimeringstider
        /// <summary>
        /// Den här metoden läser in estimerat tid för todon
        /// </summary>
        /// <param name="todoList">todo list</param>
        /// <returns>todo list</returns>
        private static List<ToDo> PrintAndReadEstimationTimes(List<ToDo> todoList)
        {
            Console.WriteLine();
            Console.WriteLine("Ange estimerad tid det tar för To-Do i minuter separera med ',' för flera (0 för att gå tillbaka): ");
            string estimationTimes;
            bool estimationTimeOk = true;
            do
            {
                estimationTimes = Console.ReadLine();

                if (estimationTimes == "0") return null;

                string[] estimationTimess = estimationTimes.Split(',');

                if (estimationTimess.Length != todoList.Count)
                {
                    Console.WriteLine("Du har angivigt felaktigt antal estimeringstider!");
                    estimationTimeOk = false;
                }

                if (estimationTimeOk)
                {
                    for (int i = 0; i < todoList.Count; i++)
                    {
                        int et;
                        if (int.TryParse(estimationTimess[i].Trim(), out et) && et > 0)
                        {
                            todoList[i].EstimationTime = et;
                        }
                        else
                        {
                            estimationTimeOk = false;
                            Console.WriteLine("felaktigt format på estimeringstiden!");
                            break;
                        }
                    }
                }

            } while (!estimationTimeOk);

            return todoList;
        }
        #endregion

        #region Abraham skapa todos - deadline
        /// <summary>
        /// Den här metoden läser in deadlines för todo objekten
        /// </summary>
        /// <param name="todoList">todo lista</param>
        /// <returns>todo lista</returns>
        private static List<ToDo> PrintAndReadDeadline(List<ToDo> todoList)
        {
            Console.WriteLine();
            Console.WriteLine("Ange slut datum och tid på formatet (2014-06-06 18:39:00) för flera separa med ',' (0 för att gå tillbaka): ");
            string deadlines;
            bool deadlineOk;
            do
            {
                deadlineOk = true;
                deadlines = Console.ReadLine();

                if (deadlines == "0") return null;

                string[] deadliness = deadlines.Split(',');

                if (deadliness.Length != todoList.Count)
                {
                    Console.WriteLine("Du har angivigt felaktigt antal deadlines!");
                    deadlineOk = false;
                }

                if (deadlineOk)
                {
                    for (int i = 0; i < todoList.Count; i++)
                    {
                        DateTime dt;
                        if (DateTime.TryParse(deadliness[i].Trim(), out dt) && dt > DateTime.Now)
                        {
                            todoList[i].DeadLine = dt;
                        }
                        else
                        {
                            Console.WriteLine("Felaktigt format eller tidpunkt bakåt i tiden!");
                            deadlineOk = false;
                            break;
                        }
                    }
                }

            } while (!deadlineOk);

            return todoList;
        }
        #endregion

        #region Abraham skapa todos - createdate
        /// <summary>
        /// Sätter nuvarande tidpunkten för ToDona
        /// </summary>
        /// <param name="todoList">todo lista</param>
        /// <returns>todo lista</returns>
        private static List<ToDo> PrintAndReadCreatedDates(List<ToDo> todoList)
        {
            foreach (var todo in todoList)
            {
                todo.CreatedDate = DateTime.Now;
            }
            return todoList;
        }
        #endregion

        #region Abraham skapa todos - finished
        /// <summary>
        /// Den här metoden läser in om todo objekten är klara
        /// </summary>
        /// <param name="todoList">todo lista</param>
        /// <returns>todo lista</returns>
        private static List<ToDo> PrintAndReadFinished(List<ToDo> todoList)
        {
            Console.WriteLine();
            Console.WriteLine("Är To-Do klar (Y/N) för flera separera med ',' (0 för att gå tillbaka): ");
            string finished;
            bool finishedOk;
            do
            {
                finishedOk = true;

                finished = Console.ReadLine();
                if (finished == "0") return null;

                string[] finishedd = finished.Split(',');

                if (finishedd.Length != todoList.Count)
                {
                    finishedOk = false;
                    Console.WriteLine("Du har angivigt felaktigt antal klar!");
                }

                if (finishedOk)
                {
                    for (int i = 0; i < todoList.Count; i++)
                    {
                        string f = finishedd[i].Trim();
                        if (f == "y" || f == "Y")
                        {
                            todoList[i].Finnished = true;
                        }
                        else if (f == "n" || f == "N")
                        {
                            todoList[i].Finnished = false;
                        }
                        else
                        {
                            Console.WriteLine("Felaktig inmatning!");
                            finishedOk = false;
                            break;
                        }
                    }
                }
            } while (!finishedOk);

            return todoList;
        }
        #endregion

        #region Abraham skapa todos - descriptions
        /// <summary>
        /// Den här metoden läser in alla beskrivningar
        /// </summary>
        /// <param name="todoList">todo lista</param>
        /// <returns>todo lista</returns>
        private static List<ToDo> PrintAndReadDescriptions(List<ToDo> todoList)
        {
            Console.WriteLine();
            Console.WriteLine("Ange beskrivning för To-Dos för flera separera med ',' (0 för gå tillbaka): ");
            string descriptions;
            bool descriptionOk;
            do
            {
                descriptionOk = true;

                descriptions = Console.ReadLine();

                if (descriptions == "0") return null;

                string[] descriptionss = descriptions.Split(',');

                if (descriptionss.Length != todoList.Count)
                {
                    descriptionOk = false;
                    Console.WriteLine("Du har angivigt felaktigt antal beskrivningar!");
                }

                if (descriptionOk)
                {

                    for (int i = 0; i < todoList.Count; i++)
                    {
                        if (string.IsNullOrEmpty(descriptionss[i]))
                        {
                            Console.WriteLine("Beskrivningen får inte vara tom!");
                            descriptionOk = false;
                            break;
                        }
                        else
                        {
                            todoList[i].Description = descriptionss[i];
                        }
                    }
                }
            } while (!descriptionOk);

            return todoList;
        }
        #endregion

        #region Abraham skapa todos - names
        /// <summary>
        /// Läser in namnen comma-separerade för todo objekt. 
        /// </summary>
        /// <param name="todoList">Todo lista</param>
        /// <returns>Todo lista</returns>
        private static List<ToDo> PrintAndReadNames(List<ToDo> todoList)
        {
            Console.WriteLine();
            Console.WriteLine("Ange namn för To-Dos separera med ',' för flera (0 för gå tillbaka):");
            string names;
            bool namesOk;
            do
            {
                namesOk = true;

                names = Console.ReadLine();

                if (names == "0") return null;

                string[] namess = names.Split(',');

                foreach (var name in namess)
                {
                    if (string.IsNullOrEmpty(name) || name.Length < 6)
                    {
                        Console.WriteLine("Namnen måste vara minst 6 tecken separerade med ','!");
                        namesOk = false;
                        break;
                    }
                    else
                    {
                        ToDo todo = new ToDo();
                        todo.Name = name.Trim();
                        todoList.Add(todo);
                    }
                }

            } while (!namesOk);

            return todoList;
        }
        #endregion

        #region Alla som läser in och kontrollerar namn
        /// <summary>
        /// Den här metoden kontrollerar att namnet som användaren angivit är minst 6 tecken.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool ControlName(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length < 6)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
