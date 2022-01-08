package solid

import "fmt"

// I - Interface Segregation
// 1. specific interfaces are better than general purpose ones
// 2. types should not implement methods they don't need

func InterfaceSegregation() {
	fmt.Println("Interface segregation")
	fmt.Println("START")

	// doc
	d := Document{"Document1"}

	// multi func printer
	mfdp := &MultiFunctionDocumentPrinter{}
	mfdp.Print(d)

	// standard printer
	sp := &StandardPrinter{}
	sp.Print(d)

	// ---

	// printer
	pr := NewPrinter()
	pr.PrintDocument(d)

	// scanner
	sc := NewScanner()
	sc.ScanDocument(d)

	// fax
	fx := NewFax()
	fx.FaxDocument(d)

	// multi func printer enhanced
	mfpe := NewMultiFunctionDocumentPrinterEnhanced(pr, sc, fx)
	mfpe.PrintDocument(d)
	mfpe.ScanDocument(d)
	mfpe.FaxDocument(d)

	fmt.Println("END")
}

// Document - represents document.
type Document struct {
	Name string
}

// DocumentWorker - represents document worker interface.
type DocumentWorker interface {
	Print(d Document)
	Scan(d Document)
	Fax(d Document)
}

// MultiFunctionDocumentPrinter - represents multifunctional printer.
type MultiFunctionDocumentPrinter struct{}

// Print - prints document.
func (m MultiFunctionDocumentPrinter) Print(d Document) {
	fmt.Printf("Printing: %s\n", d.Name)
}

// Scan - scans document.
func (m MultiFunctionDocumentPrinter) Scan(d Document) {
	fmt.Printf("Scanning: %s\n", d.Name)
}

// Fax - sending fax.
func (m MultiFunctionDocumentPrinter) Fax(d Document) {
	fmt.Printf("Faxing: %s\n", d.Name)
}

// ---

// StandardPrinter - represents standard printer.
type StandardPrinter struct{}

// Print - prints document.
func (s StandardPrinter) Print(d Document) {
	fmt.Printf("Printing: %s\n", d.Name)
}

// Scan - scans document.
func (s StandardPrinter) Scan(_ Document) {
	// don't have functionality to scan
	panic("not supported")
}

// Fax - sending fax.
func (s StandardPrinter) Fax(_ Document) {
	// don't have functionality to fax
	panic("not supported")
}

// ---

// DocumentPrinter - represents document printer interface.
type DocumentPrinter interface {
	PrintDocument(d Document)
}

// DocumentScanner - represents document scanner interface.
type DocumentScanner interface {
	ScanDocument(d Document)
}

// DocumentFax - represents document fax interface.
type DocumentFax interface {
	FaxDocument(d Document)
}

// DocumentWorkerEnhanced - represents multifunctional document worker.
type DocumentWorkerEnhanced interface {
	DocumentPrinter
	DocumentScanner
	DocumentFax
}

// --

// Printer - represents document printer.
type Printer struct{}

// NewPrinter - creates new instance of Printer.
func NewPrinter() *Printer {
	return &Printer{}
}

// PrintDocument - prints Document.
func (s *Printer) PrintDocument(d Document) {
	fmt.Printf("Printing: %s\n", d.Name)
}

// Scanner - represents document scanner.
type Scanner struct{}

// NewScanner - creates new instance of Scanner.
func NewScanner() *Scanner {
	return &Scanner{}
}

// ScanDocument - scans Document.
func (s *Scanner) ScanDocument(d Document) {
	fmt.Printf("Scanning: %s\n", d.Name)
}

// Fax - represents document fax.
type Fax struct{}

// NewFax - creates new instance of Fax
func NewFax() *Fax {
	return &Fax{}
}

// FaxDocument - faxes Document.
func (s *Fax) FaxDocument(d Document) {
	fmt.Printf("Faxing: %s\n", d.Name)
}

// MultiFunctionDocumentPrinterEnhanced - represents multifunctional printer.
// Leverage decorator pattern.
type MultiFunctionDocumentPrinterEnhanced struct {
	p DocumentPrinter
	s DocumentScanner
	f DocumentFax
}

// NewMultiFunctionDocumentPrinterEnhanced - creates new instance of MultiFunctionDocumentPrinterEnhanced.
func NewMultiFunctionDocumentPrinterEnhanced(
	p DocumentPrinter,
	s DocumentScanner,
	f DocumentFax,
) *MultiFunctionDocumentPrinterEnhanced {
	return &MultiFunctionDocumentPrinterEnhanced{p, s, f}
}

// PrintDocument - prints Document.
func (m *MultiFunctionDocumentPrinterEnhanced) PrintDocument(d Document) {
	m.p.PrintDocument(d)
}

// ScanDocument - scans Document.
func (m *MultiFunctionDocumentPrinterEnhanced) ScanDocument(d Document) {
	m.s.ScanDocument(d)
}

// FaxDocument - faxes Document.
func (m *MultiFunctionDocumentPrinterEnhanced) FaxDocument(d Document) {
	m.f.FaxDocument(d)
}
