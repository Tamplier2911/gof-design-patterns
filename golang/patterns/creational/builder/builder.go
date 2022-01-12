package builder

import (
	"errors"
	"fmt"
	"strings"
)

// Builder: when piecewise object construction is complicated provides an API for doing it succinctly.
//
// Motivation:
// Some objects are simple and can be created within a single constructor call.
// Some isn't, having object with a dozen constructor arguments is not productive.
// Builder provides API for constructing objects step by step.

func Builder() {
	fmt.Println("Builder")
	const _userId = "abc_123"
	var user string

	// query builder with fluent interface
	qb := NewQueryBuilder()
	err := qb.
		From(`users`).
		Select(`user.name`).
		Where(`user.id = ?`, _userId).
		Find(&user).
		Error
	if err != nil {
		fmt.Println(fmt.Errorf("error occured: %w", err))
		return
	}

	fmt.Printf("Query: %s Result: %s \n", qb.Query, user)

	// stepwise builder

	// functional builder

	// faceted builder
	ep := NewEmployeeBuilder().
		Address().
		City("Los Angeles").
		Street("501 N VIRGIL").
		PostalCode("90004-2315").
		Job().
		Department("Development").
		Role("Software Engineer").
		Build()

	fmt.Printf(
		"Address: %s %s %s | Job: %s %s \n",
		ep.City, ep.Street, ep.PostalCode, ep.Department, ep.Role,
	)

}

// -- Builder with fulent interface

// QueryBuilder - represents query builder.
type QueryBuilder struct {
	Query string
	Error error
}

// NewQueryBuilder - creates new instance of query builder.
func NewQueryBuilder() *QueryBuilder {
	return &QueryBuilder{}
}

// From - builds from statement.
func (qb *QueryBuilder) From(q string) *QueryBuilder {
	qb.Query += fmt.Sprintf("FROM %s ", q)
	return qb
}

// Select - builds select statement.
func (qb *QueryBuilder) Select(q string) *QueryBuilder {
	qb.Query += fmt.Sprintf("SELECT %s ", q)
	return qb
}

// Where - builds where statement.
func (qb *QueryBuilder) Where(q string, args ...string) *QueryBuilder {
	query, err := qb.appendArguments(q, args)
	if err != nil {
		qb.Error = err
		qb.Query = ""
		return qb
	}

	qb.Query += fmt.Sprintf("WHERE %s", query)
	return qb
}

// Find - executes query.
func (qb *QueryBuilder) Find(t *string) *QueryBuilder {
	qb.Query += ";"

	// execute
	const _username = "Rickiest Rick of all Ricks"
	*t = _username

	return qb
}

// appendArguments - appends arguments to query string.
func (qb *QueryBuilder) appendArguments(q string, args []string) (string, error) {
	result := ""
	curArg := 0
	for _, char := range strings.Split(q, "") {
		if char == "?" {
			if curArg > len(args)-1 {
				return "", errors.New("missing argument")
			}
			result += args[curArg]
			curArg++
			continue
		}
		result += char
	}
	return result, nil
}

// -- Faceted Builder

type Employee struct {
	// address details
	City, Street, PostalCode string
	// job details
	Department, Role string
}

func NewEmployee() *Employee {
	return &Employee{}
}

type EmployeeBuilder struct {
	e *Employee
}

func NewEmployeeBuilder() *EmployeeBuilder {
	return &EmployeeBuilder{NewEmployee()}
}

func (eb *EmployeeBuilder) Address() *EmployeeAddressBuilder {
	return NewEmployeeAddressBuilder(eb)
}

func (eb *EmployeeBuilder) Job() *EmployeeJobBuilder {
	return NewEmployeeJobBuilder(eb)
}

func (eb *EmployeeBuilder) Build() *Employee {
	return eb.e
}

type EmployeeAddressBuilder struct {
	EmployeeBuilder
}

func NewEmployeeAddressBuilder(eb *EmployeeBuilder) *EmployeeAddressBuilder {
	return &EmployeeAddressBuilder{*eb}
}

func (ab *EmployeeAddressBuilder) City(s string) *EmployeeAddressBuilder {
	ab.e.City = s
	return ab
}

func (ab *EmployeeAddressBuilder) Street(s string) *EmployeeAddressBuilder {
	ab.e.Street = s
	return ab
}

func (ab *EmployeeAddressBuilder) PostalCode(s string) *EmployeeAddressBuilder {
	ab.e.PostalCode = s
	return ab
}

type EmployeeJobBuilder struct {
	EmployeeBuilder
}

func NewEmployeeJobBuilder(eb *EmployeeBuilder) *EmployeeJobBuilder {
	return &EmployeeJobBuilder{*eb}
}

func (jb *EmployeeJobBuilder) Department(s string) *EmployeeJobBuilder {
	jb.e.Department = s
	return jb
}

func (jb *EmployeeJobBuilder) Role(s string) *EmployeeJobBuilder {
	jb.e.Role = s
	return jb
}
