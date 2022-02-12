package decorator

import "fmt"

// Decorator: dynamically adds/overrides behaviour in an existing method of an object.
//
// Motivation:
// we want to augment the object with additional functionality but we don't want to alter existing code (OCP)
// we want to keep new functionality separete (SRP)
// we need to interact with existing structures

func Decorator() {
	fmt.Println("\nDecorator")

	// create component
	bp := NewBulgarianPizza("Banica", 16)
	// decorate component
	bpc := NewTomatoPizza(bp, 3)
	// review
	fmt.Println(bpc.GetPizzaName(), "- $", bpc.GetPizzaPrice())
	fmt.Println(bpc.GetComponent())

	// create component
	ip := NewItalianPizza("Margherita", 20)
	// decorate component
	ipc := NewCheesePizza(ip, 4)
	ipct := NewTomatoPizza(ipc, 3)
	// review
	fmt.Println(ipct.GetPizzaName(), "- $", ipct.GetPizzaPrice())
	fmt.Println(ipct.GetComponent())
}

// -- Component

// Interface - represents component behaviour.
type Interface interface {
	GetPizzaName() string
	GetPizzaPrice() int
	GetComponent() string // decorator test method
}

// Pizza - represents component.
type Pizza struct {
	origin string
	name   string
	price  int
}

// GetPizzaName - returns Pizza name field value.
func (p *Pizza) GetPizzaName() string {
	return p.name
}

// GetPizzaPrice - returns Pizza price field value.
func (p *Pizza) GetPizzaPrice() int {
	return p.price
}

// GetComponent - returns decoration hierarchy in string representation.
func (p *Pizza) GetComponent() string {
	return p.origin
}

// -- Concreate Component

// BulgarianPizza - represent concreate implementation of component.
type BulgarianPizza struct {
	Pizza
}

// NewBulgarianPizza - creates new instance of BulgarianPizza.
func NewBulgarianPizza(name string, price int) Interface {
	const _bulgarian = "Bulgarian"
	return &BulgarianPizza{
		Pizza{_bulgarian, name, price},
	}
}

// ItalianPizza - represent concreate implementation of component.
type ItalianPizza struct {
	Pizza
}

// NewItalianPizza - creates new instance of ItalianPizza.
func NewItalianPizza(name string, price int) Interface {
	const _italian = "Italian"
	return &ItalianPizza{
		Pizza{_italian, name, price},
	}
}

// -- Decorator

// PizzaDecorator - represents decorator.
type PizzaDecorator struct {
	pizza Interface
	name  string
	price int
}

// GetPizzaName - returns decorated Pizza name field value.
func (pd *PizzaDecorator) GetPizzaName() string {
	return pd.pizza.GetPizzaName() + ", with " + pd.name
}

// GetPizzaPrice - returns decorated Pizza price field value.
func (pd *PizzaDecorator) GetPizzaPrice() int {
	return pd.pizza.GetPizzaPrice() + pd.price
}

// GetComponent - returns decoration hierarchy in string representation.
func (pd *PizzaDecorator) GetComponent() string {
	return fmt.Sprintf("%s(%s)", pd.name, pd.pizza.GetComponent())
}

// -- Concreate Decorator

// TomatoPizza - represents concreate decorator.
type TomatoPizza struct {
	PizzaDecorator
}

// NewTomatoPizza - create new instance of TomatoPizza.
func NewTomatoPizza(pizza Interface, price int) Interface {
	const _tomatoes = "Tomatoes"
	return &TomatoPizza{
		PizzaDecorator: PizzaDecorator{pizza, _tomatoes, price},
	}
}

// CheesePizza - represents concreate decorator.
type CheesePizza struct {
	PizzaDecorator
}

// NewCheesePizza - create new instance of CheesePizza.
func NewCheesePizza(pizza Interface, price int) Interface {
	const _cheese = "Cheese"
	return &CheesePizza{
		PizzaDecorator: PizzaDecorator{pizza, _cheese, price},
	}
}
