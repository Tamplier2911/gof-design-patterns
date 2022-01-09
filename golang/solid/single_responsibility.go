package solid

import (
	"bytes"
	"fmt"
	"os"
	"strings"
)

// S - Single Responsibility (should have one reason to change).

// 1. should have responsibility over a single part of that program's functionality, and it should encapsulate that part
// 2. all of that services should be narrowly aligned with that responsibility
// 3. separation of concerns - different modules handling different independent problems/tasks

func SingleResponsibility() {
	fmt.Println("Single Responsibility")

	const _path = "./solid/files/journal_entries.txt"

	// init journal
	j := NewJournal()

	// init local storage
	ls := NewLocalStorage(_path)

	// add entries
	//j.AddEntry("Entry one")
	//j.AddEntry("Entry two")
	//j.AddEntry("Entry three")

	// remove entry
	//j.RemoveEntry(0)

	// persist entries
	//j.StoreEntriesToLocalFile(_path)

	// load entries
	//j.LoadEntriesFromLocalFile(_path)

	// store locally
	//ls.StoreToLocalFile(j)

	// load from local
	ls.LoadFromLocalFile(j)

	fmt.Printf("Entries: %s, Count: %d \n", j.Entries, j.EntriesCount)
}

// Journal - represents journal.
type Journal struct {
	Entries      []string
	EntriesCount int
}

// NewJournal - creates instance of a Journal.
func NewJournal() *Journal {
	return &Journal{}
}

// AddEntry - adds new entry to Journal.
func (j *Journal) AddEntry(text string) {
	// add entry
	j.Entries = append(j.Entries, text)
	j.EntriesCount++
}

// RemoveEntry - removes entry from Journal.
func (j *Journal) RemoveEntry(index int) {
	// remove entry
	if index < 0 || index > (j.EntriesCount-1) {
		return
	}
	j.Entries = append(j.Entries[:index], j.Entries[index+1:]...)
	j.EntriesCount--
}

// String - convert entries to string.
func (j *Journal) String() string {
	return strings.Join(j.Entries, "\n")
}

// Parse - convert string to entries.
func (j *Journal) Parse(s string) {
	j.Entries = strings.Split(s, "\n")
	j.EntriesCount = len(j.Entries)
}

// Adding more responsibilities would violate SRP

// StoreEntriesToLocalFile - persist Journal entries locally.
func (j *Journal) StoreEntriesToLocalFile(path string) {

	// create or truncate file
	f, _ := os.Create(path)
	defer f.Close()

	// write data to a file
	_, _ = f.Write([]byte(strings.Join(j.Entries, "\n")))
	_ = f.Sync()
}

// LoadEntriesFromLocalFile - loads Journal entries from local file.
func (j *Journal) LoadEntriesFromLocalFile(path string) {

	// open file
	f, _ := os.Open(path)
	defer f.Close()

	// read data to buffer
	buf := new(bytes.Buffer)
	buf.ReadFrom(f)
	data := buf.String()

	// add data to object state
	j.Entries = strings.Split(data, "\n")
	j.EntriesCount = len(j.Entries)
}

// StoreEntriesCloud -.
// LoadEntriesCloud -.
// ...
// ...
// God Object

// Separation of concerns

// Stringer - represents stinger interface.
type Stringer interface {
	String() string
}

// Parser - represents parser interface.
type Parser interface {
	Parse(s string)
}

// LocalStorage - represents local data storage.
type LocalStorage struct {
	Path string
}

// NewLocalStorage - creates instance of LocalStorage.
func NewLocalStorage(path string) *LocalStorage {
	return &LocalStorage{path}
}

// StoreToLocalFile - stores string in local file.
func (ls *LocalStorage) StoreToLocalFile(s Stringer) {
	// create or truncate file
	f, _ := os.Create(ls.Path)
	defer f.Close()

	// write data to a file
	_, _ = f.Write([]byte(s.String()))
	_ = f.Sync()
}

// LoadFromLocalFile - loads string from local file.
func (ls *LocalStorage) LoadFromLocalFile(p Parser) {
	// open file
	f, _ := os.Open(ls.Path)
	defer f.Close()

	// read data to buffer
	buf := new(bytes.Buffer)
	buf.ReadFrom(f)

	// add data to object state
	p.Parse(buf.String())
}
