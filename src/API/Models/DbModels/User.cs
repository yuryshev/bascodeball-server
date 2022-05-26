﻿using System;

namespace API.Models.DbModels;
public class User
{
    /// <summary>
    /// Gets or sets UserId.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Gets or sets LoginName.
    /// </summary>
    public string LoginName { get; set; }
    
    /// <summary>
    /// Gets or sets Email.
    /// </summary>
    public string Email { get; set; }

    public string Role { get; set; }
    
}