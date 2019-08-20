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
        private Script _script = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WorkerThreadMain(string scriptPath)
        {
            try
            {
                var sp = new ScriptParser.ScriptParser();
                sp.ParseScript(scriptPath);
                _script = sp.ParsedScript;
                sp.ParsedScript.Progress += WorkerThreadHandleProgress;
                sp.ParsedScript.Execute();
            }
            catch (OperationCanceledException)
            {
            }
            catch (ScriptParserException exception)
            {
                WorkerThreadHandleError(exception.Message + string.Format(
                    "\npath: {0} line: {1} column: {2}",
                    exception.errorSource, exception.line, exception.column));
            }
            catch (Exception exception)
            {
                WorkerThreadHandleError(exception.Message);
            }
            finally
            {
                _script = null;
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        new Action(() =>
                        {
                            executeButton.IsEnabled = true;
                        }), null);
                }
            }
        }

        private void WorkerThreadHandleProgress(int progress)
        {
            Application.Current.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    progressBar.Value = progress;
                    resultTextBox.Text += string.Format("Executed {0} %\n", progress);
                    resultTextBox.ScrollToEnd();
                }), null);
        }

        private void WorkerThreadHandleError(string text)
        {
            Application.Current.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    resultTextBox.Foreground = new SolidColorBrush(Colors.Red);
                    resultTextBox.Text = text;
                }), null);
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
            executeButton.IsEnabled = false;

            new Thread(() => WorkerThreadMain(scriptPath)).Start();
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            var text = File.ReadAllText(scriptPathTextBox.Text);
            var scriptContentTextBox = new TextBox() { Text = text };

            var closeButton = new Button() { Content = "Close", Width = 100, Height = 30,
                HorizontalAlignment = HorizontalAlignment.Right };
            
            Grid.SetRow(scriptContentTextBox, 0);
            Grid.SetRow(closeButton, 1);
            grid.Children.Add(scriptContentTextBox);
            grid.Children.Add(closeButton);

            var window = new Window { Title = "ScriptPreview", Height = 300, Width = 500,
                Content = grid, Owner = this, ResizeMode = ResizeMode.NoResize };
            closeButton.Click += (source, arg) =>
            {
                window.Close();
            };
            window.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            scriptPathTextBox.Focus();
        }
        
        private void Window_Closed(object sender, EventArgs e)
        {
            if (_script != null)
            {
                _script.Cancel();
            }
        }
    }
}
