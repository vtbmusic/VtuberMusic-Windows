﻿using System;
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
        public string Username {
            get => this.GetSettingVlaue<string>();
            set => this.SetSettingVlaue(value);
        }

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
            return data == null ? default(T) : (T)data;
        }

        public void SetSettingVlaue(object value, [CallerMemberName] string key = "") {
            this.onPropertyChanged(key);
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }
    }

    public class RoamingSettingsManager : INotifyPropertyChanged {
        public ElementTheme? Theme {
            get {
                var data = this.GetSettingVlaue<string>();
                switch (data) {
                    case "Dark":
                        return ElementTheme.Dark;
                    case "Light":
                        return ElementTheme.Light;
                    default:
                        return ElementTheme.Default;
                }
            }
            set {
                App.RootFrame.RequestedTheme = value.GetValueOrDefault();
                this.SetSettingVlaue(value.ToString());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public T GetSettingVlaue<T>([CallerMemberName] string key = "") {
            var data = ApplicationData.Current.RoamingSettings.Values[key];
            return data == null ? default(T) : (T)data;
        }

        public void SetSettingVlaue(object value, [CallerMemberName] string key = "") {
            this.onPropertyChanged(key);
            ApplicationData.Current.RoamingSettings.Values[key] = value;
        }
    }
}
