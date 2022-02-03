package facade

import (
	"fmt"
	"math/rand"
	"os"
	"time"
)

// Facade: provides a simplified interface to a large body of code.
//
// Motivation:
// provide simple API for complicated underlying systems
// allows to define one point of interaction between the client and complex system
// reduce the number of dependencies between the client and a complex system

func Facade() {
	fmt.Println("Facade")

	// Facade - IDE

	// init faacade
	ide := NewIDEFacade(NewTextEditor(), NewCompiler(), NewRuntime(), NewConsole())
	// init client
	client := NewDeveloper(ide)
	// create app
	client.CreateApplication("Hello, world!")

	// Facade - Magic Square Generator

	// init magic square generator
	_ = NewMagicSquareGenerator(NewGenerator(), NewSplitter(), NewVerifier())
	// generate magic square
	// ms := sg.GenerateSquare(3)
}

// -- Complex Underlying System

// TextEditorInterface - represents text editor interface.
type TextEditorInterface interface {
	WriteText(text string) string
	SaveText() string
}

// TextEditor - represents basic text editor.
type TextEditor struct {
	stack []string
	cur   string
}

// NewTextEditor - creates new instance of TextEditor.
func NewTextEditor() *TextEditor {
	return &TextEditor{[]string{}, ""}
}

// WriteText - write text into text editor.
func (te *TextEditor) WriteText(text string) string {
	te.cur += text
	return te.cur
}

// SaveText - save curent text state in text editor.
func (te *TextEditor) SaveText() string {
	te.stack = append(te.stack, te.cur)
	te.cur = ""
	return te.stack[len(te.stack)-1]
}

// CompilerInterface - represents compiler interface.
type CompilerInterface interface {
	Compile(code string) string
}

// Compiler - represents compiler.
type Compiler struct{}

// NewCompiler - creates new instance of Compiler.
func NewCompiler() *Compiler {
	return &Compiler{}
}

// Compile - compiles provided code.
func (c *Compiler) Compile(code string) string {
	return fmt.Sprintf("compiled(%s)", code)
}

// RuntimeInterface - represents runtime interface.
type RuntimeInterface interface {
	Execute(bin string) string
}

// Runtime - represents application runtime.
type Runtime struct{}

// NewRuntime - creates new instance of Runtime.
func NewRuntime() *Runtime {
	return &Runtime{}
}

// Execute - executes provided binaries.
func (r *Runtime) Execute(bin string) string {
	return fmt.Sprintf("executed(%s)", bin)
}

// ConsoleInterface - represents console interface.
type ConsoleInterface interface {
	Output(result string)
}

// Console - represents console.
type Console struct{}

// NewConsole - creates new instance of Console.
func NewConsole() *Console {
	return &Console{}
}

// Output - outputs execution result into standart output.
func (cl *Console) Output(result string) {
	fmt.Fprintf(os.Stdout, "output(%s)\n", result)
}

// -- Facade

// IDEInterface - represents IDE interface.
type IDEInterface interface {
	Run(code string)
}

// IDEFacade - represents IDE, which lavarage complex underlying systems to provide simple API.
type IDEFacade struct {
	TextEditorInterface
	CompilerInterface
	RuntimeInterface
	ConsoleInterface
}

// NewIDEFacade - creates new instance of IDEFacade.
func NewIDEFacade(
	te TextEditorInterface,
	cm CompilerInterface,
	rn RuntimeInterface,
	cl ConsoleInterface,
) *IDEFacade {
	return &IDEFacade{te, cm, rn, cl}
}

// Run - runs application code.
func (ide *IDEFacade) Run(text string) {
	ide.WriteText(text)
	code := ide.SaveText()
	bin := ide.Compile(code)
	out := ide.Execute(bin)
	ide.Output(out)
}

// -- Client

// Developer - represents client.
type Developer struct {
	tool IDEInterface
}

// NewDeveloper - creates new instance of developer.
func NewDeveloper(tool IDEInterface) *Developer {
	return &Developer{tool}
}

// CreateApplication - creates application.
func (d *Developer) CreateApplication(appCode string) {
	d.tool.Run(appCode)
}

// -- Magic Square Generator

// -- Complex Underlying System

// GeneratorInterface - represents generator interface.
type GeneratorInterface interface {
	GenerateSlice(n int) []int
}

// Generator - represents slice generator.
type Generator struct{}

// NewGenerator - creates new instance of Generator.
func NewGenerator() *Generator {
	return &Generator{}
}

// GenerateSlice - generates slice of n size.
func (g *Generator) GenerateSlice(n int) []int {
	rand.Seed(time.Now().UnixNano())
	min := 1
	max := 6

	sl := make([]int, n)
	for i := 0; i < n; i++ {
		sl[i] = rand.Intn(max-min) + min
	}
	return sl
}

// SplitterInterface - represents splitter interface.
type SplitterInterface interface {
	Split(inp [][]int) [][]int
}

// Splitter - represents splitter.
type Splitter struct{}

// NewSplitter - creates new instance of Splitter.
func NewSplitter() *Splitter {
	return &Splitter{}
}

// Split - splits square matrix into individual parallel and diagonal pieces.
func (s *Splitter) Split(inp [][]int) [][]int {
	rows := len(inp)
	cols := len(inp[0])
	res := make([][]int, 0)

	// get rows
	for i := 0; i < rows; i++ {
		row := make([]int, 0)
		for j := 0; j < cols; j++ {
			row = append(row, inp[i][j])
		}
		res = append(res, row)
	}

	// get cols
	for i := 0; i < cols; i++ {
		col := make([]int, 0)
		for j := 0; j < rows; j++ {
			col = append(col, inp[j][i])
		}
		res = append(res, col)
	}

	// get diagonals
	dia1 := make([]int, 0)
	dia2 := make([]int, 0)
	for i := 0; i < cols; i++ {
		for j := 0; j < rows; j++ {
			if i == j {
				dia1 = append(dia1, inp[j][i])
			}
			k := rows - j - 1
			if i == k {
				dia2 = append(dia2, inp[j][i])
			}
		}
	}
	res = append(res, dia1)
	res = append(res, dia2)

	fmt.Println(res)
	return res
}

// VerifierInterface - represents verifier interface.
type VerifierInterface interface {
	Sum(sl []int) int
	Verify(res [][]int) bool
}

// Verifier - represents verifier.
type Verifier struct{}

// NewVerifier - creates new instance of Verifier.
func NewVerifier() *Verifier {
	return &Verifier{}
}

// Verify - verifies that provided slices have equal sum each.
func (v *Verifier) Verify(res [][]int) bool {
	sum := v.Sum(res[0])
	// fmt.Println(sum)
	for _, sl := range res {
		// fmt.Println(res)
		if sum != v.Sum(sl) {
			return false
		}
	}

	return true
}

// Sum - sums slice elements.
func (v *Verifier) Sum(sl []int) int {
	sum := 0
	for _, n := range sl {
		sum += n
	}
	return sum
}

// -- Facade

// MagicSquareGenerator - represents magic square generator facade.
type MagicSquareGenerator struct {
	GeneratorInterface
	SplitterInterface
	VerifierInterface
}

// NewMagicSquareGenerator - creates new instance of MagicSquareGenerator.
func NewMagicSquareGenerator(
	generator GeneratorInterface,
	splitter SplitterInterface,
	verifier VerifierInterface,
) *MagicSquareGenerator {
	return &MagicSquareGenerator{generator, splitter, verifier}
}

// GenerateSquare - generates magic square.
func (msg *MagicSquareGenerator) GenerateSquare(size int) [][]int {

	// generate square
	sq := make([][]int, 0)
	for i := 0; i < size; i++ {
		sq = append(sq, msg.GenerateSlice(size))
	}

	// split square into pieces
	res := msg.Split(sq)

	// verify that magic square is valid
	if !msg.Verify(res) {
		// retry if square is invalid
		return msg.GenerateSquare(size)
	}

	return sq
}
