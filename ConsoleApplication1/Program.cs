using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
/* developed by ahmed nasser */
namespace ERP
{

    class Program {
        static void UserOperationFullStack() {
            User.User user = new User.User();
            User.UserDomain newUser = new User.UserDomain("ahmed", "ahmed@live.com", "001", "ahmed",
                                                          "nasser", "fesal giza", 21, "male");
            user.insertUser(newUser);

            Console.WriteLine("******************* Done insert *******************");

            User.UserDomain userDomain = user.getUser("ahmed@live.com");
            if (user.exist_or("ahmed@live.com" , "ahmed")) {
                Console.WriteLine("******************* user exist *******************");
                userDomain.printInYou();
                Console.WriteLine("******************* Done Get *******************");
            }

            userDomain.firstName = "mohamed";
            userDomain.secondName = "ahmed";
            userDomain.printInYou();
            user.update(userDomain);
            Console.WriteLine("******************* Done update *******************");

            userDomain = user.getUser("ahmed@live.com");
            if (userDomain != null)
            {
                userDomain.printInYou();
                Console.WriteLine("******************* Done Get *******************");
            }
            user.delete(userDomain.email);
            Console.WriteLine("******************* Done delete *******************");
        }

        static void InboxOperationFull() {
            Inbox.Inbox inbox = new Inbox.Inbox();
            Inbox.InboxDomain inboxDomain = new Inbox.InboxDomain("to@live.com" , "from@gmail.com" , "test" , "test");
            inbox.insert(inboxDomain);
            Console.WriteLine("******************* Done ADD *******************");
            inboxDomain = inbox.getInbox("to@live.com", "from@gmail.com");
            Console.WriteLine("******************* Done Get *******************");
            inbox.delete("to@live.com" , "from@gmail.com");
            Console.WriteLine("******************* Done delete *******************");

        }

        static void sessionOperationFull() {
            Session.Session session = new Session.Session();
            //Session.SessionDomain sessionDomain = new Session.SessionDomain("test2", "descrition", 1, 0, 0);
            //if (session.saveSession(sessionDomain)) {
            //    Console.WriteLine("done saving");
            //}

            Session.SessionDomain sessionDomain = session.getSession("test2");
            if (sessionDomain == null){
                Console.WriteLine("null");
            } else {
                //sessionDomain.printDate();
                sessionDomain.description = "change desc";
                if (session.updateSession(sessionDomain)) {
                    Console.WriteLine("******************* Done update *******************");
                }
            }
        }

        static void Main(string[] args){
            // UserOperationFullStack();  
            // InboxOperationFull();    
            sessionOperationFull();
            Console.ReadKey();
        }
    }
}
