using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using ClickMapper.Model;
using Gma.System.MouseKeyHook;
using InputManager;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Point = System.Drawing.Point;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace ClickMapper.View.MainWindow.Pages
{
    /// <summary>
    ///     Interaction logic for MainPage.xaml
    /// </summary>
    public sealed partial class MainPage : INotifyPropertyChanged
    {
        private ObservableCollection<KeyMapping> _mappings = new ObservableCollection<KeyMapping>();
        private int _mouseX;
        private int _mouseY;

        public MainPage()
        {
            InitializeComponent();
            DataContext = this;

            Mappings.Add(new KeyMapping("F1", 100, 100));
            Mappings.Add(new KeyMapping("F2", 200, 200));
            Mappings.Add(new KeyMapping("F3", 300, 300));
            Mappings.Add(new KeyMapping("F4", 400, 400));
            Mappings.Add(new KeyMapping("F5", 500, 500));
            Mappings.Add(new KeyMapping("F6", 600, 600));


            GlobalHooks.KeyDown += GlobalHookOnKeyDown;
            GlobalHooks.MouseMove += GlobalHookOnMouseMove;

            void GlobalHookOnKeyDown(object sender, KeyEventArgs e)
            {
                Console.WriteLine(@"Key Pressed: {0}", e.KeyCode);
                if (e.KeyData.ToString().ToUpper() == "F12")
                {
                    Console.WriteLine(@"F12 pressed, open ""New Key Mapping Window""");
                    NewMappingPage page = new NewMappingPage(MouseX, MouseY);
                    Window dialog = new Window
                    {
                        Title = "New Mapping",
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Owner = (Window)Parent,
                        Content = page
                    };
                    dialog.ShowDialog();
                    if (page.Mapping?.IsValid() ?? false)
                    {
                        Mappings.Add(page.Mapping);
                    }
                }
                else
                {
                    foreach (KeyMapping map in Mappings)
                    {
                        if (map.IsValid())
                        {
                            if (e.KeyData.ToString().ToUpper() == map.Key.ToUpper())
                            {
                                Console.WriteLine(@"Matching Key Mapping found, set mouse postion to {0} / {1}", map.X, map.Y);
                                System.Windows.Forms.Cursor.Position = new Point(map.X, map.Y);
                                Mouse.PressButton(Mouse.MouseKeys.Left);
                            }
                        }
                    }
                }
            }

            void GlobalHookOnMouseMove(object sender, MouseEventArgs e)
            {
                MouseX = e.X;
                MouseY = e.Y;
                Console.WriteLine(@"New Mouse Position: {0} / {1}", MouseX, MouseY);
            }
        }

        /// <summary>
        ///     Gets or sets the key mappings.
        /// </summary>
        [UsedImplicitly]
        public ObservableCollection<KeyMapping> Mappings
        {
            get => _mappings;
            set
            {
                _mappings = value;
                NotifyPropertyChanged();
            }
        }

        [UsedImplicitly]
        public int MouseX
        {
            get => _mouseX;
            set
            {
                _mouseX = value;
                NotifyPropertyChanged();
            }
        }

        [UsedImplicitly]
        public int MouseY
        {
            get => _mouseY;
            set
            {
                _mouseY = value;
                NotifyPropertyChanged();
            }
        }

        private IKeyboardMouseEvents GlobalHooks { get; } = Hook.GlobalEvents();

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "JSON files|*.json",
                Multiselect = false
            };
            if (ofd.ShowDialog((Window)Parent).GetValueOrDefault(false))
            {
                Mappings.Clear();
                using (JsonTextReader reader = new JsonTextReader(new StreamReader(new FileStream(ofd.FileName, FileMode.Open))))
                {
                    var mappings =
                        from jsonMapping in JToken.ReadFrom(reader)
                        select new KeyMapping(jsonMapping["Key"].ToObject<string>(), jsonMapping["X"].ToObject<int>(), jsonMapping["Y"].ToObject<int>());
                    foreach (KeyMapping mapping in mappings)
                    {
                        Mappings.Add(mapping);
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = ".json",
                Filter = "JSON files|*.json"
            };
            if (sfd.ShowDialog((Window)Parent).GetValueOrDefault(false))
            {
                JArray jsonMappings = new JArray(from mapping in Mappings select mapping.ToJson());
                File.WriteAllText(sfd.FileName, jsonMappings.ToString());
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            Console.WriteLine(@"MainPage property changed: {0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}