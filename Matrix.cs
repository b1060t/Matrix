using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    public enum Type
    {
        Zero,
        Identity
    }
    public class Matrix
    {
        private float[,] Value { get; set; }
        public int Height { get; }
        public int Width { get; }
        #region Constructor
        public Matrix(int height, int width)
        {
            Value = new float[height, width];
            Height = height;
            Width = width;
        }
        public Matrix(Type t, int n)
        {
            Value = new float[n, n];
            switch (t)
            {
                case Type.Zero:
                    break;
                case Type.Identity:
                    for (int i = 0; i < n; i++)
                    {
                        Value[i, i] = 1;
                    }
                    break;
            }
        }
        #endregion

        #region Methods
        public Matrix SetValue(int row, int column, float value)
        {
            Value[row - 1, column - 1] = value;
            return this;
        }
        public float GetValue(int row, int column)
        {
            return Value[row - 1, column - 1];
        }
        #endregion

        #region Static Methods
        public static Matrix Combine(Matrix left, Matrix right)
        {
            if (left.Height == right.Height)
            {
                Matrix result = new Matrix(left.Height, left.Width + right.Width);
                for (int m = 1; m <= result.Height; m++)
                {
                    for (int n = 1; n <= result.Width; n++)
                    {
                        if (n<=left.Width)
                        {
                            result.SetValue(m, n, left.GetValue(m, n));
                        }
                        else
                        {
                            result.SetValue(m, n, right.GetValue(m, n - left.Width));
                        }
                    }
                }
                return result;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }
        #endregion
    }
}
