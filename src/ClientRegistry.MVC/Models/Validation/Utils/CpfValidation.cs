using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClientRegistry.MVC.Models.Validations.Utils
{
    public static class CpfValidation
    {
        public const int CpfLength = 11;

        public static bool Validate(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            if (cpf.Length != CpfLength) return false;

            if (new string(cpf[0], cpf.Length) == cpf) return false;

            return ValidateDigits(cpf);
        }

        private static bool ValidateDigits(string cpf)
        {
            int[] multipl1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multipl2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multipl1[i];

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            tempCpf += digit1;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multipl2[i];

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return cpf.EndsWith(digit1.ToString() + digit2.ToString());
        }

    }
}
