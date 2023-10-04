using System;
using UnityEngine;

namespace Common.Data
{
    [Serializable]
    public struct Id : IComparable, IComparable<string>, IEquatable<string>, IEquatable<Id>
    {
        [SerializeField] private string value;
        
        //[JsonConstructor]
        public Id(string value)
        {
            this.value = value;
        }

        public Id(Guid value)
        {
            this.value = value.ToString();
        }

        public static Id Create()
        {
            Id result;
            result.value = Guid.NewGuid().ToString();
            return result;
        }

        public bool IsZero => string.IsNullOrEmpty(value);
        public static Id GetZero()
        {
            Id result;
            result.value = string.Empty;
            return result;
        }

        public string GetValue()
        {
            return value;
        }
        

        #region interfaces
        public int CompareTo(object other)
        {
            if (other == null)
            {
                return 1;
            }

            if (other is Id id)
            {
                return string.Compare(id.value, value, StringComparison.Ordinal);
            }

            return 0;
        }

        public int CompareTo(string other)
        {
            return string.Compare(value, other, StringComparison.Ordinal);
        }

        public bool Equals(string other)
        {
            return string.Equals(value, other, StringComparison.Ordinal);
        }

        public bool Equals(Id other)
        {
            return value == other.value;
        }

        public override int GetHashCode()
        {
            return -1939223833 + value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return value;
        }

        public static bool operator ==(Id a, Id b)
        {
            return string.Equals(a.value, b.value, StringComparison.Ordinal);
        }

        public static bool operator !=(Id a, Id b)
        {
            return !string.Equals(a.value, b.value, StringComparison.Ordinal);
        }

        public static implicit operator Id(Guid v) => new Id(v);
        public static implicit operator Id(string v) => new Id(v);
        public static implicit operator string(Id v) => v.value.ToString();
        #endregion
    }
}
