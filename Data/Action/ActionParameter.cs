using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MrG.AI.Client.Data.Action
{
    public class ActionParameter : ObservableObject
    {
        private string _name;
        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private string _type;

        [JsonProperty("field_type")]
        public string Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }
        private bool _optional;

        [JsonProperty("optional")]
        public bool Optional
        {
            get { return _optional; }
            set { SetProperty(ref _optional, value); }
        }
        private string _defaultValue;
        [JsonProperty("default_value")]
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { SetProperty(ref _defaultValue, value); }
        }

        private string _value;
        [JsonIgnore]
        public string Value
        {
            get { if (_value != null) return _value; return _defaultValue; }
            set { SetProperty(ref _value, value); }
        }

        public void ClearValue()
        {
            Value = null;
        }
        [JsonIgnore]
        public ActionParameterCollectionValue SelectValue
        {
            get
            {
                if (Collection != null)
                {
                    if (Collection.Values != null && Collection.Values.Length > 0)
                    {

                        if (Value != null && Collection.Values.Where(a => a.Value == Value).Count() > 0)
                        {
                            return Collection.Values.Where(a => a.Value == Value).First();
                        }
                        var first = Collection.Values.FirstOrDefault();
                        if (first != null)
                        {
                            return first;
                        }
                    }
                }

                return null;
            }
            set
            {
                if (value != null)
                    Value = value.Value;
                Value = null;
            }
        }
        [JsonIgnore]
        public int IntValue
        {
            get
            {
                return (int)FloatValue;
            }
            set { Value = value.ToString(); }
        }
        [JsonIgnore]
        public float FloatValue
        {
            get
            {
                object val = Value;
                if (val == null)
                {
                    if (Collection != null)
                    {
                        if (Collection.Interval != null)
                        {
                            val = Collection.Interval.Min;
                        }
                        if (Collection.Values != null && Collection.Values.Length > 0)
                        {
                            var first = Collection.Values.FirstOrDefault();
                            if (first != null)
                            {
                                val = first.Value;
                            }
                        }
                    }
                }
                if (val == null)
                {
                    return 0f;
                }
                var strVal = val.ToString();
                if (string.IsNullOrEmpty(strVal))
                {
                    return 0f;
                }
                float.TryParse(strVal, NumberStyles.Float, CultureInfo.InvariantCulture, out float result);
                result = (float)Math.Round(result, 2);
                return result;

            }
            set { Value = value.ToString(); }
        }
        [JsonIgnore]
        public bool BoolValue
        {
            get
            {
                bool.TryParse(Value, out bool result);
                return result;
            }
            set { Value = value.ToString(); }
        }
        private string _ImageValue;
        [JsonIgnore]
        public string ImageValue
        {
            get
            {
                return _ImageValue;
            }
            set { _ImageValue = value; }
        }


        private ActionParameterCollection _collection;
        [JsonProperty("collection")]
        public ActionParameterCollection Collection
        {
            get { return _collection; }
            set { SetProperty(ref _collection, value); }
        }


    }
}
