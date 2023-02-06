using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School
{
    public partial class Client
    {
        public string FullName
        {
            get
            {
                string fio = FirstName + " " + LastName + " " + Patronymic;
                return fio;
            }
        }
    }
}
