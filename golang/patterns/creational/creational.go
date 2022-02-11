package creational

import (
	"fmt"

	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/creational/builder"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/creational/factories"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/creational/prototype"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/creational/singleton"
)

func Creational() {
	fmt.Println("\nCreational")
	factories.Factories()
	builder.Builder()
	prototype.Prototype()
	singleton.Singleton()
}
