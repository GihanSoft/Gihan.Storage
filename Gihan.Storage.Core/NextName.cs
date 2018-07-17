using System.Linq;

namespace Gihan.Helpers.String
{
    public static class NextName
    {
        public static string ProductNextName(string currentName)
        {
            //currentName = new string(currentName.ToCharArray());
            var currentNum = -1;
            if (currentName.Last() != ')')
                currentNum = 2;

            string currentNumStr = null;
            var i = 0;
            if (currentNum == -1)
                for (i = currentName.Length - 2; i > 0 && char.IsDigit(currentName[i]); i--)
                {
                    currentNumStr += currentName[i].ToString();
                }

            if (currentNum == -1 && currentNumStr != null && currentName[i] == '(')
                currentNum = int.Parse(currentNumStr);

            if (currentNum == -1) currentNum = 2;


            var result = currentName + $"({currentNum})";

            return result;
        }
    }
}