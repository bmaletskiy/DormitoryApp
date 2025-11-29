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

            loadXmlButton.Clicked += LoadXmlButton_Clicked;
            searchButton.Clicked += SearchButton_Clicked;
            clearButton.Clicked += ClearButton_Clicked;
            transformButton.Clicked += TransformButton_Clicked;

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

                // Завантаження факультетів
                var faculties = GetAttributesFromXml(selectedXmlFile, "faculty");
                facultyPicker.ItemsSource = faculties;

                if (faculties.Count > 0)
                    facultyPicker.SelectedIndex = 0;
            }
            else
            {
                await DisplayAlert("Помилка", "Будь ласка, оберіть XML файл", "OK");
            }
        }

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

            string name = keywordEntry.Text ?? "";
            string faculty = facultyPicker.SelectedItem?.ToString() ?? "";
            string department = departmentEntry.Text ?? "";
            string course = courseEntry.Text ?? "";

            var results = currentStrategy.Search(
                selectedXmlFile,
                name,
                faculty,
                department,
                course
            );

            resultsCollectionView.ItemsSource = results;
        }


        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            keywordEntry.Text = "";
            departmentEntry.Text = "";
            courseEntry.Text = "";
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
                var xslResult = await FilePicker.Default.PickAsync();

                if (xslResult == null)
                {
                    await DisplayAlert("Помилка", "Не обрано XSL файл!", "OK");
                    return;
                }

                string xslFilePath = xslResult.FullPath;
                string htmlOutputPath = Path.Combine(
                    Path.GetDirectoryName(selectedXmlFile),
                    "output.html"
                );

                var transformer = new XmlToHtmlTransformer();
                transformer.TransformXmlToHtml(selectedXmlFile, xslFilePath, htmlOutputPath);

                await DisplayAlert("Готово", $"HTML файл створено:\n{htmlOutputPath}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка при трансформації", ex.Message, "OK");
            }
        }

       private async void OnExitClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Вихід", "Ви дійсно хочете вийти з програми?", "Так", "Ні");
        if (answer)
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
    }
    }
}
