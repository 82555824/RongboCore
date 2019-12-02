using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Rongbo.Models.RequestModels
{
    public class LoginRequest
    {
        [Required(ErrorMessage ="不能为空")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name不能为空")]
        public string Name { get; set; }
    }
}
