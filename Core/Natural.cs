using static System.Math;

namespace Topos.Core
{
    /// <summary>
    /// Natural numbers are nonnegative integers.
    /// </summary>
    public class Natural : Integer
    {
        private uint m_value;
        public override double Value {
            get
            {
                return m_value;
            }
            set
            {
                m_value = (uint) Abs(value);
            }
        }

        /// <summary>
        /// Creates a natural number that equals to 0
        /// </summary>
        public Natural()
        {
            Value = 0;
        }

        /// <summary>
        /// Creates a natural number
        /// </summary>
        /// <param name="value">Value of the natural number</param>
        public Natural(uint value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static implicit operator Natural(uint i) => new Natural(i);
        public static implicit operator Natural(int i) => new Natural((uint) Abs(i));
        public static implicit operator Natural(Complex c) => new Natural((uint) c.Real.Value);
        public static implicit operator uint(Natural i) => (uint) i.Value;
        public static implicit operator double(Natural i) => (double) i.Value;
    }
}
