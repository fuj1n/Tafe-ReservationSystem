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

    /**
     * Finds the customer based on the signed in claims principal
     */
    public async Task<Customer?> FindCustomerAsync(ClaimsPrincipal user)
    {
        IdentityUser idUser = await _userManager.GetUserAsync(user);

        if (idUser == null)
            return null;

        return await FindCustomerAsync(idUser);
    }

    /**
     * Finds the customer based on the given user
     */
    public async Task<Customer?> FindCustomerAsync(IdentityUser user)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
    }
    
    /**
     * Finds a customer based on email, phone number or both
     * <param name="email">The email of the user (or null)</param>
     * <param name="phoneNumber">The phone number of the user (or null)</param>
     */
    public async Task<Customer?> FindCustomerAsync(string? email = null, string? phoneNumber = null)
    {
        email = email?.Trim().ToUpperInvariant();
        phoneNumber = phoneNumber?.Trim().ToUpperInvariant();

        return await _context.Customers.FirstOrDefaultAsync(c =>
                (string.IsNullOrWhiteSpace(email) || c.Email == email) &&
                (string.IsNullOrWhiteSpace(phoneNumber) || c.PhoneNumber == phoneNumber));
    }

    /**
     * Gets an existing customer if exists, otherwise creates a new one
     */
    public async Task<Customer> GetOrCreateCustomerAsync(string firstName, string lastName, string? email, string? phoneNumber)
    {
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

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}