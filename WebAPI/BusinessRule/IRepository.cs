using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebAPI.BusinessRule
{
    public interface IPersonRepository
    {
        DataTable GetAll(string Opt, string val);
        Person Get(int id);
        int AddPerson(Person p);
    }

    public class PersonRepository : IPersonRepository
    {
        private DataAccess.DAL DA;

        public PersonRepository()
        {
            DA = new DataAccess.DAL();
        }

        public DataTable GetAll(string Opt, string val)
        {
            return DA.getAll(Opt, val);
        }

        public Person Get(int id)
        {
            return DA.getPerson(id);
        }

        public int AddPerson(Person p)
        {
            return DA.AddPerson(p);
        }
    }
}