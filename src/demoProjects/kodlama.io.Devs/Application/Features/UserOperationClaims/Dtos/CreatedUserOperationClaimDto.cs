﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.UserOperationClaims.Dtos
{
    public class CreatedUserOperationClaimDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? OperationClaim { get; set; }
    }
}