﻿using FSL.Framework.Core.Models;

namespace FSL.DatabaseQuery.Core.Models
{
    public sealed class MyLoggedUser : IUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
    }
}
