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
	// structural
	employeeMarketingFactory := NewEmployeeFactoryS(DepartmentMarketing, RoleMarketingAnalyst)
	e1 := employeeMarketingFactory.Create("Anna")
	// functional
	employeeEngineeringFactory := NewEmployeeFactoryF(DepartmentEngineering, RoleSoftwareEngineer)
	e2 := employeeEngineeringFactory("Tom")

	// prototype factory
	e3 := NewEmployeeFactoryP(RoleSoftwareEngineer)
	e3.Name = "Jane"

	fmt.Printf("%+v\n%+v\n%+v\n", e1, e2, e3)
}

// -- Factory Function / Constructor

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

// -- Interface Factory

// Introducer - represents greeter interface.
type Introducer interface {
	Introduce()
}

// NewIntroducer - represents interface factory method.
func NewIntroducer(firstName string, lastName string) Introducer {
	return &FrenchPerson{FirstName: firstName, LastName: lastName, Nationality: "French"}
}

// -- Factory Generator

// Employee - represents employee.
type Employee struct {
	Name       string
	Department Department
	Role       Role
}

// Department - represent employee department.
type Department string

const (
	DepartmentMarketing   Department = "marketing"
	DepartmentEngineering Department = "engineering"
)

// Role - represent employee role.
type Role string

const (
	RoleMarketingAnalyst Role = "marketing analyst"
	RoleSoftwareEngineer Role = "software engineer"
)

// structural approach

type EmployeeFactory struct {
	Department Department
	Role       Role
}

// NewEmployeeFactoryS - represents instance of new Employee structural factory.
func NewEmployeeFactoryS(department Department, role Role) *EmployeeFactory {
	return &EmployeeFactory{Department: department, Role: role}
}

// Create - creates new instance of Employee.
func (e EmployeeFactory) Create(name string) *Employee {
	return &Employee{Name: name, Department: e.Department, Role: e.Role}
}

// functional approach

// NewEmployeeFactoryF - creates instance of new Employee functional factory.
func NewEmployeeFactoryF(d Department, r Role) func(name string) *Employee {
	return func(name string) *Employee {
		return &Employee{Name: name, Department: d, Role: r}
	}
}

// -- Prototype Factory

// NewEmployeeFactoryP - creates instance of new Employee prototype factory.
func NewEmployeeFactoryP(r Role) *Employee {
	switch r {
	case RoleMarketingAnalyst:
		return &Employee{Name: "", Department: DepartmentMarketing, Role: r}
	case RoleSoftwareEngineer:
		return &Employee{Name: "", Department: DepartmentEngineering, Role: r}
	default:
		panic("invalid role")
	}
}
