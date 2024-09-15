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
        public static string[][] ElementalForms(string word)
        {
            int maxElementLen = ELEMENTS.Keys.Max(k => k.Length);
            Dictionary<int, List<string>> nextElementsAtEachIndex = FindAllPossiblePath(word, maxElementLen);

            List<string[]> allForms = new();
            BuildFormList(nextElementsAtEachIndex, allForms);

            return allForms.ToArray();
        }

        // Using dynamic programming to find next possible elements from each index 
        private static Dictionary<int, List<string>> FindAllPossiblePath(string word, int maxElementLen)
        {
            Dictionary<int, List<string>> nextElementsAtEachIndex = new();

            for (int i = word.Length - 1; i >= 0; i--)
            {
                string currentWord = string.Empty;
                for (int j = i; j < Math.Min(word.Length, i + maxElementLen); j++)
                {
                    currentWord += word[j];
                    currentWord = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(currentWord.ToLower());

                    /* From index 'i', a substring is a valid element if the substring exists in the periodic table 
                       AND if at index 'i + lengthOfTheSubString' we already have one or more valid elements. 
                       Handling corner case where the substring is a suffix */
                    if (ELEMENTS.ContainsKey(currentWord) && (j == word.Length - 1 || nextElementsAtEachIndex.ContainsKey(j + 1)))
                    {
                        if (!nextElementsAtEachIndex.ContainsKey(i))
                        {
                            nextElementsAtEachIndex[i] = new();
                        }

                        nextElementsAtEachIndex[i].Add(currentWord);
                    }
                }
            }

            return nextElementsAtEachIndex;
        }

        /* Using iterative DFS to build the elemental forms.
          nextElementsAtEachIndex is basically a tree data structure, 
          where the dictionary keys made up of indexes are nodes and 
          the values which are a list of valid elements from the indexes are edges. */
        private static void BuildFormList(Dictionary<int, List<string>> nextElementsAtEachIndex, List<string[]> allForms)
        {
            List<string> form = new();
            Stack<Tuple<int, int>> S = new();
            S.Push(new(0, 0));

            while (S.Count > 0)
            {
                var (wordInd, nextElementInd) = S.Pop();

                // We are at the last element or all possible next elements from this node has been processed.
                if (!nextElementsAtEachIndex.ContainsKey(wordInd) || nextElementInd == nextElementsAtEachIndex[wordInd].Count)
                {
                    if (form.Count > 0)
                    {
                        if (!nextElementsAtEachIndex.ContainsKey(wordInd))
                        {
                            allForms.Add(form.ToArray());
                        }
                        form.RemoveAt(form.Count - 1);
                    }
                    continue;
                }

                string element = nextElementsAtEachIndex[wordInd][nextElementInd];

                if (nextElementInd < nextElementsAtEachIndex[wordInd].Count)
                {
                    S.Push(new(wordInd, nextElementInd + 1));
                }

                form.Add($"{ELEMENTS[element]} ({element})");
                S.Push(new(wordInd + element.Length, 0));
            }
        }
    }
}