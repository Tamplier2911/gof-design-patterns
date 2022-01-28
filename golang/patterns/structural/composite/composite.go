package composite

import (
	"fmt"
)

// Composite: composes zero-or-more similar objects so that they can be manipulated as one object.
//
// Motivation:
// objects use other object members through inheritance and composition
// composite is used to treat individual objects and composite objects uniformly

func Composite() {
	fmt.Println("Composite")

	// Composite

	// create directories
	dir1 := NewDirectory("Developer")
	dir2 := NewDirectory("projects")
	dir3 := NewDirectory("gof-design-patterns")
	dir4 := NewDirectory("data-structures")
	// create files
	f1 := NewFile("adapter.go")
	f2 := NewFile("builder.go")
	f3 := NewFile("composite.go")
	f4 := NewFile("binary_tree.go")
	f5 := NewFile("graph.go")
	f6 := NewFile("readme.md")
	// add files to directories
	dir3.Add(f1)
	dir3.Add(f2)
	dir3.Add(f3)
	dir4.Add(f4)
	dir4.Add(f5)
	// add directories to directories
	dir2.Add(f6)
	dir2.Add(dir3)
	dir2.Add(dir4)
	dir1.Add(dir2)
	// represent tree structured data
	fmt.Println(dir1.List())

	// Neural Network

	// create neurons
	n1, n2 := NewScalarNeuron(), NewScalarNeuron()
	// create neuron layers
	l1, l2 := NewNeuronLayer(), NewNeuronLayer()
	// add scalars to neuron layers
	l1.AddScalar(NewScalarNeuron())
	l1.AddScalar(NewScalarNeuron())
	l1.AddScalar(NewScalarNeuron())
	l2.AddScalar(NewScalarNeuron())
	l2.AddScalar(NewScalarNeuron())
	l2.AddScalar(NewScalarNeuron())
	l2.AddScalar(NewScalarNeuron())

	// connect neuron to neuron
	n1.Connect(n2)
	// connect neuron to layer
	n2.Connect(l1)
	// connect layer to neuron
	l2.Connect(n1)
	// connect layer to layer
	l1.Connect(l2)
}

// -- Component

// Component - represents an abstraction for tree-structured file system components.
type Component interface {
	List() string
}

// -- Composite

// Directory - represents composite, can contain multiple components.
type Directory struct {
	Component
	name     string
	children []Component
}

// NewDirectory - creates new instance of directory.
func NewDirectory(name string) *Directory {
	return &Directory{name: name, children: []Component{}}
}

// Add - adds new file system component.
func (d *Directory) Add(c Component) {
	d.children = append(d.children, c)
}

// List - lists current directory and add sub directories content.
func (d *Directory) List() string {
	result := "/" + d.name + "\n"

	// loop over each child
	for _, child := range d.children {
		result += child.List()
	}
	return result
}

// File - represents leaf, cannot contain multiple components.
type File struct {
	Component
	name string
}

// NewFile - creates new instance of file.
func NewFile(name string) *File {
	return &File{name: name}
}

// List - gets file name.
func (f *File) List() string {
	return "	" + f.name + "\n"
}

// -- Neural Network

// Neuron - represents neural network interface.
type Neuron interface {
	GetNeurons() []*ScalarNeuron
	Connect(other Neuron)
}

// ScalarNeuron - represents scalar Neuron.
type ScalarNeuron struct {
	In, Out []*ScalarNeuron
	Value   float64
}

// NewScalarNeuron - creates new instance of scalar Neuron.
func NewScalarNeuron() *ScalarNeuron {
	return &ScalarNeuron{}
}

// GetNeurons - exposes neurons for interconnection.
func (n *ScalarNeuron) GetNeurons() []*ScalarNeuron {
	return []*ScalarNeuron{n}
}

// Connect - connects Neurons.
func (n *ScalarNeuron) Connect(other Neuron) {
	for _, o := range other.GetNeurons() {
		n.Out = append(n.Out, o)
		o.In = append(o.In, n)
	}
}

// NeuronLayer - represents neuron connection.
type NeuronLayer struct {
	Neurons []*ScalarNeuron
}

// NewNeuronLayer - creates new instance of NeuronLayer.
func NewNeuronLayer() *NeuronLayer {
	return &NeuronLayer{}
}

// AddScalar - adds scalar neuron to NeuronsLayer.
func (n *NeuronLayer) AddScalar(neuron *ScalarNeuron) {
	n.Neurons = append(n.Neurons, neuron)
}

// GetNeurons - exposes neurons for interconnection.
func (n *NeuronLayer) GetNeurons() []*ScalarNeuron {
	return n.Neurons
}

// Connect - connects Neurons.
func (n *NeuronLayer) Connect(other Neuron) {
	for _, inner := range n.Neurons {
		for _, outer := range other.GetNeurons() {
			inner.Out = append(inner.Out, outer)
			outer.In = append(outer.In, inner)
		}
	}
}
