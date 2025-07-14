using MongoDB.Driver;
using ShortenerService.Infrastracture.Context;

namespace ShortenerService.Infrastracture.Repositories;

public class UrlDetailsRepository(ShortenerContext context)
{

    private readonly ShortenerContext _context = context;

    public async Task Create(UrlDetails urlDetail)
    {
        await _context.UrlDetails.InsertOneAsync(urlDetail);
    }

    public async Task<UrlDetails> GetByShortCode(string shortCode)
    {
        return await _context.UrlDetails.Find(p => p.ShortCode == shortCode).FirstOrDefaultAsync();
    }

}
