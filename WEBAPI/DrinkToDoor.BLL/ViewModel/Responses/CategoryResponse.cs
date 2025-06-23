
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.BLL.ViewModel.Responses
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public EnumCategoryType CategoryType { get; set; }
    }
}