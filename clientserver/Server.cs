using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using clientserver;
using DataLibary;

namespace server
{
    public class Server
    {
        private Data data;
        private Users users;


        private readonly X509Certificate2 Cert = new X509Certificate2("server.pfx", "instant");

        private Server()
        {
     
            //Gets the data from the file in the debug
            DeserializeObject();

            //Console.WriteLine(data.Persons[0].BikeId);
            //data.BroadcastMsg = new List<string>();

            //data = new Data();
            //users = new Users();
            Console.WriteLine(data.Persons.Count);
            Console.WriteLine(users.Users1.Count);

         

          //  data.BikeIDs.Add("SG1B7300");

            //Console.WriteLine(
            //    data.BroadcastMsg.Count);
          
           // data.BroadcastMsg.Add("zekur");
          //  data.Persons[0].Messages = new List<string>();

            //Console.WriteLine(data.Persons[0].Sessions.Count);

           // data.Persons[0].Messages.Add("privattestmessage");

           // data = new Data();
            //users = new Users();

          //  Console.WriteLine(data.Persons.Count);

         //   data.Persons.RemoveAt(4);
           //   data.Persons.Add(new Person("http://www.juniversel-mediums.nl/wp-content/uploads/2016/02/foto-Yvonne-voor-de-lijn.png", "Joshua", "Latusia", "SG1B7300"));
            //Console.WriteLine(users.Users1.Count);
           // users.Users1.Add(new User("admin", "password"));
            // Console.WriteLine(data.Persons[0].Sessions.Count);


            //adds the event when the exit button has been pressed to a function which saves data to an obj file (ConsoleEventHandler)
            _consoleHandler = ConsoleEventHandler;
            SetConsoleCtrlHandler(_consoleHandler, true);


            Connection();
        }

        private bool ConsoleEventHandler(ConsoleCtrlHandlerCode eventcode)
        {
            try
            {
                Stream stream = File.Open("DataTxtFile", FileMode.Create);

                var bf = new BinaryFormatter();
                bf.Serialize(stream, data);
                stream = File.Open("UsersTxtFile", FileMode.Create);
                bf.Serialize(stream, users);
                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        private void DeserializeObject()
        {
            using (Stream stream = File.Open("DataTxtFile", FileMode.Open))
            {
                var bf = new BinaryFormatter();
                data = (Data) bf.Deserialize(stream);
            }
            using (Stream stream = File.Open("UsersTxtFile", FileMode.Open))
            {
                var bf = new BinaryFormatter();

                users = (Users) bf.Deserialize(stream);
            }
        }


        private void Connection()
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Parse(SettingsData.IPADRESSSERVER), SettingsData.PORTNUMBERSERVER);

                listener.Start();

                while (true)
                {
                    Console.WriteLine("connection waiting...");

                    var client = listener.AcceptTcpClient();

                    Console.WriteLine("connectie gevonden");

                    SslStream ssl = new SslStream(client.GetStream(), false);
                          
                    ssl.AuthenticateAsServer(Cert, false, SslProtocols.Tls, true);

                    var readerTcp = new BinaryReader(ssl);

                    Console.WriteLine("leest voor doktor of patient");
                    var s = SettingsData.ReadTextMessage(readerTcp);
                    Console.WriteLine(s);
                    Thread thread;

                    if (s == "patient")
                    {
                        Console.WriteLine("Patient has been connected to your server with the correct certificate");
                        thread = new Thread(() => HandleSessionPatient(ssl));
                    }
                    else
                    {
                        Console.WriteLine("Doktor has been connected to your server with the correct certificate");
                        //doctorrequest handling
                        thread = new Thread(() => HandleSessionDoktor(ssl));
                    }
                    thread.Start();
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
            }
        }

        private void HandleSessionDoktor(SslStream ssl)
        {
            try
            {      
                var readerTcp = new BinaryReader(ssl);
                var writerTcp = new BinaryWriter(ssl);

                var correctpassword = false;

                //passwordframe communication
                while (!correctpassword)
                {
                    Console.WriteLine("leest voor een naam en wachtwoord");
                    var username = SettingsData.ReadTextMessage(readerTcp);
                    var password = SettingsData.ReadTextMessage(readerTcp);
               
                    Console.WriteLine("krijgt naam wachtwoord binnen");
                    foreach (var p in users.Users1)
                        if ((p.Name == username) && (p.Password == password))
                        {
                            
                            SettingsData.SendTextMessage(writerTcp, "correct");
                            correctpassword = true;
                            Console.WriteLine("Goed wachtwoord!");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("foooout wachtwoord");
                        }
                    if (!correctpassword)
                        SettingsData.SendTextMessage(writerTcp, "incorrect");
                }

                //contentframe communication
                while (true)
                {
                    var command = SettingsData.ReadTextMessage(readerTcp);

                    Console.WriteLine(command);
                    switch (command)
                    {
                        case "addUser":
                            users.Users1.Add(new User(SettingsData.ReadTextMessage(readerTcp),
                                SettingsData.ReadTextMessage(readerTcp)));
                            Console.WriteLine("Gebruiker is toegevoegd");
                            break;
                        case "addClient":
                            data.Persons.Add(new Person(SettingsData.ReadTextMessage(readerTcp), SettingsData.ReadTextMessage(readerTcp),
                                SettingsData.ReadTextMessage(readerTcp), SettingsData.ReadTextMessage(readerTcp)));
                            Console.WriteLine("client toegevoegd!");
                            break;
                        case "deleteClient":

                            data.Persons.RemoveAt(int.Parse(SettingsData.ReadTextMessage(readerTcp)));
                            Console.WriteLine("client deleted");
                            break;
                        case "addBroadcast":
                            data.BroadcastMsg.Add(SettingsData.ReadTextMessage(readerTcp));
                            Console.WriteLine("broadcast added");
                            break;
                        case "addPrivatemessage":
                            int selectedperson = int.Parse(SettingsData.ReadTextMessage(readerTcp));
                            data.Persons[selectedperson].Messages.Add(SettingsData.ReadTextMessage(readerTcp));
                            
                            Console.WriteLine("private message added");
                            break;
                        case "addBikeID":
                            data.BikeIDs.Add(SettingsData.ReadTextMessage(readerTcp));
                            break;
                        default:
                            SettingsData.SendObjectMessage(writerTcp, data);
                            Console.WriteLine("dataobject opgevraagd");
                            break;
                   
                    }
                }
            }
            catch (Exception)
            {            
                Console.WriteLine("Doctor has disconnected");
            }
        }

        private void HandleSessionPatient(SslStream tcp)
        {
            try
            {
                Console.WriteLine("sessie is gestart");
                BinaryReader readerTcp = new BinaryReader(tcp);
                BinaryWriter writerTcp = new BinaryWriter(tcp);

                string bikeID = SettingsData.ReadTextMessage(readerTcp);
                Console.WriteLine("bikeID: " + bikeID);

                bool IDCorrect = false;

                int counter = 0;
                foreach (var p in data.Persons)
                {

                    if (bikeID.Contains(p.BikeId))
                    {
                        IDCorrect = true;
                        break;
                    }

                    counter++;
                }
                SettingsData.SendTextMessage(writerTcp, IDCorrect ? ("correct") : "incorrect");


                if (IDCorrect)
                {
                 
                    data.Persons[counter].Sessions.Add(new Session());
                    while (true)
                    {
                        Console.WriteLine("is reading for an action");
                        string message = SettingsData.ReadTextMessage(readerTcp);
                        Console.WriteLine("actie is :" + message);

                        switch (message)
                        {
                            case "getChatData":
                                Console.WriteLine("fietser wilt chat data hebben");
                                SettingsData.SendArrayMessage(writerTcp, data.BroadcastMsg);

                                SettingsData.SendArrayMessage(writerTcp, data.Persons[counter].Messages);
                                Console.WriteLine("data van server naar chat verstuurd");
                                break;
                            case "sendChatData":
                                Console.WriteLine("chatbericht wordt aan server toegevoegd");
                                message = SettingsData.ReadTextMessage(readerTcp);
                                data.Persons[counter].Messages.Add(message);
                                Console.WriteLine("chatbericht aan server toegevoegd");
                                break;
                            case "sendBikeData":
                                 message = SettingsData.ReadTextMessage(readerTcp);
                                string[] dataIncoming = SettingsData.MessageToArray(message);
                                data.Persons[counter].Sessions[data.Persons[counter].Sessions.Count - 1].addToList(
                                    dataIncoming);
                                Console.WriteLine("data aan server toegevoegd");
                                break;
                        }
                    }
                }
                else
                    Console.WriteLine("Disconnecting patient, wrong bike ID");

            }
            catch (Exception)
            {
                Console.WriteLine("Patient has disconnected"); 
            }
        }



        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            new Server();
        }

        #region Page Event Setup

        private enum ConsoleCtrlHandlerCode : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private delegate bool ConsoleCtrlHandlerDelegate(ConsoleCtrlHandlerCode eventCode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandlerDelegate handlerProc, bool add);

        private static ConsoleCtrlHandlerDelegate _consoleHandler;

        #endregion
    }
}