using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MonoDistro.WebApi
{
    public class Setting
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class SettingsCollection : ICollection<Setting>
    {
        public Setting this[string key]
        {
            get
            {
                return _data.FirstOrDefault(x => x.Key.Equals(key));
            }
        }

        public List<Setting> _data = new List<Setting>();

        public int Count => _data.Count;

        public Setting Get(string id)
        {
            return _data.FirstOrDefault(x => x.Key.Equals(id));
        }

        public bool IsReadOnly => false;

        public void Add(string key, string value)
        {
            Add(new Setting { Key = key, Value = value });
        }

        public void Add(Setting item)
        {
            if (Contains(item))
            {
                throw new Exception("Your setting in already collection.");
            }
            _data.Add(item);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(Setting item)
        {
            return _data.Any(x => x.Key.Equals(item.Key));
        }

        public void CopyTo(Setting[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Setting> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public bool Remove(Setting item)
        {
            return _data.Remove(item);
        }

        public bool Remove(string key)
        {
            return Remove(_data.FirstOrDefault(x => x.Key.Equals(key)));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}