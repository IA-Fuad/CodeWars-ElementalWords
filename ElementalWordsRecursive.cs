// Preloaded for you:
// Dictionary<string, string> ELEMENTS
// e.g. ELEMENTS["H"] == "Hydrogen"
using static Preloaded.Elements;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Kata
{
    public class ElementalWords
    {
        private static int maxElementLen = 3;

        public static string[][] ElementalForms(string word)
        {
            maxElementLen = ELEMENTS.Keys.Max(k => k.Length);
            List<string> currentElementalForm = new();
            List<string[]> allElementalForms = new();

            FindAllElementalFormByTryingAllPossibleCombinations(allElementalForms, currentElementalForm, word, 0);

            return allElementalForms.ToArray();
        }

        // Using recursive backtracking approach to find all elemental forms.
        private static void FindAllElementalFormByTryingAllPossibleCombinations(List<string[]> allElementalForms, List<string> currentElementalForm, string word, int currentWordInd)
        {
            if (currentWordInd >= word.Length && currentElementalForm.Count > 0)
            {
                allElementalForms.Add(currentElementalForm.ToArray());
                return;
            }

            string currentWord = string.Empty;
            for (int k = currentWordInd; k < Math.Min(word.Length, currentWordInd + maxElementLen); k++)
            {
                currentWord += word[k];
                currentWord = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(currentWord.ToLower());

                if (ELEMENTS.ContainsKey(currentWord))
                {
                    currentElementalForm.Add($"{ELEMENTS[currentWord]} ({currentWord})");
                    FindAllElementalFormByTryingAllPossibleCombinations(allElementalForms, currentElementalForm, word, k + 1);
                    currentElementalForm.RemoveAt(currentElementalForm.Count - 1);
                }
            }
        }
    }
}