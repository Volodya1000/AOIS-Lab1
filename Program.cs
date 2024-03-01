static void main()
{

}

namespace Lab1
{
    public class BynaryCalculator
    {
        int N;

        public List<bool> DirectCode;
        public List<bool> ReversCode;
        public List<bool> AdditionalCode;


        public BynaryCalculator(int number, int n)
        {
            N = n;

            DirectCode = ConvertDecimalNumberToDirectCode(number);
            ReversCode = ConvertDirectCodeToRevers(DirectCode);
            AdditionalCode = ConvertReversCodeToAdditional(ReversCode);
        }

        public BynaryCalculator()
        {
        }

        public static int[] ConvertBoolArrayToIntArray(bool[] boolArray)
        {
            return boolArray.Select(b => b ? 1 : 0).ToArray();
        }

        public static bool[] ConvertBoolListToBoolArray(List<bool> BoolList, int n)
        {
            BoolList.InsertRange(1, Enumerable.Repeat(false, n - BoolList.Count).ToList());
            return BoolList.ToArray();
        }

        public int[] ConvertBoolListToIntArray(List<bool> BoolList)
        {
            BoolList.InsertRange(0, Enumerable.Repeat(false, N - BoolList.Count));
            return BoolList.Select(b => b ? 1 : 0).ToArray();
        }

        public static List<bool> SetLength(List<bool> BoolList, int n)
        {
            if (BoolList.Count == 0)
                BoolList = new List<bool>(Enumerable.Repeat(false, n));
            else if (BoolList[0])
                BoolList.InsertRange(0, Enumerable.Repeat(true, n - BoolList.Count).ToList());
            else
                BoolList.InsertRange(0, Enumerable.Repeat(false, n - BoolList.Count).ToList());
            return BoolList;
        }

        public List<bool> ConvertDecimalNumberToDirectCode(int Nubmer)
        {
            List<bool> NumberDv = new List<bool>();
            int Delimoe = Nubmer;
            while (Delimoe / 2 != 0)
            {
                NumberDv.Insert(0, !(Delimoe % 2 == 0));
                Delimoe = Delimoe / 2;
            }
            if(Nubmer != 0)//для того чтоб 0 был false, а не false false
               NumberDv.Insert(0, Delimoe != 0);
            NumberDv.Insert(0, Nubmer < 0);
            return NumberDv;
        }

        public List<bool> ConvertDirectCodeToRevers(List<bool> NumberPr)
        {
            if (!NumberPr[0])
                return NumberPr;
            List<bool> NumberRv = new List<bool>();
            for (int i = 1; i < NumberPr.Count(); i++)
                NumberRv.Add(!NumberPr[i]);
            NumberRv.Insert(0, true);
            return NumberRv;
        }


        public List<bool> ConvertReversCodeToAdditional(List<bool> NumberObr)
        {
            if (!NumberObr[0])
                return NumberObr;
            List<bool> NumberAdd = new List<bool>();
            bool R = true; //число в следующем разряде увеличивается на 1
            for (int i = NumberObr.Count - 1; i > 0; i--)
            {
                NumberAdd.Insert(0, R ^ NumberObr[i]);
                R = R && NumberObr[i];
            }
            NumberAdd.Insert(0, true);
            return NumberAdd;
        }


        public List<bool> Sum(List<bool> Number1)//входные массивы в дополнительном коде
        {
            return Sum(AdditionalCode, Number1);
        }

        public List<bool> Sum(List<bool> Num, List<bool> Num1)//входные массивы в дополнительном коде
        {
            List<bool> Number = new List<bool>(Num);
            List<bool> Number1 = new List<bool>(Num1);
            int NN;
            if (Number.Count > Number1.Count)
            {
                NN = Number.Count;
                SetLength(Number1, NN);
            }
            else
            {
                NN = Number1.Count;
                Number = SetLength(Number, NN);
            }
            List<bool> Rez = new List<bool>();
            bool R = false; //число в следующем разряде увеличивается на 1
            for (int i = NN - 1; i >= 0; i--)
            {
                Rez.Insert(0, Number[i] ^ Number1[i] ^ R);
                R = ((Number[i] || Number1[i]) & R) || (Number[i] && Number1[i]);
            }
            if (R)
                Rez.Insert(0, true);
            if (Rez.Count > NN)
                if (Number[0] == Number1[0])
                    Rez = ConvertReversCodeToAdditional(ConvertDirectCodeToRevers(Rez));
                else
                    Rez.RemoveAt(0);

            else if (Rez[0])
                if (Rez.Count == NN&& Number[0] == Number1[0])
                    Rez.Insert(0, false);
                else
                    Rez = ConvertReversCodeToAdditional(ConvertDirectCodeToRevers(Rez));
            return Rez;
        }

        public List<bool> Sum1(List<bool> Num, List<bool> Num1,ref bool RR)//входные массивы в дополнительном коде
        {
            List<bool> Number = new List<bool>(Num);
            List<bool> Number1 = new List<bool>(Num1);
            int NN;
            if (Number.Count > Number1.Count)
            {
                NN = Number.Count;
                SetLength(Number1, NN);
            }
            else
            {
                NN = Number1.Count;
                Number = SetLength(Number, NN);
            }
            List<bool> Rez = new List<bool>();
            bool R = false; //число в следующем разряде увеличивается на 1
            for (int i = NN - 1; i >= 0; i--)
            {
                Rez.Insert(0, Number[i] ^ Number1[i] ^ R);
                R = ((Number[i] || Number1[i]) & R) || (Number[i] && Number1[i]);
            }
            
            if (R)
                Rez.Insert(0, true);
            if (Rez.Count > NN)
                if (Number[0] == Number1[0])
                    Rez = ConvertReversCodeToAdditional(ConvertDirectCodeToRevers(Rez));
                else
                    Rez.RemoveAt(0);

            else if (Rez[0])
                if (Rez.Count == NN && Number[0] == Number1[0])
                {
                    Rez.Insert(0, false);
                    RR = true;
                }
                else
                    Rez = ConvertReversCodeToAdditional(ConvertDirectCodeToRevers(Rez));
            return Rez;
        }


        public List<bool> Razn(List<bool> Number1)//входные массивы в прямом коде
        {
            List<bool> Num1 = new List<bool>(Number1);
            Num1[0] = !Number1[0];
            return Sum(AdditionalCode, Num1);
        }

        public List<bool> Razn(List<bool> Number, List<bool> Number1)//входные массивы в прямом коде
        {
            List<bool> Num1 = new List<bool>(Number);
            List<bool> Num2 = new List<bool>(Number1);
            Num2[0] = !Num2[0];
            Num2 = ConvertReversCodeToAdditional(ConvertDirectCodeToRevers(Num2));
            return Sum(Num1, Num2);
        }

        public List<bool> Multiplication(List<bool> Number, List<bool> Number1)//входные массивы в прямом коде
        {
            List<bool> Rez = new List<bool>();
            List<bool> PromSlog = new List<bool>();
            for (int i = Number1.Count - 1; i > 0; i--)
            {
                if (Number1[i])
                {
                    PromSlog = new List<bool>(Number);
                    PromSlog.InsertRange(Number.Count, Enumerable.Repeat(false, Number1.Count - i - 1));
                    Rez = new List<bool>(Sum(Rez, PromSlog));
                }
            }
            Rez[0] = Number[0] != Number1[0];
            return Rez;
        }


        public int FirstModuleBiggerThenSecond(List<bool> Number, List<bool> Number1)
        {
            List<bool> Number11 = new List<bool>(Number);
            List<bool> Number22 = new List<bool>(Number1);
            int NN;
            if (Number11.Count > Number22.Count)
            {
                NN = Number11.Count;
                Number22 = SetLength(Number22, NN);
            }
            else
            {
                NN = Number22.Count;
                Number11 = SetLength(Number11, NN);
            }
            for (int i = 1; i < NN; i++)
            {
                if (Number11[i] != Number22[i])
                    if (!Number11[i])
                        return -1;
                    else
                        return 1;
            }
            return 0;
        }

        int ZnachCount(List<bool> number)
        {
            for (int i = 1; i < number.Count; i++)
            {
                if (number[i])
                    return number.Count - i;
            }
            return 0;
        }




        public List<bool> Devision(List<bool> Delimoe, List<bool> Delitel, int Count, ref List<bool> Drobn)//входные массивы в прямом коде
        {
            List<bool> Rez = new List<bool>();
            if (Delitel == Rez)
                return Rez; //нужно что-то возвращать если деление на 0
            List<bool> IntRez = new List<bool>();//целая часть ответа  Каждый следующий бит добавляется справа          
            List<bool> Prom = new List<bool>();
            List<bool> Vichitaemoe = new List<bool>(Delitel);
            Vichitaemoe[0] = !Vichitaemoe[0];
            Vichitaemoe = ConvertReversCodeToAdditional(ConvertDirectCodeToRevers(Vichitaemoe));//сюда заносится резултат вычитания  Prom - Delimoe

            int DelimoeCount = ZnachCount(Delimoe);
            int DelitelCount = ZnachCount(Delitel);



            //сравнить делитель и ZnachCount первых цифр делимого 
            int DelimoeBigger = 0;


            List<bool> zero = new List<bool>(); //массив из false для сравнения результата вычитания и 0 для окончания деления

            int NewCount = 0;
            //Если количество значащихи цифр делимого меньше количества значащих цифр делителя
            //то приваеваем в Prom сразу всё делимое и дальше нужно сносить нули

            if (DelimoeCount >= DelitelCount)
            {
                NewCount = Delitel.Count;
                Prom = Delimoe.Skip(DelimoeCount - NewCount - 1).Take(NewCount).ToList();
                DelimoeBigger = FirstModuleBiggerThenSecond(Prom, Delitel);
                if(DelimoeBigger != 1)
                {
                    if (NewCount < Delimoe.Count)
                        Prom.AddRange(Delimoe.GetRange(NewCount, 1));
                    else
                        Prom.Add(false);
                    NewCount++;
                }    
            }
            else
            {
                NewCount = Delimoe.Count;
                Prom = Delimoe.Skip(Delimoe.Count - DelimoeCount - 1).Take(DelimoeCount + 1).ToList();
                Prom.Add(false);
            }
            do
            {
                DelimoeBigger = FirstModuleBiggerThenSecond(Prom, Delitel);//1 да  -1 нет  0 продолжать цикл
                while (DelimoeBigger ==- 1)
                {
                    if (Drobn.Count() == Count)
                        break;
                    if (NewCount < Delimoe.Count)
                    {
                        IntRez.Add(false);
                        Prom.AddRange(Delimoe.GetRange(NewCount, 1));
                    }
                    else
                    {
                        Prom.Add(false);
                        Drobn.Add(false);
                    }

                    NewCount++;
                    DelimoeBigger = FirstModuleBiggerThenSecond(Prom, Delitel);
                }
                if (Drobn.Count() == Count)
                    break;

               
                if (NewCount <= Delimoe.Count)// <= так как = означает что была снесена последняя цифра и сейчас последняя цифра ответа заносящаяся в целую часть
                    IntRez.Add(true);
                else
                    Drobn.Add(true);
               
                Prom = new List<bool>(Sum(Prom, Vichitaemoe));

                if (NewCount < Delimoe.Count)
                {
                    Prom.AddRange(Delimoe.GetRange(NewCount, 1));
                    NewCount++;
                }
                else if(NewCount == Delimoe.Count)
                {
                    Prom.Add(false);
                    NewCount++;
                }
                else
                    Prom.Add(false);
            } while (DelimoeBigger != 0 && Drobn.Count() < Count);
            IntRez.Insert(0,Delimoe[0] != Delimoe[0]);
            return IntRez;
        }

        public  double ConvertBynaryDoubleToDecimalDouble(List<bool> IntList, List<bool> Drobn)
        {
            double Rez = 0;
            IntList.RemoveRange(1, IntList.Skip(1).TakeWhile(b => !b).Count());
            for (int i = 1; i < IntList.Count; i++)
                if (IntList[i])
                    Rez += Math.Pow(2, IntList.Count-i - 1);
            for (int i = 0; i < Drobn.Count; i++)
                if (Drobn[i])
                    Rez += Math.Pow(2, -(i + 1));
            if (IntList[0])
                return -Rez;
            else
                return Rez;

        }





        //public int[] Multiplication(int[] Number, int[] Number1)//входные массивы в прямом коде
        //{
        //    int[] Rez = new int[2*N];

        //    int[,] Summ = new int[N,2*N];

        //    for (int Step = 0; Step < N; Step++)//смещение влево каждого следующего промежуточного результата
        //    {
        //        for (int i = N - 1; i >= 0; i--)
        //        {
        //            if (Number[i] == 1 && Number1[N-Step-1] == 1)
        //                Summ[Step, i + N-Step] = 1;
        //            else
        //                Summ[Step, i + N- Step] = 0;
        //        }

        //    }

        //    for(int i=0;i<2*N;i++)
        //    {
        //        Rez = Sum(Rez, Summ[i]);
        //    }

        //    return Rez;
        //}





        //public int ConverReversCodeToDecimalNumber()
        //{
        //    //for()
        //}


        ////------------------------------
        //int number = 0;
        //int number1 = 0;
        //bool isValidInput = false;
        //Console.WriteLine("Введите число:");

        //while (!isValidInput)
        //{
        //    string input = Console.ReadLine();
        //    isValidInput = int.TryParse(input, out number);
        //    if (!isValidInput)
        //        Console.WriteLine("Введено не число, введите ещё раз");
        //}
        //isValidInput = false;
        //Console.WriteLine("Введите число:");

        //while (!isValidInput)
        //{
        //    string input = Console.ReadLine();
        //    isValidInput = int.TryParse(input, out number1);
        //    if (!isValidInput)
        //        Console.WriteLine("Введено не число, введите ещё раз");
        //}


        ////--- Вывод чисел во всех кодах
        //Console.WriteLine($"{number} Прямой код:");
        //int [] PriamoiN1 = ConvertDecimalNumberToDirectCode(number);
        //PrintDvNumber(PriamoiN1);
        //Console.WriteLine($"{number1} Прямой код:");
        //int[] PriamoiN2 = ConvertDecimalNumberToDirectCode(number1);
        //PrintDvNumber(PriamoiN2);
        //PrintDvNumber(ConvertDecimalNumberToDirectCode(number1));

        ////Console.WriteLine("Умножение чисел Прямой код:");
        ////PrintDvNumber(Sum(PriamoiN1, PriamoiN2));
        ////Console.WriteLine("Умножение чисел Прямой код:");
        ////PrintDvNumber(Sum(PriamoiN1, PriamoiN2));
        ////int[] DopN1 = Dopolnit(number);
        ////int[] DopN2 = Dopolnit(number1);
        //Console.WriteLine("Сложение чисел Обратный код:");
        //PrintDvNumber(Sum(PriamoiN1, PriamoiN2));

        ////Сложение в дополнительном УМножение и деление в прямом
        ////для чисел с плавающей точкой только пеевод и сложение
        ////s*m*2^(e-128)
    }
}


