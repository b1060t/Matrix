using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLib;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix m = new Matrix(3, 3);
            m.SetValue(1, 1, 1);
            m.SetValue(1, 2, 4);
            m.SetValue(1, 3, 3);
            m.SetValue(2, 1, -1);
            m.SetValue(2, 2, -2);
            m.SetValue(2, 3, 0);
            m.SetValue(3, 1, 2);
            m.SetValue(3, 2, 2);
            m.SetValue(3, 3, 3);
            //Console.Write(m.CalInvertible().ToString());
            Matrix temp = Matrix.CombineByColumn(m, new Matrix(MatrixType.Identity, m.Height)).CalRowEchelonForm();
            Console.Write(Matrix.SplitByColumn(temp, temp.Width/2).ToString());
            Console.ReadLine();
        }
    }
}
