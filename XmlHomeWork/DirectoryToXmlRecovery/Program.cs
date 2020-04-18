using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace DirectoryToXmlRecovery
{

    enum UsersChoise
    {
        Backup = 1,
        Delete = 2,
        Recreate = 3,
        Exit = 4
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the path of the directory you wish to work on");
            string path = Console.ReadLine();

            Dictionary<UsersChoise, Action> actionDictionary = new Dictionary<UsersChoise, Action>
            {
                {UsersChoise.Backup, ()=>{Console.WriteLine("Please enter the name of the  xml file you want to create");
                                           string xmlName = Console.ReadLine() + ".xml";
                                           LoadToXml(path, xmlName);} },
                {UsersChoise.Delete, ()=>DeleteDirectory(path) },
                {UsersChoise.Recreate, ()=> {Console.WriteLine("Please enter the name of the  xml file you want to re-create into folder");
                                            string xmlName = Console.ReadLine() + ".xml";
                                            ConvertObjectToFolder(xmlName); } },
                {UsersChoise.Exit, ()=>Environment.Exit(0) }

            };

            UsersChoise usersChoise = new UsersChoise();
            do
            {
                Console.WriteLine($"\n\nPlease choose (by numbers) which action you would like to do on the directory:\n1.Backup\n2.Delete\n3.Recreate\n4.Exit the Program");

                usersChoise = (UsersChoise)int.Parse(Console.ReadLine());
               

                if (actionDictionary.ContainsKey(usersChoise))
                {
                    actionDictionary[usersChoise]();
                }
            } while (actionDictionary.ContainsKey(usersChoise));

        }


        public static void DeleteDirectory(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (var item in fileInfos)
            {
                item.Delete();
            }
            Directory.Delete(path);
            Console.WriteLine($"{directoryInfo.Name} Deleted");
        }
        public static void LoadToXml(string path,string xmlName)
        {
            MyFolder folder = new MyFolder(path);
            XmlSerializer serializer = new XmlSerializer(typeof(MyFolder));

            using (var objectToTextWriter= new StreamWriter(xmlName))
            {
                serializer.Serialize(objectToTextWriter, folder);
            }
            Console.WriteLine($"Loading to Xml finished, File Name: {xmlName}");
        }
        public static void ConvertObjectToFolder(string xmlName)
        {
            MyFolder folder = MyFolder.RecoverByXml(xmlName);
            if (!Directory.Exists(folder.Path))
            {
                Directory.CreateDirectory(folder.Path);
                Console.WriteLine($"Directory: {folder.Name} Recreated");
                foreach (MyFile item in folder.MyFiles)
                {
                    using (var recreateFile=new StreamWriter(folder.Path+'\\'+item.Name))
                    {
                        recreateFile.Write(item.Content);
                    }
                Console.WriteLine($"File: {item.Name} Recreated");
                }
            }
            else
            {
                Console.WriteLine($"Directory :{folder.Name} already Exist\nChecking if all the files Exist");
                foreach (MyFile item in folder.MyFiles)
                {
                    if (!File.Exists(folder.Path + '\\' + item.Name))
                    {
                        using (var recreateFile = new StreamWriter(folder.Path + '\\' + item.Name))
                        {
                            recreateFile.Write(item.Content);
                        }
                        Console.WriteLine($"File: {item.Name} Recreated");
                        
                    }
                    else
                    {
                        Console.WriteLine($"File: {item.Name} Already Exist\nChecking if the contant is the same");
                        if (!File.ReadAllText(folder.Path + '\\' + item.Name).Equals(item.Content))
                        {
                            File.Delete(folder.Path + '\\' + item.Name);
                            using (var recreateFile = new StreamWriter(folder.Path + '\\' + item.Name))
                            {
                                recreateFile.Write(item.Content);
                            }
                            Console.WriteLine($"File: {item.Name} Content recreated");
                        }
                        Console.WriteLine($"File: {item.Name} Content is the same");
                    }
                }
            }
        }
    }
}

