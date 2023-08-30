using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhasmoSaveEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhasmophobiaSaveEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataManager dataManager;
        PhasmoDataDeclaration phasmoData;

        public MainWindow()
        {
            InitializeComponent();
            dataManager = new DataManager();
            dataManager.init();
            phasmoData = new PhasmoDataDeclaration(File.ReadAllText(dataManager.decryptedPath));
            readFromSave();

        }
        private void readFromSave() {
            money_value.Content =  phasmoData.values.PlayersMoney.value;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            
        }

        private void money_plus_Click(object sender, RoutedEventArgs e)
        {
            money_value.Content = Convert.ToInt64(money_value.Content) + 1;
        }

        private void money_minus_Click(object sender, RoutedEventArgs e)
        {
            long oldValue = Convert.ToInt64(money_value.Content);
            if (oldValue == 0) return;
            money_value.Content =  oldValue-1;
        }
    }
}
