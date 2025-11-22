using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using DormitoryApp.Strategy;
using DormitoryApp.Transform;
using System.Threading.Tasks;

namespace DormitoryApp
{
    public partial class MainPage : ContentPage
    {
        private IXmlSearchStrategy currentStrategy;
        private string selectedXmlFile;

        public MainPage()
        {
            InitializeComponent();

            // Підписка на події кнопок
            loadXmlButton.Clicked += LoadXmlButton_Clicked;
            searchButton.Clicked += SearchButton_Clicked;
            clearButton.Clicked += ClearButton_Clicked;
            transformButton.Clicked += TransformButton_Clicked;

            // Підписка на зміну Picker для стратегії
            strategyPicker.SelectedIndexChanged += StrategyPicker_SelectedIndexChanged;
        }

        private void StrategyPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strategyPicker.SelectedItem == null) return;

            switch (strategyPicker.SelectedItem.ToString())
            {
                case "SAX":
                    currentStrategy = new SaxSearchStrategy();
                    break;
                case "DOM":
                    currentStrategy = new DomSearchStrategy();
                    break;
                case "LINQ to XML":
                    currentStrategy = new LinqXmlSearchStrategy();
                    break;
            }
        }

        private async void LoadXmlButton_Clicked(object sender, EventArgs e)
        {
            var result = await FilePicker.Default.PickAsync();
            if (result != null && result.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                selectedXmlFile = result.FullPath;
                xmlFileLabel.Text = result.FileName;

                // Завантажуємо факультети для Picker
                var faculties = GetAttributesFromXml(selectedXmlFile, "faculty");
                facultyPicker.ItemsSource = faculties;

                if (faculties.Count > 0)
                    facultyPicker.SelectedIndex = 0; // Вибрати перший елемент автоматично
            }
            else
            {
                await DisplayAlert("Помилка", "Будь ласка, оберіть XML файл", "OK");
            }
        }

        // Метод для отримання унікальних значень атрибутів (наприклад, faculty)
        private List<string> GetAttributesFromXml(string xmlFile, string attributeName)
        {
            var list = new List<string>();
            var doc = new System.Xml.XmlDocument();
            doc.Load(xmlFile);

            foreach (System.Xml.XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.Attributes != null && node.Attributes[attributeName] != null)
                {
                    string val = node.Attributes[attributeName].Value;
                    if (!list.Contains(val))
                        list.Add(val);
                }
            }

            return list;
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            if (currentStrategy == null || string.IsNullOrEmpty(selectedXmlFile))
            {
                DisplayAlert("Помилка", "Оберіть файл та метод аналізу!", "OK");
                return;
            }

            string query = keywordEntry.Text ?? string.Empty;
            string selectedFaculty = facultyPicker.SelectedItem?.ToString();

            // Виконуємо пошук через стратегію
            var results = currentStrategy.Search(selectedXmlFile, query);

            // Фільтруємо за ПІБ
            if (!string.IsNullOrEmpty(query))
            {
                results = results.FindAll(s => s.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            // Фільтруємо за факультетом
            if (!string.IsNullOrEmpty(selectedFaculty))
            {
                results = results.FindAll(s => s.Faculty.Equals(selectedFaculty, StringComparison.OrdinalIgnoreCase));
            }

            // Передаємо результат у CollectionView
            resultsCollectionView.ItemsSource = results;
        }

        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            keywordEntry.Text = string.Empty;
            facultyPicker.SelectedIndex = -1;
            resultsCollectionView.ItemsSource = null;
        }

        private async void TransformButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedXmlFile))
            {
                await DisplayAlert("Помилка", "Будь ласка, оберіть XML файл!", "OK");
                return;
            }

            try
            {
                // Відкриваємо FilePicker для обрання XSL
                var xslResult = await FilePicker.Default.PickAsync(); // просто відкриває провідник

                if (xslResult == null)
                {
                    await DisplayAlert("Помилка", "Не обрано XSL файл!", "OK");
                    return;
                }

                string xslFilePath = xslResult.FullPath;

                // HTML файл у тій же директорії, що й XML
                string htmlOutputPath = Path.Combine(Path.GetDirectoryName(selectedXmlFile), "output.html");

                var transformer = new XmlToHtmlTransformer();
                transformer.TransformXmlToHtml(selectedXmlFile, xslFilePath, htmlOutputPath);

                await DisplayAlert("Готово", $"HTML файл створено:\n{htmlOutputPath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка при трансформації", ex.Message, "OK");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool answer = await DisplayAlert("Вихід", "Чи дійсно ви хочете завершити роботу?", "Так", "Ні");
                if (answer) Application.Current.Quit();
            });

            return true;
        }
    }
}
