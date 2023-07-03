using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> InsertAsync(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = "INSERT INTO Users (FirstName, LastName, Email, Password) VALUES (@FirstName, @LastName, @Email, @Password); SELECT CAST(SCOPE_IDENTITY() as int);";
        return await connection.ExecuteScalarAsync<int>(query, user);
    }

    public async Task<User> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = "SELECT * FROM Users WHERE Id = @Id;";
        return await connection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
    }

    public async Task UpdateAsync(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Password = @Password WHERE Id = @Id;";
        await connection.ExecuteAsync(query, user);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = "DELETE FROM Users WHERE Id = @Id;";
        await connection.ExecuteAsync(query, new { Id = id });
    }

    public async Task<User> GetByEmailAndPasswordAsync(string email, string password)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password;";
        return await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email, Password = password });
    }
}

public class UserController : ControllerBase
{
    private readonly UserRepository _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        var id = await _userRepository.InsertAsync(user);
        return Ok(id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User user)
    {
        user.Id = id;
        await _userRepository.UpdateAsync(user);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _userRepository.DeleteAsync(id);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        var user = await _userRepository.GetByEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
        if (user == null)
            return Unauthorized();

        return Ok(user);
    }
}

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}
