﻿namespace WebStore.Domain.Models
{
    public class Employee
    { 
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string Patronymic { get; set; }

        public int Age { get; set; }

        public override string ToString() => 
            $"id:{Id}, Surname:{SurName}, Name:{FirstName}, Patronymic:{Patronymic}, Age:{Age}";
    }
}
