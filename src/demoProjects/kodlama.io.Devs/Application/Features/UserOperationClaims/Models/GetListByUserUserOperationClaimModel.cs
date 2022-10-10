using Application.Features.UserOperationClaims.Dtos;
using Core.Persistence.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserOperationClaims.Models
{
    public class GetListByUserUserOperationClaimModel
    {
        public int UserId { get; set; }
        public string? UserEmail { get; set; }
        public int Count { get; set; }
        public IList<GetListByUserUserOperationClaimDto>? Claims{ get; set; }
        
    }
}
