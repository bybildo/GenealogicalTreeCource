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

namespace GenealogicalTreeCource.Class
{
    public class PersonTree : INotifyPropertyChanged // додай публічні методи, чомусь прізвще це дивні символи
    {
        private List<Person> _persons = new List<Person>();

        private string _filter;
        private ObservableCollection<Person> _search = new ObservableCollection<Person>();

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

        public ObservableCollection<Person> Search
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
            var filteredPersons = _persons
                .Where(p => string.IsNullOrEmpty(searchText) || p.ForSearch().Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .Take(5)
                .ToList();

            Search.Clear();
            foreach (var person in filteredPersons)
            {
                Search.Add(person);
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

