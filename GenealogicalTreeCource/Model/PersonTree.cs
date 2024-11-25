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
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace GenealogicalTreeCource.Class
{
    public class PersonTree : INotifyPropertyChanged 
    {
        private List<Person> _persons = new List<Person>();

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


        public void Generate(int knees = 5)
        {
            _persons = new PersonTreeGenerator().GetPersons(knees);
        }

        public void SaveToFile()
        {
            const string FilePath = "persons.json";
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                WriteIndented = true
            };
            string json = System.Text.Json.JsonSerializer.Serialize(_persons, options);
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
        }

        public string ForSearch(int id = 0)
        {
            if (id > -1 && id < _persons.Count)
                return _persons[id].ForSearch();
            return "Поза межою діапазону";
        }

        public int NumberOfLastElement()
        {
            return _persons.Count - 1;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

