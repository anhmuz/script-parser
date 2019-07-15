using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ScriptParser;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScriptParserGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void PrintProgress(int progress)
        {
            resultTextBox.Text += string.Format("Executed {0} %\n", progress);
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            var scriptPath = scriptPathTextBox.Text;
            if (!File.Exists(scriptPath))
            {
                resultTextBox.Foreground = new SolidColorBrush(Colors.Red);
                resultTextBox.Text = string.Format(
                    "ScriptParser.exe: failed to access '{0}': No such file",
                    scriptPath);
                return;
            }

            try
            {
                var sp = new ScriptParser.ScriptParser();
                sp.ParseScript(scriptPath);
                sp.ParsedScript.Progress += PrintProgress;
                sp.ParsedScript.Execute();
                return;
            }
            catch (ScriptParserException exeption)
            {
                resultTextBox.Foreground = new SolidColorBrush(Colors.Red);
                resultTextBox.Text = exeption.Message + string.Format("\npath: {0} line: {1} column: {2}",
                    exeption.errorSource, exeption.line, exeption.column);
                return;
            }
        }
    }
}
