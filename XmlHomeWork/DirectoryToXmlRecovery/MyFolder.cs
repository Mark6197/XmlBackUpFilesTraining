using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DirectoryToXmlRecovery
{
    public class MyFolder
    {
        #region Properties
        [XmlAttribute]
        public string Name { get; set; }
        public string Path { get; set; }
        [XmlIgnore]
        public bool IsExist { get; set; }

        public List<MyFile> MyFiles { get; set; }
    

        #endregion

        #region Ctor's

        public MyFolder()
        {

        }

        public MyFolder(string path)
        {
            if (Directory.Exists(path))
            {
                IsExist = true;
                this.Path = path;
                string[] temp = path.Split('\\');
                Name = temp.Last();
                ConvertFolderToObjects(path);
            }
            else
                throw new Exception("The Directory doesn't exist");
        }
        #endregion

        #region Methods

        public void ConvertFolderToObjects(string path)
        {
            MyFiles = new List<MyFile>();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (FileInfo item in fileInfos)
            {
                MyFile temp = new MyFile(item);
                MyFiles.Add(temp);
                temp.Number = MyFiles.Count;
            }
        }
        public static MyFolder RecoverByXml(string xmlName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MyFolder));

            using (var XmlToObjectReader = new StreamReader(xmlName))
            {
                return (MyFolder)serializer.Deserialize(XmlToObjectReader);
            }
        }
        
        #endregion

    }
}
