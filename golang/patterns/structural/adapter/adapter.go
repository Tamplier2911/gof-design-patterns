package adapter

import (
	"crypto/md5"
	"encoding/json"
	"fmt"
	"strings"
)

// Adapter: allows types with incompatible interfaces to work together by wrapping its own interface around that of an already existing type.
//
// Motivation:
// Every type cannot conform every possible interface.
// Adapter is a construct which dapats an existing interface X to conform to the required interface Y.

func Adapter() {
	fmt.Println("Adapter")

	// init client
	pr := NewImagePrinter()

	// init target
	rm := NewRasterImage()
	rm.AddPoint(NewPoint(2, 1))
	rm.AddPoint(NewPoint(7, 1))
	rm.AddPoint(NewPoint(4, 2))
	rm.AddPoint(NewPoint(5, 2))
	rm.AddPoint(NewPoint(4, 3))
	rm.AddPoint(NewPoint(5, 3))

	// use target by client
	pr.PrintImage(rm)

	// init adaptee
	vm := NewVectorImage()
	vm.AddLine(NewLine(0, 0, 10, 0))
	vm.AddLine(NewLine(0, 0, 0, 10))
	vm.AddLine(NewLine(0, 10, 10, 10))
	vm.AddLine(NewLine(10, 0, 10, 10))

	// init cache
	pc := NewPointsCache()

	// init adapter
	via := NewVectorToRasterAdapter(vm, pc)
	_ = NewVectorToRasterAdapter(vm, pc)
	_ = NewVectorToRasterAdapter(vm, pc)

	// use adaptee by client
	pr.PrintImage(via)
}

// -- Adapter

// vectorToRasterAdapter - represents vector to raster image adapter.
type vectorToRasterAdapter struct {
	image []Point
	cache PointsCacheInterface
}

// NewVectorToRasterAdapter - creates new instance of vector to raster image adapter.
func NewVectorToRasterAdapter(vi *VectorImage, lc PointsCacheInterface) PrintableImage { // return interface
	va := &vectorToRasterAdapter{
		cache: lc,
	}
	// convert vector image lines into a raster image points
	for _, line := range vi.Image {
		va.addLine(line)
	}
	return va
}

// addLine - adds vector image line in a raster image point representation.
func (va *vectorToRasterAdapter) addLine(l Line) {

	// try to get points from cache
	sum := va.cache.GetSum(l)
	if va.cache.Found(sum) {
		fmt.Println("got points from cache")
		va.image = va.cache.Retrieve(sum)
		return
	}

	fmt.Println("converting lines to points")
	// if both x and y grows, then line is diagonal
	if l.X1 != l.X2 && l.Y1 != l.Y2 {
		for i, j := l.X1, l.Y1; i < l.X2 && j < l.Y2; i, j = i+1, j+1 {
			va.image = append(va.image, Point{X: i, Y: j})
		}

		for i, j := l.X2, l.Y2; i > l.X1 && j > l.Y1; i, j = i-1, j-1 {
			va.image = append(va.image, Point{X: i, Y: j})
		}
	}

	// if y doesn't change, then line is - horizontal
	if l.Y1 == l.Y2 {
		for i := l.X1; i <= l.X2; i++ {
			va.image = append(va.image, Point{X: i, Y: l.Y1})
		}
	}

	// if x doesen't change, then line is - vertical
	if l.X1 == l.X2 {
		for i := l.Y1; i <= l.Y2; i++ {
			va.image = append(va.image, Point{X: l.X1, Y: i})
		}
	}

	// store points to cache
	va.cache.Store(sum, va.image)
}

// GetPoints - gets raster image pixel points (implement PrintableImage).
func (va *vectorToRasterAdapter) GetPoints() []Point {
	return va.image
}

// -- Adaptee

// VectorImage - represents image built of []Line.
type VectorImage struct {
	Image []Line
}

// NewVectorImage - creates instance of new VectorImage.
func NewVectorImage() *VectorImage {
	return &VectorImage{Image: make([]Line, 0)}
}

// AddLine - add line to the image.
func (rm *VectorImage) AddLine(l Line) {
	rm.Image = append(rm.Image, l)
}

// -- Target

// RasterImage - represents image build of []Point.
type RasterImage struct {
	Image []Point
}

// NewRasterImage - creates instance of new RasterImage.
func NewRasterImage() *RasterImage {
	return &RasterImage{Image: make([]Point, 0)}
}

// AddPoint - add point to the image.
func (rm *RasterImage) AddPoint(p Point) {
	rm.Image = append(rm.Image, p)
}

// GetPoints - gets raster image pixel points.
func (rm *RasterImage) GetPoints() []Point {
	return rm.Image
}

// -- Client

// PrintableImage - represets printable image interface.
type PrintableImage interface {
	GetPoints() []Point
}

// ImagePrinter - represents printer.
type ImagePrinter struct{}

// NewImagePrinter - creates new instance of ImagePrinter.
func NewImagePrinter() *ImagePrinter {
	return &ImagePrinter{}
}

// PrintImage - prints image into the console.
func (ip *ImagePrinter) PrintImage(rm PrintableImage) {

	// get max height and max width of the image
	maxH := 0 // y
	maxW := 0 // x
	for _, p := range rm.GetPoints() {
		if p.Y > maxH {
			maxH = p.Y
		}
		if p.X > maxW {
			maxW = p.X
		}
	}

	// create a matrix based on max height and max width values
	mx := make([][]string, maxH+1)
	for y := range mx {
		mx[y] = make([]string, maxW+1)
		// fill matrix with whitespaces
		for x := range mx[y] {
			mx[y][x] = " "
		}
	}

	// range over each point in raster image
	for _, p := range rm.GetPoints() {
		// represent point on matrix
		mx[p.Y][p.X] = "."
	}

	// combine represented points into a string representation
	result := ""
	for i := len(mx) - 1; i >= 0; i-- { // loop from back in order to create human friendly coordinate view
		result += strings.Join(mx[i], "") + "\n"
	}

	fmt.Println(result)
}

// -- Cache

// PointsCacheInterface - represents points cache interface.
type PointsCacheInterface interface {
	GetSum(l interface{}) [16]byte
	Found(sum [16]byte) bool
	Retrieve(sum [16]byte) []Point
	Store(sum [16]byte, pp []Point)
}

// PointsCache - represets points cache.
type PointsCache struct {
	Cache map[[16]byte][]Point
}

// NewPointsCache - creates new instance of PointsCache.
func NewPointsCache() *PointsCache {
	return &PointsCache{Cache: make(map[[16]byte][]Point)}
}

// GetSum - generates md5 sum from provided line.
func (lc *PointsCache) GetSum(l interface{}) [16]byte {
	bs, _ := json.Marshal(l)
	return md5.Sum(bs)
}

// Found - indicates if key was found in map.
func (lc *PointsCache) Found(sum [16]byte) bool {
	_, ok := lc.Cache[sum]
	return ok
}

// Retrieve - retrieves []Point from cache from by provided sum.
func (lc *PointsCache) Retrieve(sum [16]byte) []Point {
	return lc.Cache[sum]
}

// Store - stores []point in cache by provided sum.
func (lc *PointsCache) Store(sum [16]byte, pp []Point) {
	lc.Cache[sum] = pp
}

// -- Auxillary types

// Line - represents line.
type Line struct {
	X1, Y1, X2, Y2 int
}

// NewLine - creates new instance of Line.
func NewLine(x1, y1, x2, y2 int) Line {
	return Line{x1, y1, x2, y2}
}

// Point - represents point.
type Point struct {
	X, Y int
}

// NewPoint - creates new instance of Point.
func NewPoint(x, y int) Point {
	return Point{x, y}
}
