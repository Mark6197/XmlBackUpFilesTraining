using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace DirectoryToXmlRecovery
{
    public class MyFile
    {
        #region Properties
        [XmlAttribute]
        public int Number { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        #endregion

        #region Ctor's
        public MyFile()
        {

        }
        public MyFile(FileInfo fileInfo)
        {
            Name = fileInfo.Name;
            Content = File.ReadAllText(fileInfo.FullName);
        }
        #endregion
    }
}
