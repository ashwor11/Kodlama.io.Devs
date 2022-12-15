using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Dtos
{
    public class EnableEmailVertificationDto
    {
        public int UserId { get; set; }
        public string VerifyEmailPrefix { get; set; }
    }
}
