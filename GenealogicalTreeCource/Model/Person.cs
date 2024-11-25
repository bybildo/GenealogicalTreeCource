using GenealogicalTreeCource.Enum;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace GenealogicalTreeCource.Class
{
    public class Person : INotifyPropertyChanged
    {
        private string _name = "*невідомо*";
        private string _surname = "*невідомо*";
        private string _fathername = "*невідомо*";
        private Gender _gender = Gender.unknown;
        private string _photo = "Image/PersonPhoto/absent.jpg";
        private DateOnly? _birthdayDate;
        private DateOnly? _DeathDate;
        private Person? _father;
        private Person? _mother;
        private List<Person> _wifes = new List<Person>();
        private List<Person> _children = new List<Person>();
        private (int MyIdml, int FatherId, int MotherId, List<int> WifesId, List<int> ChildrenId) _id = (-1, -1, -1, new List<int>(), new List<int>());

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == "" || value == null) _name = "*невідомо*";
                if (value.Length > 20) throw new Exception("Ім'я завелике");
                if (Regex.IsMatch(value, pattern))
                {
                    _name = value;
                    return;
                }
                throw new Exception("Ім'я не відповідає вимогам");
            }
        }

        private const string pattern = @"^[А-ЯІЄЇҐа-яієїґ' -*]+$";
        public string Surname
        {
            get { return _surname; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _surname = "*невідомо*";
                    return;
                }

                if (value.Length > 20)
                    throw new Exception("Прізвище завелике");

                if (Regex.IsMatch(value, pattern))
                {
                    if ( GenderPerson == Gender.male)
                    {
                        if (value.EndsWith("а"))
                            _surname = value.Substring(0, value.Length - 1) + "ий";
                        else _surname = value;
                    }
                    else
                    {
                        if (value.EndsWith("ий"))
                            _surname = value.Substring(0, value.Length - 2) + "а";
                        else _surname = value;
                    }
                    return;
                }

                throw new Exception("Прізвище не відповідає вимогам");
            }
        }

        public string Fathername
        {
            get { return _fathername; }
            set
            {
                if (value == null || value == "")
                {
                    _fathername = "*невідомо*";
                    return;
                }

                if (value.Length > 20)
                {
                    throw new Exception("По батькові завелике");
                }

                if (Regex.IsMatch(value, pattern))
                {
                    _fathername = value;
                    return;
                }

                throw new Exception("По батькові не відповідає вимогам");
            }
        }

        public Gender GenderPerson
        {
            get { return _gender; }
            private set { _gender = value; }
        }

        public string Photo
        {
            get { return _photo; }
            set
            {
                if (value == null || value == "Image/PersonPhoto/absent.jpg") return;
                string filePath = $"Image/PersonPhoto/{value}.jpg";
                if (!File.Exists(filePath)) throw new Exception("Фото не коректне");
                _photo = filePath;
            }
        }

        public DateOnly? BirthdayDate
        {
            get
            {
                return _birthdayDate ?? DateOnly.MinValue;
            }
            set
            {
                if (value != null && value < new DateOnly(1500, 1, 1)) throw new Exception("Дата народження не коректна");
                _birthdayDate = value;
            }
        }

        public DateOnly? DeathDate
        {
            get
            {
                return _DeathDate ?? DateOnly.MinValue;
            }
            set
            {
                if (_DeathDate != null && _birthdayDate != null && _birthdayDate > value) throw new Exception("Дата смерті не може бути раніше дати народження");
                if (_DeathDate != null && value > DateOnly.FromDateTime(DateTime.Today)) throw new Exception("Дата смерті не може бути в майбутньому");
                _DeathDate = value;
            }
        }

        [JsonIgnore]
        public Person? Father
        {
            get
            {
                return _father ?? new Person();
            }
            set
            {
                _father = value;
            }
        } //

        [JsonIgnore]
        public Person? Mother
        {
            get
            {
                return _mother ?? new Person(); //чоловік
            }
            set
            {
                _mother = value;
            }
        } //

        [JsonIgnore]
        public List<Person> Wifes
        {
            get { return _wifes; }
            set { _wifes = value; }
        }

        [JsonIgnore]
        public List<Person> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public (int MyId, int FatherId, int MotherId, List<int> WifesId, List<int> ChildrenId) Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Person() { }

        public Person(string name, string surname, string fathername, Gender genderPerson, string photo, DateOnly? birthdayDate, DateOnly? deathDate, Person? father, Person? mother, List<Person> wives, List<Person> children) : this(name, surname, fathername, genderPerson, photo, birthdayDate, deathDate, father, mother)
        {
            Name = name;
            Surname = surname;
            Fathername = fathername;
            GenderPerson = genderPerson;
            Photo = photo;
            BirthdayDate = birthdayDate;
            DeathDate = deathDate;
            Father = father;
            Mother = mother;
            Wifes = wives;
            Children = children;
        }

        public Person(string name, string surname, string fathername, Gender genderPerson, string photo, DateOnly? birthdayDate, DateOnly? deathDate, Person? father, Person? mother)
        {
            Name = name;
            Surname = surname;
            Fathername = fathername;
            GenderPerson = genderPerson;
            Photo = photo;
            BirthdayDate = birthdayDate;
            DeathDate = deathDate;
            Father = father;
            Mother = mother;
        }

        public Person(string name, string surname, string fathername, Gender genderPerson, DateOnly? birthdayDate, Person? father, Person? mother)
        {
            Name = name;
            Surname = surname;
            Fathername = fathername;
            GenderPerson = genderPerson;
            BirthdayDate = birthdayDate;
            Father = father;
            Mother = mother;
        }

        public string ForSearch()
        {
            return $"{_surname} {_name} {_fathername} {((DateOnly)BirthdayDate).Year}";
        }

        /// <summary>
        /// Оновлення виконується відразу для 2 осіб
        /// </summary>
        /// <param name="person"></param>
        public void UpdateWife(Person person)
        {
            Wifes.Add(person);
            person.Wifes.Add(this);
        }

        public void UpdatePearents(Person firstPerson, Person secondPerson)
        {
            if (firstPerson.GenderPerson == Gender.male)
            {
                Father = firstPerson;
                Mother = secondPerson;
            }
            else
            {
                Mother = firstPerson;
                Father = secondPerson;
            }

            if (Father.Name == "*невідомо*")
            {
                _fathername = "*невідомо*";
                return;
            }

            Mother.Surname = Father.Surname;

            switch (GenderPerson)
            {
                case Gender.female:
                    {
                        if (Name.EndsWith("й"))
                        {
                            _fathername = Father.Name.Substring(0, Name.Length - 1) + "ївна";
                        }
                        else
                        {
                            _fathername = Father.Name + "івна";
                        }
                        break;
                    }
                default:
                    {
                        _fathername = Father.Name + "ович";
                        break;
                    }
            }
        }

        public void AddWifes(List<Person> newWifes)
        {
            Wifes.AddRange(newWifes);
        }
        public void AddChildren(List<Person> newChildren)
        {
            Wifes.AddRange(newChildren);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
