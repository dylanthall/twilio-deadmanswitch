using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadMansSwitch
{
    public class EmergencyContactList
    {
        public static List<String> ContactList = new List<String>();
        public static String DYLAN = "+19xxxxxxxxx";
        public static void InitList()
        {
            
            ContactList.Add("+19xxxxxxxxx"); //Aunt
            ContactList.Add("+16xxxxxxxxx"); //Mother
            ContactList.Add("+16xxxxxxxxx"); //Father
            ContactList.Add("+19xxxxxxxxx"); //Cousin
            ContactList.Add("+19xxxxxxxxx"); //Me


        }
    }
}
