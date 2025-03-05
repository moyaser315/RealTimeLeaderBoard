using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Dtos;
using App.Models;

namespace App.Mapping
{
    public static class UserMapping
    {
        public static GetUserDto ToUserDto(this UserModel user){
            return new GetUserDto(
                Name: user.Name,
                Email: user.Email,
                UserName: user.UserName
            );
        }
        
    }
}