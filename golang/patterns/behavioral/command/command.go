package command

import (
	"fmt"
)

// Command: creates objects that encapsulate actions and parameters.
//
// Motivation:
// helps to pass as parameters certain actions that are called in response to other actions.
// when we need to ensure the execution of the request queue, as well as their possible cancellation.
// when we want to support logging of changes as a result of requests.
//

func Command() {
	fmt.Println("\nCommand")

	// init user
	user := NewUser("user", "example@email.com")

	// init receiver
	receiver := NewBankAccount(user)

	// init commands
	deposit := NewDeposit(receiver, 1000)
	withdraw := NewWithdraw(receiver, 500)

	// init invoker
	invoker := NewTerminal()

	fmt.Printf("balance before deposit: %d\n", receiver.balance)

	// invoke commands
	invoker.SetCommand(deposit)
	invoker.Run()
	fmt.Printf("balance after deposit: %d\n", receiver.balance)

	invoker.SetCommand(withdraw)
	invoker.Run()
	fmt.Printf("balance after withdraw: %d\n", receiver.balance)

	invoker.Cancel()
	fmt.Printf("balance after withdraw undo: %d\n", receiver.balance)
}

// -- Command Abstraction

// Operation - represents Command Abstraction.
type Operation interface {
	Execute()
	Undo()
}

// -- Concreate Command

// Deposit - represents Concreate Command.
type Deposit struct {
	account   *BankAccount
	amount    int
	isCompete bool
}

// NewDeposit - creates new instance of Deposit.
func NewDeposit(acc *BankAccount, amount int) *Deposit {
	return &Deposit{acc, amount, false}
}

// Execute - executes command.
func (d *Deposit) Execute() {
	d.account.balance += d.amount
	d.isCompete = true
}

// Undo - undoes command execution.
func (d *Deposit) Undo() {
	if d.isCompete && d.account.balance-d.amount >= 0 {
		d.account.balance -= d.amount
		d.isCompete = false
	}
}

// Withdraw - represents Concreate Command
type Withdraw struct {
	account   *BankAccount
	amount    int
	isCompete bool
}

// NewWithdraw - creates new instance of Withdraw.
func NewWithdraw(acc *BankAccount, amount int) *Withdraw {
	return &Withdraw{acc, amount, false}
}

// Execute - executes command.
func (d *Withdraw) Execute() {
	if d.account.balance-d.amount >= 0 {
		d.account.balance -= d.amount
		d.isCompete = true
	}
}

// Undo - undoes command execution.
func (d *Withdraw) Undo() {
	if d.isCompete {
		d.account.balance += d.amount
		d.isCompete = false
	}
}

// -- Receiver

// Account - represents
type BankAccount struct {
	user    *User
	balance int
}

// NewBankAccount - creates new instance of BankAccount.
func NewBankAccount(u *User) *BankAccount {
	return &BankAccount{u, 0}
}

// -- Invoker

// Terminal - represents invoker.
type Terminal struct {
	operation Operation
}

// NewTerminal - creates new instance of terminal.
func NewTerminal() *Terminal {
	return &Terminal{nil}
}

// SetCommand - sets terminal command.
func (t *Terminal) SetCommand(o Operation) {
	t.operation = o
}

// Run - runs terminal command.
func (t *Terminal) Run() {
	if t.operation != nil {
		t.operation.Execute()
	}
}

// Cancel - cancels terminal command.
func (t *Terminal) Cancel() {
	if t.operation != nil {
		t.operation.Undo()
	}
}

// -- Auxiliary types

// User - represents user entity.
type User struct {
	name, email string
}

// NewUser - creates new instance of user.
func NewUser(name, email string) *User {
	return &User{name, email}
}
