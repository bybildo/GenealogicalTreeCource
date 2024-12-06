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
using GenealogicalTreeCource.View;
using GenealogicalTreeCource.View.Admin;
using System.Windows.Automation;
using System;
using System.Windows.Controls;
using System.Diagnostics.Metrics;

namespace GenealogicalTreeCource.Class
{
    public class PersonTree : INotifyPropertyChanged
    {
        private List<Person> _persons = new List<Person>();
        private List<Person> _addPerson = new List<Person>();
        private List<Person> _editPerson = new List<Person>();


        public PersonTree()
        {
            InitializeCommands();
            LoadFromFile();
        }

        #region Обрана персона mvvm
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

        private bool _fatherNameEnabled = true;
        public bool FatherNameEnabled
        {
            get { return _fatherNameEnabled; }
            set
            {
                _fatherNameEnabled = value;
                OnPropertyChanged(nameof(FatherNameEnabled));
            }
        }
        #endregion

        #region Додавання редагування mvvm
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
                _editPerson.Add(_persons[ChoosePersonaId].Clone() as Person);
                return _editPerson[_editPerson.Count - 1];
            }
        }

        private Person _ChooseEditView;
        public Person ChooseEditView
        {
            get
            {
                return _ChooseEditView;
            }
            set
            {
                _ChooseEditView = value;
                OnPropertyChanged(nameof(ChooseEditView));
            }
        }

        public Person GetVoidPerson()
        {
            _addPerson.Add(new Person("void"));
            return _addPerson[_addPerson.Count - 1];
        }

        public List<Person> AddedPerson
        {
            get { return _addPerson; }
            set { _addPerson = value; }
        }
        public List<Person> EditedPerson
        {
            get { return _editPerson; }
            set { _editPerson = value; }
        }
        #endregion

        #region Пошук mvvm
        private Visibility _show1 = Visibility.Hidden;
        public Visibility Show1
        {
            get { return _show1; }
            set
            {
                _show1 = value;
                OnPropertyChanged(nameof(Show1));
            }
        }

        private Visibility _show2 = Visibility.Collapsed;
        public Visibility Show2
        {
            get { return _show2; }
            set
            {
                _show2 = value;
                OnPropertyChanged(nameof(Show2));
            }
        }

        private Visibility _show3 = Visibility.Collapsed;
        public Visibility Show3
        {
            get { return _show3; }
            set
            {
                _show3 = value;
                OnPropertyChanged(nameof(Show3));
            }
        }

        private string _filter1 = "";
        private string _filter2 = "";
        private string _filter3 = "";
        private ObservableCollection<string> _search1 = new ObservableCollection<string>();
        private ObservableCollection<string> _search2 = new ObservableCollection<string>();
        private ObservableCollection<string> _search3 = new ObservableCollection<string>();

        public string Filter1
        {
            get { return _filter1; }
            set
            {
                if (_filter1 != value)
                {
                    _filter1 = value;
                    FilterPersons1();
                    if (value == string.Empty) Show1 = Visibility.Hidden;
                    else Show1 = Visibility.Visible;
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
                    FilterPersons2();
                    if (value == string.Empty) Show2 = Visibility.Collapsed;
                    else Show2 = Visibility.Visible;
                    OnPropertyChanged(nameof(Filter2));
                    FatherNameEnabled = true;
                }
            }
        }

        public string Filter3
        {
            get { return _filter3; }
            set
            {
                if (_filter3 != value)
                {
                    _filter3 = value;
                    FilterPersons3();
                    if (value == string.Empty) Show3 = Visibility.Collapsed;
                    else Show3 = Visibility.Visible;
                    OnPropertyChanged(nameof(Filter3));
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

        public ObservableCollection<string> Search3
        {
            get { return _search3; }
            private set
            {
                if (_search3 != value)
                {
                    _search3 = value;
                    OnPropertyChanged(nameof(Search3));
                }
            }
        }

        public void FilterPersons1()
        {
            int? searchYear1 = null;
            string textForSearch1 = Filter1?.Trim();

            var yearMatch1 = Regex.Match(textForSearch1, @"\b(\d{4})\b");

            if (yearMatch1.Success)
            {
                if (int.TryParse(yearMatch1.Value, out var year))
                {
                    searchYear1 = year;
                    textForSearch1 = textForSearch1.Replace(yearMatch1.Value, "").Trim();
                }
            }
            else searchYear1 = DateTime.Now.Year;

            var filteredPersons1 = _persons
                .Where(p => !string.Equals(p.Fathername, "*невідомо*", StringComparison.OrdinalIgnoreCase) &&
                            (string.IsNullOrEmpty(textForSearch1) || p.ForSearch().Contains(textForSearch1, StringComparison.OrdinalIgnoreCase)))
                .Reverse()
                .OrderBy(p => Math.Abs(p.BirthdayDate?.Year - searchYear1.Value ?? 0))
                .Take(5)
                .ToList();

            _search1.Clear();
            foreach (var person in filteredPersons1)
            {
                _search1.Add(person.ForSearch());
            }
        }

        public void FilterPersons2()
        {
            int? searchYear2 = null;
            string textForSearch2 = Filter2?.Trim();

            var yearMatch2 = Regex.Match(textForSearch2, @"\b(\d{4})\b");
            var today = DateOnly.FromDateTime(DateTime.Now);

            if (yearMatch2.Success)
            {
                if (int.TryParse(yearMatch2.Value, out var year))
                {
                    searchYear2 = year;
                    textForSearch2 = textForSearch2.Replace(yearMatch2.Value, "").Trim();
                }
            }
            else searchYear2 = DateTime.Now.Year;

            var filteredPersons2 = _persons
            .Where(p =>
            !string.Equals(p.Fathername, "*невідомо*", StringComparison.OrdinalIgnoreCase) &&
            p.GenderPerson == Enum.Gender.male &&
            (string.IsNullOrEmpty(textForSearch2) || p.ForSearch().Contains(textForSearch2, StringComparison.OrdinalIgnoreCase)) &&
            p.BirthdayDate != default &&
            today.Year - ((DateOnly)p.BirthdayDate).Year - (today < ((DateOnly)p.BirthdayDate).AddYears(today.Year - ((DateOnly)p.BirthdayDate).Year) ? 1 : 0) > 16)
                .OrderBy(p => Math.Abs(p.BirthdayDate?.Year - searchYear2.Value ?? 0))
                .Take(5)
                .ToList();

            _search2.Clear();
            foreach (var person in filteredPersons2)
            {
                _search2.Add(person.ForSearch());
            }
        }

        public void FilterPersons3()
        {
            int? searchYear3 = null;
            string textForSearch3 = Filter3?.Trim();

            var yearMatch3 = Regex.Match(textForSearch3, @"\b(\d{4})\b");
            var today = DateOnly.FromDateTime(DateTime.Now);

            if (yearMatch3.Success)
            {
                if (int.TryParse(yearMatch3.Value, out var year))
                {
                    searchYear3 = year;
                    textForSearch3 = textForSearch3.Replace(yearMatch3.Value, "").Trim();
                }
            }
            else searchYear3 = DateTime.Now.Year;

            var filteredPersons3 = _persons
                .Where(p => !string.Equals(p.Fathername, "*невідомо*", StringComparison.OrdinalIgnoreCase) &&
                            p.GenderPerson == Enum.Gender.female &&
                            (string.IsNullOrEmpty(textForSearch3) || p.ForSearch().Contains(textForSearch3, StringComparison.OrdinalIgnoreCase)) &&
                            p.BirthdayDate != default &&
                            today.Year - ((DateOnly)p.BirthdayDate).Year - (today < ((DateOnly)p.BirthdayDate).AddYears(today.Year - ((DateOnly)p.BirthdayDate).Year) ? 1 : 0) > 16)
                .Reverse()
                .OrderBy(p => Math.Abs(p.BirthdayDate?.Year - searchYear3.Value ?? 0))
                .Take(5)
                .ToList();

            _search3.Clear();
            foreach (var person in filteredPersons3)
            {
                _search3.Add(person.ForSearch());
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
            string FilePath = "persons.json";
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);

                var settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                };
                _persons = JsonConvert.DeserializeObject<List<Person>>(json, settings) ?? new List<Person>();
            }
            FilePath = "newPersons.json";
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);

                var settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                };
                _addPerson = JsonConvert.DeserializeObject<List<Person>>(json, settings) ?? new List<Person>();
            }
            FilePath = "editPersons.json";
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);

                var settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                };
                _editPerson = JsonConvert.DeserializeObject<List<Person>>(json, settings) ?? new List<Person>();
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

        private void SaveNewPerson()
        {
            const string FilePath = "newPersons.json";
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    IgnoreSerializableAttribute = false
                }
            };
            string json = JsonConvert.SerializeObject(_addPerson, settings);
            File.WriteAllText(FilePath, json);
        }

        private void SaveEditPerson()
        {
            const string FilePath = "editPersons.json";

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    IgnoreSerializableAttribute = false
                }
            };
            string json = JsonConvert.SerializeObject(_editPerson, settings);
            File.WriteAllText(FilePath, json);
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

        private void UpdateNewPersonID(Person me)
        {
            int fatherID = _persons.FindIndex(el => el == me.Father);
            int motherID = _persons.FindIndex(el => el == me.Mother);

            me.Id = new PersonId(me.Id.MyId, fatherID, motherID, new List<int>(), new List<int>());
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

            foreach (var person in _addPerson)
            {
                if (person.Id.FatherId >= 0 && person.Id.FatherId < _persons.Count)
                {
                    person.Father = _persons[person.Id.FatherId];
                }

                if (person.Id.MotherId >= 0 && person.Id.MotherId < _persons.Count)
                {
                    person.Mother = _persons[person.Id.MotherId];
                }
            }

            foreach (var person in _editPerson)
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

        public void UpdateIdNewPerson(Person person)
        {
            int indxm = person.Id.MotherId;
            if (indxm >= 0)
            {
                Person mother = _persons[indxm];
                mother.AddChildren(new List<Person>() { person });
                mother.Id.AddChild(person.Id.MyId);
            }

            int indxf = person.Id.FatherId;
            if (indxf >= 0)
            {
                Person father = _persons[indxf];
                father.AddChildren(new List<Person>() { person });
                father.Id.AddChild(person.Id.MyId);
                if (indxm >= 0) father.UpdateWife(_persons[indxm]);
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

        public int NumberOfLastElement()
        {
            return _persons.Count - 1;
        }

        public int GetIdFromToString(string toString)
        {
            return _persons.FindIndex(p => p.ToString() == toString);
        }

        public string ForSearch(int id = 0)
        {
            if (id > -1 && id < _persons.Count)
                return _persons[id].ForSearch();
            return "";
        }
        #endregion

        #region Генерація дерева
        public void Generate(int knees = 3)
        {
            _persons = new PersonTreeGenerator().GetPersons(knees);
        }
        #endregion

        #region TreeGenWin

        private string _textbox1 = "";

        public string Textbox1
        {
            get { return _textbox1; }
            set { _textbox1 = value; }
        }

        private bool isGenerating = false;

        private async void GenerateMasiveF()
        {
            if (isGenerating)
            {
                MessageBox.Show("Генерація вже виконується. Будь ласка, зачекайте.", "Увага", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            isGenerating = true;
            try
            {
                if (int.TryParse(_textbox1, out int knees))
                {
                    if (0 < knees && knees <= 8)
                    {
                        bool uspih = false;
                        while (!uspih)
                        {
                            var cts = new CancellationTokenSource();
                            try
                            {
                                var task = GenerateWithTimeout(knees, cts.Token);
                                if (await Task.WhenAny(task, Task.Delay(12000)) == task)
                                {
                                    uspih = true;
                                    _addPerson.Clear();
                                    _editPerson.Clear();
                                    SaveToFile();
                                    SaveEditPerson();
                                    SaveNewPerson();

                                    var genWind = Application.Current.Windows.OfType<TreeGenWind>().FirstOrDefault();
                                    if (genWind != null) genWind.Close();

                                    MessageBox.Show("Дерево згенеровано успішно. Длякоректної роботи краще перезайти в додаток", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                                    return;
                                }
                                else
                                {
                                    cts.Cancel();
                                    MessageBox.Show($"Генерація тривала занадто довго. Повторюємо...", "Попередження", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Помилка генерації: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            finally
                            {
                                cts.Dispose();
                            }
                        }
                    }
                    else
                        MessageBox.Show("Завеликі числа", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Некоректно написані значення. Введіть правильні числа.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                isGenerating = false;
            }
        }

        private async Task GenerateWithTimeout(int knees, CancellationToken token)
        {
            await Task.Run(() =>
            {
                if (token.IsCancellationRequested) token.ThrowIfCancellationRequested();
                try
                {
                    Generate(knees);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка генерації для індексу: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }, token);
        }
        #endregion

        #region Вхід в адміна

        private string _acName = "Login";
        private string _acPassword = "Password";
        private bool _robot = false;

        public string AcName
        {
            get { return _acName; }
            set
            {
                _acName = value;
                OnPropertyChanged(nameof(AcName));
            }
        }

        public string AcPassword
        {
            get { return _acPassword; }
            set
            {
                _acPassword = value;
                OnPropertyChanged(nameof(AcPassword));
            }
        }

        public bool Robot
        {
            get { return _robot; }
            set
            {
                _robot = value;
                OnPropertyChanged(nameof(Robot));
            }
        }

        private void VerifyAdminF()
        {
            if (Robot == false)
            {
                MessageBox.Show("Вхід роботам ЗАБОРОНЕНИЙ!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (AcName.ToLower() == "admin" && AcPassword.ToLower() == "admin")
            {
                BackPageAdminF();
                return;
            }

            MessageBox.Show("Неправильний логін або пароль!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region команди mvvm
        public ICommand MainWindow { get; private set; }
        public ICommand AddPersonPage { get; private set; }
        public ICommand ViewPersonPage { get; private set; }
        public ICommand EditPersonPage { get; private set; }
        public ICommand GraphPage { get; private set; }
        public ICommand AdministrationPage { get; private set; }
        public ICommand AddAdministrationPage { get; private set; }
        public ICommand EditAdministrationPage { get; private set; }
        public ICommand GenerateWinwow { get; private set; }
        public ICommand GenerateMasive { get; private set; }
        public ICommand BackPage { get; private set; }
        public ICommand BackAddPage { get; private set; }
        public ICommand BackEditPage { get; private set; }
        public ICommand BackPageAdmin { get; private set; }
        public ICommand AddPerson { get; private set; }
        public ICommand AddFather { get; private set; }
        public ICommand AddMother { get; private set; }
        public ICommand NewEditPerson { get; private set; }
        public ICommand DeleteAddPerson { get; private set; }
        public ICommand AddAddPerson { get; private set; }
        public ICommand AddEditPerson { get; private set; }
        public ICommand DeleteEditPerson { get; private set; }
        public ICommand OpenDetailsNewPerson { get; private set; }
        public ICommand VerifyAdmin { get; private set; }
        private void InitializeCommands()
        {
            AddPersonPage = new RelayCommand(_ => AddPersonPageF());
            ViewPersonPage = new RelayCommand<string>(ViewPersonPageF);
            EditPersonPage = new RelayCommand(_ => EditPersonPageF());
            GraphPage = new RelayCommand(_ => GraphPageF());
            AdministrationPage = new RelayCommand(_ => AdministrationPageF());
            AddAdministrationPage = new RelayCommand(_ => AddAdministrationPageF());
            BackPage = new RelayCommand(_ => BackPageF());
            BackAddPage = new RelayCommand(_ => BackAddPageF());
            BackEditPage = new RelayCommand(_ => BackEditPageF());
            BackPageAdmin = new RelayCommand(_ => BackPageAdminF());
            GenerateWinwow = new RelayCommand(_ => GenerateWindowF());
            MainWindow = new RelayCommand(_ => MainWindowF());
            GenerateMasive = new RelayCommand(_ => GenerateMasiveF());
            AddPerson = new RelayCommand(_ => AddPersonF());
            EditAdministrationPage = new RelayCommand(_ => EditAdministrationPageF());
            NewEditPerson = new RelayCommand(_ => NewEditPersonF());
            AddFather = new RelayCommand<string>(AddFatherF);
            AddMother = new RelayCommand<string>(AddMotherF);
            DeleteAddPerson = new RelayCommand<string>(DeleteAddPersonF);
            AddAddPerson = new RelayCommand<string>(AddAddPersonF);
            AddEditPerson = new RelayCommand<string>(AddEditPersonF);
            DeleteEditPerson = new RelayCommand<string>(DeleteEditPersonF);
            OpenDetailsNewPerson = new RelayCommand<string>(OpenDetailsNewPersonF);
            VerifyAdmin = new RelayCommand(_ => VerifyAdminF());
        }

        private void OpenDetailsNewPersonF(string ForSearch)
        {
            Person person = _addPerson.Find(p => p.ForSearch() == ForSearch);
            if (person == null)
                person = _editPerson.Find(p => p.ForSearch() == ForSearch);
            if (person != null)
            {
                ChooseEditView = person;
                var adminWindow = Application.Current.Windows.OfType<AdministrationPage>().FirstOrDefault();
                if (adminWindow != null)
                {
                    adminWindow.SetWindow.Navigate(new OperationWithPerson(TypeOperation.Administration));
                }
                else throw new Exception("Ви видалили frame");
            }
            else
            {
                MessageBox.Show($"Людину не знайдено{ForSearch}", "Програмна помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteEditPersonF(string ForSearch)
        {
            int index = _editPerson.FindIndex(p => p.ForSearch() == ForSearch);
            if (index >= 0)
            {
                _editPerson.RemoveAt(index);
                MessageBox.Show("Додавання відхилено", "Успішно", MessageBoxButton.OK, MessageBoxImage.Information);
                OnPropertyChanged(nameof(EditedPerson));
                SaveEditPerson();
            }
            else
            {
                MessageBox.Show("Людину не знайдено", "Програмна помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddEditPersonF(string ForSearch)
        {
            int index = _editPerson.FindIndex(p => p.ForSearch() == ForSearch);
            Person editedPerson = null;
            if (index >= 0)
                editedPerson = _editPerson[index];
            Person firstPerson = _persons.Find(p => p.Id.MyId == editedPerson.Id.MyId);

            if (editedPerson != null && firstPerson != null)
            {
                firstPerson.Name = editedPerson.Name;
                firstPerson.Surname = editedPerson.Surname;
                firstPerson.DeathDate = editedPerson.DeathDate;
                SaveToFile();
                _editPerson.RemoveAt(index);
                MessageBox.Show("Редагування прийнято", "Успішно", MessageBoxButton.OK, MessageBoxImage.Information);
                SaveEditPerson();
                OnPropertyChanged(nameof(EditedPerson));
            }
            else
            {
                MessageBox.Show("Людину не знайдено", "Програмна помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteAddPersonF(string ForSearch)
        {
            int index = _addPerson.FindIndex(p => p.ForSearch() == ForSearch);
            if (index >= 0)
            {
                _addPerson.RemoveAt(index);
                MessageBox.Show("Редагування відхилено", "Успішно", MessageBoxButton.OK, MessageBoxImage.Information);
                OnPropertyChanged(nameof(AddedPerson));
                SaveNewPerson();
            }
            else
            {
                MessageBox.Show("Людину не знайдено", "Програмна помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddAddPersonF(string ForSearch)
        {
            int indexf = _addPerson.FindIndex(p => p.ForSearch() == ForSearch);
            if (indexf >= 0)
            {
                Person me = _addPerson[indexf];
                _persons.Add(me);
                _persons[_persons.Count - 1].Id = new PersonId(_persons.Count - 1, me.Id.FatherId, me.Id.MotherId, new List<int>(), new List<int>());

                UpdateIdNewPerson(_persons[_persons.Count - 1]);
                SaveToFile();
                MessageBox.Show("Додавання прийнято", "Успішно", MessageBoxButton.OK, MessageBoxImage.Information);
                _addPerson.RemoveAt(indexf);
                SaveNewPerson();
                OnPropertyChanged(nameof(AddedPerson));
            }
            else
            {
                MessageBox.Show("Людину не знайдено", "Програмна помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewEditPersonF()
        {
            Person firstPerson = _persons[ChoosePersonaId];
            Person editteblePerson = _editPerson[_editPerson.Count - 1];

            if (editteblePerson.Name == firstPerson.Name && editteblePerson.Surname == firstPerson.Surname && editteblePerson.DeathDate == firstPerson.DeathDate)
            {
                MessageBox.Show("Ви не внесли ніяйких змін", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (editteblePerson.Name == "")
            {
                MessageBox.Show("Введіть нове ім'я", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (editteblePerson.Surname == "")
            {
                MessageBox.Show("Введіть нове прізвище", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if ((editteblePerson.DeathDate != null && editteblePerson.DeathDate != DateOnly.MinValue) && editteblePerson.BirthdayDate > editteblePerson.DeathDate)
            {
                MessageBox.Show("Дата народження не може бути більшою за дату смерті", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if ((editteblePerson.DeathDate != null && editteblePerson.DeathDate != DateOnly.MinValue) && editteblePerson.DeathDate > DateOnly.FromDateTime(DateTime.Now))
            {
                MessageBox.Show("Дата народження не може бути в майбутньому", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if ((editteblePerson.DeathDate != null && editteblePerson.DeathDate != DateOnly.MinValue) && editteblePerson.DeathDate > firstPerson.Children.Max(child => child.BirthdayDate))
            {
                MessageBox.Show("Дата народження не ранішою ніж дата народження дитини", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveEditPerson();
            MessageBox.Show("Чекайте схвалення адміністратором", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            MainWindowF();
        }

        private void AddPersonF()
        {
            Person person = _addPerson[_addPerson.Count - 1];

            if (person.Name == "")
            {
                MessageBox.Show("Введіть ім'я", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (person.Surname == "")
            {
                MessageBox.Show("Введіть Прізвище", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (person.Fathername == "")
            {
                MessageBox.Show("Введіть по батькові", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Filter2 != string.Empty)
            {
                var father = _persons.Find(p => p.ForSearch() == Filter2);
                if (father != null)
                {
                    person.Father = father;
                    person.Surname = father.Surname;
                }
                else
                {
                    MessageBox.Show("Батька не знайдено", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (Filter3 != string.Empty)
            {
                var mother = _persons.Find(p => p.ForSearch() == Filter3);
                if (mother != null)
                {
                    person.Mother = mother;
                }
                else
                {
                    MessageBox.Show("Матір не знайдено", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (person.Mother != null)
            {
                int ageDifference = ((DateOnly)person.BirthdayDate).Year - ((DateOnly)person.Mother.BirthdayDate).Year;
                if (ageDifference > 50)
                {
                    MessageBox.Show("Завелика різниця у віці", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (ageDifference < 16)
                {
                    MessageBox.Show("Замаленька різниця у віці", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (person.Father != null)
            {
                int ageDifference = ((DateOnly)person.BirthdayDate).Year - ((DateOnly)person.Father.BirthdayDate).Year;
                if (ageDifference > 50)
                {
                    MessageBox.Show("Завелика різниця у віці", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (ageDifference < 16)
                {
                    MessageBox.Show("Замаленька різниця у віці", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (person.BirthdayDate == null || person.BirthdayDate == DateOnly.MinValue)
            {
                MessageBox.Show("Введіть дату народження", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if ((person.DeathDate != null && person.DeathDate != DateOnly.MinValue) && person.BirthdayDate > person.DeathDate)
            {
                MessageBox.Show("Дата народження не може бути більшою за дату смерті", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if ((person.DeathDate != null && person.DeathDate != DateOnly.MinValue) && person.DeathDate > DateOnly.FromDateTime(DateTime.Now))
            {
                MessageBox.Show("Дата народження не може бути в майбутньому", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UpdateNewPersonID(person);
            SaveNewPerson();
            MainWindowF();
            MessageBox.Show("Чекайте схвалення адміністратором", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BackAddPageF()
        {
            _addPerson.RemoveAt(_addPerson.Count - 1);
            MainWindowF();
        }

        private void BackEditPageF()
        {
            _editPerson.RemoveAt(_editPerson.Count - 1);
            BackPageF();
        }

        private void AddFatherF(string fatherName)
        {
            _filter2 = fatherName;
            OnPropertyChanged(nameof(Filter2));
            Show2 = Visibility.Collapsed;
            _addPerson[_addPerson.Count - 1].Father = _persons[_persons.FindIndex(p => p.ForSearch() == fatherName)];
            _addPerson[_addPerson.Count - 1].Surname = _addPerson[_addPerson.Count - 1].Father.Surname;
            FatherNameEnabled = false;
        }
        private void AddMotherF(string motherName)
        {
            _filter3 = motherName;
            OnPropertyChanged(nameof(Filter3));
            Show3 = Visibility.Collapsed;
        }

        private void AddPersonPageF()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.SetWindow.Navigate(new OperationWithPerson(TypeOperation.Add));
            }
            else throw new Exception("Ви видалили frame");
        }

        private void EditPersonPageF()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.SetWindow.Navigate(new OperationWithPerson(TypeOperation.Edit));
            }
            else throw new Exception("Ви видалили frame");
        }

        public void ViewPersonPageF(string personForSearch = "*")
        {
            if (string.IsNullOrEmpty(personForSearch) || personForSearch.Contains("*невідомо*")) return;
            if (personForSearch != "*")
                ChoosePersonaId = _persons.FindIndex(p => p.ForSearch() == personForSearch);

            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.SetWindow.Navigate(new OperationWithPerson(TypeOperation.View));
            }
            else throw new Exception("Ви видалили frame");
        }

        private void GraphPageF()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.SetWindow.Navigate(new ViewPersonTree());
            }
            else throw new Exception("Ви видалили frame");
        }

        private void AdministrationPageF()
        {
            AdministrationPage window = new AdministrationPage();
            window.ShowDialog();
        }

        private void AddAdministrationPageF()
        {
            var adminWindow = Application.Current.Windows.OfType<AdministrationPage>().FirstOrDefault();
            if (adminWindow != null)
            {
                adminWindow.SetWindow.Navigate(new AddedUsers());
            }
            else throw new Exception("Ви видалили frame");
        }

        private void EditAdministrationPageF()
        {
            var adminWindow = Application.Current.Windows.OfType<AdministrationPage>().FirstOrDefault();
            if (adminWindow != null)
            {
                adminWindow.SetWindow.Navigate(new AddedUsers(TypeOperation.Edit));
            }
            else throw new Exception("Ви видалили frame");
        }

        private void BackPageF()
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

        private void BackPageAdminF()
        {
            var adminWindow = Application.Current.Windows.OfType<AdministrationPage>().FirstOrDefault();
            if (adminWindow != null)
            {
                adminWindow.SetWindow.Navigate(null);
            }
            else throw new Exception("Ви видалили frame");
        }

        private void GenerateWindowF()
        {
            TreeGenWind window = new TreeGenWind();
            window.ShowDialog();
        }

        private void MainWindowF()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.SetWindow.Navigate(null);
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

