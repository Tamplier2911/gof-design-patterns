package cor

import (
	"fmt"
	"math"
)

// Chain of Responsibility: delegates commands to a chain of processing objects.
//
// Motivation:
// helps to avoids hard-wiring the sender of a request to the receiver.
// helps to have more than one object capable to handle particular request.
// makes it possible to transfer a request to one of several objects without specifying exact one.
// makes it possible to dynamically construct range of handler objects.
//

func ChainOfResponsibility() {
	fmt.Println("\nChain of Responsibility")

	// init client
	c := NewClient(9305)

	// init handlers
	thousands := NewThousandsHandler()
	hundreds := NewHundredsHandler()
	tens := NewTensHandler()
	ones := NewOnesHandler()

	// chain up handlers
	thousands.
		SetNext(hundreds).
		SetNext(tens).
		SetNext(ones)

	// handle client request
	thousands.Handle(c)

	fmt.Printf("client cash: %d\n", c.GetCash())
}

// -- Handler Abstraction

// Handler - represents handler abstraction interface.
type Handler interface {
	SetNext(h Handler) Handler
	Handle(c ClinetInterface)
}

// -- Concreate Handlers

// ThousandsHandler - represents concreate handler.
type ThousandsHandler struct {
	next Handler
}

// NewThousandsHandler - creates new instance of ThousandsHandler.
func NewThousandsHandler() *ThousandsHandler {
	return &ThousandsHandler{}
}

// inBounds - determine if cash is in bounds of thousands.
func (th *ThousandsHandler) inBounds(cash int) bool {
	bounds := int(math.Floor(math.Log10(float64(cash))) + 1)
	return bounds > 3 && bounds < 7
}

// Handle - handle request and/or pass to next handler.
func (th *ThousandsHandler) Handle(c ClinetInterface) {
	cash := c.GetCash()

	// withdraw thousands if any
	if th.inBounds(cash) {
		dif := cash % 1000
		thousands := cash - dif
		cash = c.SetCash(dif)
		fmt.Printf("withdraw %d thousand\n", thousands/1000)
	}

	// if no cash left - return
	if cash <= 0 {
		return
	}

	// invoke next handler in chain
	if th.next != nil {
		th.next.Handle(c)
	}
}

// SetNext - set next handler to chain of handlers.
func (th *ThousandsHandler) SetNext(h Handler) Handler {
	th.next = h
	return h
}

// HundredsHandler - represents concreate handler.
type HundredsHandler struct {
	next Handler
}

// NewHundredsHandler - creates new instance of HundredsHandler.
func NewHundredsHandler() *HundredsHandler {
	return &HundredsHandler{}
}

// inBounds - determine if cash is in bounds of hundreds.
func (th *HundredsHandler) inBounds(cash int) bool {
	bounds := int(math.Floor(math.Log10(float64(cash))) + 1)
	return bounds > 2 && bounds < 4
}

// Handle - handle request and/or pass to next handler.
func (th *HundredsHandler) Handle(c ClinetInterface) {
	cash := c.GetCash()

	// withdraw hundreds if any
	if th.inBounds(cash) {
		dif := cash % 100
		hundreds := cash - dif
		cash = c.SetCash(dif)
		fmt.Printf("withdraw %d hundred\n", hundreds/100)
	}

	// if no cash left - return
	if cash <= 0 {
		return
	}

	// invoke next handler in chain
	if th.next != nil {
		th.next.Handle(c)
	}
}

// SetNext - set next handler to chain of handlers.
func (th *HundredsHandler) SetNext(h Handler) Handler {
	th.next = h
	return h
}

// TensHandler - represents concreate handler.
type TensHandler struct {
	next Handler
}

// NewTensHandler - creates new instance of TensHandler.
func NewTensHandler() *TensHandler {
	return &TensHandler{}
}

// inBounds - determine if cash is in bounds of tens.
func (th *TensHandler) inBounds(cash int) bool {
	bounds := int(math.Floor(math.Log10(float64(cash))) + 1)
	return bounds > 1 && bounds < 3
}

// Handle - handle request and/or pass to next handler.
func (th *TensHandler) Handle(c ClinetInterface) {
	cash := c.GetCash()

	// withdraw tens if any
	if th.inBounds(cash) {
		dif := cash % 10
		tens := cash - dif
		cash = c.SetCash(dif)
		fmt.Printf("withdraw %d tens\n", tens/10)
	}

	// if no cash left - return
	if cash <= 0 {
		return
	}

	// invoke next handler in chain
	if th.next != nil {
		th.next.Handle(c)
	}
}

// SetNext - set next handler to chain of handlers.
func (th *TensHandler) SetNext(h Handler) Handler {
	th.next = h
	return h
}

// OnesHandler - represents concreate handler.
type OnesHandler struct {
	next Handler
}

// NewOnesHandler - creates new instance of OnesHandler.
func NewOnesHandler() *OnesHandler {
	return &OnesHandler{}
}

// inBounds - determine if cash is in bounds of ones.
func (th *OnesHandler) inBounds(cash int) bool {
	bounds := int(math.Floor(math.Log10(float64(cash))) + 1)
	return bounds > 0 && bounds < 2
}

// Handle - handle request and/or pass to next handler.
func (th *OnesHandler) Handle(c ClinetInterface) {
	cash := c.GetCash()

	// withdraw ones if any
	if th.inBounds(cash) {
		c.SetCash(0)
		fmt.Printf("withdraw %d ones\n", cash)
	}

	// if no cash left - return
	if cash <= 0 {
		return
	}

	// invoke next handler in chain
	if th.next != nil {
		th.next.Handle(c)
	}
}

// SetNext - set next handler to chain of handlers.
func (th *OnesHandler) SetNext(h Handler) Handler {
	th.next = h
	return h
}

// -- Client

// ClientInterface - represents Client abstraction interface.
type ClinetInterface interface {
	GetCash() int
	SetCash(cash int) int
}

// Client - represents client entity.
type Client struct {
	cash int
}

// NewClient - creates new instance of Client.
func NewClient(cash int) *Client {
	return &Client{cash}
}

// GetCash - returns current client cash.
func (c *Client) GetCash() int {
	return c.cash
}

// SetCash - updates current client cash.
func (c *Client) SetCash(cash int) int {
	c.cash = cash
	return c.cash
}
