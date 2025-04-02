public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public int? ManagerId { get; set; }
    public string ManagerName { get; set; }  // Added Manager Name
    public int? ReviewCycle_Id { get; set; }
    public string Category { get; set; }
}
