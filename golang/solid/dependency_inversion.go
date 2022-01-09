package solid

import "fmt"

// Dependency Inversion
// 1. high-level modules should not import anything from low-level modules, both should depend on abstractions
// 2. abstractions should not depend on details, details (concrete implementations) should depend on abstractions

func DependencyInversion() {
	fmt.Println("Dependency Inversion")

	const _dadName = "Anakin Skywalker"
	const _sonName = "Luke Skywalker"
	const _daughterName = "Leia Amidala Skywalker"

	dad := NewPerson(_dadName)
	son := NewPerson(_sonName)
	daughter := NewPerson(_daughterName)

	kns := NewKinships()
	kns.AddParentKinship(dad, son)
	kns.AddParentKinship(dad, daughter)
	kns.AddSiblingKinship(son, daughter)

	sr := NewSearch(kns)
	sr.FindChildren(_dadName)
	sr.FindSiblings(_sonName)
}

// Relationship - describes relationship between two Person.
type Relationship string

const (
	ParentRelationship  Relationship = "parent"
	ChildRelationship   Relationship = "child"
	SiblingRelationship Relationship = "sibling"
)

// Person - represents person.
type Person struct {
	Name string
}

// NewPerson - creates new instance of Person.
func NewPerson(name string) *Person {
	return &Person{Name: name}
}

// Kinship - represents kinship between two Person
type Kinship struct {
	From         *Person
	To           *Person
	Relationship Relationship
}

// NewKinship - creates new instance of kinship.
func NewKinship(from *Person, to *Person, rel Relationship) *Kinship {
	return &Kinship{From: from, To: to, Relationship: rel}
}

// -- higher level module

// Kinships - represents collection of Kinship.
type Kinships struct {
	Kinships []*Kinship // if storage mechanic changed, this will break all lower level modules that depend on it.
}

// NewKinships - creates new instance of Kinship collection.
func NewKinships() *Kinships {
	return &Kinships{}
}

// AddParentKinship - add parent Kinship relation.
func (kss *Kinships) AddParentKinship(from *Person, to *Person) {
	kss.Kinships = append(kss.Kinships, NewKinship(from, to, ParentRelationship))
	kss.Kinships = append(kss.Kinships, NewKinship(to, from, ChildRelationship))
}

// AddSiblingKinship - add sibling Kinship relation.
func (kss *Kinships) AddSiblingKinship(from *Person, to *Person) {
	kss.Kinships = append(kss.Kinships, NewKinship(from, to, SiblingRelationship))
	kss.Kinships = append(kss.Kinships, NewKinship(to, from, SiblingRelationship))
}

// FindChildren - find parent Kinship relation.
func (kss *Kinships) FindChildren(name string) []*Kinship {
	kns := make([]*Kinship, 0)
	for _, kn := range kss.Kinships {
		if kn.From.Name == name && kn.Relationship == ParentRelationship {
			kns = append(kns, kn)
		}
	}
	return kns
}

// FindSiblings - find sibling Kinship relation.
func (kss *Kinships) FindSiblings(name string) []*Kinship {
	kns := make([]*Kinship, 0)
	for _, kn := range kss.Kinships {
		if kn.From.Name == name && kn.Relationship == SiblingRelationship {
			kns = append(kns, kn)
		}
	}
	return kns
}

// -- depends on interface

type KinshipsViewer interface {
	FindChildren(name string) []*Kinship
	FindSiblings(name string) []*Kinship
}

// -- lower level module

// Search - represent search mechanism (low lvl module).
type Search struct {
	// Kinships *Kinships
	Viewer KinshipsViewer
}

// NewSearch - creates new Search module.
func NewSearch(viewer KinshipsViewer) *Search {
	return &Search{Viewer: viewer}
}

/*
// NewSearch - creates new Search module.
func NewSearch(kinships *Kinships ) *Search {
	return &Search{Kinships: kinships}
}
*/

// FindChildren - searches for children by provided name.
func (sr *Search) FindChildren(name string) {
	for _, kn := range sr.Viewer.FindChildren(name) {
		if kn.From.Name == name && kn.Relationship == ParentRelationship {
			fmt.Printf("%s is a %s of %s\n", kn.From.Name, kn.Relationship, kn.To.Name)
		}
	}
}

// FindSiblings - searches for siblings by provided name.
func (sr *Search) FindSiblings(name string) {
	for _, kn := range sr.Viewer.FindSiblings(name) {
		if kn.From.Name == name && kn.Relationship == SiblingRelationship {
			fmt.Printf("%s is a %s of %s\n", kn.From.Name, kn.Relationship, kn.To.Name)
		}
	}
}
