package prototype

import (
	"bytes"
	"encoding/gob"
	"encoding/json"
	"fmt"
	"strings"
)

// Prototype: creates objects by cloning an existing object.
//
// Motivation:
// Complicated objects are not designed from scratch - existing design being re-iterated
// An existing object partially or fully constructed is a Prototype
// We make a copy (deep clone) of the prototype and customized it - cloning must be convenient

func Prototype() {
	fmt.Println("\nPrototype")

	// create person
	p1 := NewPerson(
		"Sherlock",
		NewAddress("Belfast", "Baker Street"),
		[]string{"Dr John H. Watson"},
	)

	// clone person using copy method
	p2 := p1.Copy()
	p2.Address.City = "Brighton"
	p2.Friends = append(p2.Friends, "Mrs Hudson")

	// clone person through serialization
	// p3 := p2.CopySerializationJSON()
	p3 := p2.CopySerializationBin()
	p3.Address.City = "Birmingham"
	p3.Friends = append(p3.Friends, "Molly Hooper")

	// clone person using prototype factory
	p4 := NewLondonPerson("Sherlock", "Baker Street")
	p4.Friends = append(p3.Friends, "Greg Lestrade")

	fmt.Printf(
		"Name: %s | City: %s | Street: %s | Friends: %s\n",
		p1.Name, p1.Address.City, p1.Address.Street, strings.Join(p1.Friends, ", "),
	)
	fmt.Printf(
		"Name: %s | City: %s | Street: %s | Friends: %s\n",
		p2.Name, p2.Address.City, p2.Address.Street, strings.Join(p2.Friends, ", "),
	)
	fmt.Printf(
		"Name: %s | City: %s | Street: %s | Friends: %s\n",
		p3.Name, p3.Address.City, p3.Address.Street, strings.Join(p3.Friends, ", "),
	)
	fmt.Printf(
		"Name: %s | City: %s | Street: %s | Friends: %s\n",
		p4.Name, p4.Address.City, p4.Address.Street, strings.Join(p4.Friends, ", "),
	)
}

// Person - represents person.
type Person struct {
	Name    string
	Address *Address
	Friends []string
}

// NewPerson - creates instance of new Person.
func NewPerson(name string, address *Address, friends []string) *Person {
	return &Person{Name: name, Address: address, Friends: friends}
}

// Address - represents address.
type Address struct {
	City, Street string
}

// NewAddress - creates instance of new Address.
func NewAddress(city string, street string) *Address {
	return &Address{City: city, Street: street}
}

// -- Copy Method

// Copy - deeply copying Person object.
func (p *Person) Copy() *Person {
	return &Person{Name: p.Name, Address: p.Address.Copy(), Friends: append([]string{}, p.Friends...)}
}

// Copy - deeply copying Address object.
func (a *Address) Copy() *Address {
	return &Address{City: a.City, Street: a.Street}
}

// -- Copy through Serialization

// CopySerializationJSON - deeply copying Person object using serialization.
func (p *Person) CopySerializationJSON() *Person {
	bs, _ := json.Marshal(p)
	var cp Person
	_ = json.Unmarshal(bs, &cp)
	return &cp
}

// CopySerializationBin - deeply copying Person object using serialization.
func (p *Person) CopySerializationBin() *Person {
	buff := new(bytes.Buffer)
	enc := gob.NewEncoder(buff)
	_ = enc.Encode(p)

	dec := gob.NewDecoder(buff)
	var cp Person
	_ = dec.Decode(&cp)

	return &cp
}

// -- Prototype Factory

var LondonPerson = Person{Address: &Address{City: "London"}}

func newPersonFromProto(proto *Person, name string, street string) *Person {
	cp := proto.CopySerializationJSON()
	cp.Name = name
	cp.Address.Street = street
	return cp
}

// NewLondonPerson - creates instance of Person.
func NewLondonPerson(name, street string) *Person {
	return newPersonFromProto(&LondonPerson, name, street)
}
