﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiSleepingPatener.Models.GenealogyTable
{
    public class UserGenealogyTableRightModel
    {
        public int GenealogyTableRightId { get; set; }

        public string NoOfPerson { get; set; }

        public string CommissionDescription { get; set; }

        public decimal CommissionAmount { get; set; }

        public bool UserPositionLeft { get; set; }

        public bool UserPositionRight { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public int SponsorId { get; set; }

        public int DownlineMemberId { get; set; }

        public string UserPositionName { get; set; }

        public DateTime CreateDate { get; set; }

        public bool DirectCommision { get; set; }

        public bool MatchingCommision { get; set; }

        public string SponsorName { get; set; }

        public string DownlineMemberName { get; set; }
    }
}