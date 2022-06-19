using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Play.Identity.Service.Dtos;
using Play.Identity.Service.Entities;

namespace Play.Identity.Service.Helpers
{
    public static class UserMappers
    {
        public static UserDto AsDto(this ApplicationUser user)
        {
            return new UserDto(user.Id, user.UserName, user.Email, user.Gil, user.CreatedOn);
        }
    }
}