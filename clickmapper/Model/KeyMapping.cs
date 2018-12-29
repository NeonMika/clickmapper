using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ClickMapper.Model
{
    /// <inheritdoc />
    /// <summary>
    ///     The key mapping class, representing the press of a certain key to a click a certain screen location.
    /// </summary>
    public sealed class KeyMapping : INotifyPropertyChanged
    {
        private string _key;

        private int _x;

        private int _y;

        /// <summary>
        ///     Initializes a new instance of the <see cref="KeyMapping" /> class.
        ///     This parameter-less constructor is used by <see cref="DataGrid" /> when creating a new row.
        /// </summary>
        [UsedImplicitly]
        public KeyMapping() { }

        public KeyMapping(string key, int x, int y) => (Key, X, Y) = (key, x, y);

        [UsedImplicitly]
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                NotifyPropertyChanged();
            }
        }

        [UsedImplicitly]
        public int X
        {
            get => _x;
            set
            {
                _x = value;
                NotifyPropertyChanged();
            }
        }

        [UsedImplicitly]
        public int Y
        {
            get => _y;
            set
            {
                _y = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsValid() => !string.IsNullOrEmpty(Key) && (X >= 0) && (Y >= 0);

        public string ToJson() => JsonConvert.SerializeObject(this);

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            Console.WriteLine(@"KeyMapping property changed: {0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


    }
}