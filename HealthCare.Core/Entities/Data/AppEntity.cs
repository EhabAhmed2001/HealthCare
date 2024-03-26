using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Entities.Data
{
    public class AppEntity
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        public AppEntity()
        {
            if(Id == string.Empty)
                Id = Guid.NewGuid().ToString();
        }
    }
}
