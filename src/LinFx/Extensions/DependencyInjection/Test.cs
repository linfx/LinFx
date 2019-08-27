using LinFx.Security.Users;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinFx.Extensions.DependencyInjection
{
    [Service]
    public class Test
    {
        public ICurrentUser C0 { get; set; }

        [Autowired]
        public ICurrentUser C1 { get; set; }

        [Autowired]
        private ICurrentUser C2 { get; set; }

        [Autowired]
        private ICurrentUser C3;

        public void t()
        {
            var tt = C1;
        }
    }
}
