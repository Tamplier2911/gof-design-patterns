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

	// create raster image
	rm := &RasterImage{
		[]Point{
			{X: 2, Y: 1},
			{X: 7, Y: 1},
			{X: 4, Y: 2},
			{X: 5, Y: 2},
			{X: 4, Y: 3},
			{X: 5, Y: 3},
		},
	}
	// draw raster image
	fmt.Println(DrawImage(rm))

	// create rectangle - vector image
	rc := NewRectangle(10, 10)
	// create line - vector image
	// rc := NewLine(10)
	// init cache
	cache := NewLinesCache()
	// adapt vector image to raster image
	rca := NewVectorToRasterAdapter(rc, cache)
	_ = NewVectorToRasterAdapter(rc, cache)
	_ = NewVectorToRasterAdapter(rc, cache)
	// draw vector image adapted to raster
	fmt.Println(DrawImage(rca))
}

// -- Interface given

// Line - represents line.
type Line struct {
	X1, Y1, X2, Y2 int
}

// VectorImage - represents image built of []Line.
type VectorImage struct {
	Image []Line
}

// NewLine - draws new line as VectorImage.
func NewLine(l int) *VectorImage {
	return &VectorImage{
		Image: []Line{
			{X1: 0, Y1: 0, X2: l, Y2: l},
		},
	}
}

// NewRectangle - draws new rectangle as VectorImage.
func NewRectangle(w, h int) *VectorImage { // vector image is required to create rectangle
	w -= 1
	h -= 1
	return &VectorImage{
		Image: []Line{
			{X1: 0, Y1: 0, X2: w, Y2: 0}, // bot
			{X1: 0, Y1: 0, X2: 0, Y2: h}, // left
			{X1: 0, Y1: h, X2: w, Y2: h}, // top
			{X1: w, Y1: 0, X2: w, Y2: h}, // right
		},
	}
}

// -- Interface we have

// RasterImageInterface - represets raster image interface.
type RasterImageInterface interface {
	GetPoints() []Point
}

// Point - represents point.
type Point struct {
	X, Y int
}

// RasterImage - represents image build of []Point.
type RasterImage struct {
	Image []Point
}

// GetPoints - gets raster image pixel points.
func (rm *RasterImage) GetPoints() []Point {
	return rm.Image
}

// DrawImage - draws RasterImage in console.
func DrawImage(rm RasterImageInterface) string { // RasterImageInterface is required to print.

	// get max height and max width of the image
	maxH := 0 // y
	maxW := 0 // x
	for _, p := range rm.GetPoints() {
		if p.X > maxW {
			maxW = p.X
		}
		if p.Y > maxH {
			maxH = p.Y
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

	return result
}

// -- Adapter

// vectorToRasterAdapter - represents vector to raster image adapter.
type vectorToRasterAdapter struct {
	image []Point
	cache LinesCacheInterface
}

// NewVectorToRasterAdapter - creates new instance of vector to raster image adapter.
func NewVectorToRasterAdapter(vi *VectorImage, lc LinesCacheInterface) RasterImageInterface { // return interface
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
	points, ok := va.cache.Retrieve(sum)
	if ok {
		fmt.Println("got points from cache")
		va.image = points
		return
	}

	fmt.Println("converting lines to points")
	// if both x and y grows, then line is diagonal
	if l.X1 != l.X2 && l.Y1 != l.Y2 {
		for i, j := l.X1, l.Y1; i < l.X2 && j < l.Y2; i, j = i+1, j+1 {
			va.image = append(va.image, Point{X: i, Y: j})
		}

		for i, j := l.X2, l.Y2; i > l.X2 && j > l.Y2; i, j = i-1, j-1 {
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

// GetPoints - gets raster image pixel points. // implement RasterImageInterface
func (va *vectorToRasterAdapter) GetPoints() []Point {
	return va.image
}

// -- Cache

// LinesCacheInterface - represents lines cache interface.
type LinesCacheInterface interface {
	GetSum(l Line) [16]byte
	Retrieve(sum [16]byte) ([]Point, bool)
	Store(sum [16]byte, pp []Point)
}

// LinesCache - represets lines cache.
type LinesCache struct {
	Cache map[[16]byte][]Point
}

// NewLinesCache - creates new instance of LinesCache.
func NewLinesCache() *LinesCache {
	return &LinesCache{Cache: make(map[[16]byte][]Point)}
}

// GetSum - generates md5 sum from provided line.
func (lc *LinesCache) GetSum(l Line) [16]byte {
	bs, _ := json.Marshal(l)
	return md5.Sum(bs)
}

// Retrieve - retrieves []Point from cache from by provided sum.
func (lc *LinesCache) Retrieve(sum [16]byte) ([]Point, bool) {
	points, ok := lc.Cache[sum]
	if !ok {
		return nil, false
	}
	return points, true
}

// Store - stores []point in cache by provided sum.
func (lc *LinesCache) Store(sum [16]byte, pp []Point) {
	lc.Cache[sum] = pp
}
