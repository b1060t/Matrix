using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    public static class Calculator
    {
        public static Matrix CalRowEchelonForm(this Matrix m)
        {
            int columnPtr = 1;
            for (int rowPtr = 1; rowPtr <= m.Height; rowPtr++)
            {
                if (FindRow(m, rowPtr, columnPtr) != 0)
                {
                    m.MultiplyConstant(rowPtr, 1 / m.GetValue(rowPtr, columnPtr));
                    for (int i = /*rowPtr + */1; i <= m.Height; i++)
                    {
                        m.PlusRow(i, rowPtr, -m.GetValue(i, columnPtr));
                    }
                }
                columnPtr++;
            }
            return m;
        }
        public static Matrix CalTranspose(this Matrix m)
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
        public static Matrix CalInvertible(this Matrix m)
        {
            if (m.IsSingular()) { throw new InvalidOperationException(); }
            Matrix temp = Matrix.CombineByColumn(m, new Matrix(MatrixType.Identity, m.Width));
            temp = temp.CalRowEchelonForm();
            temp = Matrix.SplitByColumn(temp, m.Width / 2);
            return temp;
        }
        private static int FindRow(Matrix m, int rowPtr, int columnPtr)
        {
            for (int i = rowPtr; i <= m.Height; i++)
            {
                if (m.GetValue(i, columnPtr) == 0) { continue; }
                else
                {
                    if (i != rowPtr)
                    {
                        m.SwapRow(rowPtr, i);
                    }
                    return rowPtr;
                }
            }
            return 0;
        }
    }
}
