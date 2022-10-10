﻿using Application.Features.OperationClaims.Dtos;
using Core.Persistence.Paging;
using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.OperationClaims.Models
{
    public class OperationClaimGetListModel: BasePageableModel
    {
        public IList<GetListOperationClaimDto> Items{ get; set; }
    }
}