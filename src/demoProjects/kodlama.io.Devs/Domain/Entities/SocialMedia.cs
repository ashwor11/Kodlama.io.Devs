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
        public int DeveloperId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }

        public virtual Developer? Developer { get; set; }

        public SocialMedia()
        {

        }

        public SocialMedia(int id, int developerId, string url, string name) : base(id)
        {
            developerId = developerId;
            Url = url;
            Name = name;
        }
    }
}
