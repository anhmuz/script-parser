using System;
using System.Collections.Generic;
using System.Windows;
using ScriptParser;
using System.Windows.Controls;

namespace ScriptParserGui
{
    /// <summary>
    /// Interaction logic for PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : Window
    {
        private Script _script = new Script();

        public PreviewWindow()
        {
            InitializeComponent();
        }

        public Script Script
        {
            get { return _script; }
            set
            {
                _script = value;
                commandsTreeView.Items.Add(MakeCommandsTreeItems(_script));
            }

        }
        
        private static TreeViewItem MakeCommandsTreeItems(Script script)
        {
            var root = new TreeViewItem
            {
                Header = script.Type,
                Tag = script
            };

            var q = new Queue<Tuple<ScriptParser.ICommand, TreeViewItem>>();
            foreach (ScriptParser.ICommand command in script.Commands)
            {
                q.Enqueue(Tuple.Create(command, root));
            }

            while (q.Count != 0)
            {
                var element = q.Dequeue();
                ScriptParser.ICommand command = element.Item1;
                TreeViewItem parent = element.Item2;

                var current = new TreeViewItem
                {
                    Header = command.Type,
                    Tag = command
                };
                parent.Items.Add(current);

                if (command is Script)
                {
                    foreach (ScriptParser.ICommand c in ((Script)command).Commands)
                    {
                        q.Enqueue(Tuple.Create(c, current));
                    }
                }
            }
            return root;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CommandsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = (TreeViewItem)commandsTreeView.SelectedItem;
            var command = (ScriptParser.ICommand)selectedItem.Tag;
            informationTextBox.Text = command.Info;
        }
    }
}
