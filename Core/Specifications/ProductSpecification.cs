using System;
using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParams productParams) : base(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.Brands.Any() || productParams.Brands.Contains(x.Brand)) &&
            (!productParams.Types.Any() || productParams.Types.Contains(x.Type)))
    {
        ApplyPaging((productParams.PageIndex - 1) * productParams.PageSize, productParams.PageSize);
        switch (productParams.Sort)
        {
            case "pricAsc":
                AddOrderBy(p => p.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(p => p.Price);
                break;
            default:
                AddOrderBy(p => p.Name);
                break;
        }
    }
}
