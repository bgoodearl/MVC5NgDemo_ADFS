using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSC = System.Security.Claims;
using MVCDemo.Models.Test;

namespace MVCDemo.Controllers.API
{
    public class TestAuthInfoController : ApiController
    {
        public AuthInfoResponse Get()
        {
            AuthInfoResponse response = new AuthInfoResponse
            {
                ServerTime = DateTime.Now
            };
            SSC.ClaimsPrincipal cp = this.User as SSC.ClaimsPrincipal;
            if (cp != null)
            {
                response.IdentityName = cp.Identity.Name;
                var rc = new List<ClaimInfo>();
                foreach(var claim in cp.Claims)
                {
                    rc.Add(new ClaimInfo{
                        Type = claim.Type,
                        Value = claim.Value
                    });
                }
                response.Claims = rc;
            };
            return response;
        }
    }
}
