using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class ModelCompatibilityDto
    {
        public int MinYear { get; set; }

        public int MaxYear { get; set; }

        public string ModelId { get; set; }

        public string SKU { get; set; }
    }
}