using System.Linq;

namespace Gihan.Helpers.String
{
    public static class NextName
    {
        public static string ProductNextName(string currentName)
        {
            bool hasNum;
            hasNum = currentName.Last() == ')';
            if (hasNum && !char.IsDigit(currentName[currentName.Length - 2]))
                    hasNum = false;
            if (!hasNum) return currentName + "(2)";

            var currentNumStr = "";
            int i;
            for (i = currentName.Length - 2; i > 0 && char.IsDigit(currentName[i]); i--)
            {
                currentNumStr = currentName[i] + currentNumStr;
            }

            if (currentName[i] != '(') return currentName + "(2)";
            var currentNum = int.Parse(currentNumStr);
            var pureName = currentName.Substring(0, i);
            return $"{pureName}({++currentNum})";
        }
    }
}
