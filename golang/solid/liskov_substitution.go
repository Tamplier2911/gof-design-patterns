package solid

import "fmt"

// L - Liskov Substitution (ability to substitute base-type with sub-type)
// "Let (x) be a property provable about objects x of type T.
// Then (y) should be true for objects y of type S where S is a subtype of T."

func LiskovSubstitution() {
	fmt.Println("Liskov Substitution")
	fmt.Println("START")

	r := &Rectangle{}
	r.SetWidth(2)
	r.SetHeight(5)
	fmt.Println(GetArea(r))

	s := &Square{}
	s.SetWidth(2)
	s.SetHeight(5)
	fmt.Println(GetArea(s))

	fmt.Println("END")
}

// Sized - represents shape size interface.
type Sized interface {
	GetWidth() int
	SetWidth(w int)
	GetHeight() int
	SetHeight(h int)
}

// Rectangle

// Rectangle - represents rectangular shape.
type Rectangle struct {
	Width, Height int
}

// GetWidth - gets rectangle width.
func (r *Rectangle) GetWidth() int {
	return r.Width
}

// SetWidth - sets rectangle width.
func (r *Rectangle) SetWidth(w int) {
	r.Width = w
}

// GetHeight - gets rectangle height.
func (r *Rectangle) GetHeight() int {
	return r.Height
}

// SetHeight - sets rectangle height.
func (r *Rectangle) SetHeight(h int) {
	r.Height = h
}

// Square

// Square - represents rectangular shape.
type Square struct {
	Rectangle
}

// SetWidth - sets rectangle width.
func (r *Square) SetWidth(s int) {
	r.Width, r.Height = s, s
}

// SetHeight - sets rectangle height.
func (r *Square) SetHeight(s int) {
	r.Height, r.Width = s, s
}

// Utilities

func GetArea(s Sized) int {
	return s.GetWidth() * s.GetHeight()
}
