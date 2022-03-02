package behavioral

import (
	"fmt"

	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/behavioral/command"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/behavioral/cor"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/behavioral/interpreter"
)

func Behavioral() {
	fmt.Println("\nBehavioral")

	cor.ChainOfResponsibility()
	command.Command()
	interpreter.Interpreter()
}
