package interpreter

import "fmt"

// Interpreter: implements a specialized language.
//
// Motivation:
// Interpreter processing text input, turning it into sequence of lexical tokens and then parsing those sequences.
// (compilers, interpreters, numeric expressions, regular expressions) are implementations of interpreter pattern.
//

func Interpreter() {
	fmt.Println("\nInterpreter")

	// init context
	ctx := NewContext()

	// init variables
	ctx.SetVariable("x", 2)
	ctx.SetVariable("y", 4)
	ctx.SetVariable("z", 8)

	// init expression
	expression := NewSubstractExpression(
		NewAddExpression(
			NewNumberExpression("y"),
			NewNumberExpression("z"),
		),
		NewNumberExpression("x"),
	)

	// interpret expression
	result := expression.Interpret(ctx)

	fmt.Println(result)
}

// -- Context

// Context - represents interpretation context.
type Context struct {
	variables map[string]int
}

// NewContext - creates new instance of context.
func NewContext() *Context {
	return &Context{variables: make(map[string]int)}
}

// SetVariable - sets variable in context.
func (ctx *Context) SetVariable(name string, value int) {
	ctx.variables[name] = value
}

// GetVariable - retrieves variable from context.
func (ctx *Context) GetVariable(name string) int {
	value, exists := ctx.variables[name]
	if !exists {
		return 0
	}
	return value
}

// -- Abstract Expression

// Expression - represents abstract expression.
type Expression interface {
	Interpret(ctx *Context) int
}

// -- Concreate Expression

// NumberExpression - represents concreate expression.
type NumberExpression struct {
	name string
}

// NewNumberExpression - creates new instance of number expression.
func NewNumberExpression(name string) *NumberExpression {
	return &NumberExpression{name}
}

// Interpret - interprets expression.
func (ne *NumberExpression) Interpret(ctx *Context) int {
	return ctx.variables[ne.name]
}

// AddExpression - represents concreate expression
type AddExpression struct {
	left  Expression
	right Expression
}

// NewAddExpression - creates new instance of add expression.
func NewAddExpression(left, right Expression) *AddExpression {
	return &AddExpression{left, right}
}

// Interpret - interprets expression.
func (as *AddExpression) Interpret(ctx *Context) int {
	return as.left.Interpret(ctx) + as.right.Interpret(ctx)
}

// SubstractExpression - represents concreate expression.
type SubstractExpression struct {
	left  Expression
	right Expression
}

// NewSubstractExpression - creates new instance of substract expression.
func NewSubstractExpression(left, right Expression) *SubstractExpression {
	return &SubstractExpression{left, right}
}

// Interpret - interprets expression.
func (as *SubstractExpression) Interpret(ctx *Context) int {
	return as.left.Interpret(ctx) - as.right.Interpret(ctx)
}
