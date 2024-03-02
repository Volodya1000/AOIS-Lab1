using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class IeeeSumator: BynaryCalculator
    {
        public bool[] sum = new bool[32] ;//1 8 23
        public bool[] N1 = new bool[32];
        public bool[] N2 = new bool[32];
        public IeeeSumator(double n1, double n2)
        {
            N1 = NormalizeBynaryNumber(n1);
            N2 = NormalizeBynaryNumber(n2);
            IeeeSum();
        }

        public IeeeSumator()
        {

        }


        public List<bool> ConvertDrobnPartToBynary(double n)//на вход подаётся только остаток меньше 1
        {
            n = Math.Abs(n);
            List<bool> result = new List<bool>();
            decimal mnog = GetFractionalPart((decimal)n);
            while (result.Count < 61 && (GetFractionalPart(mnog)) != 0)
            {
                mnog = (GetFractionalPart(mnog)) * 2;
                result.Add(Math.Floor(mnog) == 1);
            }
            return result;
        }

        static decimal GetFractionalPart(decimal number)
        {
            string numberAsString = number.ToString(System.Globalization.CultureInfo.InvariantCulture);
            int decimalSeparatorIndex = numberAsString.IndexOf('.');
            if (decimalSeparatorIndex != -1 && decimalSeparatorIndex < numberAsString.Length - 1)
            {
                string fractionalPart = numberAsString.Substring(decimalSeparatorIndex + 1);
                return decimal.Parse("0." + fractionalPart, System.Globalization.CultureInfo.InvariantCulture);
            }
            return 0; // Если нет дробной части, вернуть 0
        }



        public bool [] NormalizeBynaryNumber(double n)
        {
            bool[] rez = new bool[32];
            List<bool> IntPart = base.ConvertDecimalNumberToDirectCode((int)Math.Floor(Math.Abs(n)));
            rez[0] = n < 0; //знак

            List<bool> DrobnPart = ConvertDrobnPartToBynary(n);
            int ExponentValue = 0;
            if (IntPart.Count > 2)
            {
                ExponentValue = IntPart.Count - 2;
                DrobnPart.InsertRange(0, IntPart.GetRange(2, IntPart.Count - 2));// добавление эллементов из IntPart начиная с первого до конца в начало DrobnPart
            }
            else if (IntPart.Count==1 && DrobnPart.Count==0)
            {
                return rez;
            }
            else if(IntPart.Count != 2)
            {
                ExponentValue = (-1) * (DrobnPart.TakeWhile(b => !b).Count()) - 1; //количество false до первого true  и -1 так как убирается первая еденица
                DrobnPart.RemoveRange(0, (-1) * (ExponentValue));
            }
            List<bool> List127 = new List<bool>{false,false,true,true,true, true, true, true ,true};//127
            List<bool> ExpList = base.ConvertReversCodeToAdditional( base.ConvertDirectCodeToRevers( base.ConvertDecimalNumberToDirectCode(ExponentValue)));
            List<bool> ExpList127 = base.Sum(List127, ExpList);


            ExpList127.CopyTo(1, rez,1, ExpList127.Count-1);//копирует все эллемеенты ExpList127 в rez. Вставляет начиная с 1 позиции в rez
            if(DrobnPart.Count>23)
                DrobnPart.RemoveRange(23, DrobnPart.Count - 23);
            DrobnPart.CopyTo(0, rez, 9, DrobnPart.Count); //копирует все эллемеенты DrobnCount в rez. Вставляет начиная с 10 позиции в rez
            return rez;
        }

        public void IeeeSum()
        {
            List<bool> Pokazatel1 = N1.ToList().GetRange(1, 8);
            Pokazatel1.Insert(0, false);
            List<bool> Pokazatel2 = N2.ToList().GetRange(1, 8);
            Pokazatel2.Insert(0, false);
            List<bool> Difference = new();
            List<bool> Mant1 = N1.ToList().GetRange(9, 23);
            Mant1.Insert(0, N1[0]);
            List<bool> Mant2 = N2.ToList().GetRange(9, 23);
            Mant2.Insert(0, N2[0]);
            List<bool> NewMantisa = new List<bool>();
            List<bool> NewPokazatel = new List<bool>();



            Difference = base.Razn(Pokazatel1, Pokazatel2);
            int IntDifference = (int)Math.Abs(base.ConvertBynaryDoubleToDecimalDouble(Difference, new List<bool>()));

            int FirstBiggerThenSecond = base.FirstModuleBiggerThenSecond(Pokazatel1, Pokazatel2);
            
            if (FirstBiggerThenSecond == 1)// показатель1 > показатель
            {
                Mant1.Insert(1, true);
                Mant1.InsertRange(Mant1.Count, Enumerable.Repeat(false, IntDifference));
                Mant2.Insert(1, true);
                Mant2.InsertRange(1, Enumerable.Repeat(false, IntDifference));
                NewPokazatel = Pokazatel1;
            }
            else if (FirstBiggerThenSecond == -1)// показатель1 < показатель2
            {
                Mant2.Insert(1, true);
                Mant2.InsertRange(1, Enumerable.Repeat(false, IntDifference));
                Mant1.Insert(1, true);
                Mant1.InsertRange(Mant1.Count, Enumerable.Repeat(false, IntDifference));
                NewPokazatel = Pokazatel2;
            }
            else if (FirstBiggerThenSecond == 0)// показатель1 = показатель2
            {
                Mant1.Insert(1, true);
                Mant2.Insert(1, true);
                NewPokazatel = Pokazatel1;
            }

            if (Mant1[0])
                Mant1 = ConvertReversCodeToAdditional(ConvertDirectCodeToRevers(Mant1));
            if (Mant2[0])
                Mant2 = ConvertReversCodeToAdditional(ConvertDirectCodeToRevers(Mant2));
            bool R = false;
            NewMantisa = base.Sum1(Mant1, Mant2,ref R);
            int RaznPok = 0;
            sum[0] = NewMantisa[0];
            if (NewMantisa.Count > 25 + IntDifference) //23 + 1 знак + 1 первая еденица
            {
                if(R)
                    NewPokazatel = base.Sum(NewPokazatel, new List<bool> { false, true });
                NewMantisa.RemoveRange(0, 3);
            } 
            else
            {
                RaznPok = NewMantisa.Skip(2).TakeWhile(b => !b).Count();
                if (!NewMantisa[1])
                {
                    NewPokazatel = base.Razn(NewPokazatel, ConvertDecimalNumberToDirectCode(RaznPok + 1));
                    NewMantisa.RemoveRange(0, RaznPok + 3);
                }
                else
                    NewMantisa.RemoveRange(0, RaznPok + 2);
            }
        
                   
               
            NewPokazatel.CopyTo(1 , sum, 1,8);
            

            int c = NewMantisa.Count;
            if (NewMantisa.Count > 23)
                c = 23;
            NewMantisa.CopyTo(0, sum,9, c);
        }

    }
}
