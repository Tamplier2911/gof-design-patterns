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

	// query builder with fluent interface
	const _userId = "abc_123"
	var user string
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

	// builder parameter
	SendEmailBuilderParameter(func(b *EmailBuilder) {
		// email object should not be accessible from different module
		b.
			From("foo@email.com").
			To("bar@email.com").
			Subject("sub").
			Body("body")
	})

	// functional builder
	p := NewPersonBuilder().
		Name("Tom").
		YearOfBirth(1990).
		Build()
	fmt.Printf("Name: %s | YearOfBirth: %d \n", p.name, p.yearOfBirth)

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

// -- Builder with fluent interface

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

// -- Builder Parameter

// email - represents email.
type email struct {
	from, to, subject, body string
}

// EmailBuilder - represents email builder.
type EmailBuilder struct {
	email email
}

// From - sets from field on email.
func (mb *EmailBuilder) From(s string) *EmailBuilder {
	mb.email.from = s
	return mb
}

// To - sets to field on email.
func (mb *EmailBuilder) To(s string) *EmailBuilder {
	mb.email.to = s
	return mb
}

// Subject - sets subject field on email.
func (mb *EmailBuilder) Subject(s string) *EmailBuilder {
	mb.email.subject = s
	return mb
}

// Body - sets body field on email.
func (mb *EmailBuilder) Body(s string) *EmailBuilder {
	mb.email.body = s
	return mb
}

// build - represents build type.
type build func(*EmailBuilder)

// sendEmail - sends email private method.
func sendEmail(email *email) {
	fmt.Printf(
		"From: %s | To: %s | Subject: %s | Body: %s \n",
		email.from, email.to, email.subject, email.body,
	)
}

// SendEmailBuilderParameter - sends email public method.
func SendEmailBuilderParameter(action build) {
	mb := EmailBuilder{}
	action(&mb)
	sendEmail(&mb.email)
}

// -- Functional Builder

// Person - represents person.
type Person struct {
	name        string
	yearOfBirth int
}

// personMod - represents Person modifier.
type personMod func(p *Person)

// PersonBuilder - represents Person builder.
type PersonBuilder struct {
	actions []personMod
}

// NewPersonBuilder - creates new instance of PersonBuilder.
func NewPersonBuilder() *PersonBuilder {
	return &PersonBuilder{}
}

// Name - creates person name modifier and appends it to action list.
func (pb *PersonBuilder) Name(s string) *PersonBuilder {
	pb.actions = append(pb.actions, func(p *Person) {
		p.name = s
	})
	return pb
}

// YearOfBirth - creates person year of birth modifier and appends it to action list.
func (pb *PersonBuilder) YearOfBirth(i int) *PersonBuilder {
	pb.actions = append(pb.actions, func(p *Person) {
		p.yearOfBirth = i
	})
	return pb
}

// Build - builds person object based on defined actions.
func (pb *PersonBuilder) Build() *Person {
	p := &Person{}
	for _, action := range pb.actions {
		action(p)
	}
	return p
}

// -- Faceted Builder

// Employee - represents employee.
type Employee struct {
	// address details
	City, Street, PostalCode string
	// job details
	Department, Role string
}

// NewEmployee - creates new instance of Employee.
func NewEmployee() *Employee {
	return &Employee{}
}

// EmployeeBuilder - represents Employee builder.
type EmployeeBuilder struct {
	e *Employee
}

// NewEmployeeBuilder - creates new instance of EmployeeBuilder.
func NewEmployeeBuilder() *EmployeeBuilder {
	return &EmployeeBuilder{NewEmployee()}
}

// Address - grants access to methods required to construct Employee address details.
func (eb *EmployeeBuilder) Address() *EmployeeAddressBuilder {
	return NewEmployeeAddressBuilder(eb)
}

// Job - grants access to methods required to construct Employee job details.
func (eb *EmployeeBuilder) Job() *EmployeeJobBuilder {
	return NewEmployeeJobBuilder(eb)
}

// Build - returns instance built Employee.
func (eb *EmployeeBuilder) Build() *Employee {
	return eb.e
}

// EmployeeAddressBuilder - represents Employee address builder.
type EmployeeAddressBuilder struct {
	EmployeeBuilder
}

// NewEmployeeAddressBuilder - creates new instance of EmployeeAddressBuilder.
func NewEmployeeAddressBuilder(eb *EmployeeBuilder) *EmployeeAddressBuilder {
	return &EmployeeAddressBuilder{*eb}
}

// City - sets Employee city field.
func (ab *EmployeeAddressBuilder) City(s string) *EmployeeAddressBuilder {
	ab.e.City = s
	return ab
}

// Street - sets Employee street field.
func (ab *EmployeeAddressBuilder) Street(s string) *EmployeeAddressBuilder {
	ab.e.Street = s
	return ab
}

// PostalCode - sets Employee postal code field.
func (ab *EmployeeAddressBuilder) PostalCode(s string) *EmployeeAddressBuilder {
	ab.e.PostalCode = s
	return ab
}

// EmployeeJobBuilder - represents employee job builder.
type EmployeeJobBuilder struct {
	EmployeeBuilder
}

// NewEmployeeJobBuilder - creates new instance of EmployeeJobBuilder.
func NewEmployeeJobBuilder(eb *EmployeeBuilder) *EmployeeJobBuilder {
	return &EmployeeJobBuilder{*eb}
}

// Department - sets Employee department field.
func (jb *EmployeeJobBuilder) Department(s string) *EmployeeJobBuilder {
	jb.e.Department = s
	return jb
}

// Role - sets Employee role field.
func (jb *EmployeeJobBuilder) Role(s string) *EmployeeJobBuilder {
	jb.e.Role = s
	return jb
}
