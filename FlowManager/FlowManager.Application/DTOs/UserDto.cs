﻿namespace FlowManager.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";

        public List<UserRoleDto> UserRoles { get; set; } = new();
    }
}
