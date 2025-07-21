
using System.Text;

namespace DrinkToDoor.Business.Utils
{
    public class Base
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// hàm tạo mã gồm 8 kí tự số (0-9) và một kí tự được truyền từ ngoài vào
        /// </summary>
        public static string GenerateCode(char suffix)
        {
            var sb = new StringBuilder(9);

            // Thêm 8 chữ số ngẫu nhiên
            for (int i = 0; i < 8; i++)
            {
                sb.Append(_random.Next(0, 10));
            }

            // Thêm ký tự suffix
            sb.Append(suffix);

            return sb.ToString();
        }
    }
}