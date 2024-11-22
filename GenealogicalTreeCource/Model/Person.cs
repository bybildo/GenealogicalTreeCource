using GenealogicalTreeCource.Enum;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Navigation;

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
        private List<Person> _wives = new List<Person>();
        private List<Person> _children = new List<Person>();

        private string Name
        {
            get { return _name; }
            set
            {
                if (value == "" || value == null) _name = "*невідомо*";
                if (value.Length > 20) throw new Exception("Ім'я завелике");
                if (Regex.IsMatch(value, @"^[A-Za-zА-ЩЬЮЯЄІЇа-щьюяєії]+$"))
                {
                    _name = value;
                    return;
                }
                throw new Exception("Ім'я не відповідає вимогам");
            }
        }

        private string Surname
        {
            get { return _surname; }
            set
            {
                if (value == "" || value == null) _surname = "*невідомо*";
                if (value.Length > 20) throw new Exception("Прізвище завелике");
                if (Regex.IsMatch(value, @"^[A-Za-zА-ЩЬЮЯЄІЇа-щьюяєії]+$"))
                {
                    _surname = value;
                    return;
                }
                throw new Exception("Прізвище не відповідає вимогам");
            }
        }

        private string Fathername
        {
            get { return _fathername; }
            set
            {
                if (value == "" || value == null) _fathername = "*невідомо*";
                if (value.Length > 20) throw new Exception("По батькові завелике");
                if (Regex.IsMatch(value, @"^[A-Za-zА-ЩЬЮЯЄІЇа-щьюяєії]+$"))
                {
                    _fathername = value;
                    return;
                }
                throw new Exception("По батькові не відповідає вимогам");
            }
        }

        private Gender GenderPerson
        {
            get { return _gender; }
            set { _gender = value; }
        }

        private string Photo
        {
            get { return _photo; }
            set
            {
                if (value == null) return;
                string filePath = $"Image/PersonPhoto/{value}.jpg";
                if (!File.Exists(filePath)) throw new Exception("Фото не коректне");
                _photo = filePath;
            }
        }

        private DateOnly? BirthdayDate
        {
            get
            {
                return _birthdayDate ?? new DateOnly(0, 0, 0);
            }
            set
            {
                if (value != null && value < new DateOnly(1500, 1, 1)) throw new Exception("Дата народження не коректна");
                _birthdayDate = value;
            }
        }

        private DateOnly? DeathDate
        {
            get
            {
                return _DeathDate ?? new DateOnly(0, 0, 0);
            }
            set
            {
                if (_DeathDate != null && _birthdayDate != null && _birthdayDate > value) throw new Exception("Дата смерті не може бути раніше дати народження");
                if (_DeathDate != null && value > DateOnly.FromDateTime(DateTime.Today)) throw new Exception("Дата смерті не може бути в майбутньому");
                _DeathDate = value;
            }
        }

        private Person? Father
        {
            get
            {
                return _father ?? new Person(); //ович, йович, івна, ївна жінка діти
            }
            set
            {
                _father = value;
            }
        } //

        private Person? Mother
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

        private List<Person> Wives
        {
            get { return _wives; }
        } //

        private List<Person> Children
        {
            get { return Children; }
        } //

        public Person() { }

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

        public string GetFullName()
        {
            return $"{_surname} {_name} {_fathername}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
