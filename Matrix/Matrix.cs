using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLib
{
    public enum MatrixType
    {
        Zero,
        Identity
    }
    public struct Matrix : ICloneable
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
        public Matrix(MatrixType t, int n)
        {
            Value = new float[n, n];
            Height = n;
            Width = n;
            switch (t)
            {
                case MatrixType.Zero:
                    break;
                case MatrixType.Identity:
                    for (int i = 0; i < n; i++)
                    {
                        Value[i, i] = 1;
                    }
                    break;
            }
        }
        #endregion

        #region Operator
        public static bool operator !=(Matrix left, Matrix right)
        {
            return !left.Equals(right);
        }
        public static bool operator ==(Matrix left, Matrix right)
        {
            return left.Equals(right);
        }
        public static Matrix operator -(Matrix m)
        {
            Matrix result = new Matrix(m.Height, m.Width);
            for (int i = 1; i <= m.Height; i++)
            {
                for (int j = 1; j <= m.Width; j++)
                {
                    result.SetValue(i, j, -m.GetValue(i, j));
                }
            }
            return result;
        }
        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left.Height!=right.Height || left.Width!=right.Width)
            { throw new InvalidOperationException(); }
            else
            {
                Matrix result = new Matrix(left.Height, right.Width);
                for (int i=1;i<=left.Height;i++)
                {
                    for (int j=1;j<=left.Width;j++)
                    {
                        result.SetValue(i, j, left.GetValue(i, j) + right.GetValue(i, j));
                    }
                }
                return result;
            }
        }
        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (left.Height != right.Height || left.Width != right.Width)
            { throw new InvalidOperationException(); }
            else
            {
                //Matrix result = new Matrix(left.Height, right.Width);
                //for (int i = 1; i <= left.Height; i++)
                //{
                //    for (int j = 1; j <= left.Width; j++)
                //    {
                //        result.SetValue(i, j, left.GetValue(i, j) - right.GetValue(i, j));
                //    }
                //}
                //return result;
                return left + (-right);
            }
        }
        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.Width == right.Height)
            {
                Matrix result = new Matrix(left.Height, right.Width);
                for (int i = 1; i <= result.Height; i++)
                {
                    for (int j = 1; j <= result.Width; j++)
                    {
                        float sum = 0;
                        for (int s = 1; s <= left.Width; s++)
                        {
                            sum += left.GetValue(i, s) * right.GetValue(s, j);
                        }
                        result.SetValue(i, j, sum);
                    }
                }
                return result;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        public static Matrix operator *(float left, Matrix right)
        {
            Matrix result = new Matrix(right.Height, right.Width);
            for (int i = 1; i <= result.Height; i++)
            {
                for (int j = 1; j <= result.Width; j++)
                {
                    result.SetValue(i, j, left * right.GetValue(i, j));
                }
            }
            return result;
        }
        public static Matrix Transpose(Matrix m)
        {
            Matrix result = new Matrix(m.Width, m.Height);
            for (int i = 1; i <= result.Height; i++)
            {
                for (int j = 1; j <= result.Width; j++)
                {
                    result.SetValue(i, j, m.GetValue(j, i));
                }
            }
            return result;
        }
        #endregion

        #region Override
        public override bool Equals(object obj)
        {
            Matrix m = (Matrix)obj;
            if (Height == m.Height && Width == m.Width)
            {
                for (int i=1;i<= Height;i++)
                {
                    for (int j=1;j<= Width;j++)
                    {
                        if (GetValue(i, j)!=m.GetValue(i, j))
                        { return false; }
                    }
                }
                return true;
            }
            else { return false; }
        }
        public override string ToString()
        {
            string s = "";
            for (int i = 1; i <= Height; i++)
            {
                for (int j = 1; j <= Width; j++)
                {
                    s += GetValue(i, j).ToString() + ",";
                }
                s += "\r\n";
            }
            return s;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
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

        public Matrix Transpose()
        {
            Matrix result = new Matrix(Width, Height);
            for (int i = 1; i <= result.Height; i++)
            {
                for (int j = 1; j <= result.Width; j++)
                {
                    result.SetValue(i, j, GetValue(j, i));
                }
            }
            return result;
        }

        public bool IsSymmetric()
        {
            return Equals(Transpose());
        }
        public bool IsSingular()
        {
            if (Height != Width) { throw new InvalidOperationException(); }
            return !(this.CalRowEchelonForm() == new Matrix(MatrixType.Identity, Height));
        }


        //Elementary transformation
        public Matrix SwapRow(int rowA, int rowB)
        {
            List<float> temp = new List<float>();
            for (int i = 1; i <= Width; i++)
            {
                temp.Add(GetValue(rowA, i));
                SetValue(rowA, i, GetValue(rowB, i));
            }
            for (int i = 1; i <= Width; i++)
            {
                SetValue(rowB, i, temp[i - 1]);
            }
            return this;
        }
        public Matrix MultiplyConstant(int row, float constant)
        {
            if (constant==0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                for (int i=1;i<= Width; i++)
                {
                    SetValue(row, i, constant * GetValue(row, i));
                }
                return this;
            }
        }
        public Matrix PlusRow(int rowA, int rowB, float times)
        {
            for (int i = 1; i <= Width; i++)
            {
                SetValue(rowA, i, GetValue(rowA, i) + times * GetValue(rowB, i));
            }
            return this;
        }
        #endregion

        #region Static Methods
        public static Matrix CombineByColumn(Matrix left, Matrix right)
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
        public static Matrix SplitByColumn(Matrix m, int final)//return right
        {
            Matrix result = new Matrix(m.Height, m.Width - final);
            for (int i=1;i<=result.Height;i++)
            {
                for (int j=1;j<=result.Width;j++)
                {
                    result.SetValue(i, j, m.GetValue(i, final + j));
                }
            }
            return result;
        }
        #endregion
        public object Clone()
        {
            Matrix result = new Matrix(Height, Width);
            for (int i = 1; i <= result.Height; i++)
            {
                for (int j = 1; j <= result.Width; j++)
                {
                    result.SetValue(i, j, GetValue(i, j));
                }
            }
            return result;
        }

    }
}
