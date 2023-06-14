using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTrainer
{
    public class TreeNode<Dewey>
    {

        // The dewey data
        private Dewey data;

        // store the dewey data in a tree
        private List<TreeNode<Dewey>> children = new List<TreeNode<Dewey>>();

        public TreeNode(Dewey val)
        {
            data = val;
        }

        public TreeNode<Dewey> this[int i]
        {
            get { return children[i]; }
        }

        public TreeNode<Dewey> Parent { get; set; }

        public Dewey Value { get { return data; } }

        public List<TreeNode<Dewey>> Children
        {
            get { return children; }
        }

        public TreeNode<Dewey> AddChild(Dewey value)
        {
            var node = new TreeNode<Dewey>(value) { Parent = this };
            children.Add(node);
            return node;
        }
        public void AddChildNode(TreeNode<Dewey> value)
        {
            children.Add(value);
        }

        public void Traverse(Action<Dewey> action)
        {
            action(Value);
            foreach (var child in children)
            {
                Console.WriteLine(child.ToString());
                child.Traverse(action);
            }
        }

        public IEnumerable<Dewey> Flatten()
        {
            return new[] { Value }.Concat(children.SelectMany(x => x.Flatten()));
        }

        public int GetCount()
        {
            return children.Count;
        }

    }
}
