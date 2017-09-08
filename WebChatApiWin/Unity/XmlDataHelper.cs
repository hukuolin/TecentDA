using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
namespace WebChatApiWin
{
    public class XmlDocumentDataHelper
    {//xml数据读取帮助
        public static List<Dictionary<string, string>> ReadXmlNodeItem(string xmlFile, string node) 
        {
            if (string.IsNullOrEmpty(xmlFile)) 
            {
                return null;
            }
            if (!File.Exists(xmlFile))
            {
                return null;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFile);
            XmlNode xn = doc.SelectSingleNode(node);
            if (xn == null) 
            {
                return null;
            }
            //通过提高的节点找不到任何子节点数据
            XmlNodeList xnl = xn.ChildNodes;
            List<Dictionary<string, string>> nodes = new List<Dictionary<string, string>>();
            foreach (XmlNode item in xnl)
            {
                if (item.NodeType == XmlNodeType.Comment) { continue; }//忽略注释列
                Dictionary<string, string> nodeItem = new Dictionary<string, string>();
                XmlAttributeCollection attrs = item.Attributes;
                foreach (XmlAttribute attr in attrs)
                {
                    nodeItem.Add(attr.Name, attr.Value);
                }
                nodes.Add(nodeItem);
            }
            return nodes;
        }
    }
}
