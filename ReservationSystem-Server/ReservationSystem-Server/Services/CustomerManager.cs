using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_Server.Data;

namespace ReservationSystem_Server.Services;

[PublicAPI]
public class CustomerManager
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CustomerManager(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Gets the customer by id.
    /// </summary>
    /// <param name="id">The ID of the customer to fetch</param>
    /// <returns>The customer or null if not found</returns>
    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Finds the customer based on the signed in claims principal
    /// </summary>
    /// <param name="user">The claims principal (the User property of controllers)</param>
    /// <returns>The found customer or null if not found</returns>
    public async Task<Customer?> FindCustomerAsync(ClaimsPrincipal user)
    {
        IdentityUser idUser = await _userManager.GetUserAsync(user);

        if (idUser == null)
            return null;

        return await FindCustomerAsync(idUser);
    }

    /// <summary>
    /// Finds the customer based on the given user
    /// </summary>
    /// <param name="user">The user to find by</param>
    /// <returns>The found customer or null if not found</returns>
    public async Task<Customer?> FindCustomerAsync(IdentityUser user)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
    }
    
    /// <summary>
    /// Finds a customer based on email, phone number or both
    /// <param name="email">The email of the user (or null)</param>
    /// <param name="phoneNumber">The phone number of the user (or null)</param>
    /// </summary>
    public async Task<Customer?> FindCustomerAsync(string? email = null, string? phoneNumber = null)
    {
        email = email?.Trim().ToUpperInvariant();
        phoneNumber = phoneNumber?.Trim().ToUpperInvariant();

        return await _context.Customers.FirstOrDefaultAsync(c =>
                (string.IsNullOrWhiteSpace(email) || c.Email == email) &&
                (string.IsNullOrWhiteSpace(phoneNumber) || c.PhoneNumber == phoneNumber));
    }

    /// <summary>
    /// Gets an existing customer if exists, otherwise creates a new one
    /// </summary>
    /// <remarks>
    /// <paramref name="email"/> and <paramref name="phoneNumber"/> are used as the search criteria, so at least one must be provided 
    /// </remarks>
    /// <param name="firstName">The first name</param>
    /// <param name="lastName">The last name</param>
    /// <param name="email">The email</param>
    /// <param name="phoneNumber">The phone number</param>
    /// <returns>The existing customer or the newly created customer</returns>
    /// <exception cref="ArgumentException">If neither the email nor the phone number are provided</exception>
    public async Task<Customer> GetOrCreateCustomerAsync(string firstName, string lastName, string? email, string? phoneNumber)
    {
        if(string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Either email or phone number must be provided");
        
        email = email?.ToUpperInvariant();
        phoneNumber = phoneNumber?.ToUpperInvariant();

        Customer? customer = _context.Customers.FirstOrDefault(
                c => c.Email == email && c.PhoneNumber == phoneNumber);

        if (customer == null)
        {
            customer = new Customer
            {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PhoneNumber = phoneNumber
            };

            _context.Customers.Add(customer);
            await SaveChangesAsync();
        }
        else
        {
            // Update first and last name in the event that they were changed
            customer.FirstName = firstName;
            customer.LastName = lastName;
            await SaveChangesAsync();
        }

        return customer;
    }

    /// <summary>
    /// A helper alias to save changes to the database, used when injecting this class if it is not desirable to also inject the context
    /// </summary>
    /// <returns>The result of the <see cref="DbContext.SaveChangesAsync(System.Threading.CancellationToken)"/> operation</returns>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}