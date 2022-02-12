package proxy

import (
	"fmt"
	"log"
)

// Proxy: provides a placeholder for another object to control access, reduce cost, and reduce complexity.
//
// Motivation:
// controll access to expensive resource - virtual proxy
// restrict access to the object depending on the policies of calling object - protection proxy
// count references to an object or ensure thread-safe work with a real object - smart references
// lazy loading, caching, logging etc..
//

func Proxy() {
	fmt.Println("\nProxy")

	// init subject
	book := NewBook("Plague", []string{
		"The unusual events described in this chronicle occurred in 194- at Oran.",
		"Everyone agreed that, considering their somewhat extraordinary character, they were out of place there.",
		"For its ordinariness is what strikes one first about the town of Oran, which is merely a large " +
			"French port on the Algerian coast, headquarters of the Prefect of a French Department.",
		"The town itself, let us admit, is ugly. It has a smug, placid air and you need time to discover what " +
			"it is that makes it different from so many business centers in other parts of the world.",
		"How to conjure up a picture, for instance, of a town without pigeons, without any trees or gardens, " +
			"where you never hear the beat of wings or the rustle of leaves, a thoroughly negative place, in short?",
	})

	// init client
	s := NewStudent("Albert")

	// use subject by the client
	s.ReadBook(book)

	// init logger proxy
	lbp := NewLoggerBookProxy(book) // log current page and date
	// init caching proxy
	cbp := NewCacheBookProxy(lbp) // caching pages
	// init protection proxy
	pbp := NewPreviewBookProxy(cbp, 2) // in preview mode only three pages are available

	// use subject by the client through set of proxies
	s.ReadBook(pbp)
}

// -- Subject

// BookInterface - represents abstraction (subject interface)
type BookInterface interface {
	GetBookTitle() string
	ReadPage(page int) string
}

// -- Real Subject

// Book - represents real subject.
type Book struct {
	title string
	pages []string
}

// NewBook - creates new instance of Book.
func NewBook(title string, pages []string) BookInterface {
	return &Book{title, pages}
}

// GetBookTitle - returns book title.
func (b *Book) GetBookTitle() string {
	return b.title
}

// ReadPage - returns page content of provided page index.
func (b *Book) ReadPage(page int) string {
	if page > len(b.pages)-1 {
		return ""
	}
	return b.pages[page]
}

// -- Proxy

// LoggerBookProxy - represents logger proxy.
type LoggerBookProxy struct {
	book BookInterface
}

// NewLoggerBookProxy - creates new instance of LoggerBookProxy.
func NewLoggerBookProxy(book BookInterface) BookInterface {
	return &LoggerBookProxy{book}
}

// GetBookTitle - returns book title.
func (lp *LoggerBookProxy) GetBookTitle() string {
	// log title
	log.Printf("- book title %s\n", lp.book.GetBookTitle())
	return lp.book.GetBookTitle()
}

// ReadPage - returns page content of provided page index.
func (lp *LoggerBookProxy) ReadPage(page int) string {
	// log page
	log.Printf("- reading book page %d\n", page)
	return lp.book.ReadPage(page)
}

// CacheBookProxy - represents cache proxy.
type CacheBookProxy struct {
	book  BookInterface
	cache map[int]string
}

// NewCacheBookProxy - creates new instance of CacheBookProxy.
func NewCacheBookProxy(book BookInterface) BookInterface {
	return &CacheBookProxy{book: book, cache: make(map[int]string)}
}

// GetBookTitle - returns book title.
func (cp *CacheBookProxy) GetBookTitle() string {
	return cp.book.GetBookTitle()
}

// ReadPage - returns page content of provided page index.
func (cp *CacheBookProxy) ReadPage(page int) string {
	// get page form cache
	if content, ok := cp.cache[page]; ok {
		fmt.Printf("retrieved page from cache\n")
		return content
	}
	// save page to cache
	cp.cache[page] = cp.book.ReadPage(page)
	fmt.Printf("saved page to cache\n")
	return cp.cache[page]
}

// PreviewBookProxy - represents cache proxy.
type PreviewBookProxy struct {
	book  BookInterface
	pages int
}

// NewPreviewBookProxy - creates new instance of PreviewBookProxy.
func NewPreviewBookProxy(book BookInterface, pages int) BookInterface {
	return &PreviewBookProxy{book, pages}
}

// GetBookTitle - returns book title.
func (cp *PreviewBookProxy) GetBookTitle() string {
	return cp.book.GetBookTitle()
}

// ReadPage - returns page content of provided page index.
func (cp *PreviewBookProxy) ReadPage(page int) string {
	// in preview mode can only access few book pages
	if page > cp.pages {
		return ""
	}
	return cp.book.ReadPage(page)
}

// -- Client

// Student - represents client that going to use subject.
type Student struct {
	name string
}

// NewStudent - creates new instance of Student.
func NewStudent(name string) *Student {
	return &Student{name}
}

// GetName - returns student name.
func (s *Student) GetName() string {
	return s.name
}

// ReadBook - performs book reading.
func (s *Student) ReadBook(book BookInterface) {
	fmt.Printf("reading book: %s\n", book.GetBookTitle())
	for i := 0; ; i++ {
		page := book.ReadPage(i)
		if page == "" {
			break
		}
		fmt.Printf("reading: %s\n", page)
	}
}
