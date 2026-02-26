using System;
using Lab_rab_4_2_CherevkoG.S_BPI_23_02.Model;

namespace Lab_rab_4_2_CherevkoG.S_BPI_23_02.Model
{
    public class Person
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Birthday { get; set; }

        public Person() { }

        public Person(int id, int roleId, string firstName, string lastName, string birthday)
        {
            Id = id;
            RoleId = roleId;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
        }

        public Person CopyFromPersonDPO(PersonDpo personDpo)
        {
            Person person = new Person();
            person.Id = personDpo.Id;
            person.FirstName = personDpo.FirstName;
            person.LastName = personDpo.LastName;
            person.Birthday = personDpo.Birthday;

            var vmRole = new ViewModel.RoleViewModel();
            foreach (var r in vmRole.ListRole)
            {
                if (r.NameRole == personDpo.RoleName)
                {
                    person.RoleId = r.Id;
                    break;
                }
            }
            return person;
        }
    }
}