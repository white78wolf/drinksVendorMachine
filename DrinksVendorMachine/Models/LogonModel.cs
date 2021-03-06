﻿using System.ComponentModel.DataAnnotations;

namespace DrinksVendorMachine.Models
{
    /// <summary>
    /// Simple model for a logon.
    /// </summary>
    public class LogonModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        public string Password { get; set; }
    }
}