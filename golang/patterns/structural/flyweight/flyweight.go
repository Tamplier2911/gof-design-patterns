package flyweight

import (
	"fmt"
	"strings"
)

// Flyweight: reduces the cost of creating and manipulating a large number of similar objects.
//
// Motivation:
// avoid redundancy when storing data (app using large amount of similar objects which turns into significant memory allocation)
// inner mutable state can be extracted from the class, which helps to replate inner state with a group of small shared objects
//

func Flyweight() {
	fmt.Println("Flyweight")

	// utility method
	inc := func(i *float64) float64 { *i++; return *i }

	// extrinsic state
	const _blueColor = "blue"
	const _redColor = "red"
	x := float64(0)
	y := float64(0)

	// init flyweight factory
	ff := NewFigureFactory()

	// get figures
	sq1 := ff.GetFigure("square")
	sq2 := ff.GetFigure("square")

	tr1 := ff.GetFigure("triangle")
	tr2 := ff.GetFigure("triangle")

	pr1 := ff.GetFigure("portrait")
	pr2 := ff.GetFigure("portrait")

	rb1 := ff.GetFigure("rainbow")
	rb2 := ff.GetFigure("rainbow")

	// perform actions (operations) passing extrinsic state
	sq1.Draw(_blueColor, inc(&x), inc(&y))
	sq2.Draw(_redColor, inc(&x), inc(&y))

	tr1.Draw(_blueColor, inc(&x), inc(&y))
	tr2.Draw(_redColor, inc(&x), inc(&y))

	pr1.Draw(_blueColor, inc(&x), inc(&y))
	pr2.Draw(_redColor, inc(&x), inc(&y))

	rb1.Draw(_blueColor, inc(&x), inc(&y))
	rb2.Draw(_redColor, inc(&x), inc(&y))

	fmt.Println(ff)
}

// -- Flyweight Abstraction

// FigureInterface - represents flyweight abstraction interface.
type FigureInterface interface {
	GetName() string
	AddLine(line Line)
	// Draw - operates on extrinsic state, which can vary (position, color, etc..)
	Draw(color string, x, y float64)
}

// Figure - represents flyweight abstraction.
type Figure struct {
	// lines and name - represents intrinsic state, which are specific for each component.
	lines []Line
	name  string
}

// GetName - returns figure name.
func (f *Figure) GetName() string {
	return f.name
}

// AddLine - appends line to figure.
func (f *Figure) AddLine(line Line) {
	f.lines = append(f.lines, line)
}

// Draw - draws image on canvas.
func (f *Figure) Draw(color string, x, y float64) {
	fmt.Printf("Drawing %s %s at position x:%.0f y:%.0f \n", color, f.name, x, y)
}

// -- Concreate Flyweights

// SquareFigure - represents concreate flyweight implementation.
type SquareFigure struct {
	FigureInterface
}

// NewSquareFigure - creates new instance of SquareFigure.
func NewSquareFigure() FigureInterface {
	const _square = "square"
	return &SquareFigure{&Figure{
		[]Line{{0, 0, 5, 0}, {0, 5, 5, 5}, {0, 0, 0, 5}, {5, 0, 5, 5}}, _square},
	}
}

// TriangleFigure - represents concreate flyweight implementation.
type TriangleFigure struct {
	FigureInterface
}

// NewTriangleFigure - creates new instance of TriangleFigure.
func NewTriangleFigure() FigureInterface {
	const _triangle = "triangle"
	return &TriangleFigure{&Figure{
		[]Line{{0, 0, 0, 5}, {0, 0, 5, 0}, {0, 5, 5, 0}}, _triangle},
	}
}

// CustomFigure - represents concreate flyweight implementation.
type CustomFigure struct {
	FigureInterface
}

// NewCustomFigure - creates new instance of CustomFigure.
func NewCustomFigure(name string) FigureInterface {
	return &CustomFigure{&Figure{[]Line{}, name}}
}

// -- Flyweight Factory

// FigureFactory - represents figure factory.
type FigureFactory struct {
	figures map[string]FigureInterface
}

// NewFigureFactory - creates new instance of figure factory, initializes default factory state.
func NewFigureFactory() *FigureFactory {
	sq := NewSquareFigure()
	tr := NewTriangleFigure()
	return &FigureFactory{
		figures: map[string]FigureInterface{
			sq.GetName(): sq,
			tr.GetName(): tr,
		},
	}
}

// GetFigure - retrieves figure from state or creates new custom figure.
func (ff *FigureFactory) GetFigure(name string) FigureInterface {
	// return figure if exists
	if f, ok := ff.figures[name]; ok {
		fmt.Printf("reused %s figure\n", name)
		return f
	}

	// create new custom figure
	f := NewCustomFigure(name)
	ff.figures[f.GetName()] = f
	fmt.Printf("created new %s figure\n", name)

	return ff.figures[f.GetName()]
}

// String - represent FigureFactory state in string format.
func (ff *FigureFactory) String() string {
	res := make([]string, 0)
	for _, f := range ff.figures {
		res = append(res, f.GetName())
	}
	return strings.Join(res, "\n")
}

// -- Auxiliary Types
type Line struct {
	x1, y1, x2, y2 float64
}

// NewLine - creates new instance of Line.
func NewLine(x1, y1, x2, y2 float64) *Line {
	return &Line{x1, y1, x2, y2}
}
