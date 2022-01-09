package solid

import "fmt"

// L - Liskov Substitution (ability to substitute base-type with sub-type)
// "Let (x) be a property provable about objects x of type T.
// Then (y) should be true for objects y of type S where S is a subtype of T."

func LiskovSubstitution() {
	fmt.Println("Liskov Substitution")

	r := &Rectangle{}
	r.SetA(2)
	r.SetB(5)
	fmt.Printf("Rectangle Area: %d\n", r.GetArea())

	s := &Square1{}
	s.SetA(2)
	s.SetB(5)
	fmt.Printf("Square1 Area: %d\n", s.GetArea())

	ss := &Square2{}
	ss.SetA(2)
	fmt.Printf("Square2 Area: %d\n", ss.GetArea())
}

// ConvexQuadrilateral - represents convex quadrilateral interface.
type ConvexQuadrilateral interface {
	GetArea() int
}

// EquiangularQuadrilateral - represents equiangular equilateral interface.
type EquiangularQuadrilateral interface {
	ConvexQuadrilateral
	SetA(a int)
	SetB(b int)
}

// ---

// Rectangle - represent rectangular shape.
type Rectangle struct {
	EquiangularQuadrilateral
	A int
	B int
}

// SetA - sets rectangle width.
func (r *Rectangle) SetA(a int) {
	r.A = a
}

// SetB - sets rectangle height.
func (r *Rectangle) SetB(b int) {
	r.B = b
}

// GetArea - calculate rectangle area.
func (r *Rectangle) GetArea() int {
	return r.A * r.B
}

// ---

// Square1 - represents square shape.
type Square1 struct {
	EquiangularQuadrilateral
	A int
}

// SetA - sets square height and width.
func (s *Square1) SetA(a int) {
	s.A = a
}

// GetArea - calculate square area.
func (s *Square1) GetArea() int {
	return s.A * s.A
}

// SetB - sets square width (obsolete)
func (s *Square1) SetB(b int) {
	// should it be s.A = b ?
	// or should it be empty?
	s.A = b
}

// ---

// EquilateralQuadrilateral - represents equilateral quadrilateral interface.
type EquilateralQuadrilateral interface {
	ConvexQuadrilateral
	SetA(a int)
}

// NonEquilateralQuadrilateral - represents non equilateral quadrilateral interface.
type NonEquilateralQuadrilateral interface {
	ConvexQuadrilateral
	SetA(a int)
	SetB(b int)
}

// NonEquiangularQuadrilateral - represents non equiangular quadrilateral.
type NonEquiangularQuadrilateral interface {
	ConvexQuadrilateral
	SetAngle(angle float64)
}

// ---

// Oblong - represents oblong shape.
type Oblong struct {
	NonEquilateralQuadrilateral
	A int
	B int
}

// Parallelogram - represents parallelogram shape.
type Parallelogram struct {
	NonEquilateralQuadrilateral
	NonEquiangularQuadrilateral
	a     int
	b     int
	angle float64
}

// Rhombus - represents rhombus shape.
type Rhombus struct {
	EquilateralQuadrilateral
	NonEquiangularQuadrilateral
	a     int
	angle float64
}

// Square2 - represents square shape.
type Square2 struct {
	EquilateralQuadrilateral
	A int
}

// SetA - sets square width and height.
func (r *Square2) SetA(a int) {
	r.A = a
}

// GetArea - calculate square area.
func (r *Square2) GetArea() int {
	return r.A * r.A
}
