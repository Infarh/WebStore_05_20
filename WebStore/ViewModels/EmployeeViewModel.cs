using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.ViewModels
{
    public class EmployeeViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Имя является обязательным")]
        [StringLength(maximumLength: 200, MinimumLength = 3, ErrorMessage = "Длина строки от 3 до 200 символов")]
        [MinLength(3, ErrorMessage = "Минимальная длина 3 символа")]
        [RegularExpression(@"(?:[А-ЯЁ][а-яё]+)|(?:[A-Z][a-z]+)", ErrorMessage = "Ошибка формата имени - либо кириллица, латиница")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Фамилия является обязательным")]
        [MinLength(3, ErrorMessage = "Минимальная длина 3 символа")]
        public string SecondName { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        [Required(ErrorMessage = "Не указан возраст!")]
        [Range(18, 75, ErrorMessage = "Возраст в интервале от 18 до 75")]
        public int Age { get; set; }
    }
}
