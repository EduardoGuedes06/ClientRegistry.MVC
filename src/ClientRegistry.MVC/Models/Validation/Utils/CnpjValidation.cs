using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClientRegistry.MVC.Models.Validations.Utils
{
    public static class CnpjValidation
    {
        public const int CnpjLength = 14;

        public static bool Validate(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return false;

            if (cnpj.Length != CnpjLength) return false;

            if (new string(cnpj[0], cnpj.Length) == cnpj) return false;

            return ValidateDigits(cnpj);
        }

        private static bool ValidateDigits(string cnpj)
        {
            int[] multipl1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multipl2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int sum = 0;

            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multipl1[i];

            int remainder = sum % 11;
            int digit1 = remainder < 2 ? 0 : 11 - remainder;

            tempCnpj += digit1;
            sum = 0;

            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multipl2[i];

            remainder = sum % 11;
            int digit2 = remainder < 2 ? 0 : 11 - remainder;

            return cnpj.EndsWith(digit1.ToString() + digit2.ToString());
        }
    }
}
