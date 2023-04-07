﻿using FaturaTakip.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace FaturaTakip.Business.Concrete
{
    public static class UserManagerExtension
    {
        public static async Task<InvoiceTrackUser> GetLoginedUserAsync(this UserManager<InvoiceTrackUser> user, HttpContext httpContext)
        {
            var loginedUserId = user.GetUserId(httpContext.User);
            if (loginedUserId == null)
                return null;

            var loginedUser = await user.FindByIdAsync(loginedUserId);
            return loginedUser;
        }

        public static async Task<IEnumerable<InvoiceTrackUser>> GetAllUsersAsync(this UserManager<InvoiceTrackUser> user)
        {
            using(var context = new InvoiceTrackContext())
            {
                var users = await context.Users.ToListAsync();
                return users;
            }
        }
    }
}
