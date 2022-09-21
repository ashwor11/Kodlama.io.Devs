using Core.Security.Entities;
using Core.Security.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Developer: User
    {
        public string? GithubUrl  { get; set; }

        public Developer()
        {

        }

        public Developer(string githubUrl): this()
        {
            GithubUrl = githubUrl;
        }
    }
}
