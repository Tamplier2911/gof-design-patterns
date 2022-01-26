package structural

import (
	"fmt"

	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/structural/adapter"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/structural/bridge"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/structural/composite"
)

func Structural() {
	fmt.Println("Structural")
	adapter.Adapter()
	bridge.Bridge()
	composite.Composite()
}
