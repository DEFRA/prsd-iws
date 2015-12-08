namespace EA.Iws.Domain
{
    using System;
    using System.Text.RegularExpressions;

    public class PhoneNumber
    {
        private static Regex phoneNumberRegex = CreateRegex();

        private static Regex CreateRegex()
        {
            return
                new Regex("^[+]?[\\d]+(( |-)?[\\d]+)+?$",
                    RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        }

        public PhoneNumber(string phoneNumber)
        {
            if (!phoneNumberRegex.IsMatch(phoneNumber))
            {
                throw new ArgumentException("Phone number was not valid", "phoneNumber");
            }

            this.Value = phoneNumber;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(PhoneNumber rhs)
        {
            return rhs.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals(PhoneNumber other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((PhoneNumber)obj);
        }

        public static bool operator ==(PhoneNumber left, PhoneNumber right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PhoneNumber left, PhoneNumber right)
        {
            return !Equals(left, right);
        }
    }
}