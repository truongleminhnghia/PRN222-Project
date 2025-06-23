
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.BLL.ViewModel.Requests
{
    public class CategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public EnumCategoryType CategoryType { get; set; }
    }
}