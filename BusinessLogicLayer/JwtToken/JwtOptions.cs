﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.JwtToken
{
    public class JwtOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string SecretKey { get; set; }
        public bool Enabled { get; set; }

        public int ExpiryInMinutes { get; set; }
		public int RefreshTokenExpiryInMinutes { get; set; }

	}
}
