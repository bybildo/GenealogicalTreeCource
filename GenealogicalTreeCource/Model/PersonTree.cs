using GenealogicalTreeCource.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;
using System.Data;
using GenealogicalTreeCource.Model.Enum;
using System.Windows;
using System.Windows.Input;
using GenealogicalTreeCource.ViewModel;
using GenealogicalTreeCource.Xaml;

namespace GenealogicalTreeCource.Class
{
    public class PersonTree : INotifyPropertyChanged
    {
        private List<Person> _persons = new List<Person>();
        private List<Person> _addPerson = new List<Person>();
        private List<Person> _editPerson = new List<Person>();
        public List<Person> GetPersons { get => _persons; }


        private int _choosePersonId;
        public int ChoosePersonaId
        {
            get
            {
                LoadChoosePersonId();
                return _choosePersonId;
            }
            set
            {
                if (_choosePersonId != value)
                {
                    _choosePersonId = value;

                    SaveChoosePersonId();
                    OnPropertyChanged(nameof(ChoosePersonaId));
                }
            }
        }

        public Person ChooseAdd
        {
            get
            {
                _addPerson.Add(new Person("void"));
                return _addPerson[_addPerson.Count - 1];
            }
        }
        public Person ChooseEdit
        {
            get
            {
                _editPerson.Add(new Person("void"));
                return _editPerson[_addPerson.Count - 1];
            }
        }

        public PersonTree()
        {
            InitializeCommands();
            LoadFromFile();
        }

        #region Пошук mvvm
        private string _filter1 = "";
        private string _filter2 = "";
        private ObservableCollection<string> _search1 = new ObservableCollection<string>();
        private ObservableCollection<string> _search2 = new ObservableCollection<string>();

        public string Filter1
        {
            get { return _filter1; }
            set
            {
                if (_filter1 != value)
                {
                    _filter1 = value;
                    FilterPersons();
                    OnPropertyChanged(nameof(Filter1));
                }
            }
        }

        public string Filter2
        {
            get { return _filter2; }
            set
            {
                if (_filter2 != value)
                {
                    _filter2 = value;
                    FilterPersons();
                    OnPropertyChanged(nameof(Filter2));
                }
            }
        }

        public ObservableCollection<string> Search1
        {
            get { return _search1; }
            private set
            {
                if (_search1 != value)
                {
                    _search1 = value;
                    OnPropertyChanged(nameof(Search1));
                }
            }
        }

        public ObservableCollection<string> Search2
        {
            get { return _search2; }
            private set
            {
                if (_search2 != value)
                {
                    _search2 = value;
                    OnPropertyChanged(nameof(Search2));
                }
            }
        }

        public void FilterPersons()
        {
            int? searchYear1 = null;
            int? searchYear2 = null;
            string textForSearch1 = Filter1?.Trim();
            string textForSearch2 = Filter2?.Trim();

            var yearMatch1 = Regex.Match(textForSearch1, @"\b(\d{4})\b");
            var yearMatch2 = Regex.Match(textForSearch2, @"\b(\d{4})\b");

            if (yearMatch1.Success)
            {
                if (int.TryParse(yearMatch1.Value, out var year))
                {
                    searchYear1 = year;
                    textForSearch1 = textForSearch1.Replace(yearMatch1.Value, "").Trim();
                }
            }
            else searchYear1 = DateTime.Now.Year;

            if (yearMatch2.Success)
            {
                if (int.TryParse(yearMatch2.Value, out var year))
                {
                    searchYear2 = year;
                    textForSearch2 = textForSearch2.Replace(yearMatch2.Value, "").Trim();
                }
            }
            else searchYear2 = DateTime.Now.Year;

            var filteredPersons1 = _persons
                .Where(p => !string.Equals(p.Fathername, "*невідомо*", StringComparison.OrdinalIgnoreCase) &&
                            (string.IsNullOrEmpty(textForSearch1) || p.ForSearch().Contains(textForSearch1, StringComparison.OrdinalIgnoreCase)))
                .Reverse()
                .OrderBy(p => Math.Abs(p.BirthdayDate?.Year - searchYear1.Value ?? 0))
                .Take(5)
                .ToList();

            var filteredPersons2 = _persons
                .Where(p => !string.Equals(p.Fathername, "*невідомо*", StringComparison.OrdinalIgnoreCase) &&
                            (string.IsNullOrEmpty(textForSearch2) || p.ForSearch().Contains(textForSearch2, StringComparison.OrdinalIgnoreCase)))
                .Reverse()
                .OrderBy(p => Math.Abs(p.BirthdayDate?.Year - searchYear2.Value ?? 0))
                .Take(5)
                .ToList();

            _search1.Clear();
            foreach (var person in filteredPersons1)
            {
                _search1.Add(person.ForSearch());
            }

            _search2.Clear();
            foreach (var person in filteredPersons2)
            {
                _search2.Add(person.ForSearch());
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

        private void SaveChoosePersonId()
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(new { ChoosePersonaId = _choosePersonId });
                File.WriteAllText("ChoosePersonId.json", json);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void LoadChoosePersonId()
        {
            try
            {
                if (File.Exists("ChoosePersonId.json"))
                {
                    var json = File.ReadAllText("ChoosePersonId.json");
                    using (var doc = System.Text.Json.JsonDocument.Parse(json))
                    {
                        if (doc.RootElement.TryGetProperty("ChoosePersonaId", out var element))
                        {
                            _choosePersonId = element.GetInt32();
                        }
                    }
                }
            }
            catch
            {
                throw new Exception();
            }
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
        public int GetIdFromToString(string toString)
        {
            return _persons.FindIndex(p => p.ToString() == toString);
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

        public Person GetVoidPerson()
        {
            _addPerson.Add(new Person("void"));
            return _addPerson[_addPerson.Count - 1];
        }

        #region команди mvvm
        public ICommand AddPersonPage { get; private set; }
        public ICommand ViewPersonPage { get; private set; }
        public ICommand EditPersonPage { get; private set; }
        public ICommand GraphPage { get; private set; }
        public ICommand AdministrationPage { get; private set; }
        public ICommand BackPage { get; private set; }

        private void InitializeCommands()
        {
            AddPersonPage = new RelayCommand(_ => OpenAddPerson());
            ViewPersonPage = new RelayCommand<string>(OpenViewPerson);
            EditPersonPage = new RelayCommand(_ => OpenEditPerson());
            GraphPage = new RelayCommand(_ => OpenGraphPerson());
            AdministrationPage = new RelayCommand(_ => OpenAdministration());
            BackPage = new RelayCommand(_ => BackFrame());
        }

        private void OpenAddPerson()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.SetWindow.Navigate(new OperationWithPerson(TypeOperation.Add));
            }
            else throw new Exception("Ви видалили frame");
        }

        private void OpenEditPerson()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.SetWindow.Navigate(new OperationWithPerson(TypeOperation.Edit));
            }
            else throw new Exception("Ви видалили frame");
        }

        public void OpenViewPerson(string personForSearch = "*")
        {
            if (personForSearch.Contains("*невідомо*")) return;
            if (personForSearch != "*")
                ChoosePersonaId = _persons.FindIndex(p => p.ForSearch() == personForSearch);

            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.SetWindow.Navigate(new OperationWithPerson(TypeOperation.View));
            }
            else throw new Exception("Ви видалили frame");
        }

        private void OpenGraphPerson()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.SetWindow.Navigate(new ViewPersonTree());
            }
            else throw new Exception("Ви видалили frame");
        }

        private void OpenAdministration()
        {
          AdministrationPage window = new AdministrationPage();
            window.ShowDialog();  
        }

        private void BackFrame()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.SetWindow.Navigate(new AdministrationPage());
                try
                {
                    mainWindow.SetWindow.GoBack();
                }
                catch { mainWindow.SetWindow.Navigate(null); }
            }
            else throw new Exception("Ви видалили frame");
        }
        #endregion

        #region Забезпечення Binding
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

