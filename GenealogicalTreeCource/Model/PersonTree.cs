using GenealogicalTreeCource.Enum;
using GenealogicalTreeCource.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Data;

namespace GenealogicalTreeCource.Class
{
    public class PersonTree : INotifyPropertyChanged
    {
        private List<Person> _persons = new List<Person>();

        #region Пошук mvvm
        private string _filter = "";
        private ObservableCollection<string> _search = new ObservableCollection<string>();

        public string Filter
        {
            get { return _filter; }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    FilterPersons(_filter);
                    OnPropertyChanged(nameof(Filter));
                }
            }
        }

        public ObservableCollection<string> Search
        {
            get { return _search; }
            private set
            {
                if (_search != value)
                {
                    _search = value;
                    OnPropertyChanged(nameof(Search));
                }
            }
        }

        public void FilterPersons(string searchText)
        {
            int? searchYear = null;
            string textForSearch = searchText?.Trim();

            var yearMatch = Regex.Match(textForSearch, @"\b(\d{4})\b");

            if (yearMatch.Success)
            {
                if (int.TryParse(yearMatch.Value, out var year))
                {
                    searchYear = year;
                    textForSearch = textForSearch.Replace(yearMatch.Value, "").Trim();
                }
            }
            else searchYear = DateTime.Now.Year;


            var filteredPersons = _persons
                .Where(p => !string.Equals(p.Fathername, "*невідомо*", StringComparison.OrdinalIgnoreCase) &&
                            (string.IsNullOrEmpty(textForSearch) || p.ForSearch().Contains(textForSearch, StringComparison.OrdinalIgnoreCase)))
                .Reverse()
                .OrderBy(p => Math.Abs(p.BirthdayDate?.Year - searchYear.Value ?? 0))
                .Take(5)
                .ToList();

            _search.Clear();
            foreach (var person in filteredPersons)
            {
                _search.Add(person.ForSearch());
            }
        }
        #endregion

        #region Json збереження
        public void SaveToFile()
        {
            UpdateAllPersonId();
            const string FilePath = "persons.json";

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    IgnoreSerializableAttribute = false
                }
            };

            string json = JsonConvert.SerializeObject(_persons, settings);
            File.WriteAllText(FilePath, json);
        }

        public void LoadFromFile()
        {
            const string FilePath = "persons.json";
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);

                var settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                };
                _persons = JsonConvert.DeserializeObject<List<Person>>(json, settings) ?? new List<Person>();
            }
            UpdateRefernces();
        }
        #endregion

        #region Оновлення id
        //Id потрібно щоб забезпечити збереження файлів, адже збереження через посилання Person створює циклічність 
        private void UpdateAllPersonId()
        {
            foreach (var person in _persons)
            {
                UpdateID(person);
            }
        }

        private void UpdateID(Person me)
        {
            int fatherID = me.Father?.Id.MyId ?? -1;
            int motherID = me.Mother?.Id.MyId ?? -1;

            List<int> wifeID = new List<int>();
            List<int> childrenID = new List<int>();

            foreach (var wife in me.Wifes)
                wifeID.Add(wife.Id.MyId);

            foreach (var child in me.Children)
                childrenID.Add(child.Id.MyId);

            me.Id = new PersonId(me.Id.MyId, fatherID, motherID, wifeID, childrenID);
        }

        // Відновлення посилань через ID
        private void UpdateRefernces()
        {
            foreach (var person in _persons)
            {
                if (person.Id.FatherId >= 0 && person.Id.FatherId < _persons.Count)
                {
                    person.Father = _persons[person.Id.FatherId];
                }

                if (person.Id.MotherId >= 0 && person.Id.MotherId < _persons.Count)
                {
                    person.Mother = _persons[person.Id.MotherId];
                }

                person.Wifes = person.Id.WifesId?
                    .Where(wifeId => wifeId >= 0 && wifeId < _persons.Count)
                    .Select(wifeId => _persons[wifeId])
                    .ToList() ?? new List<Person>();

                person.Children = person.Id.ChildrenId?
                    .Where(childId => childId >= 0 && childId < _persons.Count)
                    .Select(childId => _persons[childId])
                    .ToList() ?? new List<Person>();
            }
        }
        #endregion

        #region Пошук осіб
        public Person GetPersonFromSearch(string forSearch)
        {
            return _persons.Find(p => p.ForSearch() == forSearch);
        }
        public Person GetPersonFromToString(string toString)
        {
            return _persons.Find(p => p.ToString() == toString);
        }

        public Person GetPersonFromID(int id)
        {
            if (id > -1 && id < _persons.Count)
                return _persons[id];
            return null;
        }
        #endregion

        public int NumberOfLastElement()
        {
            return _persons.Count - 1;
        }

        public string ForSearch(int id = 0)
        {
            if (id > -1 && id < _persons.Count)
                return _persons[id].ForSearch();
            return "";
        }

        public void Generate(int knees = 5)
        {
            _persons = new PersonTreeGenerator().GetPersons(knees);
        }

        #region Забезпечення Binding
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

