using DrinkToDoor.Data.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkToDoor.Business.Dtos.Requests
{
    public class CategoryRequest
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Loại danh mục là bắt buộc")]
        public EnumCategoryType CategoryType { get; set; }
    }
}
