using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceTitan6
{
    class Program
    {
        class Result
        {
            /*
             * Complete the 'circuitsOutput' function below.
             *
             * The function is expected to return an INTEGER_ARRAY.
             * The function accepts STRING_ARRAY circuitsExpression as parameter.
             */

            public static List<int> circuitsOutput(List<string> circuitsExpression)
            {
                return circuitsExpression.Select(n => Calc(GetPostfix(n))).ToList();
            }

            static string GetPostfix(string input)
            {
                StringBuilder output = new StringBuilder();
                Stack<char> operStack = new Stack<char>();

                for (int i = 0; i < input.Length; i++)
                {
                    if (char.IsDigit(input[i]))
                    {
                        output.Append(input[i]);
                    }

                    if (IsOperator(input[i]))
                    {
                        if (input[i] == '[')
                            operStack.Push(input[i]);
                        else if (input[i] == ']')
                        {
                            char s = operStack.Pop();

                            while (s != '[')
                            {

                                output.Append(s.ToString());
                                s = operStack.Pop();
                            }
                        }
                        else
                        {
                            if (operStack.Count > 0)
                                if (GetPriority(input[i]) <= GetPriority(operStack.Peek()))
                                    output.Append(operStack.Pop().ToString());

                            operStack.Push(char.Parse(input[i].ToString()));

                        }
                    }
                }

                while (operStack.Count > 0)
                    output.Append(operStack.Pop().ToString());

                return output.ToString();
            }

            static private int Calc(string postfixStr)
            {
                int result = 0;
                Stack<int> temp = new Stack<int>();

                for (int i = 0; i < postfixStr.Length; i++)
                {
                    if (char.IsDigit(postfixStr[i]))
                    {
                        temp.Push(int.Parse(postfixStr[i].ToString()));
                    }
                    else if (IsOperator(postfixStr[i])) //Если символ - оператор
                    {
                        //Берем два последних значения из стека
                        int a = temp.Pop();


                        switch (postfixStr[i]) //И производим над ними действие, согласно оператору
                        {
                            case '!': result = Math.Abs(a - 1); break;
                            case '|':
                                {
                                    int b = temp.Pop();
                                    var tmp = b + a;
                                    result = tmp > 1 ? 1 : tmp;
                                    break;
                                }
                            case '&':
                                {
                                    int b = temp.Pop();
                                    result = b * a;
                                    break;
                                }
                        }
                        temp.Push(result);
                    }
                }
                return temp.Peek();
            }

            static private bool IsOperator(char с)
            {
                if (("!|&[]".IndexOf(с) != -1))
                    return true;
                return false;
            }

            static private byte GetPriority(char s)
            {
                switch (s)
                {
                    case '[': return 0;
                    case ']': return 1;
                    case '|': return 3;
                    case '&': return 3;
                    case '!': return 5;
                    default: return 6;
                }
            }

        }

        static void Main(string[] args)
        {
            var input = new List<string>()
            {
                "[|, [&, 1, [!, 0]], [!, [|, [|, 1, 0], [!, 1]]]]"
            };

            var result = Result.circuitsOutput(input);

            for (int i = 0; i < input.Count; i++)
                Console.WriteLine("{0} => {1}", input[i], result[i]);
            Console.ReadLine();
        }
    }
}
