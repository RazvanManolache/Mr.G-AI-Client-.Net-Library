using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MrG.AI.Client
{
    public class ObservableObject : INotifyPropertyChanged
    {
       public static bool SetProperty<T>(object o, string field_name, T value)
        {
            if(o==null)
            {
                return false;
            }
            if(o is ObservableObject)
            {
                var obj = o as ObservableObject;
                if(obj==null)
                {
                    return false;
                }
                return obj.SetProperty(field_name, value);
            }
            return false;
        }

        public bool SetProperty<T>(string field_name, T value)
        {
            var type = this.GetType();
            //var field = type.GetField(field_name);
            //if (field!=null)
            //{
            //    field.SetValue(this, value);
            //    return true;
                
            //}
            var prop = type.GetProperty(field_name);
            if (prop != null)
            {
                
                prop.SetValue(this, value);
                OnPropertyChanged(field_name);
                return true;
            }
            return false;
        }

        public bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
