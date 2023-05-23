using ARLeF.Struments.CoretorOrtografic.Core.RadixTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ARLeF.Struments.CoretorOrtografic.Infrastructure.SpellChecker
{
    public class RT_Checker
    {
        private RadixTree _rt;

        public RT_Checker(RadixTree rt)
        {
            Console.WriteLine($"Called RT_Checker constructor with rt: {rt}"); // Debugging statement
            _rt = rt;
        }

        public const char NOLC_CAR = '*';

        public bool HasWord(string word)
        {
            Console.WriteLine($"Called 'HasWord' with word: {word}"); // Debugging statement
            word = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.UTF8.GetBytes(word));
            return NodeCheck(_rt.GetRoot(), word);
        }

        public string[] GetWordsED1(string word)
        {
            Console.WriteLine($"Called 'GetWordsED1' with word: {word}"); // Debugging statement
            word = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.UTF8.GetBytes(word));
            return GetWords(_rt.GetRoot(), word)
                .Select(w => Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(w)))
                .ToArray();
        }

        private bool NodeCheck(RadixTreeNode node, string suffix)
        {
            Console.WriteLine($"Called 'NodeCheck' with suffix: {suffix}"); // Debugging statement
            while (true)
            {
                RadixTreeEdge edge = node.GetNextEdge();
                if (edge == null)
                {
                    return false;
                }

                string label = edge.GetString();
                int lenConf = Math.Min(suffix.Length, label.Length);
                int resConf = String.Compare(label.Substring(0, lenConf), suffix.Substring(0, lenConf));

                if (resConf < 0)
                {
                    continue;
                }
                else if (resConf > 0)
                {
                    Console.WriteLine("NodeCheck returning: false (1st return)"); // Debugging statement
                    return false;
                }
                else
                {
                    if (label.Length > suffix.Length)
                    {
                        Console.WriteLine("NodeCheck returning: false (2nd return)"); // Debugging statement
                        return false;
                    }
                    else if (label.Length == suffix.Length)
                    {
                        bool isWord = edge.IsWord() != 0;
                        Console.WriteLine($"NodeCheck returning: {isWord} (3rd return)"); // Debugging statement
                        return isWord;
                    }
                    else
                    {
                        if (edge.IsLeaf())
                        {
                            Console.WriteLine("NodeCheck returning: false (4th return)"); // Debugging statement
                            return false;
                        }
                        else
                        {
                            return NodeCheck(edge.GetNode(), suffix.Substring(label.Length));
                        }
                    }
                }
            }
        }

        private bool EdgeCheck(RadixTreeEdge edge, string suffix, out int caseFlag)
        {
            Console.WriteLine($"Called 'EdgeCheck' with suffix: {suffix}"); // Debugging statement
            caseFlag = 0;
            var label = edge.GetString();
            var lenConf = Math.Min(label.Length, suffix.Length);
            var resConf = String.Compare(label.Substring(0, lenConf), suffix.Substring(0, lenConf), StringComparison.Ordinal);
            if (resConf != 0)
            {
                Console.WriteLine("EdgeCheck returning: false (1st return)"); // Debugging statement
                return false;
            }
            else
            {
                if (label.Length > suffix.Length)
                {
                    Console.WriteLine("EdgeCheck returning: false (2nd return)"); // Debugging statement
                    return false;
                }
                else if (label.Length == suffix.Length)
                {
                    caseFlag = edge.IsWord() != 0 ? (edge.IsLowerCase() ? 2 : 1) : 0;
                    bool isWord = edge.IsWord() != 0;
                    Console.WriteLine($"EdgeCheck returning: {isWord} (3rd return)"); // Debugging statement
                    return isWord;
                }
                else
                {
                    if (edge.IsLeaf())
                    {
                        Console.WriteLine("EdgeCheck returning: false (4th return)"); // Debugging statement
                        return false;
                    }
                    else
                    {
                        return NodeCheck(edge.GetNode(), suffix.Substring(label.Length));
                    }
                    //caseFlag = edge.IsWord() != 0 ? (edge.IsLowerCase() ? 2 : 1) : 0;
                    //return true;
                }
            }
        }

        private List<string> GetWords(RadixTreeNode node, string word)
        {
            List<string> words = new List<string>();

            RadixTreeEdge edge;
            while (node != null && (edge = node.GetNextEdge()) != null)
            {
                if (edge.GetNode() == null)
                    continue;

                string label = edge.GetString();
                int minLen = Math.Min(label.Length, word.Length);
                int i;
                for (i = 0; i < minLen && label[i] == word[i]; i++) { }

                if (i < minLen)
                {
                    string tmpWord = word.Substring(0, i) + label[i] + word.Substring(i + 1);
                    int caseFlag;
                    if (EdgeCheck(edge, tmpWord, out caseFlag))
                    {
                        words.Add(tmpWord + (caseFlag == 2 ? NOLC_CAR : ""));
                    }
                    tmpWord = word.Substring(0, i) + label[i] + word.Substring(i);
                    if (EdgeCheck(edge, tmpWord, out caseFlag))
                    {
                        words.Add(tmpWord + (caseFlag == 2 ? NOLC_CAR : ""));
                    }

                    if (word.Length > i + 1 && label[i] == word[i + 1])
                    {
                        tmpWord = word.Substring(0, i) + word.Substring(i + 1);
                        if (EdgeCheck(edge, tmpWord, out caseFlag))
                        {
                            words.Add(tmpWord + (caseFlag == 2 ? NOLC_CAR : ""));
                        }

                        tmpWord = word.Substring(0, i) + word[i + 1] + word[i] + word.Substring(i + 2);
                        if (EdgeCheck(edge, tmpWord, out caseFlag))
                        {
                            words.Add(tmpWord + (caseFlag == 2 ? NOLC_CAR : ""));
                        }
                    }
                }
                else if (i < word.Length)
                {
                    if (!edge.IsLeaf())
                    {
                        words.AddRange(GetWords(edge.GetNode(), word.Substring(i)));
                    }

                    if (word.Length == i + 1 && edge.IsWord() != 0)
                    {
                        words.Add(label + (edge.IsLowerCase() ? NOLC_CAR : ""));
                    }
                }
                else if (i < label.Length)
                {
                    if (label.Length == i + 1 && edge.IsWord() != 0)
                    {
                        words.Add(label + (edge.IsLowerCase() ? NOLC_CAR : ""));
                    }
                }
                else
                {
                    if (!edge.IsLeaf())
                    {
                        words.AddRange(GetWords(edge.GetNode(), ""));
                    }
                    else if (edge.IsWord() != 0)
                    {
                        words.Add(label + (edge.IsLowerCase() ? NOLC_CAR : ""));
                    }
                }
            }

            return words;
        }
    }
}