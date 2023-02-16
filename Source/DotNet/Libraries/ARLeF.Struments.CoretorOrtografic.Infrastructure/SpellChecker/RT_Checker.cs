using ARLeF.Struments.CoretorOrtografic.Core.Entities.RadixTree;
using ARLeF.Struments.CoretorOrtografic.Infrastructure.RadixTreeDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARLeF.Struments.CoretorOrtografic.Infrastructure.SpellChecker
{
    public class RT_Checker
    {
        private RadixTreeDatabaseService rt;
        private const char NOLC_CAR = '*';

        public RT_Checker(RadixTreeDatabaseService rt)
        {
            this.rt = rt;
        }

        public bool HasWord(string word)
        {
            byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(word);
            return NodeCheck(rt.GetRoot(), bytes);
        }

        public List<string> GetWordsEd1(string word)
        {
            byte[] bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(word);
            List<string> words = new List<string>();
            foreach (string decoded in GetWords(rt.GetRoot(), bytes))
            {
                byte[] encoded = Encoding.GetEncoding("iso-8859-1").GetBytes(decoded);
                words.Add(Encoding.UTF8.GetString(encoded));
            }
            return words;
        }

        private bool NodeCheck(RadixTreeNode node, byte[] suffix)
        {
            RadixTreeEdge edge = node.GetNextEdge();
            while (edge != null)
            {
                byte[] label = Encoding.GetEncoding("iso-8859-1").GetBytes(edge.GetString());
                int lenConf = Math.Min(label.Length, suffix.Length);
                int resConf = string.Compare(Encoding.GetEncoding("iso-8859-1").GetString(label, 0, lenConf), Encoding.GetEncoding("iso-8859-1").GetString(suffix, 0, lenConf));
                if (resConf == -1)
                {
                    edge = node.GetNextEdge();
                }
                else if (resConf == 1)
                {
                    return false;
                }
                else
                {
                    if (label.Length > suffix.Length)
                    {
                        return false;
                    }
                    else if (label.Length == suffix.Length)
                    {
                        return edge.IsWord();
                    }
                    else
                    {
                        if (edge.IsLeaf())
                        {
                            return false;
                        }
                        else
                        {
                            return NodeCheck(edge.GetNode(), Encoding.GetEncoding("iso-8859-1").GetBytes(Encoding.GetEncoding("iso-8859-1").GetString(suffix, label.Length, suffix.Length - label.Length)));
                        }
                    }
                }
            }
            return false;
        }

        private bool EdgeCheck(RadixTreeEdge edge, byte[] suffix)
        {
            byte[] label = Encoding.GetEncoding("iso-8859-1").GetBytes(edge.GetString());
            int lenConf = Math.Min(label.Length, suffix.Length);
            int resConf = string.Compare(Encoding.GetEncoding("iso-8859-1").GetString(label, 0, lenConf),
                Encoding.GetEncoding("iso-8859-1").GetString(suffix, 0, lenConf));
            if (resConf != 0)
            {
                return false;
            }
            else
            {
                if (label.Length > suffix.Length)
                {
                    return false;
                }
                else if (label.Length == suffix.Length)
                {
                    return edge.IsWord();
                }
                else
                {
                    if (edge.IsLeaf())
                    {
                        return false;
                    }
                    else
                    {
                        return NodeCheck(edge.GetNode(), Encoding.GetEncoding("iso-8859-1").GetBytes(Encoding.GetEncoding("iso-8859-1").GetString(suffix, label.Length, suffix.Length - label.Length)));
                    }
                }
            }
        }

        private List<string> GetWords(RadixTreeNode node, byte[] word)
        {
            var words = new List<string>();

            for (var i = 0; i < node.GetNumEdges(); i++)
            {
                var edge = node.GetNextEdge();
                var label = Encoding.GetEncoding("iso-8859-1").GetBytes(edge.GetString());
                var lenConf = Math.Min(label.Length, word.Length);
                var resConf = string.Compare(Encoding.GetEncoding("iso-8859-1").GetString(label, 0, lenConf),
                    Encoding.GetEncoding("iso-8859-1").GetString(word, 0, lenConf));
                if (resConf == 0)
                {
                    if (label.Length == word.Length)
                    {
                        if (edge.IsWord())
                        {
                            words.Add(Encoding.UTF8.GetString(label));
                        }
                    }
                    else if (label.Length < word.Length)
                    {
                        var suffix = word.Skip(label.Length).ToArray();
                        var subWords = GetWords(edge.GetNode(), suffix);
                        foreach (var subWord in subWords)
                        {
                            var w = Encoding.UTF8.GetString(label) + subWord;
                            if (edge.IsWord())
                            {
                                words.Add(w);
                            }
                            else
                            {
                                words.AddRange(GetWords(edge.GetNode(), suffix));
                            }
                        }
                    }
                }
                else if (resConf < 0)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            return words;
        }
    }
}
