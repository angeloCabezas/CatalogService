using CatalogService.DTO;
using CatalogService.Entities;
using System.Runtime.CompilerServices;

namespace CatalogService.Extensions
{
    public static class ExtensionDTO
    {
        public static ItemDto asDto(this Item item) {
            return new ItemDto(Guid.NewGuid(),item.Name,item.Description,item.Price,item.CreatedDate);
        } 
    }
}
