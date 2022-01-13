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
}
