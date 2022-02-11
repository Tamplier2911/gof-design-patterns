package structural

import (
	"fmt"

	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/structural/adapter"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/structural/bridge"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/structural/composite"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/structural/decorator"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/structural/facade"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/structural/flyweight"
	"github.com/Tamplier2911/gof-design-patterns/golang/patterns/structural/proxy"
)

func Structural() {
	fmt.Println("\nStructural")

	adapter.Adapter()
	bridge.Bridge()
	composite.Composite()
	decorator.Decorator()
	facade.Facade()
	flyweight.Flyweight()
	proxy.Proxy()
}
