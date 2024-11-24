using GenealogicalTreeCource.Class;
using GenealogicalTreeCource.Enum;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace GenealogicalTreeCource.Model
{
    public class PersonTreeGenerator
    {
        private List<Person> persons = new List<Person>();
        private Random random = new Random();

        /// <summary>
        /// Вказуєте скільки поколінь ви хочете згенерувати
        /// </summary>
        /// <param name="knees"></param>
        /// <returns></returns>
        public List<Person> GetPersons(int knees = 5)
        {
            NormalGeneratedTree(knees);
            return persons;
        }

        private Person NormalGeneratedTree(int knees, int curentKnees = 0, Person me = null, Gender gender = Gender.unknown)
        {
            List<Person> children = new List<Person>() { };
            List<Person> wifes = new List<Person>();

            if (gender == Gender.unknown)
            {
                gender = (Gender)random.Next(1, 3);
            }

            string surname = SurnameGenerated();
            string name = NameGenerated(gender);

            DateOnly? birthdayDate = null;
            DateOnly? deathDate = null;

            if (me != null)
            {
               
                int childYear = ((DateOnly)me.BirthdayDate).Year;
                birthdayDate = BirthdayGenerated(childYear);
                deathDate = DeathGenerated(childYear, ((DateOnly)birthdayDate).Year);

                children.Add(me);
            }
            else
                birthdayDate = BirthdayGenerated();

            Person TempMe = new Person(name, surname, "", gender, null, birthdayDate, deathDate, null, null, wifes, children);

            if (curentKnees < knees)
            {
                Person father = NormalGeneratedTree(knees, curentKnees + 1, TempMe, Gender.male);
                Person mother = NormalGeneratedTree(knees, curentKnees + 1, TempMe, Gender.female);
                father.UpdateWife(mother);
                TempMe.UpdatePearents(father, mother);
            }

            if (me != null)
            {
                TempMe.AddWifes(WifeGenerated(TempMe, knees, curentKnees));
                if (TempMe.Wifes.Count > 0)
                {
                    TempMe.AddChildren(ChildrenGenerated(TempMe, TempMe.Wifes[random.Next(TempMe.Wifes.Count)], knees, curentKnees - 1));
                }
            }

            persons.Add(TempMe);
            return TempMe;
        }

        private Person WifeGeneratedTree(int knees, int curentKnees, Person FirstWife)
        {
            List<Person> children = new List<Person>() { };
            List<Person> wives = new List<Person>() { FirstWife };

            Gender gender = (FirstWife.GenderPerson == Gender.female) ? Gender.male : Gender.female;

            string surname = SurnameGenerated();
            string name = NameGenerated(gender);

            int ImaginChildYear = ((DateOnly)FirstWife.BirthdayDate).Year + random.Next(17, 45);
            DateOnly? birthdayDate = BirthdayGenerated(ImaginChildYear);
            DateOnly? deathDate = DeathGenerated(ImaginChildYear, ((DateOnly)birthdayDate).Year);

            Person TempMe = new Person(name, surname, "", gender, null, birthdayDate, deathDate, null, null, wives, children);

            if (curentKnees < knees)
            {
                Person father = NormalGeneratedTree(knees, curentKnees + 1, TempMe, Gender.male);
                Person mother = NormalGeneratedTree(knees, curentKnees + 1, TempMe, Gender.female);
                father.UpdateWife(mother);
                TempMe.UpdatePearents(father, mother);
            }

            TempMe.AddWifes(WifeGenerated(TempMe, knees, curentKnees));
            if (TempMe.Wifes.Count > 0)
            {
                TempMe.AddChildren(ChildrenGenerated(TempMe, TempMe.Wifes[random.Next(TempMe.Wifes.Count)], knees, curentKnees - 1));
            }

            persons.Add(TempMe);
            return TempMe;
        }

        private Person ChildrenGeneratedTree(int knees, int curentKnees, Person father, Person mother)
        {
            List<Person> children = new List<Person>() { };
            List<Person> wifes = new List<Person>() { };

            Gender gender = (Gender)random.Next(1, 3);
            string surname = (random.Next(1, 30) == 29) ? mother.Surname : father.Surname;
            string name = NameGenerated(gender);

            int fatherYear = ((DateOnly)father.BirthdayDate).Year;
            DateOnly? birthdayDate = BirthdayGenerated(fatherYear, true);
            DateOnly? deathDate = null;

            int myYear = ((DateOnly)birthdayDate).Year;
            if (curentKnees > 0)
            {
                deathDate = deathDate = DeathGenerated(((DateOnly)BirthdayGenerated(myYear, true)).Year, myYear);
            }

            Person TempMe = new Person(name, surname, "", gender, null, birthdayDate, deathDate, null, null, wifes, children);
            TempMe.UpdatePearents(father, mother);

            if (myYear < DateTime.Today.Year - 18)
            {
                TempMe.AddWifes(WifeGenerated(TempMe, knees, curentKnees));
                if (TempMe.Wifes.Count > 0)
                {
                    TempMe.AddChildren(ChildrenGenerated(TempMe, TempMe.Wifes[random.Next(TempMe.Wifes.Count)], knees, curentKnees - 1));
                }
            }

            persons.Add(TempMe);
            return TempMe;
        }

        private DateOnly? BirthdayGenerated(int childBirthdayYear, bool forChildrenGenerated = false)
        {
            DateOnly? result = null;
            bool check = true;

            if (forChildrenGenerated == false)
            {
                do
                {
                    try
                    {
                        result = new DateOnly(childBirthdayYear - random.Next(17, 45), random.Next(1, 13), random.Next(1, 32));
                        check = false;
                    }
                    catch { }
                } while (check);
                return result;
            }

            do
            {
                try
                {
                    result = new DateOnly(childBirthdayYear + random.Next(17, 45), random.Next(1, 13), random.Next(1, 32));
                    check = false;
                }
                catch { }

                if (result > DateOnly.FromDateTime(DateTime.Today))
                    check = true;
            } while (check);
            return result;
        }

        private DateOnly? BirthdayGenerated()
        {
            DateOnly? result = null;
            bool check = true;
            do
            {
                try
                {
                    result = new DateOnly(random.Next(2023, 2025), random.Next(1, 13), random.Next(1, 32));
                    check = false;
                }
                catch { }
            } while (check);
            return result;
        }

        private DateOnly? DeathGenerated(int childBirthdayYear, int BirthdayYear)
        {
            DateOnly? result = DateOnly.FromDateTime(DateTime.Now);
            bool check = true;
            do
            {
                try
                {
                    if (BirthdayYear < 1920)
                        result = new DateOnly(childBirthdayYear + random.Next(0, 65), random.Next(1, 13), random.Next(1, 32));
                    else if (random.Next(0, 5) == 4)
                        result = new DateOnly(childBirthdayYear + random.Next(0, 65), random.Next(1, 13), random.Next(1, 32));
                    else result = null;
                }
                catch { }

                check = false;

                if (result != null && result > DateOnly.FromDateTime(DateTime.Today))
                    check = true;
            } while (check);
            return result;
        }

        private readonly string[] surnames = File.ReadAllLines("ForGenerate/Surname.txt");
        private string SurnameGenerated()
        {
            return surnames[random.Next(surnames.Length)];
        }

        private readonly string[] manNames = File.ReadAllLines("ForGenerate/ManName.txt");
        private readonly string[] womanNames = File.ReadAllLines("ForGenerate/WomanName.txt");
        private string NameGenerated(Gender gender)
        {
            switch (gender)
            {
                case Gender.male:
                    {
                        return manNames[random.Next(manNames.Length)];
                    }
                default:
                    {
                        return womanNames[random.Next(womanNames.Length)];
                    }
            }
        }

        private List<Person> WifeGenerated(Person me, int knees, int curentKness)
        {
            List<Person> result = new List<Person>();
            while (true)
            {
                if (random.Next(5) != 4)
                    break;
                result.Add(WifeGeneratedTree(knees, curentKness, me));
            }
            return result;
        }

        private List<Person> ChildrenGenerated(Person firstPerson, Person secondPerson, int knees, int curentKnees)
        {
            List<Person> result = new List<Person>() { };
            while (true)
            {
                if (random.Next(4) != 3)
                    break;

                if (firstPerson.GenderPerson == Gender.male)
                    result.Add(ChildrenGeneratedTree(knees, curentKnees, firstPerson, secondPerson));
                else
                    result.Add(ChildrenGeneratedTree(knees, curentKnees, secondPerson, firstPerson));
            }
            return result;
        }
    }
}
