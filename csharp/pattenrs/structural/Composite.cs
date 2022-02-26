using System.Text;
using System.Collections.ObjectModel;
using System.Collections;

namespace Composite
{
    ///
    /// <summary>
    /// Class <c>Main</c> represents Composite pattern usecase.
    /// Composite: composes zero-or-more similar objects so that they can be manipulated as one object.
    /// Motivation:
    /// objects use other object members through inheritance and composition
    /// composite is used to treat individual objects and composite objects uniformly
    /// </summary>
    ///
    class Main
    {
        public static void Run()
        {
            Console.WriteLine("\nComposite");

            // Composite

            // create directories 
            var dr1 = new Directory("Developer");
            var dr2 = new Directory("projects");
            var dr3 = new Directory("gof-design-patterns");
            var dr4 = new Directory("data-structures");
            // create files
            var f1 = new File("Adapter.cs");
            var f2 = new File("Builder.cs");
            var f3 = new File("Composite.cs");
            var f4 = new File("BinaryTree.cs");
            var f5 = new File("Graph.cs");
            // add files to directories
            dr3.Add(f1);
            dr3.Add(f2);
            dr3.Add(f3);
            dr4.Add(f4);
            dr4.Add(f5);
            // add directories to directories
            dr2.Add(dr4);
            dr2.Add(dr3);
            dr1.Add(dr2);
            // represent tree-structured data
            Console.WriteLine(dr1);

            // Neural Network

            // create neurons
            var n1 = new Neuron();
            var n2 = new Neuron();
            // create neuron layers
            var l1 = new NeuronLayer();
            var l2 = new NeuronLayer();

            // implement connections
            n1.ConnectTo(n2); // neuron to neuron
            n2.ConnectTo(l1); // neuron to layer
            l2.ConnectTo(n1); // layer to neuron
            l1.ConnectTo(l2); // layer to layer
        }
    }

    // -- Component

    /// <summary>Class <c>Component</c> represents an abstraction for tree-structured file system components.</summary>
    public abstract class Component
    {
        public string Name;
        public Component(string name)
        {
            Name = name;
        }
        // public abstract void Add(Component c);
        // public abstract void Remove(Component c);
        public virtual void Add(Component c) { }
        public virtual void Remove(Component c) { }
    }

    // -- Composite

    /// <summary>Class <c>Directory</c> represents composite, can contain multiple components.</summary>
    public class Directory : Component
    {
        private List<Component> Children = new List<Component>();
        public Directory(string name) : base(name) { }
        public override void Add(Component c)
        {
            Children.Add(c);
        }
        public override void Remove(Component c)
        {
            Children.Remove(c);
        }

        public void List(StringBuilder sb, int depth)
        {
            sb.AppendLine($"{new string(' ', depth)}/{Name}");
            foreach (var child in Children)
            {
                if (child is Directory)
                {
                    Directory dir = (Directory)child;
                    dir.List(sb, depth + 1);
                    continue;
                }
                sb.AppendLine($"{new string(' ', depth + 1)}/{child.Name}");
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            List(sb, 0);
            return sb.ToString();
        }
    }

    /// <summary>Class <c>File</c> represents leaf, cannot contain multiple components.</summary>
    public class File : Component
    {
        public File(string name) : base(name) { }
    }

    // -- Neural Network

    /// <summary>Class <c>Neuron</c> represents composite, can be connected with multiple components</summary>
    public class Neuron : IEnumerable<Neuron>
    {
        public float value;
        public List<Neuron> In = new List<Neuron>();
        public List<Neuron> Out = new List<Neuron>();

        public void ConnectTo(Neuron other)
        {
            Out.Add(other);
            other.In.Add(this);
        }

        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>Class <c>NeuronLayer</c> represents composite set of neurons, can be connected with components.</summary>
    public class NeuronLayer : Collection<Neuron> { }

    /// <summary>Class <c>ExtensionMethods</c> extend functionality of IEnumerable.</summary>
    public static class ExtensionMethods
    {
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
        {
            if (ReferenceEquals(self, other)) return;
            foreach (var from in self)
            {
                foreach (var to in other)
                {
                    from.Out.Add(to);
                    to.In.Add(from);
                }
            }
        }
    }
}