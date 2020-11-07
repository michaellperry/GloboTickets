﻿using System.ComponentModel.DataAnnotations;

namespace GloboTickets.Promotion.DataAccess.Entities
{
    public class Content
    {
        [MaxLength(88)]
        public string Hash { get; set; }
        public byte[] Binary { get; set; }
        [MaxLength(20)]
        [Required]
        public string ContentType { get; set; }
    }
}
