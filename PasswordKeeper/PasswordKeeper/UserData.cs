using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PasswordKeeper
{

    internal class UsbDriveInfo
    {
        public static readonly string nodeName_PNP_ID = "PNP_ID";

        public string ID_Device { get; private set; }
        public string ID_PNP { get; private set; }
        public string Volume { get; private set; }
        public string LogicalDisk { get; private set; }

        public UsbDriveInfo(string iD_Device, string iD_PNP, string volume, string logicalDisk)
        {
            ID_Device = iD_Device;
            ID_PNP = iD_PNP;
            Volume = volume;
            LogicalDisk = logicalDisk;
        }
    }

    public class UserData
    {
        public static readonly string nodeName_UserData = "User";
        public static readonly string nodeName_Login = "Login";
        public static readonly string nodeName_PasswordHash = "PasswordHash";


        public string Login { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public string USB_Drive_PNP { get; private set; }

        public UserData(string login, byte[] pHash, string usbID) {
            Login = login;
            PasswordHash = pHash;
            USB_Drive_PNP = usbID;
        }

        public static UserData? createUserFromXML(XmlElement root)
        {
            byte[]? pHash = null;
            string? login = null;
            string? id_pnp = null;
            if (root.Name == nodeName_UserData)
            {
                foreach(XmlElement node in root)
                {
                    if (node.Name == nodeName_Login) login = node.InnerText;
                    else if (node.Name == nodeName_PasswordHash) pHash = Convert.FromBase64String(node.InnerText);
                    else if(node.Name == UsbDriveInfo.nodeName_PNP_ID) id_pnp = node.InnerText;
                }
            }
            if (pHash == null || login == null || id_pnp == null) return null;
            return new UserData(login, pHash, id_pnp);
        }

        public void saveToXML(XmlDocument document)
        {
            XmlElement userNode = document.CreateElement(nodeName_UserData);

            XmlElement loginNode = document.CreateElement(nodeName_Login);
            loginNode.InnerText = Login;
            XmlElement pHashNode = document.CreateElement(nodeName_PasswordHash);
            pHashNode.InnerText = Convert.ToBase64String(PasswordHash);
            XmlElement usbID = document.CreateElement(UsbDriveInfo.nodeName_PNP_ID);
            usbID.InnerText = USB_Drive_PNP;

            userNode.AppendChild(loginNode);
            userNode.AppendChild(pHashNode);
            userNode.AppendChild(usbID);

            var root = document.DocumentElement;
            root.AppendChild(userNode);

            document.Save(FormSignIn.usersDocPath);
        }
    }
}
