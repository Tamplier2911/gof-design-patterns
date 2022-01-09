package solid

import "fmt"

// O - Open-Closed (open for extension, closed for modification)

// 1. should be open for extension, but closed for modification
// 2. such an entity can allow its behaviour to be extended without modifying its source code

func OpenClosed() {
	fmt.Println("Open-Closed")

	// init range of products
	pp := []Product{
		{
			Name:  "Little Black Dress",
			Color: ProductColorBlack,
			Size:  ProductSizeSmall,
		},
		{
			Name:  "Sword of Isildur",
			Color: ProductColorMetallic,
			Size:  ProductSizeLarge,
		},
		{
			Name:  "BMW M3 GTR",
			Color: ProductColorBlack,
			Size:  ProductSizeLarge,
		},
	}
	fmt.Printf("%+v\n\n", pp)

	// Not Good

	// init product filter
	f := NewProductFilter()

	largeProducts := f.FilterBySize(pp, ProductSizeLarge)
	blackProducts := f.FilterByColor(pp, ProductColorBlack)

	fmt.Printf("[B] Large Products: %+v\n", largeProducts)
	fmt.Printf("[B] Black Products: %+v\n\n", blackProducts)

	// Good

	fe := NewProductFilterEnhanced()

	largeProds := fe.FilterProducts(pp, NewProductSizeSpecification(ProductSizeLarge))
	blackProds := fe.FilterProducts(pp, NewProductColorSpecification(ProductColorBlack))
	blackAndLargeProds := fe.FilterProducts(pp,
		NewProductAndSpecification(
			NewProductSizeSpecification(ProductSizeLarge),
			NewProductColorSpecification(ProductColorBlack),
		),
	)

	fmt.Printf("[G] Large Products: %+v\n", largeProds)
	fmt.Printf("[G] Black Products: %+v\n", blackProds)
	fmt.Printf("[G] Large Black Products: %+v\n\n", blackAndLargeProds)

	fee := ProductFilterEnhanced{}
	largeProdse := fee.FilterProducts(pp, &ProductSizeSpecification{ProductSizeLarge})
	blackProdse := fee.FilterProducts(pp, &ProductColorSpecification{ProductColorBlack})
	blackAndLargeProdse := fee.FilterProducts(pp,
		&ProductAndSpecification{
			[]ProductSpecification{
				&ProductSizeSpecification{ProductSizeLarge},
				&ProductColorSpecification{ProductColorBlack},
			},
		},
	)

	fmt.Printf("[GE] Large Products: %+v\n", largeProdse)
	fmt.Printf("[GE] Black Products: %+v\n", blackProdse)
	fmt.Printf("[GE] Large Black Products: %+v\n", blackAndLargeProdse)
}

// Product - represents product.
type Product struct {
	Name  string
	Color ProductColor
	Size  ProductSize
}

// ProductColor - represents Product colors.
type ProductColor string

const (
	ProductColorBlack    ProductColor = "black"
	ProductColorBlue     ProductColor = "blue"
	ProductColorMetallic ProductColor = "metallic"
)

// ProductSize - represents Product sizes.
type ProductSize string

const (
	ProductSizeSmall  ProductSize = "S"
	ProductSizeMedium ProductSize = "M"
	ProductSizeLarge  ProductSize = "L"
)

// ProductFilter - represents ProductFilter.
type ProductFilter struct{}

// NewProductFilter - creates new instance of ProductFilter.
func NewProductFilter() *ProductFilter {
	return &ProductFilter{}
}

// FilterBySize - filters Product's by size.
func (pf *ProductFilter) FilterBySize(pp []Product, s ProductSize) []Product {
	result := make([]Product, 0)
	for _, p := range pp {
		if p.Size == s {
			result = append(result, p)
		}
	}
	return result
}

// FilterByColor - filters Product's by color.
func (pf *ProductFilter) FilterByColor(pp []Product, c ProductColor) []Product {
	result := make([]Product, 0)
	for _, p := range pp {
		if p.Color == c {
			result = append(result, p)
		}
	}
	return result
}

// FilterByColorAndSize
// FilterBy...
// FilterBy...

// Specification Pattern

// ProductSpecification - represents Product specification interface.
type ProductSpecification interface {
	ProductSatisfied(p Product) bool
}

// ProductsFilter - represents Product's filter interface.
type ProductsFilter interface {
	FilterProducts(pp []Product, s ProductSpecification) []Product
}

// Color Specification

// ProductColorSpecification - implements ProductSpecification interface.
type ProductColorSpecification struct {
	Color ProductColor
}

// NewProductColorSpecification - creates new instance of ProductColorSpecification.
func NewProductColorSpecification(color ProductColor) *ProductColorSpecification {
	return &ProductColorSpecification{color}
}

// ProductSatisfied - ensures that Product specification is satisfied.
func (pcs *ProductColorSpecification) ProductSatisfied(p Product) bool {
	return p.Color == pcs.Color
}

// Size Specification

// ProductSizeSpecification - implements ProductSpecification interface.
type ProductSizeSpecification struct {
	Size ProductSize
}

// NewProductSizeSpecification - creates new instance of ProductSizeSpecification.
func NewProductSizeSpecification(size ProductSize) *ProductSizeSpecification {
	return &ProductSizeSpecification{size}
}

// ProductSatisfied - ensures that Product specification is satisfied.
func (pss *ProductSizeSpecification) ProductSatisfied(p Product) bool {
	return p.Size == pss.Size
}

// And Specification

// ProductAndSpecification - implements ProductSpecification interface.
// combinator of ProductSpecifications.
type ProductAndSpecification struct {
	Specifications []ProductSpecification
}

// NewProductAndSpecification - creates new instance of ProductAndSpecification.
func NewProductAndSpecification(specs ...ProductSpecification) *ProductAndSpecification {
	return &ProductAndSpecification{specs}
}

// ProductSatisfied - ensures that Product specification is satisfied.
func (pas *ProductAndSpecification) ProductSatisfied(p Product) bool {
	for i := range pas.Specifications {
		if !pas.Specifications[i].ProductSatisfied(p) {
			return false
		}
	}
	return true
}

// Product Filter

// ProductFilterEnhanced - implements ProductsFilter interface.
type ProductFilterEnhanced struct{}

// NewProductFilterEnhanced - creates new instance of ProductFilterEnhanced.
func NewProductFilterEnhanced() *ProductFilterEnhanced {
	return &ProductFilterEnhanced{}
}

// FilterProducts - filters range of products based on ProductSpecification.
func (pfe *ProductFilterEnhanced) FilterProducts(pp []Product, s ProductSpecification) []Product {
	result := make([]Product, 0)
	for _, p := range pp {
		if s.ProductSatisfied(p) {
			result = append(result, p)
		}
	}
	return result
}
