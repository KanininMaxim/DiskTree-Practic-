using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree;

public class DiskTree : IEnumerable<string>
{
    public Dictionary<string, DiskTree> Trees = new Dictionary<string, DiskTree>();

    public void Add(List<string> input)
    {
        foreach (var directionary in input)
        {
            var directionaryWay = directionary.Split('\\');
            var currentDirectory = this;

            for (int i = 0; i < directionaryWay.Length; i++)
            {
                directionaryWay[i] = directionaryWay[i].PadLeft(directionaryWay[i].Length + i, ' ');
                if (!currentDirectory.Trees.ContainsKey(directionaryWay[i]))
                {
                    currentDirectory.Trees[directionaryWay[i]] = new DiskTree();
                    currentDirectory.Trees = currentDirectory.Trees
                        .OrderBy(t => t.Key, StringComparer.Ordinal)
                        .ToDictionary(k => k.Key, v => v.Value);
                }
                currentDirectory = currentDirectory.Trees[directionaryWay[i]];
            }
        }
    }

    private IEnumerator<string> TreeValueCompareAndReturn(DiskTree tree)
    {
        if (tree == null) yield break;
        foreach (var key in tree.Trees.Keys)
        {
            yield return key;
            var enumeratorForTreeNode = TreeValueCompareAndReturn(tree.Trees[key]);
            while (enumeratorForTreeNode.MoveNext())
                yield return enumeratorForTreeNode.Current;
        }
    }

    public IEnumerator<string> GetEnumerator() => TreeValueCompareAndReturn(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class DiskTreeTask
{
    public static List<string> Solve(List<string> input)
    {
        DiskTree directoryForSort = new DiskTree();

        directoryForSort.Add(input);

        var result = new List<string>();

        foreach (var item in directoryForSort)
            result.Add(item);

        return result.ToList();
    }
}