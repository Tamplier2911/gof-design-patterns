package solid

import "fmt"

// Solid Design Principles: SOLID is an acronym that stands for five key design principles.

// The single-responsibility principle: SRP
// The open–closed principle: OCP
// The Liskov substitution principle: LSP
// The interface segregation principle: ISP
// The dependency inversion principle: DIP

func Run() {
	fmt.Println("\nSOLID principles")

	SingleResponsibility()
	OpenClosed()
	LiskovSubstitution()
	InterfaceSegregation()
	DependencyInversion()
}
