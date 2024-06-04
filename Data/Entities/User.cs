using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlazingBlog.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(25), Unicode(false)]
        public string FirstName { get; set; }

        [MaxLength(25), Unicode(false)]
        public string? LastName { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(30)]
        public string Salt { get; set; }
        
        [Required, MaxLength(100)]
        public string Hash { get; set; }
    }
}