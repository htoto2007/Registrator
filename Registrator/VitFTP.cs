using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Registrator
{
    class VitFTP
    {

        public void CrateProfilesAllDisks()
        {
            // Создаем пользователей
            DriveInfo[] Drives = DriveInfo.GetDrives();
            foreach (DriveInfo Disk in Drives)
            {
                if (Disk.DriveType == DriveType.Fixed)
                {
                    long size = Disk.AvailableFreeSpace / 1000000000;
                    Console.WriteLine(Disk.Name + " " + size + "GB");
                    AddUser(Disk.Name, Disk.Name);

                }
            }

        }

        public void CreateProfileAdmin()
        {
            AddUser("Admin", Environment.CurrentDirectory);
            Console.WriteLine("login: Admin");
            Console.WriteLine("Share: " + Environment.CurrentDirectory);
        }

        public void CreateNewConfig()
        {
            File.Copy(Environment.CurrentDirectory + "\\FileZilla Server orig.xml", Environment.CurrentDirectory + "\\FileZilla Server.xml", false);
        }

        public void Reloade()
        {
            Process.Start(Environment.CurrentDirectory + "\\FileZilla Server.exe", " /reload-config");
        }

        private void AddUser(string name, string Dir)
        {
            string xmlFileName = "FileZilla Server.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileName);
            XmlElement xRoot = xmlDoc.DocumentElement;

            XmlElement xmlUsers = null;
            // получаем елемент с пользователями
            foreach (XmlElement xmlElem in xRoot.ChildNodes)
            {
                if (xmlElem.Name == "Users")
                    xmlUsers = xmlElem;
            }

            XmlElement xmlUser = createNode(xmlDoc, "User", "", "Name", name);

            XmlElement xmlOption = createNode(xmlDoc, "Option", "", "Name", "Pass");
            xmlUser.AppendChild(xmlOption);
            xmlOption = createNode(xmlDoc, "Option", "1", "Name", "Enabled");
            xmlUser.AppendChild(xmlOption);
            xmlOption = createNode(xmlDoc, "Option", "", "Name", "Salt");
            xmlUser.AppendChild(xmlOption);
            xmlOption = createNode(xmlDoc, "Option", "", "Name", "Group");
            xmlUser.AppendChild(xmlOption);
            xmlOption = createNode(xmlDoc, "Option", "0", "Name", "Bypass server userlimit");
            xmlUser.AppendChild(xmlOption);
            xmlOption = createNode(xmlDoc, "Option", "0", "Name", "User Limit");
            xmlUser.AppendChild(xmlOption);
            xmlOption = createNode(xmlDoc, "Option", "0", "Name", "IP Limit");
            xmlUser.AppendChild(xmlOption);
            xmlOption = createNode(xmlDoc, "Option", "", "Name", "Comments");
            xmlUser.AppendChild(xmlOption);
            xmlOption = createNode(xmlDoc, "Option", "0", "Name", "ForceSsl");
            xmlUser.AppendChild(xmlOption);

            XmlElement xmlIpFilter = createNode(xmlDoc, "IpFilter", "", "", "");
            xmlUser.AppendChild(xmlIpFilter);
            XmlElement xmlIpFilterDisallowed = createNode(xmlDoc, "Disallowed", "", "", "");
            xmlIpFilter.AppendChild(xmlIpFilterDisallowed);
            XmlElement xmlIpFilterAllowed = createNode(xmlDoc, "Allowed", "", "", "");
            xmlIpFilter.AppendChild(xmlIpFilterAllowed);

            XmlElement xmlSpeedLimits = createNode(xmlDoc, "SpeedLimits", "", "DlType", "0");
            xmlUser.AppendChild(xmlSpeedLimits);

            XmlText xmlAttrText = xmlDoc.CreateTextNode("10");
            XmlAttribute xmlAttribute = xmlDoc.CreateAttribute("DlLimit");
            xmlAttribute.AppendChild(xmlAttrText);
            xmlSpeedLimits.Attributes.Append(xmlAttribute);

            xmlAttrText = xmlDoc.CreateTextNode("0");
            xmlAttribute = xmlDoc.CreateAttribute("ServerDlLimitBypass");
            xmlAttribute.AppendChild(xmlAttrText);
            xmlSpeedLimits.Attributes.Append(xmlAttribute);

            xmlAttrText = xmlDoc.CreateTextNode("0");
            xmlAttribute = xmlDoc.CreateAttribute("UlType");
            xmlAttribute.AppendChild(xmlAttrText);
            xmlSpeedLimits.Attributes.Append(xmlAttribute);

            xmlAttrText = xmlDoc.CreateTextNode("10");
            xmlAttribute = xmlDoc.CreateAttribute("UlLimit");
            xmlAttribute.AppendChild(xmlAttrText);
            xmlSpeedLimits.Attributes.Append(xmlAttribute);

            xmlAttrText = xmlDoc.CreateTextNode("0");
            xmlAttribute = xmlDoc.CreateAttribute("ServerUlLimitBypass");
            xmlAttribute.AppendChild(xmlAttrText);
            xmlSpeedLimits.Attributes.Append(xmlAttribute);

            XmlElement xmlUserDownload = createNode(xmlDoc, "Download", "", "", "");
            xmlSpeedLimits.AppendChild(xmlUserDownload);
            XmlElement xmlUserUpload = createNode(xmlDoc, "Upload", "", "", "");
            xmlSpeedLimits.AppendChild(xmlUserUpload);

            xmlUser.AppendChild(xmlSpeedLimits);

            XmlElement xmlUserPermissions = createNode(xmlDoc, "Permissions", "", "", "");

            XmlElement xmlUserPermission = createNode(xmlDoc, "Permission", "", "Dir", Dir);

            XmlElement xmlUserPermissionOption = createNode(xmlDoc, "Option", "1", "Name", "FileRead");
            xmlUserPermission.AppendChild(xmlUserPermissionOption);
            xmlUserPermissionOption = createNode(xmlDoc, "Option", "1", "Name", "FileWrite");
            xmlUserPermission.AppendChild(xmlUserPermissionOption);
            xmlUserPermissionOption = createNode(xmlDoc, "Option", "1", "Name", "FileDelete");
            xmlUserPermission.AppendChild(xmlUserPermissionOption);
            xmlUserPermissionOption = createNode(xmlDoc, "Option", "1", "Name", "FileAppend");
            xmlUserPermission.AppendChild(xmlUserPermissionOption);
            xmlUserPermissionOption = createNode(xmlDoc, "Option", "1", "Name", "DirCreate");
            xmlUserPermission.AppendChild(xmlUserPermissionOption);
            xmlUserPermissionOption = createNode(xmlDoc, "Option", "1", "Name", "DirDelete");
            xmlUserPermission.AppendChild(xmlUserPermissionOption);
            xmlUserPermissionOption = createNode(xmlDoc, "Option", "1", "Name", "DirList");
            xmlUserPermission.AppendChild(xmlUserPermissionOption);
            xmlUserPermissionOption = createNode(xmlDoc, "Option", "1", "Name", "DirSubdirs");
            xmlUserPermission.AppendChild(xmlUserPermissionOption);
            xmlUserPermissionOption = createNode(xmlDoc, "Option", "1", "Name", "IsHome");
            xmlUserPermission.AppendChild(xmlUserPermissionOption);
            xmlUserPermissionOption = createNode(xmlDoc, "Option", "1", "Name", "AutoCreate");
            xmlUserPermission.AppendChild(xmlUserPermissionOption);

            xmlUserPermissions.AppendChild(xmlUserPermission);

            xmlUser.AppendChild(xmlUserPermissions);

            xmlUsers.AppendChild(xmlUser);

            xmlDoc.Save(xmlFileName);
        }

        private XmlElement createNode(XmlDocument xmlDocument,
            string elemName,
            string elemValue,
            string attrName,
            string attrValue)
        {
            XmlElement xmlElement = xmlDocument.CreateElement(elemName);

            XmlText xmlAttrText = xmlDocument.CreateTextNode(attrValue);
            XmlText xmlText = xmlDocument.CreateTextNode(elemValue);
            if (attrName != "")
            {
                XmlAttribute xmlAttribute = xmlDocument.CreateAttribute(attrName);
                xmlAttribute.AppendChild(xmlAttrText);
                xmlElement.Attributes.Append(xmlAttribute);
            }
            xmlElement.AppendChild(xmlText);
            return xmlElement;
        }

        private void userPatern()
        {
            string name = "admin";
            string user = "<User Name= \"" + name + "\" >" +
                "< Option Name= \"Pass\"></ Option >" +
                "< Option Name= \"Salt\"></ Option >" +
                "< Option Name= \"Group\"> admin </ Option >" +
                "< Option Name= \"Bypass server userlimit\"> 0 </ Option >" +
                "< Option Name= \"User Limit\"> 0 </ Option >" +
                "< Option Name= \"IP Limit\"> 0 </ Option >" +
                "< Option Name= \"Enabled\"> 1 </ Option >" +
                "< Option Name= \"Comments\"></ Option >" +
                "< Option Name= \"ForceSsl\"> 0 </ Option >" +
                "< IpFilter >" +
                    "< Disallowed />" +
                    "< Allowed />" +
                "</ IpFilter >" +
                "< Permissions />" +
                "< SpeedLimits DlType= \"0\" DlLimit= \"10\" ServerDlLimitBypass= \"0\" UlType= \"0\" UlLimit= \"10\" ServerUlLimitBypass= \"0\">" +
                    "< Download />" +
                    "< Upload />" +
                "</ SpeedLimits >" +
            "</ User >";
        }
    }
}

