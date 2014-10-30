using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCDemo.Models.Test
{
    public class AuthInfoResponse
    {
        public string IdentityName { get; set; }
        public DateTime ServerTime { get; set; }
        public IEnumerable<ClaimInfo> Claims { get; set; }
    }
}