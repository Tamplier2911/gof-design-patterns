package factories

import "fmt"

// Factories: are components responsible solely for the wholesale (not piecewise) creation of objects.
//
// Motivation:
// Object creation (non-piecewise, unlike Builder) can be outsourced to:
// - separate function (Factory Method)
// - separate type (Factory)
// - can be hierarchy of factories (Abstract Factory)

func Factories() {
	fmt.Println("Factories")

	// factory function / constructor
	p1 := NewFrenchPerson("Charles", "de Gaulle")
	p1.Introduce()
	// can access methods and fields

	// interface factory
	p2 := NewIntroducer("Jeanne", "d'Arc")
	p2.Introduce()
	// can only access methods specified by interface

	// factory generator

	// prototype factory
}

// FrenchPerson - represents person.
type FrenchPerson struct {
	FirstName, LastName, Nationality string
}

// NewFrenchPerson - creates new instance of person.
func NewFrenchPerson(firstName string, lastName string) *FrenchPerson {
	return &FrenchPerson{FirstName: firstName, LastName: lastName, Nationality: "French"}
}

// Introduce - introduce french person.
func (p FrenchPerson) Introduce() {
	fmt.Printf("My name is %s %s, I'm %s!\n", p.FirstName, p.LastName, p.Nationality)
}

// Introducer - represents greeter interface.
type Introducer interface {
	Introduce()
}

// NewIntroducer - represents interface factory method.
func NewIntroducer(firstName string, lastName string) Introducer {
	return &FrenchPerson{FirstName: firstName, LastName: lastName, Nationality: "French"}
}
