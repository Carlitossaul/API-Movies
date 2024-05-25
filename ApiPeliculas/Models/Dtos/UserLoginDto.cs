﻿using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class UserLoginDto
    {
       
        [Required(ErrorMessage = "The UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "The Password is required")]
        public string Password { get; set; }
       
    }
}
