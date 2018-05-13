using CzadRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Services.Interfaces
{
    public interface IJwtToken
    {
        string GenerateToken(User user, DateTime expirationDate);

    }
}
