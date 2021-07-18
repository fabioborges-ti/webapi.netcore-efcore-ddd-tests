using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Api.Domain.Validations.ValidationRoles
{
    [ExcludeFromCodeCoverage]
    public static class Roles
    {
        public static bool IsValidName(string name)
        {
            name = name.Replace(" ", "");

            return name.All(char.IsLetter);
        }

        public static bool IsValidDocument(string cpf)
        {
            switch (cpf)
            {
                case "":
                case "00000000000":
                case "11111111111":
                case "22222222222":
                case "33333333333":
                case "44444444444":
                case "55555555555":
                case "66666666666":
                case "77777777777":
                case "88888888888":
                case "99999999999":
                    return false;
            }

            var multiplies1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplies2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");

            if (cpf.Length != 11) return false;

            for (var j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf) return false;

            var tempCpf = cpf[..9];
            var sum = 0;

            for (var i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplies1[i];

            var mod = sum % 11;

            if (mod < 2)
                mod = 0;
            else
                mod = 11 - mod;

            var digit = mod.ToString();

            tempCpf += digit;
            sum = 0;

            for (var i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplies2[i];

            mod = sum % 11;

            if (mod < 2)
                mod = 0;
            else
                mod = 11 - mod;

            digit += mod;

            return cpf.EndsWith(digit);
        }
    }
}
