using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.SocialMedias.Dtos
{
    public class GetListSocialMediaDto
    {
        public int Id { get; set; }
        public int DeveloperId { get; set; }
        public string DeveloperEmail { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
