package bridge

import "fmt"

// Bridge: decouples an abstraction from its implementation so that the two can vary independently.
//
// Motivation:
// Prevents a 'cartesian product' complexity explosion AxBxC scenario.
// shape: Circle, Square ...
// type:  Raster, Vector ...
// types: shape * type = RasterCircle, VectorCircle, RasterSquare, VectorSquare

func Bridge() {
	fmt.Println("Bridge")

	// init concreate implementors
	rrn := NewRasterRenderer()
	vrn := NewVectorRenderer()

	// init refined abstractions
	c := NewCircle(rrn, 5.0)
	s := NewSquare(vrn, 5)

	// run refined abstraction method that utilize concreate implementors logic
	c.Render()
	s.Render()
}

// -- Abstraction

type Shape struct {
	renderer Renderer // abstraction requires implementor
}

// /\
//
// Bridge
//
// \/

// -- Implementor

// Renderer - represents rendering interface (implementor).
type Renderer interface {
	RenderCircle(radius float64)
	RenderSquare(side int)
}

// -- Refined Absctraction.

// Circle - represents circle (refined abstraction).
type Circle struct {
	Shape
	radius float64
}

// NewCircle - creates new instance of a circle.
func NewCircle(renderer Renderer, radius float64) *Circle {
	return &Circle{Shape{renderer}, radius}
}

// Render - renders circle image.
func (c *Circle) Render() {
	c.Shape.renderer.RenderCircle(c.radius)
}

// Square - represents square (refined abstraction).
type Square struct {
	Shape
	side int
}

// NewSquare - creates new instance of a square.
func NewSquare(renderer Renderer, side int) *Square {
	return &Square{Shape{renderer}, side}
}

// Render - renders square image.
func (s *Square) Render() {
	s.Shape.renderer.RenderSquare(s.side)
}

// -- Concreate Implementor

// RasterRenderer - represents raster renderer (concreate implementor).
type RasterRenderer struct{}

// NewRasterRenderer - creates new instance of raster renderer.
func NewRasterRenderer() *RasterRenderer {
	return &RasterRenderer{}
}

// RenderCircle - renders circle.
func (rr *RasterRenderer) RenderCircle(radius float64) {
	fmt.Printf("rendering circle as raster: %f\n", radius)
}

// RenderSquare - renders square.
func (rr *RasterRenderer) RenderSquare(side int) {
	fmt.Printf("rendering square as raster: %d\n", side)
}

// VectorRenderer - represents vector renderer (concreate implementor).
type VectorRenderer struct{}

// NewVectorRenderer - creates new instance of vector renderer.
func NewVectorRenderer() *VectorRenderer {
	return &VectorRenderer{}
}

// RenderCircle - renders circle.
func (vr *VectorRenderer) RenderCircle(radius float64) {
	fmt.Printf("rendering circle as vector: %f\n", radius)
}

// RenderSquare - renders square.
func (vr *VectorRenderer) RenderSquare(side int) {
	fmt.Printf("rendering square as vector: %d\n", side)
}
