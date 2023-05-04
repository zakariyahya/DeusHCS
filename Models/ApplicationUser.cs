using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace WebAdmin.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool FirstLogin { get; set; }
        public bool? isFirstLogin { get; set; }
        // public bool IsFirstLogin { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public override string PasswordHash { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }

        [JsonIgnore, IgnoreDataMember, NotMapped]
        public string Name
        {
            get
            {
                return UserName;
            }
            set
            {
                UserName = value;
            }
        }

        public ICollection<ApplicationRole> Roles { get; set; }
    }
}