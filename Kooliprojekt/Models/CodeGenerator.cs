using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Models
{
    public class CodeGenerator
    {
        private static Random random = new Random();
        public static string RandomCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, 3)
              .Select(s => s[random.Next(s.Length)]).ToArray()) + RandomNumber();
        }
        public static string RandomNumber()
        {
            string codeNumbers = random.Next(99).ToString();
            if(codeNumbers.Length == 1)
            {
                codeNumbers = "0" + codeNumbers;
            }
            return codeNumbers;
        }
        public static string codeGenerator(string code)
        {
            char[] codeChars = code.Substring(0, 3).ToCharArray();
            List<int> asciiValues = new List<int>();
            foreach (var codeC in codeChars)
            {
                asciiValues.Add((int)codeC);
            }
            int codeNum = Int32.Parse(code.Substring(3));

            codeNum++;


            if (codeNum >= 99)
            {
                codeNum = 0;
                asciiValues[2]++;
            }
            if (asciiValues[2] > 90)
            {
                asciiValues[2] = 65;
                asciiValues[1]++;
            }
            if (asciiValues[1] > 90)
            {
                asciiValues[1] = 65;
                asciiValues[0]++;
            }
            if (asciiValues[0] > 90)
            {
                asciiValues[0] = 65;
                codeNum++;
            }

            string newCode = "";
            foreach (var c in asciiValues)
            {
                newCode += (char)c;
            }
            if (codeNum.ToString().Length == 1)
            {
                newCode += "0" + codeNum.ToString();
            }
            else
            {
                newCode += codeNum.ToString();
            }

            return newCode;

        }
    }
}
