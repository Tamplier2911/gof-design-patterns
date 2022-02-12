package patterns

import (
	"fmt"

	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/behavioral"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/creational"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/structural"
)

func Run() {
	fmt.Println("\nGOF Design Patterns")

	creational.Creational()
	structural.Structural()
	behavioral.Behavioral()
}
