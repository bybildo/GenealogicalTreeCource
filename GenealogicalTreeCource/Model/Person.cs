using GenealogicalTreeCource.Enum;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Windows;

namespace GenealogicalTreeCource.Class
{
    public class Person : INotifyPropertyChanged, ICloneable
    {
        private string _name = "*невідомо*";
        private string _surname = "*невідомо*";
        private string _fathername = "*невідомо*";
        private Gender _gender = Gender.male;
        private string _photo = "Image/PersonPhoto/absent.jpg";
        private DateOnly? _birthdayDate;
        private DateOnly? _DeathDate;
        private Person? _father;
        private Person? _mother;
        private List<Person> _wifes = new List<Person>();
        private List<Person> _children = new List<Person>();
        private PersonId _id = new PersonId(-1, -1, -1, new List<int>(), new List<int>());

        #region Конструктори
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

        public Person(string name, string surname, string fathername, Gender genderPerson, string photo, DateOnly? birthdayDate, DateOnly? deathDate, Person? father, Person? mother, List<Person> wives, List<Person> children, int myId) : this(name, surname, fathername, genderPerson, photo, birthdayDate, deathDate, father, mother)
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
            _id = new PersonId(myId, -1, -1, new List<int>(), new List<int>());
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

        public Person(string ForVoidPerson)
        {
            _name = "";
            _surname = "";
            _fathername = "";
            _gender = Gender.unknown;
        }
        #endregion

        #region Публічні властивості
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == "" || value == null) _name = "*невідомо*";
                if (value.Length > 20) MessageBox.Show("Ім'я завелике", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                if (Regex.IsMatch(value, pattern))
                {
                    _name = value;
                    return;
                }
                MessageBox.Show("Ім'я не відповідає вимогам", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private const string pattern = @"^[А-ЯІЄЇҐа-яієїґ' -*]+$";
        [JsonProperty(Order = 2)]
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
                    MessageBox.Show("Прізвище завелике", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);

                if (Regex.IsMatch(value, pattern))
                {
                    if (GenderPerson == Gender.male)
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
                    OnPropertyChanged(nameof(Surname));
                    return;
                }

                MessageBox.Show("Прізвище не відповідає вимогам", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show("По батькові завелике", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (Regex.IsMatch(value, pattern))
                {
                    _fathername = value;
                    OnPropertyChanged(nameof(Fathername));
                    return;
                }

                MessageBox.Show("По батькові не відповідає вимогам", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [JsonProperty(Order = 1)]
        public Gender GenderPerson
        {
            get { return _gender; }
            set { _gender = value; }
        }

        public string Photo
        {
            get { return _photo; }
            set
            {
                if (value == null || value == "Image/PersonPhoto/absent.jpg") return;
                string filePath = $"Image/PersonPhoto/{value}.jpg";
                if (!File.Exists(filePath)) MessageBox.Show("Фото не коректне", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                if (value != null && value < new DateOnly(1500, 1, 1)) MessageBox.Show("Дата народження не може бути менша 1500 року", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                //if (value != null && _birthdayDate != null && _birthdayDate > value) MessageBox.Show("Дата смерті не може бути раніше дати народження", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                //if (value != null && value > DateOnly.FromDateTime(DateTime.Today)) MessageBox.Show("Дата смерті не може бути в майбутньому", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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

                if (_father != null)
                {
                    switch (GenderPerson)
                    {
                        case Gender.female:
                            {
                                if (Name.EndsWith("й"))
                                {
                                    Fathername = Father.Name.Substring(0, Father.Name.Length - 1) + "ївна";
                                }
                                else
                                {
                                    Fathername = (Father.Name.EndsWith("о") || Father.Name.EndsWith("а"))
                                        ? Father.Name.Substring(0, Father.Name.Length - 1) + "івна"
                                        : Father.Name + "івна";
                                }
                                break;
                            }
                        default:
                            {
                                Fathername = (Father.Name.EndsWith("о") || Father.Name.EndsWith("а"))
                                    ? Father.Name.Substring(0, Father.Name.Length - 1) + "ович"
                                    : Father.Name + "ович";
                                break;
                            }
                    }
                }
            }
        }

        [JsonIgnore]
        public List<string> GetFather
        {
            get { return _father != null ? new List<string> { _father.ForSearch() } : new List<string>() { "" }; }
        }

        [JsonIgnore]
        public Person? Mother
        {
            get
            {
                return _mother ?? new Person();
            }
            set
            {
                _mother = value;
            }
        }

        [JsonIgnore]
        public List<string> GetMother
        {
            get { return _mother != null ? new List<string> { _mother.ForSearch() } : new List<string>() { "" }; }
        }

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

        [JsonProperty]
        public PersonId Id
        {
            get { return _id; }
            set { _id = value; }
        }
        #endregion

        #region Методи для оновлення типів посилань
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
            if (!firstPerson.Children.Contains(this))
                firstPerson.Children.Add(this);

            if (!secondPerson.Children.Contains(this))
                secondPerson.Children.Add(this);

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
        }

        public void AddWifes(List<Person> newWifes)
        {
            Wifes.AddRange(newWifes);
        }
        public void AddChildren(List<Person> newChildren)
        {
            Wifes.AddRange(newChildren);
        }
        #endregion

        #region Методи виводу
        public string ForSearch()
        {
            return $"{_surname} {_name} {_fathername} {((DateOnly)BirthdayDate).Year}";
        }

        public override string ToString()
        {
            if (this.DeathDate == DateOnly.MinValue)
                return $"{_surname} {_name} {_fathername}\n{((DateOnly)BirthdayDate).Year}";
            return $"{_surname} {_name} {_fathername}\n{((DateOnly)BirthdayDate).Year}-{((DateOnly)DeathDate).Year}";
        }
        #endregion

        public int GetDownKness()
        {
            return GetGenerationsDepth(this);

            int GetGenerationsDepth(Person? person)
            {
                if (person.Fathername == "*невідомо*")
                {
                    return 0;
                }

                return 1 + Math.Max(
                    GetGenerationsDepth(person.Father),
                    GetGenerationsDepth(person.Mother)
                );
            }
        }

        public int GetUpKness()
        {
            return GetGenerationsDepthUp(this);

            int GetGenerationsDepthUp(Person person)
            {
                if (person.Children.Count == 0)
                {
                    return 0;
                }

                int maxDepth = 0;
                foreach (var child in person.Children)
                {
                    int childDepth = GetGenerationsDepthUp(child);
                    maxDepth = Math.Max(maxDepth, childDepth);
                }

                return 1 + maxDepth;
            }
        }

        #region Забезпечення Binding

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Клонування
        public object Clone()
        {
            return new Person(_name, _surname, _fathername, _gender, null, _birthdayDate, _DeathDate, _father, _mother, _wifes, _children, _id.MyId);
        }
        #endregion
    }
}


