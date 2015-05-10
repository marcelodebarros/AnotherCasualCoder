using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TreeAndHashtable
{
    class Program
    {
        static void Main(string[] args)
        {
            TreeManager.AddElement(-1, 1);
            TreeManager.AddElement(1, 2);
            TreeManager.AddElement(1, 3);
            TreeManager.AddElement(1, 4);
            TreeManager.AddElement(2, 7);
            TreeManager.AddElement(2, 8);
            TreeManager.AddElement(7, 9);
            TreeManager.AddElement(7, 10);
            TreeManager.AddElement(7, 11);
            TreeManager.AddElement(7, 12);
            TreeManager.AddElement(9, 13);

            //Root
            Console.Write("Print Tree By Level");
            Tree root = TreeManager.GetElement(-1);
            root.PrintTreeByLevel();
            Console.WriteLine();
            Console.WriteLine();

            //Another node
            Tree node = TreeManager.GetElement(3);
            Console.WriteLine("Height of the Tree at Element 3: {0}", node.GetHeight());
            Console.WriteLine();

            //Get 2, print children
            Console.WriteLine("Print Children of 2");
            node = TreeManager.GetElement(2);
            node.PrintChildren();
            Console.WriteLine();

            //Delete 7
            Console.WriteLine("Delete Element 7");
            TreeManager.DeleteElement(7);
            Console.WriteLine();

            //Root
            root = TreeManager.GetElement(-1);
            Console.WriteLine("Height of the Tree at Root: {0}", root.GetHeight());
            Console.WriteLine();

            //Get 2, print children
            Console.WriteLine("Print Children of 2");
            node = TreeManager.GetElement(2);
            node.PrintChildren();
            Console.WriteLine();

            //Root - print the tree level by level
            Console.WriteLine("Print Tree By Level");
            root = TreeManager.GetElement(-1);
            root.PrintTreeByLevel();
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    public class Tree
    {
        public int value = Int32.MinValue;
        public Hashtable htChildren = null;
        private bool deleted = false;

        private Tree() { }

        public Tree(int value)
        {
            this.value = value;
            htChildren = new Hashtable();
            this.deleted = false;
        }

        public Tree AddChild(int child)
        {
            if (!htChildren.ContainsKey(child) ||
                ((Tree)htChildren[child]).deleted)
            {
                Tree childTree = new Tree(child);
                htChildren.Add(child, childTree);
                return childTree;
            }
            else
            {
                return (Tree)htChildren[child];
            }
        }

        public void PrintChildren()
        {
            if (this.deleted)
            {
                return;
            }
            else
            {
                Console.Write("Children of {0}:", this.value);
                foreach (int childValue in htChildren.Keys)
                {
                    Tree childTree = (Tree)htChildren[childValue];
                    childTree.PrintChildrenInternal();
                }
                Console.WriteLine();
            }
        }

        private void PrintChildrenInternal()
        {
            if (!this.deleted)
            {
                Console.Write(" {0}", this.value);
            }
            else
            {
                foreach (int childValue in htChildren.Keys)
                {
                    Tree childTree = (Tree)htChildren[childValue];
                    childTree.PrintChildrenInternal();
                }
            }
        }

        public void PrintTreeByLevel()
        {
            PriorityQueue priorityQueue = new PriorityQueue(1000, false);
            int currentLevel = 1;
            int previousLevel = currentLevel - 1;

            TreeAndLevel treeAndLevel = new TreeAndLevel(this, currentLevel);
            priorityQueue.Enqueue(treeAndLevel, currentLevel);

            while (priorityQueue.Count > 0)
            {
                treeAndLevel = (TreeAndLevel)priorityQueue.Dequeue();
                currentLevel = treeAndLevel.level;

                if (previousLevel < currentLevel)
                {
                    Console.WriteLine();
                    Console.Write("Level #{0}: {1}", currentLevel, treeAndLevel.tree.value);
                }
                else
                {
                    Console.Write(" {0}", treeAndLevel.tree.value);
                }
                previousLevel = currentLevel;

                foreach (int childValue in treeAndLevel.tree.htChildren.Keys)
                {
                    Tree childTree = (Tree)treeAndLevel.tree.htChildren[childValue];
                    childTree.PrintByLevelInternal(priorityQueue, currentLevel + 1);
                }
            }
        }

        private void PrintByLevelInternal(PriorityQueue priorityQueue, int level)
        {
            if (!this.deleted)
            {
                TreeAndLevel treeAndLevel = new TreeAndLevel(this, level);
                priorityQueue.Enqueue(treeAndLevel, level);
            }
            else
            {
                foreach (int childValue in htChildren.Keys)
                {
                    Tree childTree = (Tree)htChildren[childValue];
                    childTree.PrintByLevelInternal(priorityQueue, level);
                }
            }
        }

        public void Delete()
        {
            this.deleted = true;
        }

        public int GetHeight()
        {
            int maxChildrenHeight = 0;
            foreach (int childValue in htChildren.Keys)
            {
                Tree childTree = (Tree)htChildren[childValue];
                int heightChild = childTree.GetHeight();
                if (heightChild > maxChildrenHeight)
                {
                    maxChildrenHeight = heightChild;
                }
            }

            return (this.deleted ? maxChildrenHeight : maxChildrenHeight + 1);
        }
    }

    public static class TreeManager
    {
        private static Hashtable htIndex = new Hashtable();

        public static void AddElement(int parent,
                                      int child)
        {
            if (htIndex.ContainsKey(child) ||
                (parent != -1 && !htIndex.ContainsKey(parent)) ||
                (parent == -1 && htIndex.ContainsKey(parent)))
            {
                Console.WriteLine("Error! More info later on!!!");
            }
            else if (parent == -1)
            {
                Tree rootTree = new Tree(child);
                htIndex.Add(-1, rootTree);
                htIndex.Add(child, rootTree);
            }
            else
            {
                Tree parentTree = (Tree)htIndex[parent];
                Tree childTree = parentTree.AddChild(child);
                htIndex.Add(child, childTree);
            }
        }

        public static Tree GetElement(int value)
        {
            if (!htIndex.ContainsKey(value))
            {
                return null;
            }
            else
            {
                return (Tree)htIndex[value];
            }
        }

        public static void DeleteElement(int value)
        {
            if (htIndex.ContainsKey(value))
            {
                ((Tree)htIndex[value]).Delete();
                htIndex.Remove(value);
            }
        }
    }

    public class TreeAndLevel
    {
        public Tree tree = null;
        public int level = 0;

        private TreeAndLevel() { }

        public TreeAndLevel(Tree tree, int level)
        {
            this.tree = tree;
            this.level = level;
        }
    }
}