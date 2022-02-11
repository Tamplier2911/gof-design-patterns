package singleton

import (
	"fmt"
	"sync"
)

// Singleton: restricts object creation for a type to only one instance.
//
// Motivation:
// For some components it only makes sense to have single instance in the system. (Repository, Factory)
// When constructor call is expensive and we want to restrict it to a single call, provide every consumer with same instance.
// Prevent client from making any addion copies.

func Singleton() {
	fmt.Println("\nSingleton")

	// singleton
	sdb := NewSingletoneDatabase() // constructed
	// sdb2 := NewSingletoneDatabase() // instantiated
	// sdb3 := NewSingletoneDatabase() // instantiated
	const _city = "Tokyo"
	fmt.Println(sdb.GetCityPopulation(_city))

	//
}

// -- Singleton

// Repository - represents repository interface.
type Repository interface {
	GetCityPopulation(name string) string
}

// func init(){} - thread safety only
// sync.Once - thread safety and lazyness
var once sync.Once
var instance *singletonDatabase

// singletonDatabase - represents database connection, implements repository interface.
type singletonDatabase struct {
	DB map[string]string
}

// NewSingletoneDatabase - creates new instance of singletoneDatabase
func NewSingletoneDatabase() *singletonDatabase {
	// once insures that object constructed only once
	once.Do(func() {
		fmt.Println("constructing object")
		instance = &singletonDatabase{
			// mock loading data from elsewhere
			DB: map[string]string{
				"Beijing":  "21,542,000",
				"Tokyo":    "13,929,286",
				"Kinshasa": "12,691,000",
				"Moscow":   "12,506,468",
				"Jakarta":  "10,075,310",
				"Seoul":    "9,838,892",
				"Cairo":    "9,848,576",
				"London":   "8,908,081",
				"Tehran":   "8,693,706",
				"Baghdad":  "6,719,500",
			},
		}
	})
	// every next time just return reference to singleton
	return instance
}

// GetCityByPopulation - retrieves city population by provided name.
func (sdb *singletonDatabase) GetCityPopulation(name string) string {
	return sdb.DB[name]
}
