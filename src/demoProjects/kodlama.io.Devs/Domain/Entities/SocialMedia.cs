using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SocialMedia: Entity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public SocialMedia(int id, int userId, string url, string name) : base(id)
        {
            UserId = userId;
            Url = url;
            Name = name;
        }
    }
}
