using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReaderConsoleTest
{
    public class TrieNode : IComparable<TrieNode>
    {
        private readonly char _char;
        public int _wordCount;
        private readonly TrieNode _parent = null;
        private readonly ConcurrentDictionary<char, TrieNode> _children = null;

        public TrieNode(TrieNode parent, char c)
        {
            _char = c;
            _wordCount = 0;
            _parent = parent;
            _children = new ConcurrentDictionary<char, TrieNode>();
        }

        //Adding Word to the Index
        public void AddWord(string word, int index = 0)
        {
            if (index < word.Length)
            {
                char key = word[index];
                if (char.IsLetter(key)) 
                {
                    if (!_children.ContainsKey(key))
                    {
                        _children.TryAdd(key, new TrieNode(this, key));
                    }
                    _children[key].AddWord(word, index + 1);
                }
                else
                {
                    // not a letter! retry with next char
                    AddWord(word, index + 1);
                }
            }
            else
            {
                if (_parent != null) // empty words should never be counted
                {
                    lock (this)
                    {
                        _wordCount++;
                    }
                }
            }
        }

        //Counting Word
        public int GetCount(string word, int index = 0)
        {
            if (index < word.Length)
            {
                char key = word[index];
                if (!_children.ContainsKey(key))
                {
                    return -1;
                }
                return _children[key].GetCount(word, index + 1);
            }
            else
            {
                return _wordCount;
            }
        }

        //Get Top Count using Distinct count and total count 
        //Generic class and methods combine reusability, type safety, efficiency and performance in a way that their non-generic counterparts cannot
        public void GetTopCounts(ref List<TrieNode> mostCounted, ref int distinctWordCount, ref int totalWordCount)
        {
            if (_wordCount > 0)
            {
                distinctWordCount++;
                totalWordCount += _wordCount;
            }
            if (_wordCount > mostCounted[0]._wordCount)
            {
                mostCounted[0] = this;
                mostCounted.Sort();
            }
            foreach (char key in _children.Keys)
            {
                _children[key].GetTopCounts(ref mostCounted, ref distinctWordCount, ref totalWordCount);
            }
        }

        public override string ToString()
        {
            if (_parent == null) return "";
            else return _parent.ToString() + _char;
        }

        public int CompareTo(TrieNode other)
        {
            return this._wordCount.CompareTo(other._wordCount);
        }
    }
}