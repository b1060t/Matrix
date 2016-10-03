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
                    for (int i = rowPtr + 1; i <= m.Height; i++)
                    {
                        m.PlusRow(i, rowPtr, -m.GetValue(i, columnPtr));
                    }
                }
                columnPtr++;
            }
            return m;
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
