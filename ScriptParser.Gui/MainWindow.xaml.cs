using System;
using System.Threading;
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
using System.Windows.Threading;

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
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
            progressBar.Value = progress;
            resultTextBox.Text += string.Format("Executed {0} %\n", progress);
        }
        
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                scriptPathTextBox.Text = dlg.FileName;
            }
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

            resultTextBox.Text = string.Empty;
            resultTextBox.Foreground = SystemColors.WindowTextBrush;

            try
            {
                var sp = new ScriptParser.ScriptParser();
                sp.ParseScript(scriptPath);
                sp.ParsedScript.Progress += PrintProgress;
                sp.ParsedScript.Execute();
                return;
            }
            catch (ScriptParserException exception)
            {
                resultTextBox.Foreground = new SolidColorBrush(Colors.Red);
                resultTextBox.Text = exception.Message + string.Format(
                    "\npath: {0} line: {1} column: {2}",
                    exception.errorSource, exception.line, exception.column);
            }
            catch (Exception exception)
            {
                resultTextBox.Foreground = new SolidColorBrush(Colors.Red);
                resultTextBox.Text = exception.Message;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            scriptPathTextBox.Focus();
        }
    }
}
