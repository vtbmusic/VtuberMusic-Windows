using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace VtuberMusic_UWP.Models.Main {
    public class LocalSettingsManager : INotifyPropertyChanged {
        [JsonIgnore]
        public string Username {
            get => this.GetSettingVlaue<string>();
            set => this.SetSettingVlaue(value);
        }

        [JsonIgnore]
        public string Password {
            get => this.GetSettingVlaue<string>();
            set => this.SetSettingVlaue(value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public T GetSettingVlaue<T>([CallerMemberName] string key = "") {
            var data = ApplicationData.Current.LocalSettings.Values[key];
            try {
                return data == null ? default : (T)data;
            } catch {
                return default;
            }
        }

        public void SetSettingVlaue(object value, [CallerMemberName] string key = "") {
            this.onPropertyChanged(key);
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }
    }

    public class RoamingSettingsManager : INotifyPropertyChanged {
        public ElementTheme? Theme {
            get {
                var data = this.GetSettingVlaue<int?>();
                return (ElementTheme)data.GetValueOrDefault();
            }
            set {
                (Window.Current.Content as FrameworkElement).RequestedTheme = value.GetValueOrDefault();
                this.SetSettingVlaue(((int?)value).GetValueOrDefault());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public T GetSettingVlaue<T>([CallerMemberName] string key = "") {
            var data = ApplicationData.Current.RoamingSettings.Values[key];
            try {
                return data == null ? default : (T)data;
            } catch {
                return default;
            }
        }

        public void SetSettingVlaue(object value, [CallerMemberName] string key = "") {
            this.onPropertyChanged(key);
            ApplicationData.Current.RoamingSettings.Values[key] = value;
        }
    }
}
